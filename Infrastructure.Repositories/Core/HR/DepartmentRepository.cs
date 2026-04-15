using Domain.Entities.Entities;
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.HR;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.HR;


public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
{
  public DepartmentRepository(CrmContext context) : base(context) { }
}