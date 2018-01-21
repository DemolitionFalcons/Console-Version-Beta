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

        public void SinglePlayerAttack(NumberGenerator numberGenerator)
        {
            if (!context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId).Spells.Any())
            {
                throw new ArgumentException("You cannot make an attack due to lack of spells!");
            }

            var spells = context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId && ch.GameId == roomId).Spells.ToList();
            Console.WriteLine("You can use one of the following spells to attack...");
            var counter = 1;
            foreach (var spell in spells)
            {
                Console.WriteLine($"{counter}. {spell.Name}");
                Console.WriteLine($"Description: {spell.Description}");
                Console.WriteLine($"Damage: {spell.DamageBonus} + your weapon's dmg");
                Console.WriteLine($"Range: {spell.SpellRange}");
                counter++;
            }
            Console.WriteLine($"Please select the number of your desired spell:");
            var num = numberGenerator.GenerateNumber(1, counter);
            var chosenSpell = spells[num - 1];

            var chNum = context.GameCharacters.FirstOrDefault(c => c.CharacterId == characterId && c.GameId == roomId)
                    .MapSectionNumber;
            var spellRange = chosenSpell.SpellRange;

            List<int> positions = PositionsInRange(chNum, spellRange);

            var availableCharactersToAttack = SeeAvailableCharacters(positions);
            if (availableCharactersToAttack.Count == 0)
            {
                throw new ArgumentException("Sorry, there ain't any characters in your range...");
            }
            Console.WriteLine($"You can attack the following characters:");
            int characterCounter = 1;
            foreach (var availableCharacter in availableCharactersToAttack)
            {
                var character = context.Characters.SingleOrDefault(c => c.Id == availableCharacter.CharacterId);
                var player = context.Players.SingleOrDefault(p => p.Id == availableCharacter.PlayerId);
                Console.WriteLine($"{characterCounter}.{character.Name}[{player.Username}] - currently has {availableCharacter.Health}hp and {availableCharacter.Armour}armour. Current possition - {context.GameCharacters.SingleOrDefault(gc => gc.CharacterId == character.Id && gc.GameId == roomId).MapSectionNumber}");
                characterCounter++;
            }
            Console.WriteLine($"Please select the number of the character you want to attack:");
            var charNum = numberGenerator.GenerateNumber(1, characterCounter);
            var chosenCharacter = availableCharactersToAttack[charNum - 1];
            AttackCharacter(chosenSpell, chosenCharacter);

        }

        public void Attack()
        {
            if (!context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId).Spells.Any())
            {
                throw new ArgumentException("You cannot make an attack due to lack of spells!");
            }

            var spells = context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId && ch.GameId == roomId).Spells.ToList();
            Console.WriteLine("You can use one of the following spells to attack...");
            var counter = 1;
            foreach (var spell in spells)
            {
                Console.WriteLine($"{counter}. {spell.Name}");
                Console.WriteLine($"Description: {spell.Description}");
                Console.WriteLine($"Damage: {spell.DamageBonus} + your weapon's dmg");
                Console.WriteLine($"Range: {spell.SpellRange}");
                counter++;
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
            var spellRange = chosenSpell.SpellRange;

            List<int> positions = PositionsInRange(chNum, spellRange);

            var availableCharactersToAttack = SeeAvailableCharacters(positions);
            if (availableCharactersToAttack.Count == 0)
            {
                throw new ArgumentException("Sorry, there ain't any characters in your range...");
            }
            Console.WriteLine($"You can attack the following characters:");
            int characterCounter = 1;
            foreach (var availableCharacter in availableCharactersToAttack)
            {
                var character = context.Characters.SingleOrDefault(c => c.Id == availableCharacter.CharacterId);
                var player = context.Players.SingleOrDefault(p => p.Id == availableCharacter.PlayerId);
                Console.WriteLine($"{characterCounter}.{character.Name}[{player.Username}] - currently has {availableCharacter.Health}hp and {availableCharacter.Armour}armour. Current possition - {context.GameCharacters.SingleOrDefault(gc => gc.CharacterId == character.Id && gc.GameId == roomId).MapSectionNumber}");
                characterCounter++;
            }
            Console.WriteLine($"Please select the number of the character you want to attack:");
            var charNum = int.Parse(Console.ReadLine());
            while (charNum <= 0 && charNum > spells.Count)
            {
                Console.WriteLine("Invalid number! Please select a new one:");
                num = int.Parse(Console.ReadLine());
            }
            var chosenCharacter = availableCharactersToAttack[charNum - 1];
            AttackCharacter(chosenSpell, chosenCharacter);

        }

        private void AttackCharacter(Spell chosenSpell, GameCharacter chosenCharacter)
        {
            #region AddDamageFromWeapon
            //Cannot use it currently because GC doesn't have a weapon yet because we havent made the logic for the currently logged in user
            //and yet you cannot choose a weapon when creating gc
            var gameCharacter = context.GameCharacters.SingleOrDefault(gc => gc.CharacterId == characterId && gc.GameId == roomId);
            var weaponDmg = context.Weapons.SingleOrDefault(w => w.Id == gameCharacter.WeaponId).Damage;
            #endregion

            var dmg = chosenSpell.DamageBonus + weaponDmg; // + weaponDmg
            Character attacker = context.Characters.SingleOrDefault(c => c.Id == characterId);
            Character victim = context.Characters.SingleOrDefault(c => c.Id == chosenCharacter.CharacterId);
            Console.WriteLine($"{attacker.Name} uses {chosenSpell.Name} on {victim.Name}. He deals {dmg} damage.");
            var dbGameCharacter = context.GameCharacters.SingleOrDefault(gc => gc.CharacterId == victim.Id && gc.GameId == roomId);
            var armour = chosenCharacter.Armour;
            var health = chosenCharacter.Health;
            if (armour >= dmg)
            {
                dbGameCharacter.Armour -= dmg;
                context.GameCharacters.Update(dbGameCharacter);
                context.SaveChanges();

                Console.WriteLine($"{victim.Name} {armour - dmg}armour left after the attack. His armour protected his Health.");
            }
            else if (armour < dmg && health > dmg - armour )
            {
                dbGameCharacter.Armour = 0;
                dbGameCharacter.Health -= dmg - armour;
                context.GameCharacters.Update(dbGameCharacter);
                context.SaveChanges();

                var currentHealth = context.GameCharacters.SingleOrDefault(gc => gc.CharacterId == victim.Id && gc.GameId == roomId).Health;

                Console.WriteLine($"{victim.Name}'s armour has been completely destroyed. His current Health has been reduced to {currentHealth}.");
            }
            else if (armour < dmg && health <= dmg - armour)
            {
                Console.WriteLine($"Sadly, {victim.Name} has been killed. He returns to the Start square and has his health and armour reset");

                dbGameCharacter.Armour = victim.Armour;
                dbGameCharacter.Health = victim.Hp;
                dbGameCharacter.CharacterPositionX = map[0][0].X;
                dbGameCharacter.CharacterPositionY = map[0][0].Y;
                dbGameCharacter.MapSectionNumber = map[0][0].Number;
                context.GameCharacters.Update(dbGameCharacter);
                context.SaveChanges();
            }

            var gaChar = context.GameCharacters.SingleOrDefault(gc => gc.GameId == roomId && gc.CharacterId == characterId);
            gaChar.Spells.Remove(chosenSpell);
            gaChar.SpellsCount--;
            context.GameCharacters.Update(gaChar);
            context.SaveChanges();

        }

        private List<GameCharacter> SeeAvailableCharacters(List<int> positions)
        {
            var gameCharacters = context.GameCharacters.Where(gc => gc.GameId == roomId).ToList();
            List<GameCharacter> availableCharactersToAttack = new List<GameCharacter>();
            foreach (var gc in gameCharacters)
            {
                if (positions.Any(p => p == gc.MapSectionNumber) && gc.CharacterId != characterId)
                {
                    availableCharactersToAttack.Add(gc);
                }
            }

            return availableCharactersToAttack;
        }

        private List<int> PositionsInRange(int? chNum, int spellRange)
        {
            var list = new List<int>();

            var counter = 1;
            if (chNum <= spellRange) // Add each poisition from the start to current one
            {
                while (counter != chNum + 1) // i can't directly add chNum so i have to make the counter equal to chNum
                {
                    list.Add(counter);
                    counter++;
                }
            }
            else
            {
                counter = (int)chNum - spellRange;
                while (counter != chNum + 1)
                {
                    list.Add(counter);
                    counter++;
                }
            }
            while (spellRange != 0)
            {
                counter +=  1;
                spellRange -= 1;
                list.Add(counter);
            }
            return list;
        }
    }
}
