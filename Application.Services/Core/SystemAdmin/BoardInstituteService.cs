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

internal sealed class BoardInstituteService : IBoardInstituteService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<BoardInstituteService> _logger;
    private readonly IConfiguration _configuration;

    public BoardInstituteService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<BoardInstituteService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<BoardInstituteDto> CreateAsync(CreateBoardInstituteRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateBoardInstituteRecord));

        var validator = new CreateBoardInstituteRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new board/institute. Name: {InstituteName}, Time: {Time}",
            record.InstituteName, DateTime.UtcNow);

        BoardInstitute boardInstitute = record.MapTo<BoardInstitute>();
        
        int id = await _repository.BoardInstitutes.CreateAndIdAsync(boardInstitute, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("BoardInstitute created successfully. ID: {Id}, Time: {Time}",
            id, DateTime.UtcNow);

        await _cache.RemoveAsync("BoardInstitute:All");
        await _cache.RemoveAsync("BoardInstitute:Active");

        var resultDto = boardInstitute.MapTo<BoardInstituteDto>();
        resultDto.Id = id;
        return resultDto;
    }

    public async Task<BoardInstituteDto> UpdateAsync(UpdateBoardInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateBoardInstituteRecord));

        var validator = new UpdateBoardInstituteRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var boardInstitute = await _repository.BoardInstitutes.BoardInstituteAsync(record.Id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("BoardInstitute", "Id", record.Id.ToString());

        _logger.LogInformation("Updating board/institute. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        record.MapTo(boardInstitute);
        
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("BoardInstitute updated successfully. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        await _cache.RemoveAsync($"BoardInstitute:{record.Id}");
        await _cache.RemoveAsync("BoardInstitute:All");
        await _cache.RemoveAsync("BoardInstitute:Active");

        return boardInstitute.MapTo<BoardInstituteDto>();
    }

    public async Task DeleteAsync(DeleteBoardInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(DeleteBoardInstituteRecord));

        var boardInstitute = await _repository.BoardInstitutes.BoardInstituteAsync(record.Id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("BoardInstitute", "Id", record.Id.ToString());

        _logger.LogInformation("Deleting board/institute. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        _repository.BoardInstitutes.Delete(boardInstitute);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("BoardInstitute deleted successfully. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        await _cache.RemoveAsync($"BoardInstitute:{record.Id}");
        await _cache.RemoveAsync("BoardInstitute:All");
        await _cache.RemoveAsync("BoardInstitute:Active");
    }

    public async Task<BoardInstituteDto> BoardInstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: $"BoardInstitute:{id}",
            factory: async () =>
            {
                var boardInstitute = await _repository.BoardInstitutes.BoardInstituteAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("BoardInstitute", "Id", id.ToString());
                return boardInstitute.MapTo<BoardInstituteDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("BoardInstitute", "Id", id.ToString());
    }

    public async Task<IEnumerable<BoardInstituteDto>> BoardInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "BoardInstitute:All",
            factory: async () =>
            {
                var boardInstitutes = await _repository.BoardInstitutes.BoardInstitutesAsync(trackChanges, cancellationToken);
                return boardInstitutes.MapToList<BoardInstituteDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<BoardInstituteDto>();
    }

    public async Task<IEnumerable<BoardInstituteDDLDto>> BoardInstitutesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "BoardInstitute:Active",
            factory: async () =>
            {
                var boardInstitutes = await _repository.BoardInstitutes.ActiveBoardInstitutesAsync(trackChanges, cancellationToken);
                return boardInstitutes.MapToList<BoardInstituteDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<BoardInstituteDDLDto>();
    }

    public async Task<GridEntity<BoardInstituteDto>> BoardInstitutesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        var boardInstitutes = await _repository.BoardInstitutes.BoardInstitutesAsync(false, cancellationToken);
        var boardInstituteDtos = boardInstitutes.MapToList<BoardInstituteDto>();
        return boardInstituteDtos.GridDataSource(options);
    }
}
