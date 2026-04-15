using Domain.Contracts.Repositories;
// CrmApplicationService.cs (Part 1 of 2 - Core Methods)
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>
/// CRM Application service implementing business logic for application management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmApplicationService : ICrmApplicationService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmApplicationService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmApplicationService"/> with required dependencies.
	/// </summary>
	public CrmApplicationService(IRepositoryManager repository, ILogger<CrmApplicationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new CRM application record with all nested entities.
	/// </summary>
	public async Task<CrmApplicationDto> CreateApplicationAsync(CrmApplicationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(CrmApplicationDto));

		if (entityForCreate.ApplicationId != 0)
			throw new InvalidCreateOperationException("ApplicationId must be 0 for new record.");

		_logger.LogInformation("Creating new CRM application. Time: {Time}", DateTime.UtcNow);

		entityForCreate.CreatedDate = DateTime.UtcNow;
		entityForCreate.CreatedBy = currentUser.UserId ?? 0;

		var crmApplicationEntity = entityForCreate.MapTo<CrmApplication>();

		var applicatinEntity = await _repository.CrmApplications.CreateCrmApplicationAsync(crmApplicationEntity, cancellationToken);
		entityForCreate.ApplicationId = applicatinEntity.ApplicationId;

		_logger.LogInformation("CRM application created successfully. ID: {ApplicationId}, Time: {Time}",
						entityForCreate.ApplicationId, DateTime.UtcNow);
		return entityForCreate;
	}

	/// <summary>
	/// Updates an existing CRM application record.
	/// </summary>
	public async Task<CrmApplicationDto> UpdateApplicationAsync(int applicationId, CrmApplicationDto modelDto, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmApplicationDto));

		if (applicationId != modelDto.ApplicationId)
			throw new BadRequestException(applicationId.ToString(), nameof(CrmApplicationDto));

		_logger.LogInformation("Updating CRM application. ID: {ApplicationId}, Time: {Time}", applicationId, DateTime.UtcNow);

		bool exists = await _repository.CrmApplications.ExistsAsync(x => x.ApplicationId == applicationId, cancellationToken);
		if (!exists)
			throw new NotFoundException("Application", "ApplicationId", applicationId.ToString());

		modelDto.UpdatedDate = DateTime.UtcNow;
		modelDto.UpdatedBy = currentUser.UserId ?? 0;

		var crmApplicationDB = await _repository.CrmApplications
						.FirstOrDefaultAsync(x => x.ApplicationId == applicationId, trackChanges: false, cancellationToken);

		crmApplicationDB.StateId = modelDto.StateId;
		crmApplicationDB.ApplicationDate = crmApplicationDB.ApplicationDate > DateTime.MinValue ? crmApplicationDB.ApplicationDate : DateTime.UtcNow;
		crmApplicationDB.UpdatedDate = DateTime.UtcNow;
		crmApplicationDB.UpdatedBy = currentUser.UserId ?? 0;

		_repository.CrmApplications.UpdateByState(crmApplicationDB);
		await _repository.SaveChangesAsync(cancellationToken);

		_logger.LogInformation("CRM application updated successfully. ID: {ApplicationId}, Time: {Time}",
						applicationId, DateTime.UtcNow);

		return crmApplicationDB.MapTo<CrmApplicationDto>();
	}

	/// <summary>
	/// Retrieves a single CRM application record by its ID with all related data.
	/// </summary>
	public async Task<ApplicationDto> ApplicationAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching CRM application. ID: {ApplicationId}, Time: {Time}", applicationId, DateTime.UtcNow);

		var query = $@"
SELECT ca.ApplicationId, ca.ApplicationDate, ca.StateId,
       ai.ApplicantId, ai.FirstName, ai.LastName, ai.EmailAddress,
       ac.ApplicantCourseId, ac.CourseTitle, ac.InstituteId, ac.CountryId
