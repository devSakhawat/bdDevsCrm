using Domain.Entities.Entities.System;

using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.s;
using bdDevCRM.s.Core.HR;
using bdDevCRM.ServiceContract.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Shared.Exceptions;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.HR;


internal sealed class DepartmentService : IDepartmentService
{
  private const string SELECT_DEPARTMENT_BY_COMPANYID =
            "Select Department.* from Department inner join CompanyDepartmentMap on CompanyDepartmentMap.DepartmentId =Department.DepartmentId  where CompanyId = {0} and IsActive=1 {1} order by DepartmentName asc";

  private readonly IRepositoryManager _repository;
  private readonly ILogger<DepartmentService> _logger;
  private readonly IConfiguration _configuration;

  public DepartmentService(IRepositoryManager repository, ILogger<DepartmentService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }



  // get Department types with id, name and code
  public async Task<IEnumerable<DepartmentDto>> DepartmentesByCompanyIdForCombo(int companyId, UsersDto user, bool trackChanges, CancellationToken cancellationToken = default)
  {

    if (companyId < 0 || companyId == null)
    {
      throw new BadRequestException("Invalid request!");
    }

    string query = string.Format(SELECT_DEPARTMENT_BY_COMPANYID, companyId, "");
    IEnumerable<DepartmentDto> result = await _repository.departments.AdoExecuteListQueryAsync<DepartmentDto>(query, parameters: null, cancellationToken: cancellationToken);

    return result;
  }


}
