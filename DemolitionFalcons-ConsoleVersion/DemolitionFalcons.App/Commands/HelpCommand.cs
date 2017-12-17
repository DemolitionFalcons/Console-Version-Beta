namespace DemolitionFalcons.App.Commands
{
    using System.Collections.Generic;
    using DemolitionFalcons.App.Interfaces;

    public class HelpCommand : AbstractCommand
    {
        public HelpCommand(IList<string> args, IManager manager) : base(args, manager)
        {
        }

        public override string Execute()
        {
            return base.Manager.Help(Args);
        }
    }
}