FROM CrmApplication ca
INNER JOIN CrmApplicantInfo ai ON ca.ApplicationId = ai.ApplicationId
LEFT JOIN CrmApplicantCourse ac ON ai.ApplicantId = ac.ApplicantId
WHERE ca.ApplicationId = @ApplicationId";

		var parameters = new SqlParameter[]
		{
						new SqlParameter("@ApplicationId", applicationId)
		};

		ApplicationDto result = await _repository.CrmApplications.AdoExecuteSingleDataAsync<ApplicationDto>(query, parameters, cancellationToken);

		if (result is null)
		{
			_logger.LogWarning("No application found with ApplicationId: {ApplicationId}, Time: {Time}",
							applicationId, DateTime.UtcNow);
			throw new NotFoundException("Application", "ApplicationId", applicationId.ToString());
		}

		var educationHistories = await _repository.CrmEducationHistories.CrmEducationHistorysByApplicantIdAsync(result.ApplicantId, trackChanges, cancellationToken);

		if (educationHistories is not null && educationHistories.Any())
			result.EducationHistories = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmEducationHistory, EducationHistoryDto> (educationHistories);

		var workExperiences = await _repository.CrmWorkExperiences.CrmWorkExperiencesByApplicantIdAsync(result.ApplicantId, trackChanges, cancellationToken);
		if (workExperiences is not null && workExperiences.Any())
			result.WorkExperienceHistories = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmWorkExperience, WorkExperienceHistoryDto>(workExperiences);

		_logger.LogInformation("CRM application fetched successfully. ID: {ApplicationId}, Time: {Time}", applicationId, DateTime.UtcNow);

		return result;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all CRM applications with permission-based filtering.
	/// </summary>
	public async Task<GridEntity<CrmApplicationGridDto>> ApplicationsSummaryAsync(GridOptions options, int statusId, UsersDto currentUser, MenuDto menuDto, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching CRM applications summary grid. Time: {Time}", DateTime.UtcNow);

		string query = @"
SELECT ca.ApplicationId, ca.ApplicationDate, ca.StateId,
       ai.ApplicantId, ai.FirstName, ai.LastName, ai.EmailAddress,
       ac.CourseTitle, i.InstituteName, c.CountryName
FROM CrmApplication ca
INNER JOIN CrmApplicantInfo ai ON ca.ApplicationId = ai.ApplicationId
INNER JOIN CrmApplicantCourse ac ON ai.ApplicantId = ac.ApplicantId
INNER JOIN CrmInstitute i ON ac.InstituteId = i.InstituteId
INNER JOIN CrmCountry c ON ac.CountryId = c.CountryId";

		string orderBy = "ApplicationId ASC";
		string condition = statusId > 0 ? $" WHERE ca.StateId = {statusId}" : string.Empty;

		return await _repository.CrmApplications.AdoGridDataAsync<CrmApplicationGridDto>(query, options, orderBy, condition, cancellationToken);
	}

	/// <summary>
	/// Saves a CRM application record (create or update) with all nested entities and transaction support.
	/// </summary>
	public async Task<CrmApplicationDto> SaveApplicationAsync(CrmApplicationDto modelDto, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmApplicationDto));

		_logger.LogInformation("Saving CRM application. ApplicationId: {ApplicationId}, Time: {Time}",
						modelDto.ApplicationId, DateTime.UtcNow);

		await _repository.CrmApplications.TransactionBeginAsync(cancellationToken);

		try
		{
			if (modelDto.ApplicationId == 0)
			{
				modelDto.CreatedDate = DateTime.UtcNow;
				modelDto.CreatedBy = currentUser.UserId ?? 0;

				var crmApplicationEntity = modelDto.MapTo<CrmApplication>();
				var applicationEntity = await _repository.CrmApplications.CreateCrmApplicationAsync(crmApplicationEntity, cancellationToken);
				modelDto.ApplicationId = applicationEntity.ApplicationId;

				if (modelDto.CourseInformation?.PersonalDetails is not null)
				{
					modelDto.CourseInformation.PersonalDetails.ApplicationId = applicationEntity.ApplicationId;
					modelDto.CourseInformation.PersonalDetails.CreatedDate = DateTime.UtcNow;
					modelDto.CourseInformation.PersonalDetails.CreatedBy = currentUser.UserId ?? 0;

					var applicantInfoEntity = MyMapper.JsonClone<ApplicantInfoDto, CrmApplicantInfo>(modelDto.CourseInformation.PersonalDetails);
					var applicantEntity = await _repository.CrmApplicantInfoes.CreateCrmApplicantInfoAsync(applicantInfoEntity, cancellationToken);

					SetApplicantIdInAllNestedDtos(modelDto, applicantEntity.ApplicantId);
				}

				await _repository.CrmApplications.TransactionCommitAsync(cancellationToken);

				_logger.LogInformation("CRM application created successfully via SaveApplicationAsync. ID: {ApplicationId}, Time: {Time}",
								modelDto.ApplicationId, DateTime.UtcNow);

				return modelDto;
			}
			else
			{
				modelDto.UpdatedDate = DateTime.UtcNow;
				modelDto.UpdatedBy = currentUser.UserId ?? 0;

				var crmApplicationDB = await _repository.CrmApplications
								.FirstOrDefaultAsync(x => x.ApplicationId == modelDto.ApplicationId, trackChanges: false, cancellationToken);

				if (crmApplicationDB is null)
					throw new NotFoundException("Application", "ApplicationId", modelDto.ApplicationId.ToString());

				crmApplicationDB.StateId = modelDto.StateId;
				crmApplicationDB.UpdatedDate = DateTime.UtcNow;
				crmApplicationDB.UpdatedBy = currentUser.UserId ?? 0;

				_repository.CrmApplications.UpdateByState(crmApplicationDB);
				await _repository.SaveChangesAsync(cancellationToken);

				await _repository.CrmApplications.TransactionCommitAsync(cancellationToken);

				_logger.LogInformation("CRM application updated successfully via SaveApplicationAsync. ID: {ApplicationId}, Time: {Time}",
								modelDto.ApplicationId, DateTime.UtcNow);

				return modelDto;
			}
		}
		catch (Exception ex)
		{
			await _repository.CrmApplications.TransactionRollbackAsync(cancellationToken);
			_logger.LogError(ex, "Error saving CRM application. ApplicationId: {ApplicationId}, Time: {Time}",
							modelDto.ApplicationId, DateTime.UtcNow);
			throw;
		}
		finally
		{
			await _repository.CrmApplications.TransactionDisposeAsync();
		}
	}

	/// <summary>
	/// Sets ApplicantId in all nested DTOs that have ApplicantId property.
	/// </summary>
	private void SetApplicantIdInAllNestedDtos(CrmApplicationDto dto, int applicantId)
	{
		if (dto.CourseInformation?.ApplicantCourse is not null)
			dto.CourseInformation.ApplicantCourse.ApplicantId = applicantId;

		if (dto.CourseInformation?.ApplicantAddress?.PermanentAddress is not null)
			dto.CourseInformation.ApplicantAddress.PermanentAddress.ApplicantId = applicantId;

		if (dto.CourseInformation?.ApplicantAddress?.PresentAddress is not null)
			dto.CourseInformation.ApplicantAddress.PresentAddress.ApplicantId = applicantId;

		if (dto.EducationInformation?.EducationDetails?.EducationHistory is not null)
		{
			foreach (var educationDto in dto.EducationInformation.EducationDetails.EducationHistory)
				educationDto.ApplicantId = applicantId;
		}

		if (dto.EducationInformation?.IELTSInformation is not null)
			dto.EducationInformation.IELTSInformation.ApplicantId = applicantId;

		if (dto.EducationInformation?.TOEFLInformation is not null)
			dto.EducationInformation.TOEFLInformation.ApplicantId = applicantId;

		if (dto.EducationInformation?.PTEInformation is not null)
			dto.EducationInformation.PTEInformation.ApplicantId = applicantId;

		if (dto.EducationInformation?.GMATInformation is not null)
			dto.EducationInformation.GMATInformation.ApplicantId = applicantId;

		if (dto.EducationInformation?.OTHERSInformation is not null)
			dto.EducationInformation.OTHERSInformation.ApplicantId = applicantId;

		if (dto.EducationInformation?.WorkExperience?.WorkExperienceHistory is not null)
		{
			foreach (var workExpDto in dto.EducationInformation.WorkExperience.WorkExperienceHistory)
				workExpDto.ApplicantId = applicantId;
		}

		if (dto.AdditionalInformation?.ReferenceDetails?.References is not null)
		{
			foreach (var referenceDto in dto.AdditionalInformation.ReferenceDetails.References)
				referenceDto.ApplicantId = applicantId;
		}

		if (dto.AdditionalInformation?.AdditionalDocuments?.Documents is not null)
		{
			foreach (var documentDto in dto.AdditionalInformation.AdditionalDocuments.Documents)
				documentDto.ApplicantId = applicantId;
		}
	}
}


