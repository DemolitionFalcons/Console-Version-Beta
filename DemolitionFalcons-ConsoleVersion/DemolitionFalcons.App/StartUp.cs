using DemolitionFalcons.App.Interfaces;
using DemolitionFalcons.Data;
using DemolitionFalcons.Data.DataInterfaces;
using DemolitionFalcons.Data.IO;
using DemolitionFalcons.Data.Support;

namespace DemolitionFalcons.App
{
    using DemolitionFalcons.App.Commands.DataProcessor;
    using DemolitionFalcons.App.Core;
    using System;
    using System.IO;
    using Core;

    public class StartUp
    {
        public static void Main()
        {
            DemolitionFalconsDbContext context = new DemolitionFalconsDbContext();

            IInputReader reader = new InputReader();
            IOutputWriter writer = new OutputWriter();
            SetUpDatabase.CreateDataBase(context);
            IManager gameManager = new GameManager(context, writer, reader);

            CommandEngine<ICommand> commandEngine = new CommandEngine<ICommand>();

            Engine engine = new Engine(reader, writer, commandEngine, gameManager, context);
            //const string exportDir = "./ImportResults/";

            var jsonMapAndGameStatsOutput = Serializer.ExportGameAndMapStatistics(context);
            Console.WriteLine(jsonMapAndGameStatsOutput);

            var jsonCharStatsOutput = Serializer.ExportCharacterStatistics(context);
            Console.WriteLine(jsonCharStatsOutput);
            //File.WriteAllText(exportDir + "DelayedTrains.json", jsonOutput);

            var jsonChooseHeroOutput = Serializer.ExportChooseHeroStatistics(context);
            Console.WriteLine(jsonChooseHeroOutput);

            engine.Run();
        }
    }
}
