namespace DemolitionFalcons.App.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DemolitionFalcons.App.Core.DTOs;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Models;
    using System.Threading.Tasks;
    using DemolitionFalcons.Data.DataInterfaces;
    using DemolitionFalcons.Data.ExeptionsMessages;
    using DemolitionFalcons.Data.Support;
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.App.MapSections;
    using DemolitionFalcons.App.Miscellaneous.SpecialSquares;
    using DemolitionFalcons.App.Miscellaneous.SpecialSquares.MysterySquare;
    using DemolitionFalcons.App.Miscellaneous;

    public class GameManager : IManager
    {
        // Most of the methods that WILL be added here later must work with the database or with DTO (Data Transfer Objects)
        public int playersCreated;
        private DemolitionFalconsDbContext context;
        private IOutputWriter writer;
        private IInputReader reader;
        private NumberGenerator numberGenerator;

        public GameManager(DemolitionFalconsDbContext context, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.Players = this.context.Players.ToList();
            this.writer = writer;
            this.reader = reader;
            this.numberGenerator = new NumberGenerator();
        }



        public List<Player> Players;

        public string AddRoom(IList<string> arguments)// or AddGame
        {
            var gameName = arguments[0];

            try
            {
                Console.WriteLine($"Enter map's name:");
                var map = Console.ReadLine();
                Console.WriteLine($"Enter game's capacity:");
                var capacity = int.Parse(Console.ReadLine());
                Console.WriteLine($"Enter game's xp:");
                var xp = int.Parse(Console.ReadLine());
                Console.WriteLine($"Enter game's price pool(decimal):");
                var price = decimal.Parse(Console.ReadLine());
                var gameDto = new GameDto
                {
                    Name = gameName,
                    Capacity = capacity,
                    XP = xp,
                    Money = price,
                    Map = map
                };

                var dbGame = new Game
                {
                    Name = gameDto.Name,
                    Xp = gameDto.XP,
                    Money = gameDto.Money,
                    Capacity = gameDto.Capacity,
                    Map = gameDto.Map
                };

                context.Games.Add(dbGame);
                context.SaveChanges();
                return "Room created";
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }
        }

        public string RegisterUser()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                var player = new PlayerDto();
                writer.WriteLine($"Enter player's username:");
                player.Username = Console.ReadLine();
                writer.WriteLine($"Enter player's password");
                player.Password = Console.ReadLine();

                var wand = context.Weapons.FirstOrDefault(w => w.Name == "Metal Wand");

                var dbPlayer = new Player
                {
                    Username = player.Username,
                    Password = player.Password,
                    GamesPlayed = player.GamesPlayed,
                    Wins = player.Wins
                };

                context.Players.Add(dbPlayer);

                var playerWeapon = new PlayerWeapon
                {
                    Player = dbPlayer,
                    PlayerId = dbPlayer.Id,
                    Weapon = wand,
                    WeaponId = wand.Id
                };

                context.PlayerWeapons.Add(playerWeapon);

                context.SaveChanges();

                sb.AppendLine($"Successfully created player {player.Username}");

                Players.Add(dbPlayer);
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }
            return sb.ToString();
        }

        public string CreateCharacter(IList<string> arguments)
        {
            var characterName = arguments[0];

            try
            {
                Console.WriteLine($"Please enter character's label:");
                var label = Console.ReadLine();
                Console.WriteLine($"Please enter character's description(at least 15characters):");
                var description = Console.ReadLine();
                Console.WriteLine($"Do you want to enter manually character's health and armour or use the default values?");
                Console.WriteLine($"M => Manually ; D => Default values (In case of other input the default values would be choosen");
                var decision = Console.ReadLine();
                var hp = 0;
                var armour = 0;
                bool manual = false;
                if (decision == "M")
                {
                    Console.WriteLine($"Health:");
                    hp = int.Parse(Console.ReadLine());
                    Console.WriteLine($"Armour:");
                    armour = int.Parse(Console.ReadLine());
                    manual = true;
                }

                var characterDto = new CharacterDto
                {
                    Name = characterName,
                    Label = label,
                    Description = description
                };

                var dbCharacter = new Character
                {
                    Name = characterDto.Name,
                    Label = characterDto.Label,
                    Description = characterDto.Description
                };

                if (manual)
                {
                    characterDto.Health = hp;
                    characterDto.Armour = armour;
                    dbCharacter.Hp = characterDto.Health;
                    dbCharacter.Armour = characterDto.Armour;
                }


                context.Characters.Add(dbCharacter);
                context.SaveChanges();
                return "Character created";
            }
            catch (ArgumentException ex)
            {
                return ex.Message;
            }
        }

        public string DeleteCharacter(IList<string> arguments)
        {
            string characterName = arguments[0];

            Character character = context.Characters.SingleOrDefault(c => c.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.InvalidCharakterMsg, characterName));
            }

            context.Characters.Remove(character);
            context.SaveChanges();
            return $"Character {characterName} was successfully deleted";
        }

        public string Help()
        {
            StringBuilder sb = new StringBuilder();
            //We can make option to add consumables too
            sb.AppendLine("The game here is completed by typing commands in the console.");
            sb.AppendLine("Here are the basic commands:");
            sb.AppendLine(">Register -> go on to register a user");
            sb.AppendLine(">Create {Name} -> you will be send further to edit the info of the character you're up to create with the given name");
            sb.AppendLine(">AddRoom {Name} -> you will be send further to create a playing room");
            sb.AppendLine(">JoinRoom -> choose from a list of all currently available rooms");
            sb.AppendLine(">CreateCharacter {Name} -> adds a character");
            sb.AppendLine(">StartGame -> choose a game from the list of currently available to start games");
            sb.AppendLine(">Inspect Character -> get overall info about your character");
            sb.AppendLine(">Delete Character -> delete a specified character ");
            sb.AppendLine(">Help -> you'll be shown the list with commands once again");
            sb.AppendLine(">Quit -> quit the game / and lose everything simply because we don't have DB yet :D /");

            return sb.ToString().Trim();
        }

        public string InspectCharacter(IList<string> arguments)
        {
            string characterName = arguments[0];

            Character character = context.Characters.SingleOrDefault(c => c.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.InvalidCharakterMsg, characterName));
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Character Name {character.Name}");
            sb.AppendLine($"Armor {character.Armour}");
            sb.AppendLine($"HP {character.Hp}");

            return sb.ToString().Trim();
        }

        public string JoinRoom(IList<string> arguments)
        {
            //gameName
            //string roomName = arguments[0];
            StringBuilder sb = new StringBuilder();
            try
            {
                var rooms = context.Games.ToList();

                Console.WriteLine($"Here is a list of all available rooms:");
                for (int i = 0; i < rooms.Count; i++)
                {
                    var num = i + 1;
                    var room = rooms[i];
                    Console.WriteLine($"{num}. {room.Name} {room.Characters.Count}");
                }
                Console.WriteLine($"Choose which one you would like to join.(integer)");
                var roomNumber = int.Parse(Console.ReadLine());

                while (rooms[roomNumber - 1].Characters.Count == 6)
                {
                    Console.WriteLine($"{rooms[roomNumber - 1].Name} is full, please choose another room.");
                    roomNumber = int.Parse(Console.ReadLine());
                }

                Console.WriteLine($"Successfully Joined Room {rooms[roomNumber - 1].Name}");
                Console.WriteLine($"Please choose a character from the list below:");

                var characters = context.Characters.ToList();

                for (int i = 0; i < characters.Count; i++)
                {
                    var num = i + 1;
                    var character = characters[i];
                    Console.WriteLine($"{num}. {character.Name} - {character.Hp}HP and {character.Armour}Armour");
                }

                Console.WriteLine($"Choose which one you would like to use.");
                var characterNumber = int.Parse(Console.ReadLine());
                var gameCharacter = context.GameCharacters.Where(g => g.GameId == rooms[roomNumber - 1].Id).ToList();
                while (gameCharacter.Where(g => g.GameId == rooms[roomNumber - 1].Id).Any(g => g.CharacterId == characters[characterNumber - 1].Id))
                {
                    Console.WriteLine($"{characters[characterNumber - 1]} is already taken, please choose another one.");
                    Console.WriteLine($"Or if you want to exit JoinRoom, type Exit");
                    characterNumber = int.Parse(Console.ReadLine());
                }
                var player = context.Players.LastOrDefault();
                var weaponTaker = new TakeMostPowerfulWeapon();

                GameCharacter gc = new GameCharacter
                {
                    Character = characters[characterNumber - 1],
                    CharacterId = characters[characterNumber - 1].Id,
                    Game = rooms[roomNumber - 1],
                    GameId = rooms[roomNumber - 1].Id,
                    PlayerId = player.Id,
                    Health = characters[characterNumber - 1].Hp,
                    Armour = characters[characterNumber - 1].Armour,
                    WeaponId = weaponTaker.GetMostPowerfulWeapon(context, player.Id).Id
                };
                context.GameCharacters.Add(gc);
                context.SaveChanges();
                sb.AppendLine($"Successfully joined room {rooms[roomNumber - 1].Name}. You will use {characters[characterNumber - 1].Name} as your character for this game!");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return sb.ToString().TrimEnd();
        }

        public string Quit()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Thanks for playing Demolition Falcons " + "\u00a9");
            sb.AppendLine($"See you soon.");

            return sb.ToString().Trim();
        }

        public string ResetDatabase()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                SetUpDatabase.ResetDB(context);
                this.context = SetUpDatabase.ResetDB(context);

                sb.AppendLine("Reset Database - successful");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return sb.ToString().TrimEnd();
        }

        //All the methods below feature in-game activity

        public string StartGame()
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                Console.WriteLine($"Choose from the rooms below that could begin:");
                var roomsReady = context.Games.Where(g => g.Characters.Count >= 2).ToList();

                for (int i = 0; i < roomsReady.Count; i++)
                {
                    var numToDisplay = i + 1;
                    Console.WriteLine($"{numToDisplay}. {roomsReady[i].Name} - {roomsReady[i].Characters.Count} players");
                }
                var roomNum = int.Parse(Console.ReadLine());

                var room = roomsReady[roomNum - 1];

                Console.WriteLine("Which map would you like to use?(Type in 'FirstMap' or 'DemoMap'");
                var preferredMap = Console.ReadLine().ToLower();
                while (preferredMap != "firstmap" && preferredMap != "demomap")
                {
                    Console.WriteLine("That was incorrect! Type the map again.");
                    preferredMap = Console.ReadLine().ToLower();
                }

                Console.WriteLine("'Single' - 1 or 'Multyplayer' - 2");
                var singleOrMulty = Console.ReadLine();
                while (singleOrMulty != "1" && singleOrMulty != "2")
                {
                    Console.WriteLine("1 or 2");
                    singleOrMulty = Console.ReadLine();
                }

                PlayGame game = new PlayGame(context, numberGenerator);
                var isSingle = true;

                if (singleOrMulty == "1")
                {
                    var counter = 1;
                    foreach (var chare in context.GameCharacters.Where(gc => gc.GameId == room.Id))
                    {
                        Console.WriteLine($"{counter}. {chare.Character.Name} - {chare.Character.Hp} hp and {chare.Character.Armour}");
                        counter++;
                    }
                    Console.WriteLine("Choose a character");
                    var n = int.TryParse(Console.ReadLine(), out int charId);

                    while (charId < 1 || charId > context.GameCharacters.Where(gc => gc.GameId == room.Id).Count())
                    {
                        Console.WriteLine("Enter a valid number");
                        int.TryParse(Console.ReadLine(), out charId);
                    }

                    game.HaveFun(room, sb, preferredMap, isSingle, charId);
                }
                else
                {
                    isSingle = false;
                    var charId = 0;
                    game.HaveFun(room, sb, preferredMap, isSingle, charId);
                    //HaveFun(room, sb, preferredMap);
                }

            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return sb.ToString().TrimEnd();
        }
        
    }
}
