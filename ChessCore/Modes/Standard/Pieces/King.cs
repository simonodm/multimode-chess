using ChessCore.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChessCore.Modes.Standard.Pieces
{
    /// <summary>
    /// Represents a standard king.
    /// </summary>
    public class King : StandardPiece
    {
        public King(int player) : base(player)
        {
            Value = 10;
            Symbol = "K";
            PossibleMoves = new HashSet<(int, int)>
            {
                (-2, 0),
                (-1, -1),
                (-1, 0),
                (-1, 1),
                (0, -1),
                (0, 1),
                (1, -1),
                (1, 0),
                (1, 1),
                (2, 0)
            };
        }

        /// <inheritdoc cref="StandardPiece.GetThreatenedSquares"/>
        public override IEnumerable<BoardSquare> GetThreatenedSquares(StandardBoardState state, BoardSquare from)
        {
            return base.GetThreatenedSquares(state, from).Where(square => Math.Abs(square.GetFile() - from.GetFile()) <= 1);
        }

        /// <inheritdoc cref="StandardPiece.GenerateMove"/>
        protected override StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (IsCastle(state, from, to))
            {
                return new MoveCastle()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            if (Math.Abs(from.GetFile() - to.GetFile()) <= 1)
            {
                return base.GenerateMove(state, from, to);
            }

            return null;
        }



        private bool IsCastle(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (Math.Abs(to.GetFile() - from.GetFile()) == 2)
            {
                if (IsCastlePositionValid(state, from))
                {
                    var rookSquare = GetRookSquare(state, from, to);
                    return IsCastleLegal(state, from, rookSquare);
                }
            }

            return false;
        }

        private bool IsCastlePositionValid(StandardBoardState state, BoardSquare from)
        {
            if ((GetPlayer() == 0 && from.GetRank() == 0) || (GetPlayer() == 1 && from.GetRank() == state.GetBoard().GetHeight() - 1))
            {
                return from.GetFile() == 4;
            }
            return false;
        }

        private bool IsCastleLegal(StandardBoardState state, BoardSquare from, BoardSquare rookSquare)
        {
            var rook = rookSquare.GetPiece();
            return GetMoveCount() == 0 &&
                   rook != null &&
                   rook.GetMoveCount() == 0 &&
                   !IsCastleLineBlocked(state, from, rookSquare);
        }

        private bool IsCastleLineBlocked(StandardBoardState state, BoardSquare kingSquare, BoardSquare rookSquare)
        {
            for (int file = Math.Min(rookSquare.GetFile(), kingSquare.GetFile()); file < Math.Max(rookSquare.GetFile(), kingSquare.GetFile()); file++)
            {
                var square = state.GetBoard().GetSquare(file, kingSquare.GetRank());

                if (Math.Abs(square.GetFile() - kingSquare.GetFile()) <= 2) // check threats up to king's target square
                {
                    if (state.GetThreatMap().GetThreatCount(square, (GetPlayer() + 1) % 2) > 0)
                    {
                        return true;
                    }
                }
                if (square != kingSquare && square != rookSquare) // check piece presence for all squares in-between
                {
                    if (square.GetPiece() != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private BoardSquare GetRookSquare(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (to.GetFile() == 6)
            {
                return state.GetBoard().GetSquare(7, from.GetRank());
            }
            else if (to.GetFile() == 2)
            {
                return state.GetBoard().GetSquare(0, from.GetRank());
            }
            throw new ChessCoreException("Invalid target square supplied.");
        }
    }
}
