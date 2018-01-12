namespace DemolitionFalcons.App.Maps
{
    using MapSections;
    using System;

    public class DemoMap
    {
        public string Name { get; set; }

        public DemoMap(string name)
        {
            this.Name = name;
        }

        public MapSection[][] GenerateMap()
        {
            //fill a basic level with different squares
            const int mapHeight = 10;
            const int mapLength = mapHeight;
            var map = new MapSection[mapHeight][];

        var squareNumber = 1;
            //The start square and end square will be added right under the for's
            var counter = 0;
            var bonusCounter = 0;
            for (int i = 0; i < mapHeight; i++)
            {
                map[i] = new MapSection[mapLength];
                for (int j = 0; j < mapLength; j++)
                {
                    if (bonusCounter == 7 && counter != 5)
                    {
                        var rnd = new Random();
                        if (rnd.Next(0, 10) % 2 == 0)
                        {
                            map[i][j] = new MysterySquare(i, j);

                            bonusCounter++;
                        }
                        else
                        {
                            map[i][j] = new BonusSquare(i, j);
                            bonusCounter++;
                        }
                        map[i][j].Number = squareNumber; squareNumber++;
                        bonusCounter = 0;
                        counter++;
                    }
                    else if (counter == 5)
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
                        bonusCounter++;
                    }

                    
                }
            }

            //this is the start square, can be different then 0,0 -> depends on the map
            map[0][0] = new StartSquare(0, 0);
            map[0][0].Number = 1;

            //this is the final square, who goes there first wins the game -> again depends on the map
            map[mapHeight - 1][mapLength - 1] = new FinishSquare(mapHeight - 1, mapLength - 1);
            map[mapHeight - 1][mapLength - 1].Number = 100; 

            return map;
        }

    }
}
