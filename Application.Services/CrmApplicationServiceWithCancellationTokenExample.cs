using Domain.Contracts.Repositories;
﻿//using Domain.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Contracts.Services;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Exceptions;
//using Domain.Exceptions;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Domain.Contracts.Services;


///// <summary>
///// CrmApplicationService with CancellationToken Implementation Example
/////
///// এই class টি দেখায় কিভাবে existing service এ CancellationToken যোগ করতে হয়।
/////
///// Key Changes:
///// 1. সব async methods এ CancellationToken parameter যোগ (with default value)
///// 2. Repository calls এ CancellationToken pass করা
///// 3. Long operations এ periodic cancellation check
///// 4. OperationCanceledException proper handling
///// 5. Transaction operations সহ CancellationToken usage
///// </summary>
//public class CrmApplicationServiceWithCancellationTokenExample : ICrmApplicationService
//{
//  private readonly IRepositoryManager _repository;
//  private readonly ILogger<CrmApplicationServiceWithCancellationTokenExample> _logger;
//  private readonly IConfiguration _config;
//  private readonly IHttpContextAccessor _httpContextAccessor;

//  public CrmApplicationServiceWithCancellationTokenExample(
//      IRepositoryManager repository,
//      ILogger<CrmApplicationServiceWithCancellationTokenExample> logger,
//      IConfiguration config,
//      IHttpContextAccessor httpContextAccessor)
//  {
//    _repository = repository;
//    _logger = logger;
//    _config = config;
//    _httpContextAccessor = httpContextAccessor;
//  }

//  /* ========================================
//     ✅ EXAMPLE 1: GridData Method with CancellationToken

//     Grid operations can be expensive:
//     - Complex SQL queries
//     - Large result sets
//     - Multiple permission checks

//     CancellationToken এখানে critical!
//  ======================================== */
//  public async Task<GridEntity<CrmApplicationGridDto>> SummaryGrid(
//      GridOptions options,
//      int statusId,
//      UsersDto usersDto,
//      MenuDto menuDto,
//      CancellationToken cancellationToken = default) // ✅ Added with default value
//  {
//    try
//    {
//      // ✅ Check cancellation at method start
//      cancellationToken.ThrowIfCancellationRequested();

//      if (menuDto.MenuId != null && menuDto.MenuId != 0)
//      {
//        // ✅ Pass CancellationToken to repository call
//        IEnumerable<GroupPermission> returnResult =
//            await _repository.Groups.AccessPermisionForCurrentUser(
//                menuDto.ModuleId.Value,
//                usersDto.UserId.Value,
//                cancellationToken);

//        // ✅ Check cancellation after database operation
//        cancellationToken.ThrowIfCancellationRequested();

//        IEnumerable<GroupPermissionDto> result =
//            MyMapper.JsonCloneIEnumerableToIEnumerable<GroupPermission, GroupPermissionDto>(returnResult);

//        var isApprover = result.Any(groupPermission => groupPermission.ReferenceID == 4);
//        var isRecomander = result.Any(groupPermission => groupPermission.ReferenceID == 3);
//        var isHr = result.Any(groupPermission => groupPermission.ReferenceID == 22);
//        var onlyApprovalData = result.Any(groupPermission => groupPermission.ReferenceID == 23);
//      }

//      // ✅ Check cancellation again before permission check
//      cancellationToken.ThrowIfCancellationRequested();

//      IEnumerable<GroupPermission> returnResult2 =
//          await _repository.Groups.AccessPermisionForCurrentUser(
//              menuDto.ModuleId.Value,
//              usersDto.UserId.Value,
//              cancellationToken);

//      IEnumerable<GroupPermissionDto> result2 =
//          MyMapper.JsonCloneIEnumerableToIEnumerable<GroupPermission, GroupPermissionDto>(returnResult2);

//      var isApprover2 = result2.Any(groupPermission => groupPermission.ReferenceID == 4);
//      var isRecomander2 = result2.Any(groupPermission => groupPermission.ReferenceID == 3);
//      var isHr2 = result2.Any(groupPermission => groupPermission.ReferenceID == 22);
//      var onlyApprovalData2 = result2.Any(groupPermission => groupPermission.ReferenceID == 23);

//      string condition = string.Empty;

