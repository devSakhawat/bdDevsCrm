using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.ServiceContract.DMS;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.DMS;

internal sealed class DmsDocumentTagMapService : IDmsDocumentTagMapService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<DmsDocumentTagMapService> _logger;
  private readonly IConfiguration _configuration;

  public DmsDocumentTagMapService(IRepositoryManager repository, ILogger<DmsDocumentTagMapService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  //public async Task<IEnumerable<DmsDocumentTagMapDDL>> TagMapsDDLAsync(bool trackChanges = false)
  //{
  //  var tagMaps = await _repository.DmsDocumentTagMaps.AllAsync(trackChanges);

  //  if (!tagMaps.Any())
  //    throw new GenericListNotFoundException("DmsDocumentTagMap");

  //  var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentTagMap, DmsDocumentTagMapDDL>(tagMaps);

  //  return ddlDtos;
  //}

  //public async Task<GridEntity<DmsDocumentTagMapDto>> SummaryGrid(GridOptions options)
  //{
  //  string query = "SELECT * FROM DmsDocumentTagMap";
  //  string orderBy = "Id asc";

  //  var gridEntity = await _repository.DmsDocumentTagMaps.GridData<DmsDocumentTagMapDto>(query, options, orderBy, "");

  //  return gridEntity;
  //}

  //public async Task<string> CreateNewRecordAsync(DmsDocumentTagMapDto modelDto)
  //{
  //  if (modelDto.Id != 0)
  //    throw new InvalidCreateOperationException("Id must be 0 when creating a new tag map.");

  //  // Optional: check for duplicates, e.g. same documentId & TagId combo
  //  bool isExist = await _repository.DmsDocumentTagMaps.ExistsAsync(x =>
  //      x.documentId == modelDto.documentId && x.TagId == modelDto.TagId);
  //  if (isExist) throw new DuplicateRecordException("DmsDocumentTagMap", "documentId+TagId");

  //  var tagMap = MyMapper.JsonClone<DmsDocumentTagMapDto, DmsDocumentTagMap>(modelDto);

  //  var createdId = await _repository.DmsDocumentTagMaps.CreateAndIdAsync(tagMap);
  //  if (createdId == 0)
  //    throw new InvalidCreateOperationException();

  //  await _repository.SaveAsync();
  //  _logger.LogWarn($"New document tag map created with Id: {createdId}");

  //  return OperationMessage.Success;
  //}

  //public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentTagMapDto modelDto, bool trackChanges)
  //{
  //  if (key <= 0 || key != modelDto.Id)
  //    return "Invalid update attempt: key does not match the Id.";

  //  bool exists = await _repository.DmsDocumentTagMaps.ExistsAsync(x => x.Id == key);
  //  if (!exists)
  //    return "Update failed: document tag map not found.";

  //  var tagMap = MyMapper.JsonClone<DmsDocumentTagMapDto, DmsDocumentTagMap>(modelDto);

  //  _repository.DmsDocumentTagMaps.Update(tagMap);
  //  await _repository.SaveAsync();

  //  _logger.LogWarn($"document tag map with Id: {key} updated.");

  //  return OperationMessage.Success;
  //}

  //public async Task<string> DeleteRecordAsync(int key, DmsDocumentTagMapDto modelDto)
  //{
  //  if (modelDto == null)
  //    throw new BadRequestException(nameof(DmsDocumentTagMapDto));

  //  if (key != modelDto.Id)
  //    throw new BadRequestException(key.ToString(), nameof(DmsDocumentTagMapDto));

  //  var tagMap = await _repository.DmsDocumentTagMaps.FirstOrDefaultAsync(x => x.Id == key, false);

  //  if (tagMap == null)
  //    throw new NotFoundException("DmsDocumentTagMap", "Id", key.ToString());

  //  await _repository.DmsDocumentTagMaps.DeleteAsync(x => x.Id == key, true);
  //  await _repository.SaveAsync();

  //  _logger.LogWarn($"document tag map with Id: {key} deleted.");

  //  return OperationMessage.Success;
  //}

  //public async Task<string> SaveOrUpdate(int key, DmsDocumentTagMapDto modelDto)
  //{
  //  if (modelDto.Id == 0 && key == 0)
  //  {
  //    bool isExist = await _repository.DmsDocumentTagMaps.ExistsAsync(x =>
  //        x.documentId == modelDto.documentId && x.TagId == modelDto.TagId);
  //    if (isExist) throw new DuplicateRecordException("DmsDocumentTagMap", "documentId+TagId");

  //    var newTagMap = MyMapper.JsonClone<DmsDocumentTagMapDto, DmsDocumentTagMap>(modelDto);

  //    var createdId = await _repository.DmsDocumentTagMaps.CreateAndIdAsync(newTagMap);
  //    if (createdId == 0)
  //      throw new InvalidCreateOperationException();

  //    await _repository.SaveAsync();
  //    _logger.LogWarn($"New document tag map created with Id: {createdId}");
  //    return OperationMessage.Success;
  //  }
  //  else if (key > 0 && key == modelDto.Id)
  //  {
  //    bool exists = await _repository.DmsDocumentTagMaps.ExistsAsync(x => x.Id == key);
  //    if (!exists)
  //    {
  //      var updateTagMap = MyMapper.JsonClone<DmsDocumentTagMapDto, DmsDocumentTagMap>(modelDto);
  //      _repository.DmsDocumentTagMaps.Update(updateTagMap);
  //      await _repository.SaveAsync();

  //      _logger.LogWarn($"document tag map with Id: {key} updated.");
  //      return OperationMessage.Success;
  //    }
  //    else
  //    {
  //      return "Update failed: document tag map with this Id already exists.";
  //    }
  //  }
  //  else
  //  {
  //    return "Invalid key and Id mismatch.";
  //  }
  //}
}