using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCommunicationTemplateRepository : RepositoryBase<CrmCommunicationTemplate>, ICrmCommunicationTemplateRepository
{
    public CrmCommunicationTemplateRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCommunicationTemplate>> CommunicationTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.CommunicationTemplateId, trackChanges, cancellationToken);
    }

    public async Task<CrmCommunicationTemplate?> CommunicationTemplateAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(x => x.CommunicationTemplateId == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CrmCommunicationTemplate>> CommunicationTemplatesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.CommunicationTemplateId), trackChanges, cancellationToken);
    }

    public async Task<CrmCommunicationTemplate> CreateCrmCommunicationTemplateAsync(CrmCommunicationTemplate entity, CancellationToken cancellationToken = default)
    {
        var newId = await CreateAndIdAsync(entity, cancellationToken);
        entity.CommunicationTemplateId = newId;
        return entity;
    }

    public void UpdateCrmCommunicationTemplate(CrmCommunicationTemplate entity) => UpdateByState(entity);

    public async Task DeleteCrmCommunicationTemplateAsync(CrmCommunicationTemplate entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.CommunicationTemplateId == entity.CommunicationTemplateId, trackChanges, cancellationToken);
    }
}
