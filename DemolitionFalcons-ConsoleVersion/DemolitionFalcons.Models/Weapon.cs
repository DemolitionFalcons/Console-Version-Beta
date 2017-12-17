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

        [Range(0,50)]
        public int ClipSize { get; set; }

        [Range(0,250)]
        public int TotalCapacity { get; set; }

        public ICollection<PlayerWeapon> Players { get; set; }
    }
}
