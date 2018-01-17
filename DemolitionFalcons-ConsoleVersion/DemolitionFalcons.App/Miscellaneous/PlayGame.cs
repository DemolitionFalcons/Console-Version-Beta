namespace DemolitionFalcons.App.Miscellaneous
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DemolitionFalcons.App.Core.DTOs;
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.App.Miscellaneous.SpecialSquares;
    using DemolitionFalcons.App.Miscellaneous.SpecialSquares.MysterySquare;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Data.DataInterfaces;
    using DemolitionFalcons.Models;

    public class PlayGame
    {
        private DemolitionFalconsDbContext context;
        private IInputReader reader;
        private IOutputWriter writer;
        private NumberGenerator numberGenerator;

        public PlayGame(DemolitionFalconsDbContext context, IInputReader reader, IOutputWriter writer, NumberGenerator numberGenerator)
        {
            this.context = context;
            this.reader = reader;
            this.writer = writer;
            this.numberGenerator = numberGenerator;
        }

        public void HaveFun(Game room, StringBuilder sb, string preferredMap)
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

                            MoveForwardWith(toMoveForwardWith, positionNumber, map, i, j, roomId, character);
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
                Console.WriteLine($"{character.Name} is on a bonus square :)");
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
