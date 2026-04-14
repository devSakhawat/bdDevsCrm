using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using Application.Shared.Grid;
using Domain.Contracts.Repositories;
using Microsoft.Data.SqlClient;

namespace Domain.Contracts.Repositories;

/// <summary>
/// Enterprise-level Generic Repository Interface
/// Defines comprehensive data access contract with both synchronous and asynchronous methods
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepositoryBase<T> where T : class
{
  #region Create Operations

  /// <summary>
  /// Adds an entity to the context (synchronous - no I/O)
  /// </summary>
  void Create(T entity);

  /// <summary>
  /// Adds an entity to the context asynchronously
  /// </summary>
  Task CreateAsync(T entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Creates an entity and returns its ID (synchronous)
  /// </summary>
  int CreateAndId(T entity);

  /// <summary>
  /// Creates an entity and returns its ID asynchronously
  /// </summary>
  Task<int> CreateAndIdAsync(T entity, CancellationToken cancellationToken = default);

  /// <summary>
  /// Adds multiple entities to the context (synchronous)
  /// </summary>
  void BulkInsert(IEnumerable<T> entities);

  /// <summary>
  /// Adds multiple entities to the context asynchronously
  /// </summary>
  Task BulkInsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

  #endregion

  #region Update Operations

  /// <summary>
  /// Marks an entity as modified
  /// </summary>
  void Update(T entity);

  /// <summary>
  /// Attaches an entity and marks it as modified
  /// </summary>
  void UpdateByState(T entity);

  #endregion

  #region Delete Operations

  /// <summary>
  /// Removes an entity from the context
  /// </summary>
  void Delete(T entity);

  /// <summary>
  /// Deletes an entity matching the predicate (synchronous)
  /// </summary>
  void DeleteByPredicate(Expression<Func<T, bool>> predicate, bool trackChanges = false);

  /// <summary>
  /// Deletes an entity matching the predicate asynchronously
  /// </summary>
  Task DeleteAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// Executes a DELETE statement directly against the database without loading entities into memory.
  /// This bypasses the change tracker and is more efficient for bulk deletes (EF Core 7.0+).
  /// </summary>
  Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

  /// <summary>
  /// Removes multiple entities from the context (synchronous)
  /// </summary>
  void BulkDelete(IEnumerable<T> entities);

  /// <summary>
  /// Removes multiple entities from the context asynchronously
  /// </summary>
  Task BulkDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

  #endregion

  #region Read Operations - Single Entity

  /// <summary>
  /// s a single entity by predicate (synchronous)
  /// </summary>
  T? ById(Expression<Func<T, bool>> predicate, bool trackChanges = false);

  /// <summary>
  /// s a single entity by predicate asynchronously
  /// </summary>
  Task<T?> ByIdAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// s the first entity matching the expression (synchronous)
  /// </summary>
  T? FirstOrDefault(Expression<Func<T, bool>>? expression = null, bool trackChanges = false);

  /// <summary>
  /// s the first entity matching the expression asynchronously
  /// </summary>
  Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// s the first entity matching the expression with descending order (synchronous)
  /// </summary>
  T? FirstOrDefaultWithOrderByDesc(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

  /// <summary>
  /// s the first entity matching the expression with descending order asynchronously
  /// </summary>
  Task<T?> FirstOrDefaultWithOrderByDescAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default);

  #endregion

  #region Read Operations - Multiple Entities

  /// <summary>
  /// s entities matching the expression (synchronous)
  /// </summary>
  IEnumerable<T> ListByIds(Expression<Func<T, bool>> expression, bool trackChanges = false);

  /// <summary>
  /// s entities matching the expression asynchronously
  /// </summary>
  Task<IEnumerable<T>> ListByIdsAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// s all entities with optional ordering (synchronous)
  /// </summary>
  IEnumerable<T> List(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

  /// <summary>
  /// s all entities with optional ordering asynchronously
  /// </summary>
  Task<IEnumerable<T>> ListAsync(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// s entities matching condition with optional ordering (synchronous)
  /// </summary>
  IEnumerable<T> ListByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

  /// <summary>
  /// s entities matching condition with optional ordering asynchronously
  /// </summary>
  Task<IEnumerable<T>> ListByConditionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, bool descending = false, CancellationToken cancellationToken = default);

  #endregion

  #region Projection Operations

  /// <summary>
  /// s entities with projection and optional ordering (synchronous)
  /// </summary>
  IEnumerable<TResult> ListWithSelect<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

  /// <summary>
  /// s entities with projection and optional ordering asynchronously
  /// </summary>
  Task<IEnumerable<TResult>> ListWithSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default);

  /// <summary>
  /// s entities with filter, projection, and optional ordering (synchronous)
  /// </summary>
  IEnumerable<TResult> ListByWhereWithSelect<TResult>(Expression<Func<T, bool>>? expression = null, Expression<Func<T, TResult>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

  /// <summary>
  /// s entities with filter, projection, and optional ordering asynchronously
  /// </summary>
  Task<IEnumerable<TResult>> ListByWhereWithSelectAsync<TResult>(Expression<Func<T, TResult>>? selector = null, Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default);

  #endregion

  #region Count and Exists Operations

  /// <summary>
  /// s the count of all entities (synchronous)
  /// </summary>
  int Count();

  /// <summary>
  /// s the count of entities matching the expression (synchronous)
  /// </summary>
  int Count(Expression<Func<T, bool>> expression);

  /// <summary>
  /// s the count of all entities asynchronously
  /// </summary>
  Task<int> CountAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// s the count of entities matching the expression asynchronously
  /// </summary>
  Task<int> CountAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

  /// <summary>
  /// Checks if any entity matching the expression exists (synchronous)
  /// </summary>
  bool Exists(Expression<Func<T, bool>> expression);

  /// <summary>
  /// Checks if any entity matching the expression exists asynchronously
  /// </summary>
  Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);

  #endregion

  #region Transaction Management

  /// <summary>
  /// Begins a new database transaction (synchronous)
  /// </summary>
  void TransactionBegin();

  /// <summary>
  /// Begins a new database transaction asynchronously
  /// </summary>
  Task TransactionBeginAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Commits the current transaction (synchronous)
  /// </summary>
  void TransactionCommit();

  /// <summary>
  /// Commits the current transaction asynchronously
  /// </summary>
  Task TransactionCommitAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Rolls back the current transaction (synchronous)
  /// </summary>
  void TransactionRollback();

  /// <summary>
  /// Rolls back the current transaction asynchronously
  /// </summary>
  Task TransactionRollbackAsync(CancellationToken cancellationToken = default);

  /// <summary>
  /// Disposes the current transaction if exists asynchronously
  /// </summary>
  Task TransactionDisposeAsync();

  #endregion

  #region Change Tracker Management

  /// <summary>
  /// Clears all tracked entities from the ChangeTracker
  /// </summary>
  void ClearChangeTracker();

  /// <summary>
  /// Clears all tracked entities from the ChangeTracker asynchronously
  /// </summary>
  Task ClearChangeTrackerAsync();

  #endregion

  #region Raw SQL Execution

  // =================================================================
  // EXECUTE NON QUERY (INSERT/UPDATE/DELETE)
  // =================================================================
  /// <summary>
  /// Executes a raw SQL command (synchronous)
  /// </summary>
  string EfCoreExecuteNonQuery(string query, CancellationToken cancellationToken = default);

  /// <summary>
  /// Executes a raw SQL command asynchronously
  /// </summary>
  Task<string> EfCoreExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default);


  // =================================================================
  // EXECUTE Raw query To Single Entity EF Core Methods
  // =================================================================
  /// <summary>
  /// Executes a raw SQL query and returns a single entity
  /// </summary>
  T? EfCoreExecuteSingleQuery(string query);

  /// <summary>
  /// Executes a raw SQL query and returns a single entity
  /// </summary>
  Task<T?> EfCoreExecuteSingleQueryAsync(string query, CancellationToken cancellationToken = default);


  // =================================================================
  // EXECUTE Raw Query To List of Entity EF Core Methods
  // =================================================================
  /// <summary>
  /// Executes a raw SQL query and returns a list of entities (synchronous)
  /// </summary>
  IEnumerable? EFCoreExecuteListQuery(string query);

  /// <summary>
  /// Executes a raw SQL query and returns a list of entities (asynchronous)
  /// </summary>
  Task<IEnumerable?> EFCoreExecuteListQueryAsync(string query, CancellationToken cancellationToken = default);

  #endregion

  #region ADO.NET Query Execution

  /// <summary>
  /// Executes a SQL query and returns results as a DataTable
  /// </summary>
  DataTable AdoDataTable(string sqlQuery, params DbParameter[] parameters);

  /// <summary>
  /// Executes a query and returns grid data with pagination (Synchronous)
  /// </summary>
  GridEntity<TGrid> AdoGridData<TGrid>(string query, GridOptions options, string orderBy, string condition);

  /// <summary>
  /// Executes a query and returns grid data with pagination (Asynchronous)
  /// </summary>
  Task<GridEntity<TGrid>> AdoGridDataAsync<TGrid>(string query, GridOptions options, string orderBy, string condition, CancellationToken cancellationToken = default);

  /// <summary>
  /// Executes a query and returns a single result (Synchronous)
  /// </summary>
  TResult? AdoExecuteSingleData<TResult>(string query, SqlParameter[]? parameters = null) where TResult : class, new();

  /// <summary>
  /// Executes a query and returns a single result (Asynchronous)
  /// </summary>
  Task<TResult?> AdoExecuteSingleDataAsync<TResult>(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) where TResult : class, new();

  // =================================================================
  // LIST QUERY METHODS AD0.NET
  // =================================================================  
  /// <summary>
  /// Executes a query and returns a list of results (Synchronous)
  /// </summary>
  IEnumerable<TResult> AdoExecuteListQuery<TResult>(string query, SqlParameter[]? parameters = null) where TResult : class, new();

  /// <summary>
  /// Executes a query and returns a list of results (Asynchronous)
  /// </summary>
  /// 
  Task<IEnumerable<TResult>> AdoExecuteListQueryAsync<TResult>(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) where TResult : class, new();

  #endregion
}



