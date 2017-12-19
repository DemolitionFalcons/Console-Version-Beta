namespace DemolitionFalcons.App.Core
{
  
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text; 
    using DemolitionFalcons.Data.DataInterfaces;
    using DemolitionFalcons.Data.IO;
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data;

    public class Engine : IEngine
    {
        private IManager gameManager;
        private DemolitionFalconsDbContext context;
        private ICommandEngine<ICommand> commandEngine;
        private IInputReader reader;
        private IOutputWriter writer;


        public Engine(IInputReader reader, IOutputWriter writer, ICommandEngine<ICommand> commandEngine)
        {
            //connects to the db
            this.context = new DemolitionFalconsDbContext();

            this.gameManager = new GameManager(context);
            this.commandEngine = new CommandEngine<ICommand>();
            this.reader = new InputReader();
            this.writer = new OutputWriter();
        }


        public void Run()
        {


            //SetUpDatabase.CreateDataBase(context);

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

                    command.Execute(gameManager,writer, commandInput);

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
            sb.AppendLine(">Register -> go on to register a user");
            sb.AppendLine(">Create {Name} -> you will be send further to edit the info of the character you're up to create with the given name");
            sb.AppendLine(">AddRoom {Name} -> you will be send further to create a playing room");
            sb.AppendLine(">JoinRoom -> choose from a list of all currently available rooms");
            sb.AppendLine(">CreateCharacter {Name} -> adds a character");
            sb.AppendLine(">Inspect Character -> get overall info about your character");
            sb.AppendLine(">Delete Character -> delete a specified character ");
            sb.AppendLine(">Reset Database -> (FOR TESTING PURPOSE)resets the database");
            sb.AppendLine(">Help -> you'll be shown the list with commands once again");
            sb.AppendLine(">Quit -> quit the game / and lose everything simply because we don't have DB yet :D /");

            writer.WriteLine(sb.ToString());
        }
    }
}
