//using System.Security.Cryptography;

namespace bdDevs.Shared.Constants;

public static class RouteConstants
{
	public const string BaseRoute = "bdDevs-crm";

	#region Test
	public const string TestDDL = "test-ddl";
	public const string Tests = "tests";
	public const string Test = "get-test";
	public const string ReadTest = "test/key/{key}";
	public const string TestCreate = "test";
	public const string TestUpdate = "test/{key:int}";
	public const string TestDelete = "test/{key:int}";
	public const string TestSearch = "test/{key:int}";
	#endregion Test

	//#region Authentication
	//public const string Login = "login";
	//public const string UserInfo = "user-info";
	//public const string Logout = "logout";
	//#endregion Authentication

	#region Authentication
	public const string Login = "login";
	public const string UserInfo = "user-info";
	public const string Logout = "logout";
	public const string RefreshToken = "refresh-token";
	public const string RevokeToken = "revoke-token";
	#endregion Authentication

	#region Module
	//public const string ModuleSummary = "module-summary";
	//public const string Modules = "modules";
	//public const string CreateModule = "module";
	//public const string UpdateModule = "module/{key}";
	//public const string DeleteModule = "module/{key}";

	public const string CreateModule = "module";
	public const string UpdateModule = "module/{key}";
	public const string DeleteModule = "module/{key}";
	public const string ModuleSummary = "module-summary";
	public const string ReadModules = "modules";        // ← New: List all
	public const string ModuleDDL = "modules-ddl";
	public const string ReadModule = "module/{id:int}";
	#endregion Module

	#region Menu
	public const string CreateMenu = "menu";
	public const string UpdateMenu = "menu/{key}";
	public const string DeleteMenu = "menu/{key}";
	public const string MenuSummary = "menu-summary";
	public const string ReadMenus = "menus";
	public const string MenusDDL = "menus-ddl";
	public const string MenusByUserPermission = "menus-user-permission";
	public const string MenusByModuleId = "menus-moduleId/{moduleId:int}";
	public const string ParentMenuByMenu = "parent-by-menu/{parentMenuId:int}";
	public const string ReadMenu = "menu/{menuId:int}";
	#endregion Menu

	#region Companies
	public const string Companies = "companies";
	public const string CompaniesDDL = "companies-ddl";
	public const string MotherCompany = "mother-companies";
	public const string ReadCompany = "company/key/{key}";
	public const string CreateCompany = "company";
	public const string UpdateCompany = "company/{key}";
	public const string DeleteCompany = "company/{key}";
	#endregion Companies

	#region Group
	//public const string Groups = "groups";
	//public const string GroupById = "group/key/{key}";
	//public const string GroupPermisionsbyGroupId = "grouppermission/{groupId:int}";
	//public const string Accesses = "getaccess";
	//public const string GroupSummary = "group-summary";
	//public const string CreateGroup = "group";
	//public const string ReadGroup = "group/key/{key}";
	//public const string UpdateGroup = "group/{groupId:int}";
	//public const string DeleteGroup = "group/{key}";
	//public const string GroupsByCompany = "groups/companyId";

	//public const string GroupsByUserId = "groups/{usersUserId:int}";

	//public const string GroupMemberByUserId = "groups/group-members-by-userId/";
	//public const string AccessPermisionForCurrentUser = "groups/accesspermisionforcurrentuser";

	// CUD Operations
	public const string CreateGroup = "group";
	public const string UpdateGroup = "group/{key}";
	public const string DeleteGroup = "group/{key}";

	// Read Operations (High to Low Data Volume)
	public const string GroupSummary = "group-summary";           // Grid/Paginated (Largest)
	public const string ReadGroups = "groups";                    // List All (Medium)
	public const string GroupForDDL = "groups-ddl";               // Dropdown (Small)
	public const string ReadGroup = "group/{groupId:int}";        // Single by ID (Smallest)

	// Specialized Queries
	public const string GroupPermissionsByGroupId = "group-permissions/{groupId:int}";
	public const string AccessControls = "access-controls";
	#endregion Group

	#region QueryAnalyzer
	public const string QueryAnalyzers = "query-analyzers";
	public const string CustomizedReportInfo = "customized-report";
	#endregion QueryAnalyzer

	#region Status
	//public const string StatusByMenuId = "status/key/";
	//public const string StatusByMenuId = "status/key/{menuId:int}";
	public const string StatusByMenuName = "status/menu/{key}";
	//public const string ActionsByStatusIdForGroup = "actions-4-group/status/";
	public const string StatusByMenuNUserId = "Status-MenuId-UserId";
	#endregion Status

	#region WorkFlow
	//public const string WorkFlowSummary = "workflow-summary";
	//public const string CreateWorkFlow = "workflow";
	//public const string UpdateWorkFlow = "workflow/{key:int}";
	//public const string DeleteWorkFlow = "workflow/{key:int}";
	//public const string CreateAction = "wf-action";
	//public const string UpdateAction = "wf-action/{key}";
	//public const string DeleteAction = "wf-action/{key}";
	//public const string NextStatesByMenu = "next-states-by-menu/";
	//public const string ActionSummaryByStatusId = "get-action-summary-by-statusId/";
	////public const string ActionsByStatusIdForGroup = "actions-4-group/status/";

	// CUD Operations
	public const string CreateWorkflowState = "workflow-state";
	public const string UpdateWorkflowState = "workflow-state/{key}";
	public const string DeleteWorkflowState = "workflow-state/{key}";

	// Read Operations (High to Low Data Volume)
	public const string WorkflowSummary = "workflow-summary";                  // Grid/Paginated (Largest)
	public const string StatusByMenuId = "statuses-by-menu/{menuId:int}";      // List by Menu (Medium)
	public const string ActionsByStatusIdForGroup = "actions-by-status/{statusId:int}"; // List by Status (Small)

	// CUD Operations for Action
	public const string CreateWorkflowAction = "workflow-action";
	public const string UpdateWorkflowAction = "workflow-action/{key}";
	public const string DeleteWorkflowAction = "workflow-action/{key}";
	#endregion WorkFlow

	#region AccessControl
	public const string AccessControlSummary = "access-control-summary";
	public const string ReadAccessControls = "access-controls";
	public const string ReadAccessControl = "access-control/key/{key:int}";
	public const string CreateAccessControl = "access-control";
	public const string UpdateAccessControl = "access-control/{key}";
	public const string DeleteAccessControl = "access-control/{key}";
	#endregion AccessControl

	#region user
	//public const string UserSummary = "user-summary";
	//public const string Users = "users";
	//public const string SaveUser = "user";
	//public const string UpdateUser = "user/{key}";

	// CUD Operations
	public const string CreateUser = "user";
	public const string UpdateUser = "user/{key}";
	public const string DeleteUser = "user/{key}";

	// Read Operations (High to Low Data Volume)
	public const string UserSummary = "user-summary";           // Grid/Paginated (Largest)
	public const string ReadUsers = "users";                    // List All (Medium)
	public const string UserForDDL = "users-ddl";               // Dropdown (Small)
	public const string ReadUser = "user/{id:int}";             // Single by ID (Smallest)
	#endregion user

	#region Employee
	public const string EmployeeTypes = "employeetypes";
	// Indentity means : companyId, branchId, departmentId
	public const string EmployeesByCompanyIdAndBranchIdAndDepartmentId = "employees-by-indentities";
	#endregion Employee

	#region Branch
	public const string BranchesByCompanyIdCombo = "branches/{companyId:int}";
	#endregion Branch

	#region Department
	public const string DepartmentByCompanyIdForCombo = "departments-by-compnayId/companyId/";
	#endregion Department

	#region Country
	public const string CountryDDL = "countryddl";
	public const string CountrySummary = "country-summary";
	public const string ReadCountries = "countries";
	public const string ReadCountry = "country/{countryId:int}";
	public const string CreateCountry = "country";
	public const string UpdateCountry = "country/{key}";
	public const string DeleteCountry = "country/{key}";

	public const string CreateOrUpdateCountry = "country/{key}";
	#endregion CrmCountry

	#region InstituteType
	public const string InstituteTypeDDL = "crm-institutetype-ddl";
	public const string InstituteTypeSummary = "crm-institutetype-summary";
	public const string CreateInstituteType = "crm-institutetype";
	public const string UpdateInstituteType = "crm-institutetype/{key:int}";
	public const string DeleteInstituteType = "crm-institutetype/{key:int}";
	public const string CreateOrUpdateInstituteType = "crm-institutetype-saveORupdate/{key:int}";
	#endregion InstituteType

	#region Institute
	public const string InstituteDDL = "crm-institute-ddl";
	public const string InstituteSummary = "crm-institute-summary";
	public const string CreateInstitute = "crm-institute";
	public const string UpdateInstitute = "crm-institute/{key:int}";
	public const string DeleteInstitute = "crm-institute/{key:int}";
	public const string CreateOrUpdateInstitute = "institute-saveORupdate/{key:int}";

