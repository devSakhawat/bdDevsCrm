// Interface: IUsersRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IUsersRepository : IRepositoryBase<Users>
  {
    /// <summary>
    /// Retrieves all users asynchronously.
    /// </summary>
    Task<IEnumerable<Users>> UsersAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single user by ID asynchronously.
    /// </summary>
    Task<Users?> UserAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by login ID asynchronously.
    /// </summary>
    Task<Users?> UserByLoginIdAsync(string loginId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves user details by login ID using raw SQL asynchronously.
    /// </summary>
    Task<Users?> UserByLoginIdRawAsync(string loginId, CancellationToken cancellationToken = default);


    /// <summary>
    /// Retrieves password history for a user.
    /// </summary>
    Task<IQueryable<PasswordHistory>> PasswordHistoriesAsync(int userId, int passRestriction, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves users by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<Users>> UsersByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new user.
    /// </summary>
    Task<Users> CreateUserAsync(Users user, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    void UpdateUser(Users user) => UpdateByState(user);

    /// <summary>
    /// Deletes a user.
    /// </summary>
    Task DeleteUserAsync(Users user, bool trackChanges, CancellationToken cancellationToken = default);
  }
}




//using Domain.Entities.Entities.System;
//using bdDevCRM.s.Core.HR;
//using Domain.Contracts.Repositories;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface IUsersRepository : IRepositoryBase<Users>
//{
//  IEnumerable<Users> Users(bool trackChanges);

//  Users User(int UsersId, bool trackChanges);

//  IEnumerable<Users> ByIds(IEnumerable<int> ids, bool trackChanges);

//  Task<IEnumerable<Users>> UsersAsync(bool trackChanges);

//  Task<Users> UserAsync(int usersId, bool trackChanges);

//  Users? UserByLoginIdRaw(string loginId, bool trackChanges);

//  Users? UserByLoginId(string loginId, bool trackChanges);

//  void CreateUser(Users Users);

//  void UpdateUser(Users Users);

//  void DeleteUser(Users Users);

//  ////////////////////////////

//  Task<IQueryable<PasswordHistory>> PasswordHistory(int userId, int passRestriction);

//  string UpdateUser(Users users, PasswordHistory passwordHistory);
//}
