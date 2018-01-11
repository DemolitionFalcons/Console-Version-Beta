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

        //public string Type { get; set; }

        //public byte Picture { get; set; }

        [MinLength(3)]
        public string Username { get; set; }

        [MinLength(3)]
        public string Password { get; set; }

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public int Xp { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Money { get; set; }

        public ICollection<PlayerWeapon> Weapons { get; set; }

        //public ICollection<Character> Characters { get; set; }

        //public GameCharacter GameCharacter { get; set; }
    }
}
