// CrmPermanentAddressService.cs
using bdDevCRM.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.ServicesContract.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Shared.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM Permanent Address service implementing business logic for permanent address management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmPermanentAddressService : ICrmPermanentAddressService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmPermanentAddressService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmPermanentAddressService"/> with required dependencies.
	/// </summary>
	public CrmPermanentAddressService(IRepositoryManager repository, ILogger<CrmPermanentAddressService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new permanent address record.
	/// </summary>
	public async Task<PermanentAddressDto> CreatePermanentAddressAsync(PermanentAddressDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(PermanentAddressDto));

		if (entityForCreate.PermanentAddressId != 0)
			throw new InvalidCreateOperationException("PermanentAddressId must be 0 for new record.");

		bool applicantExists = await _repository.CrmPermanentAddresses.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmPermanentAddress", "ApplicantId");

		_logger.LogInformation("Creating new permanent address. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var addressEntity = MyMapper.JsonClone<PermanentAddressDto, CrmPermanentAddress>(entityForCreate);
		addressEntity.CreatedDate = DateTime.UtcNow;
		addressEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmPermanentAddresses.CreateAsync(addressEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Permanent address could not be saved to the database.");

		_logger.LogInformation("Permanent address created successfully. ID: {PermanentAddressId}, Time: {Time}",
						addressEntity.PermanentAddressId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPermanentAddress, PermanentAddressDto>(addressEntity);
	}

	/// <summary>
	/// Updates an existing permanent address record.
	/// </summary>
	public async Task<PermanentAddressDto> UpdatePermanentAddressAsync(int permanentAddressId, PermanentAddressDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(PermanentAddressDto));

		if (permanentAddressId != modelDto.PermanentAddressId)
			throw new BadRequestException(permanentAddressId.ToString(), nameof(PermanentAddressDto));

		_logger.LogInformation("Updating permanent address. ID: {PermanentAddressId}, Time: {Time}", permanentAddressId, DateTime.UtcNow);

		var addressEntity = await _repository.CrmPermanentAddresses
						.FirstOrDefaultAsync(x => x.PermanentAddressId == permanentAddressId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmPermanentAddress", "PermanentAddressId", permanentAddressId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmPermanentAddress, PermanentAddressDto>(addressEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmPermanentAddresses.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmPermanentAddress", "PermanentAddressId", permanentAddressId.ToString());

		_logger.LogInformation("Permanent address updated successfully. ID: {PermanentAddressId}, Time: {Time}",
						permanentAddressId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPermanentAddress, PermanentAddressDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a permanent address record identified by the given ID.
	/// </summary>
	public async Task<int> DeletePermanentAddressAsync(int permanentAddressId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (permanentAddressId <= 0)
			throw new BadRequestException(permanentAddressId.ToString(), nameof(PermanentAddressDto));

		_logger.LogInformation("Deleting permanent address. ID: {PermanentAddressId}, Time: {Time}", permanentAddressId, DateTime.UtcNow);

		var addressEntity = await _repository.CrmPermanentAddresses
						.FirstOrDefaultAsync(x => x.PermanentAddressId == permanentAddressId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPermanentAddress", "PermanentAddressId", permanentAddressId.ToString());

		await _repository.CrmPermanentAddresses.DeleteAsync(x => x.PermanentAddressId == permanentAddressId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmPermanentAddress", "PermanentAddressId", permanentAddressId.ToString());

		_logger.LogInformation("Permanent address deleted successfully. ID: {PermanentAddressId}, Time: {Time}",
						permanentAddressId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single permanent address record by its ID.
	/// </summary>
	public async Task<PermanentAddressDto> PermanentAddressAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching permanent address. ID: {PermanentAddressId}, Time: {Time}", id, DateTime.UtcNow);

		var address = await _repository.CrmPermanentAddresses
						.FirstOrDefaultAsync(x => x.PermanentAddressId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPermanentAddress", "PermanentAddressId", id.ToString());

		_logger.LogInformation("Permanent address fetched successfully. ID: {PermanentAddressId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPermanentAddress, PermanentAddressDto>(address);
	}

	/// <summary>
	/// Retrieves all permanent address records from the database.
	/// </summary>
	public async Task<IEnumerable<PermanentAddressDto>> PermanentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all permanent addresses. Time: {Time}", DateTime.UtcNow);

		var addresses = await _repository.CrmPermanentAddresses
						.CrmPermanentAddresssAsync(trackChanges, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No permanent addresses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PermanentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPermanentAddress, PermanentAddressDto>(addresses);

		_logger.LogInformation("Permanent addresses fetched successfully. Count: {Count}, Time: {Time}",
						addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves active permanent address records from the database.
	/// </summary>
	public async Task<IEnumerable<PermanentAddressDto>> ActivePermanentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active permanent addresses. Time: {Time}", DateTime.UtcNow);

		var addresses = await _repository.CrmPermanentAddresses
						.CrmPermanentAddresssAsync(trackChanges, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No active permanent addresses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PermanentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPermanentAddress, PermanentAddressDto>(addresses);

		_logger.LogInformation("Active permanent addresses fetched successfully. Count: {Count}, Time: {Time}",
						addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves permanent address by the specified applicant ID.
	/// </summary>
	public async Task<PermanentAddressDto> PermanentAddressByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("PermanentAddressByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching permanent address for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var address = await _repository.CrmPermanentAddresses
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPermanentAddress", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("Permanent address fetched successfully. ID: {PermanentAddressId}, Time: {Time}",
						address.PermanentAddressId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPermanentAddress, PermanentAddressDto>(address);
	}

	/// <summary>
	/// Retrieves permanent addresses by the specified country ID.
	/// </summary>
	public async Task<IEnumerable<PermanentAddressDto>> PermanentAddressesByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (countryId <= 0)
		{
			_logger.LogWarning("PermanentAddressesByCountryIdAsync called with invalid countryId: {CountryId}", countryId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching permanent addresses for country ID: {CountryId}, Time: {Time}", countryId, DateTime.UtcNow);

		var addresses = await _repository.CrmPermanentAddresses
						.CrmPermanentAddresssByCountryIdAsync(countryId, trackChanges, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No permanent addresses found for country ID: {CountryId}, Time: {Time}", countryId, DateTime.UtcNow);
			return Enumerable.Empty<PermanentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPermanentAddress, PermanentAddressDto>(addresses);

		_logger.LogInformation("Permanent addresses fetched successfully for country ID: {CountryId}. Count: {Count}, Time: {Time}",
						countryId, addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all permanent addresses suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<PermanentAddressDto>> PermanentAddressForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching permanent addresses for dropdown list. Time: {Time}", DateTime.UtcNow);

		var addresses = await _repository.CrmPermanentAddresses
						.CrmPermanentAddresssAsync(false, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No permanent addresses found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PermanentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPermanentAddress, PermanentAddressDto>(addresses);

		_logger.LogInformation("Permanent addresses fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all permanent addresses.
	/// </summary>
	public async Task<GridEntity<PermanentAddressDto>> PermanentAddressesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    pa.PermanentAddressId,
                    pa.ApplicantId,
                    pa.Address,
                    pa.City,
                    pa.State,
                    pa.CountryId,
                    pa.CountryName,
                    pa.PostalCode,
                    pa.CreatedDate,
                    pa.CreatedBy,
                    pa.UpdatedDate,
                    pa.UpdatedBy,
                    app.ApplicationStatus,
                    c.CountryName AS CountryFullName
                FROM CrmPermanentAddress pa
                LEFT JOIN CrmApplication app ON pa.ApplicantId = app.ApplicationId
                LEFT JOIN Country c ON pa.CountryId = c.CountryId";

		const string orderBy = "pa.CreatedDate DESC";

		_logger.LogInformation("Fetching permanent addresses summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmPermanentAddresses.AdoGridDataAsync<PermanentAddressDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmPermanentAddress service implementing business logic for CrmPermanentAddress management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmPermanentAddressService : ICrmPermanentAddressService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmPermanentAddressService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmPermanentAddressService(IRepositoryManager repository, ILogger<CrmPermanentAddressService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmPermanentAddress records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmPermanentAddressDto>> CrmPermanentAddressSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmPermanentAddress summary grid");

//        string query = "SELECT * FROM CrmPermanentAddress";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmPermanentAddresss.AdoGridDataAsync<CrmPermanentAddressDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmPermanentAddress records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmPermanentAddressDto>> CrmPermanentAddresssAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmPermanentAddress records");

//        var records = await _repository.CrmPermanentAddresss.CrmPermanentAddresssAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmPermanentAddress records found");
//            return Enumerable.Empty<CrmPermanentAddressDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmPermanentAddress, CrmPermanentAddressDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmPermanentAddress record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmPermanentAddressDto> CrmPermanentAddressAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmPermanentAddressAsync called with invalid id: {CrmPermanentAddressId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmPermanentAddress record with ID: {CrmPermanentAddressId}", id);

//        var record = await _repository.CrmPermanentAddresss.CrmPermanentAddressAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmPermanentAddress record not found with ID: {CrmPermanentAddressId}", id);
//            throw new NotFoundException("CrmPermanentAddress", "CrmPermanentAddressId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmPermanentAddress, CrmPermanentAddressDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmPermanentAddress record asynchronously.
//    /// </summary>
//    public async Task<CrmPermanentAddressDto> CreateAsync(CrmPermanentAddressDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmPermanentAddressDto));

//        _logger.LogInformation("Creating new CrmPermanentAddress record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmPermanentAddresss.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmPermanentAddress", "Name");

//        // Map and create
//        CrmPermanentAddress entity = MyMapper.JsonClone<CrmPermanentAddressDto, CrmPermanentAddress>(modelDto);
//        modelDto.CrmPermanentAddressId = await _repository.CrmPermanentAddresss.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPermanentAddress record created successfully with ID: {CrmPermanentAddressId}", modelDto.CrmPermanentAddressId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmPermanentAddress record asynchronously.
//    /// </summary>
//    public async Task<CrmPermanentAddressDto> UpdateAsync(int key, CrmPermanentAddressDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmPermanentAddressDto));

//        if (key != modelDto.CrmPermanentAddressId)
//            throw new BadRequestException(key.ToString(), nameof(CrmPermanentAddressDto));

//        _logger.LogInformation("Updating CrmPermanentAddress record with ID: {CrmPermanentAddressId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmPermanentAddresss.ByIdAsync(
//            x => x.CrmPermanentAddressId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmPermanentAddress", "CrmPermanentAddressId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmPermanentAddresss.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmPermanentAddressId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmPermanentAddress", "Name");

//        // Map and update
//        CrmPermanentAddress entity = MyMapper.JsonClone<CrmPermanentAddressDto, CrmPermanentAddress>(modelDto);
//        _repository.CrmPermanentAddresss.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPermanentAddress record updated successfully: {CrmPermanentAddressId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmPermanentAddress record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmPermanentAddress record with ID: {CrmPermanentAddressId}", key);

//        var record = await _repository.CrmPermanentAddresss.ByIdAsync(
//            x => x.CrmPermanentAddressId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmPermanentAddress", "CrmPermanentAddressId", key.ToString());

//        await _repository.CrmPermanentAddresss.DeleteAsync(x => x.CrmPermanentAddressId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPermanentAddress record deleted successfully: {CrmPermanentAddressId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmPermanentAddress records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmPermanentAddressForDDLDto>> CrmPermanentAddresssForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmPermanentAddress records for dropdown list");

//        var records = await _repository.CrmPermanentAddresss.ListWithSelectAsync(
//            x => new CrmPermanentAddress
//            {
//                CrmPermanentAddressId = x.CrmPermanentAddressId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmPermanentAddressForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmPermanentAddress, CrmPermanentAddressForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
