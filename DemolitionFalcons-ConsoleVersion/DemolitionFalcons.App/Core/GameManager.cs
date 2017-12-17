namespace DemolitionFalcons.App.Core
{
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GameManager : IManager
    {
        // Most of the methods that WILL be added here later must work with the database or with DTO (Data Transfer Objects)
        public int charactersCreated;
        private DemolitionFalconsDbContext context;

        public GameManager(DemolitionFalconsDbContext context)
        {
            this.context = context;
        }

        public string AddRoom(IList<string> arguments)// or AddGame
        {
            var gameName = arguments[0];

            var game = new Game
            {
                Name = gameName,
                Money = 200,//by default
                Xp = 50
            };

            context.Games.Add(game);
            context.SaveChanges();
            return "Room created";
        }

        public string CreateCharacter(IList<string> arguments)
        {
            throw new NotImplementedException();
        }

        public string DeleteCharacter(IList<string> arguments)
        {
            throw new NotImplementedException();
        }

        public string Help(IList<string> arguments)
        {
            StringBuilder sb = new StringBuilder();
            //We can make option to add consumables too
            sb.AppendLine("The game here is completed by typing commands in the console.");
            sb.AppendLine("Here are the basic commands:");
            sb.AppendLine(">Create {Name} -> you will be send further to edit the info of the character you're up to create with the given name");
            sb.AppendLine(">AddRoom {Name} -> you will be send further to create a playing room");
            sb.AppendLine(">Join Room -> choose from a list of all currently available rooms");
            sb.AppendLine(">Inspect Character -> get overall info about your character");
            sb.AppendLine(">Delete Character -> delete a specified character ");
            sb.AppendLine(">Help -> you'll be shown the list with commands once again");
            sb.AppendLine(">Quit -> quit the game / and lose everything simply because we don't have DB yet :D /");

            return sb.ToString();
        }

        public string InspectCharacter(IList<string> arguments)
        {
            throw new NotImplementedException();
        }

        public string JoinRoom(IList<string> arguments)
        {
            throw new NotImplementedException();
        }

        public string Quit(IList<string> arguments)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Thanks for playing Demolition Falcons " + "\u00a9");
            sb.AppendLine($"See you soon.");

            return sb.ToString();
        }
    }
}
