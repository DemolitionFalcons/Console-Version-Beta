namespace DemolitionFalcons.App.Miscellaneous
{
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Data;
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

        }
    }
}
