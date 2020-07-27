using System;
using System.Timers;

namespace Data
{
  /// <summary>
  /// <see cref="RoundTimer"/> class. Used if round has round time.
  /// </summary>
  public class RoundTimer : IEntity
  {
    private TimeSpan roundTime;
    private Timer timer;
    private RoundResultRepository roundResults = new RoundResultRepository();
    private RoundRepository rounds = new RoundRepository();

    /// <summary>
    /// Initializes a new instance of the <see cref="RoundTimer"/> class.
    /// </summary>
    /// <param name="id">round ID</param>
    /// <param name="roundTime">round Time</param>
    public RoundTimer(Guid id, TimeSpan roundTime)
    {
      this.Id = id;
      this.roundTime = roundTime;
      this.IsEnabled = false;
    }

    /// <summary>
    /// Round timer ID.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Status of timer.
    /// </summary>
    public bool IsEnabled { get; private set; }

    /// <summary>
    /// Set the imer.
    /// </summary>
    public void SetTimer()
    {
      this.timer = new Timer(this.roundTime.TotalMilliseconds);
      this.timer.Elapsed += this.OnTimerTick;
      this.timer.AutoReset = false;
      this.timer.Enabled = true;
      this.IsEnabled = true;
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop()
    {
      this.timer.Stop();

      this.rounds.GetItem(this.Id).Duration = DateTime.Now - this.rounds.GetItem(this.Id).StartDate;
      this.roundResults.Add(new RoundResult(this.rounds.GetItem(this.Id)));
      this.IsEnabled = false;
    }

    /// <summary>
    /// Timer callback function.
    /// </summary>
    /// <param name="source">Caller.</param>
    /// <param name="e">Arguments.</param>
    private void OnTimerTick(object source, ElapsedEventArgs e)
    {
      this.Stop();
    }
  }
}