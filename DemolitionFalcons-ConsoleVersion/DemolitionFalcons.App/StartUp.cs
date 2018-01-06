using DemolitionFalcons.App.Interfaces;
using DemolitionFalcons.Data;
using DemolitionFalcons.Data.DataInterfaces;
using DemolitionFalcons.Data.IO;
using DemolitionFalcons.Data.Support;

namespace DemolitionFalcons.App
{
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
            engine.Run();
        }
    }
}
