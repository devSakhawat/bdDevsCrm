// Class: SystemSettingsRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin
{
	public class SystemSettingsRepository : RepositoryBase<SystemSettings>, ISystemSettingsRepository
	{
		public SystemSettingsRepository(CrmContext context) : base(context) { }

		/// <summary>
		/// Retrieves system settings by Company ID.
		/// </summary>
		public async Task<SystemSettings?> SystemSettingsByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default)
		{
			var query = string.Format("SELECT * FROM SystemSettings WHERE CompanyId = '{0}'", companyId);
			return await AdoExecuteSingleDataAsync<SystemSettings>(query, null, cancellationToken);
		}

		/// <summary>
		/// Retrieves assembly information.
		/// </summary>
		public async Task<AssemblyInfo?> AssemblyInfoResultAsync(CancellationToken cancellationToken = default)
		{
			var query = @"SELECT AssemblyInfoId, AssemblyTitle, AssemblyDescription, AssemblyCompany, AssemblyProduct, AssemblyCopyright, AssemblyVersion
                          , ProductBanner, IsAttendanceByLogin, PoweredBy, PoweredByUrl, ProductStyleSheet, CvBankPath 
                          FROM AssemblyInfo Where IsDefault=1";

			var data = await AdoExecuteSingleDataAsync<AssemblyInfo>(query, null, cancellationToken);

			if (data == null)
			{
				throw new Exception("Please set the Assembly information properly in your application database.");
			}
			return data;
		}

		public async Task<IEnumerable<SystemSettings>> SystemSettingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
		{
			return await ListAsync(x => x.SettingsId, trackChanges, cancellationToken);
		}
	}
}





//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Core.SystemAdmin;
//using Infrastructure.Sql.Context;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Repositories.Core.SystemAdmin;

//public class SystemSettingsRepository : RepositoryBase<SystemSettings>, ISystemSettingsRepository
//{
//  public SystemSettingsRepository(CrmContext context) : base(context) { }

//  #region Global Variable

//  //private CommonDbHelper oracleDbHelper = null;
//  //private DBHelper SqlDbHelper = null;
//  //private OdbcDbHelper MySqlDbHelper = null;

//  #endregion

//  #region SQL

//  private const string SELECT_SYSTEM_SETTINGS_BY_COMPANYID =
//      "SELECT SettingsId, CompanyId, Theme, Language, MinLoginLength,IsPasswordChange,IsPasswordExpire," +
//      "MinPassLength, PassType, SpecialCharAllowed, WrongAttemptNo, ChangePassDays, ChangePassFirstLogin, PassExpiryDays, " +
//      "ResetPass,PassResetBy,OldPassUseRestriction, OdbcClientList,IsWebLoginEnable,DeleteApproveLeaveUponPunch," +
//      "DeleteLateUponAttendanceApproval,IsOtLimitApplicable,IsSingleBranchApplicable,CheckPreviousAbsenteeism,BypassDefaultStateForSameBoss," +
//      "DefaultLateDeductionDays,CheckingApproverSettings,IsOtCalculateForSalary,DefaultEarlyExitDeductionDays,IsEmployeeIdAutoGenereted," +
//      "IsGradeWiseLeave,EnableMultiplePolicyForSameLeaveType,IsAbsenteeismMarge,IsTotalBillingApplicable,EnableApproverCheckingWhileApplication,EnableDelayOnShiftInGraceTime,EnableLateAfterShiftInGraceTime," +
//      "EnableEarlyExitBeforeShiftOutGraceTime,EnableAbsentAfterLateTime,EnableAbsentBeforeEarlyExitTime,EnableAbsentForNoOutPunch,LateTime,EarlyExitTime," +
//      "EnableCustomStatusOutPunch,CustomStatusForNoOutPunch,EnableCustomStatusAfterShiftInGraceTime,CustomStatusForAfterShiftinGraceTime," +
//      "IsOTCalculateOnHolidayWekend,DefaultLateDeductionDaysNext,IsArearFestibleCalculateOnSalary,RegulariseAttendaceDaysLimit,DefaultLateDeductionDaysFirstTime FROM SystemSettings where CompanyId = {0}";

