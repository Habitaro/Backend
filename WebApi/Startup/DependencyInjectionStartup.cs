using DataAccess;
using DataAccess.Repositories;
using DataAccess.Repositories.Abstractions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using WebApi.Models.Services;
using WebApi.Models.Services.Abstractions;
using WebApi.Models.Services.Helpers;
using WebApi.Startup.Filters;

namespace WebApi.Startup
{
    public static class DependencyInjectionStartup
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<HabitaroDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("HabitaroDb"));
            });
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddAutoMapper(cfg => cfg.AddProfile<HabitaroMapProfile>());
            services.AddTransient<HabitDayListToDictionaryConverter>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<GlobalExceptionFilter>();
            services.AddHostedService<SeedHabitProgressHostedService>();

            services.AddHangfire(globalConfig => globalConfig
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("HangfireDb")));

            services.AddHangfireServer();

            return services;
        }
    }
}
