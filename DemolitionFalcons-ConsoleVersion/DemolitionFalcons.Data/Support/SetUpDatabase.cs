using System.Collections.Generic;
using System.Linq;
using DemolitionFalcons.Models;
using Microsoft.EntityFrameworkCore;

namespace DemolitionFalcons.Data.Support
{
    public static class SetUpDatabase
    {

        public static void CreateDataBase(DemolitionFalconsDbContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            Seed(context);

        }
        private static void Seed(DemolitionFalconsDbContext context)
        {
            //Add just 2 basic weapons with same stats in order to test
            //If you want you're free to add more
            if (!context.Weapons.Any())
            {
                var weapons = new List<Weapon>();
                var akModel = new Weapon
                {
                    Name = "AK-47",
                    ClipSize = 30,
                    TotalCapacity = 150,
                    Damage = 50
                };

                weapons.Add(akModel);

                var m4a1s = new Weapon
                {
                    Name = "M4A1-S",
                    ClipSize = 30,
                    TotalCapacity = 150,
                    Damage = 50
                };

                weapons.Add(m4a1s);

                var glock = new Weapon
                {
                    Name = "Glock",
                    ClipSize = 15,
                    TotalCapacity = 90,
                    Damage = 10
                };

                weapons.Add(glock);

                context.Weapons.AddRange(weapons);
                context.SaveChanges();
            }

            //Add a bot player in order to test if all is correct
            //Add weapons to him
            //Add  a playerWeapon connection
            if (!context.Players.Any() || context.Players.Count() < 5)
            {
                var players = new List<Player>();

                var bot = new Player
                {
                    Username = "Bot",
                    Password = "bot123",
                    GamesPlayed = 0,
                    Wins = 0
                };

                players.Add(bot);

                var kellyane = new Player
                {
                    Username = "Kellyane",
                    Password = "kelikeli",
                    GamesPlayed = 0,
                    Wins = 0
                };

                players.Add(kellyane);

                var ricardo = new Player
                {
                    Username = "Ricardo",
                    Password = "Bonicelli",
                    GamesPlayed = 0,
                    Wins = 0
                };

                players.Add(ricardo);

                var claire = new Player
                {
                    Username = "Claire",
                    Password = "Buttss",
                    GamesPlayed = 0,
                    Wins = 0
                };

                players.Add(claire);

                var rango = new Player
                {
                    Username = "Rango",
                    Password = "ChainZ",
                    GamesPlayed = 0,
                    Wins = 0
                };

                players.Add(rango);

                context.Players.AddRange(players);
                context.SaveChanges();

                var weapons = new List<Weapon>
                {
                    context.Weapons.Where(w => w.Name == "AK-47").FirstOrDefault(),
                    context.Weapons.Where(w => w.Name == "M4A1-S").FirstOrDefault()
                };
                foreach (var weapon in weapons)
                {
                    var playerWeapon = new PlayerWeapon
                    {
                        Player = bot,
                        PlayerId = bot.Id,
                        Weapon = weapon,
                        WeaponId = weapon.Id
                    };
                    context.PlayerWeapons.Add(playerWeapon);
                    context.SaveChanges();
                }

            }

            if (!context.Games.Any())
            {
                var game = new Game()
                {
                    Name = "FirstGameEver",
                    Xp = 50,
                    Money = 250,
                };

                context.Games.Add(game);
                context.SaveChanges();
            }

            if (!context.Characters.Any())
            {
                var characters = new List<Character>();

                var ilian = new Character
                {
                    Name = "Ilian",
                    Hp = 100,
                    Armour = 100,

                };

                var alex = new Character
                {
                    Name = "Alex",
                    Hp = 100,
                    Armour = 100,

                };

                var dimitar = new Character
                {
                    Name = "Dimitar",
                    Hp = 70,
                    Armour = 0,

                };

                var zlatyo = new Character
                {
                    Name = "Zlatyo",
                    Hp = 100,
                    Armour = 25,

                };

                var stoyan = new Character
                {
                    Name = "Stoyan",
                    Hp = 100,
                    Armour = 50,

                };

                characters.Add(ilian);
                characters.Add(alex);
                characters.Add(dimitar);
                characters.Add(zlatyo);
                characters.Add(stoyan);

                context.AddRange(characters);
                context.SaveChanges();

                var game = context.Games.FirstOrDefault(x => x.Name == "FirstGameEver");
                var gameChars = new List<GameCharacter>();

                var gameCharacterOne = new GameCharacter
                {
                    Character = characters[0],
                    CharacterId = characters[0].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 1
                };

                gameChars.Add(gameCharacterOne);

                var gameCharacterTwo = new GameCharacter
                {
                    Character = characters[1],
                    CharacterId = characters[1].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 2
                };

                gameChars.Add(gameCharacterTwo);

                var gameCharacterThree = new GameCharacter
                {
                    Character = characters[2],
                    CharacterId = characters[2].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 3
                };

                gameChars.Add(gameCharacterThree);

                var gameCharacterFour = new GameCharacter
                {
                    Character = characters[3],
                    CharacterId = characters[3].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 4
                };

                gameChars.Add(gameCharacterFour);

                var gameCharacterFive = new GameCharacter
                {
                    Character = characters[4],
                    CharacterId = characters[4].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 5
                };

                gameChars.Add(gameCharacterFive);

                context.GameCharacters.AddRange(gameChars);
                context.SaveChanges();

            }
        }

        public static void ResetDB(DemolitionFalconsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //context.Database.Migrate();
            context = new DemolitionFalconsDbContext();
            Seed(context);
        }
    }
}
