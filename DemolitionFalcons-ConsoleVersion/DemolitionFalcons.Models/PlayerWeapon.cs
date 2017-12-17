namespace DemolitionFalcons.Models
{
    public class PlayerWeapon
    {
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int WeaponId { get; set; }
        public Weapon Weapon { get; set; }
    }
}
