namespace DemolitionFalcons.App.MapSections
{
    using Maps;
    using Models;
    using System.Collections.Generic;

    public class MysterySquare : MapSection
    {
        public MysterySquare(int x, int y) 
            : base(x, y)
        {
            base.isMysterySquare = true;
            base.Type = "MysterySquare";
        }

        public override int Number { get; set; }


        public ICollection<Character> Characters { get; set; }
    }
}
