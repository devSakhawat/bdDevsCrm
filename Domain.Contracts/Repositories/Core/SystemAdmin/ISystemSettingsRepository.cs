// Interface: ISystemSettingsRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface ISystemSettingsRepository : IRepositoryBase<SystemSettings>
  {
    Task<SystemSettings?> SystemSettingsByCompanyIdAsync(int companyId, CancellationToken cancellationToken = default);
    Task<AssemblyInfo?> AssemblyInfoResultAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SystemSettings>> SystemSettingsAsync(bool trackChanges, CancellationToken cancellationToken = default);


	}
}



//using Domain.Entities.Entities.System;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface ISystemSettingsRepository : IRepositoryBase<SystemSettings>
//{
//  Task<SystemSettings> SystemSettingsDataByCompanyId(int companyId);
//  Task<AssemblyInfo?> AssemblyInfoResult();
//}
