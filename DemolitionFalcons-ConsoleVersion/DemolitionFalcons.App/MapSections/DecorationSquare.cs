namespace DemolitionFalcons.App.MapSections
{
    using Maps;

    public class DecorationSquare : MapSection
    {
        public DecorationSquare(int x, int y) 
            : base(x, y)
        {
            base.Type = "DecorationSquare";
        }

        public override int Number { get; set; }
    }
}
