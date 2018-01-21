namespace DemolitionFalcons.App.Miscellaneous.SpecialSquares
{
    using DemolitionFalcons.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MiniGameAction
    {
        private NumberGenerator numberGenerator;
        public MiniGameAction()
        {
            MoveForward = false;
            GoBack = false;
            DemolitionFalcons = false;
            this.numberGenerator = new NumberGenerator();
        }

        #region ProvideInfoAboutTheResultOfTheGame
        public bool MoveForward { get; set; }
        public int MoveForwardWith { get; set; }

        public bool GoBack { get; set; }
        public int GoBackWith { get; set; }

        public bool DemolitionFalcons { get; set; }
        #endregion

        public void PlayMiniGame(bool isSingle)
        {
            ExplainMiniGame();

            int counter = 0;
            var numbers = new List<int>() { 0, 0, 0, 0, 0 };
            Console.WriteLine($"So let the game begin");
            var firstNumber = numberGenerator.GenerateNumber(2, 5);
            Console.WriteLine($"The first number generated was {firstNumber}");
            numbers[counter] = firstNumber;
            DisplayPlayerNumbers(numbers);
            bool playerSuggestedCorrectly = true;
            counter++;
            while (counter != 4 && playerSuggestedCorrectly)
            {
                var numGenerated = numberGenerator.GenerateNumber(1, 6);

                var playersSuggestion = GetSuggestion(isSingle);
                while (numGenerated == numbers[counter - 1])
                {
                    numGenerated = numberGenerator.GenerateNumber(1, 6);
                }
                playerSuggestedCorrectly = CheckSuggestion(numGenerated, numbers, counter, playersSuggestion);
                numbers[counter] = numGenerated;
                DisplayPlayerNumbers(numbers);
                if (!playerSuggestedCorrectly)
                {
                    if (counter == 1)
                    {
                        Console.WriteLine($"Sorry, you move 5 square backwards");
                        GoBack = true;
                        GoBackWith = 5;
                    }
                    else if (counter == 2)
                    {
                        Console.WriteLine($"Sorry, you move 2 square backwards");
                        GoBack = true;
                        GoBackWith = 2;
                    }
                    else if (counter == 3)
                    {
                        Console.WriteLine($"Welp, you ain't going anywhere!");
                    }
                    Console.WriteLine($"Have better luck next time :)");

                }

                counter++;
            }
            
            if (playerSuggestedCorrectly)
            {
                var finalNumber = numberGenerator.GenerateNumber(1, 6);
                while (finalNumber == numbers[counter - 1])
                {
                    finalNumber = numberGenerator.GenerateNumber(1, 6);
                }

                var playerSuggestion = GetSuggestion(isSingle);
                numbers[counter] = finalNumber;
                bool isCorrect = CheckSuggestion(finalNumber, numbers, counter, playerSuggestion);
                DisplayPlayerNumbers(numbers);
                if (isCorrect)
                {
                    Console.WriteLine($"Congratulations! You have suggested all 5 numbers correctly and can proceed 5 squares forward.");
                    MoveForward = true;
                    MoveForwardWith = 5;
                }
                else
                {
                    Console.WriteLine($"Well done kiddo, you are moving 2 square forward!");
                    Console.WriteLine($"Next time you might guess all 5 :)");

                    MoveForward = true;
                    MoveForwardWith = 2;
                }

                if (numbers[0] == 3 && numbers[2] == 3 && numbers[4] == 3)
                {
                    Console.WriteLine($"SURPRISEEEEEEEEE, DEMOLITION FALCONS!!!");
                    Console.WriteLine($"Despite proceeding this far in our mini game, your luck betrayed everyone in the game...");
                    Console.WriteLine($"Each character must go to the Start now :) :) :)");
                    DemolitionFalcons = true;
                }
            }
        }

        private void ExplainMiniGame()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("The concept of this MiniGame is as it follow:");
            sb.AppendLine("1) There are 5 boxes, each of them will contain a number between 1 and 5");
            sb.AppendLine(" //(Well, except the first one that will be between 2 and 4");
            sb.AppendLine("2) The player has to guess whether the next number will be bigger or smaller than the previous one");
            sb.AppendLine("3) The game ends either when the player suggest correctly the 5th number or if once he makes a mistake");
            sb.AppendLine("Additional features when connected with a map:");
            sb.AppendLine("1) If the player fails to suggest the 2nd or the 3rd number, he moves N squares backwards");
            sb.AppendLine("2) If the player fails to suggest the 4th he stays on the magic square where he is now");
            sb.AppendLine("3) If the player fails to suggest the 5th, he moves N squares forward.");
            sb.AppendLine("4) If the player manage to suggest all numbers correctly, he moves N + 3 squares forward.");
            sb.AppendLine("5) If the number 3 occurs 3 times within those 5 numbers, Demolition Falcons are unleashed and all players return to the start square");
            sb.AppendLine("Also in the future if we add lets say crystals that could be acquire only by paying real money, the player might pay with for example 10 crystal to have a 2nd chance if he fails to suggest a num correctly");

        }

        private string GetSuggestion(bool isSingle)
        {
            Console.WriteLine($"What the following number will be? Type:");
            Console.WriteLine("B -> bigger");
            Console.WriteLine("S -> smaller");

            string playerSuggestion;
            if (isSingle)
            {
                var num = numberGenerator.GenerateNumber(1, 3);
                if (num == 1)
                {
                    playerSuggestion = "B";
                }
                else
                {
                    playerSuggestion = "S";
                }
            }
            else
            {
                playerSuggestion = Console.ReadLine();
                bool isTrue = false;
                if (playerSuggestion == "B" || playerSuggestion == "S")
                {
                    isTrue = true;
                }
                while (!isTrue)
                {
                    Console.WriteLine($"Please type either 'B' or 'S'");
                    playerSuggestion = Console.ReadLine();
                    if (playerSuggestion == "B" || playerSuggestion == "S")
                    {
                        isTrue = true;
                    }
                }
            }
            
            return playerSuggestion;
        }

        private void DisplayPlayerNumbers(List<int> numbers)
        {
            Console.WriteLine(string.Join("|", numbers));
        }

        private bool CheckSuggestion(int numGenerated, List<int> numbers, int counter, string playerSuggestion)
        {
            if (playerSuggestion == "B" && numbers[counter - 1] > numGenerated)
            {
                return false;
            }
            if (playerSuggestion == "S" && numbers[counter - 1] < numGenerated)
            {
                return false;
            }

            return true;
        }
    }
}
