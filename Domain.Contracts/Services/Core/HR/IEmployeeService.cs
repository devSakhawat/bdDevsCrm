using bdDevs.Shared.DataTransferObjects.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.HR;

public interface IEmployeeService
{
  Task<EmploymentDto> EmploymentByHrRecordId(int hrRecordId, CancellationToken cancellationToken = default);
  Task<WfStateDto> EmployeeCurrentStatusByHrRecordId(int hrRecordId, CancellationToken cancellationToken = default);
  Task<EmployeeDto> EmployeeByHrRecordId(int hrRecordId, CancellationToken cancellationToken = default);
  // 
  Task<IEnumerable<EmployeeTypeDto>> EmployeeTypes(int param, CancellationToken cancellationToken = default);

  Task<IEnumerable<EmployeesByCompanyBranchDepartmentDto>> EmployeeByCompanyIdAndBranchIdAndDepartmentId(int companyId, int branchId, int departmentId, CancellationToken cancellationToken = default);
}
