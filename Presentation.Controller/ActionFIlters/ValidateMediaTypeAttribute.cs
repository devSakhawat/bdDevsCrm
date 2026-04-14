using bdDevs.Shared.ErrorCodes;
using Domain.Exceptions;
using Domain.Exceptions.Base;
using Domain.Exceptions.SpecialCases;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;

namespace Presentation.ActionFilters;

public class ValidateMediaTypeAttribute : IActionFilter
{
  private static readonly HashSet<string> SupportedMediaTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "application/json"
        // future: "application/xml"
    };

  public void OnActionExecuting(ActionExecutingContext context)
  {
    // Ignore attribute check
    if (context.ActionDescriptor.EndpointMetadata.Any(x => x is IgnoreMediaTypeValidationAttribute))
    {
      return;
    }

    var acceptHeader = context.HttpContext.Request.Headers.Accept.FirstOrDefault();

    // Missing Accept Header
    if (string.IsNullOrWhiteSpace(acceptHeader))
    {
      throw new BadRequestException(
          "Accept header is missing.",
          ErrorCodes.MissingAcceptHeader);
    }

    // Invalid Media Type Format
    if (!MediaTypeHeaderValue.TryParse(acceptHeader, out var mediaType))
    {
      throw new BadRequestException(
          "Invalid media type in Accept header.",
          ErrorCodes.InvalidMediaType);
    }

    // Unsupported Media Type
    //if (!SupportedMediaTypes.Contains(mediaType.MediaType))
    //{
    //  throw new AppException($"Media type '{mediaType.MediaType}' is not supported.", 406, ErrorCodes.UnsupportedMediaType);
    //}

    // Unsupported Media Type
    if (!SupportedMediaTypes.Contains(mediaType.MediaType))
    {
      throw new NotAcceptableException($"Media type '{mediaType.MediaType}' is not supported.", ErrorCodes.UnsupportedMediaType);
    }

    // Store for later use (e.g., formatter, versioning)
    context.HttpContext.Items["AcceptHeaderMediaType"] = mediaType.MediaType;
  }

  public void OnActionExecuted(ActionExecutedContext context) { }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class IgnoreMediaTypeValidationAttribute : Attribute { }


//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http.Headers;

//namespace Presentation.ActionFIlters;

//public class ValidateMediaTypeAttribute : IActionFilter
//{
//  public void OnActionExecuting(ActionExecutingContext context)
//  {
//    // IgnoreMediaTypeValidation attribute check
//    bool hasIgnoreAttribute = context.ActionDescriptor.EndpointMetadata.Any(meta => meta is IgnoreMediaTypeValidationAttribute);

//    if (hasIgnoreAttribute) return;

//    var acceptHeader = context.HttpContext.Request.Headers.Accept.FirstOrDefault();

//    if (string.IsNullOrWhiteSpace(acceptHeader))
//    {
//      context.Result = new BadRequestObjectResult(new
//      {
//        Message = "Accept header is missing.",
//        ErrorCode = "MISSING_ACCEPT_HEADER"
//      });
//      return;
//    }

//    if (!MediaTypeHeaderValue.TryParse(acceptHeader, out var outMediaType))
//    {
//      context.Result = new BadRequestObjectResult(new
//      {
//        Message = "Invalid media type in Accept header.",
//        ErrorCode = "INVALID_MEDIA_TYPE"
//      });
//      return;
//    }

//    context.HttpContext.Items["AcceptHeaderMediaType"] = outMediaType;
//  }

//  public void OnActionExecuted(ActionExecutedContext context) { }
//}

//[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//public class IgnoreMediaTypeValidationAttribute : Attribute { }









////using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Mvc.Filters;
////using System.Net.Http.Headers;

////namespace Presentation.ActionFIlters;

////public class ValidateMediaTypeAttribute : IActionFilter
////{
////  public void OnActionExecuting(ActionExecutingContext context)
////  {
////    // IgnoreMediaTypeValidation attribute check
////    var endpoint = context.HttpContext.GetEndpoint();
////    var hasIgnoreAttribute = endpoint?.Metadata.GetMetadata<IgnoreMediaTypeValidationAttribute>() != null;

////    if (hasIgnoreAttribute) return;

////    var acceptHeader = context.HttpContext.Request.Headers.Accept.FirstOrDefault();

////    if (string.IsNullOrWhiteSpace(acceptHeader))
////    {
////      context.Result = new BadRequestObjectResult(new
////      {
////        Message = "Accept header is missing.",
////        ErrorCode = "MISSING_ACCEPT_HEADER"
////      });
////      return;
////    }

////    if (!MediaTypeHeaderValue.TryParse(acceptHeader, out var outMediaType))
////    {
////      context.Result = new BadRequestObjectResult(new
////      {
////        Message = "Invalid media type in Accept header.",
////        ErrorCode = "INVALID_MEDIA_TYPE"
////      });
////      return;
////    }

////    context.HttpContext.Items["AcceptHeaderMediaType"] = outMediaType;
////  }

////  public void OnActionExecuted(ActionExecutedContext context) { }
////}

////[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
////public class IgnoreMediaTypeValidationAttribute : Attribute { }