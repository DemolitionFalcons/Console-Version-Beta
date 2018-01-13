namespace DemolitionFalcons.App.MapSections
{
    using Maps;
    using Models;
    using System.Collections.Generic;

    public class NormalSquare : MapSection
    {
        public NormalSquare(int x, int y) 
            : base(x, y)
        {
            base.Type = "NormalSquare";
        }

        public override int Number { get; set; }


        public ICollection<Character> Characters { get; set; }
    }
}
