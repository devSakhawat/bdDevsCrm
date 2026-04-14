namespace bdDevCRM.Shared.DataTransferObjects.CRM;

public class CrmCourseDDLDto
{
	public int CourseId { get; set; }
	public string CourseTitle { get; set; }
	public decimal? ApplicationFee { get; set; }
	public int? CurrencyId { get; set; }
}
