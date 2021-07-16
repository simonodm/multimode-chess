using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    class MoveCastle : Move
    {
        public BoardState Handle()
        {
            var rookSquare = GetRookSquare();

            var rookTargetSquare = To.File > From.File ?
                BoardBefore.GetSquare(From.File + 1, From.Rank) :
                BoardBefore.GetSquare(From.File - 1, From.Rank);

            var rookMove = new MoveNormal
            {
                From = rookSquare,
                To = rookTargetSquare,
                Piece = rookSquare.Piece
            };

            return BoardBefore.Move(this).Move(rookMove);
        }

        public bool IsLegal(IGameRules rules)
        {
            if (CheckBaseCastleConditions())
            {
                for (int i = Math.Min(From.File, To.File); i <= Math.Max(From.File, To.File); i++)
                {
                    if (IsSquareUnderThreat(BoardBefore.GetSquare(i, From.Rank), rules))
                    {
                        return false;
                    }
                }
                var expectedRook = GetRookSquare().Piece;
                if (expectedRook is Rook && expectedRook.Player == Piece.Player && expectedRook.MoveCount == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckBaseCastleConditions()
        {
            return Piece is King &&
                Math.Abs(To.File - From.File) == 2 &&
                From.File == 4 &&
                Piece.MoveCount == 0 &&
                (From.Rank == 0 || From.Rank == 7);
        }

        private bool IsSquareUnderThreat(BoardSquare square, IGameRules rules)
        {
            var player = (Piece.Player + 1) % 2;
            foreach(var move in rules.GetAllLegalMoves(BoardBefore, player))
            {
                if(move.To == square)
                {
                    return true;
                }
            }
            return false;
        }

        private BoardSquare GetRookSquare()
        {
            if(To.File < From.File)
            {
                return BoardBefore.GetSquare(0, From.Rank);
            }
            else
            {
                return BoardBefore.GetSquare(7, From.Rank);
            }
        }
    }
}
