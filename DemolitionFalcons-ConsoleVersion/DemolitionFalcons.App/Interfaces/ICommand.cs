namespace DemolitionFalcons.App.Interfaces
{
    using System.Collections.Generic;
    using DemolitionFalcons.Data.DataInterfaces;

    public interface ICommand
    {
        
        void Execute(IManager gameManager, IOutputWriter writer, IList<string> data);
    }
}
