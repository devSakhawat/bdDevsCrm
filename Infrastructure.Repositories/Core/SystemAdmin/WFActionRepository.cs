

// Class: WFActionRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin
{
	public class WFActionRepository : RepositoryBase<WfAction>, IWFActionRepository
	{
		public WFActionRepository(CrmContext context) : base(context) { }

		/// <summary>
		/// s all workflow actions.
		/// </summary>
		public async Task<IEnumerable<WfAction>> WfActionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
		{
			return await ListAsync(c => c.WfActionId, trackChanges, cancellationToken);
		}

		/// <summary>
		/// s a single workflow action by ID.
		/// </summary>
		public async Task<WfAction?> WfActionAsync(int actionId, bool trackChanges, CancellationToken cancellationToken = default)
		{
			return await FirstOrDefaultAsync(x => x.WfActionId.Equals(actionId), trackChanges, cancellationToken);
		}

		/// <summary>
		/// Creates a new workflow action.
		/// </summary>
		public async Task<WfAction> CreateWfActionAsync(WfAction action, CancellationToken cancellationToken = default)
		{
			int actionId = await CreateAndIdAsync(action, cancellationToken);
			action.WfActionId = actionId;
			return action;
		}

		/// <summary>
		/// Updates a workflow action.
		/// </summary>
		public void UpdateWfAction(WfAction action) => UpdateByState(action);

		/// <summary>
		/// Deletes a workflow action.
		/// </summary>
		public async Task DeleteWfActionAsync(WfAction action, bool trackChanges, CancellationToken cancellationToken = default)
			=> await DeleteAsync(x => x.WfActionId == action.WfActionId, trackChanges, cancellationToken);
	}
}



//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Core.SystemAdmin;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.Core.SystemAdmin;


//public class WFActionRepository : RepositoryBase<WfAction>, IWFActionRepository
//{
//  public WFActionRepository(CrmContext context) : base(context) { }


//}
