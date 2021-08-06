using ChessCore.Exceptions;

namespace ChessCore.Modes.Standard
{
    class MovePromotion : StandardMove
    {
        private IPieceFactory _pieceFactory;

        public MovePromotion() : base()
        {
            _pieceFactory = _rules.GetPieceFactory();
            foreach (var option in _pieceFactory.GetPieceOptions(new int[] { 0, 3, 4, 5 }))
            {
                AddOption(option);
            }
        }

        public override StandardBoardState Process()
        {
            if (SelectedOption == null)
            {
                throw new ChessCoreException("No option was selected for MovePromotion.");
            }
            StandardMove move;
            if (To.GetPiece() != null)
            {
                move = new MoveCapture()
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            else
            {
                move = new MoveNormal()
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
