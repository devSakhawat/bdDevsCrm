using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IDocumentTypeRepository : IRepositoryBase<DocumentType>
{
    Task<DocumentType?> DocumentTypeAsync(int documenttypeid, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentType>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentType>> ActiveDocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
