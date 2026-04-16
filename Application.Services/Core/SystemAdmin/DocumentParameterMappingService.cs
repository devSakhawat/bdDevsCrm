using Application.Services.Caching;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// DocumentParameterMapping service implementing business logic for document parameter mapping management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class DocumentParameterMappingService : IDocumentParameterMappingService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<DocumentParameterMappingService> _logger;
    private readonly IConfiguration _configuration;

    public DocumentParameterMappingService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<DocumentParameterMappingService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new document parameter mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentParameterMappingDto> CreateAsync(CreateDocumentParameterMappingRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateDocumentParameterMappingRecord));

        // FluentValidation
        var validator = new CreateDocumentParameterMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new document parameter mapping. DocumentTypeId: {DocumentTypeId}, ParameterId: {ParameterId}, Time: {Time}",
            record.DocumentTypeId, record.ParameterId, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        DocumentParameterMapping documentParameterMapping = record.MapTo<DocumentParameterMapping>();

        int mappingId = await _repository.DocumentParameterMappings.CreateAndIdAsync(documentParameterMapping, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document parameter mapping created successfully. ID: {MappingId}, Time: {Time}",
            mappingId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentParameterMapping:All");
        await _cache.RemoveAsync("DocumentParameterMapping:Active");

        // Return as DTO
        var resultDto = documentParameterMapping.MapTo<DocumentParameterMappingDto>();
        resultDto.MappingId = mappingId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing document parameter mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentParameterMappingDto> UpdateAsync(UpdateDocumentParameterMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateDocumentParameterMappingRecord));

        // FluentValidation
        var validator = new UpdateDocumentParameterMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating document parameter mapping. ID: {MappingId}, Time: {Time}",
            record.MappingId, DateTime.UtcNow);

        // Check if document parameter mapping exists
        var existingDocumentParameterMapping = await _repository.DocumentParameterMappings.DocumentParameterMappingAsync(
            record.MappingId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("DocumentParameterMapping", "MappingId", record.MappingId.ToString());

        // Map Record to Entity using Mapster
        DocumentParameterMapping documentParameterMapping = record.MapTo<DocumentParameterMapping>();

        _repository.DocumentParameterMappings.UpdateByState(documentParameterMapping);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document parameter mapping updated successfully. ID: {MappingId}, Time: {Time}",
            record.MappingId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentParameterMapping:All");
        await _cache.RemoveAsync("DocumentParameterMapping:Active");
        await _cache.RemoveAsync($"DocumentParameterMapping:{record.MappingId}");

        return documentParameterMapping.MapTo<DocumentParameterMappingDto>();
    }

    /// <summary>
    /// Deletes a document parameter mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteDocumentParameterMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.MappingId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteDocumentParameterMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting document parameter mapping. ID: {MappingId}, Time: {Time}",
            record.MappingId, DateTime.UtcNow);

        var documentParameterMappingEntity = await _repository.DocumentParameterMappings.DocumentParameterMappingAsync(
            record.MappingId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DocumentParameterMapping", "MappingId", record.MappingId.ToString());

        await _repository.DocumentParameterMappings.DeleteAsync(dpm => dpm.MappingId == record.MappingId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document parameter mapping deleted successfully. ID: {MappingId}, Time: {Time}",
            record.MappingId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentParameterMapping:All");
        await _cache.RemoveAsync("DocumentParameterMapping:Active");
        await _cache.RemoveAsync($"DocumentParameterMapping:{record.MappingId}");
    }

    /// <summary>
    /// Retrieves a single document parameter mapping record by its ID with caching.
    /// </summary>
    public async Task<DocumentParameterMappingDto> DocumentParameterMappingAsync(int mappingId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document parameter mapping. ID: {MappingId}, Time: {Time}", mappingId, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"DocumentParameterMapping:{mappingId}",
            factory: async () =>
            {
                var documentParameterMapping = await _repository.DocumentParameterMappings.DocumentParameterMappingAsync(mappingId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("DocumentParameterMapping", "MappingId", mappingId.ToString());

                _logger.LogInformation("Document parameter mapping fetched successfully. ID: {MappingId}, Time: {Time}",
                    mappingId, DateTime.UtcNow);

                return documentParameterMapping.MapTo<DocumentParameterMappingDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("DocumentParameterMapping", "MappingId", mappingId.ToString());
    }

    /// <summary>
    /// Retrieves all document parameter mapping records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentParameterMappingDto>> DocumentParameterMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all document parameter mappings. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "DocumentParameterMapping:All",
            factory: async () =>
            {
                var documentParameterMappings = await _repository.DocumentParameterMappings.DocumentParameterMappingsAsync(trackChanges, cancellationToken);

                if (!documentParameterMappings.Any())
                {
                    _logger.LogWarning("No document parameter mappings found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<DocumentParameterMappingDto>();
                }

                var documentParameterMappingsDto = documentParameterMappings.MapToList<DocumentParameterMappingDto>();

                _logger.LogInformation("Document parameter mappings fetched successfully. Count: {Count}, Time: {Time}",
                    documentParameterMappingsDto.Count(), DateTime.UtcNow);

                return documentParameterMappingsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentParameterMappingDto>();
    }

    /// <summary>
    /// Retrieves all document parameter mappings for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentParameterMappingDDLDto>> DocumentParameterMappingsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document parameter mappings for dropdown list");

        return await _cache.OrSetAsync(
            key: "DocumentParameterMapping:DDL",
            factory: async () =>
            {
                var documentParameterMappings = await _repository.DocumentParameterMappings.ActiveDocumentParameterMappingsAsync(trackChanges, cancellationToken);

                if (!documentParameterMappings.Any())
                {
                    _logger.LogWarning("No document parameter mappings found for dropdown");
                    return Enumerable.Empty<DocumentParameterMappingDDLDto>();
                }

                return documentParameterMappings.MapToList<DocumentParameterMappingDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentParameterMappingDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all document parameter mappings.
    /// </summary>
    public async Task<GridEntity<DocumentParameterMappingDto>> DocumentParameterMappingsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM DocumentParameterMapping";
        const string orderBy = "MappingId DESC";

        _logger.LogInformation("Fetching document parameter mappings summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.DocumentParameterMappings.AdoGridDataAsync<DocumentParameterMappingDto>(sql, options, orderBy, "", cancellationToken);
    }
}
