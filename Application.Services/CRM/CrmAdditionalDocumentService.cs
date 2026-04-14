// CrmAdditionalDocumentsService.cs
using bdDevCRM.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.ServicesContract.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevCRM.Shared.Exceptions;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM Additional Documents service implementing business logic for document management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmAdditionalDocumentsService : ICrmAdditionalDocumentService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmAdditionalDocumentsService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmAdditionalDocumentsService"/> with required dependencies.
	/// </summary>
	public CrmAdditionalDocumentsService(IRepositoryManager repository, ILogger<CrmAdditionalDocumentsService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new additional document record.
	/// </summary>
	public async Task<AdditionalDocumentDto> CreateAdditionalDocumentAsync(AdditionalDocumentDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(AdditionalDocumentDto));

		if (entityForCreate.AdditionalDocumentId != 0)
			throw new InvalidCreateOperationException("AdditionalDocumentId must be 0 for new record.");

		_logger.LogInformation("Creating new additional document. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var documentEntity = MyMapper.JsonClone<AdditionalDocumentDto, CrmAdditionalDocument>(entityForCreate);
		documentEntity.CreatedDate = DateTime.UtcNow;
		documentEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmAdditionalDocuments.CreateAsync(documentEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Additional document could not be saved to the database.");

		_logger.LogInformation("Additional document created successfully. ID: {AdditionalDocumentId}, Time: {Time}",
						documentEntity.AdditionalDocumentId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmAdditionalDocument, AdditionalDocumentDto>(documentEntity);
	}

	/// <summary>
	/// Updates an existing additional document record.
	/// </summary>
	public async Task<AdditionalDocumentDto> UpdateAdditionalDocumentAsync(int additionalDocumentId, AdditionalDocumentDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(AdditionalDocumentDto));

		if (additionalDocumentId != modelDto.AdditionalDocumentId)
			throw new BadRequestException(additionalDocumentId.ToString(), nameof(AdditionalDocumentDto));

		_logger.LogInformation("Updating additional document. ID: {AdditionalDocumentId}, Time: {Time}", additionalDocumentId, DateTime.UtcNow);

		var documentEntity = await _repository.CrmAdditionalDocuments
						.FirstOrDefaultAsync(x => x.AdditionalDocumentId == additionalDocumentId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("AdditionalDocument", "AdditionalDocumentId", additionalDocumentId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmAdditionalDocument, AdditionalDocumentDto>(documentEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmAdditionalDocuments.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("AdditionalDocument", "AdditionalDocumentId", additionalDocumentId.ToString());

		_logger.LogInformation("Additional document updated successfully. ID: {AdditionalDocumentId}, Time: {Time}",
						additionalDocumentId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmAdditionalDocument, AdditionalDocumentDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an additional document record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteAdditionalDocumentAsync(int additionalDocumentId, AdditionalDocumentDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(AdditionalDocumentDto));

		if (additionalDocumentId != modelDto.AdditionalDocumentId)
			throw new BadRequestException(additionalDocumentId.ToString(), nameof(AdditionalDocumentDto));

		_logger.LogInformation("Deleting additional document. ID: {AdditionalDocumentId}, Time: {Time}", additionalDocumentId, DateTime.UtcNow);

		var documentEntity = await _repository.CrmAdditionalDocuments
						.FirstOrDefaultAsync(x => x.AdditionalDocumentId == additionalDocumentId, trackChanges, cancellationToken)
						?? throw new NotFoundException("AdditionalDocument", "AdditionalDocumentId", additionalDocumentId.ToString());

		await _repository.CrmAdditionalDocuments.DeleteAsync(x => x.AdditionalDocumentId == additionalDocumentId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("AdditionalDocument", "AdditionalDocumentId", additionalDocumentId.ToString());

		_logger.LogInformation("Additional document deleted successfully. ID: {AdditionalDocumentId}, Time: {Time}",
						additionalDocumentId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single additional document record by its ID.
	/// </summary>
	public async Task<AdditionalDocumentDto> AdditionalDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching additional document. ID: {AdditionalDocumentId}, Time: {Time}", id, DateTime.UtcNow);

		var document = await _repository.CrmAdditionalDocuments
						.FirstOrDefaultAsync(x => x.AdditionalDocumentId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmAdditionalDocument", "AdditionalDocumentId", id.ToString());

		_logger.LogInformation("Additional document fetched successfully. ID: {AdditionalDocumentId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmAdditionalDocument, AdditionalDocumentDto>(document);
	}

	/// <summary>
	/// Retrieves all additional document records from the database.
	/// </summary>
	public async Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all additional documents. Time: {Time}", DateTime.UtcNow);

		var documents = await _repository.CrmAdditionalDocuments.CrmAdditionalDocumentsAsync(trackChanges, cancellationToken);

		if (!documents.Any())
		{
			_logger.LogWarning("No additional documents found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<AdditionalDocumentDto>();
		}

		var documentsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalDocument, AdditionalDocumentDto>(documents);

		_logger.LogInformation("Additional documents fetched successfully. Count: {Count}, Time: {Time}",
						documentsDto.Count(), DateTime.UtcNow);

		return documentsDto;
	}

	/// <summary>
	/// Retrieves active additional document records from the database.
	/// </summary>
	public async Task<IEnumerable<AdditionalDocumentDto>> ActiveAdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active additional documents. Time: {Time}", DateTime.UtcNow);

		var documents = await _repository.CrmAdditionalDocuments.CrmAdditionalDocumentsAsync(trackChanges, cancellationToken);

		if (!documents.Any())
		{
			_logger.LogWarning("No active additional documents found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<AdditionalDocumentDto>();
		}

		var documentsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalDocument, AdditionalDocumentDto>(documents);

		_logger.LogInformation("Active additional documents fetched successfully. Count: {Count}, Time: {Time}",
						documentsDto.Count(), DateTime.UtcNow);

		return documentsDto;
	}

	/// <summary>
	/// Retrieves additional documents by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("AdditionalDocumentsByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching additional documents for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var documents = await _repository.CrmAdditionalDocuments.CrmAdditionalDocumentsByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!documents.Any())
		{
			_logger.LogWarning("No additional documents found for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
			return Enumerable.Empty<AdditionalDocumentDto>();
		}

		var documentsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalDocument, AdditionalDocumentDto>(documents);

		_logger.LogInformation("Additional documents fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, documentsDto.Count(), DateTime.UtcNow);

		return documentsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all additional documents suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<AdditionalDocumentDto>> AdditionalDocumentForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching additional documents for dropdown list. Time: {Time}", DateTime.UtcNow);

		var documents = await _repository.CrmAdditionalDocuments.CrmAdditionalDocumentsAsync(trackChanges, cancellationToken);
		//var documents = await _repository.CrmAdditionalDocuments.GetActiveAdditionalDocumentsAsync(trackChanges, cancellationToken);

		if (!documents.Any())
		{
			_logger.LogWarning("No additional documents found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<AdditionalDocumentDto>();
		}

		var documentsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalDocument, AdditionalDocumentDto>(documents);

		_logger.LogInformation("Additional documents fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						documentsDto.Count(), DateTime.UtcNow);

		return documentsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all additional documents.
	/// </summary>
	public async Task<GridEntity<AdditionalDocumentDto>> AdditionalDocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    [AdditionalDocumentId],
                    [DocumentTitle],
                    [DocumentPath],
                    [DocumentName],
                    [RecordType],
                    [CreatedDate],
                    [CreatedBy],
                    [UpdatedDate],
                    [UpdatedBy],
                    [ApplicantId],
                    doc.FilePath AS AttachedDocument
                FROM [dbDevCRM].[dbo].[CrmAdditionalDocument]
                LEFT JOIN DmsDocument doc ON doc.ReferenceEntityId = CrmAdditionalDocument.ApplicantId
                AND TRIM(doc.ReferenceEntityType) = 'AdditionalDocument'";

		const string orderBy = "AdditionalDocumentId ASC";

		_logger.LogInformation("Fetching additional documents summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmAdditionalDocuments.AdoGridDataAsync<AdditionalDocumentDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}


//using bdDevCRM.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevCRM.s.CRM;
//using bdDevCRM.ServicesContract.CRM;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using bdDevCRM.Shared.Exceptions;
//using Application.Shared.Grid;
//using bdDevCRM.Utilities.OthersLibrary;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace bdDevCRM.Services.CRM;

///// <summary>
///// CrmAdditionalDocument service implementing business logic for CrmAdditionalDocument management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmAdditionalDocumentService : ICrmAdditionalDocumentService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmAdditionalDocumentService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmAdditionalDocumentService(IRepositoryManager repository, ILogger<CrmAdditionalDocumentService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmAdditionalDocument records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmAdditionalDocumentDto>> CrmAdditionalDocumentSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmAdditionalDocument summary grid");

//        string query = "SELECT * FROM CrmAdditionalDocument";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmAdditionalDocuments.AdoGridDataAsync<CrmAdditionalDocumentDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmAdditionalDocument records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmAdditionalDocumentDto>> CrmAdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmAdditionalDocument records");

//        var records = await _repository.CrmAdditionalDocuments.CrmAdditionalDocumentsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmAdditionalDocument records found");
//            return Enumerable.Empty<CrmAdditionalDocumentDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmAdditionalDocument, CrmAdditionalDocumentDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmAdditionalDocument record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmAdditionalDocumentDto> CrmAdditionalDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmAdditionalDocumentAsync called with invalid id: {CrmAdditionalDocumentId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmAdditionalDocument record with ID: {CrmAdditionalDocumentId}", id);

//        var record = await _repository.CrmAdditionalDocuments.CrmAdditionalDocumentAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmAdditionalDocument record not found with ID: {CrmAdditionalDocumentId}", id);
//            throw new NotFoundException("CrmAdditionalDocument", "CrmAdditionalDocumentId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmAdditionalDocument, CrmAdditionalDocumentDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmAdditionalDocument record asynchronously.
//    /// </summary>
//    public async Task<CrmAdditionalDocumentDto> CreateAsync(CrmAdditionalDocumentDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmAdditionalDocumentDto));

//        _logger.LogInformation("Creating new CrmAdditionalDocument record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmAdditionalDocuments.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmAdditionalDocument", "Name");

//        // Map and create
//        CrmAdditionalDocument entity = MyMapper.JsonClone<CrmAdditionalDocumentDto, CrmAdditionalDocument>(modelDto);
//        modelDto.CrmAdditionalDocumentId = await _repository.CrmAdditionalDocuments.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmAdditionalDocument record created successfully with ID: {CrmAdditionalDocumentId}", modelDto.CrmAdditionalDocumentId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmAdditionalDocument record asynchronously.
//    /// </summary>
//    public async Task<CrmAdditionalDocumentDto> UpdateAsync(int key, CrmAdditionalDocumentDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmAdditionalDocumentDto));

//        if (key != modelDto.CrmAdditionalDocumentId)
//            throw new BadRequestException(key.ToString(), nameof(CrmAdditionalDocumentDto));

//        _logger.LogInformation("Updating CrmAdditionalDocument record with ID: {CrmAdditionalDocumentId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmAdditionalDocuments.ByIdAsync(
//            x => x.CrmAdditionalDocumentId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmAdditionalDocument", "CrmAdditionalDocumentId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmAdditionalDocuments.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmAdditionalDocumentId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmAdditionalDocument", "Name");

//        // Map and update
//        CrmAdditionalDocument entity = MyMapper.JsonClone<CrmAdditionalDocumentDto, CrmAdditionalDocument>(modelDto);
//        _repository.CrmAdditionalDocuments.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmAdditionalDocument record updated successfully: {CrmAdditionalDocumentId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmAdditionalDocument record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmAdditionalDocument record with ID: {CrmAdditionalDocumentId}", key);

//        var record = await _repository.CrmAdditionalDocuments.ByIdAsync(
//            x => x.CrmAdditionalDocumentId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmAdditionalDocument", "CrmAdditionalDocumentId", key.ToString());

//        await _repository.CrmAdditionalDocuments.DeleteAsync(x => x.CrmAdditionalDocumentId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmAdditionalDocument record deleted successfully: {CrmAdditionalDocumentId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmAdditionalDocument records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmAdditionalDocumentForDDLDto>> CrmAdditionalDocumentsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmAdditionalDocument records for dropdown list");

//        var records = await _repository.CrmAdditionalDocuments.ListWithSelectAsync(
//            x => new CrmAdditionalDocument
//            {
//                CrmAdditionalDocumentId = x.CrmAdditionalDocumentId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmAdditionalDocumentForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmAdditionalDocument, CrmAdditionalDocumentForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
