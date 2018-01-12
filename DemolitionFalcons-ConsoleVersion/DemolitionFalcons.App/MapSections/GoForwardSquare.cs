namespace DemolitionFalcons.App.MapSections
{
    using Maps;
    using Models;
    using System.Collections.Generic;

    public class GoForwardSquare : MapSection
    {
        public GoForwardSquare(int x, int y) 
            : base(x, y)
        {
            base.isGoForwardSquare = true;
        }

        public override int Number { get; set; }


        public ICollection<Character> Characters { get; set; }
    }
}