//      // Complex SQL query
//      string sql = string.Format(@"
//                SELECT
//                    ca.ApplicationId,
//                    ca.ApplicationDate,
//                    ca.StateId,
//                    doc.FilePath as ApplicantImagePath,
//                    ai.ApplicantId,
//                    -- ... (full query from original)
//                FROM CrmApplication ca
//                INNER JOIN CrmApplicantInfo ai ON ca.ApplicationId = ai.ApplicationId
//                -- ... (rest of joins)
//            ");

//      string orderBy = " ApplicationId asc ";

//      // ✅ Check cancellation before expensive grid query
//      cancellationToken.ThrowIfCancellationRequested();

//      // ✅ Pass CancellationToken to GridData
//      var gridResult = await _repository.CrmApplications.GridData<CrmApplicationGridDto>(
//          sql,
//          options,
//          orderBy,
//          condition,
//          cancellationToken);

//      return gridResult;
//    }
//    catch (DataMappingException ex)
//    {
//      _logger.LogError($"Grid mapping error: {ex.Message}");
//      throw new BadRequestException($"Grid data mapping error. {ex.Message}");
//    }
//    catch (OperationCanceledException)
//    {
//      // ✅ Log cancellation as info, not error
//      _logger.LogInformation(
//          "SummaryGrid operation cancelled for StatusId: {StatusId}, UserId: {UserId}",
//          statusId,
//          usersDto.UserId);
//      throw; // Re-throw to let controller handle it
//    }
//    catch (Exception ex)
//    {
//      _logger.LogError(ex, "Error in SummaryGrid for StatusId: {StatusId}", statusId);
//      throw;
//    }
//  }

//  /* ========================================
//     ✅ EXAMPLE 2: Application with Multiple Queries

//     Multiple async calls করা হচ্ছে, প্রতিটিতে token pass করতে হবে
//  ======================================== */
//  public async Task<ApplicationDto> Application(
//      int applicationId,
//      bool trackChanges,
//      CancellationToken cancellationToken = default) // ✅ Added with default value
//  {
//    try
//    {
//      // ✅ Check cancellation at start
//      cancellationToken.ThrowIfCancellationRequested();

//      string query = @"
//                SELECT
//                    -- ... (full query)
//                FROM CrmApplication ca
//                INNER JOIN CrmApplicantInfo ai ON ca.ApplicationId = ai.ApplicationId
//                -- ... (rest of query)
//                WHERE ca.ApplicationId = @ApplicationId
//            ";

//      var parameters = new SqlParameter[]
//      {
//                new SqlParameter("@ApplicationId", applicationId)
//      };

//      // ✅ Pass CancellationToken to main query
//      ApplicationDto result = await _repository.CrmApplications.ExecuteSingleData<ApplicationDto>(
//          query,
//          parameters,
//          cancellationToken);

//      if (result == null)
//      {
//        _logger.LogWarning("No application found with ApplicationId: {ApplicationId}", applicationId);
//        return new ApplicationDto();
//      }

//      // ✅ Check cancellation before next query
//      cancellationToken.ThrowIfCancellationRequested();

//      // Load education histories
//      var educationHistories = await _repository.CrmEducationHistories.EducationHistoryByApplicantId(
//          result.ApplicantId,
//          cancellationToken);

//      if (educationHistories != null && educationHistories.Any())
//      {
//        result.EducationHistories = MyMapper.JsonCloneIEnumerableToIEnumerable<
//            EducationHistory,
//            EducationHistoryDto>(educationHistories);
//      }

//      // ✅ Check cancellation before next query
//      cancellationToken.ThrowIfCancellationRequested();

//      // Load work experiences
//      var workExperiences = await _repository.CrmWorkExperiences.WorkExperiencesByApplicantId(
//          result.ApplicantId,
//          cancellationToken);

//      if (workExperiences != null && workExperiences.Any())
//      {
//        result.WorkExperienceHistories = MyMapper.JsonCloneIEnumerableToIEnumerable<
//            WorkExperienceHistory,
//            WorkExperienceHistoryDto>(workExperiences);
//      }

//      // ✅ Check cancellation before next query
//      cancellationToken.ThrowIfCancellationRequested();

//      // Load references
//      var references = await _repository.CrmApplicantReferences.ListByConditionAsync(
//          expression: x => x.ApplicantId == result.ApplicantId,
//          orderBy: x => x.ApplicantReferenceId,
//          trackChanges: false,
//          cancellationToken);

