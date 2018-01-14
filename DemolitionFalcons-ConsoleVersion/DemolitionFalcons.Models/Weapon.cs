namespace DemolitionFalcons.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Weapon
    {
        public Weapon()
        {
            this.Players = new List<PlayerWeapon>();
        }

        public int Id { get; set; }

        [MinLength(3)]
        public string Name { get; set; }
        
        public int Damage { get; set; }
        
        public string Core { get; set; }

        public ICollection<PlayerWeapon> Players { get; set; }
    }
}
