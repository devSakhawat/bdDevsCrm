// Interface: IGroupMemberRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IGroupMemberRepository : IRepositoryBase<GroupMember>
  {
    Task<IEnumerable<GroupMember>> GroupMembersByUserIdAsync(int userId, CancellationToken cancellationToken = default);
  }
}




//using Domain.Entities.Entities.System;
//using Domain.Contracts.Repositories;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface IGroupMemberRepository : IRepositoryBase<GroupMember>
//{
//  Task<IEnumerable<GroupMember>> GroupMemberByUserId(int userId, bool trackChanges);
//}