//      if (references != null && references.Any())
//      {
//        result.ApplicantReferences = MyMapper.JsonCloneIEnumerableToIEnumerable<
//            CrmApplicantReference,
//            ApplicantReferenceDto>(references);
//      }

//      // ✅ Check cancellation before final query
//      cancellationToken.ThrowIfCancellationRequested();

//      // Load additional documents
//      var additionalDocuments = await _repository.CrmAdditionalDocuments.AdditionalDocumentsByApplicantId(
//          result.ApplicantId,
//          cancellationToken);

//      if (additionalDocuments != null && additionalDocuments.Any())
//      {
//        result.AdditionalDocuments = MyMapper.JsonCloneIEnumerableToIEnumerable<
//            AdditionalDocument,
//            AdditionalDocumentDto>(additionalDocuments);
//      }

//      return result;
//    }
//    catch (OperationCanceledException)
//    {
//      _logger.LogInformation(
//          "Application operation cancelled for ApplicationId: {ApplicationId}",
//          applicationId);
//      throw;
//    }
//    catch (Exception ex)
//    {
//      _logger.LogError(ex, "Error in Application for ApplicationId: {ApplicationId}", applicationId);
//      throw;
//    }
//  }

//  /* ========================================
//     ✅ EXAMPLE 3: CreateNewRecord with Transaction Support

//     Transaction operations এ CancellationToken especially important:
//     - Multiple database operations
//     - Rollback capability
//     - Long-running operations

//     Note: Transaction methods also need CancellationToken support
//  ======================================== */
//  public async Task<CrmApplicationDto> CreateNewRecordAsync(
//      CrmApplicationDto dto,
//      UsersDto currentUser,
//      CancellationToken cancellationToken = default) // ✅ Added with default value
//  {
//    if (dto.ApplicationId != 0)
//      throw new InvalidCreateOperationException("ApplicationId must be 0 for new record.");

//    // ✅ Check cancellation before starting transaction
//    cancellationToken.ThrowIfCancellationRequested();

//    try
//    {
//      // Set audit fields
//      dto.CreatedDate = DateTime.UtcNow;
//      dto.CreatedBy = currentUser.UserId ?? 0;
//      dto.UpdatedDate = null;
//      dto.UpdatedBy = null;

//      // ✅ Check cancellation before first database operation
//      cancellationToken.ThrowIfCancellationRequested();

//      // 1. Save CrmApplication first to get ApplicationId
//      var crmApplicationEntity = MyMapper.JsonClone<CrmApplicationDto, CrmApplication>(dto);

//      // ✅ Pass CancellationToken to CreateAndIdAsync
//      int applicationId = await _repository.CrmApplications.CreateAndIdAsync(
//          crmApplicationEntity,
//          cancellationToken);

//      dto.ApplicationId = applicationId;

//      // ✅ Check cancellation before next operation
//      cancellationToken.ThrowIfCancellationRequested();

//      // 2. Save ApplicantInfo with ApplicationId
//      if (dto.CourseInformation?.PersonalDetails != null)
//      {
//        var applicantInfoDto = dto.CourseInformation.PersonalDetails;
//        applicantInfoDto.ApplicationId = applicationId;
//        applicantInfoDto.CreatedDate = DateTime.UtcNow;
//        applicantInfoDto.CreatedBy = currentUser.UserId ?? 0;

//        var applicantInfoEntity = MyMapper.JsonClone<ApplicantInfoDto, CrmApplicantInfo>(applicantInfoDto);

//        // ✅ Pass CancellationToken
//        int applicantId = await _repository.CrmApplicantInfoes.CreateAndIdAsync(
//            applicantInfoEntity,
//            cancellationToken);

//        applicantInfoDto.ApplicantId = applicantId;

//        // Set ApplicantId in all nested DTOs
//        SetApplicantIdInAllNestedDtos(dto, applicantId);

//        // ✅ Check cancellation before next section
//        cancellationToken.ThrowIfCancellationRequested();

//        // 3. Save all other CRM entities

//        // Save ApplicantCourse
//        if (dto.CourseInformation?.ApplicantCourse != null)
//        {
//          var applicantCourseDto = dto.CourseInformation.ApplicantCourse;
//          applicantCourseDto.CreatedDate = DateTime.UtcNow;
//          applicantCourseDto.CreatedBy = currentUser.UserId ?? 0;

