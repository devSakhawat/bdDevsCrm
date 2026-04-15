using bdDevs.Shared;
using Presentation.Api.Middleware.Shared;
using Domain.Exceptions.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Presentation.Api.Middleware;



public class StandardExceptionMiddleware
{
  private readonly RequestDelegate _next;
  private readonly IHostEnvironment _env;
  private readonly ILogger<StandardExceptionMiddleware> _logger;
  private const string ApiVersion = "1.0";

  public StandardExceptionMiddleware(
      RequestDelegate next,
      IHostEnvironment env,
      ILogger<StandardExceptionMiddleware> logger)
  {
    _next = next;
    _env = env;
    _logger = logger;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private async Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    var correlationId =
        context.Items["CorrelationId"] as string
        ?? context.TraceIdentifier
        ?? Guid.NewGuid().ToString();

    _logger.LogError(ex,
        "Error | CorrelationId: {CorrelationId} | Type: {Type} | Message: {Message}",
        correlationId,
        ex.GetType().Name,
        ex.Message);

    context.Response.ContentType = "application/json";

    var response = MapException(ex, correlationId);
    context.Response.StatusCode = response.StatusCode;

    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      WriteIndented = _env.IsDevelopment()
    };

    var json = JsonSerializer.Serialize(response, options);
    await context.Response.WriteAsync(json);
  }

  // ================= CORE MAPPING =================
  private ApiResponse<object> MapException(Exception ex, string correlationId)
  {
    // ✅ 1. AppException (MAIN SYSTEM)
    if (ex is AppException appEx)
    {
      return CreateResponse(
          appEx.StatusCode,
          appEx.Message,
          appEx.ErrorCode,
          ex.GetType().Name,
          correlationId,
          details: _env.IsDevelopment() ? GetInnermostMessage(ex) : null,
          stackTrace: _env.IsDevelopment() ? ex.StackTrace : null
      );
    }

    // ✅ 2. JWT / Security
    if (ex is Microsoft.IdentityModel.Tokens.SecurityTokenExpiredException)
    {
      return CreateResponse(401,
          "Your session has expired. Please login again.",
          "TOKEN_EXPIRED",
          "SecurityTokenExpired",
          correlationId);
    }

    if (ex is Microsoft.IdentityModel.Tokens.SecurityTokenException)
    {
      return CreateResponse(401,
          "Invalid authentication token.",
          "INVALID_TOKEN",
          "SecurityTokenInvalid",
          correlationId);
    }

    // ✅ 3. Authorization fallback
    if (ex is UnauthorizedAccessException)
    {
      return CreateResponse(401,
          "You are not authorized.",
          "UNAUTHORIZED",
          "UnauthorizedAccess",
          correlationId);
    }

    // ✅ 4. Validation / Argument
    if (ex is ArgumentNullException argNull)
    {
      return CreateResponse(400,
          $"Required parameter '{argNull.ParamName}' is missing.",
          "ARGUMENT_NULL",
          "ArgumentNull",
          correlationId);
    }

    if (ex is ArgumentException)
    {
      return CreateResponse(400,
          ex.Message,
          "ARGUMENT_ERROR",
          "ArgumentError",
          correlationId);
    }

    if (ex is System.ComponentModel.DataAnnotations.ValidationException)
    {
      return CreateResponse(400,
          "Validation failed.",
          "VALIDATION_ERROR",
          "Validation",
          correlationId);
    }

    // ✅ 5. Database
    if (ex is Microsoft.EntityFrameworkCore.DbUpdateException)
    {
      return CreateResponse(500,
          SanitizeDbMessage(ex),
          "DATABASE_ERROR",
          "DatabaseError",
          correlationId,
          stackTrace: _env.IsDevelopment() ? ex.StackTrace : null);
    }

    // ✅ 6. Timeout (fallback safety)
    if (ex is TaskCanceledException)
    {
      return CreateResponse(408,
          "Request timeout.",
          "REQUEST_TIMEOUT",
          "TaskCanceled",
          correlationId);
    }

    // ✅ 7. FINAL FALLBACK
    return CreateResponse(500,
        _env.IsDevelopment() ? GetInnermostMessage(ex) : "An unexpected error occurred.",
        "INTERNAL_ERROR",
        ex.GetType().Name,
        correlationId,
        details: _env.IsDevelopment() ? GetInnermostMessage(ex) : null,
        stackTrace: _env.IsDevelopment() ? ex.StackTrace : null);
  }

  // ================= RESPONSE BUILDER =================
  private ApiResponse<object> CreateResponse(
      int statusCode,
      string message,
      string errorCode,
      string errorType,
      string correlationId,
      string? details = null,
      string? stackTrace = null)
  {
    return new ApiResponse<object>
    {
      StatusCode = statusCode,
      Success = false,
      Message = message,
      Version = ApiVersion,
      CorrelationId = correlationId,
      Error = new ErrorDetails
      {
        Code = errorCode,
        Type = errorType,
        Details = details,
        StackTrace = stackTrace
      }
    };
  }

  // ================= HELPERS =================
  private string GetInnermostMessage(Exception ex) =>
      ex.InnerException?.InnerException?.Message
      ?? ex.InnerException?.Message
      ?? ex.Message;

  private string SanitizeDbMessage(Exception ex)
  {
    var msg = GetInnermostMessage(ex);

    if (msg.Contains("foreign key", StringComparison.OrdinalIgnoreCase))
      return "Cannot delete this record because it is referenced by other data.";

    if (msg.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
        msg.Contains("unique", StringComparison.OrdinalIgnoreCase))
      return "Duplicate data found.";

    if (msg.Contains("null", StringComparison.OrdinalIgnoreCase))
      return "Required field missing.";

    return "Database operation failed.";
  }
}












