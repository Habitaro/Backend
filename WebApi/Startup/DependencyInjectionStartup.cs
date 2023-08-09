﻿using DataAccess.Repositories.Abstractions;
using DataAccess.Repositories;
using DataAccess;
using WebApi.Models.Services.Abstractions;
using WebApi.Models.Services;
using Microsoft.EntityFrameworkCore;

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
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
