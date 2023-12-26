using Application.Abstractions;
using Application.Models.Identity;
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
            
            return services;
        }
    }
}