using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCompetencyLevelRecord.
/// </summary>
public class CreateCompetencyLevelRecordValidator : BaseRecordValidator<CreateCompetencyLevelRecord>
{
    public CreateCompetencyLevelRecordValidator()
    {
        RuleFor(x => x.LevelTitle)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.LevelTitle))
            .WithMessage($"Level title cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");
    }
}

/// <summary>
/// Validator for UpdateCompetencyLevelRecord.
/// </summary>
public class UpdateCompetencyLevelRecordValidator : BaseRecordValidator<UpdateCompetencyLevelRecord>
{
    public UpdateCompetencyLevelRecordValidator()
    {
        RuleFor(x => x.LevelId)
            .GreaterThan(0)
            .WithMessage("Level ID must be greater than 0");

        RuleFor(x => x.LevelTitle)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.LevelTitle))
            .WithMessage($"Level title cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");
    }
}

/// <summary>
/// Validator for DeleteCompetencyLevelRecord.
/// </summary>
public class DeleteCompetencyLevelRecordValidator : BaseRecordValidator<DeleteCompetencyLevelRecord>
{
    public DeleteCompetencyLevelRecordValidator()
    {
        RuleFor(x => x.LevelId)
            .GreaterThan(0)
            .WithMessage("Level ID must be greater than 0");
    }
}
