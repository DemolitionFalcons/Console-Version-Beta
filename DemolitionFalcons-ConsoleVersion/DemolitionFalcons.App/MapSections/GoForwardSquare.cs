namespace DemolitionFalcons.App.MapSections
{
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Models;
    using System.Collections.Generic;

    public class GoForwardSquare : MapSection
    {
        public GoForwardSquare(int x, int y) 
            : base(x, y)
        {
        }

        public override int Number { get; set; }


        public ICollection<Character> Characters { get; set; }
    }
}
