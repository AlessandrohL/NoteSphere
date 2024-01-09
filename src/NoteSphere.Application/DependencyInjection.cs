using Application.Abstractions;
using Application.Identity;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {           
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IAuthenticationService<UserAuth>, AuthenticationService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<INoteBookService, NoteBookService>();
            services.AddScoped<INoteService, NoteService>();
            
            return services;
        }
    }
}