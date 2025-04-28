using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperApp
{
    public class MinesweeperGame
    {
        public int Size { get; }
        public int NumMines { get; }
        public bool GameOver { get; private set; }
        public bool GameWon { get; private set; }
        public GridCell[,] Grid { get; }

        public MinesweeperGame(int size, int numMines)
        {
            if (size <= 0)
            {
                throw new ArgumentException("Invalid grid size input.!");
            }
            Size = size;
            NumMines = numMines;
            Grid = new GridCell[size, size];
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            // Initialize the grid cells
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new GridCell();
                }
            }

            // Place mines randomly
            Random random = new Random();
            int placedMines = 0;
            while (placedMines < NumMines)
            {
                int row = random.Next(Size);
                int col = random.Next(Size);
                if (!Grid[row, col].IsMine)
                {
                    Grid[row, col].IsMine = true;
                    placedMines++;
                }
            }

            // Calculate adjacent mines for each cell
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (Grid[i, j].IsMine)
                    {
                        continue;
                    }

                    int adjacentMines = 0;
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            int newRow = i + x;
                            int newCol = j + y;
                            if (newRow >= 0 && newRow < Size && newCol >= 0 && newCol < Size && Grid[newRow, newCol].IsMine)
                            {
                                adjacentMines++;
                            }
                        }
                    }
                    Grid[i, j].AdjacentMines = adjacentMines;
                }
            }
        }

        public void Reveal(int row, int col)
        {
            if (row < 0 || col < 0 || row >= Size || col >= Size || Grid[row, col].IsRevealed)
                return;

            Grid[row, col].IsRevealed = true;

            if (Grid[row, col].IsMine)
            {
                GameOver = true;
                return;
            }

            // Reveal adjacent cells if no adjacent mines
            if (Grid[row, col].AdjacentMines == 0)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        int newRow = row + x;
                        int newCol = col + y;
                        if (newRow >= 0 && newRow < Size && newCol >= 0 && newCol < Size && !Grid[newRow, newCol].IsRevealed)
                        {
                            Reveal(newRow, newCol);
                        }
                    }
                }
            }

            // Check if game is won
            GameWon = CheckWin();
        }

        private bool CheckWin()
        {
            foreach (var cell in Grid)
            {
                if (!cell.IsMine && !cell.IsRevealed)
                {
                    return false;
                }
            }
            return true;
        }

        public void Display(bool revealMines = false)
        {
            Console.WriteLine("  " + string.Join(" ", Enumerable.Range(1, Size)));
            for (int i = 0; i < Size; i++)
            {
                Console.Write((char)('A' + i) + " ");
                for (int j = 0; j < Size; j++)
                {
                    if (Grid[i, j].IsRevealed || revealMines)
                    {
                        if (Grid[i, j].IsMine)
                            Console.Write("* ");
                        else
                            Console.Write(Grid[i, j].AdjacentMines + " ");
                    }
                    else
                    {
                        Console.Write("_ ");
                    }
                }
                Console.WriteLine();
            }
        }
    }

}
