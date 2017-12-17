namespace DemolitionFalcons.App.Maps
{
    using DemolitionFalcons.App.MapSections;

    public class DemoMap
    {
        public MapSection[][] GenerateMap()
        {
            //fill a basic level with different square
            int mapHeight = 10;
            int mapLength = mapHeight;
            var map = new MapSection[10][];

            //this is the start square, can be different then 0,0 -> depends on the map
            map[0][0] = new StartSquare(0,0);
            //this is the final square, who goes there first wins the game -> again depends on the map
            map[mapHeight][mapLength] = new FinishSquare(mapHeight, mapLength);
            for (int i = 1; i < mapHeight - 1; i++)
            {
                map[mapHeight] = new MapSection[10];
                for (int j = 1; j < mapLength - 1; j++)
                {
                    map[i][j] = new NormalSquare(i, j);
                }
            }

            return map;
        }

    }
}
