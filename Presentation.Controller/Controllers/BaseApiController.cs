using Presentation.AuthorizeAttributes;
using Presentation.Extensions;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Presentation.LinkFactories;

[Route(RouteConstants.BaseRoute)]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[EnableCors]
[AuthorizeUser]
public abstract class BaseApiController : ControllerBase
{
  protected readonly IServiceManager _serviceManager;

  public BaseApiController(IServiceManager serviceManager)
  {
    _serviceManager = serviceManager;
  }

  // ─── Identity ────────────────────────────────────────────────────────
  /// <summary>
  /// Gets the current authenticated user from HttpContext.
  /// </summary>
  protected UsersDto CurrentUser => HttpContext.CurrentUser()!;

  /// <summary>
  /// Gets the current user's ID from HttpContext.
  /// </summary>
  protected int CurrentUserId => HttpContext.UserId();

  // ─── CorrelationId ───────────────────────────────────────────────────
  /// <summary>
  /// Retrieves the Correlation ID set by CorrelationIdMiddleware from HttpContext.Items["CorrelationId"].
  /// This value is automatically included in all responses via this property.
  /// </summary>
  protected string? CorrelationId => HttpContext.Items["CorrelationId"]?.ToString();

  // ─── Success Response Helpers ─────────────────────────────────────────
  /// <summary>
  /// Returns a successful OK response with data and optional message.
  /// </summary>
  protected IActionResult OkResponse<T>(T data, string? message = null) =>
      Ok(ApiResponseHelper.Success(data, message, correlationId: CorrelationId));

  /// <summary>
  /// Returns a Created response with HATEOAS route links.
  /// </summary>
  protected IActionResult CreatedResponse<T>(T data, string? message = null) =>
      Ok(ApiResponseHelper.Created(data, message, correlationId: CorrelationId));

  protected IActionResult CreatedResponse<T>(
      string routeName,
      object routeValues,
      T data,
      string? message = null) =>
      CreatedAtRoute(
          routeName,
          routeValues,
          ApiResponseHelper.Created(data, message, correlationId: CorrelationId));

  /// <summary>
  /// Returns an Updated response with data and optional message.
  /// </summary>
  protected IActionResult UpdatedResponse<T>(T data, string? message = null) =>
      Ok(ApiResponseHelper.Updated(data, message, correlationId: CorrelationId));

  /// <summary>
  /// Returns a NoContent response with optional message.
  /// </summary>
  protected IActionResult NoContentResponse(string? message = null) =>
      Ok(ApiResponseHelper.NoContent<object>(message));

  // ─── Error Response Helpers ───────────────────────────────────────────
  /// <summary>
  /// Returns a NotFound response with error message.
  /// </summary>
  protected IActionResult NotFoundResponse(string message) =>
      NotFound(ApiResponseHelper.NotFound<object>(
          message, correlationId: CorrelationId));

  /// <summary>
  /// Returns a BadRequest response with error message.
  /// </summary>
  protected IActionResult BadRequestResponse(string message) =>
      BadRequest(ApiResponseHelper.BadRequest<object>(
          message, correlationId: CorrelationId));

  /// <summary>
  /// Returns a Conflict response with error message.
  /// </summary>
  protected IActionResult ConflictResponse(string message) =>
      Conflict(ApiResponseHelper.Conflict<object>(
          message, correlationId: CorrelationId));

  /// <summary>
  /// Handles ModelState validation errors manually.
  /// Required because SuppressModelStateInvalidFilter = true is set in Program.cs.
  /// </summary>
  protected IActionResult ValidationFailResponse(ModelStateDictionary modelState)
  {
    var errors = modelState
        .Where(x => x.Value?.Errors.Count > 0)
        .ToDictionary(
            k => k.Key,
            v => v.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
        );

    return UnprocessableEntity(ApiResponseHelper.ValidationError<object>(
        validationErrors: errors,
        correlationId: CorrelationId));
  }

  // ─── HATEOAS Helpers ──────────────────────────────────────────────────
  /// <summary>
  /// Wraps a single entity with HATEOAS links.
  /// </summary>
  protected LinkedResource<T> ToLinked<T>(
      T data,
      ILinkFactory<T> factory,
      int key) =>
      new LinkedResource<T>(data, factory.GenerateRowLinks(key));

  /// <summary>
  /// Converts a collection to HATEOAS-linked resources.
  /// </summary>
  protected List<LinkedResource<T>> ToLinkedList<T>(
      IEnumerable<T> dataList,
      ILinkFactory<T> factory,
      Func<T, int> keySelector) =>
      dataList.Select(item => ToLinked(item, factory, keySelector(item))).ToList();

  /// <summary>
  /// Converts a GridEntity to HATEOAS-linked grid.
  /// </summary>
  protected GridEntity<LinkedResource<T>> ToLinkedGrid<T>(
      GridEntity<T> gridEntity,
      ILinkFactory<T> factory,
      Func<T, int> keySelector) =>
      new GridEntity<LinkedResource<T>>
      {
        Items = ToLinkedList(gridEntity.Items, factory, keySelector),
        TotalCount = gridEntity.TotalCount,
        Columnses = gridEntity.Columnses
      };

