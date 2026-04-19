using Infrastructure.Sql.Context;
using Application.Shared.Grid;
using Domain.Contracts.Repositories;
using Domain.Exceptions.ServerError;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.Repositories;

/// <summary>
/// Enterprise-level Generic Repository Base Class
/// Provides both synchronous and asynchronous data access methods with comprehensive error handling,
/// transaction support, and performance optimizations.
///
/// <para><b>Usage Guidelines:</b></para>
/// <list type="bullet">
/// <item>
/// <description><b>Async methods (recommended):</b> Use in Web APIs, background services,
/// and all I/O-bound operations for better scalability (10-100x better throughput).</description>
/// </item>
/// <item>
/// <description><b>Sync methods:</b> Use only when necessary - legacy code integration,
/// constructors, property getters, or console applications.</description>
/// </item>
/// <item>
/// <description><b>CancellationToken:</b> Always pass CancellationToken in async methods
/// to support graceful cancellation.</description>
/// </item>
/// </list>
/// </summary>
/// <typeparam name="T">Entity type (must be a class)</typeparam>
public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
	#region Private Fields

	private readonly CrmContext _context;
	private readonly DbSet<T> _dbSet;
	private IDbContextTransaction? _currentTransaction;

	#endregion

	#region Constructor

	/// <summary>
	/// Initializes a new instance of the RepositoryBase class
	/// </summary>
	/// <param name="context">The database context</param>
	/// <exception cref="ArgumentNullException">Thrown when context is null</exception>
	public RepositoryBase(CrmContext context)
	{
		_context = context ?? throw new ArgumentNullException(nameof(context));
		_dbSet = _context.Set<T>();
	}

	#endregion

	#region Create Operations
	/// <summary>
	/// Adds an entity to the context (synchronous - no I/O)
	/// </summary>
	/// <param name="entity">Entity to add</param>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	public void Create(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbSet.Add(entity);
	}

	/// <summary>
	/// Adds an entity to the context asynchronously
	/// </summary>
	/// <param name="entity">Entity to add</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	public async Task CreateAsync(T entity, CancellationToken cancellationToken = default)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		await _dbSet.AddAsync(entity, cancellationToken);
	}

	/// <summary>
	/// Adds an entity to the context asynchronously
	/// </summary>
	/// <param name="entity">Entity to add</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	public async Task<int> CreateAndIdAsync(T entity, CancellationToken cancellationToken = default)
	{
		await _dbSet.AddAsync(entity, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);
		// Get the primary key property
		var keyProperty = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties[0];

		// Return the primary key value
		return (Int32)keyProperty.GetGetter().GetClrValue(entity);
	}

	/// <summary>
	/// Creates an entity and returns its ID (synchronous)
	/// </summary>
	/// <param name="entity">Entity to create</param>
	/// <returns>The primary key value</returns>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	/// <exception cref="InvalidOperationException">Thrown when entity has no primary key</exception>
	public int CreateAndId(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbSet.Add(entity);
		_context.SaveChanges();

		return PrimaryKeyValue(entity);
	}

	/// <summary>
	/// Adds multiple entities to the context (synchronous)
	/// </summary>
	/// <param name="entities">Collection of entities to add</param>
	/// <exception cref="ArgumentNullException">Thrown when entities is null or empty</exception>
	public void BulkInsert(IEnumerable<T> entities)
	{
		ValidateEntities(entities, nameof(entities));
		_dbSet.AddRange(entities);
	}

	/// <summary>
	/// Adds multiple entities to the context asynchronously
	/// </summary>
	/// <param name="entities">Collection of entities to add</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="ArgumentNullException">Thrown when entities is null or empty</exception>
	public async Task BulkInsertAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
	{
		ValidateEntities(entities, nameof(entities));
		await _dbSet.AddRangeAsync(entities, cancellationToken);
	}

	#endregion

	#region Update Operations

	/// <summary>
	/// Marks an entity as modified
	/// </summary>
	/// <param name="entity">Entity to update</param>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	public void Update(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbSet.Update(entity);
	}

	/// <summary>
	/// Attaches an entity and marks it as modified
	/// </summary>
	/// <param name="entity">Entity to update</param>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	public void UpdateByState(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbSet.Attach(entity);
		_context.Entry(entity).State = EntityState.Modified;
	}
	#endregion

	#region Delete Operations

	/// <summary>
	/// Removes an entity from the context
	/// </summary>
	/// <param name="entity">Entity to delete</param>
	/// <exception cref="ArgumentNullException">Thrown when entity is null</exception>
	public void Delete(T entity)
	{
		if (entity == null)
			throw new ArgumentNullException(nameof(entity));

		_dbSet.Remove(entity);
	}

	/// <summary>
	/// Deletes an entity matching the predicate (synchronous)
	/// </summary>
	/// <param name="predicate">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
	public void DeleteByPredicate(Expression<Func<T, bool>> predicate, bool trackChanges = false)
	{
		if (predicate == null)
			throw new ArgumentNullException(nameof(predicate));

		var entity = ById(predicate, trackChanges);
		if (entity != null)
		{
			_dbSet.Remove(entity);
		}
	}

	/// <summary>
	/// Deletes an entity matching the predicate asynchronously
	/// </summary>
	/// <param name="predicate">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
	public async Task DeleteAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (predicate == null)
			throw new ArgumentNullException(nameof(predicate));

		var entity = await ByIdAsync(predicate, trackChanges, cancellationToken);
		if (entity != null)
		{
			_dbSet.Remove(entity);
		}
	}

	/// <summary>
	/// Executes a DELETE statement directly against the database without loading entities into memory
	/// (EF Core 7.0+ feature - more efficient for bulk deletes)
	/// </summary>
	/// <param name="predicate">Filter expression</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Number of rows deleted</returns>
	/// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
	public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
	{
		if (predicate == null)
			throw new ArgumentNullException(nameof(predicate));

		return await _dbSet.Where(predicate).ExecuteDeleteAsync(cancellationToken);
	}

	/// <summary>
	/// Removes multiple entities from the context (synchronous)
	/// </summary>
	/// <param name="entities">Collection of entities to delete</param>
	/// <exception cref="ArgumentNullException">Thrown when entities is null or empty</exception>
	public void BulkDelete(IEnumerable<T> entities)
	{
		ValidateEntities(entities, nameof(entities));
		_dbSet.RemoveRange(entities);
	}

	/// <summary>
	/// Removes multiple entities from the context asynchronously
	/// </summary>
	/// <param name="entities">Collection of entities to delete</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="ArgumentNullException">Thrown when entities is null or empty</exception>
	public Task BulkDeleteAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
	{
		ValidateEntities(entities, nameof(entities));
		_dbSet.RemoveRange(entities);
		return Task.CompletedTask;
	}

	#endregion

	#region Read Operations - Single Entity

	/// <summary>
	/// s a single entity by predicate (synchronous)
	/// </summary>
	/// <param name="predicate">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes (default: false for better performance)</param>
	/// <returns>Entity or null if not found</returns>
	/// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
	public T? ById(Expression<Func<T, bool>> predicate, bool trackChanges = false)
	{
		if (predicate == null)
			throw new ArgumentNullException(nameof(predicate));

		return trackChanges
				? _dbSet.FirstOrDefault(predicate)
				: _dbSet.AsNoTracking().FirstOrDefault(predicate);
	}

	/// <summary>
	/// s a single entity by predicate asynchronously
	/// </summary>
	/// <param name="predicate">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes (default: false for better performance)</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Entity or null if not found</returns>
	/// <exception cref="ArgumentNullException">Thrown when predicate is null</exception>
	public async Task<T?> ByIdAsync(Expression<Func<T, bool>> predicate, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (predicate == null)
			throw new ArgumentNullException(nameof(predicate));

		return trackChanges
				? await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken)
				: await _dbSet.AsNoTracking().FirstOrDefaultAsync(predicate, cancellationToken);
	}

	/// <summary>
	/// s the first entity matching the expression (synchronous)
	/// </summary>
	/// <param name="expression">Filter expression (optional)</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Entity or null if not found</returns>
	public T? FirstOrDefault(Expression<Func<T, bool>>? expression = null, bool trackChanges = false)
	{
		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		return expression != null
				? query.FirstOrDefault(expression)
				: query.FirstOrDefault();
	}

	/// <summary>
	/// s the first entity matching the expression asynchronously
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Entity or null if not found</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		return trackChanges
				? await _dbSet.FirstOrDefaultAsync(expression, cancellationToken)
				: await _dbSet.AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
	}

	/// <summary>
	/// s the first entity matching the expression with descending order (synchronous)
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Entity or null if not found</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public T? FirstOrDefaultWithOrderByDesc(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (orderBy != null)
		{
			query = query.OrderByDescending(orderBy);
		}

		return query.FirstOrDefault(expression);
	}

	/// <summary>
	/// s the first entity matching the expression with descending order asynchronously
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Entity or null if not found</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public async Task<T?> FirstOrDefaultWithOrderByDescAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (orderBy != null)
		{
			query = query.OrderByDescending(orderBy);
		}

		return await query.FirstOrDefaultAsync(expression, cancellationToken);
	}

	#endregion

	#region Read Operations - Multiple Entities

	/// <summary>
	/// s entities matching the expression (synchronous)
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Collection of entities</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public IEnumerable<T> ListByIds(Expression<Func<T, bool>> expression, bool trackChanges = false)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
		return query.Where(expression).ToList();
	}

	/// <summary>
	/// s entities matching the expression asynchronously
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of entities</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public async Task<IEnumerable<T>> ListByIdsAsync(Expression<Func<T, bool>> expression, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
		return await query.Where(expression).ToListAsync(cancellationToken);
	}

	/// <summary>
	/// s all entities with optional ordering (synchronous)
	/// </summary>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Collection of entities</returns>
	public IEnumerable<T> List(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
	{
		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		return query.ToList();
	}

	/// <summary>
	/// s all entities with optional ordering asynchronously
	/// </summary>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of entities</returns>
	public async Task<IEnumerable<T>> ListAsync(Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		return await query.ToListAsync(cancellationToken);
	}

	/// <summary>
	/// s entities matching condition with optional ordering (synchronous)
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Collection of entities</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public IEnumerable<T> ListByCondition(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
		query = query.Where(expression);

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		return query.ToList();
	}

	/// <summary>
	/// s entities matching condition with optional ordering asynchronously
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="descending">Whether to sort descending</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of entities</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public async Task<IEnumerable<T>> ListByConditionAsync(Expression<Func<T, bool>> expression, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, bool descending = false, CancellationToken cancellationToken = default)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();
		query = query.Where(expression);

		if (orderBy != null)
		{
			query = descending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
		}

		return await query.ToListAsync(cancellationToken);
	}

	#endregion

	#region Projection Operations

	/// <summary>
	/// s entities with projection and optional ordering (synchronous)
	/// </summary>
	/// <typeparam name="TResult">Result type</typeparam>
	/// <param name="selector">Projection selector</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Collection of projected results</returns>
	/// <exception cref="ArgumentNullException">Thrown when selector is null</exception>
	public IEnumerable<TResult> ListWithSelect<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
	{
		if (selector == null)
			throw new ArgumentNullException(nameof(selector));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		return query.Select(selector).ToList();
	}

	/// <summary>
	/// s entities with projection and optional ordering asynchronously
	/// </summary>
	/// <typeparam name="TResult">Result type</typeparam>
	/// <param name="selector">Projection selector</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of projected results</returns>
	/// <exception cref="ArgumentNullException">Thrown when selector is null</exception>
	public async Task<IEnumerable<TResult>> ListWithSelectAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		if (selector == null)
			throw new ArgumentNullException(nameof(selector));

		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		return await query.Select(selector).ToListAsync(cancellationToken);
	}

	/// <summary>
	/// s entities with filter, projection, and optional ordering (synchronous)
	/// </summary>
	/// <typeparam name="TResult">Result type</typeparam>
	/// <param name="expression">Filter expression</param>
	/// <param name="selector">Projection selector</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <returns>Collection of projected results</returns>
	public IEnumerable<TResult> ListByWhereWithSelect<TResult>(Expression<Func<T, bool>>? expression = null, Expression<Func<T, TResult>>? selector = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false)
	{
		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (expression != null)
		{
			query = query.Where(expression);
		}

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		if (selector != null)
		{
			return query.Select(selector).ToList();
		}
		else
		{
			if (typeof(TResult) != typeof(T))
			{
				throw new InvalidOperationException($"Selector is null but TResult ({typeof(TResult).Name}) is not same as T ({typeof(T).Name}).");
			}

			return query.Cast<TResult>().ToList();
		}
	}

	/// <summary>
	/// s entities with filter, projection, and optional ordering asynchronously
	/// </summary>
	/// <typeparam name="TResult">Result type</typeparam>
	/// <param name="selector">Projection selector</param>
	/// <param name="expression">Filter expression</param>
	/// <param name="orderBy">Order by expression</param>
	/// <param name="trackChanges">Whether to track changes</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of projected results</returns>
	public async Task<IEnumerable<TResult>> ListByWhereWithSelectAsync<TResult>(Expression<Func<T, TResult>>? selector = null, Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>? orderBy = null, bool trackChanges = false, CancellationToken cancellationToken = default)
	{
		IQueryable<T> query = trackChanges ? _dbSet : _dbSet.AsNoTracking();

		if (expression != null)
		{
			query = query.Where(expression);
		}

		if (orderBy != null)
		{
			query = query.OrderBy(orderBy);
		}

		if (selector != null)
		{
			return await query.Select(selector).ToListAsync(cancellationToken);
		}
		else
		{
			if (typeof(TResult) != typeof(T))
			{
				throw new InvalidOperationException($"Selector is null but TResult ({typeof(TResult).Name}) is not same as T ({typeof(T).Name}).");
			}

			return (IEnumerable<TResult>)await query.Cast<T>().ToListAsync(cancellationToken);
		}
	}

	#endregion

	#region Count and Exists Operations

	/// <summary>
	/// s the count of all entities (synchronous)
	/// </summary>
	/// <returns>Entity count</returns>
	public int Count()
	{
		return _dbSet.AsNoTracking().Count();
	}

	/// <summary>
	/// s the count of entities matching the expression (synchronous)
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <returns>Entity count</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public int Count(Expression<Func<T, bool>> expression)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		return _dbSet.AsNoTracking().Count(expression);
	}

	/// <summary>
	/// s the count of all entities asynchronously
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Entity count</returns>
	public async Task<int> CountAsync(CancellationToken cancellationToken = default)
	{
		return await _dbSet.AsNoTracking().CountAsync(cancellationToken);
	}

	/// <summary>
	/// s the count of entities matching the expression asynchronously
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Entity count</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public async Task<int> CountAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		return await _dbSet.AsNoTracking().CountAsync(expression, cancellationToken);
	}

	/// <summary>
	/// Checks if any entity matching the expression exists (synchronous)
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <returns>True if exists, otherwise false</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public bool Exists(Expression<Func<T, bool>> expression)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		return _dbSet.AsNoTracking().Any(expression);
	}

	/// <summary>
	/// Checks if any entity matching the expression exists asynchronously
	/// </summary>
	/// <param name="expression">Filter expression</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>True if exists, otherwise false</returns>
	/// <exception cref="ArgumentNullException">Thrown when expression is null</exception>
	public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
	{
		if (expression == null)
			throw new ArgumentNullException(nameof(expression));

		return await _dbSet.AsNoTracking().AnyAsync(expression, cancellationToken);
	}

	#endregion

	#region Transaction Management

	/// <summary>
	/// Begins a new database transaction (synchronous)
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when a transaction is already in progress</exception>
	public void TransactionBegin()
	{
		if (_currentTransaction != null)
			throw new InvalidOperationException("A transaction is already in progress. Commit or rollback the current transaction before starting a new one.");

		_currentTransaction = _context.Database.BeginTransaction();
	}

	/// <summary>
	/// Begins a new database transaction asynchronously
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="InvalidOperationException">Thrown when a transaction is already in progress</exception>
	public async Task TransactionBeginAsync(CancellationToken cancellationToken = default)
	{
		if (_currentTransaction != null)
			throw new InvalidOperationException("A transaction is already in progress. Commit or rollback the current transaction before starting a new one.");

		_currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
	}

	/// <summary>
	/// Commits the current transaction (synchronous)
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when no active transaction exists</exception>
	public void TransactionCommit()
	{
		if (_currentTransaction == null)
			throw new InvalidOperationException("No active transaction to commit. Call TransactionBegin() first.");

		try
		{
			_context.SaveChanges();
			_currentTransaction.Commit();
		}
		catch
		{
			TransactionRollback();
			throw;
		}
		finally
		{
			_currentTransaction?.Dispose();
			_currentTransaction = null;
		}
	}

	/// <summary>
	/// Commits the current transaction asynchronously
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="InvalidOperationException">Thrown when no active transaction exists</exception>
	public async Task TransactionCommitAsync(CancellationToken cancellationToken = default)
	{
		if (_currentTransaction == null)
			throw new InvalidOperationException("No active transaction to commit. Call TransactionBeginAsync() first.");

		try
		{
			await _context.SaveChangesAsync(cancellationToken);
			await _currentTransaction.CommitAsync(cancellationToken);
		}
		catch
		{
			await TransactionRollbackAsync(cancellationToken);
			throw;
		}
		finally
		{
			if (_currentTransaction != null)
			{
				await _currentTransaction.DisposeAsync();
				_currentTransaction = null;
			}
		}
	}

	/// <summary>
	/// Rolls back the current transaction (synchronous)
	/// </summary>
	/// <exception cref="InvalidOperationException">Thrown when no active transaction exists</exception>
	public void TransactionRollback()
	{
		if (_currentTransaction == null)
			throw new InvalidOperationException("No active transaction to rollback.");

		try
		{
			_currentTransaction.Rollback();
		}
		finally
		{
			_currentTransaction?.Dispose();
			_currentTransaction = null;
		}
	}

	/// <summary>
	/// Rolls back the current transaction asynchronously
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <exception cref="InvalidOperationException">Thrown when no active transaction exists</exception>
	public async Task TransactionRollbackAsync(CancellationToken cancellationToken = default)
	{
		if (_currentTransaction == null)
			throw new InvalidOperationException("No active transaction to rollback.");

		try
		{
			await _currentTransaction.RollbackAsync(cancellationToken);
		}
		finally
		{
			if (_currentTransaction != null)
			{
				await _currentTransaction.DisposeAsync();
				_currentTransaction = null;
			}
		}
	}

	/// <summary>
	/// Disposes the current transaction if exists asynchronously
	/// </summary>
	public async Task TransactionDisposeAsync()
	{
		if (_currentTransaction != null)
		{
			try
			{
				await _currentTransaction.RollbackAsync();
			}
			catch
			{
				// Ignore errors during dispose
			}
			finally
			{
				await _currentTransaction.DisposeAsync();
				_currentTransaction = null;
			}
		}
	}

	#endregion

	#region Change Tracker Management

	/// <summary>
	/// Clears all tracked entities from the ChangeTracker
	/// </summary>
	public void ClearChangeTracker()
	{
		_context.ChangeTracker.Clear();
	}

	/// <summary>
	/// Clears all tracked entities from the ChangeTracker asynchronously
	/// </summary>
	public Task ClearChangeTrackerAsync()
	{
		_context.ChangeTracker.Clear();
		return Task.CompletedTask;
	}

	#endregion

	#region Raw SQL Execution EF Core

	// =================================================================
	// EXECUTE NON QUERY (INSERT/UPDATE/DELETE)
	// =================================================================
	/// <summary>
	/// Executes a raw SQL command (synchronous)
	/// </summary>
	/// <param name="query">SQL query to execute</param>
	/// <returns>"Success" if successful, error message otherwise</returns>
	/// <exception cref="ArgumentException">Thrown when query is null or whitespace</exception>
	public string EfCoreExecuteNonQuery(string query, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		try
		{
			_context.Database.SetCommandTimeout(180);
			_context.Database.ExecuteSqlRaw(query, cancellationToken);
			return "Success";
		}
		catch (Exception ex)
		{
			return $"Error: {ex.Message}";
		}
	}

	/// <summary>
	/// Executes a raw SQL command asynchronously
	/// </summary>
	/// <param name="query">SQL query to execute</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>"Success" if successful, error message otherwise</returns>
	/// <exception cref="ArgumentException">Thrown when query is null or whitespace</exception>
	/// <summary>
	/// Executes a raw SQL command (synchronous)
	/// </summary>
	public async Task<string> EfCoreExecuteNonQueryAsync(string query, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		try
		{
			_context.Database.SetCommandTimeout(180);
			await _context.Database.ExecuteSqlRawAsync(query, cancellationToken);
			return "Success";
		}
		catch (Exception ex)
		{
			return $"Error: {ex.Message}";
		}
	}



	// =================================================================
	// EXECUTE Raw query To Single Entity EF Core Methods
	// =================================================================
	/// <summary>
	/// Executes a raw SQL query and returns a single entity (Synchronous)
	/// </summary>
	/// <param name="query">SQL query to execute</param>
	/// <returns>Single entity or default instance if not found</returns>
	/// <exception cref="ArgumentException">Thrown when query is null or whitespace</exception>
	public T? EfCoreExecuteSingleQuery(string query)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		try
		{
			// Using FirstOrDefault (Sync)
			var result = _dbSet.FromSqlRaw(query).FirstOrDefault();

			// If result is null, create a new instance of T
			return result ?? Activator.CreateInstance<T>();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing SQL query: {ex.Message}", ex);
		}
	}

	/// <summary>
	/// Executes a raw SQL query and returns a single entity
	/// </summary>
	/// <param name="query">SQL query to execute</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Single entity or default instance if not found</returns>
	/// <exception cref="ArgumentException">Thrown when query is null or whitespace</exception>
	public async Task<T?> EfCoreExecuteSingleQueryAsync(string query, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		try
		{
			var result = await _dbSet.FromSqlRaw(query).FirstOrDefaultAsync(cancellationToken);
			return result ?? Activator.CreateInstance<T>();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing SQL query: {ex.Message}", ex);
		}
	}

	// =================================================================
	// EXECUTE Raw Query To List of Entity EF Core Methods
	// =================================================================
	/// <summary>
	/// Executes a raw SQL query and returns a list of entities (synchronous)
	/// </summary>
	public IEnumerable? EFCoreExecuteListQuery(string query)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		try
		{
			return _dbSet.FromSqlRaw(query).ToList();
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing SQL query: {ex.Message}", ex);
		}
	}

	/// <summary>
	/// Executes a raw SQL query and returns a list of entities (asynchronous)
	/// </summary>
	public async Task<IEnumerable?> EFCoreExecuteListQueryAsync(string query, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		try
		{
			return await _dbSet.FromSqlRaw(query).ToListAsync(cancellationToken);
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing SQL query: {ex.Message}", ex);
		}
	}
	#endregion

	#region ADO.NET Query Execution
	/// <summary>
	/// Executes a SQL query and returns results as a DataTable
	/// </summary>
	/// <param name="sqlQuery">SQL query to execute</param>
	/// <param name="parameters">Query parameters</param>
	/// <returns>DataTable with results</returns>
	/// <exception cref="ArgumentException">Thrown when sqlQuery is null or whitespace</exception>
	public DataTable AdoDataTable(string sqlQuery, params DbParameter[] parameters)
	{
		if (string.IsNullOrWhiteSpace(sqlQuery))
			throw new ArgumentException("SQL query cannot be null or empty.", nameof(sqlQuery));

		try
		{
			var dataTable = new DataTable();
			var connection = _context.Database.GetDbConnection();
			var dbFactory = DbProviderFactories.GetFactory(connection);

			using var cmd = dbFactory.CreateCommand();
			if (cmd == null)
				throw new InvalidOperationException("Failed to create database command.");

			cmd.Connection = connection;
			cmd.CommandType = CommandType.Text;
			cmd.CommandText = sqlQuery;

			if (parameters != null && parameters.Length > 0)
			{
				cmd.Parameters.AddRange(parameters);
			}

			using var adapter = dbFactory.CreateDataAdapter();
			if (adapter == null)
				throw new InvalidOperationException("Failed to create data adapter.");

			adapter.SelectCommand = cmd;
			adapter.Fill(dataTable);

			return dataTable;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing DataTable query: {ex.Message}", ex);
		}
	}

	/// <summary>
	/// Executes a query and returns grid data with pagination
	/// </summary>
	/// <typeparam name="TGrid">Grid entity type</typeparam>
	/// <param name="query">SQL query</param>
	/// <param name="options">Grid options for pagination</param>
	/// <param name="orderBy">Order by clause</param>
	/// <param name="condition">Additional condition</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Grid entity with items and total count</returns>
	//public async Task<GridEntity<TGrid>> GridDataAsync<TGrid>(string query, GridOptions options, string orderBy, string condition, CancellationToken cancellationToken = default)
	//{
	//  if (string.IsNullOrWhiteSpace(query))
	//    throw new ArgumentException("Query cannot be null or empty.", nameof(query));

	//  var connection = _context.Database.DbConnection();
	//  var sqlCount = $"SELECT COUNT(*) FROM ({query}) As tbl";
	//  query = GridDataSource<TGrid>.DataSourceQuery(options, query, orderBy, condition ?? "");

	//  var dataList = new List<TGrid>();
	//  int totalCount = 0;

	//  try
	//  {
	//    await connection.OpenAsync(cancellationToken);

	//    //  total count
	//    using (var countCommand = connection.CreateCommand())
	//    {
	//      countCommand.CommandText = sqlCount;
	//      var countResult = await countCommand.ExecuteScalarAsync(cancellationToken);
	//      totalCount = Convert.ToInt32(countResult);
	//    }

	//    //  data
	//    using (var command = connection.CreateCommand())
	//    {
	//      command.CommandText = query;
	//      using (var reader = await command.ExecuteReaderAsync(cancellationToken))
	//      {
	//        if (!reader.HasRows)
	//          return new GridEntity<TGrid> { Items = dataList, TotalCount = 0 };

	//        var columnMap = CreateColumnMap(reader);
	//        var properties = typeof(TGrid).Properties(BindingFlags.Public | BindingFlags.Instance);

	//        while (await reader.ReadAsync(cancellationToken))
	//        {
	//          var entity = Activator.CreateInstance<TGrid>();
	//          MapReaderToEntity(reader, entity, properties, columnMap);
	//          dataList.Add(entity);
	//        }
	//      }
	//    }
	//  }
	//  catch (Exception ex)
	//  {
	//    throw new InvalidOperationException($"Error in GridData execution: {ex.Message}", ex);
	//  }
	//  finally
	//  {
	//    if (connection.State == ConnectionState.Open)
	//    {
	//      await connection.CloseAsync();
	//    }
	//  }

	//  return new GridEntity<TGrid>
	//  {
	//    Items = dataList,
	//    TotalCount = totalCount,
	//    Columnses = new List<GridColumns>()
	//  };
	//}
	// =================================================================
	// GRID DATA METHODS
	// =================================================================

	/// <summary>
	/// Executes a query and returns grid data with pagination (Synchronous)
	/// </summary>
	public GridEntity<TGrid> AdoGridData<TGrid>(string query, GridOptions options, string orderBy, string condition)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		var connection = _context.Database.GetDbConnection();
		var sqlCount = $"SELECT COUNT(*) FROM ({query}) As tbl";
		query = GridDataSource<TGrid>.DataSourceQuery(options, query, orderBy, condition ?? "");

		var dataList = new List<TGrid>();
		int totalCount = 0;

		try
		{
			connection.Open(); // Sync

			//  total count
			using (var countCommand = connection.CreateCommand())
			{
				countCommand.CommandText = sqlCount;
				var countResult = countCommand.ExecuteScalar(); // Sync
				totalCount = Convert.ToInt32(countResult);
			}

			//  data
			using (var command = connection.CreateCommand())
			{
				command.CommandText = query;
				using (var reader = command.ExecuteReader()) // Sync
				{
					if (!reader.HasRows)
						return new GridEntity<TGrid> { Items = dataList, TotalCount = 0 };

					var columnMap = CreateColumnMap(reader);
					var properties = typeof(TGrid).GetProperties(BindingFlags.Public | BindingFlags.Instance);

					while (reader.Read()) // Sync
					{
						var entity = Activator.CreateInstance<TGrid>();
						MapReaderToEntity(reader, entity, properties, columnMap);
						dataList.Add(entity);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error in GridData execution: {ex.Message}", ex);
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
			{
				connection.Close(); // Sync
			}
		}

		return new GridEntity<TGrid>
		{
			Items = dataList,
			TotalCount = totalCount,
			Columnses = new List<GridColumns>()
		};
	}

	/// <summary>
	/// Executes a query and returns grid data with pagination (Asynchronous)
	/// </summary>
	public async Task<GridEntity<TGrid>> AdoGridDataAsync<TGrid>(string query, GridOptions options, string orderBy, string condition, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		var connection = _context.Database.GetDbConnection();
		var sqlCount = $"SELECT COUNT(*) FROM ({query}) As tbl";
		query = GridDataSource<TGrid>.DataSourceQuery(options, query, orderBy, condition ?? "");

		var dataList = new List<TGrid>();
		int totalCount = 0;

		try
		{
			await connection.OpenAsync(cancellationToken);

			//  total count
			using (var countCommand = connection.CreateCommand())
			{
				countCommand.CommandText = sqlCount;
				var countResult = await countCommand.ExecuteScalarAsync(cancellationToken);
				totalCount = Convert.ToInt32(countResult);
			}

			//  data
			using (var command = connection.CreateCommand())
			{
				command.CommandText = query;
				using (var reader = await command.ExecuteReaderAsync(cancellationToken))
				{
					if (!reader.HasRows)
						return new GridEntity<TGrid> { Items = dataList, TotalCount = 0 };

					var columnMap = CreateColumnMap(reader);
					var properties = typeof(TGrid).GetProperties(BindingFlags.Public | BindingFlags.Instance);

					while (await reader.ReadAsync(cancellationToken))
					{
						var entity = Activator.CreateInstance<TGrid>();
						MapReaderToEntity(reader, entity, properties, columnMap);
						dataList.Add(entity);
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error in GridData execution: {ex.Message}", ex);
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
			{
				await connection.CloseAsync();
			}
		}

		return new GridEntity<TGrid>
		{
			Items = dataList,
			TotalCount = totalCount,
			Columnses = new List<GridColumns>()
		};
	}



	/// <summary>
	/// Executes a query and returns a single result
	/// </summary>
	/// <typeparam name="TResult">Result type</typeparam>
	/// <param name="query">SQL query</param>
	/// <param name="parameters">Query parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Single result or null</returns>
	//public async Task<TResult?> ExecuteSingleDataAsync<TResult>(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) where TResult : class, new()
	//{
	//  if (string.IsNullOrWhiteSpace(query))
	//    throw new ArgumentException("Query cannot be null or empty.", nameof(query));

	//  var connection = _context.Database.DbConnection();
	//  TResult? result = null;

	//  try
	//  {
	//    await connection.OpenAsync(cancellationToken);

	//    using var command = connection.CreateCommand();
	//    command.CommandText = query;
	//    command.CommandTimeout = 120;

	//    if (parameters != null && parameters.Length > 0)
	//    {
	//      AddParameters(command, parameters);
	//    }

	//    using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);

	//    if (await reader.ReadAsync(cancellationToken))
	//    {
	//      var propertyMap = CreatePropertyMap<TResult>(reader);
	//      result = MapReaderToObject<TResult>(reader, propertyMap);
	//    }
	//  }
	//  catch (Exception ex)
	//  {
	//    throw new InvalidOperationException($"Error executing single data query: {ex.Message}", ex);
	//  }
	//  finally
	//  {
	//    if (connection.State == ConnectionState.Open)
	//      await connection.CloseAsync();
	//  }

	//  return result;
	//}
	// =================================================================
	// SINGLE DATA METHODS
	// =================================================================
	/// <summary>
	/// Executes a query and returns a single result (Synchronous)
	/// </summary>
	public TResult? AdoExecuteSingleData<TResult>(string query, SqlParameter[]? parameters = null) where TResult : class, new()
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		var connection = _context.Database.GetDbConnection();
		TResult? result = null;

		try
		{
			connection.Open(); // Sync

			using var command = connection.CreateCommand();
			command.CommandText = query;
			command.CommandTimeout = 120;

			if (parameters != null && parameters.Length > 0)
			{
				AddParameters(command, parameters);
			}

			using var reader = command.ExecuteReader(CommandBehavior.SingleRow); // Sync

			if (reader.Read()) // Sync
			{
				var columnMap = CreateColumnMap(reader);
				var properties = typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);

				result = Activator.CreateInstance<TResult>();
				MapReaderToEntity(reader, result, properties, columnMap);
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing single data query: {ex.Message}", ex);
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
				connection.Close(); // Sync
		}

		return result;
	}

	/// <summary>
	/// Executes a query and returns a single result (Asynchronous)
	/// </summary>
	public async Task<TResult?> AdoExecuteSingleDataAsync<TResult>(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) where TResult : class, new()
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		var connection = _context.Database.GetDbConnection();
		TResult? result = null;

		try
		{
			await connection.OpenAsync(cancellationToken);

			using var command = connection.CreateCommand();
			command.CommandText = query;
			command.CommandTimeout = 120;

			if (parameters != null && parameters.Length > 0)
			{
				AddParameters(command, parameters);
			}

			using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);

			if (await reader.ReadAsync(cancellationToken))
			{
				var propertyMap = CreatePropertyMap<TResult>(reader);
				result = MapReaderToObject<TResult>(reader, propertyMap);
			}
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing single data query: {ex.Message}", ex);
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
				await connection.CloseAsync();
		}

		return result;
	}

	/// <summary>
	/// Executes a query and returns a list of results
	/// </summary>
	/// <typeparam name="TResult">Result type</typeparam>
	/// <param name="query">SQL query</param>
	/// <param name="parameters">Query parameters</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of results</returns>
	// =================================================================
	// LIST QUERY METHODS AD0.NET
	// =================================================================  
	/// <summary>
	/// Executes a query and returns a list of results (Synchronous)
	/// </summary>
	public IEnumerable<TResult> AdoExecuteListQuery<TResult>(string query, SqlParameter[]? parameters = null) where TResult : class, new()
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		var connection = _context.Database.GetDbConnection();
		var results = new List<TResult>();

		try
		{
			connection.Open(); // Sync

			using var command = connection.CreateCommand();
			command.CommandText = query;
			command.CommandTimeout = 120;

			if (parameters != null && parameters.Length > 0)
			{
				AddParameters(command, parameters);
			}

			using var reader = command.ExecuteReader(CommandBehavior.SequentialAccess); // Sync

			var propertyMap = CreatePropertyMap<TResult>(reader);

			while (reader.Read()) // Sync
			{
				results.Add(MapReaderToObject<TResult>(reader, propertyMap));
			}

			return results;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing list query: {ex.Message}", ex);
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
				connection.Close(); // Sync
		}
	}

	/// <summary>
	/// Executes a query and returns a list of results (Asynchronous)
	/// </summary>
	public async Task<IEnumerable<TResult>> AdoExecuteListQueryAsync<TResult>(string query, SqlParameter[]? parameters = null, CancellationToken cancellationToken = default) where TResult : class, new()
	{
		if (string.IsNullOrWhiteSpace(query))
			throw new ArgumentException("Query cannot be null or empty.", nameof(query));

		var connection = _context.Database.GetDbConnection();
		var results = new List<TResult>();

		try
		{
			await connection.OpenAsync(cancellationToken);

			using var command = connection.CreateCommand();
			command.CommandText = query;
			command.CommandTimeout = 120;

			if (parameters != null && parameters.Length > 0)
			{
				AddParameters(command, parameters);
			}

			using var reader = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancellationToken);

			var propertyMap = CreatePropertyMap<TResult>(reader);

			while (await reader.ReadAsync(cancellationToken))
			{
				results.Add(MapReaderToObject<TResult>(reader, propertyMap));
			}

			return results;
		}
		catch (Exception ex)
		{
			throw new InvalidOperationException($"Error executing list query: {ex.Message}", ex);
		}
		finally
		{
			if (connection.State == ConnectionState.Open)
				await connection.CloseAsync();
		}
	}
	#endregion

	#region Helper Methods

	/// <summary>
	/// Validates that entities collection is not null or empty
	/// </summary>
	private void ValidateEntities(IEnumerable<T> entities, string paramName)
	{
		if (entities == null || !entities.Any())
			throw new ArgumentNullException(paramName, "Entities list cannot be null or empty.");
	}

	/// <summary>
	/// s the primary key value from an entity
	/// </summary>
	private int PrimaryKeyValue(T entity)
	{
		var entityType = _context.Model.FindEntityType(typeof(T));
		if (entityType == null)
			throw new InvalidOperationException($"Entity type {typeof(T).Name} not found in context model.");

		var primaryKey = entityType.FindPrimaryKey();
		if (primaryKey == null)
			throw new InvalidOperationException($"Entity type {typeof(T).Name} has no primary key defined.");

		var keyProperty = primaryKey.Properties[0];
		var value = keyProperty.GetGetter().GetClrValue(entity);

		if (value == null)
			throw new InvalidOperationException($"Primary key value is null for entity {typeof(T).Name}.");

		return (int)value;
	}

	/// <summary>
	/// Creates a case-insensitive column mapping from data reader
	/// </summary>
	private Dictionary<string, int> CreateColumnMap(DbDataReader reader)
	{
		var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		for (int i = 0; i < reader.FieldCount; i++)
		{
			var columnName = reader.GetName(i);
			if (columnMap.ContainsKey(columnName))
			{
				throw new InvalidOperationException($"Duplicate column name detected: {columnName}");
			}
			columnMap[columnName] = i;
		}

		return columnMap;
	}

	/// <summary>
	/// Creates a property map for efficient mapping
	/// </summary>
	private Dictionary<string, PropertyInfo> CreatePropertyMap<TResult>(DbDataReader reader)
	{
		var properties = typeof(TResult).GetProperties(BindingFlags.Public | BindingFlags.Instance);
		var propertyMap = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

		for (int i = 0; i < reader.FieldCount; i++)
		{
			string columnName = reader.GetName(i);
			var property = properties.FirstOrDefault(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
			if (property != null && property.CanWrite)
			{
				propertyMap[columnName] = property;
			}
		}

		return propertyMap;
	}

	/// <summary>
	/// Maps data reader to entity
	/// </summary>
	private void MapReaderToEntity<TEntity>(DbDataReader reader, TEntity entity, PropertyInfo[] properties, Dictionary<string, int> columnMap)
	{
		foreach (var property in properties)
		{
			if (!property.CanWrite)
				continue;

			if (columnMap.TryGetValue(property.Name, out int columnIndex))
			{
				if (!reader.IsDBNull(columnIndex))
				{
					try
					{
						var value = reader.GetValue(columnIndex);
						var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

						if (propertyType == typeof(Guid) && value is string guidString)
						{
							property.SetValue(entity, Guid.Parse(guidString));
						}
						else if (propertyType.IsEnum && value is string enumString)
						{
							property.SetValue(entity, Enum.Parse(propertyType, enumString));
						}
						else if (propertyType == typeof(DateOnly) && value is DateTime dateTime)
						{
							property.SetValue(entity, DateOnly.FromDateTime(dateTime));
						}
						else
						{
							property.SetValue(entity, Convert.ChangeType(value, propertyType));
						}
					}
					catch (Exception ex)
					{
						var columnName = reader.GetName(columnIndex);
						throw new DataMappingException(
								$"Failed to map column '{columnName}' to property '{property.Name}' on entity '{typeof(TEntity).Name}'",
								columnName,
								property.Name,
								property.PropertyType.Name,
								typeof(TEntity).Name,
								reader.GetValue(columnIndex),
								ex);
					}
				}
			}
		}
	}

	/// <summary>
	/// Maps data reader to object using property map
	/// </summary>
	private TResult MapReaderToObject<TResult>(DbDataReader reader, Dictionary<string, PropertyInfo> propertyMap) where TResult : class, new()
	{
		var obj = new TResult();

		foreach (var entry in propertyMap)
		{
			string columnName = entry.Key;
			PropertyInfo property = entry.Value;

			var ordinal = reader.GetOrdinal(columnName);

			if (!reader.IsDBNull(ordinal))
			{
				try
				{
					object value = reader[columnName];
					var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

					if (propertyType == typeof(DateOnly) && value is DateTime dateTime)
					{
						property.SetValue(obj, DateOnly.FromDateTime(dateTime));
					}
					else if (propertyType == typeof(Guid) && value is string guidString)
					{
						property.SetValue(obj, Guid.Parse(guidString));
					}
					else if (propertyType.IsEnum && value is string enumString)
					{
						property.SetValue(obj, Enum.Parse(propertyType, enumString));
					}
					else
					{
						property.SetValue(obj, Convert.ChangeType(value, propertyType));
					}
				}
				catch (Exception ex)
				{
					throw new InvalidOperationException(
							$"Error mapping column '{columnName}' to property '{property.Name}': {ex.Message}", ex);
				}
			}
		}

		return obj;
	}

	/// <summary>
	/// Adds SQL parameters to command
	/// </summary>
	private void AddParameters(DbCommand command, SqlParameter[] parameters)
	{
		foreach (var param in parameters)
		{
			var dbParam = command.CreateParameter();
			dbParam.ParameterName = param.ParameterName;
			dbParam.Value = param.Value ?? DBNull.Value;
			command.Parameters.Add(dbParam);
		}
	}

	#endregion
}

/// <summary>
/// Extension methods for DbDataReader
/// </summary>
public static class DbDataReaderExtensions
{
	/// <summary>
	/// Checks if a column exists in the reader
	/// </summary>
	/// <param name="reader">The data reader</param>
	/// <param name="columnName">Column name to check</param>
	/// <returns>True if column exists, otherwise false</returns>
	public static bool HasColumn(this DbDataReader reader, string columnName)
	{
		for (int i = 0; i < reader.FieldCount; i++)
		{
			if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}




