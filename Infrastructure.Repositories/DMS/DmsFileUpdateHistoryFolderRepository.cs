using Domain.Entities.Entities.DMS;
using Domain.Contracts.DMS;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.DMS;

public class DmsFileUpdateHistoryRepository : RepositoryBase<DmsFileUpdateHistory>, IDmsFileUpdateHistoryRepository
{
  public DmsFileUpdateHistoryRepository(CrmContext context) : base(context) { }


}
