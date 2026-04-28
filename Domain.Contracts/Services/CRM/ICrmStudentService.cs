using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmStudentService
{
    Task<CrmStudentDto> CreateAsync(CreateCrmStudentRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentDto> UpdateAsync(UpdateCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmStudentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentDto> StudentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDto>> StudentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDto>> StudentsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDto>> StudentForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmStudentDto>> StudentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmStudentDto> ChangeStatusAsync(ChangeCrmStudentStatusRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentApplicationReadyCheckDto> ApplicationReadyCheckAsync(int studentId, CancellationToken cancellationToken = default);
    Task<ConvertToStudentResultDto> EvaluateLeadConversionAsync(ConvertToStudentRequestDto request, CancellationToken cancellationToken = default);
    Task<ConvertToStudentResultDto> ConvertLeadToStudentAsync(ConvertToStudentRequestDto request, CancellationToken cancellationToken = default);
}
