namespace DemolitionFalcons.App.Commands
{
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data.DataInterfaces;
    using System.Linq;
    using System.Collections.Generic;

    public class InspectCharacterCommand : Command
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.InspectCharacter(data.Skip(1).ToList()));
        }
    }
}
