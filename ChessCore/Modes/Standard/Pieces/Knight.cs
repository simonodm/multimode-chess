using System.Collections.Generic;

namespace ChessCore.Modes.Standard.Pieces
{
    /// <summary>
    /// Represents a standard knight.
    /// </summary>
    public class Knight : StandardPiece
    {
        public Knight(int player) : base(player)
        {
            Value = 3;
            Symbol = "N";
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

        /// <inheritdoc cref="StandardPiece.GetThreatenedSquares"/>
        public override List<BoardSquare> GetThreatenedSquares(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = new List<BoardSquare>();

            foreach (var possibleMove in PossibleMoves)
            {
                if (!IsOutOfBounds(possibleMove, state.GetBoard(), from))
                {
                    var target = GetTargetSquare(possibleMove, state, from);
                    threatenedSquares.Add(target);
                }
            }

            return threatenedSquares;
        }

        /// <inheritdoc cref="StandardPiece.GenerateMove"/>
        protected override StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            var targetPiece = to.GetPiece();
            if (targetPiece == null)
            {
                return new MoveNormal
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if (targetPiece.GetPlayer() != GetPlayer())
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
