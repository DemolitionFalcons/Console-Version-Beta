namespace DemolitionFalcons.Data.IO
{
    using System;
    using DemolitionFalcons.Data.DataInterfaces;

    public class InputReader : IInputReader
    {
        public string ReadLine() => Console.ReadLine();
    }
}
