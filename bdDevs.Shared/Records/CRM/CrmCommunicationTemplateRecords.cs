namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCommunicationTemplateRecord(
    int CommunicationTypeId,
    string TemplateName,
    string? Subject,
    string TemplateBody,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmCommunicationTemplateRecord(
    int CommunicationTemplateId,
    int CommunicationTypeId,
    string TemplateName,
    string? Subject,
    string TemplateBody,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmCommunicationTemplateRecord(int CommunicationTemplateId);
