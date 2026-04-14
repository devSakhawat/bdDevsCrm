using Domain.Contracts.Repositories;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Infrastructure.Sql.Context.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace bdDevsCrm.Api.Extensions;

public static class ConfigureRepositoryManager
{
  public static void AddRepositoryManager(
      this IServiceCollection services)
  {
    services.AddScoped<IRepositoryManager, RepositoryManager>();
  }

  public static void AddInterceptors(
      this IServiceCollection services)
  {
    services.AddHttpContextAccessor();
    services.AddScoped<AuditSaveChangesInterceptor>();
    services.AddScoped<SlowQueryLoggingInterceptor>();
  }

  public static void AddSqlContext(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddDbContext<CRMContext>((serviceProvider, options) =>
    {
      var connectionString =
          configuration.GetConnectionString("DbLocation")
          ?? configuration["ConnectionStrings:DbLocation"];

      options.UseSqlServer(connectionString);

      var auditInterceptor =
          serviceProvider.GetService<AuditSaveChangesInterceptor>();
      var slowQueryInterceptor =
          serviceProvider.GetService<SlowQueryLoggingInterceptor>();

      if (auditInterceptor != null)
        options.AddInterceptors(auditInterceptor);
      if (slowQueryInterceptor != null)
        options.AddInterceptors(slowQueryInterceptor);
    });
  }
}