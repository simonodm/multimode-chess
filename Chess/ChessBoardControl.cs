using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Chess.Game;

namespace Chess
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

        public ChessBoardControl(int width, int height)
        {
            _tileMap = InitializeTileMap(width, height);
            _width = width;
            _height = height;
        }

        public ChessBoardTileControl GetTile(int x, int y)
        {
            return _tileMap[x, y];
        }

        public virtual void UpdateBoard(Board board)
        {
            for(int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    var tile = GetTile(i, j);
                    var square = tile.Square;
                    var newSquare = board.GetSquare(i, j);
                    if(square != newSquare)
                    {
                        tile.Square = newSquare;
                    }
                }
            }
        }

        protected virtual void OnTileClick(object sender, EventArgs e)
        {
            _onTileClicked?.Invoke(sender, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(_tileMap == null)
            {
                return;
            }

            int sizeX = Size.Width / _width;
            int sizeY = Size.Height / _height;

            for (int i = 0; i < _width; i++)
            {
                for(int j = 0; j < _height; j++)
                {
                    _tileMap[i, j].Size = new Size(sizeX, sizeY);
                    _tileMap[i, j].Location = new Point(i * sizeX, (_height - j - 1) * sizeY);
                }
            }
        }

        private ChessBoardTileControl[,] InitializeTileMap(int width, int height)
        {
            var tileMap = new ChessBoardTileControl[width, height];
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
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