//public class StandardExceptionMiddleware
//{
//  private readonly RequestDelegate _next;
//  private readonly IHostEnvironment _env;
//  private readonly ILogger<StandardExceptionMiddleware> _logger;
//  private const string ApiVersion = "1.0";

//  public StandardExceptionMiddleware(RequestDelegate next, IHostEnvironment env, ILogger<StandardExceptionMiddleware> logger)
//  {
//    _next = next;
//    _env = env;
//    _logger = logger;
//  }

//  public async Task InvokeAsync(HttpContext context)
//  {
//    try
//    {
//      await _next(context);
//    }
//    catch (Exception ex)
//    {
//      await HandleExceptionAsync(context, ex);
//    }
//  }

//  private async Task HandleExceptionAsync(HttpContext context, Exception ex)
//  {
//    // Conflict #4 Fix:  Correlation ID from PipelineContext
//    // If PipelineContext doesn't exist (crash before CorrelationIdMiddleware),
//    // then use fallback
//    var pipelineCtx = RequestPipelineContext.Get(context);
//    string correlationId = pipelineCtx?.CorrelationId
//                           ?? context.Items["CorrelationId"] as string
//                           ?? context.TraceIdentifier
//                           ?? Guid.NewGuid().ToString();

//    // Structured logging (no $ interpolation)
//    _logger.LogError(ex,
//        "[{CorrelationId}] {ExceptionType}: {ExceptionMessage}",
//        correlationId, ex.GetType().Name, ex.Message);

//    context.Response.ContentType = "application/json";

//    var response = MapExceptionToStandardResponse(ex, correlationId);
//    context.Response.StatusCode = response.StatusCode;

//    var options = new JsonSerializerOptions
//    {
//      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//      WriteIndented = _env.IsDevelopment(),
//    };

//    var json = JsonSerializer.Serialize(response, options);
//    await context.Response.WriteAsync(json);
//  }

//  // ... The rest of MapExceptionToStandardResponse, CreateStandardResponse,
//  //     MostRelevantMessage, SanitizeDatabaseErrorMessage
//  //     methods remain the same as before (provided in previous response)

//  private ApiResponse<object> MapExceptionToStandardResponse(Exception ex, string correlationId)
//  {
//    if (ex is BaseCustomException customEx)
//    {
//      return CreateStandardResponse(
//          customEx.StatusCode,
//          customEx.UserFriendlyMessage ?? customEx.Message,
//          customEx.ErrorCode,
//          ex.GetType().Name,
//          _env.IsDevelopment() ? MostRelevantMessage(ex) : null,
//          customEx.AdditionalData,
//          correlationId,
//          _env.IsDevelopment() ? customEx.StackTrace : null);
//    }

