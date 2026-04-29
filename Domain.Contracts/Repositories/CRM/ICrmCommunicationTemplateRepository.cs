using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCommunicationTemplateRepository : IRepositoryBase<CrmCommunicationTemplate>
{
    Task<IEnumerable<CrmCommunicationTemplate>> CommunicationTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommunicationTemplate?> CommunicationTemplateAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommunicationTemplate>> CommunicationTemplatesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommunicationTemplate> CreateCrmCommunicationTemplateAsync(CrmCommunicationTemplate entity, CancellationToken cancellationToken = default);
    void UpdateCrmCommunicationTemplate(CrmCommunicationTemplate entity);
    Task DeleteCrmCommunicationTemplateAsync(CrmCommunicationTemplate entity, bool trackChanges, CancellationToken cancellationToken = default);
}
