using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.Authentication;

public interface IAuthenticationRepository : IRepositoryBase<Users>
{
  Task<Users> AuthenticateByLoginId(string loginId, CancellationToken cancellationToken);

  Task<Users> AuthenticateByPassword(string loginId, string password, CancellationToken cancellationToken);



  //IEnumerable<Company> AllCompanies(bool trackChanges);
  //Company Company(int companyId, bool trackChanges);
  //void CreateCompany(Company Company);

  //IEnumerable<Company> ByIds(IEnumerable<int> ids, bool trackChanges);


  //Task<IEnumerable<Company>> CompaniesAsync(bool trackChanges);
  //Task<Company> CompanyAsync(int companyId, bool trackChanges);
  //void UpdateCompany(Company Company);
  //void DeleteCompany(Company Company);
}