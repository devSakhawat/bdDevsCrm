using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.HR;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;


public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
{
	public BranchRepository(CRMContext context) : base(context) { }
}