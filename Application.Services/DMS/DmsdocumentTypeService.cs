using bdDevs.Shared.Constants;
using Domain.Contracts.Repositories;
﻿// DmsDocumentTypeService.cs
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
/// DMS Document Type service implementing business logic for document type management.
/// </summary>
internal sealed class DmsDocumentTypeService : IDmsDocumentTypeService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<DmsDocumentTypeService> _logger;
	private readonly IConfiguration _configuration;

	public DmsDocumentTypeService(IRepositoryManager repository, ILogger<DmsDocumentTypeService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<IEnumerable<DmsDocumentTypeDDL>> TypesDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching document types for dropdown list. Time: {Time}", DateTime.UtcNow);

		//var types = await _repository.DmsDocumentTypes.ListWithSelectAsync(trackChanges, cancellationToken);

		var typesDDL = await _repository.DmsDocumentTypes.ListWithSelectAsync(selector: x => new DmsDocumentTypeDDL
		{
			DocumentTypeId = x.DocumentTypeId,
			Name = x.Name
		},
				orderBy: x => x.Name, trackChanges, cancellationToken);
		if (!typesDDL.Any())
		{
			_logger.LogWarning("No document types found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentTypeDDL>();
		}

		//var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentType, DmsDocumentTypeDDL>(types);

		_logger.LogInformation("Document types fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						typesDDL.Count(), DateTime.UtcNow);

		return typesDDL;
	}

	public async Task<GridEntity<DmsDocumentTypeDto>> TypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query = "SELECT * FROM DmsDocumentType";
		const string orderBy = "Name ASC";

		_logger.LogInformation("Fetching document types summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.DmsDocumentTypes.AdoGridDataAsync<DmsDocumentTypeDto>(query, options, orderBy, "", cancellationToken);
	}

	public async Task<DmsDocumentTypeDto> CreateDocumentTypeAsync(DmsDocumentTypeDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(DmsDocumentTypeDto));

		if (entityForCreate.DocumentTypeId != 0)
			throw new InvalidCreateOperationException("DocumentTypeId must be 0 for new record.");

		bool typeExists = await _repository.DmsDocumentTypes.ExistsAsync(
						x => x.Name.Trim().ToLower() == entityForCreate.Name.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (typeExists)
			throw new DuplicateRecordException("DmsDocumentType", "Name");

		_logger.LogInformation("Creating new document type. Name: {Name}, Time: {Time}",
						entityForCreate.Name, DateTime.UtcNow);

		var type = MyMapper.JsonClone<DmsDocumentTypeDto, DmsDocumentType>(entityForCreate);

		await _repository.DmsDocumentTypes.CreateAsync(type, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Document type could not be saved to the database.");

		_logger.LogInformation("Document type created successfully. ID: {DocumentTypeId}, Time: {Time}",
						type.DocumentTypeId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentType, DmsDocumentTypeDto>(type);
	}

	public async Task<DmsDocumentTypeDto> UpdateDocumentTypeAsync(int documentTypeId, DmsDocumentTypeDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(DmsDocumentTypeDto));

		if (documentTypeId != modelDto.DocumentTypeId)
			throw new BadRequestException(documentTypeId.ToString(), nameof(DmsDocumentTypeDto));

		_logger.LogInformation("Updating document type. ID: {DocumentTypeId}, Time: {Time}", documentTypeId, DateTime.UtcNow);

		var typeEntity = await _repository.DmsDocumentTypes
						.FirstOrDefaultAsync(x => x.DocumentTypeId == documentTypeId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("DmsDocumentType", "DocumentTypeId", documentTypeId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<DmsDocumentType, DmsDocumentTypeDto>(typeEntity, modelDto);
		_repository.DmsDocumentTypes.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("DmsDocumentType", "DocumentTypeId", documentTypeId.ToString());

		_logger.LogInformation("Document type updated successfully. ID: {DocumentTypeId}, Time: {Time}",
						documentTypeId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentType, DmsDocumentTypeDto>(updatedEntity);
	}

	public async Task<int> DeleteDocumentTypeAsync(int documentTypeId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (documentTypeId <= 0)
			throw new BadRequestException(documentTypeId.ToString(), nameof(DmsDocumentTypeDto));

		_logger.LogInformation("Deleting document type. ID: {DocumentTypeId}, Time: {Time}", documentTypeId, DateTime.UtcNow);

		var typeEntity = await _repository.DmsDocumentTypes
						.FirstOrDefaultAsync(x => x.DocumentTypeId == documentTypeId, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentType", "DocumentTypeId", documentTypeId.ToString());

		await _repository.DmsDocumentTypes.DeleteAsync(x => x.DocumentTypeId == documentTypeId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("DmsDocumentType", "DocumentTypeId", documentTypeId.ToString());

		_logger.LogInformation("Document type deleted successfully. ID: {DocumentTypeId}, Time: {Time}",
						documentTypeId, DateTime.UtcNow);

		return affected;
	}

	public async Task<DmsDocumentTypeDto> DocumentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching document type. ID: {DocumentTypeId}, Time: {Time}", id, DateTime.UtcNow);

		var type = await _repository.DmsDocumentTypes
						.FirstOrDefaultAsync(x => x.DocumentTypeId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentType", "DocumentTypeId", id.ToString());

		_logger.LogInformation("Document type fetched successfully. ID: {DocumentTypeId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentType, DmsDocumentTypeDto>(type);
	}

	public async Task<IEnumerable<DmsDocumentTypeDto>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all document types. Time: {Time}", DateTime.UtcNow);

		var types = await _repository.DmsDocumentTypes.DocumentTypesAsync(trackChanges, cancellationToken);

		if (!types.Any())
		{
			_logger.LogWarning("No document types found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentTypeDto>();
		}

		var typesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<DmsDocumentType, DmsDocumentTypeDto>(types);

		_logger.LogInformation("Document types fetched successfully. Count: {Count}, Time: {Time}",
						typesDto.Count(), DateTime.UtcNow);

		return typesDto;
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

//internal sealed class DmsDocumentTypeService : IDmsDocumentTypeService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<DmsDocumentTypeService> _logger;
//  private readonly IConfiguration _configuration;

//  public DmsDocumentTypeService(IRepositoryManager repository, ILogger<DmsDocumentTypeService> logger, IConfiguration configuration)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//  }

//  public async Task<IEnumerable<DmsDocumentTypeDDL>> TypesDDLAsync(bool trackChanges = false)
//  {
//    var types = await _repository.DmsDocumentTypes.ListAsync(trackChanges:trackChanges);

//    if (!types.Any())
//      throw new GenericListNotFoundException("DmsDocumentType");

//    var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentType, DmsDocumentTypeDDL>(types);
//    return ddlDtos;
//  }

//  public async Task<GridEntity<DmsDocumentTypeDto>> SummaryGrid(GridOptions options)
//  {
//    string query = "SELECT * FROM DmsDocumentType";
//    string orderBy = "Name asc";

//    var gridEntity = await _repository.DmsDocumentTypes.GridData<DmsDocumentTypeDto>(query, options, orderBy, "");

//    return gridEntity;
//  }

//  public async Task<string> CreateNewRecordAsync(DmsDocumentTypeDto modelDto)
//  {
//    if (modelDto.DocumentTypeId != 0)
//      throw new InvalidCreateOperationException("DocumentTypeId must be 0 when creating a new document type.");

//    bool isExist = await _repository.DmsDocumentTypes.ExistsAsync(x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());
//    if (isExist) throw new DuplicateRecordException("DmsDocumentType", "Name");

//    var type = MyMapper.JsonClone<DmsDocumentTypeDto, DmsDocumentType>(modelDto);

//    var createdId = await _repository.DmsDocumentTypes.CreateAndIdAsync(type);
//    if (createdId == 0)
//      throw new InvalidCreateOperationException();

//    await _repository.SaveAsync();
//    _logger.LogWarning("New document type created with Id: {CreatedId}", createdId);

//    return OperationMessage.Success;
//  }

//  public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentTypeDto modelDto, bool trackChanges)
//  {
//    if (key <= 0 || key != modelDto.DocumentTypeId)
//      return "Invalid update attempt: key does not match the DocumentTypeId.";

//    bool exists = await _repository.DmsDocumentTypes.ExistsAsync(x => x.DocumentTypeId == key);
//    if (!exists)
//      return "Update failed: document type not found.";

//    var type = MyMapper.JsonClone<DmsDocumentTypeDto, DmsDocumentType>(modelDto);

//    _repository.DmsDocumentTypes.Update(type);
//    await _repository.SaveAsync();
//    _logger.LogWarning("Document type with Id: {DocumentTypeId} updated.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> DeleteRecordAsync(int key, DmsDocumentTypeDto modelDto)
//  {
//    if (modelDto == null)
//      throw new BadRequestException(nameof(DmsDocumentTypeDto));

//    if (key != modelDto.DocumentTypeId)
//      throw new BadRequestException(key.ToString(), nameof(DmsDocumentTypeDto));

//    var type = await _repository.DmsDocumentTypes.FirstOrDefaultAsync(x => x.DocumentTypeId == key, false);

//    if (type == null)
//      throw new NotFoundException("DmsDocumentType", "DocumentTypeId", key.ToString());

//    await _repository.DmsDocumentTypes.DeleteAsync(x => x.DocumentTypeId == key, true);
//    await _repository.SaveAsync();

//    _logger.LogWarning("Document type with Id: {DocumentTypeId} deleted.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> SaveOrUpdate(int key, DmsDocumentTypeDto modelDto)
//  {
//    if (modelDto.DocumentTypeId == 0 && key == 0)
//    {
//      bool isExist = await _repository.DmsDocumentTypes.ExistsAsync(x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());
//      if (isExist) throw new DuplicateRecordException("DmsDocumentType", "Name");

//      var newType = MyMapper.JsonClone<DmsDocumentTypeDto, DmsDocumentType>(modelDto);

//      var createdId = await _repository.DmsDocumentTypes.CreateAndIdAsync(newType);
//      if (createdId == 0)
//        throw new InvalidCreateOperationException();

//      await _repository.SaveAsync();
//      _logger.LogWarning("New document type created with Id: {CreatedId}", createdId);
//      return OperationMessage.Success;
//    }
//    else if (key > 0 && key == modelDto.DocumentTypeId)
//    {
//      var exists = await _repository.DmsDocumentTypes.ExistsAsync(x => x.DocumentTypeId == key);
//      if (!exists)
//      {
//        var updateType = MyMapper.JsonClone<DmsDocumentTypeDto, DmsDocumentType>(modelDto);
//        _repository.DmsDocumentTypes.Update(updateType);
//        await _repository.SaveAsync();

//        _logger.LogWarning($"document type with Id: {key} updated.");
//        return OperationMessage.Success;
//      }
//      else
//      {
//        return "Update failed: document type with this Id already exists.";
//      }
//    }
//    else
//    {
//      return "Invalid key and DocumentTypeId mismatch.";
//    }
//  }
//}