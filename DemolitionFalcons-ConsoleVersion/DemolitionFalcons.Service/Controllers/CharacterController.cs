using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemolitionFalcons.Service.Controllers
{
    using App.Commands.DataProcessor;
    using App.Commands.DataProcessor.Export.Dto;
    using App.Maps;
    using Front;

    public class CharacterController : BaseApiController
    {
        [HttpGet]
        public CharacterFront Get()
        {
            var map = new DemoMap("map1");

            CharacterFront character = this.dbContext.GameCharacters
                .Select(c => new CharacterFront
                {
                    Map = map.Name,
                    Name = c.Game.Name,
                    NumberOfPlayers = c.Game.Characters.Count(),
                    Players = c.Game.Characters.Select(st => new CharacterDto
                    {
                        Type = st.Type,
                        Nickname = this.dbContext.Players.FirstOrDefault(p => p.Id == st.PlayerId).Username,
                        Name = st.Character.Name
                    })
                })
                .FirstOrDefault();

            return character;

        }
    }
}