	public const string InstituteDDLByCountryId = "crm-institut-by-countryid-ddl/{countryId:int}";
	#endregion Institute

	#region Course
	public const string CourseDDL = "crm-course-ddl";
	public const string CourseSummary = "crm-course-summary";
	public const string CreateCourse = "crm-course";
	public const string UpdateCourse = "crm-course/{key:int}";
	public const string DeleteCourse = "crm-course/{key:int}";
	public const string CreateOrUpdateCourse = "course-saveORupdate/{key:int}";


	public const string CourseByInstituteIdDDL = "crm-course-by-instituteid-ddl/{instituteId:int}";
	#endregion Course

	#region Currency
	//public const string CurrencyDDL = "currencyddl";
	//public const string CurrencySummary = "currency-summary";
	//public const string CreateCurrency = "currency";
	//public const string CreateOrUpdateCurrency = "currency/{key}";
	//public const string DeleteCurrency = "currency/{key}";

	public const string CreateCurrency = "currency";
	public const string UpdateCurrency = "currency/{key}";
	public const string DeleteCurrency = "currency/{key}";
	public const string CurrencySummary = "currency-summary";
	public const string ReadCurrencies = "currencies";        // ← New: List all
	public const string CurrencyDDL = "currencies-ddl";
	public const string ReadCurrency = "currency/{id:int}";
	#endregion Currency

	// routes for Intake Month, Year and Payment Method
	#region IntakeMonth
	// use this for passing id in query
	public const string IntakeMonthByKey = "intake-month/key";
	public const string IntakeMonthDDL = "intake-month-ddl";
	public const string IntakeMonthSummary = "intake-month-summary";
	public const string CreateIntakeMonth = "intake-month";
	public const string UpdateIntakeMonth = "intake-month/{key:int}";
	public const string DeleteIntakeMonth = "intake-month/{key:int}";
	public const string CreateOrUpdateIntakeMonth = "intake-month-saveORupdate/{key:int}";
	#endregion IntakeMonth

	#region IntakeYear
	public const string IntakeYearByKey = "intake-year/key";
	public const string IntakeYearDDL = "intake-year-ddl";
	public const string IntakeYearSummary = "intake-year-summary";
	public const string CreateIntakeYear = "intake-year";
	public const string UpdateIntakeYear = "intake-year/{key:int}";
	public const string DeleteIntakeYear = "intake-year/{key:int}";
	public const string CreateOrUpdateIntakeYear = "intake-year-saveORupdate/{key:int}";
	#endregion IntakeYear

	#region PaymentMethod
	public const string PaymentMethodByKey = "payment-method/key";
	public const string PaymentMethodDDL = "payment-method-ddl";
	public const string OnlinePaymentMethodDDL = "online-payment-method-ddl";
	public const string PaymentMethodSummary = "payment-method-summary";
	public const string CreatePaymentMethod = "payment-method";
	public const string UpdatePaymentMethod = "payment-method/{key:int}";
	public const string DeletePaymentMethod = "payment-method/{key:int}";
	public const string CreateOrUpdatePaymentMethod = "payment-method-saveORupdate/{key:int}";
	#endregion PaymentMethod

	// CRM Application related routes
	#region CrmApplication
	public const string CRMApplicationStatus = "crm-application-status";

	public const string CRMApplicationSummary = "crm-application-summary/{statusId:int}";
	public const string CRMApplicationCreate = "crm-application";
	public const string CRMApplicationByKey = "crm-application/key/{key}";
	public const string CRMApplicationUpdate = "crm-Application/{key:int}";
	public const string CRMApplicationDelete = "crm-Application/{key}";
	public const string CRMCountryDLL = "crm-countryddl";
	public const string CRMInstituteDLLByCountry = "crm-instituteddl-country";
	public const string CRMCourseDLLByInstitute = "crm-courseddl-institute";
	public const string CRMMonthDLL = "crm-monthddl";
	public const string CRMYearDLL = "crm-yearddl";
	public const string CRMInstituteTypeDDL = "crm-institute-type/";
	public const string CRMApplicationByApplicationId = "crm-application-by-applicationId/{applicationId:int}";
	#endregion CrmApplication

	#region ApplicantCourse
	public const string ApplicantCourseDDL = "applicant-course-ddl";
	public const string ApplicantCourseSummary = "applicant-course-summary";
	public const string CreateApplicantCourse = "applicant-course";
	public const string UpdateApplicantCourse = "applicant-course/{key:int}";
	public const string DeleteApplicantCourse = "applicant-course/{key:int}";
	public const string ApplicantCoursesByApplicantId = "applicant-courses-by-applicantid/{applicantId:int}";
	#endregion ApplicantCourse

	#region ApplicantInfo
	public const string ApplicantInfoDDL = "applicant-info-ddl";
	public const string ApplicantInfoSummary = "applicant-info-summary";
	public const string CreateApplicantInfo = "applicant-info";
	public const string UpdateApplicantInfo = "applicant-info/{key:int}";
	public const string DeleteApplicantInfo = "applicant-info/{key:int}";
	public const string ApplicantInfoByApplicationId = "applicant-info-by-applicationid/{applicationId:int}";
	#endregion ApplicantInfo

	#region PermanentAddress
	public const string PermanentAddressDDL = "permanent-address-ddl";
	public const string PermanentAddressSummary = "permanent-address-summary";
	public const string CreatePermanentAddress = "permanent-address";
	public const string UpdatePermanentAddress = "permanent-address/{key:int}";
	public const string DeletePermanentAddress = "permanent-address/{key:int}";
	public const string PermanentAddressByApplicantId = "permanent-address-by-applicantid/{applicantId:int}";
	#endregion PermanentAddress

	#region PresentAddress
	public const string PresentAddressDDL = "present-address-ddl";
	public const string PresentAddressSummary = "present-address-summary";
	public const string CreatePresentAddress = "present-address";
	public const string UpdatePresentAddress = "present-address/{key:int}";
	public const string DeletePresentAddress = "present-address/{key:int}";
	public const string PresentAddressByApplicantId = "present-address-by-applicantid/{applicantId:int}";
	#endregion PresentAddress

	#region EducationHistory
	public const string EducationHistoryDDL = "education-history-ddl";
	public const string EducationHistorySummary = "education-history-summary";
	public const string CreateEducationHistory = "education-history";
	public const string UpdateEducationHistory = "education-history/{key:int}";
	public const string DeleteEducationHistory = "education-history/{key:int}";
	public const string EducationHistoryByApplicantId = "education-history-by-applicantid/{applicantId:int}";
	#endregion EducationHistory

	#region IELTSInformation
	public const string IELTSInformationDDL = "ielts-information-ddl";
	public const string IELTSInformationSummary = "ielts-information-summary";
	public const string CreateIELTSInformation = "ielts-information";
	public const string UpdateIELTSInformation = "ielts-information/{key:int}";
	public const string DeleteIELTSInformation = "ielts-information/{key:int}";
	public const string IELTSInformationByApplicantId = "ielts-information-by-applicantid/{applicantId:int}";
	#endregion IELTSInformation

	#region TOEFLInformation
	public const string ToeflInformationDDL = "toefl-information-ddl";
	public const string ToeflInformationSummary = "toefl-information-summary";
	public const string CreateToeflInformation = "toefl-information";
	public const string UpdateToeflInformation = "toefl-information/{key:int}";
	public const string DeleteToeflInformation = "toefl-information/{key:int}";
	public const string ToeflInformationByApplicantId = "toefl-information-by-applicantid/{applicantId:int}";
	#endregion TOEFLInformation

	#region PTEInformation
	public const string PTEInformationDDL = "pte-information-ddl";
	public const string PTEInformationSummary = "pte-information-summary";
	public const string CreatePTEInformation = "pte-information";
	public const string UpdatePTEInformation = "pte-information/{key:int}";
	public const string DeletePTEInformation = "pte-information/{key:int}";
	public const string PTEInformationByApplicantId = "pte-information-by-applicantid/{applicantId:int}";
	#endregion PTEInformation

	#region GMATInformation
	public const string GmatInformationDDL = "gmat-information-ddl";
	public const string GmatInformationSummary = "gmat-information-summary";
	public const string CreateGmatInformation = "gmat-information";
	public const string UpdateGmatInformation = "gmat-information/{key:int}";
	public const string DeleteGmatInformation = "gmat-information/{key:int}";
	public const string GmatInformationByApplicantId = "gmat-information-by-applicantid/{applicantId:int}";
	#endregion GMATInformation