//    return ex switch
//    {
//      // ==================== 409 CONFLICT ====================
//      GenericConflictException or
//      DuplicateRecordException or
//      ConflictException =>
//          CreateStandardResponse(
//              statusCode: 409,
//              message: ex.Message,
//              errorCode: "CONFLICT",
//              errorType: ex.GetType().Name,
//              details: null,
//              additionalData: null,
//              correlationId: correlationId),

//      // ==================== 400 BAD REQUEST ====================
//      InvalidCreateOperationException or
//      InvalidOperationExceptionEx or
//      BadRequestException or
//      BadRequestException or
//      BadRequestException or
//      UsernamePasswordMismatchException or
//      BadRequestException =>
//          CreateStandardResponse(
//              statusCode: 400,
//              message: ex.Message,
//              errorCode: "BAD_REQUEST",
//              errorType: ex.GetType().Name,
//              details: null,
//              additionalData: null,
//              correlationId: correlationId),

//      NotFoundException or NotFoundException =>
//          CreateStandardResponse(404, ex.Message, "NOT_FOUND", ex.GetType().Name, null, null, correlationId),

//      GenericUnauthorizedException or UnauthorizedException =>
//          CreateStandardResponse(401, ex.Message, "UNAUTHORIZED", ex.GetType().Name, null, null, correlationId),

//      ForbiddenAccessException =>
//          CreateStandardResponse(403, ex.Message, "FORBIDDEN", ex.GetType().Name, null, null, correlationId),

//      //ServiceUnavailableException =>
//      //    CreateStandardResponse(503, ex.Message, "SERVICE_UNAVAILABLE", ex.Type().Name, null, null, correlationId),

//      SecurityTokenExpiredException =>
//          CreateStandardResponse(401, "Your authentication token has expired. Please log in again.",
//              "TOKEN_EXPIRED", "SecurityTokenExpired", null, null, correlationId),

//      SecurityTokenException or SecurityTokenValidationException =>
//          CreateStandardResponse(401, "Invalid authentication token. Please log in again.",
//              "INVALID_TOKEN", "SecurityTokenInvalid", null, null, correlationId),

//      UnauthorizedAccessException =>
//          CreateStandardResponse(401, "You are not authorized to perform this action.",
//              "UNAUTHORIZED_ACCESS", "UnauthorizedAccess", null, null, correlationId),

//      ValidationException =>
//          CreateStandardResponse(400, "One or more validation errors occurred.",
//              "VALIDATION_ERROR", "Validation", null, null, correlationId),

//      ArgumentNullException argNull =>
//          CreateStandardResponse(400, $"Required parameter '{argNull.ParamName}' is missing.",
//              "ARGUMENT_NULL", "ArgumentNull", null, null, correlationId),

//      ArgumentException =>
//          CreateStandardResponse(400, ex.Message, "ARGUMENT_ERROR", "ArgumentError", null, null, correlationId),

//      KeyNotFoundException =>
//          CreateStandardResponse(404, "The requested resource was not found.",
//              "KEY_NOT_FOUND", "KeyNotFound", null, null, correlationId),

//      DbUpdateException =>
//          CreateStandardResponse(500, SanitizeDatabaseErrorMessage(ex),
//              "DATABASE_ERROR", "DatabaseError", null, null, correlationId,
//              _env.IsDevelopment() ? MostRelevantStackTrace(ex) : null),


