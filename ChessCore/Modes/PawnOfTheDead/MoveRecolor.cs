using ChessCore.Exceptions;
using ChessCore.Modes.Standard;
using ChessCore.Modes.Standard.Pieces;

namespace ChessCore.Modes.PawnOfTheDead
{
    class MoveRecolor : MoveCapture
    {
        public override StandardBoardState Process()
        {
            if (Piece.GetPlayer() == 1)
            {
                var newPiece = GetNewPiece(To.GetPiece());
                var newBoard = BoardBefore.GetBoard().RemovePiece(To).AddPiece(To, newPiece);
                return new StandardBoardState(newBoard, this);
            }
            return base.Process();
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
