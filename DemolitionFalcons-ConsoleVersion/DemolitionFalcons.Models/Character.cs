namespace DemolitionFalcons.Models
{
    using DemolitionFalcons.Models.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Character
    {
        public Character()
        {
            this.Games = new List<GameCharacter>();
            this.Hp = ModelConstants.CharacterDefaultHealth;
            this.Armour = ModelConstants.CharacterDefaultArmour;
        }

        public Character(string name, string label, string description, int hp, int armour)
        {
            this.Name = name;
            this.Label = label;
            this.Description = description;
            this.Hp = hp;
            this.Armour = armour;
        }

        public int Id { get; set; }

        [MinLength(3)]
        public string Name { get; set; }

        [MinLength(3)]
        public string Label { get; set; }

        [MinLength(15)]
        public string Description { get; set; }
        
        public int Hp { get; set; }

        public int Armour { get; set; }

        public ICollection<GameCharacter> Games { get; set; }
    }
}
