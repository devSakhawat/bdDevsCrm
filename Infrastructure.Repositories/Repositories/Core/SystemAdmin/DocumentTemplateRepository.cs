using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class DocumentTemplateRepository : RepositoryBase<DocumentTemplate>, IDocumentTemplateRepository
{
    public DocumentTemplateRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DocumentTemplate?> DocumentTemplateAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(d => d.DocumentId == documentId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentTemplate>> DocumentTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentTemplate>> ActiveDocumentTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await DocumentTemplatesAsync(trackChanges, cancellationToken);
    }
}
