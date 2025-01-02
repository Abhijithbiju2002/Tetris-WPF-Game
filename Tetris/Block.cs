namespace Tetris
{
    // this is an abstract class then we will write an subclass for esch  specific block
    public abstract class Block
    {
        //a two dimentional position array which cointains the tile positions in the four rotation states
        protected abstract Position[][] Tiles { get; }
        //a start offset which decides where the block spawns in the grid
        protected abstract Position StartOffset { get; }
        //an integer id whch we need to distinguish the blocks
        public abstract int Id { get; }
        //we store the current rotation state and the current offset
        private int rotationState;
        private Position offset;

        //in constructor we set the offset equal to start offset
        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }
        //grid positions occupied by the block factoring in the current rotation and offset
        public IEnumerable<Position> TilePosition()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
            //the method loops over the tile positions in the current rotation state ands the row offset and column offset
        }
        //this method rotates the block 90 degrees clockwise,we do that incrementing the current rotation state,wrapping around zero if its in the final state
        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }
        // counter clockwise
        public void RotateCCW()
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }
        //move method which moves the block by a given number of rows and columns
        public void Move(int rows, int columns)
        {
            offset.Row += rows;
            offset.Column += columns;
        }
        //reset method which resets the rotation and position
        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }

    }
}
