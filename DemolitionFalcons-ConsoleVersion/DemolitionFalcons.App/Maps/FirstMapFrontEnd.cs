namespace DemolitionFalcons.App.Maps
{
    using DemolitionFalcons.App.Commands.DataProcessor;
    using DemolitionFalcons.App.DataProcessor.Export.Dto;
    using DemolitionFalcons.App.MapSections;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FirstMapFrontEnd
    {
        public MapSection[][] GenerateFirstMap()
        {
            //map coordinates:
            


            const int mapHeight = 5;
            const int mapLength = 10;
            var map = new MapSection[mapHeight][];

            for (int i = 0; i < mapHeight; i++)
            {
                map[i] = new MapSection[mapLength];
                for (int j = 0; j < mapLength; j++)
                {
                    map[i][j] = new NormalSquare(i, j);
                }
            }

            AddCoordinates(map);
            


            var mapSquares = new List<MapSection>();

            var squareNumber = 1;
            var counter = 0;
            var bonusCounter = 0;

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapLength; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i][j] = new StartSquare(i, j);
                    }
                    else if (i == map.Length - 1 && j == map[i].Length - 1)
                    {
                        map[i][j] = new FinishSquare(i, j);
                    }
                    else if (bonusCounter > 6)
                    {
                        if ((counter / 2 + bonusCounter) % 2 == 0)
                        {
                            map[i][j] = new BonusSquare(i, j);
                        }
                        else
                        {
                            map[i][j] = new MysterySquare(i, j);
                        }
                        bonusCounter = 0;
                    }
                    else if (counter > 5 && (bonusCounter + 1) % 2 == 0)
                    {
                        map[i][j] = new GoForwardSquare(i, j);
                        counter = 1;
                    }
                    else if (counter > 5)
                    {
                        map[i][j] = new GoBackSquare(i, j);
                        counter = 1;
                    }
                    else
                    {
                        map[i][j] = new NormalSquare(i, j);
                    }

                    map[i][j].Number = squareNumber; squareNumber++;
                    counter++;
                    bonusCounter++;

                    AddCoordinates(map);
                    mapSquares.Add(map[i][j]);

                    if (squareNumber == 51)
                    {
                        break;
                    }
                }
            }


            var json = Serializer.ExportFirstMapCoordinates(mapSquares);
            Console.WriteLine(json);

            return map;
        }

        public static List<MapSection> GetMapPath()
        {
            //Used for the map path service

            const int mapHeight = 5;
            const int mapLength = 10;
            var map = new MapSection[mapHeight][];

            for (int i = 0; i < mapHeight; i++)
            {
                map[i] = new MapSection[mapLength];
                for (int j = 0; j < mapLength; j++)
                {
                    map[i][j] = new NormalSquare(i, j);
                }
            }

            AddCoordinates(map);



            var mapSquares = new List<MapSection>();

            var squareNumber = 1;
            var counter = 0;
            var bonusCounter = 0;

            for (int i = 0; i < mapHeight; i++)
            {
                for (int j = 0; j < mapLength; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        map[i][j] = new StartSquare(i, j);
                    }
                    else if (i == map.Length - 1 && j == map[i].Length - 1)
                    {
                        map[i][j] = new FinishSquare(i, j);
                    }
                    else if (bonusCounter > 6)
                    {
                        if ((counter / 2 + bonusCounter) % 2 == 0)
                        {
                            map[i][j] = new BonusSquare(i, j);
                        }
                        else
                        {
                            map[i][j] = new MysterySquare(i, j);
                        }
                        bonusCounter = 0;
                    }
                    else if (counter > 5 && (bonusCounter + 1) % 2 == 0)
                    {
                        map[i][j] = new GoForwardSquare(i, j);
                        counter = 1;
                    }
                    else if (counter > 5)
                    {
                        map[i][j] = new GoBackSquare(i, j);
                        counter = 1;
                    }
                    else
                    {
                        map[i][j] = new NormalSquare(i, j);
                    }

                    map[i][j].Number = squareNumber; squareNumber++;
                    counter++;
                    bonusCounter++;

                    AddCoordinates(map);
                    mapSquares.Add(map[i][j]);

                    if (squareNumber == 51)
                    {
                        break;
                    }
                }
            }

            return mapSquares;
        }



        public static void AddCoordinates(MapSection[][] map)
        {
            map[0][0].X = 69; map[0][0].Y = 186; /*map[0][0].Number = 1;*/
            map[0][1].X = 69; map[0][1].Y = 272; /*map[0][1].Number = 2;*/
            map[0][2].X = 144; map[0][2].Y = 315; /*map[0][2].Number = 3;*/
            map[0][3].X = 219; map[0][3].Y = 358; /*map[0][3].Number = 4;*/
            map[0][4].X = 294; map[0][4].Y = 315; /*map[0][4].Number = 5;*/
            map[0][5].X = 369; map[0][5].Y = 272; /*map[0][5].Number = 6;*/
            map[0][6].X = 444; map[0][6].Y = 315; /*map[0][6].Number = 7;*/
            map[0][7].X = 519; map[0][7].Y = 272; /*map[0][7].Number = 8;*/
            map[0][8].X = 594; map[0][8].Y = 229; /*map[0][8].Number = 9;*/
            map[0][9].X = 669; map[0][9].Y = 272; /*map[0][9].Number = 10;*/
            map[1][0].X = 669; map[1][0].Y = 358; /*map[1][0].Number = 11;*/
            map[1][1].X = 744; map[1][1].Y = 401; /*map[1][1].Number = 12;*/
            map[1][2].X = 744; map[1][2].Y = 487; /*map[1][2].Number = 13;*/
            map[1][3].X = 819; map[1][3].Y = 530; /*map[1][3].Number = 14;*/
            map[1][4].X = 894; map[1][4].Y = 487; /*map[1][4].Number = 15;*/
            map[1][5].X = 969; map[1][5].Y = 444; /*map[1][5].Number = 16;*/
            map[1][6].X = 1044; map[1][6].Y = 573; /*map[1][6].Number = 17;*/
            map[1][7].X = 969; map[1][7].Y = 616; /*map[1][7].Number = 18;*/
            map[1][8].X = 894; map[1][8].Y = 573; /*map[1][8].Number = 19;*/
            map[1][9].X = 894; map[1][9].Y = 659; /*map[1][9].Number = 20;*/
            map[2][0].X = 819; map[2][0].Y = 702; /*map[2][0].Number = 21;*/
            map[2][1].X = 744; map[2][1].Y = 745; /*map[2][1].Number = 22;*/
            map[2][2].X = 669; map[2][2].Y = 702; /*map[2][2].Number = 23;*/
            map[2][3].X = 594; map[2][3].Y = 659; /*map[2][3].Number = 24;*/
            map[2][4].X = 519; map[2][4].Y = 702; /*map[2][4].Number = 25;*/
            map[2][5].X = 444; map[2][5].Y = 745; /*map[2][5].Number = 26;*/
            map[2][6].X = 369; map[2][6].Y = 788; /*map[2][6].Number = 27;*/
            map[2][7].X = 294; map[2][7].Y = 831; /*map[2][7].Number = 28;*/
            map[2][8].X = 219; map[2][8].Y = 874; /*map[2][8].Number = 29;*/
            map[2][9].X = 219; map[2][9].Y = 960; /*map[2][9].Number = 30;*/
            map[3][0].X = 144; map[3][0].Y = 1089;/*map[3][0].Number = 31;*/
            map[3][1].X = 69; map[3][1].Y = 1132;/*map[3][1].Number = 32;*/
            map[3][2].X = 69; map[3][2].Y = 1218;/*map[3][2].Number = 33;*/
            map[3][3].X = 144; map[3][3].Y = 1261;/*map[3][3].Number = 34;*/
            map[3][4].X = 219; map[3][4].Y = 1304;/*map[3][4].Number = 35;*/
            map[3][5].X = 294; map[3][5].Y = 1347;/*map[3][5].Number = 36;*/
            map[3][6].X = 369; map[3][6].Y = 1390;/*map[3][6].Number = 37;*/
            map[3][7].X = 444; map[3][7].Y = 1433;/*map[3][7].Number = 38;*/
            map[3][8].X = 519; map[3][8].Y = 1390;/*map[3][8].Number = 39;*/
            map[3][9].X = 594; map[3][9].Y = 1347;/*map[3][9].Number = 40;*/
            map[4][0].X = 669; map[4][0].Y = 1304;/*map[4][0].Number = 41;*/
            map[4][1].X = 744; map[4][1].Y = 1261;/*map[4][1].Number = 42;*/
            map[4][2].X = 819; map[4][2].Y = 1304;/*map[4][2].Number = 43;*/
            map[4][3].X = 894; map[4][3].Y = 1261;/*map[4][3].Number = 44;*/
            map[4][4].X = 969; map[4][4].Y = 1304;/*map[4][4].Number = 45;*/
            map[4][5].X = 1044; map[4][5].Y = 1347;/*map[4][5].Number = 46;*/
            map[4][6].X = 1044; map[4][6].Y = 1433;/*map[4][6].Number = 47;*/
            map[4][7].X = 969; map[4][7].Y = 1376;/*map[4][7].Number = 48;*/
            map[4][8].X = 969; map[4][8].Y = 1562;/*map[4][8].Number = 49;*/
            map[4][9].X = 969; map[4][9].Y = 1648;/*map[4][9].Number = 50;*/
        }
    }
}

