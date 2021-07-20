using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveCastle : ClassicMove
    {
        public MoveCastle(IGameRules rules) : base(rules) { }

        public override StandardBoardState Process()
        {
            var rookSquare = GetRookSquare(BoardBefore, From, To);

            var rookTargetSquare = To.GetFile() > From.GetFile() ?
                BoardBefore.GetBoard().GetSquare(From.GetFile() + 1, From.GetRank()) :
                BoardBefore.GetBoard().GetSquare(From.GetFile() - 1, From.GetRank());

            var rookMove = new MoveNormal(_rules)
            {
                From = rookSquare,
                To = rookTargetSquare,
                Piece = rookSquare.GetPiece()
            };

            var board = BoardBefore.GetBoard()
                .Move(this)
                .Move(rookMove);
            return new StandardBoardState(board, this);
        }

        public static bool IsLegal(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (CheckBaseCastleConditions(from, to))
            {
                var expectedRookSquare = GetRookSquare(state, from, to);

                for (int i = Math.Min(from.GetFile(), expectedRookSquare.GetFile()); i <= Math.Max(from.GetFile(), expectedRookSquare.GetFile()); i++)
                {
                    var square = state.GetBoard().GetSquare(i, from.GetRank());
                    if (state.IsSquareUnderThreat(square, (from.GetPiece().GetPlayer() + 1) % 2))
                    {
                        return false;
                    }
                }

                var expectedRook = expectedRookSquare.GetPiece();
                if (expectedRook is Rook && expectedRook.GetPlayer() == from.GetPiece().GetPlayer() && expectedRook.GetMoveCount() == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool CheckBaseCastleConditions(BoardSquare from, BoardSquare to)
        {
            var movePiece = from.GetPiece();
            return movePiece is King &&
               Math.Abs(to.GetFile() - from.GetFile()) == 2 &&
               movePiece.GetMoveCount() == 0;
        }

        private static BoardSquare GetRookSquare(BoardState state, BoardSquare from, BoardSquare to)
        {
            return to.GetFile() < from.GetFile() ?
            state.GetBoard().GetSquare(0, from.GetRank()) :
            state.GetBoard().GetSquare(7, from.GetRank());
        }
    }
}
