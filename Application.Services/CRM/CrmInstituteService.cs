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
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

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
	/// Creates a new institute record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmInstituteDto> CreateAsync(CreateCrmInstituteRecord record, UsersDto currentUser,CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmInstituteRecord));

		_logger.LogInformation("Creating new institute. Name: {InstituteName}, Time: {Time}",
						record.InstituteName, DateTime.UtcNow);

		bool instituteExists = await _repository.CrmInstitutes.ExistsAsync(
						x => x.InstituteName != null
								&& x.InstituteName.Trim().ToLower() == record.InstituteName!.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (instituteExists)
			throw new DuplicateRecordException("CrmInstitute", "InstituteName");

		// Map Record to Entity using Mapster
		CrmInstitute institute = record.MapTo<CrmInstitute>();
		int instituteId = await _repository.CrmInstitutes.CreateAndIdAsync(institute, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Institute created successfully. ID: {InstituteId}, Time: {Time}",
						instituteId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = institute.MapTo<CrmInstituteDto>();
		resultDto.InstituteId = instituteId;
		return resultDto;
	}

	/// <summary>
	/// Updates an existing institute record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmInstituteDto> UpdateAsync(UpdateCrmInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmInstituteRecord));

		_logger.LogInformation("Updating institute. ID: {InstituteId}, Time: {Time}", record.InstituteId, DateTime.UtcNow);

		// Check if institute exists
		var existingInstitute = await _repository.CrmInstitutes
						.FirstOrDefaultAsync(x => x.InstituteId == record.InstituteId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmInstitute", "InstituteId", record.InstituteId.ToString());

		// Check for duplicate name (excluding current record)
		bool duplicateExists = await _repository.CrmInstitutes.ExistsAsync(
						x => x.InstituteName != null
								&& x.InstituteName.Trim().ToLower() == record.InstituteName!.Trim().ToLower()
								&& x.InstituteId != record.InstituteId,
						cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("CrmInstitute", "InstituteName");

		// Map Record to Entity using Mapster
		CrmInstitute institute = record.MapTo<CrmInstitute>();
		_repository.CrmInstitutes.UpdateByState(institute);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Institute updated successfully. ID: {InstituteId}, Time: {Time}",
						record.InstituteId, DateTime.UtcNow);

		return institute.MapTo<CrmInstituteDto>();
	}

	/// <summary>
	/// Deletes an institute record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmInstituteRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.InstituteId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting institute. ID: {InstituteId}, Time: {Time}", record.InstituteId, DateTime.UtcNow);

		var instituteEntity = await _repository.CrmInstitutes
						.FirstOrDefaultAsync(x => x.InstituteId == record.InstituteId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmInstitute", "InstituteId", record.InstituteId.ToString());

		await _repository.CrmInstitutes.DeleteAsync(x => x.InstituteId == record.InstituteId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Institute deleted successfully. ID: {InstituteId}, Time: {Time}",
						record.InstituteId, DateTime.UtcNow);
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

		return institute.MapTo<CrmInstituteDto>();
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

		var institutesDto = institutes.MapToList<CrmInstituteDto>();

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

		var institutesDto = institutes.MapToList<CrmInstituteDto>();

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

		return institute.MapTo<CrmInstituteDto>();
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
                    CrmInstitute.CountryId,
                    InstituteName,
                    Campus,
                    Website,
                    MonthlyLivingCost,
                    FundsRequirementforVisa,
                    ApplicationFee,
                    CrmInstitute.CurrencyId,
                    IsLanguageMandatory,
                    LanguagesRequirement,
                    InstitutionalBenefits,
                    PartTimeWorkDetails,
                    ScholarshipsPolicy,
                    InstitutionStatusNotes,
                    CrmInstitute.InstituteTypeId,
                    InstituteCode,
                    InstituteEmail,
                    InstituteAddress,
                    InstitutePhoneNO,
                    InstituteMobileNo,
                    CrmInstitute.Status,
                    CrmCountry.CountryName,
                    CrmCurrencyInfo.CurrencyName,
                    CrmInstituteType.InstituteTypeName,
                    docLogo.FilePath AS InstitutionLogo,
                    docProspectus.FilePath AS InstitutionProspectus
                FROM CrmInstitute
                LEFT JOIN CrmCountry ON CrmInstitute.CountryId = CrmCountry.CountryId
                LEFT JOIN CrmCurrencyInfo ON CrmInstitute.CurrencyId = CrmCurrencyInfo.CurrencyId
                LEFT JOIN CrmInstituteType ON CrmInstitute.InstituteTypeId = CrmInstituteType.InstituteTypeId
                LEFT JOIN DMSDocument docLogo ON CrmInstitute.InstituteId = docLogo.ReferenceEntityId AND docLogo.SystemTag = 'InstitutionLogo'
                LEFT JOIN DMSDocument docProspectus ON CrmInstitute.InstituteId = docProspectus.ReferenceEntityId AND docProspectus.SystemTag = 'InstitutionProspectus'";

		const string orderBy = "InstituteName ASC";

		_logger.LogInformation("Fetching institutes summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmInstitutes.AdoGridDataAsync<CrmInstituteDto>(sql, options, orderBy, "", cancellationToken);
	}
}
