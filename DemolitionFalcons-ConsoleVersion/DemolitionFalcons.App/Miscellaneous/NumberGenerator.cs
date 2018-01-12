namespace DemolitionFalcons.App.Miscellaneous
{
    using System;

    public class NumberGenerator
    {
        public int GenerateNumber(int min, int max)
        {
            Random rnd = new Random();
            var num = rnd.Next(min, max);
            return num;
        }
    }
}
