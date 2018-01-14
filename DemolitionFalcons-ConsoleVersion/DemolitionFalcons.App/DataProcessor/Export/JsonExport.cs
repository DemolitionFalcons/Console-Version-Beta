namespace DemolitionFalcons.App.DataProcessor.Export
{
    using DemolitionFalcons.App.Commands.DataProcessor;
    using DemolitionFalcons.Data;
    using System;
    public class JsonExport
    {
        public void Export(DemolitionFalconsDbContext context)
        {
            //const string exportDir = "./ImportResults/";

            var jsonMapAndGameStatsOutput = Serializer.ExportGameAndMapStatistics(context);
            Console.WriteLine(jsonMapAndGameStatsOutput);

            var jsonCharStatsOutput = Serializer.ExportCharacterStatistics(context);
            Console.WriteLine(jsonCharStatsOutput);
            //File.WriteAllText(exportDir + "DelayedTrains.json", jsonOutput);

            var jsonChooseHeroOutput = Serializer.ExportChooseHeroStatistics(context);
            Console.WriteLine(jsonChooseHeroOutput);
        }
    }
}
