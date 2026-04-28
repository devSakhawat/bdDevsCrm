using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmCommunicationTemplateService : ICrmCommunicationTemplateService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCommunicationTemplateService> _logger;
    public CrmCommunicationTemplateService(IRepositoryManager repository, ILogger<CrmCommunicationTemplateService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CrmCommunicationTemplateDto> CreateAsync(CreateCrmCommunicationTemplateRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmCommunicationTemplateRecord));

        bool exists = await _repository.CrmCommunicationTemplates.ExistsAsync(x => x.TemplateName.Trim().ToLower() == record.TemplateName.Trim().ToLower() && x.CommunicationTypeId == record.CommunicationTypeId, cancellationToken: cancellationToken);
        if (exists)
            throw new ConflictException("Communication Template with this value already exists!");

        CrmCommunicationTemplate entity = record.MapTo<CrmCommunicationTemplate>();
        int newId = await _repository.CrmCommunicationTemplates.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Communication Template created successfully. ID: {CommunicationTemplateId}", newId);

        return entity.MapTo<CrmCommunicationTemplateDto>() with { CommunicationTemplateId = newId };
    }

    public async Task<CrmCommunicationTemplateDto> UpdateAsync(UpdateCrmCommunicationTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmCommunicationTemplateRecord));

        _ = await _repository.CrmCommunicationTemplates.FirstOrDefaultAsync(x => x.CommunicationTemplateId == record.CommunicationTemplateId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Communication Template", "CommunicationTemplateId", record.CommunicationTemplateId.ToString());

        bool duplicateExists = await _repository.CrmCommunicationTemplates.ExistsAsync(x => x.TemplateName.Trim().ToLower() == record.TemplateName.Trim().ToLower() && x.CommunicationTypeId == record.CommunicationTypeId && x.CommunicationTemplateId != record.CommunicationTemplateId, cancellationToken: cancellationToken);
        if (duplicateExists)
            throw new ConflictException("Communication Template with this value already exists!");

        CrmCommunicationTemplate entity = record.MapTo<CrmCommunicationTemplate>();
        _repository.CrmCommunicationTemplates.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Communication Template updated successfully. ID: {CommunicationTemplateId}", record.CommunicationTemplateId);

        return entity.MapTo<CrmCommunicationTemplateDto>();
    }

    public async Task DeleteAsync(DeleteCrmCommunicationTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CommunicationTemplateId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var entity = await _repository.CrmCommunicationTemplates.FirstOrDefaultAsync(x => x.CommunicationTemplateId == record.CommunicationTemplateId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Communication Template", "CommunicationTemplateId", record.CommunicationTemplateId.ToString());

        await _repository.CrmCommunicationTemplates.DeleteAsync(x => x.CommunicationTemplateId == record.CommunicationTemplateId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Communication Template deleted successfully. ID: {CommunicationTemplateId}", record.CommunicationTemplateId);
    }

    public async Task<CrmCommunicationTemplateDto> CommunicationTemplateAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmCommunicationTemplates.FirstOrDefaultAsync(x => x.CommunicationTemplateId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Communication Template", "CommunicationTemplateId", id.ToString());

        return entity.MapTo<CrmCommunicationTemplateDto>();
    }

    public async Task<IEnumerable<CrmCommunicationTemplateDto>> CommunicationTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommunicationTemplates.CommunicationTemplatesAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Communication Template records found.");
            return Enumerable.Empty<CrmCommunicationTemplateDto>();
        }

        return entities.MapToList<CrmCommunicationTemplateDto>();
    }

    public async Task<IEnumerable<CrmCommunicationTemplateDDLDto>> CommunicationTemplateForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommunicationTemplates.ListByConditionAsync(x => true, x => x.TemplateName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Communication Template records found for dropdown.");
            return Enumerable.Empty<CrmCommunicationTemplateDDLDto>();
        }

        return entities.MapToList<CrmCommunicationTemplateDDLDto>();
    }

    public async Task<GridEntity<CrmCommunicationTemplateDto>> CommunicationTemplateSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT ct.CommunicationTemplateId, ct.CommunicationTypeId, t.CommunicationTypeName, ct.TemplateName, ct.Subject, ct.TemplateBody, ct.IsActive, ct.CreatedDate, ct.CreatedBy, ct.UpdatedDate, ct.UpdatedBy FROM CrmCommunicationTemplate ct LEFT JOIN CrmCommunicationType t ON t.CommunicationTypeId = ct.CommunicationTypeId";
        const string orderBy = "TemplateName ASC";
        return await _repository.CrmCommunicationTemplates.AdoGridDataAsync<CrmCommunicationTemplateDto>(query, options, orderBy, string.Empty, cancellationToken);
    }
}
