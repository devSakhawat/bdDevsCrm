using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IDocumentTemplateRepository : IRepositoryBase<DocumentTemplate>
{
    Task<DocumentTemplate?> DocumentTemplateAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentTemplate>> DocumentTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