//public interface IRepositoryBase<T>
//{
//  #region Basic CRUD Operations
//  void Create(T entity);
//  Task CreateAsync(T entity);
//  Task<int> CreateAndIdAsync(T entity);
//  Task BulkInsertAsync(IEnumerable<T> entities);

//  #region Transaction Management Methods
//  Task TransactionBeginAsync();
//  Task TransactionCommitAsync();
//  Task TransactionRollbackAsync();
//  Task TransactionDisposeAsync();
//  #endregion Transaction Management Methods

//  void Update(T entity);
//  void UpdateByState(T entity);

//  void Delete(T entity);
//  Task DeleteAsync(Expression<Func<T, bool>> predicate, bool trackChanges);
//  /// <summary>
//  /// Executes a DELETE statement directly against the database without loading entities into memory.
//  /// This bypasses the change tracker and is more efficient for bulk deletes.
//  /// Available in EF Core 7.0+
//  /// </summary>
//  /// <param name="predicate">Filter expression to determine which entities to delete</param>
//  /// <param name="cancellationToken">Cancellation token</param>
//  /// <returns>Number of rows deleted</returns>
//  Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

//  void BulkDelete(IEnumerable<T> entities);

//  /// <summary>
//  /// Clears all tracked entities from the ChangeTracker
//  /// </summary>
//  void ClearChangeTracker();

