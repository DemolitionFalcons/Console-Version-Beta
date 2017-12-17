namespace DemolitionFalcons.App.MapSections
{
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Models;
    using System.Collections.Generic;

    public class StartSquare : MapSection
    {
        public StartSquare(int x, int y)
            :base(x, y)
        {

        }

        public ICollection<Character> Characters { get; set; }
    }
}
