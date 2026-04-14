using System.Globalization;

namespace Application.Shared.Grid;

public static class DateTimeFormatters
{
  public static String OracleDate(DateTime date)
  {
    return "to_date('" + date.ToShortDateString() + "','DD/MM/YYYY')";
  }
  public static String OracleDate(DateTime date, string format)
  {
    return "to_date('" + date.ToShortDateString() + "','" + format + "')";
  }
  public static String OracleFromDate(DateTime date)
  {
    return "to_date('" + date.ToShortDateString() + " 00:00:00" + "','DD/MM/YYYY hh24:mi:ss')";
  }
  public static String OracleToDate(DateTime date)
  {
    return "to_date('" + date.ToShortDateString() + " 23:59:59" + "','DD/MM/YYYY hh24:mi:ss')";
  }

  public static String OracleDateTime(DateTime date)
  {
    return "to_date('" + date.ToString("dd/MM/yyyy HH:mm:ss") + "','DD/MM/YYYY  hh24:mi:ss')";
  }
  public static String OracleDateTime(string date)
  {
    return "to_date('" + date + "','DD/MM/YYYY  hh24:mi:ss')";
  }
  public static String OracleDate(string date)
  {
    return "to_date('" + date + "','DD/MM/YYYY')";
  }

  public static DateTime MySystemDate(DateTime date)
  {
    ChangePattern("dd/MM/yyyy");
    DateTime dt = Convert.ToDateTime(date.ToShortDateString());
    return dt;
  }
  public static DateTime MySystemDateTime(DateTime date)
  {
    ChangePattern("dd/MM/yyyy");
    DateTime dt = Convert.ToDateTime(date);
    return dt;
  }
  public static void ChangePattern(string formate)
  {
    var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
    culture.DateTimeFormat.ShortDatePattern = formate;
    culture.DateTimeFormat.LongTimePattern = "HH:mm:ss";
    Thread.CurrentThread.CurrentCulture = culture;


  }
  public static CultureInfo Culture(string formate)
  {
    var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
    culture.DateTimeFormat.ShortDatePattern = formate;
    culture.DateTimeFormat.LongTimePattern = "";
    return culture;


  }
  public class SystemDate
  {
    public static DateTime Now()
    {
      ChangePattern("dd/MM/yyyy");
      DateTime dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
      return dt;
    }


    public static DateTime Now(string p)
    {
      ChangePattern(p);
      DateTime dt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
      return dt;
    }
  }
  public class SystemDateTime
  {
    public static DateTime Now()
    {
      ChangePattern("dd/MM/yyyy hh2:mm:ss");
      //DateTime dt = Convert.ToDateTime(DateTime.Now);
      return DateTime.Now;
    }

  }


  //applicantInfosDB.CreatedDate = DateTimeFormatters.IsValidDateTime(applicantInfosDB.CreatedDate)
  //                               ? applicantInfosDB.CreatedDate
  //                                       : DateTime.UtcNow;
  public static bool IsValidDateTime(DateTime? dateTime)
  {
    if (dateTime == null) return false;
    return dateTime > DateTime.MinValue &&
           dateTime < DateTime.MaxValue &&
           dateTime > new DateTime(1900, 1, 1);
  }
  //applicantInfosDB.CreatedDate = applicantInfosDB.CreatedDate.IsValidDate()
  //                             ? applicantInfosDB.CreatedDate
  //                                       : DateTime.UtcNow;
  public static bool IsValidDate(this DateTime? dateTime)
  {
    if (dateTime == null) return false;
    return dateTime > DateTime.MinValue && dateTime != default(DateTime);
  }
}

public struct OracleDateTime
{
  // "to_date('" + date + "','DD/MM/YYYY  hh24:mi:ss')";
}