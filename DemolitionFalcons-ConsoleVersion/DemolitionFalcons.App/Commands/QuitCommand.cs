namespace DemolitionFalcons.App.Commands
{
    using System;
    using System.Collections.Generic;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data.DataInterfaces;

    public class QuitCommand : AbstractCommand
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.Quit());
            Environment.Exit(0);
        }
    }
}
