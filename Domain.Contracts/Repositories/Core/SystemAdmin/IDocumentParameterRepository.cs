using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IDocumentParameterRepository : IRepositoryBase<DocumentParameter>
{
    Task<DocumentParameter?> DocumentParameterAsync(int parameterId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameter>> DocumentParametersAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DocumentParameter>> ActiveDocumentParametersAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
