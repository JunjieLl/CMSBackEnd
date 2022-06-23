using CMS.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ServiceStack.Redis;
namespace CMS.Business;

public class PersonalInfoBusiness : IPersonalInfoBusiness
{
    private readonly cmsContext context;
    private readonly IEmailBusiness emailBusiness;
    private readonly RedisClient redisClient;
    public PersonalInfoBusiness(cmsContext context, IEmailBusiness emailBusiness, RedisClient redisClient)
    {
        this.context = context;
        this.emailBusiness = emailBusiness;
        this.redisClient = redisClient;
    }

    public PersonalInfoOutDto getPersonalInformation(string userId)
    {
        PersonalInfoOutDto personalInfoOutDto = (from user in context.Users
                                                 join commonUser in context.CommonUsers
                                                 on user.UserId equals commonUser.UserId
                                                 select new PersonalInfoOutDto
                                                 {
                                                     UserId = user.UserId,
                                                     UserName = user.UserName,
                                                     EmailAddress = user.EmailAddress,
                                                     Identity = user.Identity,
                                                     ActivityStatus = user.ActivityStatus,
                                                     TimeLimit = commonUser.TimeLimit,
                                                     CountLimit = commonUser.CountLimit,
                                                     isSuperManager = 0
                                                 }).Single<PersonalInfoOutDto>(p => p.UserId.Equals(userId));
        return personalInfoOutDto;
    }

    public async Task<int> modifyPersonalInfo(string userId, PersonalInfoOutDto personalInfo)
    {
        if (!userId.Equals(personalInfo.UserId))
        {
            return -1;
        }
        User user = context.Users.Single(u => u.UserId.Equals(userId));
        if (personalInfo.ActivityStatus != null)
        {
            user.ActivityStatus = personalInfo.ActivityStatus;
        }
        user.Identity = personalInfo.Identity;
        user.UserName = personalInfo.UserName;
        user.EmailAddress = personalInfo.EmailAddress;
        if ("管理员".Equals(user.Identity))
        {
            RoomManager roomManager = context.RoomManagers.Single(u => u.UserId.Equals(userId));
            roomManager.IsSuperManager = personalInfo.isSuperManager;
        }
        else
        {
            CommonUser commonUser = context.CommonUsers.Single(u => u.UserId.Equals(userId));
            commonUser.CountLimit = personalInfo.CountLimit;
            commonUser.TimeLimit = personalInfo.TimeLimit;
        }

        await context.SaveChangesAsync();
        return 1;
    }

    public async Task<int> modifyAvatar(ModifyAvatarInDto modifyAvatarInDto)
    {
        User user = context.Users.Single(u => u.UserId.Equals(modifyAvatarInDto.userId));
        user.Avatar = modifyAvatarInDto.Avatar;
        await context.SaveChangesAsync();
        return 1;
    }

    public string? getAvatarUrl(string userId)
    {
        var avatar = context.Users.Single(u => u.UserId.Equals(userId)).Avatar;
        return avatar;
    }

    public List<PersonalInfoOutDto> getAllUsers()
    {
        var personalInfoOutDtos = (from user in context.Users
                                   join commonUser in context.CommonUsers
                                   on user.UserId equals commonUser.UserId
                                   select new PersonalInfoOutDto
                                   {
                                       UserId = user.UserId,
                                       UserName = user.UserName,
                                       EmailAddress = user.EmailAddress,
                                       Identity = user.Identity,
                                       ActivityStatus = user.ActivityStatus,
                                       TimeLimit = commonUser.TimeLimit,
                                       CountLimit = commonUser.CountLimit,
                                       isSuperManager = 0
                                   }).Union(
            from user in context.Users
            join superUser in context.RoomManagers
            on user.UserId equals superUser.UserId
            select new PersonalInfoOutDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                Identity = user.Identity,
                ActivityStatus = user.ActivityStatus,
                TimeLimit = 0,
                CountLimit = 0,
                isSuperManager = superUser.IsSuperManager
            }
        );
        return personalInfoOutDtos.ToList();
    }

    public async Task<int> addUser(PersonalInfoOutDto personalInfo)
    {
        var existUser = await context.Users.FindAsync(personalInfo.UserId);
        if (existUser != null)
        {
            return -1;
        }
        User user = new User();
        user.UserId = personalInfo.UserId;
        user.UserName = personalInfo.UserName;
        user.EmailAddress = personalInfo.EmailAddress;
        user.Identity = personalInfo.Identity;
        await context.Users.AddAsync(user);
        if (personalInfo.Identity.Equals("管理员"))
        {
            RoomManager roomManager = new RoomManager();
            roomManager.IsSuperManager = 0;
            roomManager.UserId = personalInfo.UserId;
            await context.RoomManagers.AddAsync(roomManager);
        }
        else
        {
            CommonUser commonUser = new CommonUser();
            commonUser.UserId = personalInfo.UserId;
            commonUser.CountLimit = personalInfo.CountLimit;
            commonUser.TimeLimit = personalInfo.TimeLimit;
            await context.CommonUsers.AddAsync(commonUser);
        }

        await context.SaveChangesAsync();
        return 1;
    }

    public void grant(GrantInDto grantInDto)
    {
        var superList = context.Manages.Where(m => m.UserId.Equals(grantInDto.superManagerId)).ToList();
        var managerList = context.Manages.Where(m => m.UserId.Equals(grantInDto.managerId)).ToList();

        HashSet<string> rooms = new HashSet<string>();
        managerList.ForEach(r => rooms.Add(r.RoomId));
        superList.ForEach(r =>
        {
            if (!rooms.Contains(r.RoomId))
            {
                Manage manage = new Manage();
                manage.UserId = grantInDto.managerId;
                manage.RoomId = r.RoomId;
                context.Manages.Add(manage);
            }
        });

        context.SaveChanges();
    }

    public string? sendEmail(string userId)
    {
        var userInfo = context.Users.Where(u => userId.Equals(u.UserId))
        .Select(u => new { u.EmailAddress, u.UserName })
        .SingleOrDefault();
        if (userInfo == null)
        {
            return null;
        }


        string code = "";
        Random random = new Random();
        for (int i = 0; i < 6; ++i)
        {
            code += (random.Next() % 10).ToString();
        }
        string content = $"Hello, {userInfo.UserName}! Your code is {code}. The verification code is valid within 5 minutes.";

        try
        {
            emailBusiness.sendEMail(userInfo.EmailAddress, "CMS Verification", content);
            redisClient.Add(userId, code, TimeSpan.FromMinutes(5));
            return userInfo.EmailAddress;
        }
        catch
        {
            return null;
        }
    }


    public int modifyPassword(FixPasswordDto fixPasswordDto)
    {
        if (redisClient.ContainsKey(fixPasswordDto.userId) && redisClient.Get<string>(fixPasswordDto.userId).Equals(fixPasswordDto.verificationCode))
        {
            User? user = context.Users.SingleOrDefault(u => fixPasswordDto.userId.Equals(u.UserId));
            if (user == null)
            {
                return -1;
            }
            user.Password = fixPasswordDto.password;
            context.SaveChanges();
            return 1;
        }
        return -1;
    }
}