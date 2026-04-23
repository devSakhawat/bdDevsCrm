using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Layout;

namespace Domain.Contracts.Services.Core.Infrastructure;

public interface IHeaderService
{
  Task<HeaderSummaryDto> GetHeaderSummaryAsync(UsersDto currentUser, CancellationToken cancellationToken = default);
}
