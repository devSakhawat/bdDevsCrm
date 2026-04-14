using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.HR;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;


public class EmployeeTypeRepository : RepositoryBase<Employeetype>, IEmployeeTypeRepository
{
	public EmployeeTypeRepository(CRMContext context) : base(context) { }
}