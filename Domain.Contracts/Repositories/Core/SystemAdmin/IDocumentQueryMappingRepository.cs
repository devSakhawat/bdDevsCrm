using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IDocumentQueryMappingRepository : IRepositoryBase<DocumentQueryMapping>
{
    Task<DocumentQueryMapping?> DocumentQueryMappingAsync(int documentQueryId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentQueryMapping>> DocumentQueryMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentQueryMapping>> ActiveDocumentQueryMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
