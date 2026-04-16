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
}
