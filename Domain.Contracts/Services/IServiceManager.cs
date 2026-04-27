using Domain.Contracts.Services.Authentication;
using Domain.Contracts.Services.Core.HR;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using Domain.Contracts.Services.DMS;

namespace Domain.Contracts.Services;

public interface IServiceManager
{
  IPropertyInspectorService PropertyInspectorService { get; }
  ITokenBlacklistService TokenBlacklist { get; }
  ICrmCountryService CrmCountries { get; }
  ICurrencyService Currencies { get; }
  IHolidayService Holidays { get; }
  ITimesheetService Timesheets { get; }
  ICurrencyRateService CurrencyRates { get; }
  IThanaService Thanas { get; }
  IMaritalStatusService MaritalStatuses { get; }
  ICompetenciesService Competencies { get; }
  ICompetencyLevelService CompetencyLevels { get; }
  IBoardInstituteService BoardInstitutes { get; }
  IAuditTypeService AuditTypes { get; }
  ICompanyService Companies { get; }
  ISystemSettingsService SystemSettings { get; }
  IUsersService Users { get; }
  IAuthenticationService CustomAuthentication { get; }
  IMenuService Menus { get; }
  IModuleService Modules { get; }
  IGroupService Groups { get; }
  IQueryAnalyzerService QueryAnalyzer { get; }
  IStatusService WfState { get; }
  IAccessControlService AccessControls { get; }
  IAccessRestrictionService AccessRestrictions { get; }

  // Approver/Workflow Services
  IApproverDetailsService ApproverDetails { get; }
  IApproverHistoryService ApproverHistory { get; }
  IApproverOrderService ApproverOrders { get; }
  IApproverTypeService ApproverTypes { get; }
  IAssignApproverService AssignApprovers { get; }
  IApproverTypeToGroupMappingService ApproverTypeToGroupMappings { get; }

  // Document Management Services
  IDocumentTemplateService DocumentTemplates { get; }
  IDocumentTypeService DocumentTypes { get; }
  IDocumentParameterService DocumentParameters { get; }
  IDocumentParameterMappingService DocumentParameterMappings { get; }
  IDocumentQueryMappingService DocumentQueryMappings { get; }

  // Audit & Security Services
  IAuditLogService AuditLogs { get; }
  IAuditTrailService AuditTrails { get; }
  IAppsTokenInfoService AppsTokenInfos { get; }
  IAppsTransactionLogService AppsTransactionLogs { get; }
  IPasswordHistoryService PasswordHistories { get; }

  #region HR
  IEmployeeService Employees { get; }
  IBranchService Branches { get; }
  IDepartmentService Departments { get; }
  #endregion HR

  #region Crm
  ICrmInstituteService CrmInstitutes { get; }
  ICrmInstituteTypeService CrmInstituteTypes { get; }
  ICrmCourseService CrmCourses { get; }
  ICrmMonthService CrmMonths { get; }
  ICrmYearService CrmYears { get; }
  
  // Existing Crm services
  ICrmApplicationService CrmApplications { get; }
  ICrmScholarshipApplicationService CrmScholarshipApplications { get; }
  ICrmVisaApplicationService CrmVisaApplications { get; }
  ICrmVisaStatusHistoryService CrmVisaStatusHistories { get; }
  ICrmStudentPaymentService CrmStudentPayments { get; }
  ICrmPaymentRefundService CrmPaymentRefunds { get; }
  ICrmApplicationConditionService CrmApplicationConditions { get; }
  ICrmApplicationDocumentService CrmApplicationDocuments { get; }
  ICrmApplicantCourseService ApplicantCourses { get; }
  ICrmApplicantInfoService ApplicantInfos { get; }
  ICrmPermanentAddressService PermanentAddresses { get; }
  ICrmPresentAddressService PresentAddresses { get; }
  
  // New 10 Crm services
  ICrmEducationHistoryService EducationHistories { get; }
  ICrmIeltsInformationService IELTSInformations { get; }
  ICrmToeflInformationService TOEFLInformations { get; }
  ICrmPteInformationService PTEInformations { get; }
  ICrmGmatInformationService GMATInformations { get; }
  ICrmOthersInformationService OTHERSInformations { get; }
  ICrmWorkExperienceService WorkExperiences { get; }
  ICrmApplicantReferenceService ApplicantReferences { get; }
  ICrmStatementOfPurposeService StatementOfPurposes { get; }
  ICrmAdditionalInfoService AdditionalInfos { get; }
  ICrmAdditionalDocumentService AdditionalDocuments { get; }
  
  // New CRM services
  ICrmIntakeMonthService CrmIntakeMonths { get; }
  ICrmIntakeYearService CrmIntakeYears { get; }
  ICrmPaymentMethodService CrmPaymentMethods { get; }
  ICrmCourseIntakeService CrmCourseIntakes { get; }
  ICrmLeadSourceService CrmLeadSources { get; }
  ICrmLeadStatusService CrmLeadStatuses { get; }
  ICrmVisaTypeService CrmVisaTypes { get; }
  ICrmAgentTypeService CrmAgentTypes { get; }
  ICrmStudentStatusService CrmStudentStatuses { get; }
  ICrmOfficeService CrmOffices { get; }
  ICrmAgentService CrmAgents { get; }
  ICrmCounselorService CrmCounselors { get; }
  ICrmLeadService CrmLeads { get; }
  ICrmStudentService CrmStudents { get; }
  ICrmStudentDocumentService CrmStudentDocuments { get; }
  ICrmDocumentVerificationHistoryService CrmDocumentVerificationHistories { get; }
  ICrmStudentDocumentChecklistService CrmStudentDocumentChecklists { get; }
  ICrmStudentAcademicProfileService CrmStudentAcademicProfiles { get; }
  ICrmStudentStatusHistoryService CrmStudentStatusHistories { get; }
  ICrmEnquiryService CrmEnquiries { get; }
  ICrmFollowUpService CrmFollowUps { get; }
  ICrmFollowUpHistoryService CrmFollowUpHistories { get; }
  ICrmCounsellingSessionService CrmCounsellingSessions { get; }
  ICrmSessionProgramShortlistService CrmSessionProgramShortlists { get; }
  ICrmNoteService CrmNotes { get; }
  ICrmTaskService CrmTasks { get; }
  ICrmDegreeLevelService CrmDegreeLevels { get; }
  ICrmFacultyService CrmFaculties { get; }
  ICrmCourseFeeService CrmCourseFees { get; }
  ICrmCountryDocumentRequirementService CrmCountryDocumentRequirements { get; }
  ICrmBranchTargetService CrmBranchTargets { get; }
  ICrmSystemConfigurationService CrmSystemConfigurations { get; }
  ICrmMasterDataSuggestionService CrmMasterDataSuggestions { get; }
  ICrmAgentLeadService CrmAgentLeads { get; }
  #endregion Crm

  #region DMS
  IDmsDocumentService DmsDocuments { get; }
  IDmsDocumentTypeService DmsDocumentTypes { get; }
  IDmsDocumentTagService DmsDocumentTags { get; }
  IDmsDocumentTagMapService DmsDocumentTagMaps { get; }
  IDmsDocumentFolderService DmsDocumentFolders { get; }
  IDmsDocumentVersionService DmsDocumentVersions { get; }
  IDmsDocumentAccessLogService DmsDocumentAccessLogs { get; }
  IDmsFileUpdateHistoryService DmsFileUpdateHistories { get; }
  #endregion

  T Cache<T>(int key);
}

