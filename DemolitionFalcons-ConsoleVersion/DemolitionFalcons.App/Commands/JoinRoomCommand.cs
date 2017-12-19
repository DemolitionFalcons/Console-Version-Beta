namespace DemolitionFalcons.App.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data.DataInterfaces;

    public class JoinRoomCommand : Command
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
            writer.WriteLine(gameManager.JoinRoom(data.Skip(1).ToList()));
        }
    }
}
