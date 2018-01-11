using DemolitionFalcons.App.Maps;
using DemolitionFalcons.Data;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DemolitionFalcons.App.Commands.DataProcessor
{
    public class Serializer
    {
        public static string ExportCharacterStatistics(DemolitionFalconsDbContext context)
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
    }
}
