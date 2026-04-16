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
/// DocumentType service implementing business logic for document type management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class DocumentTypeService : IDocumentTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<DocumentTypeService> _logger;
    private readonly IConfiguration _configuration;

    public DocumentTypeService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<DocumentTypeService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new document type record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentTypeDto> CreateAsync(CreateDocumentTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateDocumentTypeRecord));

        // FluentValidation
        var validator = new CreateDocumentTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new document type. Name: {Documentname}, Time: {Time}",
            record.Documentname, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        DocumentType documentType = record.MapTo<DocumentType>();
        documentType.Initiationdate = DateTime.UtcNow;

        int documenttypeid = await _repository.DocumentTypes.CreateAndIdAsync(documentType, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document type created successfully. ID: {Documenttypeid}, Time: {Time}",
            documenttypeid, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentType:All");
        await _cache.RemoveAsync("DocumentType:Active");

        // Return as DTO
        var resultDto = documentType.MapTo<DocumentTypeDto>();
        resultDto.Documenttypeid = documenttypeid;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing document type record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentTypeDto> UpdateAsync(UpdateDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateDocumentTypeRecord));

        // FluentValidation
        var validator = new UpdateDocumentTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating document type. ID: {Documenttypeid}, Time: {Time}",
            record.Documenttypeid, DateTime.UtcNow);

        // Check if document type exists
        var existingDocumentType = await _repository.DocumentTypes.DocumentTypeAsync(
            record.Documenttypeid, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("DocumentType", "Documenttypeid", record.Documenttypeid.ToString());

        // Map Record to Entity using Mapster
        DocumentType documentType = record.MapTo<DocumentType>();

        _repository.DocumentTypes.UpdateByState(documentType);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document type updated successfully. ID: {Documenttypeid}, Time: {Time}",
            record.Documenttypeid, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentType:All");
        await _cache.RemoveAsync("DocumentType:Active");
        await _cache.RemoveAsync($"DocumentType:{record.Documenttypeid}");

        return documentType.MapTo<DocumentTypeDto>();
    }

    /// <summary>
    /// Deletes a document type record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.Documenttypeid <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteDocumentTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting document type. ID: {Documenttypeid}, Time: {Time}",
            record.Documenttypeid, DateTime.UtcNow);

        var documentTypeEntity = await _repository.DocumentTypes.DocumentTypeAsync(
            record.Documenttypeid, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DocumentType", "Documenttypeid", record.Documenttypeid.ToString());

        await _repository.DocumentTypes.DeleteAsync(dt => dt.Documenttypeid == record.Documenttypeid, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document type deleted successfully. ID: {Documenttypeid}, Time: {Time}",
            record.Documenttypeid, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentType:All");
        await _cache.RemoveAsync("DocumentType:Active");
        await _cache.RemoveAsync($"DocumentType:{record.Documenttypeid}");
    }

    /// <summary>
    /// Retrieves a single document type record by its ID with caching.
    /// </summary>
    public async Task<DocumentTypeDto> DocumentTypeAsync(int documenttypeid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document type. ID: {Documenttypeid}, Time: {Time}", documenttypeid, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"DocumentType:{documenttypeid}",
            factory: async () =>
            {
                var documentType = await _repository.DocumentTypes.DocumentTypeAsync(documenttypeid, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("DocumentType", "Documenttypeid", documenttypeid.ToString());

                _logger.LogInformation("Document type fetched successfully. ID: {Documenttypeid}, Time: {Time}",
                    documenttypeid, DateTime.UtcNow);

                return documentType.MapTo<DocumentTypeDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("DocumentType", "Documenttypeid", documenttypeid.ToString());
    }

    /// <summary>
    /// Retrieves all document type records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentTypeDto>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all document types. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "DocumentType:All",
            factory: async () =>
            {
                var documentTypes = await _repository.DocumentTypes.DocumentTypesAsync(trackChanges, cancellationToken);

                if (!documentTypes.Any())
                {
                    _logger.LogWarning("No document types found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<DocumentTypeDto>();
                }

                var documentTypesDto = documentTypes.MapToList<DocumentTypeDto>();

                _logger.LogInformation("Document types fetched successfully. Count: {Count}, Time: {Time}",
                    documentTypesDto.Count(), DateTime.UtcNow);

                return documentTypesDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentTypeDto>();
    }

    /// <summary>
    /// Retrieves all document types for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentTypeDDLDto>> DocumentTypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document types for dropdown list");

        return await _cache.OrSetAsync(
            key: "DocumentType:DDL",
            factory: async () =>
            {
                var documentTypes = await _repository.DocumentTypes.ActiveDocumentTypesAsync(trackChanges, cancellationToken);

                if (!documentTypes.Any())
                {
                    _logger.LogWarning("No document types found for dropdown");
                    return Enumerable.Empty<DocumentTypeDDLDto>();
                }

                return documentTypes.MapToList<DocumentTypeDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentTypeDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all document types.
    /// </summary>
    public async Task<GridEntity<DocumentTypeDto>> DocumentTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM DocumentType";
        const string orderBy = "Documenttypeid DESC";

        _logger.LogInformation("Fetching document types summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.DocumentTypes.AdoGridDataAsync<DocumentTypeDto>(sql, options, orderBy, "", cancellationToken);
    }
}
