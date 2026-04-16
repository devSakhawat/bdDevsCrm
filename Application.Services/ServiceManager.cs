using Domain.Exceptions;
﻿using Application.Services.Authentication;
using Application.Services.Core.HR;
using Application.Services.Core.SystemAdmin;
using Application.Services.CRM;
using Application.Services.DMS;
using Application.Services.Caching;
using bdDevsCrm.Shared.Settings;
using Domain.Contracts.Infrastructure.Security;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services;
using Domain.Contracts.Services.Authentication;
using Domain.Contracts.Services.Core.HR;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using Domain.Contracts.Services.DMS;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Services;

public sealed class ServiceManager : IServiceManager
{
  private readonly IMemoryCache _cache;
  private readonly IPasswordHasher _passwordHasher;
  private readonly IOptions<AppSettings> _appSettings;
  private readonly IHybridCacheService _hybridCache;
  //private IOptions<AppSettings>? appSettings;

  private readonly Lazy<ITokenBlacklistService> _tokenBlackListService;
  private readonly Lazy<ICrmCountryService> _countryService;
  private readonly Lazy<ICurrencyService> _currencyService;
  private readonly Lazy<ICompanyService> _companyService;
  private readonly Lazy<ISystemSettingsService> _systemSettingsService;
  private readonly Lazy<IUsersService> _userService;
  private readonly Lazy<IAuthenticationService> _authenticationService;
  private readonly Lazy<IMenuService> _menuService;
  private readonly Lazy<IModuleService> _moduleService;
  private readonly Lazy<IGroupService> _groupService;
  private readonly Lazy<IQueryAnalyzerService> _queryAnalyzerService;
  private readonly Lazy<IStatusService> _statusService;
  private readonly Lazy<IAccessControlService> _accessControlService;
  private readonly Lazy<IPropertyInspectorService> _propertyInspectorService;
  private readonly Lazy<IHolidayService> _holidayService;
  private readonly Lazy<ITimesheetService> _timesheetService;
  private readonly Lazy<ICurrencyRateService> _currencyRateService;
  private readonly Lazy<IThanaService> _thanaService;
  private readonly Lazy<IMaritalStatusService> _maritalStatusService;
  private readonly Lazy<ICompetenciesService> _competenciesService;
  private readonly Lazy<ICompetencyLevelService> _competencyLevelService;
  private readonly Lazy<IBoardInstituteService> _boardInstituteService;
  private readonly Lazy<IAuditTypeService> _auditTypeService;

  // Approver/Workflow Services
  private readonly Lazy<IApproverDetailsService> _approverDetailsService;
  private readonly Lazy<IApproverHistoryService> _approverHistoryService;
  private readonly Lazy<IApproverOrderService> _approverOrderService;
  private readonly Lazy<IApproverTypeService> _approverTypeService;
  private readonly Lazy<IAssignApproverService> _assignApproverService;
  private readonly Lazy<IApproverTypeToGroupMappingService> _approverTypeToGroupMappingService;

  // Document Management Services
  private readonly Lazy<IDocumentTemplateService> _documentTemplateService;
  private readonly Lazy<IDocumentTypeService> _documentTypeService;
  private readonly Lazy<IDocumentParameterService> _documentParameterService;
  private readonly Lazy<IDocumentParameterMappingService> _documentParameterMappingService;
  private readonly Lazy<IDocumentQueryMappingService> _documentQueryMappingService;


  #region HR Area
  private readonly Lazy<IEmployeeService> _employeeService;
  private readonly Lazy<IBranchService> _branchService;
  private readonly Lazy<IDepartmentService> _departmentService;
  #endregion HR Area

  #region CRM
  private readonly Lazy<ICrmInstituteService> _crminstitute;
  private readonly Lazy<ICrmInstituteTypeService> _crminstituteType;
  private readonly Lazy<ICrmCourseService> _crmcourse;
  private readonly Lazy<ICrmMonthService> _crmmonth;
  private readonly Lazy<ICrmYearService> _crmyear;

