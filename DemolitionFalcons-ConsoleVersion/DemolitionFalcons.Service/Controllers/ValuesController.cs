

namespace DemolitionFalcons.Service.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Data;
    using Front;

    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        // GET api/game 
        [HttpGet]
        public CharacterFront Get()
        {
            DemolitionFalconsDbContext context = new DemolitionFalconsDbContext();

            var map = new DemoMap("map1");

            var character = context.GameCharacters
                .Select(c => new CharacterFront(c.Game.Name, map.Name, c.Game.Characters.Count()))
                .FirstOrDefault();

            return character;
        }

        // GET api/game/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
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
