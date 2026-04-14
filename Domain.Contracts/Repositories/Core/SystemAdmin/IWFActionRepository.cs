// Interface: IWFActionRepository

// Interface: IWFActionRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IWFActionRepository : IRepositoryBase<WfAction>
  {
    Task<IEnumerable<WfAction>> WfActionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfAction?> WfActionAsync(int actionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfAction> CreateWfActionAsync(WfAction action, CancellationToken cancellationToken = default);
    void UpdateWfAction(WfAction action);
    Task DeleteWfActionAsync(WfAction action, bool trackChanges, CancellationToken cancellationToken = default);
  }
}


//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Repositories;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface IWFActionRepository : IRepositoryBase<WfAction>
//{

//}