//  private const string InsertSystemSettingsData =
//      "INSERT INTO SystemSettings(CompanyId,Theme,Language,MinLoginLength,MinPassLength,PassType,SpecialCharAllowed,WrongAttemptNo,ChangePassDays," +
//      "ChangePassFirstLogin,PassExpiryDays,ResetPass,PassResetBy,OldPassUseRestriction,LastUpdatedDate,OdbcClientList,UserId,IsPasswordChange," +
//      "IsPasswordExpire,IsWebLoginEnable,DeleteApproveLeaveUponPunch,DeleteLateUponAttendanceApproval,IsOtLimitApplicable,IsSingleBranchApplicable,CheckPreviousAbsenteeism,BypassDefaultStateForSameBoss,DefaultLateDeductionDays,CheckingApproverSettings,IsOtCalculateForSalary," +
//      "DefaultEarlyExitDeductionDays,IsEmployeeIdAutoGenereted,IsGradeWiseLeave,EnableMultiplePolicyForSameLeaveType,IsAbsenteeismMarge,IsTotalBillingApplicable,EnableApproverCheckingWhileApplication,EnableDelayOnShiftInGraceTime,EnableLateAfterShiftInGraceTime," +
//      "EnableEarlyExitBeforeShiftOutGraceTime,EnableAbsentAfterLateTime,EnableAbsentBeforeEarlyExitTime,EnableAbsentForNoOutPunch,LateTime,EarlyExitTime,EnableCustomStatusOutPunch,CustomStatusForNoOutPunch,EnableCustomStatusAfterShiftInGraceTime,CustomStatusForAfterShiftinGraceTime,DefaultLateDeductionDaysNext,RegulariseAttendaceDaysLimit, DefaultLateDeductionDaysFirstTime,ShortLeaveSlot,CasualWorkerAmount) VALUES ({0}, '{1}', '{2}', '{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}',{14},'{15}','{16}',{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},'{45}',{46},'{47}','{48}','{49}','{50}','{51}','{52}')";

//  private const string UpdateSystemSettingsData =
//      "Update SystemSettings set CompanyId = {0}, Theme='{1}', Language = '{2}', MinLoginLength = '{3}', MinPassLength='{4}',PassType='{5}'," +
//      "SpecialCharAllowed = '{6}',WrongAttemptNo='{7}',ChangePassDays='{8}',ChangePassFirstLogin='{9}',PassExpiryDays='{10}',ResetPass='{11}'," +
//      "PassResetBy='{12}',OldPassUseRestriction='{13}',LastUpdatedDate={14},OdbcClientList='{15}',UserId='{16}',IsPasswordChange={17}," +
//      "IsPasswordExpire={18}, IsWebLoginEnable={19}, DeleteApproveLeaveUponPunch={20},DeleteLateUponAttendanceApproval={21},IsOtLimitApplicable={22}," +
//      "IsSingleBranchApplicable={23},CheckPreviousAbsenteeism={24},BypassDefaultStateForSameBoss={25},DefaultLateDeductionDays={26},CheckingApproverSettings={27},IsOtCalculateForSalary = {28},DefaultEarlyExitDeductionDays={29},IsEmployeeIdAutoGenereted={30},IsGradeWiseLeave={31},EnableMultiplePolicyForSameLeaveType={32},IsAbsenteeismMarge={33},IsTotalBillingApplicable={34}," +
//      "EnableApproverCheckingWhileApplication={36},EnableDelayOnShiftInGraceTime={37},EnableLateAfterShiftInGraceTime={38}," +
//      "EnableEarlyExitBeforeShiftOutGraceTime={39},EnableAbsentAfterLateTime={40},EnableAbsentBeforeEarlyExitTime={41},EnableAbsentForNoOutPunch={42},LateTime={43},EarlyExitTime={44},EnableCustomStatusOutPunch={45},CustomStatusForNoOutPunch='{46}',EnableCustomStatusAfterShiftInGraceTime={47},CustomStatusForAfterShiftinGraceTime='{48}',DefaultLateDeductionDaysNext='{49}',RegulariseAttendaceDaysLimit='{50}', DefaultLateDeductionDaysFirstTime='{51}',ShortLeaveSlot='{52}',CasualWorkerAmount='{53}' where SettingsId = {35}";
//  #endregion

