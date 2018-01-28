using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemolitionFalcons.Service.Front
{
    using App.Commands.DataProcessor.Export.Dto;

    public class CharacterFront
    {
        public CharacterFront(string name, string map, int numberOfPlayers)
        {
            this.Name = name;
            this.Map = map;
            this.NumberOfPlayers = numberOfPlayers;
        }

        public CharacterFront(string name, string map, int numberOfPlayers, IEnumerable<CharacterDto> players)
        : this(name, map, numberOfPlayers)
        {
            this.Players = players;
        }

        public CharacterFront()
        {
        }

        public string Name { get; set; }

        public string Map { get; set; }

        public int NumberOfPlayers { get; set; }

        public IEnumerable<CharacterDto> Players { get; set; }
    }
}