  /// <summary>
  /// Creates a complete Grid response with HATEOAS links, pagination info,
  /// resource links, and CorrelationId envelope.
  /// Simplifies grid responses in controllers like MenuController.MenuSummary().
  /// </summary>
  protected IActionResult ToLinkedGridResponse<T>(
      GridEntity<T> gridEntity,
      ILinkFactory<T> factory,
      Func<T, int> keySelector,
      GridOptions options,
      string? message = null)
  {
    var linkedGrid = ToLinkedGrid(gridEntity, factory, keySelector);
    var resourceLinks = factory.GenerateResourceLinks();

    return Ok(ApiResponseHelper.SuccessGrid(
        data: linkedGrid,
        currentPage: options.page,
        pageSize: options.pageSize,
        totalCount: gridEntity.TotalCount,
        message: message ?? "Data retrieved successfully",
        resourceLinks: resourceLinks,
        correlationId: CorrelationId));
  }
}

  

//using Presentation.AuthorizeAttributes;
//using Presentation.Extensions;
//using Presentation.LinkFactories;
//using Domain.Contracts.Services;
//using bdDevs.Shared;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.Constants; // Add this using directive if IServiceManager depends on IMemoryCache directly
//using Application.Shared.Grid;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;


//[Route(RouteConstants.BaseRoute)]
//[ApiController]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//[EnableCors]
//[AuthorizeUser]
//// [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
//public class BaseApiController : ControllerBase
//{
//	protected readonly IServiceManager _serviceManager;

//	public BaseApiController(IServiceManager serviceManager)
//	{
//		_serviceManager = serviceManager;
//	}

//	/// <summary>
//	/// User from set nu AuthorizeUserAttribute set 
//	/// if there are [AuthorizeUser] attribute it should not be null because AuthorizeUserAttribute sets the user in HttpContext.
//	/// </summary>
//	protected UsersDto CurrentUser => HttpContext.CurrentUser()!;

//	/// <summary>
//	/// UserId from set nu AuthorizeUserAttribute set 
//	/// if there are [AuthorizeUser] attribute it should not be 0 because AuthorizeUserAttribute sets the user in HttpContext.
//	/// </summary>
//	protected int CurrentUserId => HttpContext.UserId();


//  // ─── NEW: HATEOAS helpers ────────────────────────────────────────────
//  /// <summary>
//  /// Wraps a single DTO with its row-level HATEOAS links.
//  /// Usage: ToLinked(menuDto, _linkFactory, menuDto.MenuId)
//  /// </summary>
//  protected LinkedResource<T> ToLinked<T>( T data, ILinkFactory<T> factory, int key)
//  {
//    return new LinkedResource<T>(data, factory.GenerateRowLinks(key));
//  }

//  /// <summary>
//  /// Wraps a list of DTOs — each item gets its own row-level links.
//  /// Usage: ToLinkedList(menuDtos, _linkFactory, x => x.MenuId)
//  /// </summary>
//  protected List<LinkedResource<T>> ToLinkedList<T>( IEnumerable<T> dataList, ILinkFactory<T> factory, Func<T, int> keySelector)
//  {
//    return dataList.Select(item => ToLinked(item, factory, keySelector(item))).ToList();
//  }

//  /// <summary>
//  /// Builds GridEntity with LinkedResource items.
//  /// Preserves TotalCount for Kendo schema.total
//  /// Usage: ToLinkedGrid(gridEntity, _linkFactory, x => x.MenuId)
//  /// </summary>
//  protected GridEntity<LinkedResource<T>> ToLinkedGrid<T>(GridEntity<T> gridEntity, ILinkFactory<T> factory, Func<T, int> keySelector)
//  {
//    return new GridEntity<LinkedResource<T>>
//    {
//      Items = ToLinkedList(gridEntity.Items, factory, keySelector),
//      TotalCount = gridEntity.TotalCount,
//      Columnses = gridEntity.Columnses
//    };
//  }










  //// Centralized method to get authenticated user
  //protected UsersDto AuthenticatedUser()
  //{
  //    var userIdClaim = User.FindFirst("UserId")?.Value;

  //    if (string.IsNullOrEmpty(userIdClaim))
  //        throw new GenericUnauthorizedException("User authentication required.");

  //    if (!int.TryParse(userIdClaim, out int userId))
  //        throw new GenericBadRequestException("Invalid user ID format.");

  //    UsersDto currentUser = _serviceManager.Cache<UsersDto>(userId);

  //    if (currentUser == null)
  //        throw new GenericUnauthorizedException("User session expired.");

  //    return currentUser;
  //}

  //// OPTIONAL:  User ID only (without full user object)
  //protected int AuthenticatedUserId()
  //{
  //    var userIdClaim = User.FindFirst("UserId")?.Value;

  //    if (string.IsNullOrEmpty(userIdClaim))
  //        throw new GenericUnauthorizedException("User authentication required.");

  //    if (!int.TryParse(userIdClaim, out int userId))
  //        throw new GenericBadRequestException("Invalid user ID format.");

  //    return userId;
  //}

  //// OPTIONAL: Try-pattern for scenarios where user might not exist
  //protected bool TryAuthenticatedUser(out UsersDto? currentUser)
  //{
  //    currentUser = null;

  //    var userIdClaim = User.FindFirst("UserId")?.Value;
  //    if (string.IsNullOrEmpty(userIdClaim))
  //        return false;

  //    if (!int.TryParse(userIdClaim, out int userId))
  //        return false;

  //    currentUser = _serviceManager.Cache<UsersDto>(userId);
  //    return currentUser != null;
  //}
//}