//      //// 400 BAD REQUEST ====================
//      ////CollectionByIdsBadRequestException or
//      ////CommonBadReuqestException or
//      ////IdParametersBadRequestException or
//      ////BadRequestException or
//      ////BadRequestException =>
//      //		CreateStandardResponse(
//      //				statusCode: 400,
//      //				message: ex.Message,
//      //				errorCode: "BAD_REQUEST",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 400 FILE SIZE ====================
//      //FileSizeExceededException =>
//      //		CreateStandardResponse(
//      //				statusCode: 400,
//      //				message: ex.Message,
//      //				errorCode: "FILE_TOO_LARGE",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 404 NOT FOUND ====================
//      //NotFoundException or
//      //NotFoundException =>
//      //		CreateStandardResponse(
//      //				statusCode: 404,
//      //				message: ex.Message,
//      //				errorCode: "NOT_FOUND",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 401 UNAUTHORIZED ====================
//      //GenericUnauthorizedException or
//      //UnauthorizedException =>
//      //		CreateStandardResponse(
//      //				statusCode: 401,
//      //				message: ex.Message,
//      //				errorCode: "UNAUTHORIZED",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 401 UNAUTHORIZED ====================
//      //UnauthorizedAccessCRMException =>
//      //		CreateStandardResponse(
//      //				statusCode: 401,
//      //				message: ex.Message,
//      //				errorCode: "UNAUTHORIZED",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 403 FORBIDDEN ====================
//      //ForbiddenAccessException =>
//      //		CreateStandardResponse(
//      //				statusCode: 403,
//      //				message: ex.Message,
//      //				errorCode: "FORBIDDEN",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 403 FORBIDDEN ====================
//      //AccessDeniedException =>
//      //		CreateStandardResponse(
//      //				statusCode: 403,
//      //				message: ex.Message,
//      //				errorCode: "FORBIDDEN",
//      //				errorType: ex.Type().Name,
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      // ==================== 503 SERVICE UNAVAILABLE ====================
//      ServiceUnavailableException =>
//          CreateStandardResponse(
//              statusCode: 503,
//              message: ex.Message,
//              errorCode: "SERVICE_UNAVAILABLE",
//              errorType: ex.GetType().Name,
//              details: null,
//              additionalData: null,
//              correlationId: correlationId),

//      // ==================== 408 REQUEST TIMEOUT ====================
//      RequestTimeoutException =>
//          CreateStandardResponse(
//              statusCode: 408,
//              message: ex.Message,
//              errorCode: "REQUEST_TIMEOUT",
//              errorType: ex.GetType().Name,
//              details: null,
//              additionalData: null,
//              correlationId: correlationId),

//      // ==================== 500 DATA MAPPING ====================
//      DataMappingException =>
//          CreateStandardResponse(
//              statusCode: 500,
//              message: "Data mapping failed",
//              errorCode: "DATA_MAPPING_ERROR",
//              errorType: ex.GetType().Name,
//              details: _env.IsDevelopment() ? MostRelevantMessage(ex) : null,
//              additionalData: new Dictionary<string, object>
//              {
//                ["ColumnName"] = (ex as DataMappingException)?.ColumnName,
//                ["PropertyName"] = (ex as DataMappingException)?.PropertyName,
//                ["PropertyType"] = (ex as DataMappingException)?.PropertyType,
//                ["EntityType"] = (ex as DataMappingException)?.EntityType,
//                ["RawValue"] = (ex as DataMappingException)?.RawValue
//              },
//              correlationId: correlationId,
//              stackTrace: _env.IsDevelopment() ? ex.StackTrace : null),

//      //// ==================== 401 SECURITY TOKEN ====================
//      //SecurityTokenExpiredException =>
//      //		CreateStandardResponse(
//      //				statusCode: 401,
//      //				message: "Your authentication token has expired. Please log in again.",
//      //				errorCode: "TOKEN_EXPIRED",
//      //				errorType: "SecurityTokenExpired",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      ////SecurityTokenException or
//      //SecurityTokenValidationException =>
//      //		CreateStandardResponse(
//      //				statusCode: 401,
//      //				message: "Invalid authentication token. Please log in again.",
//      //				errorCode: "INVALID_TOKEN",
//      //				errorType: "SecurityTokenInvalid",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 401 UNAUTHORIZED ACCESS ====================
//      //UnauthorizedAccessException =>
//      //		CreateStandardResponse(
//      //				statusCode: 401,
//      //				message: "You are not authorized to perform this action.",
//      //				errorCode: "UNAUTHORIZED_ACCESS",
//      //				errorType: "UnauthorizedAccess",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 400 VALIDATION ====================
//      //ValidationException =>
//      //		CreateStandardResponse(
//      //				statusCode: 400,
//      //				message: "One or more validation errors occurred.",
//      //				errorCode: "VALIDATION_ERROR",
//      //				errorType: "Validation",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 400 ARGUMENT NULL ====================
//      //ArgumentNullException argNull =>
//      //		CreateStandardResponse(
//      //				statusCode: 400,
//      //				message: $"Required parameter '{argNull.ParamName}' is missing.",
//      //				errorCode: "ARGUMENT_NULL",
//      //				errorType: "ArgumentNull",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 404 KEY NOT FOUND ====================
//      //KeyNotFoundException =>
//      //		CreateStandardResponse(
//      //				statusCode: 404,
//      //				message: "The requested resource was not found.",
//      //				errorCode: "KEY_NOT_FOUND",
//      //				errorType: "KeyNotFound",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId),

