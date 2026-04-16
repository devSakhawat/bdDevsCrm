using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AccessRestrictionRepository : RepositoryBase<AccessRestriction>, IAccessRestrictionRepository
{
    public AccessRestrictionRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<AccessRestriction>> AccessRestrictionsAsync(int hrRecordId, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, false, cancellationToken);
    }

    public async Task<IEnumerable<Groups>> GroupsByHrRecordIdAsync(int hrRecordId, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(Enumerable.Empty<Groups>());
    }

    public async Task<IEnumerable<AccessRestriction>> AccessRestrictionsByHrRecordIdAsync(int hrRecordId, string groupCondition, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, false, cancellationToken);
    }

    public async Task<IEnumerable<AccessRestriction>> AccessRestrictionConditionsAsync(int hrRecordId, int type, string groupCondition, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, false, cancellationToken);
    }

    public async Task<string> GenerateAccessRestrictionConditionAsync(int hrRecordId, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(string.Empty);
    }

    public async Task<AccessRestriction> CreateAccessRestrictionAsync(AccessRestriction accessRestriction, CancellationToken cancellationToken = default)
    {
        await CreateAsync(accessRestriction, cancellationToken);
        return accessRestriction;
    }

    public void UpdateAccessRestriction(AccessRestriction accessRestriction)
    {
        Update(accessRestriction);
    }

    public async Task DeleteAccessRestrictionAsync(AccessRestriction accessRestriction, bool trackChanges, CancellationToken cancellationToken = default)
    {
        Delete(accessRestriction);
        await Task.CompletedTask;
    }
}
