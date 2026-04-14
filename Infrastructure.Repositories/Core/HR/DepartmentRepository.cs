using bdDevCRM.Entities.Entities;
using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.HR;
using bdDevCRM.RepositoriesContracts.Core.SystemAdmin;
using bdDevCRM.s;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;


public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
  public DepartmentRepository(CRMContext context) : base(context) { }
}