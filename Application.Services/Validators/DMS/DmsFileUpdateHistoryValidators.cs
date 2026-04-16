using bdDevs.Shared.Records.DMS;
using FluentValidation;
using Application.Services.Validators;

namespace Application.Services.Validators.DMS;

public class CreateDmsFileUpdateHistoryRecordValidator : BaseRecordValidator<CreateDmsFileUpdateHistoryRecord>
{
    public CreateDmsFileUpdateHistoryRecordValidator()
    {
        RuleFor(x => x.EntityId)
            .NotEmpty()
            .WithMessage("Entity ID is required")
            .MaximumLength(MaxCodeLength)
            .WithMessage($"Entity ID cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.EntityType)
            .NotEmpty()
            .WithMessage("Entity type is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Entity type cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.OldFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldFilePath))
            .WithMessage($"Old file path cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NewFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NewFilePath))
            .WithMessage($"New file path cannot exceed {MaxStringLength} characters");
    }
}

public class UpdateDmsFileUpdateHistoryRecordValidator : BaseRecordValidator<UpdateDmsFileUpdateHistoryRecord>
{
    public UpdateDmsFileUpdateHistoryRecordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0");

        RuleFor(x => x.EntityId)
            .NotEmpty()
            .WithMessage("Entity ID is required")
            .MaximumLength(MaxCodeLength)
            .WithMessage($"Entity ID cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.EntityType)
            .NotEmpty()
            .WithMessage("Entity type is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Entity type cannot exceed {MaxNameLength} characters");
    }
}

public class DeleteDmsFileUpdateHistoryRecordValidator : BaseRecordValidator<DeleteDmsFileUpdateHistoryRecord>
{
    public DeleteDmsFileUpdateHistoryRecordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("ID must be greater than 0");
    }
}
