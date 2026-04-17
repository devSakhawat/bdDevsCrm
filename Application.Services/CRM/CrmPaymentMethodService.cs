using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>
/// CrmPaymentMethod service implementing business logic for CrmPaymentMethod management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmPaymentMethodService : ICrmPaymentMethodService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmPaymentMethodService> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmPaymentMethodService"/> with required dependencies.
	/// </summary>
	public CrmPaymentMethodService(IRepositoryManager repository, ILogger<CrmPaymentMethodService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_config = configuration;
	}

	/// <summary>
	/// Creates a new payment method record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmPaymentMethodDto> CreateAsync(CreateCrmPaymentMethodRecord record, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmPaymentMethodRecord));

		_logger.LogInformation("Creating new payment method. PaymentMethodName: {PaymentMethodName}, Time: {Time}",
						record.PaymentMethodName, DateTime.UtcNow);

		// Map Record to Entity using Mapster
		CrmPaymentMethod paymentMethod = record.MapTo<CrmPaymentMethod>();
		int paymentMethodId = await _repository.CrmPaymentMethods.CreateAndIdAsync(paymentMethod, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Payment method created successfully. ID: {PaymentMethodId}, Time: {Time}",
						paymentMethodId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = paymentMethod.MapTo<CrmPaymentMethodDto>() with { PaymentMethodId = paymentMethodId };
		return resultDto;
	}

	/// <summary>
	/// Updates an existing payment method record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmPaymentMethodDto> UpdateAsync(UpdateCrmPaymentMethodRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmPaymentMethodRecord));

		_logger.LogInformation("Updating payment method. ID: {PaymentMethodId}, Time: {Time}", record.PaymentMethodId, DateTime.UtcNow);

		// Check if payment method exists
		var existingPaymentMethod = await _repository.CrmPaymentMethods
						.FirstOrDefaultAsync(x => x.PaymentMethodId == record.PaymentMethodId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("PaymentMethod", "PaymentMethodId", record.PaymentMethodId.ToString());

		// Map Record to Entity using Mapster
		CrmPaymentMethod paymentMethod = record.MapTo<CrmPaymentMethod>();
		_repository.CrmPaymentMethods.UpdateByState(paymentMethod);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Payment method updated successfully. ID: {PaymentMethodId}, Time: {Time}",
						record.PaymentMethodId, DateTime.UtcNow);

		return paymentMethod.MapTo<CrmPaymentMethodDto>();
	}

	/// <summary>
	/// Deletes a payment method record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmPaymentMethodRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.PaymentMethodId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting payment method. ID: {PaymentMethodId}, Time: {Time}", record.PaymentMethodId, DateTime.UtcNow);

		var paymentMethodEntity = await _repository.CrmPaymentMethods
						.FirstOrDefaultAsync(x => x.PaymentMethodId == record.PaymentMethodId, trackChanges, cancellationToken)
						?? throw new NotFoundException("PaymentMethod", "PaymentMethodId", record.PaymentMethodId.ToString());

		await _repository.CrmPaymentMethods.DeleteAsync(x => x.PaymentMethodId == record.PaymentMethodId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogWarning("Payment method deleted successfully. ID: {PaymentMethodId}, Time: {Time}",
						record.PaymentMethodId, DateTime.UtcNow);
	}

	/// <summary>
	/// Retrieves a single payment method record by its ID.
	/// </summary>
	public async Task<CrmPaymentMethodDto> PaymentMethodAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching payment method. ID: {PaymentMethodId}, Time: {Time}", id, DateTime.UtcNow);

		var paymentMethod = await _repository.CrmPaymentMethods
						.FirstOrDefaultAsync(x => x.PaymentMethodId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("PaymentMethod", "PaymentMethodId", id.ToString());

		_logger.LogInformation("Payment method fetched successfully. ID: {PaymentMethodId}, Time: {Time}",
						id, DateTime.UtcNow);

		return paymentMethod.MapTo<CrmPaymentMethodDto>();
	}

	/// <summary>
	/// Retrieves all payment method records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethodDto>> PaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all payment methods. Time: {Time}", DateTime.UtcNow);

		var paymentMethods = await _repository.CrmPaymentMethods.ListAsync(x => x.PaymentMethodId, trackChanges, cancellationToken);

		if (!paymentMethods.Any())
		{
			_logger.LogWarning("No payment methods found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmPaymentMethodDto>();
		}

		var paymentMethodsDto = paymentMethods.MapToList<CrmPaymentMethodDto>();

		_logger.LogInformation("Payment methods fetched successfully. Count: {Count}, Time: {Time}",
						paymentMethodsDto.Count(), DateTime.UtcNow);

		return paymentMethodsDto;
	}

	/// <summary>
	/// Retrieves active payment method records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethodDto>> ActivePaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active payment methods. Time: {Time}", DateTime.UtcNow);

		var paymentMethods = await _repository.CrmPaymentMethods.CrmPaymentMethodsAsync(trackChanges, cancellationToken);

		if (!paymentMethods.Any())
		{
			_logger.LogWarning("No active payment methods found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmPaymentMethodDto>();
		}

		var paymentMethodsDto = paymentMethods.MapToList<CrmPaymentMethodDto>();

		_logger.LogInformation("Active payment methods fetched successfully. Count: {Count}, Time: {Time}",
						paymentMethodsDto.Count(), DateTime.UtcNow);

		return paymentMethodsDto;
	}

	/// <summary>
	/// Retrieves active online payment method records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethodDto>> OnlinePaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching online payment methods. Time: {Time}", DateTime.UtcNow);

		var paymentMethods = await _repository.CrmPaymentMethods
						.ListByConditionAsync(
							x => x.IsActive && x.IsOnlinePayment,
							x => x.PaymentMethodName,
							trackChanges: trackChanges,
							descending: false,
							cancellationToken: cancellationToken);

		if (!paymentMethods.Any())
		{
			_logger.LogWarning("No online payment methods found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmPaymentMethodDto>();
		}

		var paymentMethodsDto = paymentMethods.MapToList<CrmPaymentMethodDto>();

		_logger.LogInformation("Online payment methods fetched successfully. Count: {Count}, Time: {Time}",
						paymentMethodsDto.Count(), DateTime.UtcNow);

		return paymentMethodsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all payment methods suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethodDto>> PaymentMethodForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching payment methods for dropdown list. Time: {Time}", DateTime.UtcNow);

		var paymentMethods = await _repository.CrmPaymentMethods.CrmPaymentMethodsAsync(false, cancellationToken);

		if (!paymentMethods.Any())
		{
			_logger.LogWarning("No payment methods found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmPaymentMethodDto>();
		}

		var paymentMethodsDto = paymentMethods.MapToList<CrmPaymentMethodDto>();

		_logger.LogInformation("Payment methods fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						paymentMethodsDto.Count(), DateTime.UtcNow);

		return paymentMethodsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of online payment methods suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethodDto>> OnlinePaymentMethodForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching online payment methods for dropdown list. Time: {Time}", DateTime.UtcNow);

		var paymentMethods = await _repository.CrmPaymentMethods
						.ListByConditionAsync(
							x => x.IsActive && x.IsOnlinePayment,
							x => x.PaymentMethodName,
							trackChanges: false,
							descending: false,
							cancellationToken: cancellationToken);

		if (!paymentMethods.Any())
		{
			_logger.LogWarning("No online payment methods found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmPaymentMethodDto>();
		}

		var paymentMethodsDto = paymentMethods.MapToList<CrmPaymentMethodDto>();

		_logger.LogInformation("Online payment methods fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						paymentMethodsDto.Count(), DateTime.UtcNow);

		return paymentMethodsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all payment methods.
	/// </summary>
	public async Task<GridEntity<CrmPaymentMethodDto>> PaymentMethodsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching payment methods summary grid. Time: {Time}", DateTime.UtcNow);

		const string sql = @"
SELECT PaymentMethodId, PaymentMethodName, PaymentMethodCode, Description, ProcessingFee, ProcessingFeeType, IsOnlinePayment, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy
FROM CrmPaymentMethod";

		const string orderBy = "PaymentMethodName ASC";

		return await _repository.CrmPaymentMethods.AdoGridDataAsync<CrmPaymentMethodDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}
