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
            if (!context.GameCharacters.SingleOrDefault(ch => ch.CharacterId == characterId).Spells.Any())
            {
                throw new ArgumentException("You cannot make an atack due to lack of spells!");
            }

            var chNum = context.GameCharacters.FirstOrDefault(c => c.CharacterId == characterId && c.GameId == roomId)
                    .MapSectionNumber;

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
