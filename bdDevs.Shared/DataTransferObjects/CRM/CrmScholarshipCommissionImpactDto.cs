namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmScholarshipCommissionImpactDto
{
    public int ApplicationId { get; set; }
    public decimal TuitionAmount { get; set; }
    public decimal ScholarshipAmount { get; set; }
    public decimal CommissionableAmount { get; set; }
    public decimal InstituteCommissionRate { get; set; }
    public byte? InstituteCommissionType { get; set; }
    public decimal EstimatedCommissionAmount { get; set; }
}
