namespace bdDevs.Shared.Records.Core.HR;

/// <summary>
/// Record for creating a new branch.
/// </summary>
/// <param name="Branchname">Name of the branch.</param>
/// <param name="Branchcode">Branch code.</param>
/// <param name="Branchdescription">Description of the branch.</param>
/// <param name="IsCostCentre">Whether the branch is a cost centre.</param>
/// <param name="IsActive">Active status (1 = active, 0 = inactive).</param>
/// <param name="DebitAccountHead">Debit account head ID.</param>
/// <param name="CreditAccountHead">Credit account head ID.</param>
/// <param name="ContraEntryApplicable">Whether contra entry is applicable.</param>
/// <param name="BranchAddress">Physical address of the branch.</param>
public record CreateBranchRecord(
    string Branchname,
    string? Branchcode,
    string? Branchdescription,
    int? IsCostCentre,
    int? IsActive,
    int? DebitAccountHead,
    int? CreditAccountHead,
    int? ContraEntryApplicable,
    string? BranchAddress);

/// <summary>
/// Record for updating an existing branch.
/// </summary>
/// <param name="Branchid">ID of the branch to update.</param>
/// <param name="Branchname">Updated branch name.</param>
/// <param name="Branchcode">Updated branch code.</param>
/// <param name="Branchdescription">Updated branch description.</param>
/// <param name="IsCostCentre">Updated cost centre flag.</param>
/// <param name="IsActive">Updated active status.</param>
/// <param name="DebitAccountHead">Updated debit account head ID.</param>
/// <param name="CreditAccountHead">Updated credit account head ID.</param>
/// <param name="ContraEntryApplicable">Updated contra entry flag.</param>
/// <param name="BranchAddress">Updated branch address.</param>
public record UpdateBranchRecord(
    int Branchid,
    string Branchname,
    string? Branchcode,
    string? Branchdescription,
    int? IsCostCentre,
    int? IsActive,
    int? DebitAccountHead,
    int? CreditAccountHead,
    int? ContraEntryApplicable,
    string? BranchAddress);

/// <summary>
/// Record for deleting a branch.
/// </summary>
/// <param name="Branchid">ID of the branch to delete.</param>
public record DeleteBranchRecord(int Branchid);
