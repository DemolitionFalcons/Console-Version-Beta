namespace DemolitionFalcons.App.Commands
{
    using System.Collections.Generic;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data.DataInterfaces;
    using System.Linq;

    public class AddRoomCommand : AbstractCommand
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.AddRoom(data.Skip(1).ToList()));
        }
    }
}
