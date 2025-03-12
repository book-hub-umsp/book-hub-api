using Microsoft.OpenApi.Models;

namespace BookHub.API.Service.Registrations;

internal static class SwaggerExtensions
{
    public static IServiceCollection AddSwagger(
        this IServiceCollection services)
        => services
            .AddSwaggerGen(c =>
            {
                c.UseOneOfForPolymorphism();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            })
            .AddSwaggerGenNewtonsoftSupport();
}
