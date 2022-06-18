using CMS;
using CMS.Business;

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

app.UseAuthorization();

app.MapControllers();

app.Run();
