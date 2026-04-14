# Backend Design - bdDevsCrm

## 📖 Overview

এই ডকুমেন্টে **bdDevsCrm** প্রজেক্টের backend architecture বিস্তারিত বর্ণনা করা হয়েছে। আমরা **Clean Architecture** এবং **SOLID Principles** অনুসরণ করে একটি scalable, maintainable এবং enterprise-ready backend তৈরি করছি।

---

## 🏗️ Architecture Layers

### Clean Architecture Pattern

```
┌─────────────────────────────────────────────┐
│         Presentation Layer                  │
│  Controllers, API Endpoints, ViewModels     │
└──────────────────┬──────────────────────────┘
                   │ (Depends on)
┌──────────────────▼──────────────────────────┐
│         Application Layer                   │
│  Application.Services (Business Logic)      │
└──────────────────┬──────────────────────────┘
                   │ (Depends on)
┌──────────────────▼──────────────────────────┐
│         Infrastructure Layer                │
│  Repositories, Data Access, External APIs   │
└──────────────────┬──────────────────────────┘
                   │ (Depends on)
┌──────────────────▼──────────────────────────┐
│            Domain Layer                     │
│  Entities, Interfaces, Domain Logic         │
└─────────────────────────────────────────────┘
       ▲
       │ (Used by all layers)
┌──────┴──────────────────────────────────────┐
│         Shared Kernel                       │
│  Common Utilities, Constants, Extensions    │
└─────────────────────────────────────────────┘
```

---

## 1️⃣ Domain Layer (Core Business)

**Projects**:
- `Domain.Entities` - Business entities এবং aggregates
- `Domain.Contracts` - Repository interfaces
- `Domain.Exceptions` - Custom domain exceptions

### Responsibilities

✅ **Domain Entities**: Core business objects
✅ **Value Objects**: Immutable domain concepts
✅ **Domain Events**: Business events
✅ **Repository Interfaces**: Data access contracts
✅ **Domain Exceptions**: Business rule violations

### Key Characteristics

