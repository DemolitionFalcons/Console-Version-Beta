namespace DemolitionFalcons.Data.Support
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DemolitionFalcons.Models;
    using Microsoft.EntityFrameworkCore;
    using DemolitionFalcons.App.Miscellaneous;

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
                var hollyWoodWand = new Weapon
                {
                    Name = "Holly Wood Wand ",
                    Core = "Phoenix feather",
                    Damage = 25
                };

                weapons.Add(hollyWoodWand);

                var vineWand = new Weapon
                {
                    Name = "Vine Wand",
                    Core = "Dragon scale",
                    Damage = 25
                };

                weapons.Add(vineWand);

                var metalWand = new Weapon
                {
                    Name = "Metal Wand",
                    Core = "Mystic tear",
                    Damage = 10
                };

                weapons.Add(metalWand);

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

                var yoner = new Player
                {
                    Username = "Yoner",
                    Password = "Jezzy",
                    GamesPlayed = 0,
                    Wins = 0
                };

                players.Add(yoner);

                context.Players.AddRange(players);
                context.SaveChanges();

                var weapons = new List<Weapon>
                {
                    context.Weapons.Where(w => w.Name == "Holly Wood Wand").FirstOrDefault(),
                    context.Weapons.Where(w => w.Name == "Vine Wand").FirstOrDefault()
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

                var vineWand = weapons.FirstOrDefault(w => w.Id == 2);
                var playersWeapons = new List<PlayerWeapon>();

                var playerWeaponSecond = new PlayerWeapon
                {
                    Player = yoner,
                    PlayerId = yoner.Id,
                    Weapon = vineWand,
                    WeaponId = vineWand.Id
                };

                playersWeapons.Add(playerWeaponSecond);

                var playerWeaponThird = new PlayerWeapon
                {
                    Player = rango,
                    PlayerId = rango.Id,
                    Weapon = vineWand,
                    WeaponId = vineWand.Id
                };

                playersWeapons.Add(playerWeaponThird);

                var playerWeaponFourth = new PlayerWeapon
                {
                    Player = claire,
                    PlayerId = claire.Id,
                    Weapon = vineWand,
                    WeaponId = vineWand.Id
                };

                playersWeapons.Add(playerWeaponFourth);

                var playerWeaponFifth = new PlayerWeapon
                {
                    Player = ricardo,
                    PlayerId = ricardo.Id,
                    Weapon = vineWand,
                    WeaponId = vineWand.Id
                };

                playersWeapons.Add(playerWeaponFifth);

                var playerWeaponSixth = new PlayerWeapon
                {
                    Player = kellyane,
                    PlayerId = kellyane.Id,
                    Weapon = vineWand,
                    WeaponId = vineWand.Id
                };

                playersWeapons.Add(playerWeaponSixth);

                context.PlayerWeapons.AddRange(playersWeapons);
                context.SaveChanges();

            }

            if (!context.Games.Any())
            {
                var game = new Game()
                {
                    Name = "FirstGameEver",
                    Xp = 50,
                    Money = 250,
                    Capacity = 6,
                    Map = "FirstMapFrontEnd"
                };

                context.Games.Add(game);
                context.SaveChanges();
            }

            if (!context.Characters.Any())
            {
                var characters = new List<Character>();

                var eagle = new Character
                {
                    Name = "Eagle",
                    Label = "The Flying Demon",
                    Description = "It comes from a distant unknown land. It has a weak defence but is very fast and subtle.",
                    Hp = 100,
                    Armour = 20,

                };

                var cloudy = new Character
                {
                    Name = "Cloudy",
                    Label = "The Cloud Potato",
                    Description = "It comes from a Cloudysland, drives a skateboard and has a strange hairstyle.",
                    Hp = 70,
                    Armour = 65,

                };

                var edward = new Character
                {
                    Name = "Edward",
                    Label = "The Raccoon's Special Troop",
                    Description = "Has developed great military skills over time. Has very strong defence but is usually unfortunate.",
                    Hp = 70,
                    Armour = 100,

                };

                var stephano = new Character
                {
                    Name = "Stephano",
                    Label = "The Mad Scientist",
                    Description = "Has worked for different non-governmental projects with an unknown role. Notably known for his bloody scientific experiments on people.",
                    Hp = 50,
                    Armour = 90,

                };

                var leonardo = new Character
                {
                    Name = "Leonardo",
                    Label = "The King Of The Jungle",
                    Description = "Has ruled over many kingdoms in all over the world and has now come to prove his strength and sageness.",
                    Hp = 90,
                    Armour = 90,

                };

                var darcus = new Character
                {
                    Name = "Darcus",
                    Label = "The Ancient Firebreather",
                    Description = "Uses his instincts and abilities to burn his enemies.",
                    Hp = 100,
                    Armour = 90,

                };

                characters.Add(eagle);
                characters.Add(cloudy);
                characters.Add(edward);
                characters.Add(stephano);
                characters.Add(leonardo);
                characters.Add(darcus);

                context.AddRange(characters);
                context.SaveChanges();

                var game = context.Games.FirstOrDefault(x => x.Name == "FirstGameEver");
                var gameChars = new List<GameCharacter>();

                var weaponTaker = new TakeMostPowerfulWeapon();

                var gameCharacterOne = new GameCharacter
                {
                    Character = characters[0],
                    CharacterId = characters[0].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 1,
                    Type = "computer",
                    Health = characters[0].Hp,
                    Armour = characters[0].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, 1).Id
                };

                gameChars.Add(gameCharacterOne);

                var gameCharacterTwo = new GameCharacter
                {
                    Character = characters[1],
                    CharacterId = characters[1].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 2,
                    Type = "computer",
                    Health = characters[1].Hp,
                    Armour = characters[1].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, 2).Id
                };

                gameChars.Add(gameCharacterTwo);

                var gameCharacterThree = new GameCharacter
                {
                    Character = characters[2],
                    CharacterId = characters[2].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 3,
                    Type = "computer",
                    Health = characters[2].Hp,
                    Armour = characters[2].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, 3).Id
                };

                gameChars.Add(gameCharacterThree);

                var gameCharacterFour = new GameCharacter
                {
                    Character = characters[3],
                    CharacterId = characters[3].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 4,
                    Type = "computer",
                    Health = characters[3].Hp,
                    Armour = characters[3].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, 4).Id
                };

                gameChars.Add(gameCharacterFour);

                var gameCharacterFive = new GameCharacter
                {
                    Character = characters[4],
                    CharacterId = characters[4].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 5,
                    Type = "computer",
                    Health = characters[4].Hp,
                    Armour = characters[4].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, 5).Id
                };

                gameChars.Add(gameCharacterFive);

                var gameCharacterSix = new GameCharacter
                {
                    Character = characters[5],
                    CharacterId = characters[5].Id,
                    Game = game,
                    GameId = game.Id,
                    PlayerId = 6,
                    Type = "computer",
                    Health = characters[5].Hp,
                    Armour = characters[5].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, 6).Id
                };

                gameChars.Add(gameCharacterSix);

                context.GameCharacters.AddRange(gameChars);
                context.SaveChanges();

            }

            if (!context.Spells.Any())
            {
                var spells = new List<Spell>();

                var fireball = new Spell("Fireball", 50, 15);
                fireball.Description = "A ball of fire flying towards any enemy and dealing huge damage";
                var crucio = new Spell("Crucio", 150, 12);
                crucio.Description = "A spells seen in the Harry Potter series for first time. One of the three Unforgivable Curses" +
                    " the Cruciatus curse causes agonising pain";
                var avadaKedavra = new Spell("Avada Kedavra", 300, 6);
                avadaKedavra.Description = "Powerful curse which instantly kills the victim";
                var expectoPatronum = new Spell("Expecto Patronum", 100, 10);
                expectoPatronum.Description = "A mytic spell that is useful against demolitional monsters";
                spells.Add(fireball);
                spells.Add(crucio);
                spells.Add(avadaKedavra);
                spells.Add(expectoPatronum);

                context.Spells.AddRange(spells);
                context.SaveChanges();
            }
        }

        public static DemolitionFalconsDbContext ResetDB(DemolitionFalconsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            //context.Database.Migrate();
            context = new DemolitionFalconsDbContext();
            Seed(context);
            return context;
        }
    }
}
