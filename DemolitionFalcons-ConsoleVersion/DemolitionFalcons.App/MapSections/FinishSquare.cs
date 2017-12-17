namespace DemolitionFalcons.App.MapSections
{

    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Models;
    using System.Collections.Generic;

    public class FinishSquare : MapSection
    {
        public FinishSquare(int x, int y) 
            : base(x, y)
        {
        }

        public Character Winner { get; set; }
    }
}
