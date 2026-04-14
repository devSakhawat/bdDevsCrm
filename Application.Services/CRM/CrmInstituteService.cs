using bdDevs.Shared.Constants;
// CrmInstituteService.cs
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
/// CRM Institute service implementing business logic for institute management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmInstituteService : ICrmInstituteService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmInstituteService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmInstituteService"/> with required dependencies.
	/// </summary>
	public CrmInstituteService(IRepositoryManager repository, ILogger<CrmInstituteService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new institute record.
	/// </summary>
	public async Task<CrmInstituteDto> CreateInstituteAsync(CrmInstituteDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(CrmInstituteDto));

		if (entityForCreate.InstituteId != 0)
			throw new InvalidCreateOperationException("InstituteId must be 0 for new record.");

		bool instituteExists = await _repository.CrmInstitutes.ExistsAsync(
						x => x.InstituteName != null
								&& x.InstituteName.Trim().ToLower() == entityForCreate.InstituteName!.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (instituteExists)
			throw new DuplicateRecordException("CrmInstitute", "InstituteName");

		_logger.LogInformation("Creating new institute. Name: {InstituteName}, Time: {Time}",
						entityForCreate.InstituteName, DateTime.UtcNow);

		var instituteEntity = MyMapper.JsonClone<CrmInstituteDto, CrmInstitute>(entityForCreate);
		//instituteEntity.CreatedDate = DateTime.UtcNow;
		//instituteEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmInstitutes.CreateAsync(instituteEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Institute could not be saved to the database.");

		_logger.LogInformation("Institute created successfully. ID: {InstituteId}, Time: {Time}",
						instituteEntity.InstituteId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmInstitute, CrmInstituteDto>(instituteEntity);
	}

	/// <summary>
	/// Updates an existing institute record.
	/// </summary>
	public async Task<CrmInstituteDto> UpdateInstituteAsync(int instituteId, CrmInstituteDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmInstituteDto));

		if (instituteId != modelDto.InstituteId)
			throw new BadRequestException(instituteId.ToString(), nameof(CrmInstituteDto));

		_logger.LogInformation("Updating institute. ID: {InstituteId}, Time: {Time}", instituteId, DateTime.UtcNow);

		var instituteEntity = await _repository.CrmInstitutes
						.FirstOrDefaultAsync(x => x.InstituteId == instituteId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmInstitute", "InstituteId", instituteId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmInstitute, CrmInstituteDto>(instituteEntity, modelDto);
		//updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmInstitutes.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmInstitute", "InstituteId", instituteId.ToString());

		_logger.LogInformation("Institute updated successfully. ID: {InstituteId}, Time: {Time}",
						instituteId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmInstitute, CrmInstituteDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an institute record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteInstituteAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (instituteId <= 0)
			throw new BadRequestException(instituteId.ToString(), nameof(CrmInstituteDto));

		_logger.LogInformation("Deleting institute. ID: {InstituteId}, Time: {Time}", instituteId, DateTime.UtcNow);

		var instituteEntity = await _repository.CrmInstitutes
						.FirstOrDefaultAsync(x => x.InstituteId == instituteId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmInstitute", "InstituteId", instituteId.ToString());

		await _repository.CrmInstitutes.DeleteAsync(x => x.InstituteId == instituteId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmInstitute", "InstituteId", instituteId.ToString());

		_logger.LogInformation("Institute deleted successfully. ID: {InstituteId}, Time: {Time}",
						instituteId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single institute record by its ID.
	/// </summary>
	public async Task<CrmInstituteDto> InstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching institute. ID: {InstituteId}, Time: {Time}", id, DateTime.UtcNow);

		var institute = await _repository.CrmInstitutes
						.FirstOrDefaultAsync(x => x.InstituteId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmInstitute", "InstituteId", id.ToString());

		_logger.LogInformation("Institute fetched successfully. ID: {InstituteId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmInstitute, CrmInstituteDto>(institute);
	}

	/// <summary>
	/// Retrieves all institute records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteDto>> InstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all institutes. Time: {Time}", DateTime.UtcNow);

		var institutes = await _repository.CrmInstitutes.CrmInstitutesAsync(trackChanges, cancellationToken);

		if (!institutes.Any())
		{
			_logger.LogWarning("No institutes found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmInstituteDto>();
		}

		var institutesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmInstitute, CrmInstituteDto>(institutes);

		_logger.LogInformation("Institutes fetched successfully. Count: {Count}, Time: {Time}",
						institutesDto.Count(), DateTime.UtcNow);

		return institutesDto;
	}

	/// <summary>
	/// Retrieves active institute records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteDto>> ActiveInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active institutes. Time: {Time}", DateTime.UtcNow);

		var institutes = await _repository.CrmInstitutes
						.CrmInstitutesAsync(trackChanges, cancellationToken);

		if (!institutes.Any())
		{
			_logger.LogWarning("No active institutes found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmInstituteDto>();
		}

		var institutesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmInstitute, CrmInstituteDto>(institutes);

		_logger.LogInformation("Active institutes fetched successfully. Count: {Count}, Time: {Time}",
						institutesDto.Count(), DateTime.UtcNow);

		return institutesDto;
	}

	/// <summary>
	/// Retrieves institutes by the specified country ID.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteDDLDto>> InstitutesByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (countryId <= 0)
		{
			_logger.LogWarning("InstitutesByCountryIdAsync called with invalid countryId: {CountryId}", countryId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching institutes for country ID: {CountryId}, Time: {Time}", countryId, DateTime.UtcNow);

		var institutes = await _repository.CrmInstitutes
						.ListByWhereWithSelectAsync(
										expression: x => x.CountryId == countryId,
										selector: x => new CrmInstituteDDLDto
										{
											InstituteId = x.InstituteId,
											InstituteName = x.InstituteName
										},
										orderBy: x => x.InstituteName,
										trackChanges: trackChanges,
										cancellationToken: cancellationToken);

		if (!institutes.Any())
		{
			_logger.LogWarning("No institutes found for country ID: {CountryId}, Time: {Time}", countryId, DateTime.UtcNow);
			return Enumerable.Empty<CrmInstituteDDLDto>();
		}

		_logger.LogInformation("Institutes fetched successfully for country ID: {CountryId}. Count: {Count}, Time: {Time}",
						countryId, institutes.Count(), DateTime.UtcNow);

		return institutes;
	}

	/// <summary>
	/// Retrieves an institute by its name.
	/// </summary>
	public async Task<CrmInstituteDto> InstituteByNameAsync(string name, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(name))
		{
			_logger.LogWarning("InstituteByNameAsync called with null or whitespace name");
			throw new BadRequestException(nameof(name));
		}

		_logger.LogInformation("Fetching institute by name: {Name}, Time: {Time}", name, DateTime.UtcNow);

		var institute = await _repository.CrmInstitutes
						.FirstOrDefaultAsync(x => x.InstituteName == name, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmInstitute", "InstituteName", name);

		_logger.LogInformation("Institute fetched successfully. ID: {InstituteId}, Time: {Time}",
						institute.InstituteId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmInstitute, CrmInstituteDto>(institute);
	}

	/// <summary>
	/// Retrieves a lightweight list of all institutes suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteDDLDto>> InstituteForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching institutes for dropdown list. Time: {Time}", DateTime.UtcNow);

		var institutesDDl = await _repository.CrmInstitutes
					.ListWithSelectAsync(
									selector: x => new CrmInstituteDDLDto
									{
										InstituteId = x.InstituteId,
										InstituteName = x.InstituteName
									},
									orderBy: x => x.InstituteName,
									trackChanges: false,
									cancellationToken: cancellationToken);

		if (!institutesDDl.Any())
		{
			_logger.LogWarning("No institutes found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmInstituteDDLDto>();
		}

		_logger.LogInformation("Institutes fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						institutesDDl.Count(), DateTime.UtcNow);

		return institutesDDl;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all institutes.
	/// </summary>
	public async Task<GridEntity<CrmInstituteDto>> InstitutesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    InstituteId,
                    CRMInstitute.CountryId,
                    InstituteName,
                    Campus,
                    Website,
                    MonthlyLivingCost,
                    FundsRequirementforVisa,
                    ApplicationFee,
                    CRMInstitute.CurrencyId,
                    IsLanguageMandatory,
                    LanguagesRequirement,
                    InstitutionalBenefits,
                    PartTimeWorkDetails,
                    ScholarshipsPolicy,
                    InstitutionStatusNotes,
                    CRMInstitute.InstituteTypeId,
                    InstituteCode,
                    InstituteEmail,
                    InstituteAddress,
                    InstitutePhoneNO,
                    InstituteMobileNo,
                    CRMInstitute.Status,
                    CrmCountry.CountryName,
                    CrmCurrencyInfo.CurrencyName,
                    CRMInstituteType.InstituteTypeName,
                    docLogo.FilePath AS InstitutionLogo,
                    docProspectus.FilePath AS InstitutionProspectus
                FROM CRMInstitute
                LEFT JOIN CrmCountry ON CRMInstitute.CountryId = CrmCountry.CountryId
                LEFT JOIN CrmCurrencyInfo ON CRMInstitute.CurrencyId = CrmCurrencyInfo.CurrencyId
                LEFT JOIN CRMInstituteType ON CRMInstitute.InstituteTypeId = CRMInstituteType.InstituteTypeId
                LEFT JOIN DMSDocument docLogo ON CRMInstitute.InstituteId = docLogo.ReferenceEntityId AND docLogo.SystemTag = 'InstitutionLogo'
                LEFT JOIN DMSDocument docProspectus ON CRMInstitute.InstituteId = docProspectus.ReferenceEntityId AND docProspectus.SystemTag = 'InstitutionProspectus'";

		const string orderBy = "InstituteName ASC";

		_logger.LogInformation("Fetching institutes summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmInstitutes.AdoGridDataAsync<CrmInstituteDto>(sql, options, orderBy, "", cancellationToken);
	}
}

//using Domain.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using Domain.Contracts.Services.CRM;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Exceptions;
//using Domain.Contracts.Repositories;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Application.Services.CRM;

///// <summary>
///// CrmInstitute service implementing business logic for CrmInstitute management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmInstituteService : ICrmInstituteService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<CrmInstituteService> _logger;
//  private readonly IConfiguration _configuration;
//  private readonly IHttpContextAccessor _httpContextAccessor;

//  public CrmInstituteService(IRepositoryManager repository, ILogger<CrmInstituteService> logger, IConfiguration configuration , IHttpContextAccessor httpContextAccessor)
//  {
//    _repository = repository;
//    _logger = logger;
//    _configuration = configuration;
//    _httpContextAccessor = httpContextAccessor;
//  }

//  /// <summary>
//  /// Retrieves paginated summary grid of CrmInstitute records asynchronously.
//  /// </summary>
//  public async Task<GridEntity<CrmInstituteDto>> CrmInstituteSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//  {
//    _logger.LogInformation("Fetching CrmInstitute summary grid");

//    string query = "SELECT * FROM CrmInstitute";
//    string orderBy = "Name ASC";

//    var gridEntity = await _repository.CrmInstitutes.AdoGridDataAsync<CrmInstituteDto>(query, options, orderBy, "", cancellationToken);
//    return gridEntity;
//  }

//  /// <summary>
//  /// Retrieves all CrmInstitute records asynchronously.
//  /// </summary>
//  public async Task<IEnumerable<CrmInstituteDto>> CrmInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//  {
//    _logger.LogInformation("Fetching all CrmInstitute records");

//    var records = await _repository.CrmInstitutes.InstitutesAsync(trackChanges, cancellationToken);

//    if (!records.Any())
//    {
//      _logger.LogWarning("No CrmInstitute records found");
//      return Enumerable.Empty<CrmInstituteDto>();
//    }

//    var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmInstitute, CrmInstituteDto>(records);
//    return recordDtos;
//  }

//  /// <summary>
//  /// Retrieves a CrmInstitute record by ID asynchronously.
//  /// </summary>
//  public async Task<CrmInstituteDto> CrmInstituteAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//  {
//    if (id <= 0)
//    {
//      _logger.LogWarning("CrmInstituteAsync called with invalid id: {InstituteId}", id);
//      throw new BadRequestException("Invalid request!");
//    }

//    _logger.LogInformation("Fetching CrmInstitute record with ID: {InstituteId}", id);

//    var record = await _repository.CrmInstitutes.InstituteAsync(id, trackChanges, cancellationToken);

//    if (record == null)
//    {
//      _logger.LogWarning("CrmInstitute record not found with ID: {InstituteId}", id);
//      throw new NotFoundException("CrmInstitute", "InstituteId", id.ToString());
//    }

//    var recordDto = MyMapper.JsonClone<CrmInstitute, CrmInstituteDto>(record);
//    return recordDto;
//  }

//  /// <summary>
//  /// Creates a new CrmInstitute record asynchronously.
//  /// </summary>
//  public async Task<CrmInstituteDto> CreateAsync(CrmInstituteDto modelDto ,UsersDto currentUser ,CancellationToken cancellation = default)
//  {
//    if (modelDto == null)
//      throw new BadRequestException(nameof(CrmInstituteDto));

//    _logger.LogInformation("Creating new CrmInstitute record");

//    // Check for duplicate record
//    bool recordExists = await _repository.CrmInstitutes.ExistsAsync(
//        x => x.InstituteName.Trim().ToLower() == modelDto.InstituteName.Trim().ToLower());

//    if (recordExists)
//      throw new DuplicateRecordException("CrmInstitute", "Name");

//    // Map and create
//    CrmInstitute entity = MyMapper.JsonClone<CrmInstituteDto, CrmInstitute>(modelDto);
//    modelDto.InstituteId = await _repository.CrmInstitutes.CreateAndIdAsync(entity ,cancellation);
//    await _repository.SaveAsync();

//    _logger.LogInformation("CrmInstitute record created successfully with ID: {InstituteId}", modelDto.InstituteId);

//    return modelDto;
//  }

//  //public async Task<CrmInstituteDto> CreateNewRecordAsync(CrmInstituteDto dto, UsersDto currentUser ,CancellationToken cancellation)
//  //{
//  //  if (dto.InstituteId != 0)
//  //    throw new InvalidCreateOperationException("InstituteId must be 0.");

//  //  //var instituteObj = await _repository.CrmInstitutes.FirstOrDefaultAsync(x => x.InstituteName.Trim().ToLower() == dto.InstituteName!.Trim().ToLower());
//  //  bool dup = await _repository.CrmInstitutes.ExistsAsync(x => x.InstituteName != null && x.InstituteName.Trim().ToLower().Equals(dto.InstituteName!.Trim().ToLower()));

//  //  if (dup) throw new DuplicateRecordException("CrmInstitute", "InstituteName");

//  //  var entity = MyMapper.JsonClone<CrmInstituteDto, CrmInstitute>(dto);
//  //  dto.InstituteId = await _repository.CrmInstitutes.CreateAndIdAsync(entity , cancellation);

//  //  return dto;
//  //}

//  /// <summary>
//  /// Updates an existing CrmInstitute record asynchronously.
//  /// </summary>
//  public async Task<CrmInstituteDto> UpdateAsync(int key, CrmInstituteDto modelDto ,bool trackChanges ,CancellationToken cancellationToken = default)
//  {
//    if (modelDto == null)
//      throw new BadRequestException(nameof(CrmInstituteDto));

//    if (key != modelDto.InstituteId)
//      throw new BadRequestException(key.ToString(), nameof(CrmInstituteDto));

//    _logger.LogInformation("Updating CrmInstitute record with ID: {InstituteId}", key);

//    // Check if record exists
//    var existingRecord = await _repository.CrmInstitutes.ByIdAsync(
//        x => x.InstituteId == key, trackChanges: false);

//    if (existingRecord == null)
//      throw new NotFoundException("CrmInstitute", "InstituteId", key.ToString());

//    // Check for duplicate InstituteName (excluding current record)
//    bool duplicateExists = await _repository.CrmInstitutes.ExistsAsync(
//        x => x.InstituteName.Trim().ToLower() == modelDto.InstituteName.Trim().ToLower()
//             && x.InstituteId != key);

//    if (duplicateExists)
//      throw new DuplicateRecordException("CrmInstitute", "InstituteName");

//    // Map and update
//    CrmInstitute entity = MyMapper.JsonClone<CrmInstituteDto, CrmInstitute>(modelDto);
//    _repository.CrmInstitutes.UpdateByState(entity);
//    await _repository.SaveAsync();

//    _logger.LogInformation("CrmInstitute record updated successfully: {InstituteId}", key);

//    return modelDto;
//  }

//  /// <summary>
//  /// Deletes a CrmInstitute record by ID asynchronously.
//  /// </summary>
//  public async Task<string> DeleteAsync(int key, bool trackChanges ,CancellationToken cancellationToken = default)
//  {
//    if (key <= 0)
//      throw new BadRequestException("Invalid request!");

//    _logger.LogInformation("Deleting CrmInstitute record with ID: {InstituteId}", key);

//    var record = await _repository.CrmInstitutes.ByIdAsync(x => x.InstituteId == key, trackChanges: false);

//    if (record == null)
//      throw new NotFoundException("CrmInstitute", "InstituteId", key.ToString());

//    await _repository.CrmInstitutes.DeleteAsync(x => x.InstituteId == key, trackChanges: true);
//    await _repository.SaveAsync();

//    _logger.LogInformation("CrmInstitute record deleted successfully: {InstituteId}", key);
//    return OperationMessage.Success;
//  }

//  /// <summary>
//  /// Retrieves CrmInstitute records for dropdown list asynchronously.
//  /// </summary>
//  public async Task<IEnumerable<CrmInstituteDLLDto>> CrmInstitutesForDDLAsync()
//  {
//    _logger.LogInformation("Fetching CrmInstitute records for dropdown list");

//    var records = await _repository.CrmInstitutes.ListWithSelectAsync(
//        x => new CrmInstitute
//        {
//          InstituteId = x.InstituteId,
//          InstituteName = x.InstituteName
//        },
//        orderBy: x => x.InstituteName,
//        trackChanges: false
//    );

//    if (!records.Any())
//      return new List<CrmInstituteDLLDto>();

//    var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmInstitute, CrmInstituteDLLDto>(records);
//    return recordsForDDLDto;
//  }

//  public async Task<IEnumerable<CrmInstituteDLLDto>> InstitutesByCountryIdDDLAsync(int countryId, bool trackChanges = false)
//  {
//    IEnumerable<CrmInstituteDLLDto> list = await _repository.CrmInstitutes.ListByWhereWithSelectAsync(
//      expression: x => x.CountryId == countryId,
//      selector: x => new CrmInstituteDLLDto
//      {
//        InstituteId = x.InstituteId,
//        InstituteName = x.InstituteName
//      },
//      orderBy: x => x.InstituteName,
//      trackChanges: trackChanges
//      );

//    if (!list.Any()) return new List<CrmInstituteDLLDto>();
//    //return MyMapper.JsonCloneIEnumerableToList<CrmInstituteDLLDto, CrmInstituteDto>(list);

//    return list;
//  }
//}
