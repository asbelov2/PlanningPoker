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
    /// <summary>
    /// Collection of data.
    /// </summary>
    protected static ICollection<T> Data { get; } = new List<T>();

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item.</param>
    /// <returns>Id of item.</returns>
    public virtual Guid Add(T item)
    {
      if (!Data.Any(x => x.Id == item?.Id))
      {
        Data.Add(item);
        return item.Id;
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
        Data.Remove(item);
      }
    }

    /// <summary>
    /// Returns item.
    /// </summary>
    /// <param name="id">Item's ID.</param>
    /// <returns>Item.</returns>
    public virtual T GetItem(Guid id)
    {
      return Data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Return collection of items.
    /// </summary>
    /// <returns>collection of items.</returns>
    public virtual IEnumerable<T> GetList()
    {
      return Data;
    }

    /// <summary>
    /// Save item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Save(T item)
    {
      this.Add(item);
    }

    /// <summary>
    /// Update item.
    /// </summary>
    /// <param name="item">Item.</param>
    public virtual void Update(T item)
    {
      this.Delete(this.GetItem(item.Id));
      this.Add(item);
    }

    /// <summary>
    /// Clears data.
    /// </summary>
    public virtual void ClearRepository()
    {
      Data.Clear();
    }
  }
}