//      //// ==================== 500 DATABASE ERROR ====================
//      //DbUpdateException =>
//      //		CreateStandardResponse(
//      //				statusCode: 500,
//      //				message: SanitizeDatabaseErrorMessage(ex),
//      //				errorCode: "DATABASE_ERROR",
//      //				errorType: "DatabaseError",
//      //				details: null,
//      //				additionalData: null,
//      //				correlationId: correlationId,
//      //				stackTrace: _env.IsDevelopment() ? MostRelevantStackTrace(ex) : null),


//      ////New case for DataMappingException
//      //DataMappingException =>
//      //	CreateStandardResponse(500, "Data mapping failed", "DATA_MAPPING_ERROR", "DataMapping", null, null, correlationId),


//      // Generic fallback for unhandled exceptions
//      _ => CreateStandardResponse(500,
//          _env.IsDevelopment() ? MostRelevantMessage(ex) : "An unexpected error occurred. Please try again later.",
//          "INTERNAL_ERROR", ex.GetType().Name,
//          _env.IsDevelopment() ? MostRelevantMessage(ex) : null,
//          null, correlationId,
//          _env.IsDevelopment() ? MostRelevantStackTrace(ex) : null)
//    };
//  }

//  private ApiResponse<object> CreateStandardResponse(int statusCode, string message, string errorCode, string errorType, string? details, Dictionary<string, object>? additionalData,
//      string correlationId, string? stackTrace = null)
//  {
//    return new ApiResponse<object>
//    {
//      StatusCode = statusCode,
//      Success = false,
//      Message = message,
//      Version = ApiVersion,
//      CorrelationId = correlationId,
//      Error = new ErrorDetails
//      {
//        Code = errorCode,
//        Type = errorType,
//        Details = details,
//        StackTrace = stackTrace,
//        AdditionalData = additionalData
//      }
//    };
//  }

//  private string MostRelevantMessage(Exception ex) =>
//      ex.InnerException?.InnerException?.Message
//      ?? ex.InnerException?.Message
//      ?? ex.Message;

//  private string? MostRelevantStackTrace(Exception ex) =>
//      ex.InnerException?.InnerException?.StackTrace
//      ?? ex.InnerException?.StackTrace
//      ?? ex.StackTrace;

//  private string SanitizeDatabaseErrorMessage(Exception ex)
//  {
//    var msg = MostRelevantMessage(ex);
//    if (msg.Contains("foreign key", StringComparison.OrdinalIgnoreCase))
//      return "Cannot delete this record because it is referenced by other data.";
//    if (msg.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
//        msg.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
//      return "This data already exists. Please use a different value.";
//    if (msg.Contains("null", StringComparison.OrdinalIgnoreCase) ||
//        msg.Contains("required", StringComparison.OrdinalIgnoreCase))
//      return "Required field is missing. Please provide all necessary information.";
//    if (msg.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
//        msg.Contains("connection", StringComparison.OrdinalIgnoreCase))
//      return "Database connection issue. Please try again later.";
//    return "A database error occurred. Please verify your input and try again.";
//  }
//}





//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.CRM;
//using bdDevs.Shared;
//using Domain.Exceptions;
//using Domain.Exceptions;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.ComponentModel.DataAnnotations;
//using System.Text.Json;

//namespace Presentation.Api.Middleware;

