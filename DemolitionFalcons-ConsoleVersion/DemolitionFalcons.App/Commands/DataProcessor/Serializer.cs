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
            var characters = context.Characters
                .Select(c => new
                {
                    c.Name,
                    Health = c.Hp,
                    c.Armour,
                    GamesPlayed = c.Games,
                })
                .ToArray();

            var json = JsonConvert.SerializeObject(characters, Formatting.Indented,
            new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            }
                );

            return json;
        }
    }
}
