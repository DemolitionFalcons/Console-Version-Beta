namespace DemolitionFalcons.App.Maps
{
    using Interfaces;

    public abstract class MapSection : IMapSection
    {
        public MapSection(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public abstract int Number { get; set; }

        public int X {get; set;}
                     
        public int Y { get; set; }

        
    }
}
