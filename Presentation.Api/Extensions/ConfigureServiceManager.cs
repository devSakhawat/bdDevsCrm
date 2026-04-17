using Application.Services;
using Application.Services.Caching;
using Application.Services.Core.Infrastructure;
using Application.Services.Mappings;
using bdDevsCrm.Shared.Settings;
using Domain.Contracts.Services;
using Domain.Contracts.Services.Core.Infrastructure;

namespace Presentation.Api.Extensions;

public static class ConfigureServiceManager
{
  public static void AddServiceManager(this IServiceCollection services,  IConfiguration configuration)
  {
    services.AddScoped<IServiceManager, ServiceManager>();
    services.Configure<AppSettings>(configuration.GetSection(AppSettings.SectionName));

    // Startup- validate
    services.AddOptions<AppSettings>()
      .Bind(configuration.GetSection(AppSettings.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();
  }

  public static void AddInfrastructureServices(this IServiceCollection services)
  {
    // HTTP/Cookie/Cache abstraction services
    services.AddScoped<ICookieManagementService, CookieManagementService>();
    services.AddScoped<IHttpContextService, HttpContextService>();
    services.AddScoped<ICacheManagementService, CacheManagementService>();

    // HybridCache — Redis + Memory
    services.AddSingleton<IHybridCacheService, HybridCacheService>();
  }

  public static void AddAppSettings(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.Configure<AppSettings>(
        configuration.GetSection(AppSettings.SectionName));
  }

  public static void AddMapster(
      this IServiceCollection services)
  {
    // Mapster global config register
    services.AddMapsterConfiguration();
  }
}