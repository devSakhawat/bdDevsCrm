using System.Text;

namespace bdDevsCrm.Api.Middleware.Shared;

public static class RequestBodyReader
{
  public static async Task<string?> ReadOnceAsync(
      HttpContext context, int maxBytes = 4096)
  {
    var pipelineCtx = RequestPipelineContext.OrCreate(context);

    if (pipelineCtx.IsRequestBodyCaptured)
      return pipelineCtx.CachedRequestBody;

    var request = context.Request;

    if (request.ContentLength is null or 0 ||
        request.ContentLength > maxBytes ||
        !IsJsonContentType(request.ContentType))
    {
      pipelineCtx.IsRequestBodyCaptured = true;
      pipelineCtx.CachedRequestBody = null;
      return null;
    }

    try
    {
      request.EnableBuffering();

      using var reader = new StreamReader(
          request.Body,
          Encoding.UTF8,
          detectEncodingFromByteOrderMarks: false,
          bufferSize: 4096,
          leaveOpen: true);

      var body = await reader.ReadToEndAsync();
      request.Body.Position = 0;

      pipelineCtx.IsRequestBodyCaptured = true;
      pipelineCtx.CachedRequestBody =
          string.IsNullOrEmpty(body) ? null : body;

      return pipelineCtx.CachedRequestBody;
    }
    catch
    {
      pipelineCtx.IsRequestBodyCaptured = true;
      pipelineCtx.CachedRequestBody = null;
      return null;
    }
  }

  private static bool IsJsonContentType(string? contentType)
  {
    if (string.IsNullOrEmpty(contentType)) return false;
    return contentType.Contains(
        "application/json",
        StringComparison.OrdinalIgnoreCase);
  }
}










//using System.Text;

//namespace bdDevCRM.Api.Middleware.Shared;

///// <summary>
///// Reads the request body once and caches it in PipelineContext.
///// Subsequent middleware will use the cached version.
///// 
///// Conflict #3 resolved: Read three times → Read once
///// </summary>
//public static class RequestBodyReader
//{
//  /// <summary>
//  /// Reads and returns the request body.
//  /// If already read, returns the cached version.
//  /// </summary>
//  /// <param name="context">HttpContext</param>
//  /// <param name="maxBytes">Maximum bytes to read (default 4KB)</param>
//  /// <returns>Body string or null</returns>
//  public static async Task<string?> ReadOnceAsync(HttpContext context, int maxBytes = 4096)
//  {
//    var pipelineCtx = RequestPipelineContext.OrCreate(context);

//    // If already read, return cached version
//    if (pipelineCtx.IsRequestBodyCaptured)
//    {
//      return pipelineCtx.CachedRequestBody;
//    }

//    var request = context.Request;

//    // Check conditions for reading body
//    if (request.ContentLength is null or 0 ||           // no body
//        request.ContentLength > maxBytes ||              // body too large
//        !IsJsonContentType(request.ContentType))         // not JSON
//    {
//      pipelineCtx.IsRequestBodyCaptured = true;
//      pipelineCtx.CachedRequestBody = null;
//      return null;
//    }

//    try
//    {
//      // EnableBuffering — so body can be read multiple times
//      request.EnableBuffering();

//      using var reader = new StreamReader(
//          request.Body,
//          Encoding.UTF8,
//          detectEncodingFromByteOrderMarks: false,
//          bufferSize: 4096,
//          leaveOpen: true);  // ← Important: do not close the stream

//      var body = await reader.ReadToEndAsync();

//      // Reset body stream position to start
//      // so subsequent middleware/controller can read it again
//      request.Body.Position = 0;

//      // Cache the body
//      pipelineCtx.IsRequestBodyCaptured = true;
//      pipelineCtx.CachedRequestBody = string.IsNullOrEmpty(body) ? null : body;

//      return pipelineCtx.CachedRequestBody;
//    }
//    catch
//    {
//      pipelineCtx.IsRequestBodyCaptured = true;
//      pipelineCtx.CachedRequestBody = null;
//      return null;
//    }
//  }

//  private static bool IsJsonContentType(string? contentType)
//  {
//    if (string.IsNullOrEmpty(contentType)) return false;
//    return contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase);
//  }
//}