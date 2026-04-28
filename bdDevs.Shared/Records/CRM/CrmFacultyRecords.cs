namespace bdDevs.Shared.Records.CRM;

public record CreateCrmFacultyRecord(int InstituteId, string FacultyName, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmFacultyRecord(int FacultyId, int InstituteId, string FacultyName, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmFacultyRecord(int FacultyId);
