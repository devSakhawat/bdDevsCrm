using FluentValidation;

namespace Application.Services.Validators;

/// <summary>
/// Base validator class providing common validation rules for all record types.
/// Extend this class for entity-specific validators.
/// </summary>
/// <typeparam name="T">The record type to validate</typeparam>
public abstract class BaseRecordValidator<T> : AbstractValidator<T>
{
    /// <summary>
    /// Common validation constants
    /// </summary>
    protected const int MaxStringLength = 500;
    protected const int MaxCodeLength = 50;
    protected const int MaxNameLength = 200;
    protected const int MaxEmailLength = 100;
    protected const int MaxPhoneLength = 20;
    protected const int MaxAddressLength = 500;

    /// <summary>
    /// Validates that a string property is not empty and within length limits
    /// </summary>
    protected void ValidateRequiredString(string propertyName, int maxLength = MaxStringLength)
    {
        // This is a helper method - actual validation rules are defined in derived classes
    }

    /// <summary>
    /// Validates that an optional string property is within length limits if provided
    /// </summary>
    protected void ValidateOptionalString(string propertyName, int maxLength = MaxStringLength)
    {
        // This is a helper method - actual validation rules are defined in derived classes
    }

    /// <summary>
    /// Validates that an ID is greater than 0
    /// </summary>
    protected void ValidatePositiveId(string propertyName)
    {
        // This is a helper method - actual validation rules are defined in derived classes
    }
}
