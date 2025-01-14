namespace Tetris
{
    // to represent a position or cell in a grid.
    //this class will stare a row and column
    public class Position
    {
        public int Row { get; set; }
        public int Column { get; set; }

        //simple constructor
        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }


    }
}
