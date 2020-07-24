using Data;

namespace RoomApi
{
  /// <summary>
  /// ChoiceDTO class
  /// </summary>
  public class ChoiceDTO
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ChoiceDTO"/> class.
    /// </summary>
    /// <param name="user">User.</param>
    /// <param name="card">Card.</param>
    public ChoiceDTO(UserDTO user, CardDTO card)
    {
      this.User = user;
      this.Card = card;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ChoiceDTO"/> class.
    /// </summary>
    /// <param name="choice">Choice.</param>
    public ChoiceDTO(Choice choice)
    {
      this.User = new UserDTO(choice.User);
      this.Card = new CardDTO(choice.Card);
    }

    /// <summary>
    /// Gets user.
    /// </summary>
    public UserDTO User { get; }

    /// <summary>
    /// Gets card.
    /// </summary>
    public CardDTO Card { get; }
  }
}