//// CrmApplicationService.cs
//using Domain.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Contracts.Services.CRM;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Domain.Exceptions;
//using Domain.Exceptions;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Application.Services.CRM;

///// <summary>
///// CRM Application service implementing business logic for application management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmApplicationService : ICrmApplicationService
//{
//	private readonly IRepositoryManager _repository;
//	private readonly ILogger<CrmApplicationService> _logger;
//	private readonly IConfiguration _config;
//	private readonly IHttpContextAccessor _httpContextAccessor;

//	/// <summary>
//	/// Initializes a new instance of <see cref="CrmApplicationService"/> with required dependencies.
//	/// </summary>
//	public CrmApplicationService(IRepositoryManager repository, ILogger<CrmApplicationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
//	{
//		_repository = repository;
//		_logger = logger;
//		_config = config;
//		_httpContextAccessor = httpContextAccessor;
//	}

//	/// <summary>
//	/// Creates a new CRM application record after validating for null input.
//	/// </summary>
//	public async Task<CrmApplicationDto> CreateApplicationAsync(CrmApplicationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
//	{
//		if (entityForCreate is null)
//			throw new BadRequestException(nameof(CrmApplicationDto));

//		if (entityForCreate.ApplicationId != 0)
//			throw new InvalidCreateOperationException("ApplicationId must be 0 for new record.");

