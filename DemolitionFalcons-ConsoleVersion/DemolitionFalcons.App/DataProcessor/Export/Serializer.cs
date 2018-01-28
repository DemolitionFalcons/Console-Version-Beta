namespace DemolitionFalcons.App.Commands.DataProcessor
{
    using DemolitionFalcons.App.Commands.DataProcessor.Export.Dto;
    using DemolitionFalcons.App.DataProcessor.Export.Dto;
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Data;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Serializer
    {
        public static string ExportGameAndMapStatistics(DemolitionFalconsDbContext context)
        {
            var map = new DemoMap("map1");

            var characters = context.GameCharacters
                .Select(c => new
                {
                    name = c.Game.Name,
                    map = map.Name,
                    numberOfOpponents = c.Game.Characters.Count()
                })
                .FirstOrDefault();

            var json = JsonConvert.SerializeObject(characters, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
                );

            return json;
        }

        public static string ExportCharacterStatistics(DemolitionFalconsDbContext context)
        {
            var map = new DemoMap("map1");

            var characters = context.Characters
                .Select(c => new
                {
                    Id = c.Id,
                    name = c.Name,
                    c.Label,
                    c.Description,
                    c.Hp,
                    c.Armour
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(characters, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
                );

            return json;
        }

        public static string ExportChooseHeroStatistics(DemolitionFalconsDbContext context)
        {
            var map = new DemoMap("map1");

            var characters = context.GameCharacters
                .Select(c => new
                {
                    map = map.Name,
                    name = c.Game.Name,
                    numberOfPlayers = c.Game.Characters.Count(),
                    players = c.Game.Characters.Select(st => new CharacterDto
                    {
                        Type = st.Type,
                        Nickname = context.Players.FirstOrDefault(p => p.Id == st.PlayerId).Username,
                        Name = st.Character.Name
                    })
                })
                .FirstOrDefault();

            var json = JsonConvert.SerializeObject(characters, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
                );

            return json;
        }

        public static string ExportFirstMapCoordinates(List<MapSection> mapSquares)
        {
            var squares = mapSquares.Select(sq => new FirstMapDto
            {
                X = sq.X,
                Y = sq.Y,
                Type = sq.Type
            })
           .ToArray();


            var json = JsonConvert.SerializeObject(squares, Formatting.None,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
                );

            return json;
        }
    }
}