	#region OTHERSInformation
	public const string OthersInformationDDL = "others-information-ddl";
	public const string OthersInformationSummary = "others-information-summary";
	public const string CreateOthersInformation = "others-information";
	public const string UpdateOthersInformation = "others-information/{key:int}";
	public const string DeleteOthersInformation = "others-information/{key:int}";
	public const string OthersInformationByApplicantId = "others-information-by-applicantid/{applicantId:int}";
	#endregion OTHERSInformation

	#region WorkExperience
	public const string WorkExperienceDDL = "work-experience-ddl";
	public const string WorkExperienceSummary = "work-experience-summary";
	public const string CreateWorkExperience = "work-experience";
	public const string UpdateWorkExperience = "work-experience/{key:int}";
	public const string DeleteWorkExperience = "work-experience/{key:int}";
	public const string WorkExperienceByApplicantId = "work-experience-by-applicantid/{applicantId:int}";
	#endregion WorkExperience

	#region ApplicantReference
	public const string ApplicantReferenceDDL = "applicant-reference-ddl";
	public const string ApplicantReferenceSummary = "applicant-reference-summary";
	public const string CreateApplicantReference = "applicant-reference";
	public const string UpdateApplicantReference = "applicant-reference/{key:int}";
	public const string DeleteApplicantReference = "applicant-reference/{key:int}";
	public const string ApplicantReferenceByApplicantId = "applicant-reference-by-applicantid/{applicantId:int}";
	#endregion ApplicantReference

	#region StatementOfPurpose
	public const string StatementOfPurposeDDL = "statement-of-purpose-ddl";
	public const string StatementOfPurposeSummary = "statement-of-purpose-summary";
	public const string CreateStatementOfPurpose = "statement-of-purpose";
	public const string UpdateStatementOfPurpose = "statement-of-purpose/{key:int}";
	public const string DeleteStatementOfPurpose = "statement-of-purpose/{key:int}";
	public const string StatementOfPurposeByApplicantId = "statement-of-purpose-by-applicantid/{applicantId:int}";
	#endregion StatementOfPurpose

	#region AdditionalInfo
	public const string AdditionalInfoDDL = "additional-info-ddl";
	public const string AdditionalInfoSummary = "additional-info-summary";
	public const string CreateAdditionalInfo = "additional-info";
	public const string UpdateAdditionalInfo = "additional-info/{key:int}";
	public const string DeleteAdditionalInfo = "additional-info/{key:int}";
	public const string AdditionalInfoByApplicantId = "additional-info-by-applicantid/{applicantId:int}";
	#endregion AdditionalInfo

	#region Holiday
	public const string CreateHoliday = "holiday";
	public const string UpdateHoliday = "holiday/{key}";
	public const string DeleteHoliday = "holiday/{key}";
	public const string HolidaySummary = "holiday-summary";
	public const string ReadHolidays = "holidays";
	public const string HolidayDDL = "holidays-ddl";
	public const string ReadHoliday = "holiday/{id:int}";
	#endregion Holiday

	#region Timesheet
	public const string CreateTimesheet = "timesheet";
	public const string UpdateTimesheet = "timesheet/{key}";
	public const string DeleteTimesheet = "timesheet/{key}";
	public const string TimesheetSummary = "timesheet-summary";
	public const string ReadTimesheets = "timesheets";
	public const string TimesheetDDL = "timesheets-ddl";
	public const string ReadTimesheet = "timesheet/{id:int}";
	public const string TimesheetsByEmployee = "timesheets-by-employee/{hrRecordId:int}";
	#endregion Timesheet

	#region CurrencyRate
	public const string CreateCurrencyRate = "currency-rate";
	public const string UpdateCurrencyRate = "currency-rate/{key}";
	public const string DeleteCurrencyRate = "currency-rate/{key}";
	public const string CurrencyRateSummary = "currency-rate-summary";
	public const string ReadCurrencyRates = "currency-rates";
	public const string CurrencyRateDDL = "currency-rates-ddl";
	public const string ReadCurrencyRate = "currency-rate/{id:int}";
	public const string CurrencyRatesByCurrency = "currency-rates-by-currency/{currencyId:int}";
	#endregion CurrencyRate

	#region Thana
	public const string CreateThana = "thana";
	public const string UpdateThana = "thana/{key}";
	public const string DeleteThana = "thana/{key}";
	public const string ThanaSummary = "thana-summary";
	public const string ReadThanas = "thanas";
	public const string ThanaDDL = "thanas-ddl";
	public const string ReadThana = "thana/{id:int}";
	public const string ThanasByDistrict = "thanas-by-district/{districtId:int}";
	#endregion Thana

	#region CrmCourseIntake
	public const string CreateCrmCourseIntake = "crm-course-intake";
	public const string UpdateCrmCourseIntake = "crm-course-intake/{key}";
	public const string DeleteCrmCourseIntake = "crm-course-intake/{key}";
	public const string CrmCourseIntakeSummary = "crm-course-intake-summary";
	public const string ReadCrmCourseIntakes = "crm-course-intakes";
	public const string CrmCourseIntakeDDL = "crm-course-intakes-ddl";
	public const string ReadCrmCourseIntake = "crm-course-intake/{id:int}";
	public const string CrmCourseIntakesByCourse = "crm-course-intakes-by-course/{courseId:int}";
	#endregion CrmCourseIntake

	#region CrmApplicantInfo
	public const string CreateCrmApplicantInfo = "crm-applicant-info";
	public const string UpdateCrmApplicantInfo = "crm-applicant-info/{key}";
	public const string DeleteCrmApplicantInfo = "crm-applicant-info/{key}";
	public const string CrmApplicantInfoSummary = "crm-applicant-info-summary";
	public const string ReadCrmApplicantInfos = "crm-applicant-infos";
	public const string CrmApplicantInfoDDL = "crm-applicant-infos-ddl";
	public const string ReadCrmApplicantInfo = "crm-applicant-info/{id:int}";
	public const string CrmApplicantInfoByApplicationId = "crm-applicant-info-by-application/{applicationId:int}";
	public const string CrmApplicantInfoByEmail = "crm-applicant-info-by-email";
	#endregion CrmApplicantInfo

	#region CrmApplication
	public const string CreateCrmApplication = "crm-application";
	public const string UpdateCrmApplication = "crm-application/{key}";
	public const string DeleteCrmApplication = "crm-application/{key}";
	public const string CrmApplicationSummary = "crm-application-summary";
	public const string ReadCrmApplications = "crm-applications";
	public const string ReadCrmApplication = "crm-application/{id:int}";
	public const string CrmApplicationsByStudentId = "crm-applications-by-student/{studentId:int}";
	public const string CrmApplicationStatusTransition = "crm-application-status-transition";
	public const string CrmApplicationBoard = "crm-application-board";
	#endregion CrmApplication

	#region CrmApplicationCondition
	public const string CreateCrmApplicationCondition = "crm-application-condition";
	public const string UpdateCrmApplicationCondition = "crm-application-condition/{key}";
	public const string DeleteCrmApplicationCondition = "crm-application-condition/{key}";
	public const string CrmApplicationConditionSummary = "crm-application-condition-summary";
	public const string ReadCrmApplicationConditions = "crm-application-conditions";
	public const string ReadCrmApplicationCondition = "crm-application-condition/{id:int}";
	public const string ApplicationConditionsByApplicationId = "crm-application-conditions-by-application/{applicationId:int}";
	public const string CrmApplicationConditionStatusTransition = "crm-application-condition-status-transition";
	#endregion CrmApplicationCondition

	#region CrmApplicationDocument
	public const string CreateCrmApplicationDocument = "crm-application-document";
	public const string UpdateCrmApplicationDocument = "crm-application-document/{key}";
	public const string DeleteCrmApplicationDocument = "crm-application-document/{key}";
	public const string CrmApplicationDocumentSummary = "crm-application-document-summary";
	public const string ReadCrmApplicationDocuments = "crm-application-documents";
	public const string ReadCrmApplicationDocument = "crm-application-document/{id:int}";
	public const string ApplicationDocumentsByApplicationId = "crm-application-documents-by-application/{applicationId:int}";
	#endregion CrmApplicationDocument

	#region CrmApplicantCourse
	public const string CreateCrmApplicantCourse = "crm-applicant-course";
	public const string UpdateCrmApplicantCourse = "crm-applicant-course/{key}";
	public const string DeleteCrmApplicantCourse = "crm-applicant-course/{key}";
	public const string CrmApplicantCourseSummary = "crm-applicant-course-summary";
	public const string ReadCrmApplicantCourses = "crm-applicant-courses";
	public const string ReadCrmApplicantCourse = "crm-applicant-course/{id:int}";
	public const string CrmApplicantCoursesByApplicationId = "crm-applicant-courses-by-application/{applicationId:int}";
	#endregion CrmApplicantCourse

