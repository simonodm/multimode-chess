using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveEnPassant : ClassicMove
    {
        public MoveEnPassant(IGameRules rules) : base(rules) { }
        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .RemovePiece(BoardBefore.GetLastMove().To)
                .Move(this);
            return new StandardBoardState(board, this);
        }

        public static bool IsLegal(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            var previousMove = state.GetLastMove();
            if (previousMove == null)
            {
                return false;
            }
            return from.GetPiece() is Pawn &&
                previousMove.Piece is Pawn &&
                Math.Abs(previousMove.To.GetRank() - previousMove.From.GetRank()) == 2 &&
                to == GetEnPassantSquare(state);
        }

        private static BoardSquare GetEnPassantSquare(BoardState state)
        {
            var enPassantFile = state.GetLastMove().From.GetFile();
            var enPassantRank = state.GetLastMove().From.GetRank() + (state.GetLastMove().To.GetRank() - state.GetLastMove().From.GetRank()) / 2;
            var square = state.GetBoard().GetSquare(enPassantFile, enPassantRank);
            return square;
        }
    }
}
