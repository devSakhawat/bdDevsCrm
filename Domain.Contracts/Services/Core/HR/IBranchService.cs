using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.HR;

public interface IBranchService
{
  // param because it should be branchId
  Task<IEnumerable<BranchDto>> BranchesByCompanyIdForCombo(int companyId, UsersDto user, bool trackChanges, CancellationToken cancellationToken = default);


}