	#region CrmCourse
	public const string CreateCrmCourse = "crm-course";
	public const string UpdateCrmCourse = "crm-course/{key}";
	public const string DeleteCrmCourse = "crm-course/{key}";
	public const string CrmCourseSummary = "crm-course-summary";
	public const string ReadCrmCourses = "crm-courses";
	public const string CrmCourseDDL = "crm-courses-ddl";
	public const string ReadCrmCourse = "crm-course/{id:int}";
	public const string CrmCoursesByInstituteId = "crm-courses-by-institute/{instituteId:int}";
	#endregion CrmCourse

	#region CrmInstitute
	public const string CreateCrmInstitute = "crm-institute";
	public const string UpdateCrmInstitute = "crm-institute/{key}";
	public const string DeleteCrmInstitute = "crm-institute/{key}";
	public const string CrmInstituteSummary = "crm-institute-summary";
	public const string ReadCrmInstitutes = "crm-institutes";
	public const string CrmInstituteDDL = "crm-institutes-ddl";
	public const string ReadCrmInstitute = "crm-institute/{id:int}";
	public const string CrmInstitutesByCountryId = "crm-institutes-by-country/{countryId:int}";
	#endregion CrmInstitute

	#region CrmInstituteType
	public const string CreateCrmInstituteType = "crm-institute-type";
	public const string UpdateCrmInstituteType = "crm-institute-type/{key}";
	public const string DeleteCrmInstituteType = "crm-institute-type/{key}";
	public const string CrmInstituteTypeSummary = "crm-institute-type-summary";
	public const string ReadCrmInstituteTypes = "crm-institute-types";
	public const string ReadCrmInstituteType = "crm-institute-type/{id:int}";
	#endregion CrmInstituteType

	#region CrmEducationHistory
	public const string CreateCrmEducationHistory = "crm-education-history";
	public const string UpdateCrmEducationHistory = "crm-education-history/{key}";
	public const string DeleteCrmEducationHistory = "crm-education-history/{key}";
	public const string CrmEducationHistorySummary = "crm-education-history-summary";
	public const string ReadCrmEducationHistories = "crm-education-histories";
	public const string ReadCrmEducationHistory = "crm-education-history/{id:int}";
	#endregion CrmEducationHistory

	#region CrmWorkExperience
	public const string CreateCrmWorkExperience = "crm-work-experience";
	public const string UpdateCrmWorkExperience = "crm-work-experience/{key}";
	public const string DeleteCrmWorkExperience = "crm-work-experience/{key}";
	public const string CrmWorkExperienceSummary = "crm-work-experience-summary";
	public const string ReadCrmWorkExperiences = "crm-work-experiences";
	public const string ReadCrmWorkExperience = "crm-work-experience/{id:int}";
	#endregion CrmWorkExperience

	#region CrmPresentAddress
	public const string CreateCrmPresentAddress = "crm-present-address";
	public const string UpdateCrmPresentAddress = "crm-present-address/{key}";
	public const string DeleteCrmPresentAddress = "crm-present-address/{key}";
	public const string CrmPresentAddressSummary = "crm-present-address-summary";
	public const string ReadCrmPresentAddresses = "crm-present-addresses";
	public const string ReadCrmPresentAddress = "crm-present-address/{id:int}";
	#endregion CrmPresentAddress

	#region CrmPermanentAddress
	public const string CreateCrmPermanentAddress = "crm-permanent-address";
	public const string UpdateCrmPermanentAddress = "crm-permanent-address/{key}";
	public const string DeleteCrmPermanentAddress = "crm-permanent-address/{key}";
	public const string CrmPermanentAddressSummary = "crm-permanent-address-summary";
	public const string ReadCrmPermanentAddresses = "crm-permanent-addresses";
	public const string ReadCrmPermanentAddress = "crm-permanent-address/{id:int}";
	#endregion CrmPermanentAddress

	#region CrmApplicantReference
	public const string CreateCrmApplicantReference = "crm-applicant-reference";
	public const string UpdateCrmApplicantReference = "crm-applicant-reference/{key}";
	public const string DeleteCrmApplicantReference = "crm-applicant-reference/{key}";
	public const string CrmApplicantReferenceSummary = "crm-applicant-reference-summary";
	public const string ReadCrmApplicantReferences = "crm-applicant-references";
	public const string ReadCrmApplicantReference = "crm-applicant-reference/{id:int}";
	#endregion CrmApplicantReference

	#region CrmIeltsInformation
	public const string CreateCrmIeltsInformation = "crm-ielts-information";
	public const string UpdateCrmIeltsInformation = "crm-ielts-information/{key}";
	public const string DeleteCrmIeltsInformation = "crm-ielts-information/{key}";
	public const string CrmIeltsInformationSummary = "crm-ielts-information-summary";
	public const string ReadCrmIeltsInformations = "crm-ielts-informations";
	public const string ReadCrmIeltsInformation = "crm-ielts-information/{id:int}";
	#endregion CrmIeltsInformation

	#region CrmToeflInformation
	public const string CreateCrmToeflInformation = "crm-toefl-information";
	public const string UpdateCrmToeflInformation = "crm-toefl-information/{key}";
	public const string DeleteCrmToeflInformation = "crm-toefl-information/{key}";
	public const string CrmToeflInformationSummary = "crm-toefl-information-summary";
	public const string ReadCrmToeflInformations = "crm-toefl-informations";
	public const string ReadCrmToeflInformation = "crm-toefl-information/{id:int}";
	#endregion CrmToeflInformation

	#region CrmGmatInformation
	public const string CreateCrmGmatInformation = "crm-gmat-information";
	public const string UpdateCrmGmatInformation = "crm-gmat-information/{key}";
	public const string DeleteCrmGmatInformation = "crm-gmat-information/{key}";
	public const string CrmGmatInformationSummary = "crm-gmat-information-summary";
	public const string ReadCrmGmatInformations = "crm-gmat-informations";
	public const string ReadCrmGmatInformation = "crm-gmat-information/{id:int}";
	#endregion CrmGmatInformation

	#region CrmStatementOfPurpose
	public const string CreateCrmStatementOfPurpose = "crm-statement-of-purpose";
	public const string UpdateCrmStatementOfPurpose = "crm-statement-of-purpose/{key}";
	public const string DeleteCrmStatementOfPurpose = "crm-statement-of-purpose/{key}";
	public const string CrmStatementOfPurposeSummary = "crm-statement-of-purpose-summary";
	public const string ReadCrmStatementOfPurposes = "crm-statement-of-purposes";
	public const string ReadCrmStatementOfPurpose = "crm-statement-of-purpose/{id:int}";
	#endregion CrmStatementOfPurpose

	#region CrmAdditionalInfo
	public const string CreateCrmAdditionalInfo = "crm-additional-info";
	public const string UpdateCrmAdditionalInfo = "crm-additional-info/{key}";
	public const string DeleteCrmAdditionalInfo = "crm-additional-info/{key}";
	public const string CrmAdditionalInfoSummary = "crm-additional-info-summary";
	public const string ReadCrmAdditionalInfos = "crm-additional-infos";
	public const string ReadCrmAdditionalInfo = "crm-additional-info/{id:int}";
	#endregion CrmAdditionalInfo

	#region CrmAdditionalDocument
	public const string CreateCrmAdditionalDocument = "crm-additional-document";
	public const string UpdateCrmAdditionalDocument = "crm-additional-document/{key}";
	public const string DeleteCrmAdditionalDocument = "crm-additional-document/{key}";
	public const string CrmAdditionalDocumentSummary = "crm-additional-document-summary";
	public const string ReadCrmAdditionalDocuments = "crm-additional-documents";
	public const string ReadCrmAdditionalDocument = "crm-additional-document/{id:int}";
	#endregion CrmAdditionalDocument

	#region CrmOthersInformation
	public const string CreateCrmOthersInformation = "crm-others-information";
	public const string UpdateCrmOthersInformation = "crm-others-information/{key}";
	public const string DeleteCrmOthersInformation = "crm-others-information/{key}";
	public const string CrmOthersInformationSummary = "crm-others-information-summary";
	public const string ReadCrmOthersInformations = "crm-others-informations";
	public const string ReadCrmOthersInformation = "crm-others-information/{id:int}";
	#endregion CrmOthersInformation

	#region CrmPaymentMethod
	public const string CreateCrmPaymentMethod = "crm-payment-method";
	public const string UpdateCrmPaymentMethod = "crm-payment-method/{key}";
	public const string DeleteCrmPaymentMethod = "crm-payment-method/{key}";
	public const string CrmPaymentMethodSummary = "crm-payment-method-summary";
	public const string ReadCrmPaymentMethods = "crm-payment-methods";
	public const string ReadCrmPaymentMethod = "crm-payment-method/{id:int}";
	#endregion CrmPaymentMethod

