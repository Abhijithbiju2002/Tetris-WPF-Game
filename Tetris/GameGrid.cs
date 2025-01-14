using System.Text;

namespace Tetris_2._0
{
    public class GameGrid
    {
        private readonly int[,] grid;

        public int Rows { get; }

        public int Columns { get; }

        public int this[int r, int c]
        {
            get
            {
                if (!IsInside(r, c)) throw new IndexOutOfRangeException($"Index ({r}, {c}) is outside the grid.");
                return grid[r, c];
            }
            set
            {
                if (!IsInside(r, c)) throw new IndexOutOfRangeException($"Index ({r}, {c}) is outside the grid.");
                grid[r, c] = value;
            }
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInside(int r, int c) => r >= 0 && r < Rows && c >= 0 && c < Columns;

        public bool IsEmpty(int r, int c) => IsInside(r, c) && grid[r, c] == 0;

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0) return false;
            }
            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0) return false;
            }
            return true;
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++) grid[r, c] = 0;
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;
            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
            }
            return cleared;
        }

        public void ResetGrid()
        {
            for (int r = 0; r < Rows; r++) ClearRow(r);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++) sb.Append(grid[r, c] + " ");
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}