//		_logger.LogInformation("Creating new CRM application. Time: {Time}", DateTime.UtcNow);

//		entityForCreate.CreatedDate = DateTime.UtcNow;
//		entityForCreate.CreatedBy = currentUser.UserId ?? 0;

//		var crmApplicationEntity = entityForCreate.MapTo<CrmApplication>();

//		//int applicationId = await _repository.CrmApplications.CreateAndGetIdAsync(crmApplicationEntity, cancellationToken);
//		crmApplicationEntity = await _repository.CrmApplications.CreateCrmApplicationAsync(crmApplicationEntity, cancellationToken);
//		entityForCreate.ApplicationId = crmApplicationEntity.ApplicationId;

//		_logger.LogInformation("CRM application created successfully. ID: {ApplicationId}, Time: {Time}",
//						crmApplicationEntity.ApplicationId, DateTime.UtcNow);
//		return entityForCreate;
//	}

//	/// <summary>
//	/// Updates an existing CRM application record.
//	/// </summary>
//	public async Task<CrmApplicationDto> UpdateApplicationAsync(int applicationId, CrmApplicationDto modelDto, UsersDto currentUser, bool trackChanges, CancellationToken cancellationToken = default)
//	{
//		if (modelDto is null)
//			throw new BadRequestException(nameof(CrmApplicationDto));

//		if (applicationId != modelDto.ApplicationId)
//			throw new BadRequestException(applicationId.ToString(), nameof(CrmApplicationDto));

//		_logger.LogInformation("Updating CRM application. ID: {ApplicationId}, Time: {Time}", applicationId, DateTime.UtcNow);

//		bool exists = await _repository.CrmApplications.ExistsAsync(x => x.ApplicationId == applicationId, cancellationToken);
//		if (!exists)
//			throw new NotFoundException("Application", "ApplicationId", applicationId.ToString());

//		modelDto.UpdatedDate = DateTime.UtcNow;
//		modelDto.UpdatedBy = currentUser.UserId ?? 0;

//		var crmApplicationDB = await _repository.CrmApplications
//						.FirstOrDefaultAsync(x => x.ApplicationId == applicationId, trackChanges: false, cancellationToken);

//		crmApplicationDB.StateId = modelDto.StateId;
//		crmApplicationDB.ApplicationDate = crmApplicationDB.ApplicationDate > DateTime.MinValue ? crmApplicationDB.ApplicationDate : DateTime.UtcNow;
//		crmApplicationDB.UpdatedDate = DateTime.UtcNow;
//		crmApplicationDB.UpdatedBy = currentUser.UserId ?? 0;

//		_repository.CrmApplications.UpdateByState(crmApplicationDB);
//		await _repository.SaveChangesAsync(cancellationToken);

//		_logger.LogInformation("CRM application updated successfully. ID: {ApplicationId}, Time: {Time}",
//						applicationId, DateTime.UtcNow);

//		return crmApplicationDB.MapTo<CrmApplicationDto>();
//	}

//	/// <summary>
//	/// Retrieves a single CRM application record by its ID with all related data.
//	/// </summary>
//	public async Task<CrmApplicationDto> ApplicationAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
//	{
//		_logger.LogInformation("Fetching CRM application. ID: {ApplicationId}, Time: {Time}", applicationId, DateTime.UtcNow);