///// <summary>
///// Enhanced global exception handling middleware using standardized response format
///// Captures all unhandled exceptions and returns consistent API responses
///// </summary>
//public class StandardExceptionMiddleware
//{
//    private readonly RequestDelegate _next;
//    private readonly IHostEnvironment _env;
//    private readonly ILogger<StandardExceptionMiddleware> _logger;
//    private readonly ILogger<StandardExceptionMiddleware> _loggerManager;
//    private const string ApiVersion = "1.0";

//    public StandardExceptionMiddleware( RequestDelegate next, IHostEnvironment env, ILogger<StandardExceptionMiddleware> logger, ILogger<StandardExceptionMiddleware> loggerManager)
//    {
//        _next = next;
//        _env = env;
//        _logger = logger;
//        _loggerManager = loggerManager;
//    }

//    public async Task InvokeAsync(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception ex)
//        {
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
//    {
//        // Generate correlation ID for tracking
//        string correlationId = Guid.NewGuid().ToString();

//        // Log the error with full details
//        _logger.LogError(ex, $"[{correlationId}] {ex.Type().Name}: {ex.Message}");
//        _loggerManager.LogError($"[{correlationId}] {ex.Type().Name}: {ex.Message}");

//        context.Response.ContentType = "application/json";

//        // Map exception to standardized API response
//        var response = MapExceptionToStandardResponse(ex, correlationId);

//        context.Response.StatusCode = response.StatusCode;

//        var options = new JsonSerializerOptions
//        {
//            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
//            WriteIndented = _env.IsDevelopment(),
//        };

//        var json = JsonSerializer.Serialize(response, options);
//        await context.Response.WriteAsync(json);
//    }

//    private ApiResponse<object> MapExceptionToStandardResponse(Exception ex, string correlationId)
//    {
//        // Handle BaseCustomException first (highest priority)
//        if (ex is BaseCustomException customEx)
//        {
//            return CreateStandardResponse(
//                customEx.StatusCode,
//                customEx.UserFriendlyMessage ?? customEx.Message,
//                customEx.ErrorCode,
//                ex.Type().Name,
//                MostRelevantMessage(ex),
//                customEx.AdditionalData,
//                correlationId,
//                _env.IsDevelopment() ? customEx.StackTrace : null
//            );
//        }

//        // Pattern matching for specific exceptions
//        return ex switch
//        {
//            // Conflict Exceptions (409)
//            GenericConflictException or DuplicateRecordException or ConflictException =>
//                CreateStandardResponse(409, ex.Message, "CONFLICT", ex.Type().Name, null, null, correlationId),

//            // BadRequest Exceptions (400)
//            InvalidCreateOperationException or InvalidOperationExceptionEx or
//            BadRequestException or BadRequestException or
//            BadRequestException or UsernamePasswordMismatchException or BadRequestException =>
//                CreateStandardResponse(400, ex.Message, "BAD_REQUEST", ex.Type().Name, null, null, correlationId),

//            // NotFound Exceptions (404)
//            NotFoundException or NotFoundException =>
//                CreateStandardResponse(404, ex.Message, "NOT_FOUND", ex.Type().Name, null, null, correlationId),

//            // Unauthorized Exceptions (401)
//            GenericUnauthorizedException or UnauthorizedException =>
//                CreateStandardResponse(401, ex.Message, "UNAUTHORIZED", ex.Type().Name, null, null, correlationId),

//            // Forbidden Exceptions (403)
//            ForbiddenAccessException =>
//                CreateStandardResponse(403, ex.Message, "FORBIDDEN", ex.Type().Name, null, null, correlationId),

//            // ServiceUnavailable Exceptions (503)
//            ServiceUnavailableException =>
//                CreateStandardResponse(503, ex.Message, "SERVICE_UNAVAILABLE", ex.Type().Name, null, null, correlationId),

//            // JWT Token Exceptions
//            SecurityTokenExpiredException =>
//                CreateStandardResponse(401, "Your authentication token has expired. Please log in again.",
//                    "TOKEN_EXPIRED", "SecurityTokenExpired", null, null, correlationId),

//            SecurityTokenException or SecurityTokenValidationException =>
//                CreateStandardResponse(401, "Invalid authentication token. Please log in again.",
//                    "INVALID_TOKEN", "SecurityTokenInvalid", null, null, correlationId),

