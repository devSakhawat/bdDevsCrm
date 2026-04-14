using bdDevs.Shared.DataTransferObjects.Core.HR;
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.HR;

public interface IEmployeeRepository : IRepositoryBase<Employee>
{
  Task<Employment> EmploymentByHrRecordId(int hrRecordId, CancellationToken cancellationToken);
  Task<WfState> EmployeeCurrentStatusByHrRecordId(int hrRecordId, CancellationToken cancellationToken);
  Task<Employee> EmployeeByHrRecordId(int hrRecordId, CancellationToken cancellationToken);
  Task<IEnumerable<EmployeesByCompanyBranchDepartmentDto>> EmployeeByCompanyIdAndBranchIdAndDepartmentId(string condition, CancellationToken cancellationToken);
}
