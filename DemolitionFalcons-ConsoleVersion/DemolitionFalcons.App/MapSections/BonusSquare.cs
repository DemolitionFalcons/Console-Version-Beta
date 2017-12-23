namespace DemolitionFalcons.App.MapSections
{

    using DemolitionFalcons.App.Maps;

    public class BonusSquare : MapSection
    {
        //gain ammunition and maybe HP and armour
        public BonusSquare(int x, int y) 
            : base(x, y)
        {
        }

        public override int Number { get ; set ; }
    }
}
