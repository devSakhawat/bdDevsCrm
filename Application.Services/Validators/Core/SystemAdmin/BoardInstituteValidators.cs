using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateBoardInstituteRecord.
/// </summary>
public class CreateBoardInstituteRecordValidator : BaseRecordValidator<CreateBoardInstituteRecord>
{
    public CreateBoardInstituteRecordValidator()
    {
        RuleFor(x => x.BoardInstituteName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.BoardInstituteName))
            .WithMessage($"BoardInstituteName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for UpdateBoardInstituteRecord.
/// </summary>
public class UpdateBoardInstituteRecordValidator : BaseRecordValidator<UpdateBoardInstituteRecord>
{
    public UpdateBoardInstituteRecordValidator()
    {
        RuleFor(x => x.BoardInstituteId)
            .GreaterThan(0)
            .WithMessage("BoardInstituteId must be greater than 0");

        RuleFor(x => x.BoardInstituteName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.BoardInstituteName))
            .WithMessage($"BoardInstituteName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for DeleteBoardInstituteRecord.
/// </summary>
public class DeleteBoardInstituteRecordValidator : BaseRecordValidator<DeleteBoardInstituteRecord>
{
    public DeleteBoardInstituteRecordValidator()
    {
        RuleFor(x => x.BoardInstituteId)
            .GreaterThan(0)
            .WithMessage("BoardInstituteId must be greater than 0");

    }
}