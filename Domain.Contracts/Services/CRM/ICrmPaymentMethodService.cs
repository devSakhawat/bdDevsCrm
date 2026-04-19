using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmPaymentMethodService
{
  //Task<IEnumerable<CrmPaymentMethodDDL>> PaymentMethodsDDLAsync(bool trackChanges);
  //Task<IEnumerable<CrmPaymentMethodDDL>> OnlinePaymentMethodsDDLAsync(bool trackChanges);
  //Task<GridEntity<CrmPaymentMethodDto>> SummaryGrid(GridOptions options);
  //Task<string> CreateNewRecordAsync(CrmPaymentMethodDto modelDto);
  //Task<string> UpdateNewRecordAsync(int key, CrmPaymentMethodDto modelDto, bool trackChanges);
  //Task<string> DeleteRecordAsync(int key, CrmPaymentMethodDto modelDto);
  //Task<string> SaveOrUpdate(int key, CrmPaymentMethodDto modelDto);
  //Task<IEnumerable<CrmPaymentMethodDto>> PaymentMethodsAsync(bool trackChanges);
  //Task<CrmPaymentMethodDto> PaymentMethodAsync(int paymentMethodId, bool trackChanges);
  //Task<CrmPaymentMethodDto> CreatePaymentMethodAsync(CrmPaymentMethodDto entityForCreate);

  Task<CrmPaymentMethodDto> CreateAsync(CreateCrmPaymentMethodRecord record, CancellationToken cancellationToken = default);
  Task<CrmPaymentMethodDto> UpdateAsync(UpdateCrmPaymentMethodRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmPaymentMethodRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmPaymentMethodDto> PaymentMethodAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmPaymentMethodDto>> PaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmPaymentMethodDto>> ActivePaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmPaymentMethodDto>> OnlinePaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmPaymentMethodDto>> PaymentMethodForDDLAsync(CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmPaymentMethodDto>> OnlinePaymentMethodForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmPaymentMethodDto>> PaymentMethodsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}