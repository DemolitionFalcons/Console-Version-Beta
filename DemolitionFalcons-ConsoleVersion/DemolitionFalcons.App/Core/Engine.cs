namespace DemolitionFalcons.App.Core
{
    using DemolitionFalcons.App.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private GameManager gameManager;

        public Engine()
        {
            this.gameManager = new GameManager();
        }

        public void Run()
        {
            bool isRunning = true;

            SetUpDatabase();
            ShowCommandsExample();
            Console.WriteLine("Type command: )");
            while (isRunning)
            {
                string input = Console.ReadLine();
                //while (!input.StartsWith("Create"))
                //{
                //    Console.WriteLine("Please first create your fighter :)");
                //    input = Console.ReadLine();
                //}
                List<string> arguments = this.ParseInput(input);
                Console.WriteLine(this.ProcessInput(arguments));
                isRunning = !this.ShouldEnd(input);
            }
        }

        private string ProcessInput(List<string> arguments)
        {
            string command = arguments[0];
            arguments.RemoveAt(0);

            if (command != "Create" && gameManager.charactersCreated == 0 && command != "Help" && command != "Quit")
            {
                return "In order to proceed further in the features of the game, you must first create a character!";
            }
            Type commandType = Type.GetType("DemolitionFalcons.App.Commands" + "." + command + "Command");
            var constructor = commandType.GetConstructor(new Type[] { typeof(IList<string>), typeof(IManager) });
            ICommand cmd = (ICommand)constructor.Invoke(new object[] { arguments, this.gameManager });
            return cmd.Execute();
        }

        private List<string> ParseInput(string input)
        {
            return input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        private bool ShouldEnd(string input)
        {
            return input.Equals("Quit");
        }

        private void SetUpDatabase()
        {
            // this method will create new db(if there ain't one) or load an existing one when the game starts
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
            sb.AppendLine(">Create {Name} -> you will be send further to edit the info of the character you're up to create with the given name");
            sb.AppendLine(">Add Room -> you will be send further to create a playing room");
            sb.AppendLine(">Join Room -> choose from a list of all currently available rooms");
            sb.AppendLine(">Inspect Character -> get overall info about your character");
            sb.AppendLine(">Delete Character -> delete a specified character ");
            sb.AppendLine(">Help -> you'll be shown the list with commands once again");
            sb.AppendLine(">Quit -> quit the game / and lose everything simply because we don't have DB yet :D /");

            Console.WriteLine(sb.ToString());
        }
    }
}