//          var applicantCourseEntity = MyMapper.JsonClone<ApplicantCourseDto, CrmApplicantCourse>(applicantCourseDto);

//          // ✅ Pass CancellationToken
//          var applicantCourseId = await _repository.CrmApplicantCourses.CreateAndIdAsync(
//              applicantCourseEntity,
//              cancellationToken);
//        }

//        // ✅ Check cancellation before next batch
//        cancellationToken.ThrowIfCancellationRequested();

//        // Save Permanent Address
//        if (dto.CourseInformation?.ApplicantAddress?.PermanentAddress != null)
//        {
//          var permanentAddressDto = dto.CourseInformation.ApplicantAddress.PermanentAddress;
//          permanentAddressDto.CreatedDate = DateTime.UtcNow;
//          permanentAddressDto.CreatedBy = currentUser.UserId ?? 0;

//          var permanentAddressEntity = MyMapper.JsonClone<PermanentAddressDto, CrmPermanentAddress>(permanentAddressDto);

//          // ✅ Pass CancellationToken
//          var permanentAddressId = await _repository.CrmPermanentAddresses.CreateAndIdAsync(
//              permanentAddressEntity,
//              cancellationToken);
//        }

//        // ✅ Check cancellation
//        cancellationToken.ThrowIfCancellationRequested();

//        // Save Present Address
//        if (dto.CourseInformation?.ApplicantAddress?.PresentAddress != null)
//        {
//          var presentAddressDto = dto.CourseInformation.ApplicantAddress.PresentAddress;
//          presentAddressDto.CreatedDate = DateTime.UtcNow;
//          presentAddressDto.CreatedBy = currentUser.UserId ?? 0;

//          var presentAddressEntity = MyMapper.JsonClone<PresentAddressDto, CrmPresentAddress>(presentAddressDto);

//          // ✅ Pass CancellationToken
//          var presentAddressId = await _repository.CrmPresentAddresses.CreateAndIdAsync(
//              presentAddressEntity,
//              cancellationToken);
//        }

//        // ✅ Check cancellation before loop
//        cancellationToken.ThrowIfCancellationRequested();

//        // Save Education History
//        if (dto.EducationInformation?.EducationDetails?.EducationHistory != null &&
//            dto.EducationInformation.EducationDetails.EducationHistory.Any())
//        {
//          foreach (var educationDto in dto.EducationInformation.EducationDetails.EducationHistory)
//          {
//            // ✅ Check cancellation in each iteration
//            cancellationToken.ThrowIfCancellationRequested();

//            educationDto.CreatedDate = DateTime.UtcNow;
//            educationDto.CreatedBy = currentUser.UserId ?? 0;

//            var educationEntity = MyMapper.JsonClone<EducationHistoryDto, CrmEducationHistory>(educationDto);

//            // ✅ Pass CancellationToken
//            var educationHistoryId = await _repository.CrmEducationHistories.CreateAndIdAsync(
//                educationEntity,
//                cancellationToken);
//          }
//        }

//        // ✅ Check cancellation
//        cancellationToken.ThrowIfCancellationRequested();

//        // Save IELTS Information
//        if (dto.EducationInformation?.IELTSInformation != null)
//        {
//          var ieltsDto = dto.EducationInformation.IELTSInformation;
//          ieltsDto.CreatedDate = DateTime.UtcNow;
//          ieltsDto.CreatedBy = currentUser.UserId ?? 0;

//          var ieltsEntity = MyMapper.JsonClone<IELTSInformationDto, CrmIELTSInformation>(ieltsDto);

//          // ✅ Pass CancellationToken
//          var ieltsEntityId = await _repository.CrmIELTSInformations.CreateAndIdAsync(
//              ieltsEntity,
//              cancellationToken);
//        }

//        // Continue similar pattern for:
//        // - TOEFL, PTE, GMAT, OTHERS Information
//        // - Work Experience
//        // - Applicant References
//        // - Statement of Purpose
//        // - Additional Information
//        // - Additional Documents

//        // Always:
//        // ✅ Check cancellation before expensive operations
//        // ✅ Pass CancellationToken to all async calls
//        // ✅ Check cancellation in loops
//      }

