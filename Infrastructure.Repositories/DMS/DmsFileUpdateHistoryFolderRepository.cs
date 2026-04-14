using bdDevCRM.Entities.Entities.DMS;
using bdDevCRM.RepositoriesContracts.DMS;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.DMS;

public class DmsFileUpdateHistoryRepository : RepositoryBase<DmsFileUpdateHistory>, IDmsFileUpdateHistoryRepository
{
  public DmsFileUpdateHistoryRepository(CRMContext context) : base(context) { }


}
