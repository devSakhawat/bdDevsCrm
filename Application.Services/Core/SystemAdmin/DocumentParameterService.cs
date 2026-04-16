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
/// DocumentParameter service implementing business logic for document parameter management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class DocumentParameterService : IDocumentParameterService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<DocumentParameterService> _logger;
    private readonly IConfiguration _configuration;

    public DocumentParameterService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<DocumentParameterService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new document parameter record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentParameterDto> CreateAsync(CreateDocumentParameterRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateDocumentParameterRecord));

        // FluentValidation
        var validator = new CreateDocumentParameterRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new document parameter. Name: {ParameterName}, Time: {Time}",
            record.ParameterName, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        DocumentParameter documentParameter = record.MapTo<DocumentParameter>();

        int parameterId = await _repository.DocumentParameters.CreateAndIdAsync(documentParameter, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document parameter created successfully. ID: {ParameterId}, Time: {Time}",
            parameterId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentParameter:All");
        await _cache.RemoveAsync("DocumentParameter:Active");

        // Return as DTO
        var resultDto = documentParameter.MapTo<DocumentParameterDto>();
        resultDto.ParameterId = parameterId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing document parameter record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<DocumentParameterDto> UpdateAsync(UpdateDocumentParameterRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateDocumentParameterRecord));

        // FluentValidation
        var validator = new UpdateDocumentParameterRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating document parameter. ID: {ParameterId}, Time: {Time}",
            record.ParameterId, DateTime.UtcNow);

        // Check if document parameter exists
        var existingDocumentParameter = await _repository.DocumentParameters.DocumentParameterAsync(
            record.ParameterId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("DocumentParameter", "ParameterId", record.ParameterId.ToString());

        // Map Record to Entity using Mapster
        DocumentParameter documentParameter = record.MapTo<DocumentParameter>();

        _repository.DocumentParameters.UpdateByState(documentParameter);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document parameter updated successfully. ID: {ParameterId}, Time: {Time}",
            record.ParameterId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentParameter:All");
        await _cache.RemoveAsync("DocumentParameter:Active");
        await _cache.RemoveAsync($"DocumentParameter:{record.ParameterId}");

        return documentParameter.MapTo<DocumentParameterDto>();
    }

    /// <summary>
    /// Deletes a document parameter record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteDocumentParameterRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ParameterId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteDocumentParameterRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting document parameter. ID: {ParameterId}, Time: {Time}",
            record.ParameterId, DateTime.UtcNow);

        var documentParameterEntity = await _repository.DocumentParameters.DocumentParameterAsync(
            record.ParameterId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DocumentParameter", "ParameterId", record.ParameterId.ToString());

        await _repository.DocumentParameters.DeleteAsync(dp => dp.ParameterId == record.ParameterId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Document parameter deleted successfully. ID: {ParameterId}, Time: {Time}",
            record.ParameterId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("DocumentParameter:All");
        await _cache.RemoveAsync("DocumentParameter:Active");
        await _cache.RemoveAsync($"DocumentParameter:{record.ParameterId}");
    }

    /// <summary>
    /// Retrieves a single document parameter record by its ID with caching.
    /// </summary>
    public async Task<DocumentParameterDto> DocumentParameterAsync(int parameterId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document parameter. ID: {ParameterId}, Time: {Time}", parameterId, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"DocumentParameter:{parameterId}",
            factory: async () =>
            {
                var documentParameter = await _repository.DocumentParameters.DocumentParameterAsync(parameterId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("DocumentParameter", "ParameterId", parameterId.ToString());

                _logger.LogInformation("Document parameter fetched successfully. ID: {ParameterId}, Time: {Time}",
                    parameterId, DateTime.UtcNow);

                return documentParameter.MapTo<DocumentParameterDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("DocumentParameter", "ParameterId", parameterId.ToString());
    }

    /// <summary>
    /// Retrieves all document parameter records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentParameterDto>> DocumentParametersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all document parameters. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "DocumentParameter:All",
            factory: async () =>
            {
                var documentParameters = await _repository.DocumentParameters.DocumentParametersAsync(trackChanges, cancellationToken);

                if (!documentParameters.Any())
                {
                    _logger.LogWarning("No document parameters found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<DocumentParameterDto>();
                }

                var documentParametersDto = documentParameters.MapToList<DocumentParameterDto>();

                _logger.LogInformation("Document parameters fetched successfully. Count: {Count}, Time: {Time}",
                    documentParametersDto.Count(), DateTime.UtcNow);

                return documentParametersDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentParameterDto>();
    }

    /// <summary>
    /// Retrieves all document parameters for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<DocumentParameterDDLDto>> DocumentParametersForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching document parameters for dropdown list");

        return await _cache.OrSetAsync(
            key: "DocumentParameter:DDL",
            factory: async () =>
            {
                var documentParameters = await _repository.DocumentParameters.ActiveDocumentParametersAsync(trackChanges, cancellationToken);

                if (!documentParameters.Any())
                {
                    _logger.LogWarning("No document parameters found for dropdown");
                    return Enumerable.Empty<DocumentParameterDDLDto>();
                }

                return documentParameters.MapToList<DocumentParameterDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<DocumentParameterDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all document parameters.
    /// </summary>
    public async Task<GridEntity<DocumentParameterDto>> DocumentParametersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM DocumentParameter";
        const string orderBy = "ParameterId DESC";

        _logger.LogInformation("Fetching document parameters summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.DocumentParameters.AdoGridDataAsync<DocumentParameterDto>(sql, options, orderBy, "", cancellationToken);
    }
}
