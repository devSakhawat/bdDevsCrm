//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Core.SystemAdmin;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.Core.SystemAdmin;


//public class WorkFlowSettingsRepository : RepositoryBase<WfState>, IWorkFlowSettingsRepository
//{
//  public WorkFlowSettingsRepository(CRMContext context) : base(context) { }


//}

// Class: WorkFlowSettingsRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin
{
	public class WorkFlowSettingsRepository : RepositoryBase<WfState>, IWorkFlowSettingsRepository
	{
		public WorkFlowSettingsRepository(CRMContext context) : base(context) { }

		/// <summary>
		/// All workflow states.
		/// </summary>
		public async Task<IEnumerable<WfState>> WfStatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
		{
			return await ListAsync(c => c.WfStateId, trackChanges, cancellationToken);
		}

		/// <summary>
		/// Single workflow state by ID.
		/// </summary>
		public async Task<WfState?> WfStateAsync(int wfStateId, bool trackChanges, CancellationToken cancellationToken = default)
		{
			return await FirstOrDefaultAsync(x => x.WfStateId.Equals(wfStateId), trackChanges, cancellationToken);
		}

		/// <summary>
		/// Creates a new workflow state.
		/// </summary>
		public async Task<WfState> CreateWfStateAsync(WfState wfState, CancellationToken cancellationToken = default)
		{
			int wfStateId = await CreateAndIdAsync(wfState, cancellationToken);
			wfState.WfStateId = wfStateId;
			return wfState;
		}

		/// <summary>
		/// Updates a workflow state.
		/// </summary>
		public void UpdateWfState(WfState wfState) => UpdateByState(wfState);

		/// <summary>
		/// Deletes a workflow state.
		/// </summary>
		public Task DeleteWfStateAsync(WfState wfState, bool trackChanges, CancellationToken cancellationToken = default)
			=> DeleteAsync(x => x.WfStateId.Equals(wfState.WfStateId), trackChanges, cancellationToken);
	}
}
