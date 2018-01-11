using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemolitionFalcons.Service.Front
{
    public class CharacterFront
    {
        public CharacterFront(string name, string map, int numberOfOpponents)
        {
            this.Name = name;
            this.Map = map;
            this.NumberOfOpponents = numberOfOpponents;
        }

        public string Name { get; set; }

        public string Map { get; set; }

        public int NumberOfOpponents { get; set; }
    }
}
