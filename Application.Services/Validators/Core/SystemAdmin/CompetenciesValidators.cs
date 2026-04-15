using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCompetenciesRecord.
/// </summary>
public class CreateCompetenciesRecordValidator : BaseRecordValidator<CreateCompetenciesRecord>
{
    public CreateCompetenciesRecordValidator()
    {
        RuleFor(x => x.CompetencyName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.CompetencyName))
            .WithMessage($"Competency name cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for UpdateCompetenciesRecord.
/// </summary>
public class UpdateCompetenciesRecordValidator : BaseRecordValidator<UpdateCompetenciesRecord>
{
    public UpdateCompetenciesRecordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Competency ID must be greater than 0");

        RuleFor(x => x.CompetencyName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.CompetencyName))
            .WithMessage($"Competency name cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for DeleteCompetenciesRecord.
/// </summary>
public class DeleteCompetenciesRecordValidator : BaseRecordValidator<DeleteCompetenciesRecord>
{
    public DeleteCompetenciesRecordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Competency ID must be greater than 0");
    }
}
