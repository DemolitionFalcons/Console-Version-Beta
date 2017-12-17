namespace DemolitionFalcons.Models
{
    public class GameCharacter
    {
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }
    }
}
