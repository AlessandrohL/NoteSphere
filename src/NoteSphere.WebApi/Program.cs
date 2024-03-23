using Application;
using Application.Abstractions;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WebApi.ContextAcessor;
using WebApi.Extensions;
using WebApi.Middlewares;
using WebApi.Swagger;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    // options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NoteSphere", Version = "v1" });

    c.AddSecurityDefinition("JWT_Cookie", new OpenApiSecurityScheme()
    {
        Name = "accessToken",
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = ParameterLocation.Cookie,
        Description = "JWT Authorization cookie using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });

    c.DocumentFilter<JsonPatchDocumentFilter>();
})
    .AddSwaggerGenNewtonsoftSupport();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddAuthenticationWithJWT(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddScoped<UserContextAccessor>();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, ServiceExtensions.GetJsonPatchInputFormatter());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseMiddleware<ExceptionHandlerMiddleware>();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
