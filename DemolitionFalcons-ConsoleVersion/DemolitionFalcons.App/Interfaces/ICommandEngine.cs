using System.Collections.Generic;

namespace DemolitionFalcons.App.Interfaces
{
    public interface ICommandEngine<out T>
        where T : class
    {
        T ExecuteCommand(List<string> args);
    }
}
