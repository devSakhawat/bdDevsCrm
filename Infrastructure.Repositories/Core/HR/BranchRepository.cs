using Domain.Entities.Entities.System;
using Domain.Contracts.Core.HR;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;


public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
{
	public BranchRepository(CRMContext context) : base(context) { }
}