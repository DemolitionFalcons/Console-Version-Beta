namespace DemolitionFalcons.App.Maps
{
    using DemolitionFalcons.App.Interfaces;

    public class MapSection : IMapSection
    {
        public MapSection(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X {get; set;}
                     
        public int Y { get; set; }
    }
}
