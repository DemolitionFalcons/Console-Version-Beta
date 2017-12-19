using DemolitionFalcons.App.Interfaces;
using DemolitionFalcons.Data.DataInterfaces;
using DemolitionFalcons.Data.IO;

namespace DemolitionFalcons.App
{
    using DemolitionFalcons.App.Core;
    using System;

    public class StartUp
    {
        public static void Main()
        {

            IInputReader reader = new InputReader();
            IOutputWriter writer = new OutputWriter();

            CommandEngine<ICommand> commandEngine = new CommandEngine<ICommand>();

            Engine engine = new Engine(reader,writer,commandEngine);
            engine.Run();
        }
    }
}
