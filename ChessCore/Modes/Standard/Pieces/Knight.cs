using System.Collections.Generic;

namespace ChessCore.Game.Modes.Standard.Pieces
{
    public class Knight : StandardPiece
    {
        public Knight(int player) : base(player)
        {
            _value = 3;
            _symbol = "N";
            PossibleMoves = new HashSet<(int, int)> {
                (-2, -1),
                (-2, +1),
                (+2, -1),
                (+2, +1),
                (-1, -2),
                (-1, +2),
                (+1, -2),
                (+1, +2)
            };
        }

        public override List<BoardSquare> GetThreatenedSquares(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = new List<BoardSquare>();

            foreach(var possibleMove in PossibleMoves)
            {
                if(!IsOutOfBounds(possibleMove, state, from))
                {
                    var target = GetTargetSquare(possibleMove, state, from);
                    threatenedSquares.Add(target);
                }
            }

            return threatenedSquares;
        }

        protected override StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            var targetPiece = to.GetPiece();
            if(targetPiece == null)
            {
                return new MoveNormal
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if(targetPiece.GetPlayer() != GetPlayer())
            {
                return new MoveCapture
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            return null;
        }


    }
}