//  /// <summary>
//  /// Clears all tracked entities from the ChangeTracker asynchronously
//  /// </summary>
//  Task ClearChangeTrackerAsync();

//  T ById(Expression<Func<T, bool>> predicate, bool trackChanges = false);
//  Task<T> ByIdAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false);
//  T FirstOrDefault(Expression<Func<T, bool>>? expression = null, bool trackChanges = false);
//  Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool trackChanges = false);
//  Task<T> FirstOrDefaultWithOrderByDescAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

//  IEnumerable<T> ListByIds(Expression<Func<T, bool>> expression, bool trackChanges = false);
//  Task<IEnumerable<T>> ListByIdsAsync(Expression<Func<T, bool>> expression, bool trackChanges = false);

//  IEnumerable<T> List(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);
//  Task<IEnumerable<T>> ListAsync(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);

//  IEnumerable<T> ListByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);
//  Task<IEnumerable<T>> ListByConditionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, bool descending = false);


//  IEnumerable<T> ListWithSelect<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false);
//  Task<IEnumerable<TResult>> ListWithSelectAsync<TResult>(
//    Expression<Func<T, TResult>> selector,
//    Expression<Func<T, object>>? orderBy = null,
//    bool trackChanges = false);


//  IEnumerable<TResult> ListByWhereWithSelect<TResult>(
//     Expression<Func<T, bool>>? expression = null,
//     Expression<Func<T, TResult>>? selector = null,
//     Expression<Func<T, object>>? orderBy = null,
//     bool trackChanges = false);

//  Task<IEnumerable<TResult>> ListByWhereWithSelectAsync<TResult>(
//     Expression<Func<T, TResult>>? selector = null,
//     Expression<Func<T, bool>>? expression = null,
//     Expression<Func<T, object>>? orderBy = null,
//     bool trackChanges = false);


//  Task<int> CountAsync();
//  Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
//  #endregion  Basic CRUD Operations

//  #region  Query Execute by TDT
//  string ExecuteNonQuery(string query);
//  Task<IEnumerable<T>?> ExecuteListSql(string query);
//  Task<T?> ExecuteSingleSql(string query);

//  DataTable DataTable(string sqlQuery, params DbParameter[] parameters);
//  #endregion  Query Execute by TDT



//  #region  Data using ado.net
//  Task<GridEntity<T>> GridData<T>(string query, GridOptions options, string orderBy, string condition);
//  Task<TResult> ExecuteSingleData<TResult>(string query, SqlParameter[] parameters = null) where TResult : class, new();
//  TResult ExecuteSingleDataSyncronous<TResult>(string query, SqlParameter[] parameters = null) where TResult : class, new();
//  Task<IEnumerable<TResult>> ExecuteListQuery<TResult>(string query, SqlParameter[] parameters = null) where TResult : class, new();
//  #endregion  Data using ado.net

//}
