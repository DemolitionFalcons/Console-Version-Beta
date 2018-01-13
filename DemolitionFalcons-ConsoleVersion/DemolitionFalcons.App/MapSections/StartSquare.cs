namespace DemolitionFalcons.App.MapSections
{
    using Maps;
    using Models;
    using System.Collections.Generic;

    public class StartSquare : MapSection
    {
        public StartSquare(int x, int y)
            :base(x, y)
        {
            base.Type = "StartSquare";
        }

        public ICollection<Character> Characters { get; set; }

        public override int Number { get; set; }
    }
}
