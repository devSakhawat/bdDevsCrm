using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM enquiry management operations.</summary>
public interface ICrmEnquiryService
{
    /// <summary>Creates a new enquiry record.</summary>
    Task<CrmEnquiryDto> CreateAsync(CreateCrmEnquiryRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing enquiry record.</summary>
    Task<CrmEnquiryDto> UpdateAsync(UpdateCrmEnquiryRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes an enquiry record.</summary>
    Task DeleteAsync(DeleteCrmEnquiryRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single enquiry record by ID.</summary>
    Task<CrmEnquiryDto> EnquiryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all enquiry records.</summary>
    Task<IEnumerable<CrmEnquiryDto>> EnquiriesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmEnquiryDto>> EnquiryForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of enquiries.</summary>
    Task<GridEntity<CrmEnquiryDto>> EnquiriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
