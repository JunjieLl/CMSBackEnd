
using System.Net;
using System.Net.Mail;
using ServiceStack.Redis;
using EmailLibrary;

namespace CMS.Business;

public class EmailBusiness : IEmailBusiness
{
    private readonly IConfiguration configuration;

    public EmailBusiness(IConfiguration configuration, RedisClient redisClient)
    {
        this.configuration = configuration;

    }



    public void sendEMail(string toEmail, string subject, string content)
    {
        MyMail myMail = new MyMail(new MailAddress(configuration["MyMail:account"]), configuration["MyMail:server"], int.Parse(configuration["MyMail:port"]),
            configuration["MyMail:account"], configuration["MyMail:password"]);
        myMail.sendEMail(toEmail, subject, content);

        //MailAddress to = new MailAddress(toEmail);
        //MailAddress from = new MailAddress(configuration["MyMail:account"]);

        //MailMessage message = new MailMessage(from, to);
        //message.Subject = subject;
        //message.Body = content;

        //SmtpClient client = new SmtpClient(configuration["MyMail:server"], int.Parse(configuration["MyMail:port"]))
        //{
        //    Credentials = new NetworkCredential(configuration["MyMail:account"], configuration["MyMail:password"]),
        //    EnableSsl = true
        //};


        // code in brackets above needed if authentication required

        //client.SendAsync(message, "mymail");
        //client.Send(message);
    }

}