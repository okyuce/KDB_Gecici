using System.Reflection;
using Csb.YerindeDonusum.Application.Behaviors;
using Csb.YerindeDonusum.Application.CQRS.SikcaSorulanSoruCQRS.Commands.EkleSikcaSorulanSoru;
using Csb.YerindeDonusum.Application.CustomAddons;
using Csb.YerindeDonusum.Application.Filters;
using Csb.YerindeDonusum.Application.Interfaces;
using CSB.Core.LogHandler.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Csb.YerindeDonusum.Application.ServiceRegistration;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServiceRegistration(this IServiceCollection serviceProviders)
    {
        // proje asseblies bilgilerini dinamik olarak aliyoruz
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        
        // http-s://dev.to/moe23/add-automapper-to-net-6-3fdn
        serviceProviders.AddAutoMapper(assemblies);

        serviceProviders.AddIntegrationExtension();
        // http-s://github.com/jbogard/MediatR
        serviceProviders.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        serviceProviders.AddFluentValidationAutoValidation();
        serviceProviders.AddValidatorsFromAssembly(typeof(EkleSikcaSorulanSoruCommandValidator).GetTypeInfo().Assembly);
		serviceProviders.AddControllers(options => { options.Filters.Add<FluentValidationFilter>(); });
        serviceProviders.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheBehavior<,>));
        serviceProviders.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemoveBehavior<,>));

        serviceProviders.AddScoped<IKullaniciBilgi, KullaniciBilgi>();
        serviceProviders.AddScoped<IMailService, MailService>();
        serviceProviders.AddScoped<ISmsService, CustomAddons.SmsService>();

        return serviceProviders;
    }
}