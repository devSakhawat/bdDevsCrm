using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Domain.Contracts.Services.CRM;

public interface ICrmIntakeYearService
{
  //Task<IEnumerable<CrmIntakeYearDDL>> IntakeYearsDDLAsync(bool trackChanges);
  //Task<GridEntity<CrmIntakeYearDto>> SummaryGrid(bool trackChanges, GridOptions options, UsersDto user);
  //Task<string> CreateNewRecordAsync(CrmIntakeYearDto modelDto);
  //Task<string> UpdateNewRecordAsync(int key, CrmIntakeYearDto modelDto, bool trackChanges);
  //Task<string> DeleteRecordAsync(int key, CrmIntakeYearDto modelDto);
  //Task<string> SaveOrUpdate(int key, CrmIntakeYearDto modelDto);
  //Task<IEnumerable<CrmIntakeYearDto>> IntakeYearsAsync(bool trackChanges);
  //Task<CrmIntakeYearDto> IntakeYearAsync(int intakeYearId, bool trackChanges);
  //Task<CrmIntakeYearDto> CreateIntakeYearAsync(CrmIntakeYearDto entityForCreate);

  Task<CrmIntakeYearDto> CreateAsync(CreateCrmIntakeYearRecord record, CancellationToken cancellationToken = default);
  Task<CrmIntakeYearDto> UpdateAsync(UpdateCrmIntakeYearRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmIntakeYearRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmIntakeYearDto> IntakeYearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmIntakeYearDto>> IntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmIntakeYearDto>> ActiveIntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmIntakeYearDto>> IntakeYearForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmIntakeYearDto>> IntakeYearsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}