//  #region Global Method

//  //public void DbHelper()
//  //{
//  //  var connectionString = "";
//  //  var connectionType = ConfigurationSettings.AppSettings["DataBaseType"];
//  //  if (connectionType == "SQL")
//  //  {
//  //    connectionString = "SqlConnectionString";
//  //    SqlDbHelper = new DBHelper(connectionString);
//  //  }
//  //  else if (connectionType == "MySql")
//  //  {
//  //    connectionString = "MySqlConnectionString";
//  //    MySqlDbHelper = new OdbcDbHelper(connectionString);

//  //  }
//  //  else if (connectionType == "Oracle")
//  //  {
//  //    connectionString = "OracleConnectionString";
//  //    oracleDbHelper = new CommonDbHelper(connectionString);
//  //  }
//  //}

//  #endregion

//  public async Task<SystemSettings> SystemSettingsDataByCompanyId(int companyId)
//  {
//    // Define the query
//    var query = string.Format(@"SELECT * FROM SystemSettings WHERE CompanyId = '{0}'", companyId);

//    // Call the base method to execute the query and map the result to SystemSettings
//    return await ExecuteSingleSql(query);
//  }

//  public async Task<AssemblyInfo?> AssemblyInfoResult()
//  {
//    var objAssembly = new AssemblyInfo();

//    var query = @"SELECT  AssemblyInfoId,AssemblyTitle,AssemblyDescription,AssemblyCompany,AssemblyProduct,AssemblyCopyright,AssemblyVersion
//                   ,ProductBanner,IsAttendanceByLogin,PoweredBy,PoweredByUrl,ProductStyleSheet,CvBankPath FROM  AssemblyInfo Where IsDefault=1";
//    var data = await ExecuteSingleSql(query);

//    if (data == null)
//    {
//      throw new Exception("Please set the Assembly information properly in your application database.");
//    }
//    return objAssembly;
//  }


//  #region EmpressCode
//  //  public async Task<SystemSettings> SystemSettingsDataByCompanyId(int companyId)
//  //  {
//  //    //var quary = string.Format(SELECT_SYSTEM_SETTINGS_BY_COMPANYID, companyId);
//  //    var quary = string.Format(@"SELECT * FROM SystemSettings where CompanyId = '{0}'", companyId);
//  //    return await ExecuteListSqlAsync<SystemSettings>(quary);
//  //  }

//  //  public string SaveSystemSettings(SystemSettings objSystemSettings)
//  //  {
//  //    var res = "";
//  //    DbHelper();
//  //    if (SqlDbHelper == null && oracleDbHelper == null && MySqlDbHelper == null)
//  //    {
//  //      res = "Please Configure Database type";
//  //      return res;
//  //    }
//  //    else
//  //    {

//  //      var quary = "";
//  //      if (SqlDbHelper != null)
//  //      {
//  //        try
//  //        {

