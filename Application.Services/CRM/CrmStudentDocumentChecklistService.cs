using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmStudentDocumentChecklistService : ICrmStudentDocumentChecklistService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmStudentDocumentChecklistService> _logger;
    private readonly IConfiguration _config;

    public CrmStudentDocumentChecklistService(IRepositoryManager repository, ILogger<CrmStudentDocumentChecklistService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmStudentDocumentChecklistDto> CreateAsync(CreateCrmStudentDocumentChecklistRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmStudentDocumentChecklistRecord));
        var entity = record.MapTo<CrmStudentDocumentChecklist>();
        int newId = await _repository.CrmStudentDocumentChecklists.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        var dto = entity.MapTo<CrmStudentDocumentChecklistDto>();
        dto.StudentDocumentChecklistId = newId;
        return dto;
    }

    public async Task<CrmStudentDocumentChecklistDto> UpdateAsync(UpdateCrmStudentDocumentChecklistRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmStudentDocumentChecklistRecord));
        _ = await _repository.CrmStudentDocumentChecklists.StudentDocumentChecklistAsync(record.StudentDocumentChecklistId, false, cancellationToken)
            ?? throw new NotFoundException("CrmStudentDocumentChecklist", "StudentDocumentChecklistId", record.StudentDocumentChecklistId.ToString());
        var entity = record.MapTo<CrmStudentDocumentChecklist>();
        _repository.CrmStudentDocumentChecklists.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmStudentDocumentChecklistDto>();
    }

    public async Task DeleteAsync(DeleteCrmStudentDocumentChecklistRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.StudentDocumentChecklistId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmStudentDocumentChecklists.DeleteAsync(x => x.StudentDocumentChecklistId == record.StudentDocumentChecklistId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmStudentDocumentChecklistDto> StudentDocumentChecklistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmStudentDocumentChecklists.StudentDocumentChecklistAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmStudentDocumentChecklist", "StudentDocumentChecklistId", id.ToString())).MapTo<CrmStudentDocumentChecklistDto>();

    public async Task<IEnumerable<CrmStudentDocumentChecklistDto>> StudentDocumentChecklistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentDocumentChecklists.StudentDocumentChecklistsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDocumentChecklistDto>() : Enumerable.Empty<CrmStudentDocumentChecklistDto>();
    }

    public async Task<IEnumerable<CrmStudentDocumentChecklistDto>> StudentDocumentChecklistsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmStudentDocumentChecklists.StudentDocumentChecklistsByStudentIdAsync(studentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmStudentDocumentChecklistDto>() : Enumerable.Empty<CrmStudentDocumentChecklistDto>();
    }

    public async Task<GridEntity<CrmStudentDocumentChecklistDto>> StudentDocumentChecklistsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT StudentDocumentChecklistId, StudentId, DocumentTypeId, IsMandatory, IsSubmitted, IsVerified, RequiredByApplicationId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmStudentDocumentChecklist";
        return await _repository.CrmStudentDocumentChecklists.AdoGridDataAsync<CrmStudentDocumentChecklistDto>(sql, options, "StudentDocumentChecklistId DESC", string.Empty, cancellationToken);
    }
}
