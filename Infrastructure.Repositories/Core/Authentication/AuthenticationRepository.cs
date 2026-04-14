using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.Authentication;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.Authentication;

public class AuthenticationRepository : RepositoryBase<Users>, IAuthenticationRepository
{
  public AuthenticationRepository(CRMContext context) : base(context) { }


  public async Task<Users> AuthenticateByLoginId(string loginId, CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(loginId)) return null;

    Users user = await FirstOrDefaultAsync(u => u.LoginId == loginId, true, cancellationToken);
    if (user == null)
      return null;

    return user;
  }


  public async Task<Users> AuthenticateByPassword(string loginId, string password, CancellationToken cancellationToken)
  {
    if (string.IsNullOrEmpty(loginId) || string.IsNullOrEmpty(password))
      return null;


    Users user = await FirstOrDefaultAsync(u => u.LoginId == loginId, true);
    return user;
  }
}
