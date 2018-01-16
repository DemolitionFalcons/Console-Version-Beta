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
                GameCharacter gc = new GameCharacter
                {
                    Character = characters[characterNumber - 1],
                    CharacterId = characters[characterNumber - 1].Id,
                    Game = rooms[roomNumber - 1],
                    GameId = rooms[roomNumber - 1].Id,
                    PlayerId = player.Id
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

                HaveFun(room, sb, preferredMap);

            }
            catch (Exception ex)
            {

                return ex.Message;
            }

            return sb.ToString().TrimEnd();
        }

        private void HaveFun(Game room, StringBuilder sb, string preferredMap)
        {
            int roomId = room.Id;

            //Create dice
            DiceDto dice = new DiceDto();

            //Add Characters to the room
            var game = context.Games.FirstOrDefault(g => g.Id == room.Id);
            var characters = new List<Character>();
            foreach (var gc in game.Characters)
            {
                var character = gc.Character;
                characters.Add(character);
            }


            if (preferredMap == "demomap")
            {
                var map = new DemoMap("map1");
                var playableMap = map.GenerateMap();
                SetOnStart(room, playableMap, roomId);// set all chars on start
                ProceedGame(game, sb, playableMap, characters, dice, roomId);
            }
            else if (preferredMap == "firstmap")
            {
                var firstMap = new FirstMapFrontEnd();
                var generatedFirstMap = firstMap.GenerateFirstMap();
                SetOnStart(room, generatedFirstMap, roomId);// set all chars on start
                ProceedGame(game, sb, generatedFirstMap, characters, dice, roomId);
            }

        }


        private void CheckIfSpecialSquare(MapSection[][] map, int i, int j, int positionNumber, Character character, int roomId)
        {
            //Character moves back by 3 positions if he is on GoBackSquare
            if (map[i][j].isGoBackSquare)
            {
                Console.WriteLine("Oops, it seems you stopped on a special square...");
                positionNumber -= 3;
                if (j >= 3)
                {
                    if (j == 3)
                    {
                        j = 0;
                    }
                    else
                    {
                        j -= 3;
                    }

                }
                else
                {
                    var toTakeDown = 3 - j;
                    if (i == 0)
                    {
                        throw new ArgumentException($"Cannot move back from {i}{j}");
                    }
                    else

                    {
                        i -= 1;
                        j = map[i].Length - 1;
                        toTakeDown--;
                        j -= toTakeDown;
                    }
                }
                positionNumber = map[i][j].Number;
                UpdateCharacterPositionInDb(character, map[i][j].X, map[i][j].Y, positionNumber, roomId);
                Console.WriteLine($"{character.Name} moves back by 3 positions to square number {positionNumber} :)");
                CheckIfSpecialSquare(map, i, j, positionNumber, character, roomId);
            }
            //Character goes forward by 3 positions if he is on GoBackSquare
            else if (map[i][j].isGoForwardSquare)
            {

                Console.WriteLine("Oops, it seems you stopped on a special square...");
                positionNumber += 3;
                if (j <= 6)
                {
                    j += 3;
                }
                else //if(j != 9)
                {
                    var thisRowAdd = 9 - j;
                    var nextRowAdd = 3 - thisRowAdd;
                    if (i == map.Length - 1)
                    {
                        throw new ArgumentException($"Cannot move forward from {i}{j}");
                    }
                    else
                    {
                        i += 1;
                        j = 0;
                        if (j == 0)
                        {
                            nextRowAdd--;
                        }
                        j += nextRowAdd;
                    }

                }
                positionNumber = map[i][j].Number;
                UpdateCharacterPositionInDb(character, map[i][j].X, map[i][j].Y, positionNumber, roomId);
                Console.WriteLine($"{character.Name} moves forward with 3 positions to square number {positionNumber} ^.^");
                CheckIfSpecialSquare(map, i, j, positionNumber, character, roomId);
            }
            //ToDo
            else if (map[i][j].isMysterySquare)
            {
                Console.WriteLine("Oops, it seems you stopped on a special square...");

                var num = numberGenerator.GenerateNumber(1, 3);
                if (num == 1)
                {
                    //play mini game
                    //Can be found in Miscellaneous/SpecialSquares/MysterySquare/MiniGameAction.cs
                    MiniGameAction msa = new MiniGameAction();
                    msa.PlayMiniGame();
                    if (msa.DemolitionFalcons)
                    {
                        //All characters return to the first square
                        var characters = context.GameCharacters.Where(g => g.GameId == roomId).ToList();
                        foreach (var charche in characters)
                        {
                            context.GameCharacters.FirstOrDefault(c => c.CharacterId == charche.CharacterId).CharacterPositionX = 0;
                            context.GameCharacters.FirstOrDefault(c => c.CharacterId == charche.CharacterId).CharacterPositionY = 0;
                            context.GameCharacters.FirstOrDefault(c => c.CharacterId == charche.CharacterId).MapSectionNumber = 1;
                            context.SaveChanges();
                        }
                    }
                    else if (msa.GoBack)
                    {
                        var toGoBackWith = msa.GoBackWith;
                        positionNumber -= toGoBackWith;
                        if (j >= toGoBackWith)
                        {
                            if (j == toGoBackWith)
                            {
                                j = 0;
                            }
                            else
                            {
                                j -= toGoBackWith;
                            }

                        }
                        else
                        {
                            var toTakeDown = toGoBackWith - j;
                            if (i == 0)
                            {
                                throw new ArgumentException($"Cannot move back from {i}{j}");
                            }
                            else

                            {
                                i -= 1;
                                j = map[i].Length - 1;
                                toTakeDown--;
                                j -= toTakeDown;
                            }
                        }
                        positionNumber = map[i][j].Number;
                        UpdateCharacterPositionInDb(character, map[i][j].X, map[i][j].Y, positionNumber, roomId);
                        Console.WriteLine($"{character.Name} moves back by {toGoBackWith} positions to square number {positionNumber} :)");
                        CheckIfSpecialSquare(map, i, j, positionNumber, character, roomId);
                    }
                    else if (msa.MoveForward)
                    {
                        var toMoveForwardWith = msa.MoveForwardWith;

                        positionNumber += toMoveForwardWith;
                        if (j < map[i].Length - toMoveForwardWith)
                        {
                            j += toMoveForwardWith;
                        }
                        else //if(j != 9)
                        {
                            var thisRowAdd = 9 - j;
                            var nextRowAdd = toMoveForwardWith - thisRowAdd;
                            if (i == map.Length - 1)
                            {
                                throw new ArgumentException($"Cannot move forward from {i}{j}");
                            }
                            else
                            {
                                i += 1;
                                j = 0;
                                if (j == 0)
                                {
                                    nextRowAdd--;
                                }
                                j += nextRowAdd;
                            }

                        }
                        positionNumber = map[i][j].Number;
                        UpdateCharacterPositionInDb(character, map[i][j].X, map[i][j].Y, positionNumber, roomId);
                        Console.WriteLine($"{character.Name} moves forward with {toMoveForwardWith} positions to square number {positionNumber} ^.^");
                        CheckIfSpecialSquare(map, i, j, positionNumber, character, roomId);

                    }
                }
                else
                {
                    var demoMatrixRows = 3;
                    var demoMatrixCols = 3;
                    var firstNumTyped = 0;
                    char[][] demoMatrix = new char[3][];
                    var isSecondChance = false;

                    var counter = 1;


                    for (int row = 0; row < demoMatrixRows; row++)
                    {
                        demoMatrix[row] = new char[demoMatrixCols];
                        for (int col = 0; col < demoMatrixCols; col++)
                        {
                            demoMatrix[row][col] = counter.ToString()[0];
                            counter++;
                        }
                    }

                    //play double chance
                    Console.WriteLine("Welcome to the Double Chance game! You have 9 hidden sayings. Choose one!");
                    var doubleChanceGame = new DoubleChance();
                    var matrix = doubleChanceGame.StartDoubleChance();

                    Start:
                    var isNumeric = int.TryParse(Console.ReadLine(), out int numTyped);
                    while (numTyped < 1 || numTyped > 9 || numTyped == firstNumTyped || !isNumeric)
                    {
                        if (firstNumTyped != 0)
                        {
                            Console.WriteLine($"Type a number from 1 to 9 which is different from {firstNumTyped}.");
                        }
                        Console.WriteLine("Type a number from 1 to 9");
                        isNumeric = int.TryParse(Console.ReadLine(), out numTyped);
                    }

                    char letter;

                    if (numTyped >= 1 && numTyped <= 3)
                    {
                        letter = matrix[0][numTyped - 1];
                        demoMatrix[0][numTyped - 1] = letter;
                    }
                    else if (numTyped <= 6)
                    {
                        letter = matrix[1][numTyped - 4];
                        demoMatrix[1][numTyped - 4] = letter;
                    }
                    else
                    {
                        letter = matrix[2][numTyped - 7];
                        demoMatrix[2][numTyped - 7] = letter;
                    }

                    foreach (var row in demoMatrix)
                    {
                        Console.WriteLine(string.Join("{0}",
                            $"[{string.Join(" | ", row)}]"));
                    }

                    if (letter == 'S')
                    {
                        if (!isSecondChance)
                        {
                            firstNumTyped = numTyped;
                            BonusSquareAction bsa = new BonusSquareAction(context, roomId, character);
                            //Gets a random spell drawn with a special algorythm that allow the character to atack another character
                            var spell = bsa.RandomSpell();

                            Console.WriteLine($"Congrats you have received a new spell -> {spell.Name}. If you want to keep it type 'Y' and if you want a second chance type 'N'");

                            var response = Console.ReadLine();
                            while (response != "Y" && response != "N")
                            {
                                Console.WriteLine("Type 'Y' or 'N'");
                                response = Console.ReadLine();
                            }

                            if (response == "N")
                            {
                                isSecondChance = true;

                                foreach (var row in demoMatrix)
                                {
                                    Console.WriteLine(string.Join("{0}",
                                        $"[{string.Join(" | ", row)}]"));
                                }

                                goto Start;
                            }
                            else
                            {
                                bsa.GetSpell(spell.Name);
                            }

                        }
                        else
                        {
                            Console.WriteLine("This was your second shot! Congrats you win a spell!");

                            BonusSquareAction bsa = new BonusSquareAction(context, roomId, character);
                            bsa.GetSpell("");
                        }
                    }
                    else if (letter == 'F')
                    {
                        if (!isSecondChance)
                        {
                            num = numberGenerator.GenerateNumber(2, 6);
                            var toMoveForwardWith = num;

                            Console.WriteLine($"Congrats you can move {toMoveForwardWith} spaces forward if you wish. If so type 'Y' and if you want a second chance type 'N'");

                            var response = Console.ReadLine();
                            while (response != "Y" && response != "N")
                            {
                                Console.WriteLine("Type 'Y' or 'N'");
                                response = Console.ReadLine();
                            }

                            if (response == "N")
                            {
                                isSecondChance = true;

                                foreach (var row in demoMatrix)
                                {
                                    Console.WriteLine(string.Join("{0}",
                                        $"[{string.Join(" | ", row)}]"));
                                }

                                goto Start;

                            }
                            else
                            {
                                MoveForwardWith(toMoveForwardWith, positionNumber, map, i, j, roomId, character);
                            }
                        }
                        else// is secondChance
                        {
                            num = numberGenerator.GenerateNumber(2, 6);
                            var toMoveForwardWith = num;

                            Console.WriteLine($"Congrats, this was your second shot. You can now move forward with {toMoveForwardWith} places if possible");

                            MoveForwardWith(toMoveForwardWith, positionNumber, map, i , j, roomId, character);
                        }
                    }
                    else if (letter == 'B')
                    {
                        var toGoBackWith = numberGenerator.GenerateNumber(3, 8);

                        if (!isSecondChance)
                        {
                            Console.WriteLine($"Sadly you have to move {toGoBackWith} places backwards if possible. You still have a second chance. If you want to go back type 'Y' or type 'N' for a second shot.");

                            var response = Console.ReadLine();
                            while (response != "Y" && response != "N")
                            {
                                Console.WriteLine("Type 'Y' if you want to go back and 'N' for a second try.");
                                response = Console.ReadLine();
                            }

                            if (response == "Y")
                            {
                                GoBackWith(toGoBackWith, positionNumber, map, i, j, roomId, character);
                            }
                            else
                            {
                                isSecondChance = true;

                                foreach (var row in demoMatrix)
                                {
                                    Console.WriteLine(string.Join("{0}",
                                        $"[{string.Join(" | ", row)}]"));
                                }

                                goto Start;
                            }
                        }
                        else
                        {
                            num = numberGenerator.GenerateNumber(2, 6);
                            var toMoveBackWith = num;

                            Console.WriteLine($"Sorry this was your second shot. You will move with {toMoveBackWith} places backwards if possible.");

                            GoBackWith(toMoveBackWith, positionNumber, map, i, j, roomId, character);
                        }
                        
                    }

                }


            }
            //ToDo
            else if (map[i][j].isBonusSquare)
            {

                Console.WriteLine("Oops, it seems you stopped on a special square...");
                Console.WriteLine($"{character.Name} is on a bonus square which logic is due to be implemented soon :)");
                BonusSquareAction bsa = new BonusSquareAction(context, roomId, character);
                //Gets a random spell drawn with a special algorythm that allow the character to atack another character
                bsa.GetSpell("");
            }
        }

        private void GoBackWith(int toGoBackWith, int positionNumber, MapSection[][] map, int i, int j, int roomId, Character character)
        {
            positionNumber -= toGoBackWith;
            if (j >= toGoBackWith)
            {
                if (j == toGoBackWith)
                {
                    j = 0;
                }
                else
                {
                    j -= toGoBackWith;
                }

            }
            else
            {
                var toTakeDown = toGoBackWith - j;
                if (i == 0)
                {
                    throw new ArgumentException($"Cannot move back from {i}{j}");
                }
                else

                {
                    i -= 1;
                    j = map[i].Length - 1;
                    toTakeDown--;
                    j -= toTakeDown;
                }
            }

            positionNumber = map[i][j].Number;
            UpdateCharacterPositionInDb(character, map[i][j].X, map[i][j].Y, positionNumber, roomId);
            Console.WriteLine($"{character.Name} moves back by {toGoBackWith} positions to square number {positionNumber} :)");
            CheckIfSpecialSquare(map, i, j, positionNumber, character, roomId);
        }

        private void MoveForwardWith(int toMoveForwardWith, int positionNumber, MapSection[][] map, int i, int j, int roomId, Character character)
        {
            positionNumber += toMoveForwardWith;
            if (j < map[i].Length - toMoveForwardWith)
            {
                j += toMoveForwardWith;
            }
            else //if(j != 9)
            {
                var thisRowAdd = 9 - j;
                var nextRowAdd = toMoveForwardWith - thisRowAdd;
                if (i == map.Length - 1)
                {
                    throw new ArgumentException($"Cannot move forward from {i}{j}");
                }
                else
                {
                    i += 1;
                    j = 0;
                    if (j == 0)
                    {
                        nextRowAdd--;
                    }
                    j += nextRowAdd;
                }
            }


            positionNumber = map[i][j].Number;
            UpdateCharacterPositionInDb(character, map[i][j].X, map[i][j].Y, positionNumber, roomId);
            Console.WriteLine($"{character.Name} moves forward with {toMoveForwardWith} positions to square number {positionNumber} ^.^");
            CheckIfSpecialSquare(map, i, j, positionNumber, character, roomId);
        }

        private void ProceedGame(Game game, StringBuilder sb, MapSection[][] firstMap, List<Character> characters, DiceDto dice, int roomId)
        {
            bool hasReachedFinalSpot = false;

            var playerInTurn = 1;

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
                var chNum = context.GameCharacters.FirstOrDefault(c => c.CharacterId == character.Id && c.GameId == roomId)
                    .MapSectionNumber;
                var chNewPos = chNum + diceResult;
                var charMoved = false;

                for (int i = 0; i < firstMap.Length; i++)
                {
                    for (int j = 0; j < firstMap[i].Length; j++)
                    {
                        if (firstMap[i][j].Number == chNewPos)
                        {
                            if (i == firstMap.Length - 1 && j == firstMap[i].Length - 1)
                            {
                                sb.AppendLine($"{character.Name} wins the game by reaching the final first!");

                                //add money, xp and winrate for winner


                                AddWinnerStats(roomId, character.Id, game);
                                AddGamesPlayedForPlayers(roomId);
                                UpdateCharacterPositionInDb(character, firstMap[i][j].X, firstMap[i][j].Y, firstMap[i][j].Number, roomId);

                                hasReachedFinalSpot = true;
                                charMoved = true;
                                break;
                            }

                            var positionNumber = firstMap[i][j].Number;
                            UpdateCharacterPositionInDb(character, firstMap[i][j].X, firstMap[i][j].Y, positionNumber, roomId);
                            Console.WriteLine($"{character.Name} successfully moved to square number {chNewPos}");
                            //TODO - add clauses to check if it is a special square and what actions should
                            //be taken in that case
                            try
                            {
                                CheckIfSpecialSquare(firstMap, i, j, positionNumber, character, roomId);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            charMoved = true;
                        }
                        else if (chNewPos > firstMap[firstMap.Length - 1][firstMap[0].Length - 1].Number)
                        {
                            Console.WriteLine("Better luck next time, you can't go further than the final :)");
                            charMoved = true;
                            break;
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

        private void AddGamesPlayedForPlayers(int roomId)
        {
            var gameCharPlayers = context.GameCharacters.Where(gc => gc.GameId == roomId).ToList();
            var players = context.Players.ToList();

            foreach (var playerInGame in players)
            {
                if (gameCharPlayers.Any(p => p.PlayerId == playerInGame.Id))
                {
                    playerInGame.GamesPlayed++;
                }
            }
        }

        private void AddWinnerStats(int roomId, int characterId, Game game)
        {
            var playerChar = context.GameCharacters.FirstOrDefault(gc => gc.GameId == roomId && gc.CharacterId == characterId);
            var player = context.Players.FirstOrDefault(x => x.Id == playerChar.PlayerId);
            player.Money += game.Money;
            player.Wins++;
            player.Xp += game.Xp;
        }

        private void SetOnStart(Game room, MapSection[][] firstMap, int roomId)
        {
            var toBreak = false;
            for (int i = 0; i < firstMap.Length; i++)
            {
                for (int j = 0; j < firstMap[i].Length; j++)
                {
                    if (firstMap[i][j].Number == 1)
                    {
                        var positionNumber = firstMap[i][j].Number;
                        foreach (var chare in room.Characters)
                        {
                            var character = chare.Character;
                            UpdateCharacterPositionInDb(character, firstMap[0][0].X, firstMap[0][0].Y, positionNumber, roomId);

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

        }

        private void UpdateCharacterPositionInDb(Character character, int X, int Y, int positionNumber, int roomId)
        {
            var dbChar = context.GameCharacters
                .FirstOrDefault(c => c.CharacterId == character.Id && c.GameId == roomId);
            dbChar.CharacterPositionX = X;
            dbChar.CharacterPositionY = Y;
            dbChar.MapSectionNumber = positionNumber;
            context.GameCharacters.Update(dbChar);
            context.SaveChanges();
        }
    }
}