  private readonly Lazy<ICrmCourseIntakeService> _crmCourseIntakeService;
  // New CRM services for Intake and Payment Method
  private readonly Lazy<ICrmIntakeMonthService> _crmIntakeMonthService;
  private readonly Lazy<ICrmIntakeYearService> _crmIntakeYearService;
  private readonly Lazy<ICrmPaymentMethodService> _crmPaymentMethodService;

  // Existing Crm services
  private readonly Lazy<ICrmApplicationService> _crmApplicationService;
  private readonly Lazy<ICrmApplicantCourseService> _applicantCourseService;
  private readonly Lazy<ICrmApplicantInfoService> _applicantInfoService;
  private readonly Lazy<ICrmPermanentAddressService> _permanentAddressService;
  private readonly Lazy<ICrmPresentAddressService> _presentAddressService;

  // New 10 CRM services
  private readonly Lazy<ICrmEducationHistoryService> _educationHistoryService;
  private readonly Lazy<ICrmIeltsInformationService> _ieltsinformationService;
  private readonly Lazy<ICrmToeflInformationService> _toeflinformationService;
  private readonly Lazy<ICrmPteInformationService> _PTEInformationService;
  private readonly Lazy<ICrmGmatInformationService> _gmatinformationService;
  private readonly Lazy<ICrmOthersInformationService> _othersinformationService;
  private readonly Lazy<ICrmWorkExperienceService> _workExperienceService;
  private readonly Lazy<ICrmApplicantReferenceService> _applicantReferenceService;
  private readonly Lazy<ICrmStatementOfPurposeService> _statementOfPurposeService;
  private readonly Lazy<ICrmAdditionalInfoService> _additionalInfoService;
  private readonly Lazy<ICrmAdditionalDocumentService> _additionalDocumentService;
  #endregion CRM

  #region DMS Lazy Fields
  private readonly Lazy<IDmsDocumentService> _dmsdocumentService;
  private readonly Lazy<IDmsDocumentTypeService> _dmsdocumentTypeService;
  private readonly Lazy<IDmsFileUpdateHistoryService> _dmsFileUpdateHistoryService;
  private readonly Lazy<IDmsDocumentTagService> _dmsdocumentTagService;
  private readonly Lazy<IDmsDocumentTagMapService> _dmsdocumentTagMapService;
  private readonly Lazy<IDmsDocumentFolderService> _dmsdocumentFolderService;
  private readonly Lazy<IDmsDocumentVersionService> _dmsdocumentVersionService;
  private readonly Lazy<IDmsDocumentAccessLogService> _dmsdocumentAccessLogService;
  #endregion

