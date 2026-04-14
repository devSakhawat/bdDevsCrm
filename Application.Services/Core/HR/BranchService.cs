using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.HR;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.HR;


internal sealed class BranchService : IBranchService
{
  private const string SELECT_BRANCH_BY_COMPANYID = @"
SELECT Branch.* 
FROM Branch 
INNER JOIN CompanyLocationMap ON CompanyLocationMap.BranchId = Branch.BranchId 
WHERE CompanyId = {0}{1} 
ORDER BY BranchName ASC";

  private readonly IRepositoryManager _repository;
  private readonly ILogger<BranchService> _logger;
  private readonly IConfiguration _configuration;

  public BranchService(IRepositoryManager repository, ILogger<BranchService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }



  // get Branch types with id, name and code
  public async Task<IEnumerable<BranchDto>> BranchesByCompanyIdForCombo(int companyId, UsersDto user, bool trackChanges, CancellationToken cancellationToken = default)
  {

    if (companyId < 0 || companyId == null)
    {
      throw new BadRequestException("Invalid request!");
    }

    string query = string.Format(SELECT_BRANCH_BY_COMPANYID, companyId, "");
    IEnumerable<BranchDto> result = await _repository.Branches.AdoExecuteListQueryAsync<BranchDto>(query, parameters: null, cancellationToken);

    return result;
  }


}
