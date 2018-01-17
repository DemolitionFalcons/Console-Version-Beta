namespace DemolitionFalcons.App.Miscellaneous
{
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AttackACharacter
    {
        private DemolitionFalconsDbContext context;
        private MapSection[][] map;
        private int roomId;
        private int characterId;
        public AttackACharacter(DemolitionFalconsDbContext context,MapSection[][] map, int roomId, int characterId)
        {
            this.context = context;
            this.map = map;
            this.roomId = roomId;
            this.characterId = characterId;
        }

        public void Atack()
        {
            if (!context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId ).Spells.Any())
            {
                throw new ArgumentException("You cannot make an atack due to lack of spells!");
            }

            var spells = context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId && ch.GameId == roomId).Spells.ToList();
            Console.WriteLine("You can use one of the following spells to atack...");
            var counter = 1;
            foreach (var spell in spells)
            {
                Console.WriteLine($"{counter}. {spell.Name}");
                Console.WriteLine($"Description: {spell.Description}");
                Console.WriteLine($"Damage: { spell.DamageBonus} + your weapon's dmg");
                Console.WriteLine($"Range: {spell.SpellRange}");
            }
            Console.WriteLine($"Please select the number of your desired spell:");
            var num = int.Parse(Console.ReadLine());
            while (num <= 0 && num > spells.Count)
            {
                Console.WriteLine("Invalid number! Please select a new one:");
                num = int.Parse(Console.ReadLine());
            }
            var chosenSpell = spells[num - 1];

            var chNum = context.GameCharacters.FirstOrDefault(c => c.CharacterId == characterId && c.GameId == roomId)
                    .MapSectionNumber;
            var range = chosenSpell.SpellRange;

            var availableCharactersToAtack = new List<Character>();           
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    
                }
            }
        }
    }
}