//		var query = $@"
//SELECT ca.ApplicationId, ca.ApplicationDate, ca.StateId,
//       ai.ApplicantId, ai.FirstName, ai.LastName, ai.EmailAddress,
//       ac.ApplicantCourseId, ac.CourseTitle, ac.InstituteId, ac.CountryId
//FROM CrmApplication ca
//INNER JOIN CrmApplicantInfo ai ON ca.ApplicationId = ai.ApplicationId
//LEFT JOIN CrmApplicantCourse ac ON ai.ApplicantId = ac.ApplicantId
//WHERE ca.ApplicationId = @ApplicationId";

//		var parameters = new SqlParameter[]
//		{
//						new SqlParameter("@ApplicationId", applicationId)
//		};

//		CrmApplicationDto result = await _repository.CrmApplications.AdoExecuteSingleDataAsync<CrmApplicationDto>(query, parameters, cancellationToken);
//		//GetApplicationDto result = await _repository.CrmApplications.ExecuteSingleData<GetApplicationDto>(query, parameters, cancellationToken);

//		if (result is null)
//		{
//			_logger.LogWarning("No application found with ApplicationId: {ApplicationId}, Time: {Time}",
//							applicationId, DateTime.UtcNow);
//			throw new NotFoundException("Application", "ApplicationId", applicationId.ToString());
//		}

//		var educationHistories = await _repository.CrmEducationHistories.CrmEducationHistorysByApplicantIdAsync(result.appli, trackChanges ,cancellationToken);
//		//var educationHistories = await _repository.CrmEducationHistories.EducationHistoryByApplicantId(result.ApplicantId, cancellationToken);

//		if (educationHistories is not null && educationHistories.Any())
//			result.EducationHistories = MyMapper.JsonCloneIEnumerableToIEnumerable<EducationHistory, EducationHistoryDto>(educationHistories);

//		var workExperiences = await _repository.CrmWorkExperiences
//						.WorkExperiencesByApplicantId(result.ApplicantId, cancellationToken);

//		if (workExperiences is not null && workExperiences.Any())
//			result.WorkExperienceHistories = MyMapper.JsonCloneIEnumerableToIEnumerable<WorkExperienceHistory, WorkExperienceHistoryDto>(workExperiences);

//		_logger.LogInformation("CRM application fetched successfully. ID: {ApplicationId}, Time: {Time}",
//						applicationId, DateTime.UtcNow);

//		return result;
//	}

//	/// <summary>
//	/// Retrieves a paginated summary grid of all CRM applications.
//	/// </summary>
//	public async Task<GridEntity<CrmApplicationGridDto>> ApplicationsSummaryAsync(GridOptions options, int statusId, UsersDto currentUser, MenuDto menuDto, CancellationToken cancellationToken = default)
//	{
//		_logger.LogInformation("Fetching CRM applications summary grid. Time: {Time}", DateTime.UtcNow);

//		string query = @"
//SELECT ca.ApplicationId, ca.ApplicationDate, ca.StateId,
//       ai.ApplicantId, ai.FirstName, ai.LastName, ai.EmailAddress,
//       ac.CourseTitle, i.InstituteName, c.CountryName
//FROM CrmApplication ca
//INNER JOIN CrmApplicantInfo ai ON ca.ApplicationId = ai.ApplicationId
//INNER JOIN CrmApplicantCourse ac ON ai.ApplicantId = ac.ApplicantId
//INNER JOIN CrmInstitute i ON ac.InstituteId = i.InstituteId
//INNER JOIN CrmCountry c ON ac.CountryId = c.CountryId";

//		string orderBy = "ApplicationId ASC";
//		string condition = statusId > 0 ? $" WHERE ca.StateId = {statusId}" : string.Empty;

//		return await _repository.CrmApplications.GridData<CrmApplicationGridDto>(query, options, orderBy, condition, cancellationToken);
//	}

//	/// <summary>
//	/// Saves a CRM application record (create or update) with all nested entities.
//	/// </summary>
//	public async Task<CrmApplicationDto> SaveApplicationAsync(CrmApplicationDto modelDto, UsersDto currentUser, CancellationToken cancellationToken = default)
//	{
//		if (modelDto is null)
//			throw new BadRequestException(nameof(CrmApplicationDto));

//		_logger.LogInformation("Saving CRM application. ApplicationId: {ApplicationId}, Time: {Time}",
//						modelDto.ApplicationId, DateTime.UtcNow);

//		await using var transaction = await _repository.CrmApplications.TransactionBeginAsync(cancellationToken);

