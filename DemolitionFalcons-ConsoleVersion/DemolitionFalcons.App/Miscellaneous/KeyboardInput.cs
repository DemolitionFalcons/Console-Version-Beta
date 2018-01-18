using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemolitionFalcons.App.Miscellaneous
{
    public class KeyboardInput
    {
        public bool ReadInput()
        {
            var command = Console.ReadKey(true);
            //ConsoleKey.Spacebar
            if (command.Key.ToString() == "Spacebar")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
