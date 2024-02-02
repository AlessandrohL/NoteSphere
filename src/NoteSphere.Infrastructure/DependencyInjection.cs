using Application.Abstractions;
using Application.Identity;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Infrastructure.Data.Contexts;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Identity.Jwt;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
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
                ?? throw new ArgumentNullException("Connection string is null.");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connection));

            services.AddIdentityCore<UserAuth>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequiredLength = 8;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSingleton<JwtConfigHelper>();

            services.AddScoped<INotebookRepository, NotebookRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();


            return services;
        }
    }
}
