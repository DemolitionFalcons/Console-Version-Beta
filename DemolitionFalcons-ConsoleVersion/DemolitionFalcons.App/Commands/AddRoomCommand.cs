namespace DemolitionFalcons.App.Commands
{
    using DemolitionFalcons.App.Interfaces;
    using System.Collections.Generic;

    public class AddRoomCommand : AbstractCommand
    {
        public AddRoomCommand(IList<string> args, IManager manager) : base(args, manager)
        {
        }

        public override string Execute()
        {
            return base.Manager.AddRoom(Args);
        }
    }
}
