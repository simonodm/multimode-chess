using ChessCore.Exceptions;

namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// A move type representing Pawn promotion. This move requires user input.
    /// </summary>
    internal class MovePromotion : StandardMove
    {
        private readonly IPieceFactory _pieceFactory;

        public MovePromotion()
        {
            _pieceFactory = Rules.GetPieceFactory();
            foreach (var option in _pieceFactory.GetPieceOptions(new[] { 0, 3, 4, 5 }))
            {
                AddOption(option);
            }
        }

        /// <summary>
        /// Processes the promotion according to the selected option.
        /// </summary>
        /// <exception cref="ChessCoreException">Thrown if no option was selected before processing.</exception>
        /// <returns>Board state after processing the move</returns>
        public override StandardBoardState Process()
        {
            if (SelectedOption == null)
            {
                throw new ChessCoreException("No option was selected for MovePromotion.");
            }
            StandardMove move;
            if (To.GetPiece() != null)
            {
                move = new MoveCapture
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            else
            {
                move = new MoveNormal
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }

            var newState = move.Process();
            var board = newState.GetBoard()
                .RemovePiece(To)
                .AddPiece(To, GetPieceFromSelectedOption());

            return new StandardBoardState(board, this);
        }

        private GamePiece GetPieceFromSelectedOption()
        {
            return _pieceFactory.GetPiece(SelectedOption.Id, Piece.GetPlayer());
        }
    }
}
