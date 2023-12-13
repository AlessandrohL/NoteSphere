using Infrastructure.Data.Contexts;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure
            (this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DevConnection")
                ?? throw new InvalidOperationException("Connection string is null.");

            // Configure DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            // Configure Identity
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();




            return services;
        }
    }
}
