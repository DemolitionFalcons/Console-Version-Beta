namespace DemolitionFalcons.App.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DemolitionFalcons.Data.Support;
    using DemolitionFalcons.Data.DataInterfaces;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Data.IO;

    public class Engine : IEngine
    {
        private readonly IManager gameManager;
        private readonly DemolitionFalconsDbContext context;
        private readonly ICommandEngine<ICommand> commandEngine;
        private readonly IInputReader reader;
        private readonly IOutputWriter writer;


        public Engine(DemolitionFalconsDbContext context)
        {

            this.context = context;
            this.commandEngine = new CommandEngine<ICommand>();
            this.reader = new InputReader();
            this.writer = new OutputWriter();
            this.gameManager = new GameManager(context, writer, reader); ;
        }


        public void Run()
        {


            //SetUpDatabase.ResetDB(context);

            ShowCommandsExample();
            ProcessCommandFromUser();
        }

        private void ProcessCommandFromUser()
        {
            writer.WriteLine("Type command: )");

            bool isRunning = true;

            while (isRunning)
            {

                List<string> commandInput = reader.ReadLine().Split().ToList();

                try
                {

                    ICommand command = this.commandEngine.ExecuteCommand(commandInput);

                    command.Execute(gameManager, writer, commandInput);

                }
                catch (Exception e)
                {
                    writer.WriteLine(e.Message);

                }
            }

        }

        private void ShowCommandsExample()
        {
            StringBuilder sb = new StringBuilder();
            //We can make option to add consumables too
            sb.AppendLine("Welcome to the test console version of Demolition Falcons!");
            sb.AppendLine("We hope you have a lot of good and fun experience with our new game.");
            sb.AppendLine("The game here will be completed by typing commands in the console.");
            sb.AppendLine("So, in order to play the game you should know some basic commands:");
            sb.AppendLine("In order to begin you professional experience in the fighting environment, you must first create a characcter!");
            sb.AppendLine(">register -> go on to register a user");
            sb.AppendLine(">create {Name} -> you will be send further to edit the info of the character you're up to create with the given name");
            sb.AppendLine(">addRoom {Name} -> you will be send further to create a playing room");
            sb.AppendLine(">joinRoom -> choose from a list of all currently available rooms");
            sb.AppendLine(">startgame -> choose a game from the list of currently available to start games");
            sb.AppendLine(">createCharacter {Name} -> adds a character");
            sb.AppendLine(">inspect Character -> get overall info about your character");
            sb.AppendLine(">delete Character -> delete a specified character ");
            sb.AppendLine(">reset Database -> (FOR TESTING PURPOSE)resets the database");
            sb.AppendLine(">help -> you'll be shown the list with commands once again");
            sb.AppendLine(">quit -> quit the game / and lose everything simply because we don't have DB yet :D /");

            writer.WriteLine(sb.ToString());
        }
    }
}
