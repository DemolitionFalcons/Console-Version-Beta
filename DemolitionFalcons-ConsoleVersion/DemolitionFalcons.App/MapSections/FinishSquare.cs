namespace DemolitionFalcons.App.MapSections
{
    using Maps;
    using Models;

    public class FinishSquare : MapSection
    {
        public FinishSquare(int x, int y) 
            : base(x, y)
        {
            base.Type = "FinishSquare";
        }

        public Character Winner { get; set; }

        public override int Number { get; set; }
        
    }
}
