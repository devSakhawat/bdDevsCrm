using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCounsellingTypeService
{
    Task<CrmCounsellingTypeDto> CreateAsync(CreateCrmCounsellingTypeRecord record, CancellationToken cancellationToken = default);
    Task<CrmCounsellingTypeDto> UpdateAsync(UpdateCrmCounsellingTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCounsellingTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCounsellingTypeDto> CounsellingTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingTypeDto>> CounsellingTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingTypeDDLDto>> CounsellingTypeForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCounsellingTypeDto>> CounsellingTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
