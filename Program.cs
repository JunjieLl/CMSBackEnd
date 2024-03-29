using CMS;
using CMS.Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ServiceStack.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<cmsContext>();
//mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("mycors", buildPolicy =>
    buildPolicy.WithOrigins("*", "*", "*")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin());
});
//business
builder.Services.AddScoped<ILoginBusiness, LoginBusiness>();
builder.Services.AddScoped<IPersonalInfoBusiness, PersonalInfoBusiness>();
builder.Services.AddScoped<IRoomBusiness, RoomBusiness>();
builder.Services.AddScoped<IFavoriteBusiness, FavoriteBusiness>();
builder.Services.AddScoped<IActivityBusiness, ActivityBusiness>();
builder.Services.AddScoped<IModifyBusiness, ModifyBusiness>();
builder.Services.AddScoped<IEmailBusiness, EmailBusiness>();
//jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateLifetime = true
    };
    //get token from header
    options.Events = new JwtBearerEvents()
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Headers["helloc"];
            return Task.CompletedTask;
        }
    };
});
//redis
builder.Services.AddSingleton<RedisClient>(
    i => new RedisClient(builder.Configuration["MyRedis:url"], builder.Configuration.GetValue<int>("MyRedis:port"), builder.Configuration["MyRedis:password"]));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("mycors");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
