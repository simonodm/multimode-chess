using ChessCore.Exceptions;
using ChessCore.Modes.Standard;
using ChessCore.Modes.Standard.Pieces;

namespace ChessCore.Modes.PawnOfTheDead
{
    /// <summary>
    /// Represents the Pawn of the Dead's black capture (convert) move.
    /// </summary>
    class MoveConvert : MoveCapture
    {
        /// <inheritdoc cref="MoveCapture.Process"/>
        public override StandardBoardState Process()
        {
            if (Piece.GetPlayer() != 1) return base.Process();

            var newPiece = GetNewPiece(To.GetPiece());
            var newBoard = BoardBefore.GetBoard().RemovePiece(To).AddPiece(To, newPiece);
            return new StandardBoardState(newBoard, this);
        }

        private StandardPiece GetNewPiece(GamePiece piece)
        {
            if (piece is Pawn)
            {
                return new Pawn(1);
            }
            if (piece is King)
            {
                return new King(1);
            }
            if (piece is Rook)
            {
                return new Rook(1);
            }
            if (piece is Knight)
            {
                return new Knight(1);
            }
            if (piece is Bishop)
            {
                return new Bishop(1);
            }
            if (piece is Queen)
            {
                return new Queen(1);
            }
            throw new ChessCoreException("Unsupported piece.");
        }
    }
}
