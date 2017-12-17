namespace DemolitionFalcons.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Player
    {
        public Player()
        {
            this.Weapons = new List<PlayerWeapon>();
            //this.Characters = new List<Character>();
        }
        public int Id { get; set; }

        public byte Picture { get; set; }

        [MinLength(3)]
        public string Username { get; set; }

        [MinLength(3)]
        public string Password { get; set; }

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public ICollection<PlayerWeapon> Weapons { get; set; }

        //public ICollection<Character> Characters { get; set; }


    }
}
