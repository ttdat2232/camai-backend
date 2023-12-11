﻿using Core.Domain.Interfaces.Repositories;
using Core.Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Repositories
{
    public static class DependencyInjection
    {
        public static IServiceCollection RepositoryDependencyInjection(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
            services.AddScoped(typeof(IRepository<>), typeof(Base.Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
