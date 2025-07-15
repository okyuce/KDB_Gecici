using Csb.YerindeDonusum.Application.Models;
using Csb.YerindeDonusum.WebApp.Middleware;
using Csb.YerindeDonusum.WebApp.Models;
using Csb.YerindeDonusum.WebApp.Services;
using CSB.Core.LogHandler.MiddleWare;
using CSB.Core.LogHandler.Model;
using CSB.Core.MessageProducer.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;


var builder = WebApplication.CreateBuilder(args);


var messageUserName = builder.Configuration.GetValue<string>("LogMessageOptions:UserName");
var messagePassword = builder.Configuration.GetValue<string>("LogMessageOptions:Password");
builder.Services.AddCSBMessageProducer(cnf =>
{
    cnf.Username = messageUserName;
    cnf.Password = messagePassword;
});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHttpService, HttpService>();
builder.Services.AddTransient<INatamamYapiAppService, NatamamYapiAppService>();

builder.Services.Configure<OptionsModel>(builder.Configuration.GetSection("Options"));

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

builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = builder.Configuration.GetValue<int>("Options:ValueLengthLimit") * 1024 * 1024;
});

//HttpClientFactory
builder.Services.AddHttpClient("YerindeDonusumApi", config =>
{
    config.BaseAddress = new Uri(builder.Configuration.GetValue<string>("Options:ApiUrl"));
    config.Timeout = new TimeSpan(0, 0, builder.Configuration.GetValue<int>("Options:HttpClientTimeoutSecond"));
    config.DefaultRequestHeaders.Clear();
    config.DefaultRequestHeaders.Add("x-request-by", "yerinde.donusum.app");
});

//JWT Authentication
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptionModel>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
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
}).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(jwtOptions.CookieExpireTimeMinute ?? jwtOptions.ExpireTimeMinute);
    options.SlidingExpiration = false;
    options.AccessDeniedPath = "/admin/erisimYetkisiYok/";
    options.LoginPath = "/admin/giris/";
    options.Cookie = new CookieBuilder
    {
        HttpOnly = true, //önemli
        Name = ".Csb.YerindeDonusum.WebApp.Security.Cookies", //eski cookie adınız uygunsa kalabilir değilse bu şekilde revize edilebilir
        SameSite = SameSiteMode.Strict, //önemli
        SecurePolicy = CookieSecurePolicy.Always //önemli
    };
});

var app = builder.Build();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseMiddleware<LoggingMiddleware>(new LogOptions()
{
    UseDatabase = false,
    UseMessageQueue = true
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();//Production Ortamında F5 Üzerinden Redirection Yapıldığı İçin Kapatıldı
app.UseStaticFiles();

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (localizeOptions != null) app.UseRequestLocalization(localizeOptions.Value);
app.UseHsts();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<KullaniciParolaZamanAsimiMiddleware>();

app.MapControllerRoute(
    //name: "default",
    //pattern: "{controller=Home}/{action=Index}/{id?}"
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();