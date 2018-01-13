namespace DemolitionFalcons.App.MapSections
{
    using Maps;

    public class BonusSquare : MapSection
    {
        //gain ammunition and maybe HP and armour
        public BonusSquare(int x, int y) 
            : base(x, y)
        {
            base.isBonusSquare = true;
            base.Type = "BonusSquare";
        }

        public override int Number { get ; set ; }
    }
}
