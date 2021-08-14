using ChessCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGUI.Controls
{
    /// <summary>
    /// A base class for an interactive board control.
    /// </summary>
    internal abstract class ChessBoardControl : UserControl
    {
        /// <summary>
        /// Occurs when a tile is clicked.
        /// </summary>
        public event EventHandler TileClick;

        private readonly ChessBoardTileControl[,] _tileMap;
        private readonly int _width;
        private readonly int _height;
        private bool _blackOriented;

        protected ChessBoardControl(int width, int height, bool blackOriented = false)
        {
            _tileMap = InitializeTileMap(width, height);
            _width = width;
            _height = height;
            _blackOriented = blackOriented;
        }

        /// <summary>
        /// Retrieves the tile control at the specified chessboard coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The ChessBoardTileControl at the specified coordinates</returns>
        public ChessBoardTileControl GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }

        /// <summary>
        /// Updates the displayed board.
        /// </summary>
        /// <param name="board">Board to display</param>
        public virtual void UpdateBoard(Board board)
        {
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    var tile = GetTile(i, j);
                    var square = tile.Square;
                    var newSquare = board.GetSquare(i, j);
                    if (square != newSquare)
                    {
                        tile.Square = newSquare;
                    }
                }
            }
        }

        /// <summary>
        /// Flips the board vertically and horizontally
        /// </summary>
        public void Flip()
        {
            _blackOriented = !_blackOriented;
            Invalidate();
        }

        protected virtual void OnTileClick(object sender, EventArgs e)
        {
            TileClick?.Invoke(sender, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_tileMap == null)
            {
                return;
            }

            int sizeX = Size.Width / _width;
            int sizeY = Size.Height / _height;

            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    _tileMap[i, j].Size = new Size(sizeX, sizeY);
                    var tileLocation = _blackOriented ?
                        new Point((_width - i - 1) * sizeX, j * sizeY) :
                        new Point(i * sizeX, (_height - j - 1) * sizeY);
                    _tileMap[i, j].Location = tileLocation;
                }
            }
        }

        private ChessBoardTileControl[,] InitializeTileMap(int width, int height)
        {
            var tileMap = new ChessBoardTileControl[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var tile = new ChessBoardTileControl(new BoardSquare(i, j));
                    tile.Click += OnTileClick;
                    tileMap[i, j] = tile;
                    Controls.Add(tile);
                }
            }

            return tileMap;
        }
    }
}
