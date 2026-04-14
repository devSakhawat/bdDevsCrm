namespace Application.Shared.Grid;


public class DateDifference
{
  private readonly DiffValue _diffValue = new DiffValue();

  public static DiffValue DateDiff(DateTime fromDate, DateTime toDate)
  {
    int years = 0;
    int months = 0;
    int days = 0;
    if ((toDate.Year - fromDate.Year) > 0 ||
        (((toDate.Year - fromDate.Year) == 0) && ((fromDate.Month < toDate.Month) ||
                                                  ((fromDate.Month == toDate.Month) &&
                                                   (fromDate.Day <= toDate.Day)))))
    {
      int daysInFromdayMonth = DateTime.DaysInMonth(fromDate.Year, fromDate.Month);
      int daysRemain = toDate.Day + (daysInFromdayMonth - fromDate.Day);
      if (toDate.Month > fromDate.Month)
      {
        years = toDate.Year - fromDate.Year;
        months = toDate.Month - (fromDate.Month + 1) + Math.Abs(daysRemain / daysInFromdayMonth);
        days = (daysRemain % daysInFromdayMonth + daysInFromdayMonth) % daysInFromdayMonth;
      }
      else if (toDate.Month == fromDate.Month)
      {
        if (toDate.Day >= fromDate.Day)
        {
          years = toDate.Year - fromDate.Year;
          months = 0;
          days = toDate.Day - fromDate.Day;
        }
        else
        {
          years = (toDate.Year - 1) - fromDate.Year;
          months = 11;
          days = DateTime.DaysInMonth(fromDate.Year, fromDate.Month) - (fromDate.Day - toDate.Day);
        }
      }
      else
      {
        years = (toDate.Year - 1) - fromDate.Year;
        months = toDate.Month + (11 - fromDate.Month) + Math.Abs(daysRemain / daysInFromdayMonth);
        days = (daysRemain % daysInFromdayMonth + daysInFromdayMonth) % daysInFromdayMonth;
      }
    }
    else
    {
      throw new ArgumentException("To date must be greater than From date!");
    }
    return new DiffValue() { Years = years, Months = months, Days = days };
  }
}

public class DiffValue
{
  public int Years { get; set; }
  public int Months { get; set; }
  public int Days { get; set; }

  public override string ToString()
  {
    string priodString = "";
    if (Years > 0)
    {
      priodString = string.Format("{0} Year(s)", Years);
    }
    if (Months > 0)
    {
      if (Years > 0)
        priodString += string.Format(", {0} Month(s)", Months);
      else
        priodString += string.Format("{0} Month(s)", Months);

    }
    if (Days > 0)
    {
      if (Months > 0 || Years > 0)
        priodString += string.Format(", {0} Day(s)", Days);
      else
        priodString += string.Format("{0} Day(s)", Days);

    }

    return priodString;
  }
}
