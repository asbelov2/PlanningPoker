using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
  /// <summary>
  /// <see cref="Repository{T}"/> abstract class.
  /// </summary>
  /// <typeparam name="T">Entity with ID</typeparam>
  public abstract class Repository<T> : IRepository<T>
    where T : class, IEntity
  {
    /// <summary>
    /// Collection of data.
    /// </summary>
    protected static ICollection<T> data = new List<T>();

    private bool disposed = false;

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Add(T item)
    {
      if (!data.Contains(item) && (data.FirstOrDefault(x => x.Id == item.Id) == null))
      {
        data.Add(item);
      }
    }

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Delete(T item)
    {
      if (item != null)
      {
        data.Remove(item);
      }
    }

    /// <summary>
    /// Dispose.
    /// </summary>
    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose for override.
    /// </summary>
    /// <param name="disposing"> Disposing.</param>
    public virtual void Dispose(bool disposing)
    {
      if (!this.disposed)
      {
        if (disposing)
        {
        }
      }

      this.disposed = true;
    }

    /// <summary>
    /// Returns item.
    /// </summary>
    /// <param name="id">Item's ID.</param>
    /// <returns>Item.</returns>
    public virtual T GetItem(string id)
    {
      return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Return collection of items.
    /// </summary>
    /// <returns>collection of items.</returns>
    public virtual IEnumerable<T> GetList()
    {
      return data;
    }

    /// <summary>
    /// Save item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Save(T item)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Update item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Update(T item)
    {
      throw new NotImplementedException();
    }

    /// <summary>
    /// Clears data.
    /// </summary>
    public virtual void ClearRepository()
    {
      data.Clear();
    }
  }
}