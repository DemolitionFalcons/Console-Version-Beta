namespace DemolitionFalcons.App.Commands
{
    using System.Collections.Generic;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data.DataInterfaces;

    public class ResetDatabaseCommand : AbstractCommand
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.ResetDatabase());
        }
    }
}