  // Note: replaced ILoggerManager with ILoggerFactory to create typed ILogger<T> for each service.
  public ServiceManager(IRepositoryManager repository, ILoggerFactory loggerFactory, IConfiguration configuration, IMemoryCache cache, IHttpContextAccessor httpContextAccessor, IPasswordHasher passwordHasher, IOptions<AppSettings> appSetting, IHybridCacheService hybridCache)
  {
    _cache = cache;
    _passwordHasher = passwordHasher;
    _hybridCache = hybridCache;
    //_appSettings = appSettings;

    _propertyInspectorService = new Lazy<IPropertyInspectorService>(() => new PropertyInspectorService(configuration, loggerFactory.CreateLogger<PropertyInspectorService>(), httpContextAccessor));
    _tokenBlackListService = new Lazy<ITokenBlacklistService>(() => new TokenBlacklistService(repository, loggerFactory.CreateLogger<TokenBlacklistService>()));
    _countryService = new Lazy<ICrmCountryService>(() => new CrmCountryService(repository, loggerFactory.CreateLogger<CrmCountryService>(), configuration));
    _currencyService = new Lazy<ICurrencyService>(() => new CurrencyService(repository, loggerFactory.CreateLogger<CurrencyService>(), configuration));
    _companyService = new Lazy<ICompanyService>(() => new CompanyService(repository, loggerFactory.CreateLogger<CompanyService>(), configuration, _appSettings));
    _systemSettingsService = new Lazy<ISystemSettingsService>(() => new SystemSettingsService(repository, loggerFactory.CreateLogger<SystemSettingsService>(), configuration));
    _userService = new Lazy<IUsersService>(() => new UsersService(repository, loggerFactory.CreateLogger<UsersService>(), configuration));
    _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repository, loggerFactory.CreateLogger<AuthenticationService>(), configuration, httpContextAccessor, _passwordHasher));
    _menuService = new Lazy<IMenuService>(() => new MenuService(repository, loggerFactory.CreateLogger<MenuService>(), configuration));
    _moduleService = new Lazy<IModuleService>(() => new ModuleService(repository, loggerFactory.CreateLogger<ModuleService>(), configuration));
    _groupService = new Lazy<IGroupService>(() => new GroupService(repository, loggerFactory.CreateLogger<GroupService>(), configuration));
    _queryAnalyzerService = new Lazy<IQueryAnalyzerService>(() => new QueryAnalyzerService(repository, loggerFactory.CreateLogger<QueryAnalyzerService>(), configuration));
    _statusService = new Lazy<IStatusService>(() => new StatusService(repository, loggerFactory.CreateLogger<StatusService>(), configuration));
    _accessControlService = new Lazy<IAccessControlService>(() => new AccessControlService(repository, loggerFactory.CreateLogger<AccessControlService>(), configuration));
    _holidayService = new Lazy<IHolidayService>(() => new HolidayService(repository, _hybridCache, loggerFactory.CreateLogger<HolidayService>(), configuration));
    _timesheetService = new Lazy<ITimesheetService>(() => new TimesheetService(repository, _hybridCache, loggerFactory.CreateLogger<TimesheetService>(), configuration));
    _currencyRateService = new Lazy<ICurrencyRateService>(() => new CurrencyRateService(repository, _hybridCache, loggerFactory.CreateLogger<CurrencyRateService>(), configuration));
    _thanaService = new Lazy<IThanaService>(() => new ThanaService(repository, _hybridCache, loggerFactory.CreateLogger<ThanaService>(), configuration));
    _maritalStatusService = new Lazy<IMaritalStatusService>(() => new MaritalStatusService(repository, _hybridCache, loggerFactory.CreateLogger<MaritalStatusService>(), configuration));
    _competenciesService = new Lazy<ICompetenciesService>(() => new CompetenciesService(repository, _hybridCache, loggerFactory.CreateLogger<CompetenciesService>(), configuration));
    _competencyLevelService = new Lazy<ICompetencyLevelService>(() => new CompetencyLevelService(repository, _hybridCache, loggerFactory.CreateLogger<CompetencyLevelService>(), configuration));
    _boardInstituteService = new Lazy<IBoardInstituteService>(() => new BoardInstituteService(repository, _hybridCache, loggerFactory.CreateLogger<BoardInstituteService>(), configuration));
    _auditTypeService = new Lazy<IAuditTypeService>(() => new AuditTypeService(repository, _hybridCache, loggerFactory.CreateLogger<AuditTypeService>(), configuration));

    // Approver/Workflow Services initialization
    _approverDetailsService = new Lazy<IApproverDetailsService>(() => new ApproverDetailsService(repository, _hybridCache, loggerFactory.CreateLogger<ApproverDetailsService>(), configuration));
    _approverHistoryService = new Lazy<IApproverHistoryService>(() => new ApproverHistoryService(repository, _hybridCache, loggerFactory.CreateLogger<ApproverHistoryService>(), configuration));
    _approverOrderService = new Lazy<IApproverOrderService>(() => new ApproverOrderService(repository, _hybridCache, loggerFactory.CreateLogger<ApproverOrderService>(), configuration));
    _approverTypeService = new Lazy<IApproverTypeService>(() => new ApproverTypeService(repository, _hybridCache, loggerFactory.CreateLogger<ApproverTypeService>(), configuration));
    _assignApproverService = new Lazy<IAssignApproverService>(() => new AssignApproverService(repository, _hybridCache, loggerFactory.CreateLogger<AssignApproverService>(), configuration));
    _approverTypeToGroupMappingService = new Lazy<IApproverTypeToGroupMappingService>(() => new ApproverTypeToGroupMappingService(repository, _hybridCache, loggerFactory.CreateLogger<ApproverTypeToGroupMappingService>(), configuration));

    // Document Management Services
    _documentTemplateService = new Lazy<IDocumentTemplateService>(() => new DocumentTemplateService(repository, _hybridCache, loggerFactory.CreateLogger<DocumentTemplateService>(), configuration));
    _documentTypeService = new Lazy<IDocumentTypeService>(() => new DocumentTypeService(repository, _hybridCache, loggerFactory.CreateLogger<DocumentTypeService>(), configuration));
    _documentParameterService = new Lazy<IDocumentParameterService>(() => new DocumentParameterService(repository, _hybridCache, loggerFactory.CreateLogger<DocumentParameterService>(), configuration));
    _documentParameterMappingService = new Lazy<IDocumentParameterMappingService>(() => new DocumentParameterMappingService(repository, _hybridCache, loggerFactory.CreateLogger<DocumentParameterMappingService>(), configuration));
    _documentQueryMappingService = new Lazy<IDocumentQueryMappingService>(() => new DocumentQueryMappingService(repository, _hybridCache, loggerFactory.CreateLogger<DocumentQueryMappingService>(), configuration));

    // HR Area
    _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repository, loggerFactory.CreateLogger<EmployeeService>(), configuration));
    _branchService = new Lazy<IBranchService>(() => new BranchService(repository, loggerFactory.CreateLogger<BranchService>(), configuration));
    _departmentService = new Lazy<IDepartmentService>(() => new DepartmentService(repository, loggerFactory.CreateLogger<DepartmentService>(), configuration));

    #region CRM
    _crminstitute = new Lazy<ICrmInstituteService>(() => new CrmInstituteService(repository, loggerFactory.CreateLogger<CrmInstituteService>(), configuration, httpContextAccessor));

    _crminstituteType = new Lazy<ICrmInstituteTypeService>(() => new CrmInstituteTypeService(repository, loggerFactory.CreateLogger<CrmInstituteTypeService>(), configuration));
    _crmcourse = new Lazy<ICrmCourseService>(() => new CrmCourseService(repository, loggerFactory.CreateLogger<CrmCourseService>(), configuration, httpContextAccessor));
    _crmmonth = new Lazy<ICrmMonthService>(() => new CrmMonthService(repository, loggerFactory.CreateLogger<CrmMonthService>(), configuration));
    _crmyear = new Lazy<ICrmYearService>(() => new CrmYearService(repository, loggerFactory.CreateLogger<CrmYearService>(), configuration));

    // New CRM services initialization
    _crmIntakeMonthService = new Lazy<ICrmIntakeMonthService>(() => new CrmIntakeMonthService(repository, loggerFactory.CreateLogger<CrmIntakeMonthService>(), configuration));
    _crmIntakeYearService = new Lazy<ICrmIntakeYearService>(() => new CrmIntakeYearService(repository, loggerFactory.CreateLogger<CrmIntakeYearService>(), configuration));
    _crmPaymentMethodService = new Lazy<ICrmPaymentMethodService>(() => new CrmPaymentMethodService(repository, loggerFactory.CreateLogger<CrmPaymentMethodService>(), configuration));
    _crmCourseIntakeService = new Lazy<ICrmCourseIntakeService>(() => new CrmCourseIntakeService(repository, _hybridCache, loggerFactory.CreateLogger<CrmCourseIntakeService>(), configuration));

    // Existing Crm services initialization
    _crmApplicationService = new Lazy<ICrmApplicationService>(() => new CrmApplicationService(repository, loggerFactory.CreateLogger<CrmApplicationService>(), configuration, httpContextAccessor));
    _applicantCourseService = new Lazy<ICrmApplicantCourseService>(() => new CrmApplicantCourseService(repository, loggerFactory.CreateLogger<CrmApplicantCourseService>(), configuration, httpContextAccessor));
    _applicantInfoService = new Lazy<ICrmApplicantInfoService>(() => new CrmApplicantInfoService(repository, loggerFactory.CreateLogger<CrmApplicantInfoService>(), configuration, httpContextAccessor));
    _permanentAddressService = new Lazy<ICrmPermanentAddressService>(() => new CrmPermanentAddressService(repository, loggerFactory.CreateLogger<CrmPermanentAddressService>(), configuration, httpContextAccessor));
    _presentAddressService = new Lazy<ICrmPresentAddressService>(() => new CrmPresentAddressService(repository, loggerFactory.CreateLogger<CrmPresentAddressService>(), configuration, httpContextAccessor));

    // New 10 Crm services initialization
    _educationHistoryService = new Lazy<ICrmEducationHistoryService>(() => new CrmEducationHistoryService(repository, loggerFactory.CreateLogger<CrmEducationHistoryService>(), configuration, httpContextAccessor));
    _ieltsinformationService = new Lazy<ICrmIeltsInformationService>(() => new CrmIeltsInformationService(repository, loggerFactory.CreateLogger<CrmIeltsInformationService>(), configuration, httpContextAccessor));
    _toeflinformationService = new Lazy<ICrmToeflInformationService>(() => new CrmToeflInformationService(repository, loggerFactory.CreateLogger<CrmToeflInformationService>(), configuration, httpContextAccessor));
    _PTEInformationService = new Lazy<ICrmPteInformationService>(() => new CrmPteInformationService(repository, loggerFactory.CreateLogger<CrmPteInformationService>(), configuration, httpContextAccessor));
    _gmatinformationService = new Lazy<ICrmGmatInformationService>(() => new CrmGmatInformationService(repository, loggerFactory.CreateLogger<CrmGmatInformationService>(), configuration, httpContextAccessor));
    _othersinformationService = new Lazy<ICrmOthersInformationService>(() => new CrmOthersInformationService(repository, loggerFactory.CreateLogger<CrmOthersInformationService>(), configuration, httpContextAccessor));
    _workExperienceService = new Lazy<ICrmWorkExperienceService>(() => new CrmWorkExperienceService(repository, loggerFactory.CreateLogger<CrmWorkExperienceService>(), configuration, httpContextAccessor));
    _applicantReferenceService = new Lazy<ICrmApplicantReferenceService>(() => new CrmApplicantReferenceService(repository, loggerFactory.CreateLogger<CrmApplicantReferenceService>(), configuration, httpContextAccessor));
    _statementOfPurposeService = new Lazy<ICrmStatementOfPurposeService>(() => new CrmStatementOfPurposeService(repository, loggerFactory.CreateLogger<CrmStatementOfPurposeService>(), configuration, httpContextAccessor));
    _additionalInfoService = new Lazy<ICrmAdditionalInfoService>(() => new CrmAdditionalInfoService(repository, loggerFactory.CreateLogger<CrmAdditionalInfoService>(), configuration, httpContextAccessor));
    _additionalDocumentService = new Lazy<ICrmAdditionalDocumentService>(() => new CrmAdditionalDocumentsService(repository, loggerFactory.CreateLogger<CrmAdditionalDocumentsService>(), configuration, httpContextAccessor));
    #endregion Crm

    #region DMS Lazy Initializations
    _dmsdocumentService = new Lazy<IDmsDocumentService>(() => new DmsDocumentService(repository, loggerFactory.CreateLogger<DmsDocumentService>(), configuration, httpContextAccessor));
    _dmsdocumentTypeService = new Lazy<IDmsDocumentTypeService>(() => new DmsDocumentTypeService(repository, loggerFactory.CreateLogger<DmsDocumentTypeService>(), configuration));
    _dmsdocumentTagService = new Lazy<IDmsDocumentTagService>(() => new DmsDocumentTagService(repository, loggerFactory.CreateLogger<DmsDocumentTagService>(), configuration));
    _dmsdocumentTagMapService = new Lazy<IDmsDocumentTagMapService>(() => new DmsDocumentTagMapService(repository, loggerFactory.CreateLogger<DmsDocumentTagMapService>(), configuration));
    _dmsdocumentFolderService = new Lazy<IDmsDocumentFolderService>(() => new DmsDocumentFolderService(repository, loggerFactory.CreateLogger<DmsDocumentFolderService>(), configuration));
    _dmsdocumentVersionService = new Lazy<IDmsDocumentVersionService>(() => new DmsDocumentVersionService(repository, loggerFactory.CreateLogger<DmsDocumentVersionService>(), configuration));
    _dmsdocumentAccessLogService = new Lazy<IDmsDocumentAccessLogService>(() => new DmsDocumentAccessLogService(repository, loggerFactory.CreateLogger<DmsDocumentAccessLogService>(), configuration));
    _dmsFileUpdateHistoryService = new Lazy<IDmsFileUpdateHistoryService>(() => new DmsFileUpdateHistoryService(repository, _hybridCache, loggerFactory.CreateLogger<DmsFileUpdateHistoryService>(), configuration));
    #endregion
  }

  public IPropertyInspectorService PropertyInspectorService => _propertyInspectorService.Value;
  public ITokenBlacklistService TokenBlacklist => _tokenBlackListService.Value;
  public ICrmCountryService CrmCountries => _countryService.Value;
  public ICurrencyService Currencies => _currencyService.Value;
  public IHolidayService Holidays => _holidayService.Value;
  public ITimesheetService Timesheets => _timesheetService.Value;
  public ICurrencyRateService CurrencyRates => _currencyRateService.Value;
  public IThanaService Thanas => _thanaService.Value;
  public IMaritalStatusService MaritalStatuses => _maritalStatusService.Value;
  public ICompetenciesService Competencies => _competenciesService.Value;
  public ICompetencyLevelService CompetencyLevels => _competencyLevelService.Value;
  public IBoardInstituteService BoardInstitutes => _boardInstituteService.Value;
  public IAuditTypeService AuditTypes => _auditTypeService.Value;
  public ICompanyService Companies => _companyService.Value; // preserve original property but safe-guard compiled name
  public ISystemSettingsService SystemSettings => _systemSettingsService.Value;
  public IUsersService Users => _userService.Value;
  public IAuthenticationService CustomAuthentication => _authenticationService.Value;
  public IMenuService Menus => _menuService.Value;
  public IModuleService Modules => _moduleService.Value;
  public IGroupService Groups => _groupService.Value;
  public IQueryAnalyzerService QueryAnalyzer => _queryAnalyzerService.Value;
  public IStatusService WfState => _statusService.Value;
  public IAccessControlService AccessControls => _accessControlService.Value;

  // Approver/Workflow Services
  public IApproverDetailsService ApproverDetails => _approverDetailsService.Value;
  public IApproverHistoryService ApproverHistories => _approverHistoryService.Value;
  public IApproverOrderService ApproverOrders => _approverOrderService.Value;
  public IApproverTypeService ApproverTypes => _approverTypeService.Value;
  public IAssignApproverService AssignApprovers => _assignApproverService.Value;
  public IApproverTypeToGroupMappingService ApproverTypeToGroupMappings => _approverTypeToGroupMappingService.Value;

  // Document Management Services
  public IDocumentTemplateService DocumentTemplates => _documentTemplateService.Value;
  public IDocumentTypeService DocumentTypes => _documentTypeService.Value;
  public IDocumentParameterService DocumentParameters => _documentParameterService.Value;
  public IDocumentParameterMappingService DocumentParameterMappings => _documentParameterMappingService.Value;
  public IDocumentQueryMappingService DocumentQueryMappings => _documentQueryMappingService.Value;

  #region HR Area
  public IEmployeeService Employees => _employeeService.Value;
  public IBranchService Branches => _branchService.Value;
  public IDepartmentService departments => _departmentService.Value;
  #endregion HR Area

  #region Crm
  public ICrmInstituteService CrmInstitutes => _crminstitute.Value;
  public ICrmInstituteTypeService CrmInstituteTypes => _crminstituteType.Value;
  public ICrmCourseService CrmCourses => _crmcourse.Value;
  public ICrmMonthService CrmMonths => _crmmonth.Value;
  public ICrmYearService CrmYears => _crmyear.Value;

  // New CRM service properties
  public ICrmIntakeMonthService CrmIntakeMonths => _crmIntakeMonthService.Value;
  public ICrmIntakeYearService CrmIntakeYears => _crmIntakeYearService.Value;
  public ICrmPaymentMethodService CrmPaymentMethods => _crmPaymentMethodService.Value;
  public ICrmCourseIntakeService CrmCourseIntakes => _crmCourseIntakeService.Value;

  // Existing Crm service properties
  public ICrmApplicationService CrmApplications => _crmApplicationService.Value;
  public ICrmApplicantCourseService ApplicantCourses => _applicantCourseService.Value;
  public ICrmApplicantInfoService ApplicantInfos => _applicantInfoService.Value;
  public ICrmPermanentAddressService PermanentAddresses => _permanentAddressService.Value;
  public ICrmPresentAddressService PresentAddresses => _presentAddressService.Value;

  // New 10 Crm service properties - ALL IMPLEMENTED NOW!
  public ICrmEducationHistoryService EducationHistories => _educationHistoryService.Value;
  public ICrmIeltsInformationService IELTSInformations => _ieltsinformationService.Value;
  public ICrmToeflInformationService TOEFLInformations => _toeflinformationService.Value;
  public ICrmPteInformationService PTEInformations => _PTEInformationService.Value;
  public ICrmGmatInformationService GMATInformations => _gmatinformationService.Value;
  public ICrmOthersInformationService OTHERSInformations => _othersinformationService.Value;
  public ICrmWorkExperienceService WorkExperiences => _workExperienceService.Value;
  public ICrmApplicantReferenceService ApplicantReferences => _applicantReferenceService.Value;
  public ICrmStatementOfPurposeService StatementOfPurposes => _statementOfPurposeService.Value;
  public ICrmAdditionalInfoService AdditionalInfos => _additionalInfoService.Value;
  public ICrmAdditionalDocumentService AdditionalDocuments => _additionalDocumentService.Value;
  #endregion Crm

  #region DMS Property Exposures
  public IDmsDocumentService DmsDocuments => _dmsdocumentService.Value;
  public IDmsDocumentTypeService DmsDocumentTypes => _dmsdocumentTypeService.Value;
  public IDmsDocumentTagService DmsDocumentTags => _dmsdocumentTagService.Value;
  public IDmsDocumentTagMapService DmsDocumentTagMaps => _dmsdocumentTagMapService.Value;
  public IDmsDocumentFolderService DmsDocumentFolders => _dmsdocumentFolderService.Value;
  public IDmsDocumentVersionService DmsDocumentVersions => _dmsdocumentVersionService.Value;
  public IDmsDocumentAccessLogService DmsDocumentAccessLogs => _dmsdocumentAccessLogService.Value;
  public IDmsFileUpdateHistoryService DmsFileUpdateHistories => _dmsFileUpdateHistoryService.Value;
  #endregion


  //  Cache // generic function for getting cache from memory cache all of them.
  // This method retrieves an object from the cache using the provided key.
  // If the object is found, it returns the object; otherwise, it throws an exception with 401 status code.
  public T Cache<T>(int key)
  {
    var cacheKey = $"User_{key}";
    if (_cache.TryGetValue(cacheKey, out T value))
    {
      return value;
    }
    // If not found in cache, throw an exception
    throw new UnauthorizedAccessCRMException("User session has expired or is invalid. Please login again.");
  }



}


