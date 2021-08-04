using ChessCore.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chess.Controls
{
    class PlayableChessBoardControl : ChessBoardControl
    {
        public event MoveEventHandler MovePlayed;
        public event MoveEventHandler MoveInputRequested;
        public event LegalMovesEventHandler LegalMovesRequested;
        public event BoardEventHandler BoardRequested;

        private ChessBoardTileControl _selectedTile;
        private List<Move> _selectedLegalMoves;
        private bool _isBoardCurrent = true;
        private bool _isEnabled = true;

        public PlayableChessBoardControl(Board board, bool blackOriented = false) : base(board.GetWidth(), board.GetHeight(), blackOriented)
        {
            TileClick += Tile_Click;
            base.UpdateBoard(board);
        }

        public override void UpdateBoard(Board board)
        {
            base.UpdateBoard(board);

            _isBoardCurrent = GetCurrentBoard() == board;
        }

        public void Disable()
        {
            _isEnabled = false;
        }

        public void Enable()
        {
            _isEnabled = true;
        }

        protected virtual void OnChessMove(MoveEventArgs e)
        {
            MovePlayed?.Invoke(this, e);
        }

        protected virtual void OnMoveInputRequired(MoveEventArgs e)
        {
            MoveInputRequested?.Invoke(this, e);
        }

        protected virtual void OnLegalMovesRequested(LegalMovesEventArgs e)
        {
            LegalMovesRequested?.Invoke(this, e);
        }

        protected virtual void OnBoardRequested(BoardEventArgs e)
        {
            BoardRequested?.Invoke(this, e);
        }

        private void Tile_Click(object sender, EventArgs e)
        {
            if (!_isBoardCurrent || !_isEnabled)
            {
                return;
            }

            var tile = (ChessBoardTileControl)sender;
            if (_selectedTile == null)
            {
                SelectTile(tile);
            }
            else
            {
                var move = _selectedLegalMoves.FirstOrDefault(move => move.To == tile.Square);

                if (tile != _selectedTile)
                {
                    if (move != default(Move))
                    {
                        ProcessMove(move);
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
            if (square.GetPiece() != null)
            {
                tile.Highlight();
                _selectedTile = tile;
                _selectedLegalMoves = new List<Move>();
                var legalMovesArgs = new LegalMovesEventArgs { Square = square };
                OnLegalMovesRequested(legalMovesArgs);
                foreach (var move in legalMovesArgs.LegalMoves)
                {
                    GetTile(move.To.GetFile(), move.To.GetRank()).Highlight();
                    _selectedLegalMoves.Add(move);
                }
            }
        }

        private void UnselectAll()
        {
            foreach (var tile in Controls)
            {
                if (tile is not ChessBoardTileControl)
                {
                    continue;
                }
                var castTile = (ChessBoardTileControl)tile;
                if (castTile.IsHighlighted)
                {
                    castTile.RemoveHighlighting();
                }
            }

            if (_selectedTile != null)
            {
                _selectedTile = null;
            }
        }

        private void ProcessMove(Move move)
        {
            var moveArgs = new MoveEventArgs { Move = move };
            if (move.IsUserInputRequired)
            {
                OnMoveInputRequired(moveArgs);
            }
            OnChessMove(moveArgs);

            UpdateBoard(GetCurrentBoard());
        }

        private Board GetCurrentBoard()
        {
            var args = new BoardEventArgs();
            OnBoardRequested(args);
            if (args.Board != null)
            {
                return args.Board;
            }
            throw new Exception("No board received via BoardRequested event.");
        }
    }
}
