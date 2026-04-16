using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class DocumentTypeRepository : RepositoryBase<DocumentType>, IDocumentTypeRepository
{
    public DocumentTypeRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DocumentType?> DocumentTypeAsync(int documenttypeid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(d => d.Documenttypeid == documenttypeid, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentType>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
