using Microsoft.OpenApi;

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
    });
  }
}