using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace Presentation.Api.Extensions;

public static class ConfigureSwagger
{
  public static void AddSwaggerDocumentation(
      this IServiceCollection services)
  {
    services.AddSwaggerGen(options =>
    {
      options.SwaggerDoc("v1", new OpenApiInfo
      {
        Title = "bdDevsCrm API",
        Version = "v1"
      });

      options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
      {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Format: Bearer {token}"
      });

      options.AddSecurityRequirement(document =>
          new OpenApiSecurityRequirement
          {
                    {
                        new OpenApiSecuritySchemeReference(
                            "Bearer", document),
                        new List<string>()
                    }
          });
    });
  }
}