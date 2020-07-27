using Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RoomApi
{
  public class RoundTimerService
  {
    private static ICollection<RoundTimer> data = new List<RoundTimer>();


    /// <summary>
    /// Add timer.
    /// </summary>
    /// <param name="timer">Timer.</param>
    /// <returns>Id of timer.</returns>
    public Guid Add(RoundTimer timer)
    {
      if (data.Any(x => x.Id == timer.Id) == false)
      {
        data.Add(timer);
        return timer.Id;
      }

      return default;
    }

    /// <summary>
    /// Delete timer.
    /// </summary>
    /// <param name="timer">Timer.</param>
    public void Delete(RoundTimer timer)
    {
      if (timer != null)
      {
        data.Remove(timer);
      }
    }

    /// <summary>
    /// Returns timer.
    /// </summary>
    /// <param name="id">Timer's ID.</param>
    /// <returns>Timer.</returns>
    public RoundTimer GetTimer(Guid id)
    {
      return data.FirstOrDefault(x => x.Id == id);
    }

    /// <summary>
    /// Return collection of timers.
    /// </summary>
    /// <returns>Collection of timers.</returns>
    public IEnumerable<RoundTimer> GetList()
    {
      return data;
    }

  }
}
