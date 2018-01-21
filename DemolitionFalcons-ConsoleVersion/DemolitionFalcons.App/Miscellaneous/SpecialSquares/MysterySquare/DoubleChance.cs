namespace DemolitionFalcons.App.Miscellaneous.SpecialSquares.MysterySquare
{
    using DemolitionFalcons.App.Maps;
    using DemolitionFalcons.Data;
    using DemolitionFalcons.Data.DataInterfaces;
    using DemolitionFalcons.Models;
    using System;
    using System.Linq;

    public class DoubleChance
    {
        const char spellLetter = 'S';
        const char goBackLetter = 'B';
        const char goForwardLetter = 'F';
        const char notFilledLetter = 'N';
        bool isGoBackLetFilled = false;
        bool isGoForwardLetFilled = false;
        bool isSpellLetFilled = false;
        private NumberGenerator numberGenerator;

        public DoubleChance()
        {
            this.numberGenerator = new NumberGenerator();
        }

        public void StartDoubleChance(DemolitionFalconsDbContext context, int roomId, Character character, int positionNumber, MapSection[][] map, int i, int j, bool isSingle)
        {
            var demoMatrixRows = 3;
            var demoMatrixCols = 3;
            var firstNumTyped = 0;
            char[][] demoMatrix = new char[3][];
            var isSecondChance = false;
            int num;
            var playGame = new PlayGame(context, numberGenerator);

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
            var matrix = doubleChanceGame.LoadDoubleChance();

            Start:
            int numTyped;
            if (isSingle)
            {
                numTyped = numberGenerator.GenerateNumber(1, 10);
                while (numTyped == firstNumTyped)
                {
                    numTyped = numberGenerator.GenerateNumber(1, 10);
                }
                
                goto SinglePlayer;
            }

            var isNumeric = int.TryParse(Console.ReadLine(), out numTyped);
            while (numTyped < 1 || numTyped > 9 || numTyped == firstNumTyped || !isNumeric)
            {
                if (firstNumTyped != 0)
                {
                    Console.WriteLine($"Type a number from 1 to 9 which is different from {firstNumTyped}.");
                }
                Console.WriteLine("Type a number from 1 to 9");
                isNumeric = int.TryParse(Console.ReadLine(), out numTyped);
            }

            SinglePlayer:
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

                    string response;
                    if (isSingle)
                    {
                        response = TakeOrNot();
                    }
                    else
                    {
                        response = Console.ReadLine();
                        while (response != "Y" && response != "N")
                        {
                            Console.WriteLine("Type 'Y' or 'N'");
                            response = Console.ReadLine();
                        }
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

                    string response;
                    if (isSingle)
                    {
                        response = TakeOrNot();
                    }
                    else
                    {
                        response = Console.ReadLine();
                        while (response != "Y" && response != "N")
                        {
                            Console.WriteLine("Type 'Y' or 'N'");
                            response = Console.ReadLine();
                        }
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
                        playGame.MoveForwardWith(toMoveForwardWith, positionNumber, map, i, j, roomId, character);
                    }
                }
                else// is secondChance
                {
                    num = numberGenerator.GenerateNumber(2, 6);
                    var toMoveForwardWith = num;

                    Console.WriteLine($"Congrats, this was your second shot. You can now move forward with {toMoveForwardWith} places if possible");

                    playGame.MoveForwardWith(toMoveForwardWith, positionNumber, map, i, j, roomId, character);
                }
            }
            else if (letter == 'B')
            {
                var toGoBackWith = numberGenerator.GenerateNumber(3, 8);

                if (!isSecondChance)
                {
                    Console.WriteLine($"Sadly you have to move {toGoBackWith} places backwards if possible. You still have a second chance. If you want to go back type 'Y' or type 'N' for a second shot.");

                    string response;
                    if (isSingle)
                    {
                        response = TakeOrNot();
                    }
                    else
                    {
                        response = Console.ReadLine();
                        while (response != "Y" && response != "N")
                        {
                            Console.WriteLine("Type 'Y' if you want to go back and 'N' for a second try.");
                            response = Console.ReadLine();
                        }
                    }


                    if (response == "Y")
                    {
                        playGame.GoBackWith(toGoBackWith, positionNumber, map, i, j, roomId, character);
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

                    playGame.GoBackWith(toMoveBackWith, positionNumber, map, i, j, roomId, character);
                }

            }
        }

        private string TakeOrNot()
        {
            var num = numberGenerator.GenerateNumber(1, 3);
            if (num == 1)
            {
                return "Y";
            }
            return "N";
        }

        public char[][] LoadDoubleChance()
        {
            const int matrixRows = 3;
            const int matrixCols = 3;

            char[][] matrix = new char[3][];

            var counter = 1;


            for (int row = 0; row < matrixRows; row++)
            {
                matrix[row] = new char[matrixCols];
                for (int col = 0; col < matrixCols; col++)
                {
                    matrix[row][col] = counter.ToString()[0];
                    counter++;
                }
            }

            foreach (var row in matrix)
            {
                Console.WriteLine(string.Join("{0}",
                    $"[{string.Join(" | ", row)}]"));
            }

            matrix = FillMatrix(matrix);

            return matrix;
        }

        private char[][] FillMatrix(char[][] matrix)
        {
            var letToFill = TakeALetterForFill();

            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    var isNumber = int.TryParse(matrix[row][col].ToString(), out int s);
                    if (isNumber)
                    {
                        matrix[row][col] = letToFill;

                        var timesFound = CheckLetterUsed(matrix, letToFill);
                        if (timesFound == 3)
                        {
                            if (letToFill == spellLetter)
                            {
                                isSpellLetFilled = true;
                            }
                            else if (letToFill == goBackLetter)
                            {
                                isGoBackLetFilled = true;
                            }
                            else
                            {
                                isGoForwardLetFilled = true;
                            }
                        }

                        if (MatrixIsFilled(matrix))
                        {
                            return matrix;
                        }
                        else
                        {
                            letToFill = TakeALetterForFill();
                        }
                    }
                }
            }

            return matrix;
        }

        private char TakeALetterForFill()
        {
            var num = numberGenerator.GenerateNumber(1, 13);

            if (isSpellLetFilled == false && isGoForwardLetFilled == false && isGoBackLetFilled == false)
            {
                if (num == 1 || num == 4 || num == 8 || num == 12)
                {
                    return spellLetter;
                }
                else if (num == 2 || num == 3 || num == 6 || num == 11)
                {
                    return goBackLetter;
                }
                else//num is 5 or 7 or 9 or 10
                {
                    return goForwardLetter;
                }
            }
            else if (isSpellLetFilled == true && isGoForwardLetFilled == true)
            {
                return goBackLetter;
            }
            else if (isSpellLetFilled == true && isGoBackLetFilled == true)
            {
                return goForwardLetter;
            }
            else if (isGoForwardLetFilled == true && isGoBackLetFilled == true)
            {
                return spellLetter;
            }
            else if (isSpellLetFilled == true)
            {
                if (num == 1 || num == 4 || num == 5 || num == 9 || num == 11 || num == 12)
                {
                    return goForwardLetter;
                }
                else//num is 2 or 3 or 6 or 7 or 8 or 10
                {
                    return goBackLetter;
                }
            }
            else if (isGoForwardLetFilled == true)
            {
                if (num == 1 || num == 4 || num == 5 || num == 9 || num == 11 || num == 12)
                {
                    return goBackLetter;
                }
                else//num is 2 or 3 or 6 or 7 or 8 or 10
                {
                    return spellLetter;
                }
            }
            else //isGoBackLetFilled == true
            {
                if (num == 1 || num == 4 || num == 5 || num == 9 || num == 11 || num == 12)
                {
                    return goForwardLetter;
                }
                else//num is 2 or 3 or 6 or 7 or 8 or 10
                {
                    return spellLetter;
                }
            }

        }

        private bool MatrixIsFilled(char[][] matrix)
        {
            if (isGoBackLetFilled == true && isGoForwardLetFilled == true && isSpellLetFilled == true)
            {
                return true;
            }

            return false;


            //for (int row = 0; row < matrix.Length; row++)
            //{
            //    for (int col = 0; col < matrix[row].Length; col++)
            //    {
            //        if (matrix[row][col] == notFilledLetter)
            //        {
            //            return false;
            //        }
            //    }
            //}

            //return true;
        }

        private int CheckLetterUsed(char[][] matrix, char letterToCheck)
        {
            var counter = 0;
            for (int row = 0; row < matrix.Length; row++)
            {
                for (int col = 0; col < matrix[row].Length; col++)
                {
                    if (matrix[row][col] == letterToCheck)
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }
    }
}
