using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IDocumentParameterMappingRepository : IRepositoryBase<DocumentParameterMapping>
{
    Task<DocumentParameterMapping?> DocumentParameterMappingAsync(int mappingId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameterMapping>> DocumentParameterMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