//		try
//		{
//			if (modelDto.ApplicationId == 0)
//			{
//				modelDto.CreatedDate = DateTime.UtcNow;
//				modelDto.CreatedBy = currentUser.UserId ?? 0;

//				var crmApplicationEntity = modelDto.MapTo<CrmApplication>();
//				int applicationId = await _repository.CrmApplications.CreateAndGetIdAsync(crmApplicationEntity, cancellationToken);
//				modelDto.ApplicationId = applicationId;

//				if (modelDto.CourseInformation?.PersonalDetails is not null)
//				{
//					modelDto.CourseInformation.PersonalDetails.ApplicationId = applicationId;
//					modelDto.CourseInformation.PersonalDetails.CreatedDate = DateTime.UtcNow;
//					modelDto.CourseInformation.PersonalDetails.CreatedBy = currentUser.UserId ?? 0;

//					var applicantInfoEntity = MyMapper.JsonClone<ApplicantInfoDto, CrmApplicantInfo>(modelDto.CourseInformation.PersonalDetails);
//					int applicantId = await _repository.CrmApplicantInfoes.CreateAndGetIdAsync(applicantInfoEntity, cancellationToken);

//					SetApplicantIdInAllNestedDtos(modelDto, applicantId);
//				}

//				await _repository.CrmApplications.TransactionCommitAsync(cancellationToken);

//				_logger.LogInformation("CRM application created successfully via SaveApplicationAsync. ID: {ApplicationId}, Time: {Time}",
//								modelDto.ApplicationId, DateTime.UtcNow);

//				return modelDto;
//			}
//			else
//			{
//				modelDto.UpdatedDate = DateTime.UtcNow;
//				modelDto.UpdatedBy = currentUser.UserId ?? 0;

//				var crmApplicationDB = await _repository.CrmApplications
//								.FirstOrDefaultAsync(x => x.ApplicationId == modelDto.ApplicationId, trackChanges: false, cancellationToken);

//				if (crmApplicationDB is null)
//					throw new NotFoundException("Application", "ApplicationId", modelDto.ApplicationId.ToString());

//				crmApplicationDB.StateId = modelDto.StateId;
//				crmApplicationDB.UpdatedDate = DateTime.UtcNow;
//				crmApplicationDB.UpdatedBy = currentUser.UserId ?? 0;

//				_repository.CrmApplications.UpdateByState(crmApplicationDB);
//				await _repository.SaveChangesAsync(cancellationToken);

//				await _repository.CrmApplications.TransactionCommitAsync(cancellationToken);

//				_logger.LogInformation("CRM application updated successfully via SaveApplicationAsync. ID: {ApplicationId}, Time: {Time}",
//								modelDto.ApplicationId, DateTime.UtcNow);

//				return modelDto;
//			}
//		}
//		catch (Exception ex)
//		{
//			await _repository.CrmApplications.TransactionRollbackAsync(cancellationToken);
//			_logger.LogError(ex, "Error saving CRM application. ApplicationId: {ApplicationId}, Time: {Time}",
//							modelDto.ApplicationId, DateTime.UtcNow);
//			throw;
//		}
//		finally
//		{
//			await _repository.CrmApplications.TransactionDisposeAsync();
//		}
//	}

//	/// <summary>
//	/// Sets ApplicantId in all nested DTOs that have ApplicantId property.
//	/// </summary>
//	private void SetApplicantIdInAllNestedDtos(CrmApplicationDto dto, int applicantId)
//	{
//		if (dto.CourseInformation?.ApplicantCourse is not null)
//			dto.CourseInformation.ApplicantCourse.ApplicantId = applicantId;

//		if (dto.CourseInformation?.ApplicantAddress?.PermanentAddress is not null)
//			dto.CourseInformation.ApplicantAddress.PermanentAddress.ApplicantId = applicantId;

//		if (dto.CourseInformation?.ApplicantAddress?.PresentAddress is not null)
//			dto.CourseInformation.ApplicantAddress.PresentAddress.ApplicantId = applicantId;

//		if (dto.EducationInformation?.EducationDetails?.EducationHistory is not null)
//		{
//			foreach (var educationDto in dto.EducationInformation.EducationDetails.EducationHistory)
//				educationDto.ApplicantId = applicantId;
//		}

//		if (dto.EducationInformation?.IELTSInformation is not null)
//			dto.EducationInformation.IELTSInformation.ApplicantId = applicantId;

