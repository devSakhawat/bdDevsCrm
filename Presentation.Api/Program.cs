using Presentation.Api.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var environment = builder.Environment;

// ── Serilog ──────────────────────────────────────────────
builder.Services.ConfigureSerilog(configuration, environment);
builder.Host.UseSerilog();

// ── Controllers + Newtonsoft JSON ────────────────────────
builder.Services.AddControllers()
    .AddNewtonsoftJson();

// ── Swagger / OpenAPI ────────────────────────────────────
builder.Services.AddSwaggerDocumentation();

// ── CORS ─────────────────────────────────────────────────
builder.Services.ConfigureCors(configuration);

// ── IIS Integration ──────────────────────────────────────
builder.Services.ConfigureIisIntegration();

// ── Response Compression ─────────────────────────────────
builder.Services.ConfigureResponseCompression();
builder.Services.ConfigureGzipCompression();

// ── File Upload Limit ────────────────────────────────────
builder.Services.ConfigureFileLimit();

// ── Cookie Policy ────────────────────────────────────────
builder.Services.ConfigureCookiePolicy(environment);

// ── Application Insights ─────────────────────────────────
builder.Services.ConfigureApplicationInsights(configuration);

// ── Distributed Cache (Redis / Memory) ───────────────────
builder.Services.ConfigureDistributedCache(configuration);

// ── EF Core Interceptors + DbContext ─────────────────────
builder.Services.AddInterceptors();
builder.Services.AddSqlContext(configuration);

// ── Repository Manager ───────────────────────────────────
builder.Services.AddRepositoryManager();

// ── Service Manager + Infrastructure ─────────────────────
builder.Services.AddServiceManager(configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddMapster();

// ── Authentication + Authorization ───────────────────────
builder.Services.AddJwtAuthentication(configuration);
builder.Services.AddPasswordSecurity(configuration);
builder.Services.AddAuthorizationPolicies();

var app = builder.Build();

// ── Swagger UI (Development only) ────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ── Middleware pipeline ──────────────────────────────────
app.UseApiMiddleware(configuration);

// ── Endpoints ────────────────────────────────────────────
app.MapControllers();

app.Run();
