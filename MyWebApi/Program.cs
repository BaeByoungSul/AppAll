using Models.Auth;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Models.Database;
using Models.Email;
using Services.AuthService;
using Services.DbService;
using Services.EmailService;
using Services.FileService;
using Services.SapService;
using Services.TokenService;
using Services.SapWcfService;
using MyWebApi.Models.Auth;

var builder = WebApplication.CreateBuilder(args);

//  Kestrel �� ������ �ɼ� ����
// �⺻������ Kestrel ������ Kestrel ���ǿ��� �ε�ǰ� ���� ������ �ٽ� �ε��� �� �ֽ��ϴ�.
builder.WebHost.ConfigureKestrel(opts =>
{
    opts.AllowSynchronousIO = true;
    opts.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB

});

// Add services to the container.
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
    })
   ;
    
//.AddXmlSerializerFormatters();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtConfig = builder.Configuration
        .GetSection("Jwt")
        .Get<JwtConfig>();
builder.Services.AddSingleton(jwtConfig);

//var angularConfig = builder.Configuration
//        .GetSection("AngularApp")
//        .Get<AngularConfig>();
//builder.Services.AddSingleton(angularConfig);


//ClockSkew: ��ū ���� �ð��� �������� �ʾƼ� �߰�..(from stackOverflow )
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //options.RequireHttpsMetadata = false;
        //options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            RequireExpirationTime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey))
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                }
                return Task.CompletedTask;
            }
        };
    });
// configure DI for application services
var dbConnConfig = builder.Configuration
        .GetSection("DbConnectInfo")
        .Get<List<ConnectDBInfo>>();
builder.Services.AddSingleton(dbConnConfig);

var emailConfig = builder.Configuration
        .GetSection("EmailConfig")
        .Get<EmailConfig>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ISapService, SapService>();
builder.Services.AddScoped<SapWcfService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/


// 2023.01.31. angular���� header�� �������� ���ؼ�..Content-Disposition �߰�
// 2024.02.02. AllowAnyOrigin >> WithOrigins
var settingOrigins = builder.Configuration.GetValue<string>("AllowedOrigins");
Console.WriteLine(settingOrigins);
if (!settingOrigins.IsNullOrEmpty())
{
    var origins  = settingOrigins.Split(';');
    app.UseCors(policy => policy.WithOrigins(origins)
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .WithExposedHeaders("Content-Disposition"));

}
//app.UseCors(policy => policy.AllowAnyOrigin()
//                            .AllowAnyMethod()
//                            .AllowAnyHeader()
//                            .WithExposedHeaders("Content-Disposition"));

// https�� �������� ������ �ʿ����
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
