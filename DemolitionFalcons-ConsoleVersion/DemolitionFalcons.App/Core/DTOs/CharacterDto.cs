namespace DemolitionFalcons.App.Core.DTOs
{
    using System;

    public class CharacterDto
    {
        private string name;
        private string label;
        private string description;
        private int health;
        private int armour;

        public CharacterDto()
        {
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (value.Length < 3 || value.Length > 12)
                {
                    throw new ArgumentException("Character's name must be between 3 and 12 symbols!");
                }

                this.name = value;
            }
        }

        public string Label
        {
            get => this.label;
            set
            {
                if (value.Length < 3 || value.Length > 50)
                {
                    throw new ArgumentException("Character's label must be between 3 and 12 symbols!");
                }

                this.label = value;
            }
        }

        public string Description
        {
            get => this.description;
            set
            {
                if (value.Length < 15 || value.Length > 200)
                {
                    throw new ArgumentException("Character's description must be between 15 and 200 symbols!");
                }

                this.description = value;
            }
        }

        public int Health
        {
            get => this.health;
            set
            {
                if (value < 70 || value > 150)
                {
                    throw new ArgumentException("Character's health must be at least 70 and not more than 150!");
                }
                this.health = value;
            }
        }

        public int Armour
        {
            get => this.armour;
            set
            {
                if (value < 50 || value > 100)
                {
                    throw new ArgumentException("Character's armour must be at least 50 and not more than 100!");
                }
                this.armour = value;
            }
        }
    }
}
