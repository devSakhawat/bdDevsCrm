using Domain.Entities.Entities.System;
using Domain.Contracts.Core.HR;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;


public class EmployeeTypeRepository : RepositoryBase<Employeetype>, IEmployeeTypeRepository
{
	public EmployeeTypeRepository(CrmContext context) : base(context) { }
}