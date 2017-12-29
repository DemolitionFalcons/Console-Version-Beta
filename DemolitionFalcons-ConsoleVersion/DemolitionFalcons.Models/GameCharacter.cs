namespace DemolitionFalcons.Models
{
    public class GameCharacter
    {
        public GameCharacter()
        {
            CharacterPositionX = 0;
            CharacterPositionY = 0;
            MapSectionNumber = 0;
        }
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int? CharacterPositionX { get; set; }
        public int? CharacterPositionY { get; set; }

        public int? MapSectionNumber { get; set; }

        public int? PlayerId { get; set; }
    }
}
