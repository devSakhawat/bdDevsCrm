using bdDevCRM.ServicesContract;
using bdDevCRM.Shared.ApiResponse;
using bdDevCRM.Shared.DataTransferObjects.Core.HR;
using bdDevCRM.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Presentation.Controllers.Core.HR;

public class DepartmentController : BaseApiController
{
  //private readonly IServiceManager _serviceManager;
  private readonly IMemoryCache _cache;
  public DepartmentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    //_serviceManager = serviceManager;
    _cache = cache;
  }

  [HttpGet(RouteConstants.DepartmentByCompanyIdForCombo)]
  [AllowAnonymous]
  public async Task<IActionResult> DepartmentByCompanyIdForCombo([FromQuery] int companyId, CancellationToken cancellationToken = default)
  {
    //int userId = HttpContext.UserId();
    //var currentUser = HttpContext.CurrentUser();

    var userIdClaim = User.FindFirst("UserId")?.Value;
    if (string.IsNullOrEmpty(userIdClaim))
      return Unauthorized("Unauthorized attempt to get data!");

    int userId = Convert.ToInt32(userIdClaim);
    UsersDto currentUser = _serviceManager.Cache<UsersDto>(userId);
    if (currentUser == null) return Unauthorized("User not found in cache.");

    IEnumerable<DepartmentDto> res = await _serviceManager.departments.DepartmentesByCompanyIdForCombo(companyId, currentUser, trackChanges: false, cancellationToken);
    //return Ok(branchList);

    if (res == null || !res.Any())
      return Ok(ApiResponseHelper.NoContent<IEnumerable<DepartmentDto>>("No Data found"));

    return Ok(ApiResponseHelper.Success(res, "Data retrieved successfully"));
  }




}
