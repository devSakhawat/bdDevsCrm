using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmEnquiryRecord.</summary>
public class CreateCrmEnquiryRecordValidator : BaseRecordValidator<CreateCrmEnquiryRecord>
{
    public CreateCrmEnquiryRecordValidator()
    {
        RuleFor(x => x.EnquiryDate)
            .NotEmpty().WithMessage("EnquiryDate is required");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmEnquiryRecord.</summary>
public class UpdateCrmEnquiryRecordValidator : BaseRecordValidator<UpdateCrmEnquiryRecord>
{
    public UpdateCrmEnquiryRecordValidator()
    {
        RuleFor(x => x.EnquiryId)
            .GreaterThan(0).WithMessage("EnquiryId must be greater than 0");

        RuleFor(x => x.EnquiryDate)
            .NotEmpty().WithMessage("EnquiryDate is required");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmEnquiryRecord.</summary>
public class DeleteCrmEnquiryRecordValidator : BaseRecordValidator<DeleteCrmEnquiryRecord>
{
    public DeleteCrmEnquiryRecordValidator()
    {
        RuleFor(x => x.EnquiryId)
            .GreaterThan(0).WithMessage("EnquiryId must be greater than 0");
    }
}
