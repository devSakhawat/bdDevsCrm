using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

public class CreateCrmCommunicationTemplateRecordValidator : BaseRecordValidator<CreateCrmCommunicationTemplateRecord>
{
    public CreateCrmCommunicationTemplateRecordValidator()
    {
        RuleFor(x => x.CommunicationTypeId)
            .GreaterThan(0)
            .WithMessage("CommunicationTypeId must be greater than 0");

        RuleFor(x => x.TemplateName)
            .NotEmpty()
            .WithMessage("TemplateName is required")
            .MaximumLength(150)
            .WithMessage("TemplateName cannot exceed 150 characters");

        RuleFor(x => x.Subject)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Subject))
            .WithMessage("Subject cannot exceed 250 characters");

        RuleFor(x => x.TemplateBody)
            .NotEmpty()
            .WithMessage("TemplateBody is required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class UpdateCrmCommunicationTemplateRecordValidator : BaseRecordValidator<UpdateCrmCommunicationTemplateRecord>
{
    public UpdateCrmCommunicationTemplateRecordValidator()
    {
        RuleFor(x => x.CommunicationTemplateId)
            .GreaterThan(0)
            .WithMessage("CommunicationTemplateId must be greater than 0");

        RuleFor(x => x.CommunicationTypeId)
            .GreaterThan(0)
            .WithMessage("CommunicationTypeId must be greater than 0");

        RuleFor(x => x.TemplateName)
            .NotEmpty()
            .WithMessage("TemplateName is required")
            .MaximumLength(150)
            .WithMessage("TemplateName cannot exceed 150 characters");

        RuleFor(x => x.Subject)
            .MaximumLength(250)
            .When(x => !string.IsNullOrEmpty(x.Subject))
            .WithMessage("Subject cannot exceed 250 characters");

        RuleFor(x => x.TemplateBody)
            .NotEmpty()
            .WithMessage("TemplateBody is required");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class DeleteCrmCommunicationTemplateRecordValidator : BaseRecordValidator<DeleteCrmCommunicationTemplateRecord>
{
    public DeleteCrmCommunicationTemplateRecordValidator()
    {
        RuleFor(x => x.CommunicationTemplateId)
            .GreaterThan(0)
            .WithMessage("CommunicationTemplateId must be greater than 0");
    }
}
