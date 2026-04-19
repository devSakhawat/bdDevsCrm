using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmIntakeMonthService
{
  //Task<IEnumerable<CrmIntakeMonthDDL>> IntakeMonthsDDLAsync(bool trackChanges);
  //Task<GridEntity<CrmIntakeMonthDto>> SummaryGrid(GridOptions options);
  //Task<string> CreateNewRecordAsync(CrmIntakeMonthDto modelDto);
  //Task<string> UpdateNewRecordAsync(int key, CrmIntakeMonthDto modelDto, bool trackChanges);
  //Task<string> DeleteRecordAsync(int key, CrmIntakeMonthDto modelDto);
  //Task<string> SaveOrUpdate(int key, CrmIntakeMonthDto modelDto);
  //Task<IEnumerable<CrmIntakeMonthDto>> IntakeMonthsAsync(bool trackChanges);
  //Task<CrmIntakeMonthDto> IntakeMonthAsync(int intakeMonthId, bool trackChanges);
  //Task<CrmIntakeMonthDto> CreateIntakeMonthAsync(CrmIntakeMonthDto entityForCreate);

  Task<CrmIntakeMonthDto> CreateAsync(CreateCrmIntakeMonthRecord record, CancellationToken cancellationToken = default);
  Task<CrmIntakeMonthDto> UpdateAsync(UpdateCrmIntakeMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmIntakeMonthRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmIntakeMonthDto> IntakeMonthAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmIntakeMonthDto>> IntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmIntakeMonthDto>> ActiveIntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmIntakeMonthDto>> IntakeMonthForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmIntakeMonthDto>> IntakeMonthsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}