//      return dto;
//    }
//    catch (OperationCanceledException)
//    {
//      _logger.LogInformation(
//          "CreateNewRecordAsync operation cancelled for ApplicationId: {ApplicationId}",
//          dto.ApplicationId);
//      // Note: EF Core will automatically rollback on exception
//      throw;
//    }
//    catch (Exception ex)
//    {
//      _logger.LogError(ex, "Error creating CRM Application");
//      throw;
//    }
//  }

//  /* ========================================
//     ✅ EXAMPLE 4: UpdateCrmApplication with Complex Logic

//     Update operations often have:
//     - Exists checks
//     - Multiple conditional saves
//     - Bulk operations (delete, update, insert)
//  ======================================== */
//  public async Task<CrmApplicationDto> UpdateCrmApplicationAsync(
//      int key,
//      CrmApplicationDto updateDto,
//      UsersDto currentUser,
//      CancellationToken cancellationToken = default) // ✅ Added with default value
//  {
//    if (updateDto.ApplicationId == 0)
//      throw new InvalidCreateOperationException("ApplicationId must be greater than 0 for existing record.");

//    if (key != updateDto.ApplicationId)
//      throw new BadRequestException(key.ToString(), "Application Update");

//    // ✅ Check cancellation before exists check
//    cancellationToken.ThrowIfCancellationRequested();

//    // ✅ Pass CancellationToken to ExistsAsync
//    bool exists = await _repository.CrmApplications.ExistsAsync(
//        x => x.ApplicationId == key,
//        cancellationToken);

//    if (!exists)
//      throw new NotFoundException("Application", "ApplicationId", key.ToString());

//    try
//    {
//      // Set audit fields
//      updateDto.UpdatedDate = DateTime.UtcNow;
//      updateDto.UpdatedBy = currentUser.UserId ?? 0;

//      // ✅ Check cancellation before database operation
//      cancellationToken.ThrowIfCancellationRequested();

//      // 1. Update CrmApplication
//      var crmApplicationDB = await _repository.CrmApplications.FirstOrDefaultAsync(
//          expression: x => x.ApplicationId == updateDto.ApplicationId,
//          trackChanges: false,
//          cancellationToken);

//      crmApplicationDB.StateId = updateDto.StateId;
//      crmApplicationDB.ApplicationDate = (crmApplicationDB.ApplicationDate > DateTime.MinValue)
//          ? crmApplicationDB.ApplicationDate
//          : DateTime.UtcNow;
//      crmApplicationDB.CreatedDate = (crmApplicationDB.CreatedDate > DateTime.MinValue)
//          ? crmApplicationDB.CreatedDate
//          : DateTime.UtcNow;
//      crmApplicationDB.CreatedBy = (crmApplicationDB.CreatedBy > 0)
//          ? crmApplicationDB.CreatedBy
//          : (currentUser.UserId ?? 0);
//      crmApplicationDB.UpdatedDate = DateTime.UtcNow;
//      crmApplicationDB.UpdatedBy = currentUser.UserId ?? 0;

//      _repository.CrmApplications.UpdateByState(crmApplicationDB);

//      // ✅ Pass CancellationToken to SaveAsync
//      await _repository.SaveAsync(cancellationToken);

//      // ✅ Check cancellation before next section
//      cancellationToken.ThrowIfCancellationRequested();

//      // 2. Save or Update ApplicantInfo
//      if (updateDto.CourseInformation?.PersonalDetails != null)
//      {
//        var applicantInfoDto = updateDto.CourseInformation.PersonalDetails;

//        // ✅ Pass CancellationToken to ExistsAsync
//        bool applicantExists = await _repository.CrmApplicantInfoes.ExistsAsync(
//            x => x.ApplicantId == applicantInfoDto.ApplicantId && x.ApplicationId == key,
//            cancellationToken);

//        if (!applicantExists)
//        {
//          // Create new
//          applicantInfoDto.CreatedDate = DateTime.UtcNow;
//          applicantInfoDto.CreatedBy = currentUser.UserId ?? 0;

//          CrmApplicantInfo crmApplicantInfo = MyMapper.JsonClone<ApplicantInfoDto, CrmApplicantInfo>(applicantInfoDto);

