namespace DemolitionFalcons.App.Commands
{
    using System.Collections.Generic;
    using Interfaces;
    using Data.DataInterfaces;

    public class StartGameCommand : AbstractCommand
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.StartGame());
        }
    }
}
