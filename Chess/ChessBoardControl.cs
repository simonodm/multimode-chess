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
        public delegate void MoveEventHandler(object sender, MoveEventArgs e);
        public event MoveEventHandler MovePlayed
        {
            add
            {
                onChessMove += value;
            }
            remove
            {
                onChessMove -= value;
            }
        }
        public event MoveEventHandler MoveInputRequired
        {
            add
            {
                _onMoveInputRequired += value;
            }
            remove
            {
                _onMoveInputRequired -= value;
            }
        }

        public ChessGame Game;
        private ChessBoardTileControl _selectedTile;
        private List<Move> _selectedLegalMoves;
        private ChessBoardTileControl[,] _tileMap;
        private event MoveEventHandler onChessMove;
        private event MoveEventHandler _onMoveInputRequired;
        private bool _isBoardCurrent = true;

        public ChessBoardControl(ChessGame game)
        {
            Game = game;
            _tileMap = new ChessBoardTileControl[8, 8];
        }

        public void UpdateBoard(BoardState state)
        {
            if(_tileMap == null)
            {
                return;
            }
            for(int i = 0; i < _tileMap.GetLength(0); i++)
            {
                for(int j = 0; j < _tileMap.GetLength(1); j++)
                {
                    var currentSquare = _tileMap[i, j].Square;
                    var newSquare = state.GetBoard().GetSquare(i, j);
                    if(currentSquare.GetPiece() != newSquare.GetPiece())
                    {
                        _tileMap[i, j].Square = newSquare;
                    }
                }
            }
            if (state == Game.BoardState)
            {
                _isBoardCurrent = true;
            }
            else
            {
                _isBoardCurrent = false;
            }
        }

        protected virtual void OnChessMove(MoveEventArgs e)
        {
            onChessMove?.Invoke(this, e);
        }

        protected virtual void OnMoveInputRequired(MoveEventArgs e)
        {
            _onMoveInputRequired?.Invoke(this, e);
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            if(!_isBoardCurrent)
            {
                return;
            }
            var tile = (ChessBoardTileControl)sender;
            if(_selectedTile == null)
            {
                SelectTile(tile);
            }
            else
            {
                var move = _selectedLegalMoves.FirstOrDefault(move => move.To == tile.Square);
                if(tile != _selectedTile)
                {
                    if(move != default(Move))
                    {
                        var moveArgs = new MoveEventArgs { Move = move };
                        if(move.IsUserInputRequired)
                        {
                            OnMoveInputRequired(moveArgs);
                        }
                        Game.ProcessMove(move);
                        OnChessMove(moveArgs);
                        UpdateBoard(Game.BoardState);
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
            if(square.GetPiece() != null && square.GetPiece().GetPlayer() == Game.CurrentPlayer)
            {
                tile.Select();
                _selectedTile = tile;
                _selectedLegalMoves = new List<Move>();
                var legalMoves = Game.Rules.GetLegalMoves(tile.Square, Game.BoardState);
                foreach (var move in legalMoves)
                {
                    _tileMap[move.To.GetFile(), move.To.GetRank()].Select();
                    _selectedLegalMoves.Add(move);
                }
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
            if(Game.BoardState == null || Controls.Count > 0)
            {
                return;
            }

            var board = Game.BoardState.GetBoard();

            int boardWidth = board.GetWidth();
            int boardHeight = board.GetHeight();

            int sizeX = Size.Width / boardWidth;
            int sizeY = Size.Height / boardHeight;
            for (int i = 0; i < boardWidth; i++)
            {
                for (int j = 0; j < boardHeight; j++)
                {
                    var square = board.GetSquare(i, j);
                    var tile = new ChessBoardTileControl(square)
                    {
                        Size = new Size(sizeX, sizeY),
                        Location = new Point(i * sizeX, (boardHeight - j - 1) * sizeY)
                    };
                    _tileMap[square.GetFile(), square.GetRank()] = tile;
                    tile.Click += Tile_Click;
                    Controls.Add(tile);
                }
            }
        }
    }
}
