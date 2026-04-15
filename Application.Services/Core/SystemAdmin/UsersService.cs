using Application.Shared.Grid;
using Application.Services.Mappings;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using Domain.Exceptions.DomainSpecific;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;

namespace Application.Services.Core.SystemAdmin;

/// <summary>
/// Users service implementing business logic for user management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class UsersService : IUsersService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<UsersService> _logger;
  private readonly IConfiguration _configuration;

  /// <summary>
  /// Initializes a new instance of <see cref="UsersService"/> with required dependencies.
  /// </summary>
  /// <param name="repository">The repository manager for data access operations.</param>
  /// <param name="logger">The logger for capturing service-level events.</param>
  /// <param name="configuration">The application configuration accessor.</param>
  public UsersService(IRepositoryManager repository, ILogger<UsersService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  /// <summary>
  /// Creates a new user record after validating for null input and duplicate login ID.
  /// </summary>
  public async Task<UsersDto> CreateUserAsync(UsersDto entityForCreate, CancellationToken cancellationToken = default)
  {
    if (entityForCreate is null)
      throw new BadRequestException(nameof(UsersDto));

    bool userExists = await _repository.Users.ExistsAsync(
            x => x.LoginId.Trim().ToLower() == entityForCreate.LoginId.Trim().ToLower(),
            cancellationToken: cancellationToken);

    if (userExists)
      throw new NotFoundException("Data not found!");

    entityForCreate.CreatedDate = DateTime.UtcNow;
    entityForCreate.LastUpdatedDate = DateTime.UtcNow;
    entityForCreate.IsExpired = false;

    if (!string.IsNullOrEmpty(entityForCreate.Password))
      entityForCreate.Password = EncryptDecryptHelper.Encrypt(entityForCreate.Password);

    Users userEntity = MyMapper.JsonClone<UsersDto, Users>(entityForCreate);

    await _repository.Users.CreateAsync(userEntity, cancellationToken);
    int affected = await _repository.SaveChangesAsync(cancellationToken);

    if (affected <= 0)
      throw new InvalidOperationException("User could not be saved to the database.");

    _logger.LogInformation(
            "User created successfully. ID: {UserId}, LoginId: {LoginId}, Time: {Time}",
            userEntity.UserId,
            userEntity.LoginId,
            DateTime.UtcNow);

    return MyMapper.JsonClone<Users, UsersDto>(userEntity);
  }

  /// <summary>
  /// Updates an existing user record by merging only the changed values from the provided DTO.
  /// </summary>
  public async Task<UsersDto> UpdateUserAsync(int userId, UsersDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (modelDto is null)
      throw new BadRequestException(nameof(UsersDto));

    if (userId != modelDto.UserId)
      throw new BadRequestException(userId.ToString(), nameof(UsersDto));

    Users existingEntity = await _repository.Users
            .FirstOrDefaultAsync(x => x.UserId == userId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Data not found!");

    bool duplicateExists = await _repository.Users.ExistsAsync(
            x => x.LoginId.Trim().ToLower() == modelDto.LoginId.Trim().ToLower()
                && x.UserId != userId,
            cancellationToken: cancellationToken);

    if (duplicateExists)
      throw new NotFoundException("Data not found!");

    if (!string.IsNullOrEmpty(modelDto.Password))
      modelDto.Password = EncryptDecryptHelper.Encrypt(modelDto.Password);

    Users updatedEntity = MyMapper.MergeChangedValues<Users, UsersDto>(existingEntity, modelDto);
    updatedEntity.LastUpdatedDate = DateTime.UtcNow;

    if (updatedEntity.IsActive == true)
      updatedEntity.FailedLoginNo = 0;

    _repository.Users.UpdateByState(updatedEntity);

    int affected = await _repository.SaveChangesAsync(cancellationToken);
    if (affected <= 0)
      throw new NotFoundException("Data not found!");

    _logger.LogInformation(
            "User updated. ID: {UserId}, LoginId: {LoginId}, Time: {Time}",
            updatedEntity.UserId,
            updatedEntity.LoginId,
            DateTime.UtcNow);

    return MyMapper.JsonClone<Users, UsersDto>(updatedEntity);
  }

  /// <summary>
  /// Deletes a user record identified by the given ID.
  /// </summary>
  public async Task<int> DeleteUserAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (userId <= 0)
      throw new BadRequestException(userId.ToString(), nameof(UsersDto));

    Users userEntity = await _repository.Users
            .FirstOrDefaultAsync(x => x.UserId == userId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Data not found!");

    await _repository.Users.DeleteAsync(x => x.UserId == userId, trackChanges, cancellationToken);
    int affected = await _repository.SaveChangesAsync(cancellationToken);

    if (affected <= 0)
      throw new NotFoundException("Data not found!");

    _logger.LogWarning(
            "User deleted. ID: {UserId}, LoginId: {LoginId}, Time: {Time}",
            userEntity.UserId,
            userEntity.LoginId,
            DateTime.UtcNow);

    return affected;
  }

  /// <summary>
  /// Retrieves a single user record by its ID.
  /// </summary>
  public async Task<UsersDto> UserAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    Users user = await _repository.Users
            .FirstOrDefaultAsync(x => x.UserId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Data not found!");

    _logger.LogInformation(
            "User fetched successfully. ID: {UserId}, LoginId: {LoginId}, Time: {Time}",
            user.UserId,
            user.LoginId,
            DateTime.UtcNow);

    return MyMapper.JsonClone<Users, UsersDto>(user);
  }

  /// <summary>
  /// Retrieves all user records from the database.
  /// </summary>
  public async Task<IEnumerable<UsersDto>> UsersAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    _logger.LogInformation("Fetching all users. Time: {Time}", DateTime.UtcNow);

    IEnumerable<Users> users = await _repository.Users.UsersAsync(trackChanges, cancellationToken);

    if (!users.Any())
    {
      _logger.LogWarning("No users found. Time: {Time}", DateTime.UtcNow);
      return Enumerable.Empty<UsersDto>();
    }

    IEnumerable<UsersDto> usersDto = MyMapper.JsonCloneIEnumerableToIEnumerable<Users, UsersDto>(users);

    _logger.LogInformation(
            "Users fetched successfully. Count: {Count}, Time: {Time}",
            usersDto.Count(),
            DateTime.UtcNow);

    return usersDto;
  }

  /// <summary>
  /// Retrieves multiple user records by their IDs.
  /// </summary>
  public async Task<IEnumerable<UsersDto>> UsersByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (ids is null)
      throw new BadRequestException("Invalid request!");

    List<int> idList = ids.ToList();

    IEnumerable<Users> userEntities = await _repository.Users.UsersByIdsAsync(idList, trackChanges, cancellationToken);

    if (idList.Count != userEntities.ToList().Count)
      throw new BadRequestException("Invalid request!");

    IEnumerable<UsersDto> usersToReturn = MyMapper.JsonCloneIEnumerableToIEnumerable<Users, UsersDto>(userEntities);

    _logger.LogInformation(
            "Users fetched by IDs. Count: {Count}, Time: {Time}",
            usersToReturn.Count(),
            DateTime.UtcNow);

    return usersToReturn;
  }

  /// <summary>
  /// Retrieves a user record by login ID.
  /// </summary>
  public async Task<UsersDto?> UserByLoginIdAsync(string loginId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (string.IsNullOrWhiteSpace(loginId))
    {
      _logger.LogWarning("UserByLoginIdAsync called with null or whitespace loginId");
      return null;
    }

    _logger.LogInformation("Fetching user by login ID: {LoginId}, Time: {Time}", loginId, DateTime.UtcNow);

    //Users? user = await _repository.Users.GetUserByLoginIdRaw(loginId, trackChanges, cancellationToken);
    Users? user = await _repository.Users.UserByLoginIdRawAsync(loginId, cancellationToken);

    if (user is null)
    {
      _logger.LogWarning("No user found for login ID: {LoginId}, Time: {Time}", loginId, DateTime.UtcNow);
      return null;
    }

    UsersDto usersDto = MyMapper.JsonClone<Users, UsersDto>(user);

    _logger.LogInformation(
            "User fetched by login ID successfully. ID: {UserId}, LoginId: {LoginId}, Time: {Time}",
            usersDto.UserId,
            loginId,
            DateTime.UtcNow);

    return usersDto;
  }

  /// <summary>
  /// Retrieves password history for the specified user.
  /// </summary>
  public async Task<IEnumerable<PasswordHistoryDto>> PasswordHistoryByUserIdAsync(int userId, int passRestriction, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (userId <= 0)
    {
      _logger.LogWarning("PasswordHistoryByUserIdAsync called with invalid userId: {UserId}", userId);
      throw new BadRequestException("Invalid request!");
    }

    _logger.LogInformation("Fetching password history for userId: {UserId}, Time: {Time}", userId, DateTime.UtcNow);

    IEnumerable<PasswordHistory> passwordHistory = await _repository.Users
            .PasswordHistoriesAsync(userId, passRestriction, cancellationToken);

    if (passwordHistory is null || !passwordHistory.Any())
    {
      _logger.LogWarning("No password history found for userId: {UserId}, Time: {Time}", userId, DateTime.UtcNow);
      return Enumerable.Empty<PasswordHistoryDto>();
    }

    IEnumerable<PasswordHistoryDto> passwordHistoryDto = MyMapper.JsonCloneIEnumerableToList<PasswordHistory, PasswordHistoryDto>(passwordHistory);

    _logger.LogInformation(
            "Password history fetched successfully. UserId: {UserId}, Count: {Count}, Time: {Time}",
            userId,
            passwordHistoryDto.Count(),
            DateTime.UtcNow);

    return passwordHistoryDto;
  }

  /// <summary>
  /// Retrieves a paginated summary grid of all users with company, branch, and department information.
  /// </summary>
  public async Task<GridEntity<UsersDto>> UsersSummaryAsync(int companyId, bool trackChanges, GridOptions options, UsersDto currentUser, CancellationToken cancellationToken = default)
  {
    if (currentUser is null || currentUser.HrRecordId is null)
    {
      _logger.LogWarning("UsersSummaryAsync called with null currentUser or HrRecordId");
      throw new BadRequestException(nameof(UsersDto));
    }

    _logger.LogInformation("Fetching users summary grid. CompanyId: {CompanyId}, Time: {Time}", companyId, DateTime.UtcNow);

    IEnumerable<Groups> objGroups = await _repository.AccessRestrictions.GroupsByHrRecordIdAsync((int)currentUser.HrRecordId, cancellationToken);
    //IEnumerable<Groups> objGroups = await _repository.AccessRestrictions.AccessRestrictionGroupsByHrRecordId((int)currentUser.HrRecordId, cancellationToken);

    string condition = string.Empty;
    string groupCondition = string.Empty;

    if (objGroups.Any())
    {
      string groupIds = string.Join(",", objGroups.Select(x => x.GroupId));
      if (!string.IsNullOrEmpty(groupIds))
        groupCondition = $" OR GroupId IN ({groupIds})";
    }

    string query =
        $@"SELECT 
                Users.*,
                Employment.DepartmentId,
                BranchId,
                Employment.EmployeeId AS Employee_Id,
                Employee.ShortName,
                Department.DepartmentName,
                Users.EmployeeId AS HrRecordId
            FROM Users
            INNER JOIN Employment ON Employment.HrRecordId = Users.EmployeeID
            INNER JOIN Employee ON Employee.HrRecordId = Employment.HrRecordId
            LEFT JOIN Department ON Employment.DepartmentId = Department.DepartmentId
            {condition}";

    string orderBy = "UserName ASC";

    return await _repository.Users.AdoGridDataAsync<UsersDto>(query, options, orderBy, "", cancellationToken);
  }

  /// <summary>
  /// Saves a user record (create or update) with validation and group member assignment.
  /// </summary>
  public async Task<UsersDto> SaveUserAsync(UsersDto usersDto, CancellationToken cancellationToken = default)
  {
    if (usersDto is null)
      throw new BadRequestException(nameof(UsersDto));

    _logger.LogInformation("Saving user. UserId: {UserId}, LoginId: {LoginId}, Time: {Time}",
            usersDto.UserId, usersDto.LoginId, DateTime.UtcNow);

    SystemSettings objsystem = await _repository.SystemSettings.SystemSettingsByCompanyIdAsync((int)usersDto.CompanyId, cancellationToken)
            //SystemSettings objsystem = await _repository.SystemSettings.GetSystemSettingsDataByCompanyId((int)usersDto.CompanyId, cancellationToken)
            ?? throw new BadRequestException("Please First Save System Settings Data");

    string validate = usersDto.UserId == 0
        ? ValidateUser(usersDto, objsystem)
        : await ValidateUserForUpdateAsync(usersDto, objsystem, cancellationToken);

    if (validate != "Valid")
      throw new InvalidOperationExceptionEx(validate);

    //await using var transaction = await _repository.Users.TransactionBeginAsync(cancellationToken);
    await _repository.Users.TransactionBeginAsync(cancellationToken);

    try
    {
      if (usersDto.UserId == 0)
      {
        Users userByLoginId = await _repository.Users
                .FirstOrDefaultAsync(x => x.LoginId.ToLower().Trim() == usersDto.LoginId.ToLower().Trim(), trackChanges: false, cancellationToken);

        if (userByLoginId is null)
        {
          bool userExistsByEmployee = await _repository.Users
                  .ExistsAsync(x => x.EmployeeId == usersDto.EmployeeId, cancellationToken);

          if (!userExistsByEmployee)
          {
            usersDto.CreatedDate = DateTime.UtcNow;
            usersDto.LastUpdatedDate = DateTime.UtcNow;
            usersDto.IsExpired = false;
            usersDto.Password = EncryptDecryptHelper.Encrypt(usersDto.Password);

            Users objUsers = MyMapper.JsonClone<UsersDto, Users>(usersDto);
            objUsers = await _repository.Users.CreateUserAsync(objUsers, cancellationToken);
            usersDto.UserId = objUsers.UserId;
            //usersDto.UserId = await _repository.Users.CreateUserAsync(objUsers, cancellationToken);

            List<GroupMember> groupMembers = new();
            if (usersDto.GroupMembers is not null)
            {
              foreach (var groupMember in usersDto.GroupMembers)
              {
                groupMembers.Add(new GroupMember { GroupId = groupMember.GroupId, UserId = (int)usersDto.UserId });
              }
            }

            if (groupMembers.Count > 0)
              await _repository.GroupMembers.BulkInsertAsync(groupMembers, cancellationToken);

            await _repository.GroupMembers.TransactionCommitAsync(cancellationToken);

            _logger.LogInformation("User created successfully via SaveUserAsync. ID: {UserId}, Time: {Time}",
                    usersDto.UserId, DateTime.UtcNow);

            return usersDto;
          }
          else
          {
            _logger.LogWarning("User already exists for EmployeeId: {EmployeeId}", usersDto.EmployeeId);
            throw new ConflictException("Duplicate data found!");
          }
        }
        else
        {
          _logger.LogWarning("User already exists for LoginId: {LoginId}", usersDto.LoginId);
          throw new ConflictException("Duplicate data found!");
        }
      }
      else
      {
        var objUserNewByLoginId = await _repository.Users
                .FirstOrDefaultAsync(x => x.LoginId == usersDto.LoginId && x.UserId != usersDto.UserId, trackChanges: false, cancellationToken);

        if (objUserNewByLoginId is null)
        {
          var objUserForDb = await _repository.Users
                  .FirstOrDefaultAsync(x => x.UserId == usersDto.UserId, trackChanges: false, cancellationToken);

          objUserForDb.CompanyId = usersDto.CompanyId;
          objUserForDb.LoginId = usersDto.LoginId;
          objUserForDb.Password = EncryptDecryptHelper.Encrypt(usersDto.Password);
          objUserForDb.UserName = usersDto.UserName;
          objUserForDb.IsActive = usersDto.IsActive;
          objUserForDb.AccessParentCompany = usersDto.AccessParentCompany;
          objUserForDb.LastUpdatedDate = DateTime.UtcNow;
          objUserForDb.DefaultDashboard = usersDto.DefaultDashboard;

          if (objUserForDb.IsActive == true)
            objUserForDb.FailedLoginNo = 0;

          objUserForDb.LastLoginDate = objUserForDb.LastLoginDate.HasValue && objUserForDb.LastLoginDate.Value != DateTime.MinValue
              ? objUserForDb.LastLoginDate
              : null;

          _repository.Users.Update(objUserForDb);
          await _repository.SaveChangesAsync(cancellationToken);

          _repository.Users.ClearChangeTracker();

          string deleteSql = $"DELETE FROM GroupMembers WHERE UserId = {usersDto.UserId}";
          _repository.GroupMembers.EfCoreExecuteNonQuery(deleteSql, cancellationToken);

          _repository.GroupMembers.ClearChangeTracker();

          if (usersDto.GroupMembers is not null && usersDto.GroupMembers.Any())
          {
            string insertSql = string.Empty;
            foreach (var gm in usersDto.GroupMembers)
            {
              insertSql += $"INSERT INTO GroupMembers (GroupId, UserId) VALUES ({gm.GroupId}, {usersDto.UserId});";
            }
            _repository.GroupMembers.EfCoreExecuteNonQuery(insertSql, cancellationToken);
          }

          await _repository.Users.TransactionCommitAsync(cancellationToken);

          _logger.LogInformation("User updated successfully via SaveUserAsync. ID: {UserId}, Time: {Time}",
                  usersDto.UserId, DateTime.UtcNow);

          return usersDto;
        }
        else
        {
          _logger.LogWarning("User already exists for LoginId: {LoginId}", usersDto.LoginId);
          throw new NotFoundException("Data not found!");
        }
      }
    }
    catch (Exception ex)
    {
      await _repository.Users.TransactionRollbackAsync(cancellationToken);
      _logger.LogError(ex, "Error saving user. UserId: {UserId}, Time: {Time}", usersDto.UserId, DateTime.UtcNow);
      throw;
    }
    finally
    {
      await _repository.Users.TransactionDisposeAsync();
    }
  }

  /// <summary>
  /// Validates user data for creation based on system settings.
  /// </summary>
  private string ValidateUser(UsersDto users, SystemSettings objsystem)
  {
    string specialChs = @"! ~ @ # $ % ^ & * ( ) _ - + = { } [ ] : ; , . < > ? / | \";
    string[] specialCharacters = specialChs.Split(' ');
    string message = "Valid";

    if (!string.IsNullOrEmpty(users.LoginId))
    {
      if (objsystem.MinLoginLength > users.LoginId.Trim().Length)
      {
        message = $"Login ID must have to be minimum {objsystem.MinLoginLength} character length!";
        throw new InvalidOperationExceptionEx(message);
      }
    }

    if (objsystem.MinPassLength > users.Password.Trim().Length)
    {
      message = $"Password must have to be minimum {objsystem.MinPassLength} character length!";
      throw new InvalidOperationExceptionEx(message);
    }

    if (objsystem.MinLoginLength == 0 && objsystem.MinPassLength == 0 && objsystem.SpecialCharAllowed == false)
      throw new InvalidOperationExceptionEx(message);

    int numCount = 0;
    int charCount = 0;
    int specialcharCount = 0;
    char[] pasChars = users.Password.ToCharArray();

    for (int i = 0; i < pasChars.Length; i++)
    {
      if (char.IsDigit(pasChars[i]))
        numCount++;
      else
      {
        IEnumerable<string> found = specialCharacters.Where(x => x == pasChars[i].ToString());
        if (!found.Any())
          charCount++;
        else
          specialcharCount++;
      }
    }

    if (objsystem.PassType == 0)
    {
      if (numCount > 0)
      {
        message = "Password must not have any number!";
        throw new InvalidOperationExceptionEx(message);
      }

      if (charCount == 0)
      {
        message = "Password must have to be alphabetic characters!";
        throw new InvalidOperationExceptionEx(message);
      }
    }
    else if (objsystem.PassType == 1)
    {
      if (numCount == 0)
      {
        message = "Password must have atleast one numeric character!";
        throw new InvalidOperationExceptionEx(message);
      }

      if (charCount > 0)
      {
        message = "Password must not have any alphabetic character!";
        throw new InvalidOperationExceptionEx(message);
      }
    }
    else
    {
      if (numCount == 0)
      {
        message = "Password must have atleast one numeric character!";
        throw new InvalidOperationExceptionEx(message);
      }

      if (charCount == 0)
      {
        message = "Password must have atleast one alphabetic character!";
        throw new InvalidOperationExceptionEx(message);
      }
    }

    if (objsystem.SpecialCharAllowed == true && specialcharCount == 0)
    {
      message = "Password must have atleast one special character!";
      throw new InvalidOperationExceptionEx(message);
    }

    return message;
  }

  /// <summary>
  /// Validates user data for update based on system settings.
  /// </summary>
  private async Task<string> ValidateUserForUpdateAsync(UsersDto users, SystemSettings objsystem, CancellationToken cancellationToken)
  {
    Users userByUserId = await _repository.Users
            .FirstOrDefaultAsync(x => x.UserId == users.UserId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Data not found!");

    users.Password = string.IsNullOrEmpty(users.Password)
        ? EncryptDecryptHelper.Decrypt(userByUserId.Password)
        : users.Password;

    return ValidateUser(users, objsystem);
  }
}