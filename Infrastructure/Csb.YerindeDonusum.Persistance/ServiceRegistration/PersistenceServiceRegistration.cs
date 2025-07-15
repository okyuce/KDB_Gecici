using Csb.YerindeDonusum.Application.Interfaces;
using Csb.YerindeDonusum.Persistance.Context;
using Csb.YerindeDonusum.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Csb.YerindeDonusum.Persistance.ServiceRegistration
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IIstisnaAskiKoduRepository, IstisnaAskiKoduRepository>();
            services.AddScoped<IIstisnaAskiKoduDosyaRepository, IstisnaAskiKoduDosyaRepository>();

            return services;
        }
    }
}
