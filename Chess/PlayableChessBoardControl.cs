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
    class PlayableChessBoardControl : ChessBoardControl
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
        private event MoveEventHandler onChessMove;
        private event MoveEventHandler _onMoveInputRequired;
        private bool _isBoardCurrent = true;

        public PlayableChessBoardControl(ChessGame game) : base(game.GetBoardState().GetBoard().GetWidth(), game.GetBoardState().GetBoard().GetHeight())
        {
            Game = game;

            TileClick += Tile_Click;
            UpdateBoard(game.GetBoardState().GetBoard());
        }

        public override void UpdateBoard(Board board)
        {
            base.UpdateBoard(board);

            _isBoardCurrent = board == Game.GetBoardState().GetBoard();
        }

        protected override void OnTileClick(object sender, EventArgs e)
        {
            base.OnTileClick(sender, e);


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
                        UpdateBoard(Game.GetBoardState().GetBoard());
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
            if(square.GetPiece() != null && square.GetPiece().GetPlayer() == Game.GetCurrentPlayer())
            {
                tile.Select();
                _selectedTile = tile;
                _selectedLegalMoves = new List<Move>();
                var legalMoves = Game.GetLegalMoves(tile.Square, Game.GetBoardState());
                foreach (var move in legalMoves)
                {
                    GetTile(move.To.GetFile(), move.To.GetRank()).Select();
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
    }
}
