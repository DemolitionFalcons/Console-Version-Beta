

namespace DemolitionFalcons.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Data.Support;
    using Front;
    using Models;

    public class GameController : BaseApiController
    {
        [HttpGet]
        public IEnumerable<CharacterFront> Get()
        {       
            var map = new DemoMap("map1");

            var character = this.dbContext.GameCharacters
                .Select(c => new CharacterFront(c.Game.Name, map.Name, c.Game.Characters.Count()))
                .ToArray();

            return character;
        }

        // GET api/game/5
        [HttpGet("{id}")]
        public GameFront Get(int id)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Game, GameFront>());

            Game result = this.dbContext.Games
                .Where(game => game.Id == id)
                .ToArray()
                .FirstOrDefault();

            return Mapper.Map<GameFront>(result);
        }

        // POST api/game
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/game/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/game/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
