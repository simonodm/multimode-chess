using ChessCore.Exceptions;

namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// A move type representing Pawn en passant moves.
    /// </summary>
    internal class MoveEnPassant : StandardMove
    {
        /// <inheritdoc cref="StandardMove.Process"/>
        public override StandardBoardState Process()
        {
            var lastMove = BoardBefore.GetLastMove();
            if (lastMove == null)
            {
                throw new InvalidMoveException("En passant cannot be processed with a null previous move.");
            }
            var board = BoardBefore.GetBoard()
                .RemovePiece(BoardBefore.GetLastMove().To)
                .Move(this);

            return new StandardBoardState(board, this);
        }
    }
}