//		if (dto.EducationInformation?.TOEFLInformation is not null)
//			dto.EducationInformation.TOEFLInformation.ApplicantId = applicantId;

//		if (dto.EducationInformation?.PTEInformation is not null)
//			dto.EducationInformation.PTEInformation.ApplicantId = applicantId;

//		if (dto.EducationInformation?.GMATInformation is not null)
//			dto.EducationInformation.GMATInformation.ApplicantId = applicantId;

//		if (dto.EducationInformation?.OTHERSInformation is not null)
//			dto.EducationInformation.OTHERSInformation.ApplicantId = applicantId;

//		if (dto.EducationInformation?.WorkExperience?.WorkExperienceHistory is not null)
//		{
//			foreach (var workExpDto in dto.EducationInformation.WorkExperience.WorkExperienceHistory)
//				workExpDto.ApplicantId = applicantId;
//		}

//		if (dto.AdditionalInformation?.ReferenceDetails?.References is not null)
//		{
//			foreach (var referenceDto in dto.AdditionalInformation.ReferenceDetails.References)
//				referenceDto.ApplicantId = applicantId;
//		}

//		if (dto.AdditionalInformation?.AdditionalDocuments?.Documents is not null)
//		{
//			foreach (var documentDto in dto.AdditionalInformation.AdditionalDocuments.Documents)
//				documentDto.ApplicantId = applicantId;
//		}
//	}
//}


////using Domain.Entities.Entities.CRM;
////using Domain.Contracts.Services.Core.SystemAdmin;
////using bdDevs.Shared.DataTransferObjects.CRM;
////using Domain.Contracts.Services.CRM;
////using bdDevs.Shared.DataTransferObjects.CRM;
////using Domain.Exceptions;
////using Application.Shared.Grid;
////using Application.Services.Mappings;
////using Microsoft.Extensions.Configuration;
////using Microsoft.Extensions.Logging;

////namespace Application.Services.CRM;

/////// <summary>
/////// CrmApplication service implementing business logic for CrmApplication management.
/////// Follows enterprise patterns with structured logging and exception handling.
/////// </summary>
////internal sealed class CrmApplicationService : ICrmApplicationService
////{
////    private readonly IRepositoryManager _repository;
////    private readonly ILogger<CrmApplicationService> _logger;
////    private readonly IConfiguration _configuration;

////    public CrmApplicationService(IRepositoryManager repository, ILogger<CrmApplicationService> logger, IConfiguration configuration)
////    {
////        _repository = repository;
////        _logger = logger;
////        _configuration = configuration;
////    }

////    /// <summary>
////    /// Retrieves paginated summary grid of CrmApplication records asynchronously.
////    /// </summary>
////    public async Task<GridEntity<CrmApplicationDto>> CrmApplicationSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
////    {
////        _logger.LogInformation("Fetching CrmApplication summary grid");

////        string query = "SELECT * FROM CrmApplication";
////        string orderBy = "Title ASC";

////        var gridEntity = await _repository.CrmApplications.AdoGridDataAsync<CrmApplicationDto>(query, options, orderBy, "", cancellationToken);
////        return gridEntity;
////    }

////    /// <summary>
////    /// Retrieves all CrmApplication records asynchronously.
////    /// </summary>
////    public async Task<IEnumerable<CrmApplicationDto>> CrmApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
////    {
////        _logger.LogInformation("Fetching all CrmApplication records");

////        var records = await _repository.CrmApplications.CrmApplicationsAsync(trackChanges, cancellationToken);

////        if (!records.Any())
////        {
////            _logger.LogWarning("No CrmApplication records found");
////            return Enumerable.Empty<CrmApplicationDto>();
////        }

////        var recordDtos = records.MapToList<CrmApplicationDto>();
////        return recordDtos;
////    }

////    /// <summary>
////    /// Retrieves a CrmApplication record by ID asynchronously.
////    /// </summary>
////    public async Task<CrmApplicationDto> CrmApplicationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
////    {
////        if (id <= 0)
////        {
////            _logger.LogWarning("CrmApplicationAsync called with invalid id: {ApplicationId}", id);
////            throw new BadRequestException("Invalid request!");
////        }

////        _logger.LogInformation("Fetching CrmApplication record with ID: {ApplicationId}", id);

////        var record = await _repository.CrmApplications.CrmApplicationAsync(id, trackChanges, cancellationToken);

