using Application.Services.Authentication.Security;
using Application.Services.Authentication.Settings;
using Domain.Contracts.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation.Api.Extensions;

public static class ConfigureAuthentication
{
  public static void AddJwtAuthentication(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme =
          JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme =
          JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(
                      configuration["Jwt:SecretKey"]!))
      };
    });
  }

  public static void AddPasswordSecurity(
      this IServiceCollection services,
      IConfiguration configuration)
  {
    services.Configure<PasswordHashingSettings>(
        configuration.GetSection(PasswordHashingSettings.SectionName));

    services.AddOptions<PasswordHashingSettings>()
        .Bind(configuration.GetSection(PasswordHashingSettings.SectionName))
        .ValidateDataAnnotations()
        .ValidateOnStart();

    // IPasswordHasher → Infrastructure.Security project-
    services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
  }

  public static void AddAuthorizationPolicies(
      this IServiceCollection services)
  {
    services.AddAuthorization();
  }
}