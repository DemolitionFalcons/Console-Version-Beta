namespace DemolitionFalcons.App.Core.DTOs
{
    using System;

    public class GameDto
    {
        private string name;
        private string map;
        private int xp;
        private decimal money;
        private int capacity;

        public GameDto()
        {
        }

        public string Name
        {
            get => this.name;
            set
            {
                if (value.Length < 4 || value.Length > 30)
                {
                    throw new ArgumentException("Game name must be between 4 and 30 symbols!");
                }

                this.name = value;
            }
        }

        public string Map
        {
            get => this.map;
            set
            {
                this.map = value;
            }
        }

        public int Capacity
        {
            get => this.capacity;
            set
            {
                if (value < 2 || value > 6)
                {
                    throw new ArgumentException("Capacity should be between 2 and 6 players");
                }
                this.capacity = value;
            }
        }

        public int XP
        {
            get => this.xp;
            //ToDo -> Add restrictions
            set => this.xp = value;
        }

        public decimal Money
        {
            get => this.money;
            //ToDo -> Add restrictions
            set => this.money = value;
        }
    }
}