	#region CrmIntakeYear
	public const string CreateCrmIntakeYear = "crm-intake-year";
	public const string UpdateCrmIntakeYear = "crm-intake-year/{key}";
	public const string DeleteCrmIntakeYear = "crm-intake-year/{key}";
	public const string CrmIntakeYearSummary = "crm-intake-year-summary";
	public const string ReadCrmIntakeYears = "crm-intake-years";
	public const string ReadCrmIntakeYear = "crm-intake-year/{id:int}";
	#endregion CrmIntakeYear

	#region CrmIntakeMonth
	public const string CreateCrmIntakeMonth = "crm-intake-month";
	public const string UpdateCrmIntakeMonth = "crm-intake-month/{key}";
	public const string DeleteCrmIntakeMonth = "crm-intake-month/{key}";
	public const string CrmIntakeMonthSummary = "crm-intake-month-summary";
	public const string ReadCrmIntakeMonths = "crm-intake-months";
	public const string ReadCrmIntakeMonth = "crm-intake-month/{id:int}";
	#endregion CrmIntakeMonth

	#region CrmMonth
	public const string CreateCrmMonth = "crm-month";
	public const string UpdateCrmMonth = "crm-month/{key}";
	public const string DeleteCrmMonth = "crm-month/{key}";
	public const string CrmMonthSummary = "crm-month-summary";
	public const string ReadCrmMonths = "crm-months";
	public const string ReadCrmMonth = "crm-month/{id:int}";
	#endregion CrmMonth

	#region CrmYear
	public const string CreateCrmYear = "crm-year";
	public const string UpdateCrmYear = "crm-year/{key}";
	public const string DeleteCrmYear = "crm-year/{key}";
	public const string CrmYearSummary = "crm-year-summary";
	public const string ReadCrmYears = "crm-years";
	public const string ReadCrmYear = "crm-year/{id:int}";
	#endregion CrmYear

	#region DmsFileUpdateHistory
	public const string CreateDmsFileUpdateHistory = "dms-file-update-history";
	public const string UpdateDmsFileUpdateHistory = "dms-file-update-history/{key}";
	public const string DeleteDmsFileUpdateHistory = "dms-file-update-history/{key}";
	public const string DmsFileUpdateHistorySummary = "dms-file-update-history-summary";
	public const string ReadDmsFileUpdateHistories = "dms-file-update-histories";
	public const string DmsFileUpdateHistoryDDL = "dms-file-update-histories-ddl";
	public const string ReadDmsFileUpdateHistory = "dms-file-update-history/{id:int}";
	public const string DmsFileUpdateHistoriesByEntity = "dms-file-update-histories-by-entity/{entityId}";
	#endregion DmsFileUpdateHistory

	#region ApproverDetails
	public const string CreateApproverDetails = "approver-details";
	public const string UpdateApproverDetails = "approver-details/{key}";
	public const string DeleteApproverDetails = "approver-details/{key}";
	public const string ApproverDetailsSummary = "approver-details-summary";
	public const string ReadApproverDetails = "approver-details";
	public const string ApproverDetailsDDL = "approver-details-ddl";
	public const string ReadApproverDetail = "approver-details/{id:int}";
	#endregion ApproverDetails

	#region ApproverHistory
	public const string CreateApproverHistory = "approver-history";
	public const string UpdateApproverHistory = "approver-history/{key}";
	public const string DeleteApproverHistory = "approver-history/{key}";
	public const string ApproverHistorySummary = "approver-history-summary";
	public const string ReadApproverHistories = "approver-histories";
	public const string ApproverHistoryDDL = "approver-histories-ddl";
	public const string ReadApproverHistory = "approver-history/{id:int}";
	#endregion ApproverHistory

	#region ApproverOrder
	public const string CreateApproverOrder = "approver-order";
	public const string UpdateApproverOrder = "approver-order/{key}";
	public const string DeleteApproverOrder = "approver-order/{key}";
	public const string ApproverOrderSummary = "approver-order-summary";
	public const string ReadApproverOrders = "approver-orders";
	public const string ApproverOrderDDL = "approver-orders-ddl";
	public const string ReadApproverOrder = "approver-order/{id:int}";
	#endregion ApproverOrder

	#region ApproverType
	public const string CreateApproverType = "approver-type";
	public const string UpdateApproverType = "approver-type/{key}";
	public const string DeleteApproverType = "approver-type/{key}";
	public const string ApproverTypeSummary = "approver-type-summary";
	public const string ReadApproverTypes = "approver-types";
	public const string ApproverTypeDDL = "approver-types-ddl";
	public const string ReadApproverType = "approver-type/{id:int}";
	#endregion ApproverType

	#region AssignApprover
	public const string CreateAssignApprover = "assign-approver";
	public const string UpdateAssignApprover = "assign-approver/{key}";
	public const string DeleteAssignApprover = "assign-approver/{key}";
	public const string AssignApproverSummary = "assign-approver-summary";
	public const string ReadAssignApprovers = "assign-approvers";
	public const string AssignApproverDDL = "assign-approvers-ddl";
	public const string ReadAssignApprover = "assign-approver/{id:int}";
	#endregion AssignApprover

	#region ApproverTypeToGroupMapping
	public const string CreateApproverTypeToGroupMapping = "approver-type-to-group-mapping";
	public const string UpdateApproverTypeToGroupMapping = "approver-type-to-group-mapping/{key}";
	public const string DeleteApproverTypeToGroupMapping = "approver-type-to-group-mapping/{key}";
	public const string ApproverTypeToGroupMappingSummary = "approver-type-to-group-mapping-summary";
	public const string ReadApproverTypeToGroupMappings = "approver-type-to-group-mappings";
	public const string ApproverTypeToGroupMappingDDL = "approver-type-to-group-mappings-ddl";
	public const string ReadApproverTypeToGroupMapping = "approver-type-to-group-mapping/{id:int}";
	#endregion ApproverTypeToGroupMapping

	#region DocumentTemplate
	public const string CreateDocumentTemplate = "document-template";
	public const string UpdateDocumentTemplate = "document-template/{key}";
	public const string DeleteDocumentTemplate = "document-template/{key}";
	public const string DocumentTemplateSummary = "document-template-summary";
	public const string ReadDocumentTemplates = "document-templates";
	public const string DocumentTemplateDDL = "document-templates-ddl";
	public const string ReadDocumentTemplate = "document-template/{id:int}";
	#endregion DocumentTemplate

	#region DocumentType
	public const string CreateDocumentType = "document-type";
	public const string UpdateDocumentType = "document-type/{key}";
	public const string DeleteDocumentType = "document-type/{key}";
	public const string DocumentTypeSummary = "document-type-summary";
	public const string ReadDocumentTypes = "document-types";
	public const string DocumentTypeDDL = "document-types-ddl";
	public const string ReadDocumentType = "document-type/{id:int}";
	#endregion DocumentType

	#region DocumentParameter
	public const string CreateDocumentParameter = "document-parameter";
	public const string UpdateDocumentParameter = "document-parameter/{key}";
	public const string DeleteDocumentParameter = "document-parameter/{key}";
	public const string DocumentParameterSummary = "document-parameter-summary";
	public const string ReadDocumentParameters = "document-parameters";
	public const string DocumentParameterDDL = "document-parameters-ddl";
	public const string ReadDocumentParameter = "document-parameter/{id:int}";
	#endregion DocumentParameter

	#region DocumentParameterMapping
	public const string CreateDocumentParameterMapping = "document-parameter-mapping";
	public const string UpdateDocumentParameterMapping = "document-parameter-mapping/{key}";
	public const string DeleteDocumentParameterMapping = "document-parameter-mapping/{key}";
	public const string DocumentParameterMappingSummary = "document-parameter-mapping-summary";
	public const string ReadDocumentParameterMappings = "document-parameter-mappings";
	public const string DocumentParameterMappingDDL = "document-parameter-mappings-ddl";
	public const string ReadDocumentParameterMapping = "document-parameter-mapping/{id:int}";
	#endregion DocumentParameterMapping

	#region DocumentQueryMapping
	public const string CreateDocumentQueryMapping = "document-query-mapping";
	public const string UpdateDocumentQueryMapping = "document-query-mapping/{key}";
	public const string DeleteDocumentQueryMapping = "document-query-mapping/{key}";
	public const string DocumentQueryMappingSummary = "document-query-mapping-summary";
	public const string ReadDocumentQueryMappings = "document-query-mappings";
	public const string DocumentQueryMappingDDL = "document-query-mappings-ddl";
	public const string ReadDocumentQueryMapping = "document-query-mapping/{id:int}";
	#endregion DocumentQueryMapping

