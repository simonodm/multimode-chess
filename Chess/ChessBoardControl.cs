using Chess.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    class ChessBoardControl : Control
    {
        public ChessGame Game;
        private ChessBoardTileControl _selectedTile;
        private List<Move> _selectedLegalMoves;
        private ChessBoardTileControl[,] _tileMap;

        public ChessBoardControl()
        {
            Game = new ChessGame(new ClassicRules());
            _tileMap = new ChessBoardTileControl[8, 8];
        }

        public ChessBoardControl(ChessGame game)
        {
            Game = game;
            _tileMap = new ChessBoardTileControl[8, 8];
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            var tile = (ChessBoardTileControl)sender;
            if(_selectedTile == null)
            {
                SelectTile(tile);
            }
            else
            {
                var move = _selectedLegalMoves.FirstOrDefault(move => move.To == tile.Square);
                if(tile != _selectedTile && move != default(Move))
                {
                    if(move != default(Move))
                    {
                        if(move.IsUserInputRequired)
                        {
                            move.SelectOption(0);
                        }
                        Game.ProcessMove(move);
                        UpdateBoard();
                    }
                    UnselectAll();
                }
                else
                {
                    UnselectAll();
                    SelectTile(tile);
                }
            }
        }

        private void SelectTile(ChessBoardTileControl tile)
        {
            var square = tile.Square;
            if(square.Piece != null && square.Piece.Player == Game.Rules.CurrentPlayer)
            {
                tile.Select();
                _selectedTile = tile;
                _selectedLegalMoves = new List<Move>();
                var legalMoves = Game.Rules.GetLegalMoves(tile.Square, Game.BoardState);
                foreach (var move in legalMoves)
                {
                    _tileMap[move.To.File, move.To.Rank].Select();
                    _selectedLegalMoves.Add(move);
                }
            }
        }

        private void UpdateBoard()
        {
            foreach(var square in Game.BoardState.GetAllSquares())
            {
                var tile = _tileMap[square.File, square.Rank];
                tile.Square = square;
            }
        }

        private void UnselectAll()
        {
            foreach(var tile in Controls)
            {
                if(tile is not ChessBoardTileControl)
                {
                    continue;
                }
                var castTile = (ChessBoardTileControl)tile;
                if(castTile.IsSelected)
                {
                    castTile.Unselect();
                }
            }

            if(_selectedTile != null)
            {
                _selectedTile = null;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if(Game.BoardState == null)
            {
                return;
            }
            int sizeX = Size.Width / Game.BoardState.GetBoardWidth();
            int sizeY = Size.Height / Game.BoardState.GetBoardHeight();
            for(int i = 0; i < Game.BoardState.GetBoardWidth(); i++)
            {
                for(int j = 0; j < Game.BoardState.GetBoardHeight(); j++)
                {
                    var square = Game.BoardState.GetSquare(i, j);
                    var tile = new ChessBoardTileControl(square)
                    {
                        Size = new Size(sizeX, sizeY),
                        Location = new Point(i * sizeX, (Game.BoardState.GetBoardHeight()-j-1) * sizeY)
                    };
                    _tileMap[square.File, square.Rank] = tile;
                    tile.Click += Tile_Click;
                    Controls.Add(tile);
                }
            }
        }
    }
}
