using bdDevs.Shared.Constants;
using Domain.Contracts.Repositories;
﻿// DmsDocumentTagService.cs
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
/// DMS Document Tag service implementing business logic for tag management.
/// </summary>
internal sealed class DmsDocumentTagService : IDmsDocumentTagService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<DmsDocumentTagService> _logger;
	private readonly IConfiguration _configuration;

	public DmsDocumentTagService(IRepositoryManager repository, ILogger<DmsDocumentTagService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	public async Task<IEnumerable<DmsDocumentTagDDL>> TagsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching tags for dropdown list. Time: {Time}", DateTime.UtcNow);

		var tags = await _repository.DmsDocumentTags.ListWithSelectAsync(selector: x => new DmsDocumentTagDDL
		{
			TagId = x.TagId,
			Name = x.DocumentTagName
		}, orderBy: x => x.DocumentTagName, trackChanges, cancellationToken);

		if (!tags.Any())
		{
			_logger.LogWarning("No tags found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentTagDDL>();
		}

		//var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentTag, DmsDocumentTagDDL>(tags);

		_logger.LogInformation("Tags fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						tags.Count(), DateTime.UtcNow);

		return tags;
	}

	public async Task<GridEntity<DmsDocumentTagDto>> TagsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string query = "SELECT * FROM DmsDocumentTag";
		const string orderBy = "DocumentTagName ASC";

		_logger.LogInformation("Fetching tags summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.DmsDocumentTags.AdoGridDataAsync<DmsDocumentTagDto>(query, options, orderBy, "", cancellationToken);
	}

	public async Task<DmsDocumentTagDto> CreateTagAsync(DmsDocumentTagDto entityForCreate, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(DmsDocumentTagDto));

		if (entityForCreate.TagId != 0)
			throw new InvalidCreateOperationException("TagId must be 0 for new record.");

		bool tagExists = await _repository.DmsDocumentTags.ExistsAsync(
						x => x.DocumentTagName.Trim().ToLower() == entityForCreate.Name.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (tagExists)
			throw new DuplicateRecordException("DmsDocumentTag", "Name");

		_logger.LogInformation("Creating new tag. Name: {Name}, Time: {Time}",
						entityForCreate.Name, DateTime.UtcNow);

		var tag = MyMapper.JsonClone<DmsDocumentTagDto, DmsDocumentTag>(entityForCreate);

		await _repository.DmsDocumentTags.CreateAsync(tag, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Tag could not be saved to the database.");

		_logger.LogInformation("Tag created successfully. ID: {TagId}, Time: {Time}",
						tag.TagId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentTag, DmsDocumentTagDto>(tag);
	}

	public async Task<DmsDocumentTagDto> UpdateTagAsync(int tagId, DmsDocumentTagDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(DmsDocumentTagDto));

		if (tagId != modelDto.TagId)
			throw new BadRequestException(tagId.ToString(), nameof(DmsDocumentTagDto));

		_logger.LogInformation("Updating tag. ID: {TagId}, Time: {Time}", tagId, DateTime.UtcNow);

		var tagEntity = await _repository.DmsDocumentTags
						.FirstOrDefaultAsync(x => x.TagId == tagId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("DmsDocumentTag", "TagId", tagId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<DmsDocumentTag, DmsDocumentTagDto>(tagEntity, modelDto);
		_repository.DmsDocumentTags.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("DmsDocumentTag", "TagId", tagId.ToString());

		_logger.LogInformation("Tag updated successfully. ID: {TagId}, Time: {Time}",
						tagId, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentTag, DmsDocumentTagDto>(updatedEntity);
	}

	public async Task<int> DeleteTagAsync(int tagId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (tagId <= 0)
			throw new BadRequestException(tagId.ToString(), nameof(DmsDocumentTagDto));

		_logger.LogInformation("Deleting tag. ID: {TagId}, Time: {Time}", tagId, DateTime.UtcNow);

		var tagEntity = await _repository.DmsDocumentTags
						.FirstOrDefaultAsync(x => x.TagId == tagId, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentTag", "TagId", tagId.ToString());

		await _repository.DmsDocumentTags.DeleteAsync(x => x.TagId == tagId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("DmsDocumentTag", "TagId", tagId.ToString());

		_logger.LogInformation("Tag deleted successfully. ID: {TagId}, Time: {Time}",
						tagId, DateTime.UtcNow);

		return affected;
	}

	public async Task<DmsDocumentTagDto> TagAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching tag. ID: {TagId}, Time: {Time}", id, DateTime.UtcNow);

		var tag = await _repository.DmsDocumentTags
						.FirstOrDefaultAsync(x => x.TagId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("DmsDocumentTag", "TagId", id.ToString());

		_logger.LogInformation("Tag fetched successfully. ID: {TagId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<DmsDocumentTag, DmsDocumentTagDto>(tag);
	}

	public async Task<IEnumerable<DmsDocumentTagDto>> TagsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all tags. Time: {Time}", DateTime.UtcNow);

		var tags = await _repository.DmsDocumentTags.TagsAsync(trackChanges, cancellationToken);

		if (!tags.Any())
		{
			_logger.LogWarning("No tags found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<DmsDocumentTagDto>();
		}

		var tagsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<DmsDocumentTag, DmsDocumentTagDto>(tags);

		_logger.LogInformation("Tags fetched successfully. Count: {Count}, Time: {Time}",
						tagsDto.Count(), DateTime.UtcNow);

		return tagsDto;
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

//internal sealed class DmsDocumentTagService : IDmsDocumentTagService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<DmsDocumentTagService> _logger;
//  private readonly IConfiguration _configuration;

//  public DmsDocumentTagService(IRepositoryManager repository, ILogger<DmsDocumentTagService> logger, IConfiguration configuration)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//  }

//  public async Task<IEnumerable<DmsDocumentTagDDL>> TagsDDLAsync(bool trackChanges = false)
//  {
//    var tags = await _repository.DmsDocumentTags.ListAsync(trackChanges:trackChanges);

//    if (!tags.Any())
//      throw new GenericListNotFoundException("DmsDocumentTag");

//    var ddlDtos = MyMapper.JsonCloneIEnumerableToList<DmsDocumentTag, DmsDocumentTagDDL>(tags);
//    return ddlDtos;
//  }

//  public async Task<GridEntity<DmsDocumentTagDto>> SummaryGrid(GridOptions options)
//  {
//    string query = "SELECT * FROM DmsDocumentTag";
//    string orderBy = "Name asc";

//    var gridEntity = await _repository.DmsDocumentTags.GridData<DmsDocumentTagDto>(query, options, orderBy, "");

//    return gridEntity;
//  }

//  public async Task<string> CreateNewRecordAsync(DmsDocumentTagDto modelDto)
//  {
//    if (modelDto.TagId != 0)
//      throw new InvalidCreateOperationException("TagId must be 0 when creating a new tag.");

//    bool isExist = await _repository.DmsDocumentTags.ExistsAsync(x => x.DocumentTagName.Trim().ToLower() == modelDto.Name.Trim().ToLower());
//    if (isExist) throw new DuplicateRecordException("DmsDocumentTag", "Name");

//    var tag = MyMapper.JsonClone<DmsDocumentTagDto, DmsDocumentTag>(modelDto);

//    var createdId = await _repository.DmsDocumentTags.CreateAndIdAsync(tag);
//    if (createdId == 0)
//      throw new InvalidCreateOperationException();

//    await _repository.SaveAsync();
//    _logger.LogWarning("New document tag created with Id: {CreatedId}", createdId);

//    return OperationMessage.Success;
//  }

//  public async Task<string> UpdateNewRecordAsync(int key, DmsDocumentTagDto modelDto, bool trackChanges)
//  {
//    if (key <= 0 || key != modelDto.TagId)
//      return "Invalid update attempt: key does not match the TagId.";

//    bool exists = await _repository.DmsDocumentTags.ExistsAsync(x => x.TagId == key);
//    if (!exists)
//      return "Update failed: tag not found.";

//    var tag = MyMapper.JsonClone<DmsDocumentTagDto, DmsDocumentTag>(modelDto);

//    _repository.DmsDocumentTags.Update(tag);
//    await _repository.SaveAsync();
//    _logger.LogWarning("Tag with Id: {TagId} updated.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> DeleteRecordAsync(int key, DmsDocumentTagDto modelDto)
//  {
//    if (modelDto == null)
//      throw new BadRequestException(nameof(DmsDocumentTagDto));

//    if (key != modelDto.TagId)
//      throw new BadRequestException(key.ToString(), nameof(DmsDocumentTagDto));

//    var tag = await _repository.DmsDocumentTags.FirstOrDefaultAsync(x => x.TagId == key, false);

//    if (tag == null)
//      throw new NotFoundException("DmsDocumentTag", "TagId", key.ToString());

//    await _repository.DmsDocumentTags.DeleteAsync(x => x.TagId == key, true);
//    await _repository.SaveAsync();

//    _logger.LogWarning("Tag with Id: {TagId} deleted.", key);

//    return OperationMessage.Success;
//  }

//  public async Task<string> SaveOrUpdate(int key, DmsDocumentTagDto modelDto)
//  {
//    if (modelDto.TagId == 0 && key == 0)
//    {
//      bool isExist = await _repository.DmsDocumentTags.ExistsAsync(x => x.DocumentTagName.Trim().ToLower() == modelDto.Name.Trim().ToLower());
//      if (isExist) throw new DuplicateRecordException("DmsDocumentTag", "Name");

//      var newTag = MyMapper.JsonClone<DmsDocumentTagDto, DmsDocumentTag>(modelDto);

//      var createdId = await _repository.DmsDocumentTags.CreateAndIdAsync(newTag);
//      if (createdId == 0)
//        throw new InvalidCreateOperationException();

//      await _repository.SaveAsync();
//      _logger.LogWarning("New document tag created with Id: {CreatedId}", createdId);
//      return OperationMessage.Success;
//    }
//    else if (key > 0 && key == modelDto.TagId)
//    {
//      var exists = await _repository.DmsDocumentTags.ExistsAsync(x => x.TagId == key);
//      if (!exists)
//      {
//        var updateTag = MyMapper.JsonClone<DmsDocumentTagDto, DmsDocumentTag>(modelDto);
//        _repository.DmsDocumentTags.Update(updateTag);
//        await _repository.SaveAsync();

//        _logger.LogWarning("Tag with Id: {TagId} updated.", key);
//        return OperationMessage.Success;
//      }
//      else
//      {
//        return "Update failed: tag with this Id already exists.";
//      }
//    }
//    else
//    {
//      return "Invalid key and TagId mismatch.";
//    }
//  }
//}