//            System.UnauthorizedAccessException =>
//                CreateStandardResponse(401, "You are not authorized to perform this action.",
//                    "UNAUTHORIZED_ACCESS", "UnauthorizedAccess", null, null, correlationId),

//            // Validation Exceptions
//            ValidationException =>
//                CreateStandardResponse(400, "One or more validation errors occurred.",
//                    "VALIDATION_ERROR", "Validation", null, null, correlationId),

//            ArgumentNullException argNull =>
//                CreateStandardResponse(400, $"Required parameter '{argNull.ParamName}' is missing.",
//                    "ARGUMENT_NULL", "ArgumentNull", null, null, correlationId),

//            ArgumentException =>
//                CreateStandardResponse(400, ex.Message, "ARGUMENT_ERROR", "ArgumentError", null, null, correlationId),

//            KeyNotFoundException =>
//                CreateStandardResponse(404, "The requested resource was not found.",
//                    "KEY_NOT_FOUND", "KeyNotFound", null, null, correlationId),

//            // Database Exceptions
//            DbUpdateException =>
//                CreateStandardResponse(500, SanitizeDatabaseErrorMessage(ex),
//                    "DATABASE_ERROR", "DatabaseError", null, null, correlationId,
//                    _env.IsDevelopment() ? MostRelevantStackTrace(ex) : null),

//            // Generic Fallback
//            _ => CreateStandardResponse(
//                500,
//                _env.IsDevelopment() ? MostRelevantMessage(ex) : "An unexpected error occurred. Please try again later.",
//                "INTERNAL_ERROR",
//                ex.Type().Name,
//                _env.IsDevelopment() ? MostRelevantMessage(ex) : null,
//                null,
//                correlationId,
//                _env.IsDevelopment() ? MostRelevantStackTrace(ex) : null
//            )
//        };
//    }

//    private ApiResponse<object> CreateStandardResponse(
//        int statusCode,
//        string message,
//        string errorCode,
//        string errorType,
//        string details,
//        Dictionary<string, object> additionalData,
//        string correlationId,
//        string stackTrace = null)
//    {
//        return new ApiResponse<object>
//        {
//            StatusCode = statusCode,
//            Success = false,
//            Message = message,
//            Version = ApiVersion,
//            CorrelationId = correlationId,
//            Error = new ErrorDetails
//            {
//                Code = errorCode,
//                Type = errorType,
//                Details = details,
//                StackTrace = stackTrace,
//                AdditionalData = additionalData
//            }
//        };
//    }

//    private string MostRelevantMessage(Exception exception)
//    {
//        if (exception.InnerException?.InnerException != null)
//            return exception.InnerException.InnerException.Message;
//        if (exception.InnerException != null)
//            return exception.InnerException.Message;
//        return exception.Message;
//    }

//    private string MostRelevantStackTrace(Exception exception)
//    {
//        if (exception.InnerException?.InnerException != null)
//            return exception.InnerException.InnerException.StackTrace;
//        if (exception.InnerException != null)
//            return exception.InnerException.StackTrace;
//        return exception.StackTrace;
//    }

//    private string SanitizeDatabaseErrorMessage(Exception exception)
//    {
//        string message = MostRelevantMessage(exception);

//        if (message.Contains("foreign key", StringComparison.OrdinalIgnoreCase))
//            return "Cannot delete this record because it is referenced by other data. Please remove related records first.";

//        if (message.Contains("unique", StringComparison.OrdinalIgnoreCase) ||
//            message.Contains("duplicate", StringComparison.OrdinalIgnoreCase))
//            return "This data already exists. Please use a different value.";

//        if (message.Contains("null", StringComparison.OrdinalIgnoreCase) ||
//            message.Contains("required", StringComparison.OrdinalIgnoreCase))
//            return "Required field is missing. Please provide all necessary information.";

//        if (message.Contains("timeout", StringComparison.OrdinalIgnoreCase) ||
//            message.Contains("connection", StringComparison.OrdinalIgnoreCase))
//            return "Database connection issue. Please try again later.";

//        return "A database error occurred. Please verify your input and try again.";
//    }
//}
