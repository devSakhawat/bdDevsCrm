// CrmPresentAddressService.cs
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM Present Address service implementing business logic for present address management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmPresentAddressService : ICrmPresentAddressService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmPresentAddressService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmPresentAddressService"/> with required dependencies.
	/// </summary>
	public CrmPresentAddressService(IRepositoryManager repository, ILogger<CrmPresentAddressService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new present address record.
	/// </summary>
	public async Task<PresentAddressDto> CreatePresentAddressAsync(PresentAddressDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(PresentAddressDto));

		if (entityForCreate.PresentAddressId != 0)
			throw new InvalidCreateOperationException("PresentAddressId must be 0 for new record.");

		bool applicantExists = await _repository.CrmPresentAddresses.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmPresentAddress", "ApplicantId");

		_logger.LogInformation("Creating new present address. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var addressEntity = MyMapper.JsonClone<PresentAddressDto, CrmPresentAddress>(entityForCreate);
		addressEntity.CreatedDate = DateTime.UtcNow;
		addressEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmPresentAddresses.CreateAsync(addressEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Present address could not be saved to the database.");

		_logger.LogInformation("Present address created successfully. ID: {PresentAddressId}, Time: {Time}",
						addressEntity.PresentAddressId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPresentAddress, PresentAddressDto>(addressEntity);
	}

	/// <summary>
	/// Updates an existing present address record.
	/// </summary>
	public async Task<PresentAddressDto> UpdatePresentAddressAsync(int presentAddressId, PresentAddressDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(PresentAddressDto));

		if (presentAddressId != modelDto.PresentAddressId)
			throw new BadRequestException(presentAddressId.ToString(), nameof(PresentAddressDto));

		_logger.LogInformation("Updating present address. ID: {PresentAddressId}, Time: {Time}", presentAddressId, DateTime.UtcNow);

		var addressEntity = await _repository.CrmPresentAddresses
						.FirstOrDefaultAsync(x => x.PresentAddressId == presentAddressId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmPresentAddress", "PresentAddressId", presentAddressId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmPresentAddress, PresentAddressDto>(addressEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmPresentAddresses.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmPresentAddress", "PresentAddressId", presentAddressId.ToString());

		_logger.LogInformation("Present address updated successfully. ID: {PresentAddressId}, Time: {Time}",
						presentAddressId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPresentAddress, PresentAddressDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a present address record identified by the given ID.
	/// </summary>
	public async Task<int> DeletePresentAddressAsync(int presentAddressId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (presentAddressId <= 0)
			throw new BadRequestException(presentAddressId.ToString(), nameof(PresentAddressDto));

		_logger.LogInformation("Deleting present address. ID: {PresentAddressId}, Time: {Time}", presentAddressId, DateTime.UtcNow);

		var addressEntity = await _repository.CrmPresentAddresses
						.FirstOrDefaultAsync(x => x.PresentAddressId == presentAddressId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPresentAddress", "PresentAddressId", presentAddressId.ToString());

		await _repository.CrmPresentAddresses.DeleteAsync(x => x.PresentAddressId == presentAddressId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmPresentAddress", "PresentAddressId", presentAddressId.ToString());

		_logger.LogInformation("Present address deleted successfully. ID: {PresentAddressId}, Time: {Time}",
						presentAddressId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single present address record by its ID.
	/// </summary>
	public async Task<PresentAddressDto> PresentAddressAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching present address. ID: {PresentAddressId}, Time: {Time}", id, DateTime.UtcNow);

		var address = await _repository.CrmPresentAddresses
						.FirstOrDefaultAsync(x => x.PresentAddressId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPresentAddress", "PresentAddressId", id.ToString());

		_logger.LogInformation("Present address fetched successfully. ID: {PresentAddressId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPresentAddress, PresentAddressDto>(address);
	}

	/// <summary>
	/// Retrieves all present address records from the database.
	/// </summary>
	public async Task<IEnumerable<PresentAddressDto>> PresentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all present addresses. Time: {Time}", DateTime.UtcNow);

		var addresses = await _repository.CrmPresentAddresses
						.CrmPresentAddresssAsync(trackChanges, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No present addresses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PresentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPresentAddress, PresentAddressDto>(addresses);

		_logger.LogInformation("Present addresses fetched successfully. Count: {Count}, Time: {Time}",
						addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves active present address records from the database.
	/// </summary>
	public async Task<IEnumerable<PresentAddressDto>> ActivePresentAddressesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active present addresses. Time: {Time}", DateTime.UtcNow);

		var addresses = await _repository.CrmPresentAddresses
						.CrmPresentAddresssAsync(trackChanges, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No active present addresses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PresentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPresentAddress, PresentAddressDto>(addresses);

		_logger.LogInformation("Active present addresses fetched successfully. Count: {Count}, Time: {Time}",
						addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves present address by the specified applicant ID.
	/// </summary>
	public async Task<PresentAddressDto> PresentAddressByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("PresentAddressByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching present address for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var address = await _repository.CrmPresentAddresses
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPresentAddress", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("Present address fetched successfully. ID: {PresentAddressId}, Time: {Time}",
						address.PresentAddressId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPresentAddress, PresentAddressDto>(address);
	}

	/// <summary>
	/// Retrieves present addresses by the specified country ID.
	/// </summary>
	public async Task<IEnumerable<PresentAddressDto>> PresentAddressesByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (countryId <= 0)
		{
			_logger.LogWarning("PresentAddressesByCountryIdAsync called with invalid countryId: {CountryId}", countryId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching present addresses for country ID: {CountryId}, Time: {Time}", countryId, DateTime.UtcNow);

		var addresses = await _repository.CrmPresentAddresses
						.CrmPresentAddresssByCountryIdAsync(countryId, trackChanges, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No present addresses found for country ID: {CountryId}, Time: {Time}", countryId, DateTime.UtcNow);
			return Enumerable.Empty<PresentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPresentAddress, PresentAddressDto>(addresses);

		_logger.LogInformation("Present addresses fetched successfully for country ID: {CountryId}. Count: {Count}, Time: {Time}",
						countryId, addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all present addresses suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<PresentAddressDto>> PresentAddressForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching present addresses for dropdown list. Time: {Time}", DateTime.UtcNow);

		var addresses = await _repository.CrmPresentAddresses
						.CrmPresentAddresssAsync(false, cancellationToken);

		if (!addresses.Any())
		{
			_logger.LogWarning("No present addresses found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PresentAddressDto>();
		}

		var addressesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPresentAddress, PresentAddressDto>(addresses);

		_logger.LogInformation("Present addresses fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						addressesDto.Count(), DateTime.UtcNow);

		return addressesDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all present addresses.
	/// </summary>
	public async Task<GridEntity<PresentAddressDto>> PresentAddressesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    pra.PresentAddressId,
                    pra.ApplicantId,
                    pra.SameAsPermanentAddress,
                    pra.Address,
                    pra.City,
                    pra.State,
                    pra.CountryId,
                    pra.CountryName,
                    pra.PostalCode,
                    pra.CreatedDate,
                    pra.CreatedBy,
                    pra.UpdatedDate,
                    pra.UpdatedBy,
                    app.ApplicationStatus,
                    c.CountryName AS CountryFullName
                FROM CrmPresentAddress pra
                LEFT JOIN CrmApplication app ON pra.ApplicantId = app.ApplicationId
                LEFT JOIN Country c ON pra.CountryId = c.CountryId";

		const string orderBy = "pra.CreatedDate DESC";

		_logger.LogInformation("Fetching present addresses summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmPresentAddresses.AdoGridDataAsync<PresentAddressDto>(sql, options, orderBy, "", cancellationToken);
	}
}


//using Domain.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Contracts.Services.CRM;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Exceptions;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Application.Services.CRM;

///// <summary>
///// CrmPresentAddress service implementing business logic for CrmPresentAddress management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmPresentAddressService : ICrmPresentAddressService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmPresentAddressService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmPresentAddressService(IRepositoryManager repository, ILogger<CrmPresentAddressService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmPresentAddress records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmPresentAddressDto>> CrmPresentAddressSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmPresentAddress summary grid");

//        string query = "SELECT * FROM CrmPresentAddress";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmPresentAddresss.AdoGridDataAsync<CrmPresentAddressDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmPresentAddress records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmPresentAddressDto>> CrmPresentAddresssAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmPresentAddress records");

//        var records = await _repository.CrmPresentAddresss.CrmPresentAddresssAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmPresentAddress records found");
//            return Enumerable.Empty<CrmPresentAddressDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmPresentAddress, CrmPresentAddressDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmPresentAddress record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmPresentAddressDto> CrmPresentAddressAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmPresentAddressAsync called with invalid id: {CrmPresentAddressId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmPresentAddress record with ID: {CrmPresentAddressId}", id);

//        var record = await _repository.CrmPresentAddresss.CrmPresentAddressAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmPresentAddress record not found with ID: {CrmPresentAddressId}", id);
//            throw new NotFoundException("CrmPresentAddress", "CrmPresentAddressId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmPresentAddress, CrmPresentAddressDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmPresentAddress record asynchronously.
//    /// </summary>
//    public async Task<CrmPresentAddressDto> CreateAsync(CrmPresentAddressDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmPresentAddressDto));

//        _logger.LogInformation("Creating new CrmPresentAddress record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmPresentAddresss.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmPresentAddress", "Name");

//        // Map and create
//        CrmPresentAddress entity = MyMapper.JsonClone<CrmPresentAddressDto, CrmPresentAddress>(modelDto);
//        modelDto.CrmPresentAddressId = await _repository.CrmPresentAddresss.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPresentAddress record created successfully with ID: {CrmPresentAddressId}", modelDto.CrmPresentAddressId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmPresentAddress record asynchronously.
//    /// </summary>
//    public async Task<CrmPresentAddressDto> UpdateAsync(int key, CrmPresentAddressDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmPresentAddressDto));

//        if (key != modelDto.CrmPresentAddressId)
//            throw new BadRequestException(key.ToString(), nameof(CrmPresentAddressDto));

//        _logger.LogInformation("Updating CrmPresentAddress record with ID: {CrmPresentAddressId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmPresentAddresss.ByIdAsync(
//            x => x.CrmPresentAddressId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmPresentAddress", "CrmPresentAddressId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmPresentAddresss.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmPresentAddressId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmPresentAddress", "Name");

//        // Map and update
//        CrmPresentAddress entity = MyMapper.JsonClone<CrmPresentAddressDto, CrmPresentAddress>(modelDto);
//        _repository.CrmPresentAddresss.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPresentAddress record updated successfully: {CrmPresentAddressId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmPresentAddress record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmPresentAddress record with ID: {CrmPresentAddressId}", key);

//        var record = await _repository.CrmPresentAddresss.ByIdAsync(
//            x => x.CrmPresentAddressId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmPresentAddress", "CrmPresentAddressId", key.ToString());

//        await _repository.CrmPresentAddresss.DeleteAsync(x => x.CrmPresentAddressId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPresentAddress record deleted successfully: {CrmPresentAddressId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmPresentAddress records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmPresentAddressForDDLDto>> CrmPresentAddresssForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmPresentAddress records for dropdown list");

//        var records = await _repository.CrmPresentAddresss.ListWithSelectAsync(
//            x => new CrmPresentAddress
//            {
//                CrmPresentAddressId = x.CrmPresentAddressId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmPresentAddressForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmPresentAddress, CrmPresentAddressForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
