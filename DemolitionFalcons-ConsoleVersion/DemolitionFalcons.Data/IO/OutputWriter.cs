namespace DemolitionFalcons.Data.IO
{
    using System;
    using DemolitionFalcons.Data.DataInterfaces;

    public class OutputWriter : IOutputWriter
    {
        public void WriteLine(string message) => Console.WriteLine(message);
    }
}