//  //          if (objSystemSettings.SettingsId == 0)
//  //          {
//  //            quary = string.Format(InsertSystemSettingsData, objSystemSettings.CompanyId,
//  //                                  objSystemSettings.Theme, objSystemSettings.Language,
//  //                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
//  //                                  objSystemSettings.PassType, objSystemSettings.SpecialCharAllowed,
//  //                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
//  //                                  objSystemSettings.ChangePassFirstLogin,
//  //                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
//  //                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
//  //                                  "'" + DateTime.Now + "'", objSystemSettings.OdbcClientList,
//  //                                  objSystemSettings.UserId, objSystemSettings.IsPasswordChange,
//  //                                  objSystemSettings.IsPasswordExpire, objSystemSettings.IsWebLoginEnable,
//  //                                  objSystemSettings.DeleteApproveLeaveUponPunch,
//  //                                  objSystemSettings.DeleteLateUponAttendanceApproval, objSystemSettings.IsOtLimitApplicable, objSystemSettings.IsSingleBranchApplicable, objSystemSettings.CheckPreviousAbsenteeism
//  //                                  , objSystemSettings.BypassDefaultStateForSameBoss, objSystemSettings.DefaultLateDeductionDays, objSystemSettings.CheckApproverSettings, objSystemSettings.IsOtCalculateForSalary,
//  //                                  objSystemSettings.DefaultEarlyExitDeductionDays, objSystemSettings.IsEmployeeIdAutoGenereted, objSystemSettings.IsGradeWiseLeave,
//  //                                  objSystemSettings.EnableMultiplePolicyForSameLeaveType, objSystemSettings.IsAbsenteeismMarge,
//  //                                  objSystemSettings.IsTotalBillingApplicable, objSystemSettings.EnableApproverCheckingWhileApplication, objSystemSettings.EnableDelayOnShiftInGraceTime, objSystemSettings.EnableLateAfterShiftInGraceTime,
//  //objSystemSettings.EnableEarlyExitBeforeShiftOutGraceTime, objSystemSettings.EnableAbsentAfterLateTime, objSystemSettings.EnableAbsentBeforeEarlyExitTime, objSystemSettings.EnableAbsentForNoOutPunch, objSystemSettings.LateTime,
//  //objSystemSettings.EarlyExitTime, objSystemSettings.EnableCustomStatusOutPunch, objSystemSettings.CustomStatusForNoOutPunch, objSystemSettings.EnableCustomStatusAfterShiftInGraceTime, objSystemSettings.CustomStatusForAfterShiftinGraceTime, objSystemSettings.DefaultLateDeductionDaysNext, objSystemSettings.RegulariseAttendaceDaysLimit, objSystemSettings.DefaultLateDeductionDaysFirstTime, objSystemSettings.ShortLeaveSlot, objSystemSettings.CasualWorkerAmount
//  //                                  );
//  //          }
//  //          else
//  //          {
//  //            quary = string.Format(UpdateSystemSettingsData, objSystemSettings.CompanyId,
//  //                                  objSystemSettings.Theme, objSystemSettings.Language,
//  //                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
//  //                                  objSystemSettings.PassType, objSystemSettings.SpecialCharAllowed,
//  //                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
//  //                                  objSystemSettings.ChangePassFirstLogin,
//  //                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
//  //                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
//  //                                  "'" + DateTime.Now + "'", objSystemSettings.OdbcClientList,
//  //                                  objSystemSettings.UserId, objSystemSettings.IsPasswordChange, objSystemSettings.IsPasswordExpire,
//  //                                  objSystemSettings.IsWebLoginEnable, objSystemSettings.DeleteApproveLeaveUponPunch,
//  //                                  objSystemSettings.DeleteLateUponAttendanceApproval, objSystemSettings.IsOtLimitApplicable
//  //                                  , objSystemSettings.IsSingleBranchApplicable, objSystemSettings.CheckPreviousAbsenteeism
//  //                                  , objSystemSettings.BypassDefaultStateForSameBoss, objSystemSettings.DefaultLateDeductionDays,
//  //                                  objSystemSettings.CheckApproverSettings, objSystemSettings.IsOtCalculateForSalary,
//  //                                  objSystemSettings.DefaultEarlyExitDeductionDays, objSystemSettings.IsEmployeeIdAutoGenereted,
//  //                                  objSystemSettings.IsGradeWiseLeave, objSystemSettings.EnableMultiplePolicyForSameLeaveType,
//  //                                  objSystemSettings.IsAbsenteeismMarge, objSystemSettings.IsTotalBillingApplicable, objSystemSettings.SettingsId,
//  //                                  objSystemSettings.EnableApproverCheckingWhileApplication, objSystemSettings.EnableDelayOnShiftInGraceTime, objSystemSettings.EnableLateAfterShiftInGraceTime,
//  //objSystemSettings.EnableEarlyExitBeforeShiftOutGraceTime, objSystemSettings.EnableAbsentAfterLateTime, objSystemSettings.EnableAbsentBeforeEarlyExitTime, objSystemSettings.EnableAbsentForNoOutPunch, objSystemSettings.LateTime, objSystemSettings.EarlyExitTime, objSystemSettings.EnableCustomStatusOutPunch, objSystemSettings.CustomStatusForNoOutPunch, objSystemSettings.EnableCustomStatusAfterShiftInGraceTime, objSystemSettings.CustomStatusForAfterShiftinGraceTime, objSystemSettings.DefaultLateDeductionDaysNext, objSystemSettings.RegulariseAttendaceDaysLimit, objSystemSettings.DefaultLateDeductionDaysFirstTime, objSystemSettings.ShortLeaveSlot, objSystemSettings.CasualWorkerAmount);
//  //          }

