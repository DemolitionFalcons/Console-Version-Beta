namespace DemolitionFalcons.App.Commands
{
    using DemolitionFalcons.App.Interfaces;
    using System.Collections.Generic;

    public class CreateCharacterCommand : AbstractCommand
    {
        public CreateCharacterCommand(IList<string> args, IManager manager) : base(args, manager)
        {
        }

        public override string Execute()
        {
            return base.Manager.CreateCharacter(Args);
        }
    }
}
