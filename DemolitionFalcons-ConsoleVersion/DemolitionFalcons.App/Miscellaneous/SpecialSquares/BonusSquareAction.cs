namespace DemolitionFalcons.App.Miscellaneous.SpecialSquares
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Models;

    public class BonusSquareAction
    {
        private DemolitionFalconsDbContext context;
        private int roomId;
        private Character character;

        public BonusSquareAction(DemolitionFalconsDbContext context, int roomId, Character character)
        {
            this.context = context;
            this.roomId = roomId;
            this.character = character;
        }

        public Spell RandomSpell()
        {
            var rnd = new Random();
            var num = rnd.Next(1, 11);
            var spell = new Spell();

            if (num == 1 || num == 2 || num == 4 || num == 5 || num == 8)
            {
                spell = context.Spells.FirstOrDefault(s => s.Name == "Fireball");
            }
            else if (num == 6 || num == 10)
            {
                spell = context.Spells.FirstOrDefault(s => s.Name == "Crucio");
            }
            else if (num == 7 || num == 9)
            {
                spell = context.Spells.FirstOrDefault(s => s.Name == "Expecto Patronum");
            }
            else
            {
                spell = context.Spells.FirstOrDefault(s => s.Name == "Avada Kedavra");
            }

            return spell;
        }

        public void GetSpell(string spellName)
        {
            var charId = character.Id;
            GameCharacter gameCharacter = context.GameCharacters.FirstOrDefault(gc => gc.CharacterId == charId && gc.GameId == roomId);

            var spell = new Spell();
            if (string.IsNullOrEmpty(spellName))
            {
                var rnd = new Random();
                var num = rnd.Next(1, 11);
                //Firebal -> 1,2,4,5,8
                //AvadaKedavra -> 3
                //Crucio -> 6, 10
                //ExpectoPatronum -> 7,9
                if (num == 1 || num == 2 || num == 4 || num == 5 || num == 8)
                {
                    spell = context.Spells.FirstOrDefault(s => s.Name == "Fireball");
                }
                else if (num == 6 || num == 10)
                {
                    spell = context.Spells.FirstOrDefault(s => s.Name == "Crucio");
                }
                else if (num == 7 || num == 9)
                {
                    spell = context.Spells.FirstOrDefault(s => s.Name == "Expecto Patronum");
                }
                else
                {
                    spell = context.Spells.FirstOrDefault(s => s.Name == "Avada Kedavra");
                }

                Console.WriteLine($"Congratulation, you discovered {spell.Name}. It allows you to provide an atack over anyone within the spell's range. After that your spell will disappear.");
                context.GameCharacters.FirstOrDefault(gc => gc.CharacterId == charId && gc.GameId == roomId).Spells.Add(spell);
                context.GameCharacters.FirstOrDefault(gc => gc.CharacterId == charId && gc.GameId == roomId).SpellsCount++;
                context.SaveChanges();

                return;
            }

            spell = context.Spells.FirstOrDefault(s => s.Name == spellName);

            Console.WriteLine($"Congratulation, you discovered {spell.Name}. It allows you to provide an atack over anyone within the spell's range. After that your spell will disappear.");
            context.GameCharacters.FirstOrDefault(gc => gc.CharacterId == charId && gc.GameId == roomId).Spells.Add(spell);
            context.GameCharacters.FirstOrDefault(gc => gc.CharacterId == charId && gc.GameId == roomId).SpellsCount++;
            context.SaveChanges();
        }
    }
}