//  //          SqlDbHelper.ExecuteNonQuery(quary);
//  //          res = "Success";
//  //        }
//  //        catch (Exception ex)
//  //        {
//  //          res = ex.Message;
//  //          SqlDbHelper.RollBack();
//  //        }
//  //        finally
//  //        {
//  //          SqlDbHelper.Close();
//  //        }
//  //      }
//  //      else if (oracleDbHelper != null)
//  //      {
//  //        try
//  //        {
//  //          var specialCharAllowed = objSystemSettings.SpecialCharAllowed == false ? 0 : 1;
//  //          var changePassAfterFirstLogin = objSystemSettings.ChangePassFirstLogin == false ? 0 : 1;
//  //          var odbcClientList = objSystemSettings.OdbcClientList == false ? 0 : 1;
//  //          string LastUpdatedDate = DateTime.Today.ToString("dd/MMM/yyyy");
//  //          LastUpdatedDate = LastUpdatedDate.Replace('/', '-');
//  //          if (objSystemSettings.SettingsId == 0)
//  //          {
//  //            quary = string.Format(InsertSystemSettingsData, objSystemSettings.CompanyId,
//  //                                  objSystemSettings.Theme, objSystemSettings.Language,
//  //                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
//  //                                  objSystemSettings.PassType, specialCharAllowed,
//  //                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
//  //                                  changePassAfterFirstLogin,
//  //                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
//  //                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
//  //                                  "to_date('" + LastUpdatedDate + "')", odbcClientList,
//  //                                  objSystemSettings.UserId, objSystemSettings.IsPasswordChange, objSystemSettings.IsPasswordExpire,
//  //                                  objSystemSettings.IsWebLoginEnable,
//  //                                  objSystemSettings.DeleteApproveLeaveUponPunch,
//  //                                  objSystemSettings.DeleteLateUponAttendanceApproval, objSystemSettings.IsSingleBranchApplicable,
//  //                                  objSystemSettings.CheckPreviousAbsenteeism, objSystemSettings.IsTotalBillingApplicable
//  //                                  , objSystemSettings.BypassDefaultStateForSameBoss, objSystemSettings.DefaultLateDeductionDays, objSystemSettings.CheckApproverSettings);
//  //          }
//  //          else
//  //          {
//  //            quary = string.Format(UpdateSystemSettingsData, objSystemSettings.CompanyId,
//  //                                  objSystemSettings.Theme, objSystemSettings.Language,
//  //                                  objSystemSettings.MinLoginLength, objSystemSettings.MinPassLength,
//  //                                  objSystemSettings.PassType, specialCharAllowed,
//  //                                  objSystemSettings.WrongAttemptNo, objSystemSettings.ChangePassDays,
//  //                                  changePassAfterFirstLogin,
//  //                                  objSystemSettings.PassExpiryDays, objSystemSettings.ResetPass,
//  //                                  objSystemSettings.PassResetBy, objSystemSettings.OldPassUseRestriction,
//  //                                  "to_date('" + LastUpdatedDate + "')", odbcClientList,
//  //                                  objSystemSettings.UserId, objSystemSettings.IsPasswordChange, objSystemSettings.IsPasswordExpire,
//  //                                  objSystemSettings.IsWebLoginEnable,
//  //                                  objSystemSettings.DeleteApproveLeaveUponPunch,
//  //                                  objSystemSettings.DeleteLateUponAttendanceApproval, objSystemSettings.IsSingleBranchApplicable, objSystemSettings.CheckPreviousAbsenteeism, objSystemSettings.IsTotalBillingApplicable
//  //                                  , objSystemSettings.BypassDefaultStateForSameBoss, objSystemSettings.DefaultLateDeductionDays, objSystemSettings.CheckApproverSettings, objSystemSettings.SettingsId);
//  //          }
//  //          oracleDbHelper.ExecuteNonQuery(quary);
//  //          res = "Success";
//  //        }
//  //        catch (Exception ex)
//  //        {
//  //          res = ex.Message;
//  //          oracleDbHelper.RollBack();
//  //        }
//  //        finally
//  //        {
//  //          oracleDbHelper.Close();
//  //        }
//  //      }
//  //    }