//          // ✅ Pass CancellationToken
//          updateDto.CourseInformation.PersonalDetails.ApplicantId =
//              await _repository.CrmApplicantInfoes.CreateAndIdAsync(
//                  crmApplicantInfo,
//                  cancellationToken);

//          applicantInfoDto.ApplicantId = (int)updateDto.CourseInformation.PersonalDetails.ApplicantId;

//          SetApplicantIdInAllNestedDtos(updateDto, applicantInfoDto.ApplicantId);
//        }
//        else
//        {
//          // Update existing
//          var applicantInfosDB = await _repository.CrmApplicantInfoes.FirstOrDefaultAsync(
//              x => x.ApplicantId == applicantInfoDto.ApplicantId && x.ApplicationId == updateDto.ApplicationId,
//              false,
//              cancellationToken);

//          applicantInfoDto.ApplicationId = (applicantInfoDto.ApplicationId == 0 || applicantInfoDto.ApplicationId == null)
//              ? applicantInfosDB.ApplicationId
//              : applicantInfoDto.ApplicationId;

//          applicantInfoDto.CreatedDate = DateTimeFormatters.IsValidDateTime(applicantInfosDB.CreatedDate)
//              ? applicantInfosDB.CreatedDate
//              : DateTime.UtcNow;
//          applicantInfoDto.CreatedBy = (applicantInfosDB.CreatedBy > 0)
//              ? applicantInfosDB.CreatedBy
//              : (currentUser.UserId ?? 0);

//          applicantInfoDto.UpdatedDate = DateTime.UtcNow;
//          applicantInfoDto.UpdatedBy = currentUser.UserId ?? 0;
//          applicantInfosDB = MyMapper.JsonCloneSafe<ApplicantInfoDto, CrmApplicantInfo>(applicantInfoDto);
//          _repository.CrmApplicantInfoes.UpdateByState(applicantInfosDB);

//          // ✅ Pass CancellationToken
//          await _repository.SaveAsync(cancellationToken);

//          SetApplicantIdInAllNestedDtos(updateDto, applicantInfoDto.ApplicantId);
//        }

//        // Continue similar pattern for all nested entities...
//        // Always check cancellation and pass token to async operations
//      }

//      // ✅ Check cancellation before final return
//      cancellationToken.ThrowIfCancellationRequested();

//      return updateDto;
//    }
//    catch (OperationCanceledException)
//    {
//      _logger.LogInformation(
//          "UpdateCrmApplicationAsync operation cancelled for ApplicationId: {ApplicationId}",
//          key);
//      throw;
//    }
//    catch (Exception ex)
//    {
//      _logger.LogError(ex, "Error updating CRM Application: {ApplicationId}", key);
//      throw;
//    }
//  }

//  /* ========================================
//     Private Helper Methods
//  ======================================== */

//  /// <summary>
//  /// Sets ApplicantId in all nested DTOs
//  /// Note: This is synchronous, no CancellationToken needed
//  /// </summary>
//  private void SetApplicantIdInAllNestedDtos(CrmApplicationDto dto, int applicantId)
//  {
//    // Course Information Section
//    if (dto.CourseInformation != null)
//    {
//      if (dto.CourseInformation.ApplicantCourse != null)
//      {
//        dto.CourseInformation.ApplicantCourse.ApplicantId = applicantId;
//      }

//      if (dto.CourseInformation.ApplicantAddress != null)
//      {
//        if (dto.CourseInformation.ApplicantAddress.PermanentAddress != null)
//        {
//          dto.CourseInformation.ApplicantAddress.PermanentAddress.ApplicantId = applicantId;
//        }

//        if (dto.CourseInformation.ApplicantAddress.PresentAddress != null)
//        {
//          dto.CourseInformation.ApplicantAddress.PresentAddress.ApplicantId = applicantId;
//        }
//      }
//    }

//    // Education Information Section
//    if (dto.EducationInformation != null)
//    {
//      if (dto.EducationInformation.EducationDetails?.EducationHistory != null &&
//          dto.EducationInformation.EducationDetails?.EducationHistory.Count > 0)
//      {
//        foreach (var educationDto in dto.EducationInformation.EducationDetails.EducationHistory)
//        {
//          educationDto.ApplicantId = applicantId;
//        }
//      }

//      // ... rest of nested DTOs
//    }

//    // ... rest of sections
//  }
//}
