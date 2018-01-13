namespace DemolitionFalcons.Service.Front
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class GameFront
    {
        public GameFront()
        {
            
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int? WinnerId { get; set; }

        public int Xp { get; set; }

        public decimal Money { get; set; }

        public TimeSpan? Time { get; set; }

        public ICollection<GameCharacter> Characters { get; set; }

        public int GameSize { get; set; }
    }
}