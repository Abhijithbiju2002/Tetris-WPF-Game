using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tetris;

namespace Tetris_2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),

        };
        private readonly ImageSource[] blockImages = new ImageSource[]
        {
             new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
             new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),

        };
        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();

            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++) // Corrected to `grid.Rows`
            {
                for (int c = 0; c < grid.Columns; c++) // Corrected to `grid.Columns`
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }


        private void DrawGrid(GameGrid grid)
        {
            if (grid == null) return; // Safeguard: Ensure grid is not null

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    // Safeguard: Ensure position is within bounds of imageControls
                    if (r < 0 || r >= imageControls.GetLength(0) ||
                        c < 0 || c >= imageControls.GetLength(1))
                    {
                        continue;
                    }

                    int id = grid[r, c];

                    // Safeguard: Ensure id is within bounds of tileImages array
                    if (id < 0 || id >= tileImages.Length)
                    {
                        imageControls[r, c].Opacity = 0; // Default to invisible if id is invalid
                        imageControls[r, c].Source = null;
                        continue;
                    }

                    // Safeguard: Ensure imageControls element is initialized
                    if (imageControls[r, c] != null)
                    {
                        imageControls[r, c].Opacity = 1;
                        imageControls[r, c].Source = tileImages[id];
                    }
                }
            }
        }


        private void DrawBlock(Block block)
        {
            if (block == null) return; // Safeguard: If block is null, exit method

            foreach (Position p in block.TilePositions())
            {
                // Safeguard: Ensure the position is within bounds
                if (p.Row < 0 || p.Row >= imageControls.GetLength(0) ||
                    p.Column < 0 || p.Column >= imageControls.GetLength(1))
                {
                    continue;
                }

                if (imageControls[p.Row, p.Column] != null) // Check if control is initialized
                {
                    imageControls[p.Row, p.Column].Opacity = 1;
                    imageControls[p.Row, p.Column].Source = tileImages[block.Id];
                }
            }
        }


        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }
        private void DrawHeldBlock(Block? heldBlock)


        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }
        private void DrawGhostBlock(Block block)
        {
            if (block == null) return; // Safeguard: Ensure block is not null

            int dropDistance = gameState.BlockDropDistance();

            foreach (Position p in block.TilePositions())
            {
                int row = p.Row + dropDistance;
                int column = p.Column;

                // Safeguard: Ensure position is within bounds
                if (row < 0 || row >= imageControls.GetLength(0) ||
                    column < 0 || column >= imageControls.GetLength(1))
                {
                    continue;
                }

                // Safeguard: Ensure imageControls element is initialized
                if (imageControls[row, column] != null)
                {
                    imageControls[row, column].Opacity = 0.25;

                    // Safeguard: Ensure block.Id is within bounds of tileImages
                    if (block.Id >= 0 && block.Id < tileImages.Length)
                    {
                        imageControls[row, column].Source = tileImages[block.Id];
                    }
                }
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();
                    break;
                case Key.Space:
                    gameState.DropBlock();
                    break;
                default:
                    return;

            }

            Draw(gameState);

        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();

        }
    }
}