using Domain.Contracts.Core.Authentication;
using Domain.Contracts.Core.HR;
using Domain.Contracts.Core.SystemAdmin;
using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Contracts.CRM;
using Domain.Contracts.DMS;
using System.Linq.Expressions;

namespace Domain.Contracts.Repositories;

public interface IRepositoryManager : IDisposable
{
  // SystemAdmin Part
  ITokenBlacklistRepository TokenBlacklists { get; }
  IRefreshTokenRepository RefreshTokens { get; }
  ICrmCountryRepository Countries { get; }
  ICompanyRepository Companies { get; }
  ISystemSettingsRepository SystemSettings { get; }
  IUsersRepository Users { get; }
  IAuthenticationRepository CustomAuthentication { get; }
  IMenuRepository Menus { get; }
  IModuleRepository Modules { get; }
  IGroupRepository Groups { get; }
  IGroupMemberRepository GroupMembers { get; }
  IQueryAnalyzerRepository QueryAnalyzers { get; }
  IStatusRepository WfStates { get; }
  IWFActionRepository WfActions { get; }
  IWorkFlowSettingsRepository Workflows { get; }
  IGroupPermissionRepository GroupPermissions { get; }
  IAccessControlRepository AccessControls { get; }
  IAccessRestrictionRepository AccessRestrictions { get; }
  ICurrencyRepository Currencies { get; }
  IHolidayRepository Holidays { get; }
  ITimesheetRepository Timesheets { get; }
  ICurrencyRateRepository CurrencyRates { get; }
  IThanaRepository Thanas { get; }
  IMaritalStatusRepository MaritalStatuses { get; }
  ICompetenciesRepository Competencies { get; }
  ICompetencyLevelRepository CompetencyLevels { get; }
  IBoardInstituteRepository BoardInstitutes { get; }
  IAuditTypeRepository AuditTypes { get; }

  // Approver/Workflow Part
  IApproverDetailsRepository ApproverDetails { get; }
  IApproverHistoryRepository ApproverHistories { get; }
  IApproverOrderRepository ApproverOrders { get; }
  IApproverTypeRepository ApproverTypes { get; }
  IAssignApproverRepository AssignApprovers { get; }
  IApproverTypeToGroupMappingRepository ApproverTypeToGroupMappings { get; }

  // Document Management Part
  IDocumentTemplateRepository DocumentTemplates { get; }
  IDocumentTypeRepository DocumentTypes { get; }
  IDocumentParameterRepository DocumentParameters { get; }
  IDocumentParameterMappingRepository DocumentParameterMappings { get; }
  IDocumentQueryMappingRepository DocumentQueryMappings { get; }

  // Audit & Security Part
  IAuditLogRepository AuditLogs { get; }
  IAuditTrailRepository AuditTrails { get; }
  IAppsTokenInfoRepository AppsTokenInfos { get; }
  IAppsTransactionLogRepository AppsTransactionLogs { get; }
  IPasswordHistoryRepository PasswordHistories { get; }

  // HR Part
  IEmployeeRepository Employees { get; }
  IEmployeeTypeRepository EmployeeTypes { get; }
  IBranchRepository Branches { get; }
  IDepartmentRepository Departments { get; }
  // instance should be small letter.

  #region CRM
  ICrmCourseRepository CrmCourses { get; }
  ICrmMonthRepository CrmMonths { get; }
  ICrmYearRepository CrmYears { get; }
  ICrmInstituteRepository CrmInstitutes { get; }
  ICrmInstituteTypeRepository CrmInstituteTypes { get; }


  // Existing CRM repositories
  ICrmApplicationRepository CrmApplications { get; }
  ICrmApplicantCourseRepository CrmApplicantCourses { get; }
  ICrmApplicantInfoRepository CrmApplicantInfos { get; }
  ICrmPermanentAddressRepository CrmPermanentAddresses { get; }
  ICrmPresentAddressRepository CrmPresentAddresses { get; }
  
  // New CRM repositories for 10 entities
  ICrmEducationHistoryRepository CrmEducationHistories { get; }
  ICrmIeltsInformationRepository CrmIeltsInformations { get; }
  ICrmToeflInformationRepository CrmToeflInformations { get; }
  ICrmPteInformationRepository CrmPteInformations { get; }
  ICrmGmatInformationRepository CrmGmatInformations { get; }
  ICrmOthersInformationRepository CrmOthersInformations { get; }
  ICrmWorkExperienceRepository CrmWorkExperiences { get; }
  ICrmApplicantReferenceRepository CrmApplicantReferences { get; }
  ICrmStatementOfPurposeRepository CrmStatementOfPurposes { get; }
  ICrmAdditionalInfoRepository CrmAdditionalInfos { get; }
  ICrmAdditionalDocumentRepository CrmAdditionalDocuments { get; }
  // New CRM repositories
  ICrmIntakeMonthRepository CrmIntakeMonths { get; }
  ICrmIntakeYearRepository CrmIntakeYears { get; }
  ICrmPaymentMethodRepository CrmPaymentMethods { get; }
  ICrmCourseIntakeRepository CrmCourseIntakes { get; }
  ICrmLeadSourceRepository CrmLeadSources { get; }
  ICrmLeadStatusRepository CrmLeadStatuses { get; }
  ICrmVisaTypeRepository CrmVisaTypes { get; }
  ICrmAgentTypeRepository CrmAgentTypes { get; }
  ICrmStudentStatusRepository CrmStudentStatuses { get; }
  ICrmOfficeRepository CrmOffices { get; }
  ICrmAgentRepository CrmAgents { get; }
  ICrmCounselorRepository CrmCounselors { get; }
  ICrmLeadRepository CrmLeads { get; }
  ICrmStudentRepository CrmStudents { get; }
  ICrmEnquiryRepository CrmEnquiries { get; }
  ICrmFollowUpRepository CrmFollowUps { get; }
  ICrmNoteRepository CrmNotes { get; }
  ICrmTaskRepository CrmTasks { get; }
  ICrmDegreeLevelRepository CrmDegreeLevels { get; }
  ICrmFacultyRepository CrmFaculties { get; }
  ICrmCourseFeeRepository CrmCourseFees { get; }
  ICrmCountryDocumentRequirementRepository CrmCountryDocumentRequirements { get; }
  ICrmBranchTargetRepository CrmBranchTargets { get; }
  ICrmSystemConfigurationRepository CrmSystemConfigurations { get; }
  ICrmMasterDataSuggestionRepository CrmMasterDataSuggestions { get; }
  ICrmAgentLeadRepository CrmAgentLeads { get; }
  #endregion CRM

  #region DMS
  IDmsDocumentRepository DmsDocuments { get; }
  IDmsDocumentTypeRepository DmsDocumentTypes { get; }
  IDmsDocumentTagRepository DmsDocumentTags { get; }
  IDmsDocumentTagMapRepository DmsDocumentTagMaps { get; }
  IDmsDocumentFolderRepository DmsDocumentFolders { get; }
  IDmsDocumentVersionRepository DmsDocumentVersions { get; }
  IDmsDocumentAccessLogRepository DmsDocumentAccessLogs { get; }
  IDmsFileUpdateHistoryRepository DmsFileUpdateHistories { get; }
	#endregion

	// Save changes to the database
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	Task SaveAsync(CancellationToken cancellationToken = default);

	void Save();
}

