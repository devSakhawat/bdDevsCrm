using Swashbuckle.AspNetCore.SwaggerGen;

namespace Presentation.Api.Extensions;

public static class ConfigureSwagger
{
  /// <summary>
  /// Configures Swagger/OpenAPI documentation with JWT Bearer authentication support
  /// </summary>
  public static void AddSwaggerDocumentation(
      this IServiceCollection services)
  {
    services.AddSwaggerGen();
    services.AddEndpointsApiExplorer();
  }
}
