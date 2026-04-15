namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new country.
/// </summary>
/// <param name="CountryName">Name of the country.</param>
/// <param name="CountryCode">ISO country code.</param>
/// <param name="Status">Active status (1 = active, 0 = inactive).</param>
public record CreateCountryRecord(
    string CountryName,
    string? CountryCode,
    int? Status);

/// <summary>
/// Record for updating an existing country.
/// </summary>
/// <param name="CountryId">ID of the country to update.</param>
/// <param name="CountryName">Updated country name.</param>
/// <param name="CountryCode">Updated ISO country code.</param>
/// <param name="Status">Updated active status.</param>
public record UpdateCountryRecord(
    int CountryId,
    string CountryName,
    string? CountryCode,
    int? Status);

/// <summary>
/// Record for deleting a country.
/// </summary>
/// <param name="CountryId">ID of the country to delete.</param>
public record DeleteCountryRecord(int CountryId);
