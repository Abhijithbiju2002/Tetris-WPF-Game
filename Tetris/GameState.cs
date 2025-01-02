namespace Tetris
{

    public class GameState
    {
        // adding a property with a backing field for the current block
        private Block currentBlock;

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
                // when we update the current vlock the reset method is called to set  the correct start position and rotation
                for (int i = 0; i < 2; i++)
                {
                    currentBlock.Move(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }

            }
        }
        // adding properties fo the game grid the block queue and a game over boolean

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }

        //in the constructor we initialize the game grid with 22 rows and 10 columns
        //we also initialize the block queue and use it to get a random block for the current block property

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            currentBlock = BlockQueue.GetAndUpdate();
        }
        //checks the method  if the current block is in a legal position or not

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column))
                {
                    return false;
                }
            }
            return true;
            // the method loops over the tile positions of the current block and if any of them are
            // outside the grid or overlapping another tile then we return false
            //otherwise if we get through the entire loop we return true
        }

        // we write a method to rotate the current block clockwise but only if its possible to do so
        //from where it is..
        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
            // the strategy we use is simply rotating the block and if it ends up in an illegal
            //position then we rotate it back
        }
        // method for counter clockwise works in the same way
        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }
        // need method for moving the current block left and right our strategy will be the same as above
        //if it moves to an illegal position we move it back

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1);
            }
        }
        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1);
            }
        }
        // check if the game is over
        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
            //if either of the hidden rows at the top are not empty then the game is lost
        }
        //this method is called when the current block cannot be moved down
        // first it loops over the tile positions of the current block and sets those
        //positions in the game grid equal to the blocks id
        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }
            //we clear any potentially full rows and check if the game is over
            GameGrid.ClearFullRows();

            //if it is.. we set our gameover property to true
            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                currentBlock = BlockQueue.GetAndUpdate();
            }
        }
        //move down method
        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);

            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }
    }
}
