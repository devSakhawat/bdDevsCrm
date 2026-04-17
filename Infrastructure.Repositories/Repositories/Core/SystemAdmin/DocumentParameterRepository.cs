using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class DocumentParameterRepository : RepositoryBase<DocumentParameter>, IDocumentParameterRepository
{
    public DocumentParameterRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DocumentParameter?> DocumentParameterAsync(int parameterId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(d => d.ParameterId == parameterId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentParameter>> DocumentParametersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DocumentParameter>> ActiveDocumentParametersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await DocumentParametersAsync(trackChanges, cancellationToken);
    }
}
