using Domain.Entities.Entities.System;
using Domain.Contracts.Core.HR;
using bdDevs.Shared.DataTransferObjects.Core.HR;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
	public EmployeeRepository(CRMContext context) : base(context) { }


	private const string SELECT_EMPLOYEE_BY_HRRECORDID =
			"Select * from Employee where HRRecordId = {0} order by FullName";

	private const string SELECT_EMPLOYEE_BY_CompanyBranchDepartment_SQL =
					 @"
Select * from (
	Select Employee.HRRecordId,EmployeeId,FullName,CompanyId,ReportTo,DepartmentId,BranchId  
	from Employee 
	inner join Employment ON Employment.HRRecordId = Employee.HRRecordId
) as tbl 
{0} 
order by FullName";

	public async Task<Employment> EmploymentByHrRecordId(int hrRecordId, CancellationToken cancellationToken)
	{
		string employmentQuery = string.Format(@"SELECT EMPLOYEETYPE.IsContract, EMPLOYEETYPE.IsProbationary, Employment.HRRecordId
, EmployeeId, EmployeeType, DESIGNATIONID, StartDate, EmploymentDate, CompanyId, DepartmentId, ReportTo, TelephoneExtension
, OfficialEmail, EmergencyContactName, EmergencyContactNo, PossibleConfirmationDate, Duties, AttendanceCardNo, UserId
, Employment.LastUpdatedDate, BankBranchId, BankAccountNo, BRANCHID, GPFNO, JobEndDate, JOININGPOST, EXPERIENCE, REPORTDEPID
, Func_Id, ContractEndDate, JobEndTypeId, GradeId, TinNumber, PostingType, IsOTEligible, DivisionId,FacilityId, SectionId
, Approver, ApproverDepartmentId, SalaryLocation 
FROM Employment 
INNER JOIN EMPLOYEETYPE ON EMPLOYEETYPE.EMPLOYEETYPEID = Employment.EmployeeType 
      WHERE Employment.HRRecordId = {0}", hrRecordId);

		// Call the generic method with the hardcoded query
		var result = await AdoExecuteSingleDataAsync<Employment>(employmentQuery, cancellationToken: cancellationToken);
		return result;
	}

	public async Task<WfState> EmployeeCurrentStatusByHrRecordId(int hrRecordId, CancellationToken cancellationToken)
	{
		string sql = string.Format(@"Select case when (WFState.IsClosed =3 and Employment.JobEndDate<'{1}') then WFState.StateName else '' end as StateName ,Employment.JobEndDate,Date() as CurrentDate  
from Employee 
inner join Employment on Employment.HRRecordId=Employee.HRRecordId 
left outer join WFState on WFState.WfStateId=Employee.StateId where Employment.HRRecordId={0} ", hrRecordId, DateTime.Today.ToString("MM-dd-yyyy"));

		var data = await AdoExecuteSingleDataAsync<WfState>(sql, cancellationToken: cancellationToken);
		return data;
	}

	public async Task<Employee> EmployeeByHrRecordId(int hrRecordId, CancellationToken cancellationToken)
	{
		string quary = string.Format(SELECT_EMPLOYEE_BY_HRRECORDID, hrRecordId);

		var data = await AdoExecuteSingleDataAsync<Employee>(quary, cancellationToken: cancellationToken);
		return data;
	}

	public async Task<IEnumerable<EmployeesByCompanyBranchDepartmentRepositoroyDto>> EmployeeByCompanyIdAndBranchIdAndDepartmentId(string condition, CancellationToken cancellationToken)
	{
		string sql = string.Format(SELECT_EMPLOYEE_BY_CompanyBranchDepartment_SQL, condition);
		IEnumerable<EmployeesByCompanyBranchDepartmentRepositoroyDto> returnList = await AdoExecuteListQueryAsync<EmployeesByCompanyBranchDepartmentRepositoroyDto>(sql, cancellationToken: cancellationToken);
		return returnList;
	}

}
