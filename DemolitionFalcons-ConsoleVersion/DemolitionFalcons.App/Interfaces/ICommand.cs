using System.Collections.Generic;
using DemolitionFalcons.Data.DataInterfaces;

namespace DemolitionFalcons.App.Interfaces
{
    public interface ICommand
    {
        
        void Execute(IManager gameManager, IOutputWriter writer, IList<string> data);
    }
}
