namespace DemolitionFalcons.App.Maps
{
    using DemolitionFalcons.App.MapSections;
    using System;

    public class DemoMap
    {
        public MapSection[][] GenerateMap()
        {
            //fill a basic level with different squares
            int mapHeight = 10;
            int mapLength = mapHeight;
            var map = new MapSection[10][];


            var squareNumber = 2;
            //The start square and end square will be added right under the for's
            var counter = 0;
            for (int i = 1; i < mapHeight - 2; i++)
            {
                map[mapHeight] = new MapSection[10];
                for (int j = 1; j < mapLength - 2; j++)
                {
                    if (counter == 5)
                    {
                        var rnd = new Random();
                        if (rnd.Next(0,4) % 2 == 0)
                        {
                            map[i][j] = new GoForwardSquare(i, j);

                            counter++;
                        }
                        else
                        {
                            map[i][j] = new GoBackSquare(i, j);
                            counter++; 
                        }
                        counter = 0;
                        map[i][j].Number = squareNumber; squareNumber++;
                    }
                    else
                    {
                        map[i][j] = new NormalSquare(i, j);
                        map[i][j].Number = squareNumber; squareNumber++;
                        counter++; 
                    }
                }
            }

            //this is the start square, can be different then 0,0 -> depends on the map
            map[0][0] = new StartSquare(0, 0);
            map[0][0].Number = 1;
            //this is the final square, who goes there first wins the game -> again depends on the map
            map[mapHeight][mapLength] = new FinishSquare(mapHeight, mapLength);
            map[mapHeight][mapLength].Number = 100; 

            return map;
        }

    }
}
