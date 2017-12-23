namespace DemolitionFalcons.App.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data.DataInterfaces;

    public class RegisterCommand : AbstractCommand
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.RegisterUser());
        }
    }
}
