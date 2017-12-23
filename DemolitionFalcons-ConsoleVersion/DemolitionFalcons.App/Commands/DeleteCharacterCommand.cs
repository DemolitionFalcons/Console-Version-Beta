using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DemolitionFalcons.App.Interfaces;
using DemolitionFalcons.Data.DataInterfaces;

namespace DemolitionFalcons.App.Commands
{
   public class DeleteCharacterCommand : AbstractCommand
    {
        public override void Execute(IManager gameManager, IOutputWriter writer, IList<string> data)
        {
           writer.WriteLine(gameManager.DeleteCharacter(data.Skip(1).ToList()));
        }
    }
}
