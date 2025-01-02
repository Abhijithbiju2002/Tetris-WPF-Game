namespace Tetris
{
    // class holds two dimentional rectangular array
    public class GameGrid
    {
        private readonly int[,] grid;

        public int Rows { get; }

        public int Columns { get; }

        //define indexer to provide easy access to the array
        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        //constructor
        //it takes the number of rows and columns as parameters
        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[Rows, Columns];
            //expl:in the body we save the number of rows and columns and intialize the array
        }

        // creating a few convenience methods

        //checks if the given row and column is inside the grid or not
        public bool IsInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
            //expl: to be inside the grid the row must be greater than or equal to zero and less than the number of rows,
            //similarly for column...it must be less than the number of columns
        }

        // checks the given cell is empty or not
        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;//it must be inside the grid and the value at the entry
                                                     //in the array must be zero
        }

        //checks the entire row is full
        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        //checks row is empty
        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        // method for clear row

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        //one that moves a row down by a certain no. of rows
        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }

        // implement a clear full row method
        public int ClearFullRows()
        {
            int cleared = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                //if the current row is full and if it is we clear it and increment cleared

                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleared++;
                }
                //otherwise if cleared is greater than zero, then we move the current row down by the number of cleared rows
                else if (cleared > 0)
                {
                    MoveRowDown(r, cleared);
                }
                // in the end we return the number of cleared rows
            }
            return cleared;


        }

    }
}
