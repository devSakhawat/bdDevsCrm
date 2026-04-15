using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Presentation.ActionFilters;

/// <summary>
/// Action filter — Returns BadRequest when a complex object is null in POST/PUT requests.
/// 
/// IsComplexType() based detection — Does not rely on "Dto" suffix
/// ModelState validation — Includes field-level errors
/// Skips GET/DELETE requests (no body)
/// </summary>
public class EmptyObjectFilterAttribute : IActionFilter
{
	public void OnActionExecuting(ActionExecutingContext context)
	{
		var method = context.HttpContext.Request.Method;

		// Skip body validation for GET and DELETE requests
		if (method.Equals("GET", StringComparison.OrdinalIgnoreCase) ||
			method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		var action = context.RouteData.Values["action"]?.ToString();
		var controller = context.RouteData.Values["controller"]?.ToString();

		// Find complex type argument (without relying on Dto suffix)
		// Complex type = class/struct that is not a primitive type like string, int, Guid, etc.
		var complexArgument = context.ActionArguments
			.FirstOrDefault(arg =>
			{
				if (arg.Value == null) return true; // null argument = potential DTO

				var type = arg.Value.GetType();

				// Skip simple types
				if (type.IsPrimitive || type.IsEnum ||
					type == typeof(string) || type == typeof(decimal) ||
					type == typeof(Guid) || type == typeof(DateTime) ||
					type == typeof(DateTimeOffset) || type == typeof(TimeSpan))
					return false;

				// Skip CancellationToken
				if (type == typeof(CancellationToken)) return false;

				// All remaining class/struct = complex type (DTO, Options, etc.)
				return type.IsClass || type.IsValueType;
			});

		// Skip if no complex argument exists
		// (e.g., action with only simple parameters)
		if (complexArgument.Equals(default(KeyValuePair<string, object?>)))
		{
			return;
		}

		// If complex argument is null, return BadRequest
		if (complexArgument.Value == null)
		{
			context.Result = new BadRequestObjectResult(new
			{
				Message = $"Request body is required. Controller: {controller}, Action: {action}",
				ErrorCode = "NULL_BODY"
			});
			return;
		}

		// If ModelState is invalid, return BadRequest with validation errors
		if (!context.ModelState.IsValid)
		{
			var errors = context.ModelState
				.Where(e => e.Value?.Errors.Count > 0)
				.ToDictionary(
					kvp => kvp.Key,
					kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
				);

			context.Result = new BadRequestObjectResult(new
			{
				Message = "Validation failed",
				Errors = errors,
				ErrorCode = "VALIDATION_ERROR"
			});
		}
	}

	public void OnActionExecuted(ActionExecutedContext context)
	{
	}
}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;

//namespace Presentation.ActionFilters;

///// <summary>
///// Request body DTO null check ও ModelState validation।
///// 
///// </summary>
//public class EmptyObjectFilterAttribute : IActionFilter
//{
//  public void OnActionExecuting(ActionExecutingContext context)
//  {
//    var action = context.RouteData.Values["action"]?.ToString();
//    var controller = context.RouteData.Values["controller"]?.ToString();
//    var method = context.HttpContext.Request.Method;

//    if (method is "GET" or "DELETE" or "HEAD" or "OPTIONS")
//    {
//      if (!context.ModelState.IsValid)
//      {
//        context.Result = new UnprocessableEntityObjectResult(
//            CreateValidationError(context));
//      }
//      return;
//    }

//    // POST/PUT/PATCH — body DTO check
//    var bodyParam = context.ActionArguments
//        .FirstOrDefault(arg =>
//            arg.Value == null ||                                        // null value
//            IsComplexType(arg.Value?.Type()));                       // complex type (DTO/Model)

//    if (bodyParam.Key != null && bodyParam.Value == null)
//    {
//      context.Result = new BadRequestObjectResult(new
//      {
//        Message = $"Request body is null. Controller: {controller}, Action: {action}",
//        ErrorCode = "NULL_REQUEST_BODY"
//      });
//      return;
//    }

//    // ModelState validation
//    if (!context.ModelState.IsValid)
//    {
//      context.Result = new UnprocessableEntityObjectResult(
//          CreateValidationError(context));
//    }
//  }

//  public void OnActionExecuted(ActionExecutedContext context) { }

//  private static bool IsComplexType(Type? type)
//  {
//    if (type == null) return false;

//    // Primitive types, strings, value types → no complex type 
//    return !type.IsPrimitive
//           && type != typeof(string)
//           && type != typeof(decimal)
//           && type != typeof(DateTime)
//           && type != typeof(DateTimeOffset)
//           && type != typeof(Guid)
//           && !type.IsEnum;
//  }

//  private static object CreateValidationError(ActionExecutingContext context)
//  {
//    var errors = context.ModelState
//        .Where(e => e.Value?.Errors.Count > 0)
//        .ToDictionary(
//            kvp => kvp.Key,
//            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

//    return new
//    {
//      Message = "Validation failed",
//      ErrorCode = "VALIDATION_FAILED",
//      Errors = errors
//    };
//  }
//}