	#region AuditLog
	public const string CreateAuditLog = "audit-log";
	public const string UpdateAuditLog = "audit-log/{key}";
	public const string DeleteAuditLog = "audit-log/{key}";
	public const string AuditLogSummary = "audit-log-summary";
	public const string ReadAuditLogs = "audit-logs";
	public const string AuditLogDDL = "audit-logs-ddl";
	public const string ReadAuditLog = "audit-log/{id:long}";
	#endregion AuditLog

	#region AuditTrail
	public const string CreateAuditTrail = "audit-trail";
	public const string UpdateAuditTrail = "audit-trail/{key}";
	public const string DeleteAuditTrail = "audit-trail/{key}";
	public const string AuditTrailSummary = "audit-trail-summary";
	public const string ReadAuditTrails = "audit-trails";
	public const string AuditTrailDDL = "audit-trails-ddl";
	public const string ReadAuditTrail = "audit-trail/{id:int}";
	#endregion AuditTrail

	#region AppsTokenInfo
	public const string CreateAppsTokenInfo = "apps-token-info";
	public const string UpdateAppsTokenInfo = "apps-token-info/{key}";
	public const string DeleteAppsTokenInfo = "apps-token-info/{key}";
	public const string AppsTokenInfoSummary = "apps-token-info-summary";
	public const string ReadAppsTokenInfos = "apps-token-infos";
	public const string AppsTokenInfoDDL = "apps-token-infos-ddl";
	public const string ReadAppsTokenInfo = "apps-token-info/{id:int}";
	#endregion AppsTokenInfo

	#region AppsTransactionLog
	public const string CreateAppsTransactionLog = "apps-transaction-log";
	public const string UpdateAppsTransactionLog = "apps-transaction-log/{key}";
	public const string DeleteAppsTransactionLog = "apps-transaction-log/{key}";
	public const string AppsTransactionLogSummary = "apps-transaction-log-summary";
	public const string ReadAppsTransactionLogs = "apps-transaction-logs";
	public const string AppsTransactionLogDDL = "apps-transaction-logs-ddl";
	public const string ReadAppsTransactionLog = "apps-transaction-log/{id:int}";
	#endregion AppsTransactionLog

	#region PasswordHistory
	public const string CreatePasswordHistory = "password-history";
	public const string UpdatePasswordHistory = "password-history/{key}";
	public const string DeletePasswordHistory = "password-history/{key}";
	public const string PasswordHistorySummary = "password-history-summary";
	public const string ReadPasswordHistories = "password-histories";
	public const string PasswordHistoryDDL = "password-histories-ddl";
	public const string ReadPasswordHistory = "password-history/{id:int}";
	#endregion PasswordHistory

	#region AccessRestriction
	public const string CreateAccessRestriction = "access-restriction";
	public const string UpdateAccessRestriction = "access-restriction/{key}";
	public const string DeleteAccessRestriction = "access-restriction/{key}";
	public const string AccessRestrictionSummary = "access-restriction-summary";
	public const string ReadAccessRestrictions = "access-restrictions";
	public const string AccessRestrictionDDL = "access-restrictions-ddl";
	public const string ReadAccessRestriction = "access-restriction/{id:int}";
	#endregion AccessRestriction

	// =============================================
	// Phase 2: System Configuration & Workflow Entities
	// =============================================

	#region BoardInstitute
	public const string CreateBoardInstitute = "board-institute";
	public const string UpdateBoardInstitute = "board-institute/{key}";
	public const string DeleteBoardInstitute = "board-institute/{key}";
	public const string BoardInstituteSummary = "board-institute-summary";
	public const string ReadBoardInstitutes = "board-institutes";
	public const string BoardInstituteDDL = "board-institutes-ddl";
	public const string ReadBoardInstitute = "board-institute/{id:int}";
	#endregion BoardInstitute

	#region MaritalStatus
	public const string CreateMaritalStatus = "marital-status";
	public const string UpdateMaritalStatus = "marital-status/{key}";
	public const string DeleteMaritalStatus = "marital-status/{key}";
	public const string MaritalStatusSummary = "marital-status-summary";
	public const string ReadMaritalStatuses = "marital-statuses";
	public const string MaritalStatusDDL = "marital-statuses-ddl";
	public const string ReadMaritalStatus = "marital-status/{id:int}";
	#endregion MaritalStatus

	#region SystemSettings
	public const string SystemSettingsByCompanyId = "system-settings/company/{companyId:int}";
	public const string AssemblyInfo = "assembly-info";
	public const string UpdateSystemSettings = "system-settings";
	public const string ReadSystemSettings = "system-settings";
	#endregion SystemSettings

	#region AuditType
	public const string CreateAuditType = "audit-type";
	public const string UpdateAuditType = "audit-type/{key}";
	public const string DeleteAuditType = "audit-type/{key}";
	public const string AuditTypeSummary = "audit-type-summary";
	public const string ReadAuditTypes = "audit-types";
	public const string AuditTypeDDL = "audit-types-ddl";
	public const string ReadAuditType = "audit-type/{id:int}";
	#endregion AuditType

	#region Competencies
	public const string CreateCompetency = "competency";
	public const string UpdateCompetency = "competency/{key}";
	public const string DeleteCompetency = "competency/{key}";
	public const string CompetenciesSummary = "competencies-summary";
	public const string ReadCompetencies = "competencies";
	public const string CompetenciesDDL = "competencies-ddl";
	public const string ReadCompetency = "competency/{id:int}";
	#endregion Competencies

	#region CompetencyLevel
	public const string CreateCompetencyLevel = "competency-level";
	public const string UpdateCompetencyLevel = "competency-level/{key}";
	public const string DeleteCompetencyLevel = "competency-level/{key}";
	public const string CompetencyLevelSummary = "competency-level-summary";
	public const string ReadCompetencyLevels = "competency-levels";
	public const string CompetencyLevelDDL = "competency-levels-ddl";
	public const string ReadCompetencyLevel = "competency-level/{id:int}";
	#endregion CompetencyLevel

	#region CrmLeadSource
	public const string CreateCrmLeadSource = "crm-lead-source";
	public const string UpdateCrmLeadSource = "crm-lead-source/{key}";
	public const string DeleteCrmLeadSource = "crm-lead-source/{key}";
	public const string CrmLeadSourceSummary = "crm-lead-source-summary";
	public const string ReadCrmLeadSources = "crm-lead-sources";
	public const string CrmLeadSourceDDL = "crm-lead-sources-ddl";
	public const string ReadCrmLeadSource = "crm-lead-source/{id:int}";
	#endregion CrmLeadSource

	#region CrmLeadStatus
	public const string CreateCrmLeadStatus = "crm-lead-status";
	public const string UpdateCrmLeadStatus = "crm-lead-status/{key}";
	public const string DeleteCrmLeadStatus = "crm-lead-status/{key}";
	public const string CrmLeadStatusSummary = "crm-lead-status-summary";
	public const string ReadCrmLeadStatuses = "crm-lead-statuses";
	public const string CrmLeadStatusDDL = "crm-lead-statuses-ddl";
	public const string ReadCrmLeadStatus = "crm-lead-status/{id:int}";
	#endregion CrmLeadStatus

	#region CrmVisaType
	public const string CreateCrmVisaType = "crm-visa-type";
	public const string UpdateCrmVisaType = "crm-visa-type/{key}";
	public const string DeleteCrmVisaType = "crm-visa-type/{key}";
	public const string CrmVisaTypeSummary = "crm-visa-type-summary";
	public const string ReadCrmVisaTypes = "crm-visa-types";
	public const string CrmVisaTypeDDL = "crm-visa-types-ddl";
	public const string ReadCrmVisaType = "crm-visa-type/{id:int}";
	#endregion CrmVisaType

	#region CrmAgentType
	public const string CreateCrmAgentType = "crm-agent-type";
	public const string UpdateCrmAgentType = "crm-agent-type/{key}";
	public const string DeleteCrmAgentType = "crm-agent-type/{key}";
	public const string CrmAgentTypeSummary = "crm-agent-type-summary";
	public const string ReadCrmAgentTypes = "crm-agent-types";
	public const string CrmAgentTypeDDL = "crm-agent-types-ddl";
	public const string ReadCrmAgentType = "crm-agent-type/{id:int}";
	#endregion CrmAgentType

	#region CrmStudentStatus
	public const string CreateCrmStudentStatus = "crm-student-status";
	public const string UpdateCrmStudentStatus = "crm-student-status/{key}";
	public const string DeleteCrmStudentStatus = "crm-student-status/{key}";
	public const string CrmStudentStatusSummary = "crm-student-status-summary";
	public const string ReadCrmStudentStatuses = "crm-student-statuses";
	public const string CrmStudentStatusDDL = "crm-student-statuses-ddl";
	public const string ReadCrmStudentStatus = "crm-student-status/{id:int}";
	#endregion CrmStudentStatus

