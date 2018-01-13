namespace DemolitionFalcons.App.MapSections
{
    using Maps;
    using Models;
    using System.Collections.Generic;

    public class GoBackSquare : MapSection
    {
        public GoBackSquare(int x, int y)
            : base(x, y)
        {
            base.isGoBackSquare = true;
            base.Type = "GoBackSquare";
        }

        public override int Number { get; set; }


        public ICollection<Character> Characters { get; set; }
    }
}
