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

    public class GameManager : IManager
    {
        // Most of the methods that WILL be added here later must work with the database or with DTO (Data Transfer Objects)
        public int playersCreated;
        private DemolitionFalconsDbContext context;
        private IOutputWriter writer;
        private IInputReader reader;

        public GameManager(DemolitionFalconsDbContext context, IOutputWriter writer, IInputReader reader)
        {
            this.context = context;
            this.Players = this.context.Players.ToList();
            this.writer = writer;
            this.reader = reader;
        }



        public List<Player> Players;

        public string AddRoom(IList<string> arguments)// or AddGame
        {
            var gameName = arguments[0];

            try
            {
                var gameDto = new GameDto
                {
                    Name = gameName,
                };

                var dbGame = new Game
                {
                    Name = gameDto.Name,
                    Xp = 50,
                    Money = 200
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

                var pistol = context.Weapons.FirstOrDefault(w => w.Name == "Glock");

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
                    Weapon = pistol,
                    WeaponId = pistol.Id
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
                var characterDto = new CharacterDto
                {
                    Name = characterName
                };

                var dbCharacter = new Character
                {
                    Name = characterDto.Name
                };

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
                GameCharacter gc = new GameCharacter
                {
                    Character = characters[characterNumber - 1],
                    CharacterId = characters[characterNumber - 1].Id,
                    Game = rooms[roomNumber - 1],
                    GameId = rooms[roomNumber - 1].Id
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

                sb.AppendLine("Reset Database - successful");
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return sb.ToString().TrimEnd();
        }

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

                HaveFun(room, sb);


            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return sb.ToString().TrimEnd();
        }

        private void HaveFun(Game room, StringBuilder sb)
        {
            var map = new DemoMap();
            var playableMap = map.GenerateMap();

            //Set all characters on the start
            var toBreak = false;
            for (int i = 0; i < playableMap.Length; i++)
            {
                for (int j = 0; j < playableMap[i].Length; j++)
                {
                    if (playableMap[i][j].Number == 1)
                    {
                        var positionNumber = playableMap[i][j].Number;
                        foreach (var chare in room.Characters)
                        {
                            var character = chare.Character;
                            UpdateCharacterPositionInDb(character, i, j, positionNumber);

                            //sb.AppendLine($"Characters set on the Start");
                            Console.WriteLine($"Characters set on the Start");
                        }
                        toBreak = true;
                        break;
                    }
                }

                if (toBreak)
                {
                    break;
                }
            }

            DiceDto dice = new DiceDto();

            bool hasReachedFinalSpot = false;

            var playerInTurn = 1;

            var game = context.Games.FirstOrDefault(g => g.Id == room.Id);
            var characters = new List<Character>();
            foreach (var gc in game.Characters)
            {
                var character = gc.Character;
                characters.Add(character);
            }
            while (!hasReachedFinalSpot)
            {
                Console.WriteLine($"Type R in order to roll the dice!");
                var input = Console.ReadLine();
                while (input != "R")
                {
                    Console.WriteLine("Invalid input, please try again:");
                    input = Console.ReadLine();
                }

                var diceResult = dice.RollDice();

                var character = characters[playerInTurn - 1];
                var chNum = character.mapSectionNumber;
                var chNewPos = chNum + diceResult;
                var charMoved = false;

                for (int i = 0; i < playableMap.Length; i++)
                {
                    for (int j = 0; j < playableMap[i].Length; j++)
                    {
                        if (playableMap[i][j].Number == chNewPos)
                        {
                            if (i == playableMap.Length - 1 && j == playableMap[i].Length - 1)
                            {
                                sb.AppendLine($"{character.Name} wins the game by reaching the final first!");
                                UpdateCharacterPositionInDb(character, playableMap.Length - 1, playableMap.Length - 1, playableMap[i][j].Number);
                                hasReachedFinalSpot = true;
                                charMoved = true;
                                break;
                            }

                            var positionNumber = playableMap[i][j].Number;
                            UpdateCharacterPositionInDb(character, i, j, positionNumber);
                            //TODO - add clauses to check if it is a special square and what actions should
                            //be taken in that case

                            Console.WriteLine($"{character.Name} successfully moved to square number {chNewPos}");
                            charMoved = true;
                        }
                        else if (chNewPos > playableMap[playableMap.Length-1][playableMap.Length-1].Number)
                        {
                            Console.WriteLine("Better luck next time, you can't go further than the final :)");
                            charMoved = true;
                        }
                    }

                    if (charMoved)
                    {
                        break;
                    }
                }

                if (playerInTurn != characters.Count)
                {
                    playerInTurn++;
                }
                else
                {
                    playerInTurn = 1;
                }
            }
        }

        private void UpdateCharacterPositionInDb(Character character, int i, int j, int positionNumber)
        {
            var dbChar = context.GameCharacters
                .FirstOrDefault(c => c.CharacterId == character.Id)
                .Character;
            dbChar.X = i;
            dbChar.Y = j;
            dbChar.mapSectionNumber = positionNumber;
            context.Characters.Update(dbChar);
            context.SaveChanges();
        }
    }
}
