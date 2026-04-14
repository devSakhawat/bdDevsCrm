using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.FileProviders;
using Serilog;

namespace bdDevsCrm.Api.Extensions;

public static class ServiceExtensions
{
  public static void ConfigureCors(this IServiceCollection services,
      IConfiguration configuration)
  {
    var allowedOrigins = configuration
        .GetSection("Cors:AllowedOrigins")
        .Get<string[]>() ?? Array.Empty<string>();
    var allowCredentials = configuration
        .GetValue<bool>("Cors:AllowCredentials");
    var preflightMaxAge = configuration
        .GetValue<int?>("Cors:PreflightMaxAge") ?? 0;

    services.AddCors(options =>
    {
      options.AddPolicy("CorsPolicy", builder =>
      {
        builder
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();

        if (allowCredentials)
          builder.AllowCredentials();

        if (preflightMaxAge > 0)
          builder.SetPreflightMaxAge(
              TimeSpan.FromSeconds(preflightMaxAge));
      });
    });
  }

  public static void ConfigureIisIntegration(
      this IServiceCollection services) =>
      services.Configure<IISOptions>(options =>
      {
        options.AutomaticAuthentication = false;
      });

  public static void ConfigureResponseCompression(
      this IServiceCollection services)
  {
    services.AddResponseCompression(options =>
    {
      options.EnableForHttps = true;
      options.Providers.Add<GzipCompressionProvider>();
      options.MimeTypes = ResponseCompressionDefaults.MimeTypes
          .Concat(new[] { "application/json" });
    });
  }

  public static void ConfigureGzipCompression(
      this IServiceCollection services)
  {
    services.Configure<GzipCompressionProviderOptions>(options =>
    {
      options.Level = System.IO.Compression.CompressionLevel.Optimal;
    });
  }

  public static void ConfigureFileLimit(
      this IServiceCollection services)
  {
    services.Configure<FormOptions>(options =>
    {
      options.MultipartBodyLengthLimit = 10_000_000;
      options.ValueLengthLimit = int.MaxValue;
      options.ValueCountLimit = int.MaxValue;
      options.KeyLengthLimit = int.MaxValue;
    });
  }

  public static void ConfigureCookiePolicy(
      this IServiceCollection services,
      IWebHostEnvironment environment)
  {
    services.Configure<CookiePolicyOptions>(options =>
    {
      options.MinimumSameSitePolicy = SameSiteMode.Strict;
      options.HttpOnly =
          Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
      options.Secure = environment.IsDevelopment()
          ? CookieSecurePolicy.None
          : CookieSecurePolicy.Always;
    });
  }

  public static void ConfigureApplicationInsights(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    string? connectionString =
        configuration["ApplicationInsights:ConnectionString"];

    if (string.IsNullOrWhiteSpace(connectionString))
      return;

    services.AddApplicationInsightsTelemetry(options =>
    {
      options.ConnectionString = connectionString;

      double? tracesPerSecond = configuration
          .GetValue<double?>("ApplicationInsights:TracesPerSecond");
      if (tracesPerSecond.HasValue)
        options.TracesPerSecond = tracesPerSecond.Value;

      float? samplingRatio = configuration
          .GetValue<float?>("ApplicationInsights:SamplingRatio");
      if (samplingRatio.HasValue)
        options.SamplingRatio = samplingRatio.Value;
    });
  }

  public static void ConfigureDistributedCache(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    var enableDistributedCache = configuration
        .GetValue<bool>("CacheSettings:EnableDistributedCache", false);

    if (enableDistributedCache)
    {
      services.AddStackExchangeRedisCache(options =>
      {
        options.Configuration = configuration["Redis:Configuration"];
        options.InstanceName = configuration["Redis:InstanceName"];
      });
    }
    else
    {
      services.AddDistributedMemoryCache();
    }
  }

  public static void ConfigureSerilog(
      this IServiceCollection services,
      IConfiguration configuration,
      IWebHostEnvironment environment)
  {
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithProperty("Application", "bdDevsCrm")
        .Enrich.WithProperty("Environment", environment.EnvironmentName)
        .Enrich.WithCorrelationId()
        .CreateLogger();

    services.AddSingleton<Serilog.ILogger>(Log.Logger);
  }

  // private mock — unit test safety
  private class WebHostEnvironmentMock : IWebHostEnvironment
  {
    public string EnvironmentName { get; set; } = "Production";
    public string ApplicationName { get; set; } =
        AppDomain.CurrentDomain.FriendlyName;
    public string WebRootPath { get; set; } = string.Empty;
    public string ContentRootPath { get; set; } =
        AppContext.BaseDirectory;
    public IFileProvider? WebRootFileProvider { get; set; }
    public IFileProvider? ContentRootFileProvider { get; set; }
  }
}