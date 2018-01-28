namespace DemolitionFalcons.App.Interfaces
{
    using System.Collections.Generic;

    public interface ICommandEngine<out T>
        where T : class
    {
        T ExecuteCommand(List<string> args);
    }
}
