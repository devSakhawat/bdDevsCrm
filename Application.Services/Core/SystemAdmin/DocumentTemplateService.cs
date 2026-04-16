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
/// DocumentTemplate service implementing business logic for document template management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class DocumentTemplateService : IDocumentTemplateService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<DocumentTemplateService> _logger;
    private readonly IConfiguration _configuration;

    public DocumentTemplateService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<DocumentTemplateService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new document template record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentTemplateDto> CreateAsync(CreateDocumentTemplateRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateDocumentTemplateRecord));

        // FluentValidation
        var validator = new CreateDocumentTemplateRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new document template. Title: {DocumentTitle}, Time: {Time}",
            record.DocumentTitle, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        DocumentTemplate documentTemplate = record.MapTo<DocumentTemplate>();

        int documentId = await _repository.DocumentTemplates.CreateAndIdAsync(documentTemplate, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document template created successfully. ID: {DocumentId}, Time: {Time}",
            documentId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentTemplate:All");
        await _cache.RemoveAsync("DocumentTemplate:Active");

        // Return as DTO
        var resultDto = documentTemplate.MapTo<DocumentTemplateDto>();
        resultDto.DocumentId = documentId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing document template record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentTemplateDto> UpdateAsync(UpdateDocumentTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateDocumentTemplateRecord));

        // FluentValidation
        var validator = new UpdateDocumentTemplateRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating document template. ID: {DocumentId}, Time: {Time}",
            record.DocumentId, DateTime.UtcNow);

        // Check if document template exists
        var existingDocumentTemplate = await _repository.DocumentTemplates.DocumentTemplateAsync(
            record.DocumentId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("DocumentTemplate", "DocumentId", record.DocumentId.ToString());

        // Map Record to Entity using Mapster
        DocumentTemplate documentTemplate = record.MapTo<DocumentTemplate>();

        _repository.DocumentTemplates.UpdateByState(documentTemplate);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document template updated successfully. ID: {DocumentId}, Time: {Time}",
            record.DocumentId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentTemplate:All");
        await _cache.RemoveAsync("DocumentTemplate:Active");
        await _cache.RemoveAsync($"DocumentTemplate:{record.DocumentId}");

        return documentTemplate.MapTo<DocumentTemplateDto>();
    }

    /// <summary>
    /// Deletes a document template record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteDocumentTemplateRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.DocumentId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteDocumentTemplateRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting document template. ID: {DocumentId}, Time: {Time}",
            record.DocumentId, DateTime.UtcNow);

        var documentTemplateEntity = await _repository.DocumentTemplates.DocumentTemplateAsync(
            record.DocumentId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DocumentTemplate", "DocumentId", record.DocumentId.ToString());

        await _repository.DocumentTemplates.DeleteAsync(dt => dt.DocumentId == record.DocumentId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document template deleted successfully. ID: {DocumentId}, Time: {Time}",
            record.DocumentId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentTemplate:All");
        await _cache.RemoveAsync("DocumentTemplate:Active");
        await _cache.RemoveAsync($"DocumentTemplate:{record.DocumentId}");
    }

    /// <summary>
    /// Retrieves a single document template record by its ID with caching.
    /// </summary>
    public async Task<DocumentTemplateDto> DocumentTemplateAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document template. ID: {DocumentId}, Time: {Time}", documentId, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"DocumentTemplate:{documentId}",
            factory: async () =>
            {
                var documentTemplate = await _repository.DocumentTemplates.DocumentTemplateAsync(documentId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("DocumentTemplate", "DocumentId", documentId.ToString());

                _logger.LogInformation("Document template fetched successfully. ID: {DocumentId}, Time: {Time}",
                    documentId, DateTime.UtcNow);

                return documentTemplate.MapTo<DocumentTemplateDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("DocumentTemplate", "DocumentId", documentId.ToString());
    }

    /// <summary>
    /// Retrieves all document template records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentTemplateDto>> DocumentTemplatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all document templates. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "DocumentTemplate:All",
            factory: async () =>
            {
                var documentTemplates = await _repository.DocumentTemplates.DocumentTemplatesAsync(trackChanges, cancellationToken);

                if (!documentTemplates.Any())
                {
                    _logger.LogWarning("No document templates found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<DocumentTemplateDto>();
                }

                var documentTemplatesDto = documentTemplates.MapToList<DocumentTemplateDto>();

                _logger.LogInformation("Document templates fetched successfully. Count: {Count}, Time: {Time}",
                    documentTemplatesDto.Count(), DateTime.UtcNow);

                return documentTemplatesDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentTemplateDto>();
    }

    /// <summary>
    /// Retrieves all document templates for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentTemplateDDLDto>> DocumentTemplatesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document templates for dropdown list");

        return await _cache.OrSetAsync(
            key: "DocumentTemplate:DDL",
            factory: async () =>
            {
                var documentTemplates = await _repository.DocumentTemplates.ActiveDocumentTemplatesAsync(trackChanges, cancellationToken);

                if (!documentTemplates.Any())
                {
                    _logger.LogWarning("No document templates found for dropdown");
                    return Enumerable.Empty<DocumentTemplateDDLDto>();
                }

                return documentTemplates.MapToList<DocumentTemplateDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentTemplateDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all document templates.
    /// </summary>
    public async Task<GridEntity<DocumentTemplateDto>> DocumentTemplatesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM DocumentTemplate";
        const string orderBy = "DocumentId DESC";

        _logger.LogInformation("Fetching document templates summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.DocumentTemplates.AdoGridDataAsync<DocumentTemplateDto>(sql, options, orderBy, "", cancellationToken);
    }
}
