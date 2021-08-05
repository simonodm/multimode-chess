using ChessCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.Controls
{
    abstract class ChessBoardControl : Control
    {
        public event EventHandler TileClick
        {
            add
            {
                _onTileClicked += value;
            }
            remove
            {
                _onTileClicked -= value;
            }
        }
        private ChessBoardTileControl[,] _tileMap;
        private int _width;
        private int _height;
        private event EventHandler _onTileClicked;
        private bool _blackOriented;

        public ChessBoardControl(int width, int height, bool blackOriented = false)
        {
            _tileMap = InitializeTileMap(width, height);
            _width = width;
            _height = height;
            _blackOriented = blackOriented;
        }

        public ChessBoardTileControl GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }

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

        public void Flip()
        {
            _blackOriented = !_blackOriented;
            Invalidate();
        }

        protected virtual void OnTileClick(object sender, EventArgs e)
        {
            _onTileClicked?.Invoke(sender, e);
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
