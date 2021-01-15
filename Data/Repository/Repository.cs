using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="Repository{T}"/> abstract class. Used for storing entities.
  /// </summary>
  /// <typeparam name="T">Entity with ID</typeparam>
  public abstract class Repository<T> : IRepository<T>
    where T : class, IEntity
  {
    DbContext _context;
    DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
      _context = context;
      _dbSet = context.Set<T>();
    }

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <returns>Id of item.</returns>
    public virtual Guid Add(T item)
    {
      if (item != null)
      {
        if (!_dbSet.Any(x => x.Id == item.Id))
        {
          _dbSet.Add(item);
          _context.SaveChanges();
          return item.Id;
        }
      }
      return default;
    }

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Delete(T item)
    {
      if (item != null)
      {
        _dbSet.Remove(item);
        _context.SaveChanges();
      }
    }

    /// <summary>
    /// Returns item.
    /// </summary>
    /// <param name="id">Item's ID.</param>
    /// <returns>Item.</returns>
    public virtual T GetItem(Guid id)
    {
      return _dbSet.Find(id);
    }

    public virtual IEnumerable<T> GetItems(Func<T, bool> predicate)
    {
      return _dbSet.AsNoTracking().Where(predicate).ToList();
    }

    /// <summary>
    /// Return collection of items.
    /// </summary>
    /// <returns>collection of items.</returns>
    public virtual IEnumerable<T> GetList()
    {
      return _dbSet.AsNoTracking().ToList();
    }

    /// <summary>
    /// Save item. (outdated?)
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Save(T item)
    {
      this.Add(item);
      _context.SaveChanges();
    }

    /// <summary>
    /// Update item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Update(T item)
    {
      _context.Entry(item).State = EntityState.Modified;
      _context.SaveChanges();
    }

    /// <summary>
    /// Clears data.
    /// </summary>
    public virtual void ClearRepository()
    {
      /*_dbSet.RemoveRange(); (Outdated)
      Data.Clear();*/
    }
  }
}