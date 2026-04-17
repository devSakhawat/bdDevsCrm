using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class DocumentQueryMappingRepository : RepositoryBase<DocumentQueryMapping>, IDocumentQueryMappingRepository
{
    public DocumentQueryMappingRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DocumentQueryMapping?> DocumentQueryMappingAsync(int documentQueryId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(d => d.DocumentQueryId == documentQueryId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentQueryMapping>> DocumentQueryMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentQueryMapping>> ActiveDocumentQueryMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await DocumentQueryMappingsAsync(trackChanges, cancellationToken);
    }
}
