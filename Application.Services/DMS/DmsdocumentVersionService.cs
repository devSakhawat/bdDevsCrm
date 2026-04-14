using bdDevs.Shared.Constants;
using Domain.Contracts.Repositories;
﻿// DmsDocumentVersionService.cs
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
/// DMS Document Version service implementing business logic for version management.
/// </summary>
internal sealed class DmsDocumentVersionService : IDmsDocumentVersionService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<DmsDocumentVersionService> _logger;
	private readonly IConfiguration _configuration;

	public DmsDocumentVersionService(IRepositoryManager repository, ILogger<DmsDocumentVersionService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<IEnumerable<DmsDocumentVersionDDL>> VersionsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching document versions for dropdown list. Time: {Time}", DateTime.UtcNow);

		var versionsDDL = await _repository.DmsDocumentVersions.ListWithSelectAsync(selector: x => new DmsDocumentVersionDDL
		{
			VersionId = x.VersionId,
			VersionNumber = x.VersionNumber
		},
				orderBy: x => x.VersionNumber, trackChanges, cancellationToken);
		if (!versionsDDL.Any())
		{
			_logger.LogWarning("No document versions found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentVersionDDL>();
		}

		//var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentVersion, DmsDocumentVersionDDL>(versions);

		_logger.LogInformation("Document versions fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						versionsDDL.Count(), DateTime.UtcNow);

		return versionsDDL;
	}

	public async Task<GridEntity<DmsDocumentVersionDto>> VersionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query = "SELECT * FROM DmsDocumentVersion";
		const string orderBy = "VersionNumber DESC";

		_logger.LogInformation("Fetching document versions summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.DmsDocumentVersions.AdoGridDataAsync<DmsDocumentVersionDto>(query, options, orderBy, "", cancellationToken);
	}

	public async Task<DmsDocumentVersionDto> CreateDocumentVersionAsync(DmsDocumentVersionDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(DmsDocumentVersionDto));

		if (entityForCreate.VersionId != 0)
			throw new InvalidCreateOperationException("VersionId must be 0 for new record.");

		_logger.LogInformation("Creating new document version. Time: {Time}", DateTime.UtcNow);

		var version = MyMapper.JsonClone<DmsDocumentVersionDto, DmsDocumentVersion>(entityForCreate);

		await _repository.DmsDocumentVersions.CreateAsync(version, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Document version could not be saved to the database.");

		_logger.LogInformation("Document version created successfully. ID: {VersionId}, Time: {Time}",
						version.VersionId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentVersion, DmsDocumentVersionDto>(version);
	}

	public async Task<DmsDocumentVersionDto> UpdateDocumentVersionAsync(int versionId, DmsDocumentVersionDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(DmsDocumentVersionDto));

		if (versionId != modelDto.VersionId)
			throw new BadRequestException(versionId.ToString(), nameof(DmsDocumentVersionDto));

		_logger.LogInformation("Updating document version. ID: {VersionId}, Time: {Time}", versionId, DateTime.UtcNow);

		var versionEntity = await _repository.DmsDocumentVersions
						.FirstOrDefaultAsync(x => x.VersionId == versionId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("DmsDocumentVersion", "VersionId", versionId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<DmsDocumentVersion, DmsDocumentVersionDto>(versionEntity, modelDto);
		_repository.DmsDocumentVersions.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("DmsDocumentVersion", "VersionId", versionId.ToString());

		_logger.LogInformation("Document version updated successfully. ID: {VersionId}, Time: {Time}",
						versionId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentVersion, DmsDocumentVersionDto>(updatedEntity);
	}

	public async Task<int> DeleteDocumentVersionAsync(int versionId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (versionId <= 0)
			throw new BadRequestException(versionId.ToString(), nameof(DmsDocumentVersionDto));

		_logger.LogInformation("Deleting document version. ID: {VersionId}, Time: {Time}", versionId, DateTime.UtcNow);

		var versionEntity = await _repository.DmsDocumentVersions
						.FirstOrDefaultAsync(x => x.VersionId == versionId, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentVersion", "VersionId", versionId.ToString());

		await _repository.DmsDocumentVersions.DeleteAsync(x => x.VersionId == versionId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("DmsDocumentVersion", "VersionId", versionId.ToString());

		_logger.LogInformation("Document version deleted successfully. ID: {VersionId}, Time: {Time}",
						versionId, DateTime.UtcNow);

		return affected;
	}

	public async Task<DmsDocumentVersionDto> DocumentVersionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching document version. ID: {VersionId}, Time: {Time}", id, DateTime.UtcNow);

		var version = await _repository.DmsDocumentVersions
						.FirstOrDefaultAsync(x => x.VersionId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentVersion", "VersionId", id.ToString());

		_logger.LogInformation("Document version fetched successfully. ID: {VersionId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentVersion, DmsDocumentVersionDto>(version);
	}

	public async Task<IEnumerable<DmsDocumentVersionDto>> DocumentVersionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all document versions. Time: {Time}", DateTime.UtcNow);

		var versions = await _repository.DmsDocumentVersions.DocumentVersionsAsync(trackChanges, cancellationToken);

		if (!versions.Any())
		{
			_logger.LogWarning("No document versions found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentVersionDto>();
		}

		var versionsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<DmsDocumentVersion, DmsDocumentVersionDto>(versions);

		_logger.LogInformation("Document versions fetched successfully. Count: {Count}, Time: {Time}",
						versionsDto.Count(), DateTime.UtcNow);

		return versionsDto;
	}
}


//using Domain.Entities.Entities.DMS;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.DMS;
//using Domain.Exceptions;
//using Domain.Contracts.Repositories;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Domain.Contracts.Services.DMS;

//internal sealed class DmsDocumentVersionService : IDmsDocumentVersionService
//{
//	private readonly IRepositoryManager _repository;
//	private readonly ILogger<DmsDocumentVersionService> _logger;
//	private readonly IConfiguration _configuration;

//	public DmsDocumentVersionService(IRepositoryManager repository, ILogger<DmsDocumentVersionService> logger, IConfiguration configuration)
//	{
//		_repository = repository;
//		_logger = logger;
//		_configuration = configuration;
//	}

//	public async Task<IEnumerable<DmsDocumentVersionDDL>> VersionsDDLAsync(bool trackChanges = false)
//	{
//		var versions = await _repository.DmsDocumentVersions.ListAsync(trackChanges: trackChanges);

//		if (!versions.Any())
//			throw new GenericListNotFoundException("DmsDocumentVersion");

//		var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentVersion, DmsDocumentVersionDDL>(versions);

//		return ddlDtos;
//	}

//	public async Task<GridEntity<DmsDocumentVersionDto>> SummaryGrid(GridOptions options)
//	{
//		string query = "SELECT * FROM DmsDocumentVersion";
//		string orderBy = "VersionNumber desc";

//		var gridEntity = await _repository.DmsDocumentVersions.GridData<DmsDocumentVersionDto>(query, options, orderBy, "");

//		return gridEntity;
//	}

//	public async Task<string> CreateNewRecordAsync(DmsDocumentVersionDto modelDto)
//	{
//		if (modelDto.VersionId != 0)
//			throw new InvalidCreateOperationException("VersionId must be 0 when creating a new document version.");

//		var version = MyMapper.JsonClone<DmsDocumentVersionDto, DmsDocumentVersion>(modelDto);

//		var createdId = await _repository.DmsDocumentVersions.CreateAndIdAsync(version);
//		if (createdId == 0)
//			throw new InvalidCreateOperationException();

//		await _repository.SaveAsync();
//		_logger.LogWarning("New document version created with Id: {CreatedId}", createdId);

//		return OperationMessage.Success;
//	}

//	public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentVersionDto modelDto, bool trackChanges)
//	{
//		if (key <= 0 || key != modelDto.VersionId)
//			return "Invalid update attempt: key does not match the VersionId.";

//		bool exists = await _repository.DmsDocumentVersions.ExistsAsync(x => x.VersionId == key);
//		if (!exists)
//			return "Update failed: document version not found.";

//		var version = MyMapper.JsonClone<DmsDocumentVersionDto, DmsDocumentVersion>(modelDto);

//		_repository.DmsDocumentVersions.Update(version);
//		await _repository.SaveAsync();
//		_logger.LogWarning("Document version with Id: {VersionId} updated.", key);

//		return OperationMessage.Success;
//	}

//	public async Task<string> DeleteRecordAsync(int key, DmsDocumentVersionDto modelDto)
//	{
//		if (modelDto == null)
//			throw new BadRequestException(nameof(DmsDocumentVersionDto));

//		if (key != modelDto.VersionId)
//			throw new BadRequestException(key.ToString(), nameof(DmsDocumentVersionDto));

//		var version = await _repository.DmsDocumentVersions.FirstOrDefaultAsync(x => x.VersionId == key, false);

//		if (version == null)
//			throw new NotFoundException("DmsDocumentVersion", "VersionId", key.ToString());

//		await _repository.DmsDocumentVersions.DeleteAsync(x => x.VersionId == key, true);
//		await _repository.SaveAsync();

//		_logger.LogWarning("Document version with Id: {VersionId} deleted.", key);

//		return OperationMessage.Success;
//	}

//	public async Task<string> SaveOrUpdate(int key, DmsDocumentVersionDto modelDto)
//	{
//		if (modelDto.VersionId == 0 && key == 0)
//		{
//			var newVersion = MyMapper.JsonClone<DmsDocumentVersionDto, DmsDocumentVersion>(modelDto);

//			var createdId = await _repository.DmsDocumentVersions.CreateAndIdAsync(newVersion);
//			if (createdId == 0)
//				throw new InvalidCreateOperationException();

//			await _repository.SaveAsync();
//			_logger.LogWarning("New document version created with Id: {CreatedId}", createdId);
//			return OperationMessage.Success;
//		}
//		else if (key > 0 && key == modelDto.VersionId)
//		{
//			var exists = await _repository.DmsDocumentVersions.ExistsAsync(x => x.VersionId == key);
//			if (!exists)
//			{
//				var updateVersion = MyMapper.JsonClone<DmsDocumentVersionDto, DmsDocumentVersion>(modelDto);
//				_repository.DmsDocumentVersions.Update(updateVersion);
//				await _repository.SaveAsync();

//				_logger.LogWarning("Document version with Id: {VersionId} updated.", key);
//				return OperationMessage.Success;
//			}
//			else
//			{
//				return "Update failed: document version with this Id already exists.";
//			}
//		}
//		else
//		{
//			return "Invalid key and VersionId mismatch.";
//		}
//	}
//}
