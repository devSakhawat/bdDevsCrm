// Interface: IGroupPermissionRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IGroupPermissionRepository : IRepositoryBase<GroupPermission>
  {
    Task<IEnumerable<GroupPermission>> AccessPermissionsForCurrentUserAsync(int moduleId, int userId, CancellationToken cancellationToken = default);
  }
}


//using Domain.Entities.Entities.System;
//using Domain.Contracts.Repositories;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface IGroupPermissionRepository : IRepositoryBase<GroupPermission>
//{
//  Task<IEnumerable<GroupPermission>> AccessPermisionForCurrentUser(int moduleId, int userId);
//}
