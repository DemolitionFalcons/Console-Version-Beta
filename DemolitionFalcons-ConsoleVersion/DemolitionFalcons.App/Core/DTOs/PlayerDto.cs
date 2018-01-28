namespace DemolitionFalcons.App.Core.DTOs
{
    using System;
    using System.Collections.Generic;

    public class PlayerDto
    {
        private string username;
        private string password;

        public PlayerDto()
        {
            //this.Weapons = new List<PlayerWeapon>();
            //this.Characters = new List<Character>();
            this.GamesPlayed = 0;
            this.Wins = 0;
        }
        //public int Id { get; set; }

        //public byte Picture { get; set; }

        //[MinLength(3)]
        public string Username
        {
            get => this.username;
            set
            {
                if (value.Length < 3 && value.Length > 30)
                {
                    throw new ArgumentException("Username MUST be between 3 and 30 characters");
                }
                this.username = value;
            }
        }

        //[MinLength(3)]
        public string Password
        {
            get => this.password;
            set
            {
                if (value.Length < 3 && value.Length > 30)
                {
                    throw new ArgumentException("Password MUST be between 3 and 30 characters");
                }
                this.password = value;
            }
        }
        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        //public ICollection<WeaponDto> Weapons { get; set; }

        //public ICollection<Character> Characters { get; set; }
    }
}
