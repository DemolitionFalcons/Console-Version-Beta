namespace DemolitionFalcons.App.Maps
{
    using Interfaces;

    public abstract class MapSection : IMapSection
    {
        public MapSection(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this.isGoBackSquare = false;
            this.isGoForwardSquare = false;
            this.isBonusSquare = false;
            this.isMysterySquare = false;
        }

        public bool isGoBackSquare { get; set; }
        public bool isGoForwardSquare { get; set; }
        public bool isBonusSquare { get; set; }
        public bool isMysterySquare { get; set; }

        public abstract int Number { get; set; }

        public int X {get; set;}
                     
        public int Y { get; set; }

        public string Type { get; set; }


    }
}
