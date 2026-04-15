using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Presentation.Controllers.Core.HR;

public class EmployeeController : BaseApiController
{
  //private readonly IServiceManager _serviceManager;
  private readonly IMemoryCache _cache;
  public EmployeeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
  {
    //_serviceManager = serviceManager;
    _cache = cache;
  }

  [HttpGet(RouteConstants.EmployeeTypes)]
  public async Task<IActionResult> EmployeeTypes()
  {
    // from claim.
    var userId = Convert.ToInt32(User.FindFirst("UserId")?.Value);
    // userId : which key is reponsible to when cache was created .
    // get user from cache. if cache is not founded by key then it will thow Unauthorized exception with 401 status code.
    UsersDto user = _serviceManager.Cache<UsersDto>(userId);

    IEnumerable<EmployeeTypeDto> employees = await _serviceManager.Employees.EmployeeTypes((int)user.AccessParentCompany);
    return Ok(employees);
  }

  [HttpGet(RouteConstants.EmployeesByCompanyIdAndBranchIdAndDepartmentId)]
  public async Task<IActionResult> EmployeeByCompanyIdAndBranchIdAndDepartmentId(int companyid, int branchId, int departmentId)
  {
    //int userId = HttpContext.UserId();
    //var currentUser = HttpContext.CurrentUser();

    var userIdClaim = User.FindFirst("UserId")?.Value;
    if (string.IsNullOrEmpty(userIdClaim))
      return Unauthorized("Unauthorized attempt to get data!");

    int userId = Convert.ToInt32(userIdClaim);
    UsersDto currentUser = _serviceManager.Cache<UsersDto>(userId);
    if (currentUser == null) return Unauthorized("User not found in cache.");

    IEnumerable<EmployeesByCompanyBranchDepartmentDto> res = await _serviceManager.Employees.EmployeeByCompanyIdAndBranchIdAndDepartmentId(companyid, branchId, departmentId);

    if (res == null || !res.Any())
      return Ok(ApiResponseHelper.NoContent<IEnumerable<EmployeesByCompanyBranchDepartmentDto>>("No Data found"));

    return Ok(ApiResponseHelper.Success(res, "Data retrieved successfully"));
  }




}