	#region CrmOffice
	public const string CreateCrmOffice = "crm-office";
	public const string UpdateCrmOffice = "crm-office/{key}";
	public const string DeleteCrmOffice = "crm-office/{key}";
	public const string CrmOfficeSummary = "crm-office-summary";
	public const string ReadCrmOffices = "crm-offices";
	public const string CrmOfficeDDL = "crm-offices-ddl";
	public const string ReadCrmOffice = "crm-office/{id:int}";
	#endregion CrmOffice

	#region CrmAgent
	public const string CreateCrmAgent = "crm-agent";
	public const string UpdateCrmAgent = "crm-agent/{key}";
	public const string DeleteCrmAgent = "crm-agent/{key}";
	public const string CrmAgentSummary = "crm-agent-summary";
	public const string ReadCrmAgents = "crm-agents";
	public const string CrmAgentDDL = "crm-agents-ddl";
	public const string ReadCrmAgent = "crm-agent/{id:int}";
	#endregion CrmAgent

	#region CrmCounselor
	public const string CreateCrmCounselor = "crm-counselor";
	public const string UpdateCrmCounselor = "crm-counselor/{key}";
	public const string DeleteCrmCounselor = "crm-counselor/{key}";
	public const string CrmCounselorSummary = "crm-counselor-summary";
	public const string ReadCrmCounselors = "crm-counselors";
	public const string CrmCounselorDDL = "crm-counselors-ddl";
	public const string ReadCrmCounselor = "crm-counselor/{id:int}";
	#endregion CrmCounselor

	#region CrmLead
	public const string CreateCrmLead = "crm-lead";
	public const string UpdateCrmLead = "crm-lead/{key}";
	public const string DeleteCrmLead = "crm-lead/{key}";
	public const string CrmLeadSummary = "crm-lead-summary";
	public const string ReadCrmLeads = "crm-leads";
	public const string CrmLeadDDL = "crm-leads-ddl";
	public const string ReadCrmLead = "crm-lead/{id:int}";
	public const string CrmConvertLeadToStudentPreflight = "crm-convert-lead-to-student-preflight";
	public const string CrmConvertLeadToStudent = "crm-convert-lead-to-student";
	#endregion CrmLead

	#region CrmStudent
	public const string CreateCrmStudent = "crm-student";
	public const string UpdateCrmStudent = "crm-student/{key}";
	public const string DeleteCrmStudent = "crm-student/{key}";
	public const string CrmStudentSummary = "crm-student-summary";
	public const string ReadCrmStudents = "crm-students";
	public const string CrmStudentDDL = "crm-students-ddl";
	public const string ReadCrmStudent = "crm-student/{id:int}";
	public const string CrmStudentsByBranchId = "crm-students-by-branch/{branchId:int}";
	public const string CrmStudentStatusTransition = "crm-student-status-transition";
	public const string CrmStudentApplicationReadyCheck = "crm-student-application-ready/{studentId:int}";
	#endregion CrmStudent

	#region CrmStudentDocument
	public const string CreateCrmStudentDocument = "crm-student-document";
	public const string UpdateCrmStudentDocument = "crm-student-document/{key}";
	public const string DeleteCrmStudentDocument = "crm-student-document/{key}";
	public const string CrmStudentDocumentSummary = "crm-student-document-summary";
	public const string ReadCrmStudentDocuments = "crm-student-documents";
	public const string ReadCrmStudentDocument = "crm-student-document/{id:int}";
	public const string StudentDocumentsByStudentId = "crm-student-documents-by-student/{studentId:int}";
	public const string CrmStudentDocumentUpload = "crm-student-document-upload";
	public const string CrmStudentDocumentStatusTransition = "crm-student-document-status-transition";
	#endregion CrmStudentDocument

	#region CrmDocumentVerificationHistory
	public const string CreateCrmDocumentVerificationHistory = "crm-document-verification-history";
	public const string UpdateCrmDocumentVerificationHistory = "crm-document-verification-history/{key}";
	public const string DeleteCrmDocumentVerificationHistory = "crm-document-verification-history/{key}";
	public const string CrmDocumentVerificationHistorySummary = "crm-document-verification-history-summary";
	public const string ReadCrmDocumentVerificationHistories = "crm-document-verification-histories";
	public const string ReadCrmDocumentVerificationHistory = "crm-document-verification-history/{id:int}";
	public const string DocumentVerificationHistoriesByDocumentId = "crm-document-verification-histories-by-document/{documentId:int}";
	#endregion CrmDocumentVerificationHistory

	#region CrmStudentDocumentChecklist
	public const string CreateCrmStudentDocumentChecklist = "crm-student-document-checklist";
	public const string UpdateCrmStudentDocumentChecklist = "crm-student-document-checklist/{key}";
	public const string DeleteCrmStudentDocumentChecklist = "crm-student-document-checklist/{key}";
	public const string CrmStudentDocumentChecklistSummary = "crm-student-document-checklist-summary";
	public const string ReadCrmStudentDocumentChecklists = "crm-student-document-checklists";
	public const string ReadCrmStudentDocumentChecklist = "crm-student-document-checklist/{id:int}";
	public const string StudentDocumentChecklistsByStudentId = "crm-student-document-checklists-by-student/{studentId:int}";
	#endregion CrmStudentDocumentChecklist

	#region CrmStudentAcademicProfile
	public const string CreateCrmStudentAcademicProfile = "crm-student-academic-profile";
	public const string UpdateCrmStudentAcademicProfile = "crm-student-academic-profile/{key}";
	public const string DeleteCrmStudentAcademicProfile = "crm-student-academic-profile/{key}";
	public const string CrmStudentAcademicProfileSummary = "crm-student-academic-profile-summary";
	public const string ReadCrmStudentAcademicProfiles = "crm-student-academic-profiles";
	public const string ReadStudentAcademicProfiles = ReadCrmStudentAcademicProfiles;
	public const string ReadCrmStudentAcademicProfile = "crm-student-academic-profile/{id:int}";
	public const string StudentAcademicProfilesByStudentId = "crm-student-academic-profiles-by-student/{studentId:int}";
	#endregion CrmStudentAcademicProfile

	#region CrmStudentStatusHistory
	public const string CreateCrmStudentStatusHistory = "crm-student-status-history";
	public const string UpdateCrmStudentStatusHistory = "crm-student-status-history/{key}";
	public const string DeleteCrmStudentStatusHistory = "crm-student-status-history/{key}";
	public const string CrmStudentStatusHistorySummary = "crm-student-status-history-summary";
	public const string ReadCrmStudentStatusHistories = "crm-student-status-histories";
	public const string ReadStudentStatusHistories = ReadCrmStudentStatusHistories;
	public const string ReadCrmStudentStatusHistory = "crm-student-status-history/{id:int}";
	public const string StudentStatusHistoriesByStudentId = "crm-student-status-histories-by-student/{studentId:int}";
	#endregion CrmStudentStatusHistory

	#region CrmEnquiry
	public const string CreateCrmEnquiry = "crm-enquiry";
	public const string UpdateCrmEnquiry = "crm-enquiry/{key}";
	public const string DeleteCrmEnquiry = "crm-enquiry/{key}";
	public const string CrmEnquirySummary = "crm-enquiry-summary";
	public const string ReadCrmEnquiries = "crm-enquiries";
	public const string CrmEnquiryDDL = "crm-enquiries-ddl";
	public const string ReadCrmEnquiry = "crm-enquiry/{id:int}";
	#endregion CrmEnquiry

	#region CrmFollowUp
	public const string CreateCrmFollowUp = "crm-followup";
	public const string UpdateCrmFollowUp = "crm-followup/{key}";
	public const string DeleteCrmFollowUp = "crm-followup/{key}";
	public const string CrmFollowUpSummary = "crm-followup-summary";
	public const string ReadCrmFollowUps = "crm-followups";
	public const string CrmFollowUpDDL = "crm-followups-ddl";
	public const string ReadCrmFollowUp = "crm-followup/{id:int}";
	public const string CrmFollowUpsByLeadId = "crm-followups-by-lead/{leadId:int}";
	public const string CrmFollowUpStatusTransition = "crm-followup-status-transition";
	public const string CrmProcessOverdueFollowUps = "crm-followups-process-overdue";
	public const string CrmMarkUnresponsiveLeads = "crm-followups-mark-unresponsive";
	#endregion CrmFollowUp

	#region CrmFollowUpHistory
	public const string CreateCrmFollowUpHistory = "crm-followup-history";
	public const string UpdateCrmFollowUpHistory = "crm-followup-history/{key}";
	public const string DeleteCrmFollowUpHistory = "crm-followup-history/{key}";
	public const string CrmFollowUpHistorySummary = "crm-followup-history-summary";
	public const string ReadCrmFollowUpHistories = "crm-followup-histories";
	public const string ReadFollowUpHistories = ReadCrmFollowUpHistories;
	public const string ReadCrmFollowUpHistory = "crm-followup-history/{id:int}";
	public const string FollowUpHistoriesByFollowUpId = "crm-followup-histories-by-followup/{followUpId:int}";
	#endregion CrmFollowUpHistory