////        if (record == null)
////        {
////            _logger.LogWarning("CrmApplication record not found with ID: {ApplicationId}", id);
////            throw new NotFoundException("CrmApplication", "ApplicationId", id.ToString());
////        }

////        var recordDto = record.MapTo<CrmApplicationDto>();
////        return recordDto;
////    }

////    /// <summary>
////    /// Creates a new CrmApplication record asynchronously.
////    /// </summary>
////    public async Task<CrmApplicationDto> CreateAsync(CrmApplicationDto modelDto)
////    {
////        if (modelDto == null)
////            throw new BadRequestException(nameof(CrmApplicationDto));

////        _logger.LogInformation("Creating new CrmApplication record");

////        // Check for duplicate record
////        bool recordExists = await _repository.CrmApplications.ExistsAsync(
////            x => x.Title.Trim().ToLower() == modelDto.Title.Trim().ToLower());

////        if (recordExists)
////            throw new DuplicateRecordException("CrmApplication", "Title");

////        // Map and create
////        CrmApplication entity = modelDto.MapTo<CrmApplication>();
////        modelDto.ApplicationId = await _repository.CrmApplications.CreateAndIdAsync(entity);
////        await _repository.SaveAsync();

////        _logger.LogInformation("CrmApplication record created successfully with ID: {ApplicationId}", modelDto.ApplicationId);

////        return modelDto;
////    }

////    /// <summary>
////    /// Updates an existing CrmApplication record asynchronously.
////    /// </summary>
////    public async Task<CrmApplicationDto> UpdateAsync(int key, CrmApplicationDto modelDto)
////    {
////        if (modelDto == null)
////            throw new BadRequestException(nameof(CrmApplicationDto));

////        if (key != modelDto.ApplicationId)
////            throw new BadRequestException(key.ToString(), nameof(CrmApplicationDto));

////        _logger.LogInformation("Updating CrmApplication record with ID: {ApplicationId}", key);

////        // Check if record exists
////        var existingRecord = await _repository.CrmApplications.ByIdAsync(
////            x => x.ApplicationId == key, trackChanges: false);

////        if (existingRecord == null)
////            throw new NotFoundException("CrmApplication", "ApplicationId", key.ToString());

////        // Check for duplicate name (excluding current record)
////        bool duplicateExists = await _repository.CrmApplications.ExistsAsync(
////            x => x.Title.Trim().ToLower() == modelDto.Title.Trim().ToLower() 
////                 && x.ApplicationId != key);

////        if (duplicateExists)
////            throw new DuplicateRecordException("CrmApplication", "Title");

////        // Map and update
////        CrmApplication entity = modelDto.MapTo<CrmApplication>();
////        _repository.CrmApplications.UpdateByState(entity);
////        await _repository.SaveAsync();

////        _logger.LogInformation("CrmApplication record updated successfully: {ApplicationId}", key);

////        return modelDto;
////    }

////    /// <summary>
////    /// Deletes a CrmApplication record by ID asynchronously.
////    /// </summary>
////    public async Task DeleteAsync(int key)
////    {
////        if (key <= 0)
////            throw new BadRequestException("Invalid request!");

////        _logger.LogInformation("Deleting CrmApplication record with ID: {ApplicationId}", key);

////        var record = await _repository.CrmApplications.ByIdAsync(
////            x => x.ApplicationId == key, trackChanges: false);

////        if (record == null)
////            throw new NotFoundException("CrmApplication", "ApplicationId", key.ToString());

////        await _repository.CrmApplications.DeleteAsync(x => x.ApplicationId == key, trackChanges: false);
////        await _repository.SaveAsync();

////        _logger.LogInformation("CrmApplication record deleted successfully: {ApplicationId}", key);
////    }

////    /// <summary>
////    /// Retrieves CrmApplication records for dropdown list asynchronously.
////    /// </summary>
////    public async Task<IEnumerable<CrmApplicationForDDLDto>> CrmApplicationsForDDLAsync()
////    {
////        _logger.LogInformation("Fetching CrmApplication records for dropdown list");

////        var records = await _repository.CrmApplications.ListWithSelectAsync(
////            x => new CrmApplication
////            {
////                ApplicationId = x.ApplicationId,
////                Title = x.Title
////            },
////            orderBy: x => x.Title,
////            trackChanges: false
////        );

////        if (!records.Any())
////            return new List<CrmApplicationForDDLDto>();

////        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmApplication, CrmApplicationForDDLDto>(records);
////        return recordsForDDLDto;
////    }
////}
