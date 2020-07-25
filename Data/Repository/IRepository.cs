using System;
using System.Collections.Generic;

namespace Data
{
  /// <summary>
  /// Repository interface.
  /// </summary>
  /// <typeparam name="T">Entity with ID.</typeparam>
  internal interface IRepository<T> : IDisposable
    where T : class, IEntity
  {
    /// <summary>
    /// Return collection of items.
    /// </summary>
    /// <returns>collection of items.</returns>
    ICollection<T> GetList();

    /// <summary>
    /// Returns item.
    /// </summary>
    /// <param name="id">Item's ID.</param>
    /// <returns>Item.</returns>
    T GetItem(string id);

    /// <summary>
    /// Add item.
    /// </summary>
    /// <param name="item">Item.</param>
    void Add(T item);

    /// <summary>
    /// Update item.
    /// </summary>
    /// <param name="item">Item.</param>
    void Update(T item);

    /// <summary>
    /// Delete item.
    /// </summary>
    /// <param name="item">Item.</param>
    void Delete(T item);

    /// <summary>
    /// Save item.
    /// </summary>
    /// <param name="item">Item.</param>
    void Save(T item);
  }
}