using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>CrmNote service implementing business logic for note management.</summary>
internal sealed class CrmNoteService : ICrmNoteService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmNoteService> _logger;
    private readonly IConfiguration _config;

    public CrmNoteService(IRepositoryManager repository, ILogger<CrmNoteService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new note record.</summary>
    public async Task<CrmNoteDto> CreateAsync(CreateCrmNoteRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmNoteRecord));

        _logger.LogInformation("Creating new Note. Time: {Time}", DateTime.UtcNow);

        CrmNote entity = record.MapTo<CrmNote>();
        int newId = await _repository.CrmNotes.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Note created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmNoteDto>() with { NoteId = newId };
    }

    /// <summary>Updates an existing note record.</summary>
    public async Task<CrmNoteDto> UpdateAsync(UpdateCrmNoteRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmNoteRecord));

        _ = await _repository.CrmNotes
            .FirstOrDefaultAsync(x => x.NoteId == record.NoteId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Note", "NoteId", record.NoteId.ToString());

        _logger.LogInformation("Updating Note. ID: {Id}, Time: {Time}", record.NoteId, DateTime.UtcNow);

        CrmNote entity = record.MapTo<CrmNote>();
        _repository.CrmNotes.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Note updated successfully. ID: {Id}, Time: {Time}", record.NoteId, DateTime.UtcNow);
        return entity.MapTo<CrmNoteDto>();
    }

    /// <summary>Deletes a note record.</summary>
    public async Task DeleteAsync(DeleteCrmNoteRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.NoteId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmNotes
            .FirstOrDefaultAsync(x => x.NoteId == record.NoteId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Note", "NoteId", record.NoteId.ToString());

        _logger.LogInformation("Deleting Note. ID: {Id}, Time: {Time}", record.NoteId, DateTime.UtcNow);
        await _repository.CrmNotes.DeleteAsync(x => x.NoteId == record.NoteId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Note deleted successfully. ID: {Id}, Time: {Time}", record.NoteId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single note record by ID.</summary>
    public async Task<CrmNoteDto> NoteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Note. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmNotes
            .FirstOrDefaultAsync(x => x.NoteId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Note", "NoteId", id.ToString());
        return entity.MapTo<CrmNoteDto>();
    }

    /// <summary>Retrieves all note records.</summary>
    public async Task<IEnumerable<CrmNoteDto>> NotesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Notes. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmNotes.CrmNotesAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Notes found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmNoteDto>();
        }
        _logger.LogInformation("Notes fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmNoteDto>();
    }

    /// <summary>Retrieves a lightweight list of notes for dropdown binding.</summary>
    public async Task<IEnumerable<CrmNoteDto>> NoteForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Notes for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmNotes.CrmNotesAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Notes found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmNoteDto>();
        }
        return entities.MapToList<CrmNoteDto>();
    }

    /// <summary>Retrieves a paginated summary grid of notes.</summary>
    public async Task<GridEntity<CrmNoteDto>> NotesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Notes summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT NoteId, EntityType, EntityId, NoteText, NoteDate, IsPrivate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmNote";
        const string orderBy = "NoteDate DESC";
        return await _repository.CrmNotes.AdoGridDataAsync<CrmNoteDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
