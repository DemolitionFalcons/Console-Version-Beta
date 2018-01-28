using DemolitionFalcons.App.Interfaces;
using DemolitionFalcons.Data;
using DemolitionFalcons.Data.DataInterfaces;
using DemolitionFalcons.Data.IO;
using DemolitionFalcons.Data.Support;

namespace DemolitionFalcons.App
{
    using DemolitionFalcons.App.Commands.DataProcessor;
    using DemolitionFalcons.App.Core;
    using DemolitionFalcons.App.DataProcessor.Export;
    using System;
    using System.IO;

    public class StartUp
    {
        public static void Main()
        {
            DemolitionFalconsDbContext context = new DemolitionFalconsDbContext();
            SetUpDatabase.CreateDataBase(context);

            Engine engine = new Engine(context);

            JsonExport jsonExporter = new JsonExport();
            jsonExporter.Export(context);

            engine.Run();
        }

    }
}
