using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateTokenBlacklistRecord.
/// </summary>
public class CreateTokenBlacklistRecordValidator : BaseRecordValidator<CreateTokenBlacklistRecord>
{
    public CreateTokenBlacklistRecordValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"Token cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TokenHash)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TokenHash))
            .WithMessage($"TokenHash cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateTokenBlacklistRecord.
/// </summary>
public class UpdateTokenBlacklistRecordValidator : BaseRecordValidator<UpdateTokenBlacklistRecord>
{
    public UpdateTokenBlacklistRecordValidator()
    {
        RuleFor(x => x.TokenId)
            .NotEmpty()
            .WithMessage("TokenId is required");

        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"Token cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TokenHash)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TokenHash))
            .WithMessage($"TokenHash cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteTokenBlacklistRecord.
/// </summary>
public class DeleteTokenBlacklistRecordValidator : BaseRecordValidator<DeleteTokenBlacklistRecord>
{
    public DeleteTokenBlacklistRecordValidator()
    {
        RuleFor(x => x.TokenId)
            .NotEmpty()
            .WithMessage("TokenId is required");

    }
}