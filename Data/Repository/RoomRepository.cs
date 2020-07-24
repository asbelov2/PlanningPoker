namespace Data
{
  /// <summary>
  /// <see cref="RoomRepository"/> class.
  /// </summary>
  public class RoomRepository : Repository<Room>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="RoomRepository"/> class.
    /// </summary>
    public RoomRepository()
    {
    }

    /// <summary>
    /// Edit room.
    /// </summary>
    /// <param name="item">Room to edit.</param>
    /// <param name="cardInterpretation">New card interpretation.</param>
    public void Edit(Room item, string cardInterpretation)
    {
      if (item != null)
      {
        item.CardInterpretation = cardInterpretation;
      }
    }
  }
}