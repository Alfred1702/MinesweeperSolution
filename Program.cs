using System;

namespace MinesweeperApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Minesweeper!\n");

            while (true)
            {
                int size = GetPositiveNumber("Enter the size of the grid (e.g. 4 for a 4x4 grid): ");
                int maxMines = (size * size * 35) / 100;
                int mines = GetMinesInRange($"Enter the number of mines (maximum is 35% of the total squares) (e.g. 0-{maxMines}): ", 0, maxMines);

                var game = new MinesweeperGame(size, mines);

                Console.WriteLine("\nHere is your minefield:");
                game.Display();

                while (!game.GameOver && !game.GameWon)
                {
                    Console.Write("Select a square to reveal (e.g., A1): ");
                    var input = Console.ReadLine();
                    if (ParseInput(input, size, out int row, out int col))
                    {
                        if (game.Grid[row, col].IsRevealed)
                        {
                            Console.WriteLine("This square has already been revealed. Please choose another one.");
                            continue;
                        }

                        game.Reveal(row, col);

                        if (game.GameOver)
                        {
                            Console.WriteLine("Oh no, you detonated a mine! Game over.");
                            game.Display(true);
                            break;
                        }
                        else if (game.GameWon)
                        {
                            Console.WriteLine("Congratulations, you have won the game!");
                            game.Display(true);
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"This square contains {game.Grid[row, col].AdjacentMines} adjacent mines.");
                            Console.WriteLine("\nHere is your updated minefield:");
                            game.Display();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Try again.");
                    }
                }

                Console.WriteLine("Press Enter to play again or Ctrl+C to exit...");
                Console.ReadLine();
            }
        }

        static int GetPositiveNumber(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int number) && number > 0)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a positive whole number.");
                }
            }
        }

        static int GetMinesInRange(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int number) && number >= min && number <= max)
                {
                    return number;
                }
                else
                {
                    Console.WriteLine($"Invalid input. Pleaase enter a number between {min} and {max}");
                }
            }
        }

        // At least input 2 to play minesweeper function
        //static int GetGridSize(string prompt)
        //{
        //    while (true)
        //    {
        //        Console.Write(prompt);
        //        if (int.TryParse(Console.ReadLine(), out int number))
        //        {
        //            if (number >= 2)
        //                return number;
        //            else
        //                Console.WriteLine("Grid size must be at least 2 to play Minesweeper.");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Invalid input. Please enter a positive whole number.");
        //        }
        //    }
        //}

        public static bool ParseInput(string input, int size, out int row, out int col)
        {
            row = col = -1;
            input = input.Trim().ToUpper();
            if (string.IsNullOrEmpty(input) || input.Length < 2)
                return false;

            char rowChar = input[0];
            if (rowChar < 'A' || rowChar >= 'A' + size)
                return false;

            if (!int.TryParse(input.Substring(1), out col))
                return false;

            if (col < 1 || col > size)
                return false;

            row = rowChar - 'A';
            col -= 1;
            return true;
        }
    }
}