namespace DemolitionFalcons.Models
{
    using System.Collections.Generic;

    public class GameCharacter
    {
        public GameCharacter()
        {
            CharacterPositionX = 0;
            CharacterPositionY = 0;
            MapSectionNumber = 0;
            this.SpellsCount = 0;
        }
        public int GameId { get; set; }
        public Game Game { get; set; }

        public int CharacterId { get; set; }
        public Character Character { get; set; }

        public int? CharacterPositionX { get; set; }
        public int? CharacterPositionY { get; set; }

        public int? MapSectionNumber { get; set; }

        public int? PlayerId { get; set; }

        public int? WeaponId { get; set; }

        public string Type { get; set; }

        public int Health { get; set; }

        public int Armour { get; set; }

        public int SpellsCount { get; set; }


        public IList<Spell> Spells { get; set; } = new List<Spell>();
    }
}
