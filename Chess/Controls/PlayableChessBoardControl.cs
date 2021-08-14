using ChessCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessGUI.Controls
{
    /// <summary>
    /// An interactive, playable chess board.
    /// </summary>
    internal class PlayableChessBoardControl : ChessBoardControl
    {
        /// <summary>
        /// Occurs when the user plays a move.
        /// </summary>
        public event MoveEventHandler MovePlayed;

        /// <summary>
        /// Occurs when the control requires additional input.
        /// </summary>
        public event MoveEventHandler MoveInputRequested;

        /// <summary>
        /// Occurs when the board requires a list of legal moves to continue.
        /// </summary>
        public event LegalMovesEventHandler LegalMovesRequested;

        private ChessBoardTileControl _selectedTile;
        private List<Move> _selectedLegalMoves;
        private bool _isBoardCurrent = true;
        private bool _isEnabled = true;

        public PlayableChessBoardControl(Board gameBoard, bool blackOriented = false) : base(gameBoard.GetWidth(), gameBoard.GetHeight(), blackOriented)
        {
            TileClick += Tile_Click;
            base.UpdateBoard(gameBoard);
        }

        /// <inheritdoc cref="ChessBoardControl.UpdateBoard"/>
        /// <param name="isCurrent">Whether the board is the game's current board</param>
        public void UpdateBoard(Board board, bool isCurrent = false)
        {
            base.UpdateBoard(board);

            _isBoardCurrent = isCurrent;
        }

        /// <summary>
        /// Disables the board's interactivity.
        /// </summary>
        public void Disable()
        {
            _isEnabled = false;
        }

        /// <summary>
        /// Enables the board's interactivity.
        /// </summary>
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
                var move = _selectedLegalMoves.FirstOrDefault(legalMove => legalMove.To == tile.Square);

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
            if (square.GetPiece() == null) return;

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

            _selectedTile = null;
        }

        private void ProcessMove(Move move)
        {
            var moveArgs = new MoveEventArgs { Move = move };
            if (move.IsUserInputRequired)
            {
                OnMoveInputRequired(moveArgs);
            }
            OnChessMove(moveArgs);
        }
    }
}
