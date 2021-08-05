﻿using ChessCore.Exceptions;

namespace ChessCore.Modes.Standard
{
    class MoveEnPassant : StandardMove
    {
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
