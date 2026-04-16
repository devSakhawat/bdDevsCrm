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
/// DocumentQueryMapping service implementing business logic for document query mapping management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class DocumentQueryMappingService : IDocumentQueryMappingService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<DocumentQueryMappingService> _logger;
    private readonly IConfiguration _configuration;

    public DocumentQueryMappingService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<DocumentQueryMappingService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new document query mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentQueryMappingDto> CreateAsync(CreateDocumentQueryMappingRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateDocumentQueryMappingRecord));

        // FluentValidation
        var validator = new CreateDocumentQueryMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new document query mapping. ReportHeaderId: {ReportHeaderId}, DocumentTypeId: {DocumentTypeId}, Time: {Time}",
            record.ReportHeaderId, record.DocumentTypeId, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        DocumentQueryMapping documentQueryMapping = record.MapTo<DocumentQueryMapping>();

        int documentQueryId = await _repository.DocumentQueryMappings.CreateAndIdAsync(documentQueryMapping, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document query mapping created successfully. ID: {DocumentQueryId}, Time: {Time}",
            documentQueryId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentQueryMapping:All");
        await _cache.RemoveAsync("DocumentQueryMapping:Active");

        // Return as DTO
        var resultDto = documentQueryMapping.MapTo<DocumentQueryMappingDto>();
        resultDto.DocumentQueryId = documentQueryId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing document query mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentQueryMappingDto> UpdateAsync(UpdateDocumentQueryMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateDocumentQueryMappingRecord));

        // FluentValidation
        var validator = new UpdateDocumentQueryMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating document query mapping. ID: {DocumentQueryId}, Time: {Time}",
            record.DocumentQueryId, DateTime.UtcNow);

        // Check if document query mapping exists
        var existingDocumentQueryMapping = await _repository.DocumentQueryMappings.DocumentQueryMappingAsync(
            record.DocumentQueryId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("DocumentQueryMapping", "DocumentQueryId", record.DocumentQueryId.ToString());

        // Map Record to Entity using Mapster
        DocumentQueryMapping documentQueryMapping = record.MapTo<DocumentQueryMapping>();

        _repository.DocumentQueryMappings.UpdateByState(documentQueryMapping);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document query mapping updated successfully. ID: {DocumentQueryId}, Time: {Time}",
            record.DocumentQueryId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentQueryMapping:All");
        await _cache.RemoveAsync("DocumentQueryMapping:Active");
        await _cache.RemoveAsync($"DocumentQueryMapping:{record.DocumentQueryId}");

        return documentQueryMapping.MapTo<DocumentQueryMappingDto>();
    }

    /// <summary>
    /// Deletes a document query mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteDocumentQueryMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.DocumentQueryId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteDocumentQueryMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting document query mapping. ID: {DocumentQueryId}, Time: {Time}",
            record.DocumentQueryId, DateTime.UtcNow);

        var documentQueryMappingEntity = await _repository.DocumentQueryMappings.DocumentQueryMappingAsync(
            record.DocumentQueryId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DocumentQueryMapping", "DocumentQueryId", record.DocumentQueryId.ToString());

        await _repository.DocumentQueryMappings.DeleteAsync(dqm => dqm.DocumentQueryId == record.DocumentQueryId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document query mapping deleted successfully. ID: {DocumentQueryId}, Time: {Time}",
            record.DocumentQueryId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentQueryMapping:All");
        await _cache.RemoveAsync("DocumentQueryMapping:Active");
        await _cache.RemoveAsync($"DocumentQueryMapping:{record.DocumentQueryId}");
    }

    /// <summary>
    /// Retrieves a single document query mapping record by its ID with caching.
    /// </summary>
    public async Task<DocumentQueryMappingDto> DocumentQueryMappingAsync(int documentQueryId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document query mapping. ID: {DocumentQueryId}, Time: {Time}", documentQueryId, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"DocumentQueryMapping:{documentQueryId}",
            factory: async () =>
            {
                var documentQueryMapping = await _repository.DocumentQueryMappings.DocumentQueryMappingAsync(documentQueryId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("DocumentQueryMapping", "DocumentQueryId", documentQueryId.ToString());

                _logger.LogInformation("Document query mapping fetched successfully. ID: {DocumentQueryId}, Time: {Time}",
                    documentQueryId, DateTime.UtcNow);

                return documentQueryMapping.MapTo<DocumentQueryMappingDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("DocumentQueryMapping", "DocumentQueryId", documentQueryId.ToString());
    }

    /// <summary>
    /// Retrieves all document query mapping records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentQueryMappingDto>> DocumentQueryMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all document query mappings. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "DocumentQueryMapping:All",
            factory: async () =>
            {
                var documentQueryMappings = await _repository.DocumentQueryMappings.DocumentQueryMappingsAsync(trackChanges, cancellationToken);

                if (!documentQueryMappings.Any())
                {
                    _logger.LogWarning("No document query mappings found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<DocumentQueryMappingDto>();
                }

                var documentQueryMappingsDto = documentQueryMappings.MapToList<DocumentQueryMappingDto>();

                _logger.LogInformation("Document query mappings fetched successfully. Count: {Count}, Time: {Time}",
                    documentQueryMappingsDto.Count(), DateTime.UtcNow);

                return documentQueryMappingsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentQueryMappingDto>();
    }

    /// <summary>
    /// Retrieves all document query mappings for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentQueryMappingDDLDto>> DocumentQueryMappingsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document query mappings for dropdown list");

        return await _cache.OrSetAsync(
            key: "DocumentQueryMapping:DDL",
            factory: async () =>
            {
                var documentQueryMappings = await _repository.DocumentQueryMappings.ActiveDocumentQueryMappingsAsync(trackChanges, cancellationToken);

                if (!documentQueryMappings.Any())
                {
                    _logger.LogWarning("No document query mappings found for dropdown");
                    return Enumerable.Empty<DocumentQueryMappingDDLDto>();
                }

                return documentQueryMappings.MapToList<DocumentQueryMappingDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentQueryMappingDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all document query mappings.
    /// </summary>
    public async Task<GridEntity<DocumentQueryMappingDto>> DocumentQueryMappingsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM DocumentQueryMapping";
        const string orderBy = "DocumentQueryId DESC";

        _logger.LogInformation("Fetching document query mappings summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.DocumentQueryMappings.AdoGridDataAsync<DocumentQueryMappingDto>(sql, options, orderBy, "", cancellationToken);
    }
}
