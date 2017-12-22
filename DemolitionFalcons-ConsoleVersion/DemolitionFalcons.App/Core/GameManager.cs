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
            string roomName = arguments[0];

            //TO DO


            return $"Successfully";
        }

        public string Quit()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Thanks for playing Demolition Falcons " + "\u00a9");
            sb.AppendLine($"See you soon.");

            return sb.ToString().Trim();
        }
    }
}
