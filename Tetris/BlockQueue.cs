using System;

namespace Tetris
{
    //it contain a block array with an instance of the 7 block classes which we will recycle
    public class BlockQueue
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        // need random object
        private readonly Random random = new Random();

        //finally a property for the next block in the queue
        public Block NextBlock {  get; private set; }

        //in the constructor we initialize the next block with a random block
        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }


        // when we write the ui we will preview this block so  the player knows whats coming
        //method for a random block

        private Block RandomBlock()
        {
            return blocks[random.Next(blocks.Length)];
        }

        //last method we need returns the next block and updates the property
        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while(block.Id == NextBlock.Id);

            return block;   
        }
    }
}
