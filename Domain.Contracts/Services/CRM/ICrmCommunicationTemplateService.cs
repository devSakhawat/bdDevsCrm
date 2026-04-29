using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCommunicationTemplateService
{
    Task<CrmCommunicationTemplateDto> CreateAsync(CreateCrmCommunicationTemplateRecord record, CancellationToken cancellationToken = default);
    Task<CrmCommunicationTemplateDto> UpdateAsync(UpdateCrmCommunicationTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCommunicationTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommunicationTemplateDto> CommunicationTemplateAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommunicationTemplateDto>> CommunicationTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommunicationTemplateDDLDto>> CommunicationTemplateForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCommunicationTemplateDto>> CommunicationTemplateSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
