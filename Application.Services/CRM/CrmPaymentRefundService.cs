using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmPaymentRefundService : ICrmPaymentRefundService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmPaymentRefundService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ICrmCommissionService _commissionService;

    public CrmPaymentRefundService(IRepositoryManager repository, ILogger<CrmPaymentRefundService> logger, IConfiguration configuration, ICrmCommissionService commissionService)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
        _commissionService = commissionService;
    }

    public async Task<CrmPaymentRefundDto> CreateAsync(CreateCrmPaymentRefundRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmPaymentRefundRecord));
        var payment = await _repository.CrmStudentPayments.StudentPaymentAsync(record.PaymentId, true, cancellationToken)
            ?? throw new NotFoundException("CrmStudentPayment", "StudentPaymentId", record.PaymentId.ToString());
        if (record.RefundAmount <= 0 || record.RefundAmount > payment.Amount)
            throw new BadRequestException("Refund amount must be greater than zero and not exceed the payment amount.");

        var entity = record.MapTo<CrmPaymentRefund>();
        int newId = await _repository.CrmPaymentRefunds.CreateAndIdAsync(entity, cancellationToken);
        payment.Status = record.Status >= 2 ? (byte)6 : (byte)5;
        payment.UpdatedBy = record.CreatedBy;
        payment.UpdatedDate = DateTime.UtcNow;
        _repository.CrmStudentPayments.UpdateByState(payment);
        await _repository.SaveAsync(cancellationToken);
        await _commissionService.ReverseByRefundAsync(record.PaymentId, record.RefundAmount, record.RefundDate, record.CreatedBy, record.Reason, cancellationToken);
        entity.PaymentRefundId = newId;
        return entity.MapTo<CrmPaymentRefundDto>();
    }

    public async Task<CrmPaymentRefundDto> UpdateAsync(UpdateCrmPaymentRefundRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmPaymentRefundRecord));
        _ = await _repository.CrmPaymentRefunds.PaymentRefundAsync(record.PaymentRefundId, false, cancellationToken)
            ?? throw new NotFoundException("CrmPaymentRefund", "PaymentRefundId", record.PaymentRefundId.ToString());
        var entity = record.MapTo<CrmPaymentRefund>();
        _repository.CrmPaymentRefunds.UpdateByState(entity);
        var payment = await _repository.CrmStudentPayments.StudentPaymentAsync(record.PaymentId, true, cancellationToken);
        if (payment != null)
        {
            payment.Status = record.Status >= 2 ? (byte)6 : (byte)5;
            payment.UpdatedBy = record.UpdatedBy ?? record.CreatedBy;
            payment.UpdatedDate = DateTime.UtcNow;
            _repository.CrmStudentPayments.UpdateByState(payment);
        }
        await _repository.SaveAsync(cancellationToken);
        await _commissionService.ReverseByRefundAsync(record.PaymentId, record.RefundAmount, record.RefundDate, record.UpdatedBy ?? record.CreatedBy, record.Reason, cancellationToken);
        return entity.MapTo<CrmPaymentRefundDto>();
    }

    public async Task DeleteAsync(DeleteCrmPaymentRefundRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.PaymentRefundId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmPaymentRefunds.DeleteAsync(x => x.PaymentRefundId == record.PaymentRefundId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmPaymentRefundDto> PaymentRefundAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmPaymentRefunds.PaymentRefundAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmPaymentRefund", "PaymentRefundId", id.ToString())).MapTo<CrmPaymentRefundDto>();

    public async Task<IEnumerable<CrmPaymentRefundDto>> PaymentRefundsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmPaymentRefunds.PaymentRefundsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmPaymentRefundDto>() : Enumerable.Empty<CrmPaymentRefundDto>();
    }

    public async Task<IEnumerable<CrmPaymentRefundDto>> PaymentRefundsByPaymentIdAsync(int paymentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmPaymentRefunds.PaymentRefundsByPaymentIdAsync(paymentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmPaymentRefundDto>() : Enumerable.Empty<CrmPaymentRefundDto>();
    }

    public async Task<GridEntity<CrmPaymentRefundDto>> PaymentRefundsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT PaymentRefundId, PaymentId, RefundAmount, RefundDate, RefundMethod, ApprovedBy, Reason, Status, ProcessedDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmPaymentRefund";
        return await _repository.CrmPaymentRefunds.AdoGridDataAsync<CrmPaymentRefundDto>(sql, options, "PaymentRefundId DESC", string.Empty, cancellationToken);
    }
}
