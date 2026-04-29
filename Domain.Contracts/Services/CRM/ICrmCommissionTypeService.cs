using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCommissionTypeService
{
    Task<CrmCommissionTypeDto> CreateAsync(CreateCrmCommissionTypeRecord record, CancellationToken cancellationToken = default);
    Task<CrmCommissionTypeDto> UpdateAsync(UpdateCrmCommissionTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCommissionTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommissionTypeDto> CommissionTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionTypeDto>> CommissionTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommissionTypeDDLDto>> CommissionTypeForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCommissionTypeDto>> CommissionTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
