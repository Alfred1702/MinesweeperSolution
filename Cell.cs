using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperApp
{
    public class GridCell
    {
        public bool IsMine { get; set; }
        public bool IsRevealed { get; set; }
        public int AdjacentMines { get; set; }
    }
}
