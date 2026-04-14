using bdDevs.Shared.Constants;
using Domain.Contracts.Repositories;
﻿// DmsDocumentFolderService.cs
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
/// DMS Document Folder service implementing business logic for folder management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class DmsDocumentFolderService : IDmsDocumentFolderService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<DmsDocumentFolderService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="DmsDocumentFolderService"/> with required dependencies.
	/// </summary>
	public DmsDocumentFolderService(IRepositoryManager repository, ILogger<DmsDocumentFolderService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<IEnumerable<DmsDocumentFolderDto>> FoldersAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all folders. Time: {Time}", DateTime.UtcNow);

		var folders = await _repository.DmsDocumentFolders.FoldersAsync(trackChanges, cancellationToken);

		if (!folders.Any())
		{
			_logger.LogWarning("No folders found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentFolderDto>();
		}

		var foldersDto = MyMapper.JsonCloneIEnumerableToIEnumerable<DmsDocumentFolder, DmsDocumentFolderDto>(folders);

		_logger.LogInformation("Folders fetched successfully. Count: {Count}, Time: {Time}",
						foldersDto.Count(), DateTime.UtcNow);

		return foldersDto;
	}

	public async Task<IEnumerable<DmsDocumentFolderDDL>> FoldersDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching folders for dropdown list. Time: {Time}", DateTime.UtcNow);

		var foldersDDL = await _repository.DmsDocumentFolders.ListWithSelectAsync(selector: x => new DmsDocumentFolderDDL
		{
			FolderId = x.FolderId,
			FolderName = x.FolderName
		}, 
		orderBy: x => x.ParentFolder.FolderName != null ? x.ParentFolder.FolderName : x.FolderName
		, trackChanges, cancellationToken);

		if (!foldersDDL.Any())
		{
			_logger.LogWarning("No folders found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentFolderDDL>();
		}

		_logger.LogInformation("Folders fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						foldersDDL.Count(), DateTime.UtcNow);

		return foldersDDL;
	}

	public async Task<GridEntity<DmsDocumentFolderDto>> FoldersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query = "SELECT * FROM DmsDocumentFolder";
		const string orderBy = "FolderName ASC";

		_logger.LogInformation("Fetching folders summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.DmsDocumentFolders.AdoGridDataAsync<DmsDocumentFolderDto>(query, options, orderBy, "", cancellationToken);
	}

	public async Task<DmsDocumentFolderDto> FolderAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching folder. ID: {FolderId}, Time: {Time}", id, DateTime.UtcNow);

		var folder = await _repository.DmsDocumentFolders
						.FirstOrDefaultAsync(x => x.FolderId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentFolder", "FolderId", id.ToString());

		_logger.LogInformation("Folder fetched successfully. ID: {FolderId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentFolder, DmsDocumentFolderDto>(folder);
	}

	public async Task<DmsDocumentFolderDto> CreateFolderAsync(DmsDocumentFolderDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(DmsDocumentFolderDto));

		if (entityForCreate.FolderId != 0)
			throw new InvalidCreateOperationException("FolderId must be 0 for new record.");

		bool folderExists = await _repository.DmsDocumentFolders.ExistsAsync(
						x => x.FolderName.Trim().ToLower() == entityForCreate.FolderName.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (folderExists)
			throw new DuplicateRecordException("DmsDocumentFolder", "FolderName");

		_logger.LogInformation("Creating new folder. Name: {FolderName}, Time: {Time}",
						entityForCreate.FolderName, DateTime.UtcNow);

		var folder = MyMapper.JsonClone<DmsDocumentFolderDto, DmsDocumentFolder>(entityForCreate);

		await _repository.DmsDocumentFolders.CreateAsync(folder, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Folder could not be saved to the database.");

		_logger.LogInformation("Folder created successfully. ID: {FolderId}, Time: {Time}",
						folder.FolderId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentFolder, DmsDocumentFolderDto>(folder);
	}

	public async Task<DmsDocumentFolderDto> UpdateFolderAsync(int folderId, DmsDocumentFolderDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(DmsDocumentFolderDto));

		if (folderId != modelDto.FolderId)
			throw new BadRequestException(folderId.ToString(), nameof(DmsDocumentFolderDto));

		_logger.LogInformation("Updating folder. ID: {FolderId}, Time: {Time}", folderId, DateTime.UtcNow);

		var folderEntity = await _repository.DmsDocumentFolders
						.FirstOrDefaultAsync(x => x.FolderId == folderId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("DmsDocumentFolder", "FolderId", folderId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<DmsDocumentFolder, DmsDocumentFolderDto>(folderEntity, modelDto);
		_repository.DmsDocumentFolders.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("DmsDocumentFolder", "FolderId", folderId.ToString());

		_logger.LogInformation("Folder updated successfully. ID: {FolderId}, Time: {Time}",
						folderId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentFolder, DmsDocumentFolderDto>(updatedEntity);
	}

	public async Task<int> DeleteFolderAsync(int folderId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (folderId <= 0)
			throw new BadRequestException(folderId.ToString(), nameof(DmsDocumentFolderDto));

		_logger.LogInformation("Deleting folder. ID: {FolderId}, Time: {Time}", folderId, DateTime.UtcNow);

		var folderEntity = await _repository.DmsDocumentFolders
						.FirstOrDefaultAsync(x => x.FolderId == folderId, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentFolder", "FolderId", folderId.ToString());

		await _repository.DmsDocumentFolders.DeleteAsync(x => x.FolderId == folderId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("DmsDocumentFolder", "FolderId", folderId.ToString());

		_logger.LogInformation("Folder deleted successfully. ID: {FolderId}, Time: {Time}",
						folderId, DateTime.UtcNow);

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

//namespace Domain.Contracts.Services.DMS;

//internal sealed class DmsDocumentFolderService : IDmsDocumentFolderService
//{
//	private readonly IRepositoryManager _repository;
//	private readonly ILogger<DmsDocumentFolderService> _logger;
//	private readonly IConfiguration _configuration;

//	public DmsDocumentFolderService(IRepositoryManager repository, ILogger<DmsDocumentFolderService> logger, IConfiguration configuration)
//	{
//		_repository = repository;
//		_logger = logger;
//		_configuration = configuration;
//	}

//	public async Task<IEnumerable<DmsDocumentFolderDDL>> FoldersDDLAsync(bool trackChanges = false)
//	{
//		var folders = await _repository.DmsDocumentFolders.ListAsync(trackChanges: trackChanges);

//		if (!folders.Any())
//			throw new GenericListNotFoundException("DmsDocumentFolder");

//		var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentFolder, DmsDocumentFolderDDL>(folders);

//		return ddlDtos;
//	}

//	public async Task<GridEntity<DmsDocumentFolderDto>> SummaryGrid(GridOptions options)
//	{
//		string query = "SELECT * FROM DmsDocumentFolder";
//		string orderBy = "FolderName asc";

//		var gridEntity = await _repository.DmsDocumentFolders.AdoGridDataAsync<DmsDocumentFolderDto>(query, options, orderBy, "");

//		return gridEntity;
//	}

//	public async Task<string> CreateNewRecordAsync(DmsDocumentFolderDto modelDto)
//	{
//		if (modelDto.FolderId != 0)
//			throw new InvalidCreateOperationException("FolderId must be 0 when creating a new folder.");

//		bool isExist = await _repository.DmsDocumentFolders.ExistsAsync(x => x.FolderName.Trim().ToLower() == modelDto.FolderName.Trim().ToLower());
//		if (isExist) throw new DuplicateRecordException("DmsDocumentFolder", "FolderName");

//		var folder = MyMapper.JsonClone<DmsDocumentFolderDto, DmsDocumentFolder>(modelDto);

//		var createdId = await _repository.DmsDocumentFolders.CreateAndIdAsync(folder);
//		if (createdId == 0)
//			throw new InvalidCreateOperationException();

//		await _repository.SaveAsync();
//		_logger.LogWarning("New document folder created with Id: {FolderId}", createdId);

//		return OperationMessage.Success;
//	}

//	public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentFolderDto modelDto, bool trackChanges)
//	{
//		if (key <= 0 || key != modelDto.FolderId)
//			return "Invalid update attempt: key does not match the FolderId.";

//		bool exists = await _repository.DmsDocumentFolders.ExistsAsync(x => x.FolderId == key);
//		if (!exists)
//			return "Update failed: folder not found.";

//		var folder = MyMapper.JsonClone<DmsDocumentFolderDto, DmsDocumentFolder>(modelDto);

//		_repository.DmsDocumentFolders.Update(folder);
//		await _repository.SaveAsync();
//		_logger.LogWarning("Folder with Id: {FolderId} updated.", key);

//		return OperationMessage.Success;
//	}

//	public async Task<string> DeleteRecordAsync(int key, DmsDocumentFolderDto modelDto)
//	{
//		if (modelDto == null)
//			throw new BadRequestException(nameof(DmsDocumentFolderDto));

//		if (key != modelDto.FolderId)
//			throw new BadRequestException(key.ToString(), nameof(DmsDocumentFolderDto));

//		var folder = await _repository.DmsDocumentFolders.FirstOrDefaultAsync(x => x.FolderId == key, false);

//		if (folder == null)
//			throw new NotFoundException("DmsDocumentFolder", "FolderId", key.ToString());

//		await _repository.DmsDocumentFolders.DeleteAsync(x => x.FolderId == key, true);
//		await _repository.SaveAsync();

//		_logger.LogWarning("Folder with Id: {FolderId} deleted.", key);

//		return OperationMessage.Success;
//	}

//	public async Task<string> SaveOrUpdate(int key, DmsDocumentFolderDto modelDto)
//	{
//		if (modelDto.FolderId == 0 && key == 0)
//		{
//			bool isExist = await _repository.DmsDocumentFolders.ExistsAsync(x => x.FolderName.Trim().ToLower() == modelDto.FolderName.Trim().ToLower());
//			if (isExist) throw new DuplicateRecordException("DmsDocumentFolder", "FolderName");

//			var newFolder = MyMapper.JsonClone<DmsDocumentFolderDto, DmsDocumentFolder>(modelDto);

//			var createdId = await _repository.DmsDocumentFolders.CreateAndIdAsync(newFolder);
//			if (createdId == 0)
//				throw new InvalidCreateOperationException();

//			await _repository.SaveAsync();
//			_logger.LogWarning("New document folder created with Id: {FolderId}", createdId);
//			return OperationMessage.Success;
//		}
//		else if (key > 0 && key == modelDto.FolderId)
//		{
//			var exists = await _repository.DmsDocumentFolders.ExistsAsync(x => x.FolderId == key);
//			if (!exists)
//			{
//				var updateFolder = MyMapper.JsonClone<DmsDocumentFolderDto, DmsDocumentFolder>(modelDto);
//				_repository.DmsDocumentFolders.Update(updateFolder);
//				await _repository.SaveAsync();

//				_logger.LogWarning("Folder with Id: {FolderId} updated.", key);
//				return OperationMessage.Success;
//			}
//			else
//			{
//				return "Update failed: folder with this Id already exists.";
//			}
//		}
//		else
//		{
//			return "Invalid key and FolderId mismatch.";
//		}
//	}
//}