- ❌ **NO external dependencies** (completely independent)
- ❌ **NO framework dependencies** (pure C#)
- ✅ **Contains only business logic**
- ✅ **Framework-agnostic**

### Example Structure

```csharp
// Domain.Entities/User.cs
namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public string PasswordHash { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedDate { get; set; }

        // Navigation properties
        public virtual Company Company { get; set; }
        public virtual Employee Employee { get; set; }

        // Domain methods (business logic)
        public bool IsActive() => StatusId == 1;
        public bool CanLogin() => IsActive() && !string.IsNullOrEmpty(PasswordHash);
    }
}

// Domain.Contracts/IUserRepository.cs
namespace Domain.Contracts
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByLoginIdAsync(string loginId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int userId);
    }
}

// Domain.Exceptions/UserNotFoundException.cs
namespace Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(int userId)
            : base($"User with ID {userId} was not found")
        {
        }
    }
}
```

---

## 2️⃣ Application Layer (Business Logic)

**Projects**:
- `Application.Services` - Service implementations (business logic)

### Architecture Change (Refactored)

আগের structure:
```
❌ Application.ServiceContracts  - Service interfaces
❌ Application.Shared            - Shared DTOs, models
✅ Application.Services           - Business logic (KEPT)
```

নতুন simplified structure:
```
✅ Application.Services (All-in-one)
   ├── Services/          - Service implementations
   ├── Contracts/         - Service interfaces (moved here)
   ├── DTOs/              - Data Transfer Objects (moved here)
   ├── Validators/        - FluentValidation validators
   └── Mappings/          - AutoMapper profiles
```

### Responsibilities

✅ **Business Logic**: Core application workflows
✅ **Use Cases**: Application-specific operations
✅ **Validation**: Business rule validation
✅ **Data Transformation**: Entity ↔ DTO mapping
✅ **Coordination**: Between repositories এবং external services

### Service Layer Pattern

```csharp
// Application.Services/Contracts/IUserService.cs
namespace Application.Services.Contracts
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<UserDTO> GetUserByLoginIdAsync(string loginId);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> CreateUserAsync(CreateUserDTO createUserDto);
        Task<UserDTO> UpdateUserAsync(int userId, UpdateUserDTO updateUserDto);
        Task DeleteUserAsync(int userId);
        Task<LoginResponseDTO> AuthenticateAsync(LoginRequestDTO loginRequest);
    }
}

// Application.Services/Services/UserService.cs
namespace Application.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            IRepositoryManager repository,
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            // 1. Input validation
            if (userId <= 0)
            {
                _logger.LogWarning("Invalid user ID: {UserId}", userId);
                throw new ArgumentException("User ID must be positive", nameof(userId));
            }

            // 2. Business logic - call repository
            var user = await _repository.Users.GetByIdAsync(userId);

            // 3. Business validation
            if (user == null)
            {
                _logger.LogWarning("User not found: {UserId}", userId);
                throw new UserNotFoundException(userId);
            }

            if (user.StatusId == StatusConstants.Deleted)
            {
                _logger.LogWarning("Attempted to access deleted user: {UserId}", userId);
                throw new InvalidOperationException("Cannot access deleted user");
            }

            // 4. Transform to DTO
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<LoginResponseDTO> AuthenticateAsync(LoginRequestDTO loginRequest)
        {
            // 1. Validation
            if (string.IsNullOrWhiteSpace(loginRequest.LoginId))
                throw new ArgumentException("Login ID is required");

            if (string.IsNullOrWhiteSpace(loginRequest.Password))
                throw new ArgumentException("Password is required");

            // 2. Get user
            var user = await _repository.Users.GetByLoginIdAsync(loginRequest.LoginId);

            if (user == null)
            {
                _logger.LogWarning("Login attempt for non-existent user: {LoginId}", loginRequest.LoginId);
                throw new UnauthorizedException("Invalid credentials");
            }

            // 3. Business logic - verify password
            if (!VerifyPassword(loginRequest.Password, user.PasswordHash))
            {
                _logger.LogWarning("Failed login attempt for user: {LoginId}", loginRequest.LoginId);
                throw new UnauthorizedException("Invalid credentials");
            }

            // 4. Check if user can login
            if (!user.CanLogin())
            {
                _logger.LogWarning("Inactive user login attempt: {LoginId}", loginRequest.LoginId);
                throw new UnauthorizedException("User account is inactive");
            }

            // 5. Generate JWT token
            var token = GenerateJwtToken(user);

            // 6. Return response
            return new LoginResponseDTO
            {
                Token = token,
                User = _mapper.Map<UserDTO>(user)
            };
        }
    }
}
```

### DTOs (Data Transfer Objects)

```csharp
// Application.Services/DTOs/UserDTO.cs
namespace Application.Services.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string LoginId { get; set; }
        public string Email { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class CreateUserDTO
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int CompanyId { get; set; }
        public int EmployeeId { get; set; }
    }

    public class UpdateUserDTO
    {
        public string Email { get; set; }
        public int StatusId { get; set; }
    }

    public class LoginRequestDTO
    {
        public string LoginId { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public UserDTO User { get; set; }
    }
}
```

---

## 3️⃣ Infrastructure Layer (Data & External Services)

**Projects**:
- `Infrastructure.Repositories` - Repository implementations
- `Infrastructure.Sql` - EF Core, DbContext, Migrations
- `Infrastructure.Security` - Authentication, Encryption
- `Infrastructure.Utilities` - Helper functions, Extensions

### Responsibilities

✅ **Data Access**: Database operations
✅ **Query Execution**: EF Core query building
✅ **External Services**: Third-party API integrations
✅ **Caching**: Multi-tier caching implementation
✅ **Security**: Password encryption, JWT token generation

### Repository Pattern

```csharp
// Infrastructure.Repositories/RepositoryBase.cs
namespace Infrastructure.Repositories
{
    public abstract class RepositoryBase<T> where T : class
    {
        protected readonly CRMContext _context;

        protected RepositoryBase(CRMContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

// Infrastructure.Repositories/UserRepository.cs
namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(CRMContext context) : base(context) { }

        public async Task<User> GetByIdAsync(int userId)
        {
            // Service layer-এর শর্ত অনুযায়ী query execute করা
            return await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User> GetByLoginIdAsync(string loginId)
        {
            return await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.LoginId == loginId);
        }

        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Company)
                .Include(u => u.Employee)
                .Where(u => u.StatusId == StatusConstants.Active)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

// Infrastructure.Repositories/RepositoryManager.cs
namespace Infrastructure.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly CRMContext _context;
        private IUserRepository _users;
        private ICompanyRepository _companies;

        public RepositoryManager(CRMContext context)
        {
            _context = context;
        }

        public IUserRepository Users =>
            _users ??= new UserRepository(_context);

        public ICompanyRepository Companies =>
            _companies ??= new CompanyRepository(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
```

### DbContext Configuration

```csharp
// Infrastructure.Sql/CRMContext.cs
namespace Infrastructure.Sql
{
    public class CRMContext : DbContext
    {
        public CRMContext(DbContextOptions<CRMContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.LoginId).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.LoginId).IsUnique();

                // Relationships
                entity.HasOne(e => e.Company)
                    .WithMany()
                    .HasForeignKey(e => e.CompanyId);

                entity.HasOne(e => e.Employee)
                    .WithMany()
                    .HasForeignKey(e => e.EmployeeId);
            });

            // Seed data
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    LoginId = "admin",
                    PasswordHash = "hashed_password",
                    CompanyId = 1,
                    EmployeeId = 1,
                    StatusId = 1,
                    CreatedDate = DateTime.UtcNow
                }
            );
        }
    }
}
```

### Multi-tier Caching

```csharp
// Infrastructure.Utilities/Caching/HybridCacheService.cs
namespace Infrastructure.Utilities.Caching
{
    public class HybridCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<HybridCacheService> _logger;

        public async Task<T> GetOrSetAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan? expiry = null)
        {
            // L1: Check memory cache first (fastest)
            if (_memoryCache.TryGetValue(key, out T cachedValue))
            {
                _logger.LogDebug("Cache hit (L1): {Key}", key);
                return cachedValue;
            }

            // L2: Check Redis cache (medium speed)
            var distributedData = await _distributedCache.GetStringAsync(key);
            if (distributedData != null)
            {
                _logger.LogDebug("Cache hit (L2): {Key}", key);
                var value = JsonSerializer.Deserialize<T>(distributedData);

                // Store in L1 for faster subsequent access
                _memoryCache.Set(key, value, TimeSpan.FromMinutes(5));

                return value;
            }

            // L3: Get from database (slowest)
            _logger.LogDebug("Cache miss: {Key}", key);
            var data = await factory();

            // Store in both caches
            await SetAsync(key, data, expiry);

            return data;
        }

        private async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            expiry ??= TimeSpan.FromHours(1);

            // Store in L1 (memory)
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(5));

            // Store in L2 (Redis)
            await _distributedCache.SetStringAsync(
                key,
                JsonSerializer.Serialize(value),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiry
                });
        }
    }
}
```

---

## 4️⃣ Presentation Layer (API & UI)

**Projects**:
- `Presentation.Api` - RESTful Web API
- `Presentation.Mvc` - ASP.NET Core MVC
- `Presentation.Logger` - Centralized logging

### API Controller Pattern

```csharp
// Presentation.Api/Controllers/UsersController.cs
namespace Presentation.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserService userService,
            ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);

                return Ok(new ApiResponse<UserDTO>
                {
                    CorrelationId = HttpContext.TraceIdentifier,
                    StatusCode = 200,
                    Success = true,
                    Message = "User retrieved successfully",
                    Data = user,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogWarning(ex, "User not found: {UserId}", id);

                return NotFound(new ApiResponse<object>
                {
                    CorrelationId = HttpContext.TraceIdentifier,
                    StatusCode = 404,
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO createUserDto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(createUserDto);

                return CreatedAtAction(
                    nameof(GetUser),
                    new { id = user.UserId },
                    new ApiResponse<UserDTO>
                    {
                        CorrelationId = HttpContext.TraceIdentifier,
                        StatusCode = 201,
                        Success = true,
                        Message = "User created successfully",
                        Data = user,
                        Timestamp = DateTime.UtcNow
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");

                return BadRequest(new ApiResponse<object>
                {
                    CorrelationId = HttpContext.TraceIdentifier,
                    StatusCode = 400,
                    Success = false,
                    Message = "Failed to create user",
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Authenticate user
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            try
            {
                var response = await _userService.AuthenticateAsync(loginRequest);

                return Ok(new ApiResponse<LoginResponseDTO>
                {
                    CorrelationId = HttpContext.TraceIdentifier,
                    StatusCode = 200,
                    Success = true,
                    Message = "Login successful",
                    Data = response,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (UnauthorizedException ex)
            {
                _logger.LogWarning(ex, "Failed login attempt");

                return Unauthorized(new ApiResponse<object>
                {
                    CorrelationId = HttpContext.TraceIdentifier,
                    StatusCode = 401,
                    Success = false,
                    Message = "Authentication failed",
                    Errors = new List<string> { ex.Message },
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
```

---

## 5️⃣ Shared Kernel

**Project**: `bdDevs.Shared`

### Common Utilities

```csharp
// bdDevs.Shared/Constants/StatusConstants.cs
namespace bdDevs.Shared.Constants
{
    public static class StatusConstants
    {
        public const int Active = 1;
        public const int Inactive = 2;
        public const int Pending = 3;
        public const int Deleted = 4;
    }
}

// bdDevs.Shared/Extensions/StringExtensions.cs
namespace bdDevs.Shared.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static string ToSafeString(this string value)
        {
            return value?.Trim() ?? string.Empty;
        }
    }
}

// bdDevs.Shared/Models/ApiResponse.cs
namespace bdDevs.Shared.Models
{
    public class ApiResponse<T>
    {
        public string CorrelationId { get; set; }
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
```

---

## 🔑 Key Features

### 1. Dependency Injection

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Add DbContext
builder.Services.AddDbContext<CRMContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbLocation")));

// Add caching
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
```

### 2. Middleware Pipeline

```csharp
// Configure middleware
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Custom middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapControllers();
```

### 3. JWT Authentication

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });
```

---

## 📊 Data Flow Diagram

```
┌──────────────┐
│   Client     │
│  (Browser)   │
└──────┬───────┘
       │ HTTP Request
       ▼
┌──────────────────────┐
│   API Controller     │
│  (Presentation)      │
└──────┬───────────────┘
       │ DTO
       ▼
┌──────────────────────┐
│   Service Layer      │
│  (Application)       │ ◄─── Business Logic, Validation
└──────┬───────────────┘
       │ Conditions
       ▼
┌──────────────────────┐
│  Repository Layer    │
│  (Infrastructure)    │ ◄─── Query Building
└──────┬───────────────┘
       │ EF Core Query
       ▼
┌──────────────────────┐
│     Database         │
│   (MS SQL Server)    │
└──────┬───────────────┘
       │ Entity
       ▼
┌──────────────────────┐
│  Repository Layer    │ ─────► Entity
└──────┬───────────────┘
       │
       ▼
┌──────────────────────┐
│   Service Layer      │ ─────► Transform to DTO
└──────┬───────────────┘
       │
       ▼
┌──────────────────────┐
│   API Controller     │ ─────► ApiResponse<DTO>
└──────┬───────────────┘
       │
       ▼
┌──────────────┐
│   Client     │ ◄──── JSON Response
└──────────────┘
```

---

## 🎯 Best Practices

### ✅ DO
- Use async/await for all I/O operations
- Return DTOs from services, not entities
- Keep business logic in service layer
- Use repository for data access only
- Implement proper exception handling
- Add logging at all layers
- Use dependency injection
- Follow naming conventions

### ❌ DON'T
- Don't put business logic in repositories
- Don't return entities from controllers
- Don't use sync methods for I/O
- Don't skip validation
- Don't hardcode connection strings
- Don't expose internal exceptions to clients

---

## 📚 References

- Clean Architecture: https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html
- Repository Pattern: https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/infrastructure-persistence-layer-design
- ASP.NET Core Best Practices: https://docs.microsoft.com/en-us/aspnet/core/

---

**Last Updated**: 2026-04-14
**Version**: 2.0.0
**Status**: ✅ Refactored & Simplified
