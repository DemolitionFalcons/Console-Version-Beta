namespace DemolitionFalcons.App.Core.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class DiceDto
    {
        public int LastRollResult;
        public int RollDice()
        {
            int result = 1;
            var rnd = new Random();
            result = rnd.Next(1, 6);

            LastRollResult = result;

            return result;
        }
    }
}
