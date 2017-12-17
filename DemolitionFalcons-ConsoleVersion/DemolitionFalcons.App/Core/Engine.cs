namespace DemolitionFalcons.App.Core
{
    using DemolitionFalcons.App.Interfaces;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Engine
    {
        private GameManager gameManager;
        private DemolitionFalconsDbContext context;

        public Engine()
        {
            this.gameManager = new GameManager();
            //connects to the db
            this.context = new DemolitionFalconsDbContext();
            SetUpDatabase();
            //tries to connect to the db
            Seed(context);
        }

        private void Seed(DemolitionFalconsDbContext context)
        {
            //Add just 2 basic weapons with same stats in order to test
            //If you want you're free to add more
            if (!context.Weapons.Any())
            {
                var weapons = new List<Weapon>();
                var akModel = new Weapon();

                akModel.Name = "AK-47";
                akModel.ClipSize = 30;
                akModel.TotalCapacity = 150;
                akModel.Damage = 50;

                weapons.Add(akModel);

                var m4a1s = new Weapon();

                m4a1s.Name = "M4A1-S";
                m4a1s.ClipSize = 30;
                m4a1s.TotalCapacity = 150;
                m4a1s.Damage = 50;

                weapons.Add(m4a1s);

                context.Weapons.AddRange(weapons);
                context.SaveChanges();
            }

            //Add a bot player in order to test if all is correct
            //Add weapons to him 
            //Add  a playerWeapon connection
            if (!context.Players.Any())
            {
                var bot = new Player
                {
                    Username = "Bot",
                    Password = "bot123",
                    GamesPlayed = 0,
                    Wins = 0
                };

                context.Players.Add(bot);
                context.SaveChanges();

                var weapons = new List<Weapon>
                {
                    context.Weapons.Where(w => w.Name == "AK-47").FirstOrDefault(),
                    context.Weapons.Where(w => w.Name == "M4A1-S").FirstOrDefault()
                };
                foreach (var weapon in weapons)
                {
                    var playerWeapon = new PlayerWeapon();
                    playerWeapon.Player = bot;
                    playerWeapon.PlayerId = bot.Id;
                    playerWeapon.Weapon = weapon;
                    playerWeapon.WeaponId = weapon.Id;
                    context.PlayerWeapons.Add(playerWeapon);
                    context.SaveChanges();
                }

            }
        }

        public void Run()
        {
            bool isRunning = true;

            //SetUpDatabase();
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

            if (command == "Reset")
            {
                return ResetDatabase(context);
            }
            if (command != "Create" && gameManager.charactersCreated == 0 && command != "Help" && command != "Quit")
            {
                return "In order to proceed further in the features of the game, you must first create a character!";
            }
            Type commandType = Type.GetType("DemolitionFalcons.App.Commands" + "." + command + "Command");
            var constructor = commandType.GetConstructor(new Type[] { typeof(IList<string>), typeof(IManager) });
            ICommand cmd = (ICommand)constructor.Invoke(new object[] { arguments, this.gameManager });
            return cmd.Execute();
        }

        private string ResetDatabase(DemolitionFalconsDbContext contextArg)
        {
            try
            {
                contextArg.Database.EnsureDeleted();
                contextArg.Database.EnsureCreated();
                Seed(context);
            }
            catch (Exception ex)
            {
                return "Execution Failed. Database was not reset.";
            }

            return "Database Reset!";
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
            
            context.Database.EnsureCreated();

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
            sb.AppendLine(">Reset Database -> (FOR TESTING PURPOSE)resets the database");
            sb.AppendLine(">Help -> you'll be shown the list with commands once again");
            sb.AppendLine(">Quit -> quit the game / and lose everything simply because we don't have DB yet :D /");

            Console.WriteLine(sb.ToString());
        }
    }
}
