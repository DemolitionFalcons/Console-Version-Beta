namespace DemolitionFalcons.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Spell
    {
        public Spell()
        {
        }
        public Spell(string name,int damageBonus, int spellRange)
            :this()
        {
            this.Name = name;
            this.DamageBonus = damageBonus;
            this.SpellRange = spellRange;
        }

        public int Id { get; set; }

        [MinLength(3)]
        public string Name { get; set; }

        public int DamageBonus {get; set;} // adds a bonus dmg to the dmg that the char already possesses

        public int SpellRange { get; set; }

        public string Description { get; set; }

    }
}
