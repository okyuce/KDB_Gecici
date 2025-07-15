using Asp.Versioning;
using Csb.YerindeDonusum.Application.Hubs;
using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.Application.Models.Afad;
using Csb.YerindeDonusum.Application.ServiceRegistration;
using Csb.YerindeDonusum.BackgroundJob.Hangfire.Schedules;
using Csb.YerindeDonusum.BackgroundJob.ServiceRegistration;
using Csb.YerindeDonusum.Caching.ServiceRegistration;
using Csb.YerindeDonusum.Persistance.ServiceRegistration;
using Csb.YerindeDonusum.Takbis.ServiceRegistration;
using Csb.YerindeDonusum.EDevlet.ServiceRegistration;
using Csb.YerindeDonusum.NVIYapiBelge.ServiceRegistration;
using Csb.YerindeDonusum.Santiyem.ServiceRegistration;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles.Extensions;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles.Services;
using Csb.YerindeDonusum.WebApi.BasicAuthFiles.SwaggerBasicAuth;
using Csb.YerindeDonusum.WebApi.Middleware;
using CSB.Core;
using CSB.Core.Integration;
using CSB.Core.Integration.KPS;
using CSB.Core.Integration.NVI;
using CSB.Core.Utilities.Messaging;
using Hangfire;
using Hangfire.PostgreSql;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using CSB.Core.MessageProducer.Extensions;
using CSB.Core.LogHandler.MiddleWare;
using CSB.Core.LogHandler.Model;
using System.Net.Mail;
using System.Net;
using Csb.YerindeDonusum.Application.Models.Mail;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Csb.YerindeDonusum.Application;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

var swaggerIsActive = builder.Configuration.GetValue<bool>("SwaggerBasicAuth:IsActive");
var hangfireIsActive = builder.Configuration.GetValue<bool>("HangfireOptions:IsActive");
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});
#region ...: Redis :...
builder.Services.AddCachingServiceRegistration(builder.Configuration);
#endregion

builder.Services.AddSignalR().AddStackExchangeRedis("172.17.80.238:6379");

builder.Services.AddSingleton<IPostConfigureOptions<BasicAuthenticationOptions>, BasicAuthenticationPostConfigureOptions>();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var cultureInfo = new CultureInfo("tr-TR");
    cultureInfo.DateTimeFormat = new DateTimeFormatInfo
    {
        DateSeparator = "/",
        TimeSeparator = ":",
        ShortDatePattern = "dd/MM/yyyy",
    };
    cultureInfo.NumberFormat.CurrencySymbol = "₺";
    cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
    cultureInfo.NumberFormat.CurrencyDecimalSeparator = ",";
    cultureInfo.NumberFormat.NumberGroupSeparator = ".";
    cultureInfo.NumberFormat.NumberDecimalDigits = 2;
    cultureInfo.NumberFormat.CurrencyDecimalDigits = 2;
    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

    var supportedCultures = new List<CultureInfo> { cultureInfo };

    options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});
// Add services to the container.

