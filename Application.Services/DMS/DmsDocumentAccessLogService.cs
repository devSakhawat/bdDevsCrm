using Domain.Contracts.Repositories;
﻿// DmsDocumentAccessLogService.cs
using Domain.Entities.Entities.DMS;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.DMS;
using bdDevs.Shared.DataTransferObjects.DMS;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.DMS;

/// <summary>
/// DMS Document Access Log service implementing business logic for access log management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class DmsDocumentAccessLogService : IDmsDocumentAccessLogService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<DmsDocumentAccessLogService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="DmsDocumentAccessLogService"/> with required dependencies.
	/// </summary>
	/// <param name="repository">The repository manager for data access operations.</param>
	/// <param name="logger">The logger for capturing service-level events.</param>
	/// <param name="configuration">The application configuration accessor.</param>
	public DmsDocumentAccessLogService(IRepositoryManager repository, ILogger<DmsDocumentAccessLogService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Retrieves all document access log records from the database.
	/// </summary>
	public async Task<IEnumerable<DmsDocumentAccessLogDto>> AccessLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all access logs. Time: {Time}", DateTime.UtcNow);

		var logs = await _repository.DmsDocumentAccessLogs.AccessLogsAsync(trackChanges, cancellationToken);

		if (!logs.Any())
		{
			_logger.LogWarning("No access logs found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentAccessLogDto>();
		}

		var logsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<DmsDocumentAccessLog, DmsDocumentAccessLogDto>(logs);

		_logger.LogInformation("Access logs fetched successfully. Count: {Count}, Time: {Time}", logsDto.Count(), DateTime.UtcNow);

		return logsDto;
	}

	///// <summary>
	///// Retrieves a lightweight list of all document access logs suitable for use in dropdown lists.
	///// </summary>
	//public async Task<IEnumerable<DmsDocumentAccessLogDDL>> AccessLogsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	//{
	//	_logger.LogInformation("Fetching access logs for dropdown list. Time: {Time}", DateTime.UtcNow);

	//	var logs = await _repository.DmsDocumentAccessLogs.ListAsync(trackChanges, cancellationToken);

	//	if (!logs.Any())
	//	{
	//		_logger.LogWarning("No access logs found for dropdown list. Time: {Time}", DateTime.UtcNow);
	//		return Enumerable.Empty<DmsDocumentAccessLogDDL>();
	//	}

	//	var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentAccessLog, DmsDocumentAccessLogDDL>(logs);

	//	_logger.LogInformation("Access logs fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
	//					ddlDtos.Count(), DateTime.UtcNow);

	//	return ddlDtos;
	//}

	/// <summary>
	/// Retrieves a paginated summary grid of all document access logs.
	/// </summary>
	public async Task<GridEntity<DmsDocumentAccessLogDto>> AccessLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query = "SELECT * FROM DmsDocumentAccessLog";
		const string orderBy = "AccessDateTime DESC";

		_logger.LogInformation("Fetching access logs summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.DmsDocumentAccessLogs.AdoGridDataAsync<DmsDocumentAccessLogDto>(query, options, orderBy, "", cancellationToken);
	}

	/// <summary>
	/// Retrieves a single document access log record by its ID.
	/// </summary>
	public async Task<DmsDocumentAccessLogDto> AccessLogAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching access log. ID: {LogId}, Time: {Time}", id, DateTime.UtcNow);

		var log = await _repository.DmsDocumentAccessLogs
						.FirstOrDefaultAsync(x => x.LogId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentAccessLog", "LogId", id.ToString());

		_logger.LogInformation("Access log fetched successfully. ID: {LogId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentAccessLog, DmsDocumentAccessLogDto>(log);
	}

	/// <summary>
	/// Creates a new document access log record.
	/// </summary>
	public async Task<DmsDocumentAccessLogDto> CreateAccessLogAsync(DmsDocumentAccessLogDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(DmsDocumentAccessLogDto));

		if (entityForCreate.LogId != 0)
			throw new InvalidCreateOperationException("LogId must be 0 for new record.");

		_logger.LogInformation("Creating new access log. Time: {Time}", DateTime.UtcNow);

		var log = MyMapper.JsonClone<DmsDocumentAccessLogDto, DmsDocumentAccessLog>(entityForCreate);

		await _repository.DmsDocumentAccessLogs.CreateAsync(log, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Access log could not be saved to the database.");

		_logger.LogInformation("Access log created successfully. ID: {LogId}, Time: {Time}",
						log.LogId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentAccessLog, DmsDocumentAccessLogDto>(log);
	}

	/// <summary>
	/// Updates an existing document access log record.
	/// </summary>
	public async Task<DmsDocumentAccessLogDto> UpdateAccessLogAsync(int logId, DmsDocumentAccessLogDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(DmsDocumentAccessLogDto));

		if (logId != modelDto.LogId)
			throw new BadRequestException(logId.ToString(), nameof(DmsDocumentAccessLogDto));

		_logger.LogInformation("Updating access log. ID: {LogId}, Time: {Time}", logId, DateTime.UtcNow);

		var logEntity = await _repository.DmsDocumentAccessLogs
						.FirstOrDefaultAsync(x => x.LogId == logId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("DmsDocumentAccessLog", "LogId", logId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<DmsDocumentAccessLog, DmsDocumentAccessLogDto>(logEntity, modelDto);
		_repository.DmsDocumentAccessLogs.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("DmsDocumentAccessLog", "LogId", logId.ToString());

		_logger.LogInformation("Access log updated successfully. ID: {LogId}, Time: {Time}",
						logId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentAccessLog, DmsDocumentAccessLogDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a document access log record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteAccessLogAsync(int logId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (logId <= 0)
			throw new BadRequestException(logId.ToString(), nameof(DmsDocumentAccessLogDto));

		_logger.LogInformation("Deleting access log. ID: {LogId}, Time: {Time}", logId, DateTime.UtcNow);

		var logEntity = await _repository.DmsDocumentAccessLogs
						.FirstOrDefaultAsync(x => x.LogId == logId, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentAccessLog", "LogId", logId.ToString());

		await _repository.DmsDocumentAccessLogs.DeleteAsync(x => x.LogId == logId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("DmsDocumentAccessLog", "LogId", logId.ToString());

		_logger.LogInformation("Access log deleted successfully. ID: {LogId}, Time: {Time}",
						logId, DateTime.UtcNow);

		return affected;
	}
}




//using Domain.Entities.Entities.DMS;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.DMS;
//using bdDevs.Shared.DataTransferObjects.DMS;
//using Domain.Exceptions;
//using Domain.Contracts.Repositories;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Domain.Contracts.Services.DMS;



//internal sealed class DmsDocumentAccessLogService : IDmsDocumentAccessLogService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<DmsDocumentAccessLogService> _logger;
//  private readonly IConfiguration _configuration;

//  public DmsDocumentAccessLogService(IRepositoryManager repository, ILogger<DmsDocumentAccessLogService> logger, IConfiguration configuration)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//  }

//  public async Task<IEnumerable<DmsDocumentAccessLogDDL>> AccessLogsDDLAsync(bool trackChanges = false)
//  {
//    var logs = await _repository.DmsDocumentAccessLogs.ListAsync(trackChanges:trackChanges);

//    if (!logs.Any())
//      throw new GenericListNotFoundException("DmsDocumentAccessLog");

//    var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentAccessLog, DmsDocumentAccessLogDDL>(logs);

//    return ddlDtos;
//  }

//  public async Task<GridEntity<DmsDocumentAccessLogDto>> SummaryGrid(GridOptions options)
//  {
//    string query = "SELECT * FROM DmsDocumentAccessLog";  // adjust if needed
//    string orderBy = "AccessDateTime desc";

//    var gridEntity = await _repository.DmsDocumentAccessLogs.GridData<DmsDocumentAccessLogDto>(query, options, orderBy, "");

//    return gridEntity;
//  }

//  public async Task<string> CreateNewRecordAsync(DmsDocumentAccessLogDto modelDto)
//  {
//    if (modelDto.LogId != 0)
//      throw new InvalidCreateOperationException("LogId must be 0 when creating a new log.");

//    var log = MyMapper.JsonClone<DmsDocumentAccessLogDto, DmsDocumentAccessLog>(modelDto);

//    var createdId = await _repository.DmsDocumentAccessLogs.CreateAndIdAsync(log);
//    if (createdId == 0)
//      throw new InvalidCreateOperationException();

//    await _repository.SaveAsync();
//    _logger.LogWarning("New access log created with Id: {LogId}", createdId);

//    return OperationMessage.Success;
//  }

//  public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentAccessLogDto modelDto, bool trackChanges)
//  {
//    if (key <= 0 || key != modelDto.LogId)
//      return "Invalid update attempt: key does not match the LogId.";

//    bool exists = await _repository.DmsDocumentAccessLogs.ExistsAsync(x => x.LogId == key);
//    if (!exists)
//      return "Update failed: log not found.";

//    var log = MyMapper.JsonClone<DmsDocumentAccessLogDto, DmsDocumentAccessLog>(modelDto);

//    _repository.DmsDocumentAccessLogs.Update(log);
//    await _repository.SaveAsync();
//    _logger.LogWarning("Access log with Id: {LogId} updated.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> DeleteRecordAsync(int key, DmsDocumentAccessLogDto modelDto)
//  {
//    if (modelDto == null)
//      throw new BadRequestException(nameof(DmsDocumentAccessLogDto));

//    if (key != modelDto.LogId)
//      throw new BadRequestException(key.ToString(), nameof(DmsDocumentAccessLogDto));

//    var log = await _repository.DmsDocumentAccessLogs.FirstOrDefaultAsync(x => x.LogId == key, false);

//    if (log == null)
//      throw new NotFoundException("DmsDocumentAccessLog", "LogId", key.ToString());

//    await _repository.DmsDocumentAccessLogs.DeleteAsync(x => x.LogId == key, true);
//    await _repository.SaveAsync();

//    _logger.LogWarning("Access log with Id: {LogId} deleted.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> SaveOrUpdate(int key, DmsDocumentAccessLogDto modelDto)
//  {
//    if (modelDto.LogId == 0 && key == 0)
//    {
//      var newLog = MyMapper.JsonClone<DmsDocumentAccessLogDto, DmsDocumentAccessLog>(modelDto);

//      var createdId = await _repository.DmsDocumentAccessLogs.CreateAndIdAsync(newLog);
//      if (createdId == 0)
//        throw new InvalidCreateOperationException();

//      await _repository.SaveAsync();
//      _logger.LogWarning("New access log created with Id: {LogId}", createdId);
//      return OperationMessage.Success;
//    }
//    else if (key > 0 && key == modelDto.LogId)
//    {
//      var exists = await _repository.DmsDocumentAccessLogs.ExistsAsync(x => x.LogId == key);
//      if (!exists)
//      {
//        var updateLog = MyMapper.JsonClone<DmsDocumentAccessLogDto, DmsDocumentAccessLog>(modelDto);
//        _repository.DmsDocumentAccessLogs.Update(updateLog);
//        await _repository.SaveAsync();

//        _logger.LogWarning("Access log with Id: {LogId} updated.", key);
//        return OperationMessage.Success;
//      }
//      else
//      {
//        return "Update failed: access log with this Id already exists.";
//      }
//    }
//    else
//    {
//      return "Invalid key and LogId mismatch.";
//    }
//  }
//}