//  //    return res;
//  //  }

//  //  public DataTable SystemSettingsData()
//  //  {
//  //    var dt = new DataTable();
//  //    DbHelper();
//  //    var quary = string.Format(@"Select * from SYSTEMSETTINGS");
//  //    if (SqlDbHelper != null)
//  //    {

//  //      //to_date('" + attendanceDate.ToString("MM/dd/yyyy");
//  //      try
//  //      {


//  //        dt = SqlDbHelper.DataTable(quary);
//  //      }
//  //      catch (Exception)
//  //      {

//  //        throw;
//  //      }
//  //      finally
//  //      {
//  //        SqlDbHelper.Close();
//  //      }
//  //    }
//  //    if (oracleDbHelper != null)
//  //    {
//  //      try
//  //      {

//  //        dt = oracleDbHelper.Table(quary);
//  //      }
//  //      catch (Exception)
//  //      {

//  //        throw;
//  //      }
//  //      finally
//  //      {
//  //        oracleDbHelper.Close();
//  //      }
//  //    }
//  //    return dt;
//  //  }


//  //  public SystemSettings CheckPaddingExistOrNot(int hrRecordId)
//  //  {
//  //    string quary = string.Format(@"Select * from SystemSettings
//  //where CompanyId = (Select CompanyId from Employment where HrREcordId={0})", hrRecordId);

//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings CheckPaddingExistOrNot(int hrRecordId, CommonConnection connection)
//  //  {
//  //    string quary = string.Format(@"Select * from SystemSettings
//  //where CompanyId = (Select CompanyId from Employment where HrREcordId={0})", hrRecordId);

//  //    return connection.Data<SystemSettings>(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings CheckPaddingExistOrNotByCompanyId(int companyId)
//  //  {
//  //    string quary = string.Format(@"Select * from SystemSettings
//  //where CompanyId = {0}", companyId);

//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings CheckPadding()
//  //  {
//  //    string quary = string.Format(@"Select * from SystemSettings where IsPaddingApplicable = 1");

//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings SystemSettingsDataByUserId(int userId)
//  //  {
//  //    string quary =
//  //        string.Format(
//  //            @"Select * from SystemSettings where CompanyId = (Select CompanyId from Employment where HrRecordId = (Select EmployeeId from Users where UserId={0}))",
//  //            userId);
//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings SystemSettingsDataByHrRecordId(int hrRecordId)
//  //  {
//  //    string quary =
//  //        string.Format(
//  //            @"Select * from SystemSettings where CompanyId = (Select CompanyId from Employment where HrRecordId = {0})",
//  //            hrRecordId);
//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings SystemSettingsDataByEmployeeId(string employeeId)
//  //  {
//  //    string quary =
//  //         string.Format(
//  //             @"Select * from SystemSettings where CompanyId = (Select CompanyId from Employment where EmployeeId = '{0}')",
//  //             employeeId);
//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }

//  //  public SystemSettings SystemSettingsDataByCostCentreSalaryMappingCompanyId(int costCentreId)
//  //  {
//  //    string quary =
//  //     string.Format(
//  //         @"Select * from SystemSettings where CompanyId = (Select SalaryCompanyMappingId from CostCentre where CostCentreId = {0})",
//  //         costCentreId);
//  //    return Data<SystemSettings>.DataSource(quary).FirstOrDefault();
//  //  }
//  #endregion EmpressCode

//}
