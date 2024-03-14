using Application.Abstractions;
using Application.Identity;
using Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<IAuthenticationService<UserAuth>, AuthenticationService>();
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<INotebookService, NotebookService>();
            services.AddScoped<INoteService, NoteService>();

            ValidatorOptions.Global.LanguageManager.Enabled = false;
            services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly);

            return services;
        }
    }
}