	#region CrmCounsellingSession
	public const string CreateCrmCounsellingSession = "crm-counselling-session";
	public const string UpdateCrmCounsellingSession = "crm-counselling-session/{key}";
	public const string DeleteCrmCounsellingSession = "crm-counselling-session/{key}";
	public const string CrmCounsellingSessionSummary = "crm-counselling-session-summary";
	public const string ReadCrmCounsellingSessions = "crm-counselling-sessions";
	public const string ReadCounsellingSessions = ReadCrmCounsellingSessions;
	public const string ReadCrmCounsellingSession = "crm-counselling-session/{id:int}";
	public const string CounsellingSessionsByLeadId = "crm-counselling-sessions-by-lead/{leadId:int}";
	public const string CrmCounsellingSessionEligibility = "crm-counselling-session-eligibility/{studentId:int}";
	#endregion CrmCounsellingSession

	#region CrmSessionProgramShortlist
	public const string CreateCrmSessionProgramShortlist = "crm-session-program-shortlist";
	public const string UpdateCrmSessionProgramShortlist = "crm-session-program-shortlist/{key}";
	public const string DeleteCrmSessionProgramShortlist = "crm-session-program-shortlist/{key}";
	public const string CrmSessionProgramShortlistSummary = "crm-session-program-shortlist-summary";
	public const string ReadCrmSessionProgramShortlists = "crm-session-program-shortlists";
	public const string ReadSessionProgramShortlists = ReadCrmSessionProgramShortlists;
	public const string ReadCrmSessionProgramShortlist = "crm-session-program-shortlist/{id:int}";
	public const string SessionProgramShortlistsBySessionId = "crm-session-program-shortlists-by-session/{sessionId:int}";
	#endregion CrmSessionProgramShortlist

	#region CrmNote
	public const string CreateCrmNote = "crm-note";
	public const string UpdateCrmNote = "crm-note/{key}";
	public const string DeleteCrmNote = "crm-note/{key}";
	public const string CrmNoteSummary = "crm-note-summary";
	public const string ReadCrmNotes = "crm-notes";
	public const string CrmNoteDDL = "crm-notes-ddl";
	public const string ReadCrmNote = "crm-note/{id:int}";
	#endregion CrmNote

	#region CrmTask
	public const string CreateCrmTask = "crm-task";
	public const string UpdateCrmTask = "crm-task/{key}";
	public const string DeleteCrmTask = "crm-task/{key}";
	public const string CrmTaskSummary = "crm-task-summary";
	public const string ReadCrmTasks = "crm-tasks";
	public const string CrmTaskDDL = "crm-tasks-ddl";
	public const string ReadCrmTask = "crm-task/{id:int}";
	#endregion CrmTask

	#region CrmDegreeLevel
	public const string CreateCrmDegreeLevel = "crm-degree-level";
	public const string UpdateCrmDegreeLevel = "crm-degree-level/{key}";
	public const string DeleteCrmDegreeLevel = "crm-degree-level/{key}";
	public const string CrmDegreeLevelSummary = "crm-degree-level-summary";
	public const string ReadCrmDegreeLevels = "crm-degree-levels";
	public const string CrmDegreeLevelDDL = "crm-degree-levels-ddl";
	public const string ReadCrmDegreeLevel = "crm-degree-level/{id:int}";
	#endregion CrmDegreeLevel

	#region CrmFaculty
	public const string CreateCrmFaculty = "crm-faculty";
	public const string UpdateCrmFaculty = "crm-faculty/{key}";
	public const string DeleteCrmFaculty = "crm-faculty/{key}";
	public const string CrmFacultySummary = "crm-faculty-summary";
	public const string ReadCrmFaculties = "crm-faculties";
	public const string CrmFacultyDDL = "crm-faculties-ddl";
	public const string ReadCrmFaculty = "crm-faculty/{id:int}";
	public const string CrmFacultiesByInstituteId = "crm-faculties-by-institute/{instituteId:int}";
	#endregion CrmFaculty

	#region CrmCourseFee
	public const string CreateCrmCourseFee = "crm-course-fee";
	public const string UpdateCrmCourseFee = "crm-course-fee/{key}";
	public const string DeleteCrmCourseFee = "crm-course-fee/{key}";
	public const string CrmCourseFeeSummary = "crm-course-fee-summary";
	public const string ReadCrmCourseFees = "crm-course-fees";
	public const string ReadCrmCourseFee = "crm-course-fee/{id:int}";
	public const string CrmCourseFeesByCourseId = "crm-course-fees-by-course/{courseId:int}";
	#endregion CrmCourseFee

	#region CrmCountryDocumentRequirement
	public const string CreateCrmCountryDocReq = "crm-country-doc-req";
	public const string UpdateCrmCountryDocReq = "crm-country-doc-req/{key}";
	public const string DeleteCrmCountryDocReq = "crm-country-doc-req/{key}";
	public const string CrmCountryDocReqSummary = "crm-country-doc-req-summary";
	public const string ReadCrmCountryDocReqs = "crm-country-doc-reqs";
	public const string ReadCrmCountryDocReq = "crm-country-doc-req/{id:int}";
	public const string CrmCountryDocReqsByCountryId = "crm-country-doc-reqs-by-country/{countryId:int}";
	#endregion CrmCountryDocumentRequirement

	#region CrmBranchTarget
	public const string CreateCrmBranchTarget = "crm-branch-target";
	public const string UpdateCrmBranchTarget = "crm-branch-target/{key}";
	public const string DeleteCrmBranchTarget = "crm-branch-target/{key}";
	public const string CrmBranchTargetSummary = "crm-branch-target-summary";
	public const string ReadCrmBranchTargets = "crm-branch-targets";
	public const string ReadCrmBranchTarget = "crm-branch-target/{id:int}";
	public const string CrmBranchTargetsByBranchId = "crm-branch-targets-by-branch/{branchId:int}";
	#endregion CrmBranchTarget

	#region CrmSystemConfiguration
	public const string CreateCrmSystemConfig = "crm-system-config";
	public const string UpdateCrmSystemConfig = "crm-system-config/{key}";
	public const string DeleteCrmSystemConfig = "crm-system-config/{key}";
	public const string CrmSystemConfigSummary = "crm-system-config-summary";
	public const string ReadCrmSystemConfigs = "crm-system-configs";
	public const string ReadCrmSystemConfig = "crm-system-config/{id:int}";
	public const string CrmSystemConfigByKey = "crm-system-config-by-key/{key}";
	#endregion CrmSystemConfiguration

	#region CrmMasterDataSuggestion
	public const string CreateCrmMasterDataSuggestion = "crm-master-data-suggestion";
	public const string UpdateCrmMasterDataSuggestion = "crm-master-data-suggestion/{key}";
	public const string DeleteCrmMasterDataSuggestion = "crm-master-data-suggestion/{key}";
	public const string CrmMasterDataSuggestionSummary = "crm-master-data-suggestion-summary";
	public const string ReadCrmMasterDataSuggestions = "crm-master-data-suggestions";
	public const string ReadCrmMasterDataSuggestion = "crm-master-data-suggestion/{id:int}";
	#endregion CrmMasterDataSuggestion

	#region CrmAgentLead
	public const string CreateCrmAgentLead = "crm-agent-lead";
	public const string UpdateCrmAgentLead = "crm-agent-lead/{key}";
	public const string DeleteCrmAgentLead = "crm-agent-lead/{key}";
	public const string CrmAgentLeadSummary = "crm-agent-lead-summary";
	public const string ReadCrmAgentLeads = "crm-agent-leads";
	public const string ReadCrmAgentLead = "crm-agent-lead/{id:int}";
	public const string CrmAgentLeadByLeadId = "crm-agent-lead-by-lead/{leadId:int}";
	public const string CrmAgentLeadsByAgentId = "crm-agent-leads-by-agent/{agentId:int}";
	#endregion CrmAgentLead

	#region TokenBlacklist
	public const string CreateTokenBlacklist = "token-blacklist";
	public const string UpdateTokenBlacklist = "token-blacklist/{key:guid}";
	public const string DeleteTokenBlacklist = "token-blacklist/{key:guid}";
	public const string TokenBlacklistSummary = "token-blacklist-summary";
	public const string ReadTokenBlacklists = "token-blacklists";
	public const string ReadTokenBlacklist = "token-blacklist/{id:guid}";
	public const string IsTokenBlacklisted = "token-blacklist/check";
	public const string BlacklistToken = "token-blacklist/blacklist";
	public const string RemoveExpiredTokens = "token-blacklist/cleanup";
	#endregion TokenBlacklist
}
