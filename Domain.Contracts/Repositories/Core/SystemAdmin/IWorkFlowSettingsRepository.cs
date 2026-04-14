// Interface: IWorkFlowSettingsRepository

// Interface: IWorkFlowSettingsRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IWorkFlowSettingsRepository : IRepositoryBase<WfState>
  {
    Task<IEnumerable<WfState>> WfStatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfState?> WfStateAsync(int wfStateId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfState> CreateWfStateAsync(WfState wfState, CancellationToken cancellationToken = default);
    void UpdateWfState(WfState wfState);
    Task DeleteWfStateAsync(WfState wfState, bool trackChanges, CancellationToken cancellationToken = default);
  }
}

