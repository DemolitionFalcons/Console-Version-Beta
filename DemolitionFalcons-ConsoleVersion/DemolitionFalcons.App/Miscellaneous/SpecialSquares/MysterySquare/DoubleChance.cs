namespace DemolitionFalcons.App.Miscellaneous.SpecialSquares.MysterySquare
{
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

        public char[][] StartDoubleChance()
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
                else//num is 5 or 6 or 9 or 10
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
