using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class DocumentParameterMappingRepository : RepositoryBase<DocumentParameterMapping>, IDocumentParameterMappingRepository
{
    public DocumentParameterMappingRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DocumentParameterMapping?> DocumentParameterMappingAsync(int mappingId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(d => d.MappingId == mappingId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentParameterMapping>> DocumentParameterMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentParameterMapping>> ActiveDocumentParameterMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(d => d.IsVisible == true, orderBy: null, trackChanges: trackChanges, descending: false, cancellationToken: cancellationToken);
    }
}
