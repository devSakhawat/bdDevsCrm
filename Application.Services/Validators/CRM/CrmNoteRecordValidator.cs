using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmNoteRecord.</summary>
public class CreateCrmNoteRecordValidator : BaseRecordValidator<CreateCrmNoteRecord>
{
    public CreateCrmNoteRecordValidator()
    {
        RuleFor(x => x.EntityType)
            .NotEmpty().WithMessage("EntityType is required")
            .MaximumLength(100).WithMessage("EntityType cannot exceed 100 characters");

        RuleFor(x => x.EntityId)
            .GreaterThan(0).WithMessage("EntityId must be greater than 0");

        RuleFor(x => x.NoteText)
            .NotEmpty().WithMessage("NoteText is required")
            .MaximumLength(4000).WithMessage("NoteText cannot exceed 4000 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmNoteRecord.</summary>
public class UpdateCrmNoteRecordValidator : BaseRecordValidator<UpdateCrmNoteRecord>
{
    public UpdateCrmNoteRecordValidator()
    {
        RuleFor(x => x.NoteId)
            .GreaterThan(0).WithMessage("NoteId must be greater than 0");

        RuleFor(x => x.EntityType)
            .NotEmpty().WithMessage("EntityType is required")
            .MaximumLength(100).WithMessage("EntityType cannot exceed 100 characters");

        RuleFor(x => x.EntityId)
            .GreaterThan(0).WithMessage("EntityId must be greater than 0");

        RuleFor(x => x.NoteText)
            .NotEmpty().WithMessage("NoteText is required")
            .MaximumLength(4000).WithMessage("NoteText cannot exceed 4000 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmNoteRecord.</summary>
public class DeleteCrmNoteRecordValidator : BaseRecordValidator<DeleteCrmNoteRecord>
{
    public DeleteCrmNoteRecordValidator()
    {
        RuleFor(x => x.NoteId)
            .GreaterThan(0).WithMessage("NoteId must be greater than 0");
    }
}
