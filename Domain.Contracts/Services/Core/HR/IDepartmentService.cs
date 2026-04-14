using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.HR;

public interface IDepartmentService
{
  Task<IEnumerable<DepartmentDto>> DepartmentesByCompanyIdForCombo(int companyId, UsersDto user, bool trackChanges, CancellationToken cancellationToken = default);


}
