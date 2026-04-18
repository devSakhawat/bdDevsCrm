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