//Mobil Uygulama İçerisinde Kullanılan Bir Html Plugin'i İçin Eklendi.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                      policy =>
                      {
                          policy.AllowAnyOrigin();
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CacheOptionModel>(builder.Configuration.GetSection("CacheOptions"));

var messageUserName = builder.Configuration.GetValue<string>("LogMessageOptions:UserName");
var messagePassword = builder.Configuration.GetValue<string>("LogMessageOptions:Password");
builder.Services.AddCSBMessageProducer(cnf =>
{
    cnf.Username = messageUserName;
    cnf.Password = messagePassword;
});
var jwtOptionConfiguration = builder.Configuration.GetSection("JwtOptions");
var jwtOptions = jwtOptionConfiguration.Get<JwtOptionModel>();

builder.Services.Configure<JwtOptionModel>(jwtOptionConfiguration);

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
    .AddJsonOptions(options => options.JsonSerializerOptions.MaxDepth = 20)
    ;

#region ...: calling custom service providers :...
// persistance katmani service provider sinifini ayarliyoruz
// repository dependency injection islemleri bu method yardimi ile tamamlaniyor
NpgsqlConnection.GlobalTypeMapper.UseNetTopologySuite();
builder.Services.AddPersistanceServiceRegistration(builder.Configuration);

// application katmani service provider sinifini ayarliyoruz
builder.Services.AddApplicationServiceRegistration();

// takbis service katmani service provider sinifini ayarliyoruz
builder.Services.AddTakbisServiceRegistration(builder.Configuration);
// edevlet service katmani service provider sinifini ayarliyoruz
builder.Services.AddEDevletServiceRegistration(builder.Configuration);
// nvi yapi belge service katmani service provider sinifini ayarliyoruz
builder.Services.AddNVIYapiBelgeServiceRegistration(builder.Configuration);

// kps, nvi, messagin vb. gibi service katmanlari için ortak service provider siniflarini ayarliyoruz
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddCoreIntegrationServices(builder.Configuration);

// kps service katmani service provider sinifini ayarliyoruz
builder.Services.AddKPSIntegrationService(builder.Configuration);

// nvi service katmani service provider sinifini ayarliyoruz
builder.Services.AddNVIIntegrationService(builder.Configuration);

// şantiyem yambis service katmani service provider sinifini ayarliyoruz
builder.Services.AddSantiyemServiceRegistration(builder.Configuration);

// messaging service katmani (mail ve sms için kullanıyoruz) service provider sinifini ayarliyoruz
builder.Services.AddMessagingService(builder.Configuration);

// background job (hangfire) service katmani service provider sinifini ayarliyoruz
builder.Services.AddBackgroundJobServiceRegistration();
#endregion

#region ...: Api Versiyonlama :...
builder.Services
    .AddApiVersioning(opt =>
    {
        opt.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
        opt.AssumeDefaultVersionWhenUnspecified = true;
        opt.ReportApiVersions = true;
        opt.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),
                                                        new HeaderApiVersionReader("x-api-version"),
                                                        new MediaTypeApiVersionReader("x-api-version"));
    })
    .AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        }
    );
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Default validation disable edildi, fluent validation kullanılıyor.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
//HttpClientFactory
var afadOptionConfiguration = builder.Configuration.GetSection("AFADOptions");
var afadOptions = afadOptionConfiguration.Get<AfadOptionModel>();
builder.Services.AddHttpClient("AfadAuthApiClient", config =>
{
    config.BaseAddress = new Uri(afadOptions.AuthOptions.Url);
    config.Timeout = new TimeSpan(0, 0, 30);
    config.DefaultRequestHeaders.Clear();
    config.DefaultRequestHeaders.Add("Authorization", $"Basic {Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Concat(afadOptions.AuthOptions.BasicUserName, ":", afadOptions.AuthOptions.BasicPassword)))}");
});
builder.Services.AddHttpClient("AfadExportedApiClient", config =>
{
    config.BaseAddress = new Uri(afadOptions.BaseUrl);
    config.Timeout = new TimeSpan(0, 0, 60);
    config.DefaultRequestHeaders.Clear();
});
#region Mail Settings
var mailOption = builder.Configuration.GetSection("MailOptions").Get<MailOptionModel>()!;

builder.Services
            .AddFluentEmail(mailOption.FromEmail)
            .AddSmtpSender(new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = mailOption.UseDefaultCredentials,
                Host = mailOption.Host,
                Port = mailOption.Port,
                EnableSsl = mailOption.EnableSsl,
                Credentials = !string.IsNullOrWhiteSpace(mailOption.Username) && !string.IsNullOrWhiteSpace(mailOption.Password)
                                ? new NetworkCredential(mailOption.Username, mailOption.Password, mailOption.Domain)
                                : null,
                Timeout = mailOption.TimeoutSeconds * 1000
            });
#endregion
if (swaggerIsActive)
{
    builder.Services.AddSwaggerGen(option =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"

        });

        option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });

    });
}

#region ...: jwtBearer provider :...
// ...
builder.Services.AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
    .AddBasic<AuthenticationService>(o =>
    {
        o.Realm = "Yerinde Donusum Basvuru Web API";
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = jwtOptions.ValidateIssuer,
            ValidateAudience = jwtOptions.ValidateIssuer,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
            ValidIssuer = jwtOptions.ValidIssuer,
            ValidAudience = jwtOptions.ValidAudience,
            IssuerSigningKey = new SymmetricSecurityKey(jwtOptions.IssuerSigningKey),
        };
    });
#endregion

#region ...: hangfire providers :...
if (hangfireIsActive)
{
    builder.Services.AddHangfire(x => x.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("YerindeDonusum")));
    builder.Services.AddHangfireServer();
}
#endregion

var app = builder.Build();

app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseMiddleware<LoggingMiddleware>(new LogOptions()
{
    UseDatabase = false,
    UseMessageQueue = true
});
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
if (swaggerIsActive)
{
    app.UseSwaggerAuthorized();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Production Ortamında F5 Üzerinden Redirection Yapıldığı İçin Kapatıldı
//if (app.Environment.IsProduction())
//{
//    app.UseHttpsRedirection();
//}

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (localizeOptions != null) app.UseRequestLocalization(localizeOptions.Value);
app.UseHsts();
app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();


app.MapControllers();

app.MapHub<KdsHub>("/kdsHub");

#region ...: Hangfire Dashboard And Job Init Options :...
if (hangfireIsActive)
{
    app.UseHangfireDashboard("/ky/hangfire", new DashboardOptions
    {
        DashboardTitle = "Hangfire Zamanlanmış Görev Yönetim Paneli",
        //AppPath = "/KY/Hangfire",
        Authorization = new[]
        {
            new HangfireCustomBasicAuthenticationFilter {
                User = builder.Configuration.GetValue<string>("HangfireOptions:UserName"),
                Pass = builder.Configuration.GetValue<string>("HangfireOptions:Password")
            }
        }
    });

    // hangfire job'larini init ediyoruz
    RecurringJobInit.Init(builder.Environment.IsProduction());
}
#endregion

app.Run();