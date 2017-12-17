namespace DemolitionFalcons.App.Commands
{
    using System;
    using System.Collections.Generic;
    using DemolitionFalcons.App.Interfaces;
    public class RegisterCommand : AbstractCommand
    {
        public RegisterCommand(IList<string> args, IManager manager) : base(args, manager)
        {
        }

        public override string Execute()
        {
            return base.Manager.RegisterUser(Args);
        }
    }
}
