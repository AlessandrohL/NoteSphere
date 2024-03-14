using Application.Abstractions;
using Application.Helpers;
using Application.Identity;
using Domain.Abstractions;
using Domain.Abstractions.Repositories;
using Infrastructure.Data.Contexts;
using Infrastructure.Email;
using Infrastructure.Helpers;
using Infrastructure.Identity.Extensions;
using Infrastructure.Identity.Jwt;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
using Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
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
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            var emailConfig = configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();

            var clientConfig = configuration
                .GetSection("ClientConfiguration")
                .Get<ClientConfig>();

            services.AddSingleton(emailConfig!);
            services.AddSingleton(clientConfig!);
            services.AddSingleton<JwtConfigHelper>();

            services.AddScoped<IUserManagerExtensions<UserAuth>, UserManagerExtensions>();
            services.AddScoped<INotebookRepository, NotebookRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUrlUtility, UrlUtility>();
            
            return services;
        }
    }
}
