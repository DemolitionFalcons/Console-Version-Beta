namespace DemolitionFalcons.Models
{
    using DemolitionFalcons.Models.Utilities;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public Game()
        {
            this.Characters = new List<GameCharacter>();
            this.Xp = ModelConstants.DefaultGameXp;
            this.Money = ModelConstants.DefaultGameMoney;
            this.Capacity = ModelConstants.DefaultGameCapacity;
        }

        public Game(string name,string map,int capacity, int xp, decimal money)
        {
            this.Name = name;
            this.Map = map;
            this.Capacity = capacity;
            this.Xp = xp;
            this.Money = money;
        }

        public int Id { get; set; }

        [MinLength(4)]
        public string Name { get; set; }

        public int? WinnerId { get; set; }
        //public Player Winner { get; set; }

        public int Xp { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Money { get; set; }

        public TimeSpan? Time { get; set; }

        [Range(2,6)]
        public int Capacity { get; set; }

        public string Map { get; set; }

        //public int MapId { get; set; }
        //public Map Map { get; set; }

        public ICollection<GameCharacter> Characters { get; set; }

        //public int GameSize { get; set; }
    }
}
