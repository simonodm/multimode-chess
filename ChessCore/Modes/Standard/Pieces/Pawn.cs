using System;
using System.Collections.Generic;

namespace ChessCore.Modes.Standard.Pieces
{
    /// <summary>
    /// Represents a standard pawn.
    /// </summary>
    public class Pawn : StandardPiece
    {
        public Pawn(int player) : base(player)
        {
            Value = 1;
            Symbol = "";
            PossibleMoves = new HashSet<(int, int)>
            {
                (0, -2),
                (0, -1),
                (0, 1),
                (0, 2),
                (1, 1),
                (-1, 1),
                (1, -1),
                (-1, -1)
            };
        }

        /// <inheritdoc cref="StandardPiece.GetVisibleSquares"/>
        public override IEnumerable<BoardSquare> GetVisibleSquares(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = new List<BoardSquare>();

            foreach (var possibleMove in PossibleMoves)
            {
                if (IsOutOfBounds(possibleMove, state.GetBoard(), from)) continue;

                var to = GetTargetSquare(possibleMove, state, from);
                if (IsDirectionCorrect(from, to) && (IsValidMoveForward(state, from, to) || IsValidMoveDiagonal(state, from, to)))
                {
                    threatenedSquares.Add(to);
                }
            }

            return threatenedSquares;
        }

        /// <inheritdoc cref="StandardPiece.GenerateMove"/>
        protected override StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (!IsDirectionCorrect(from, to))
            {
                return null;
            }

            StandardMove move = null;
            if (IsEnPassant(state, to))
            {
                move = new MoveEnPassant()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if (IsPromotion(state, from, to))
            {
                move = new MovePromotion()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if (IsNormal(state, from, to))
            {
                move = new MoveNormal()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if (IsCapture(state, from, to))
            {
                move = new MoveCapture()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }

            return move;
        }

        private bool IsDirectionCorrect(BoardSquare from, BoardSquare to)
        {
            return (GetPlayer() == 0 && to.GetRank() > from.GetRank()) ||
                (GetPlayer() == 1 && to.GetRank() < from.GetRank());
        }

        private bool IsEnPassant(StandardBoardState state, BoardSquare to)
        {
            var previousMove = state.GetLastMove();

            if (previousMove?.Piece is Pawn && Math.Abs(previousMove.To.GetRank() - previousMove.From.GetRank()) == 2)
            {
                var enPassantRank = previousMove.From.GetRank() + (previousMove.To.GetRank() - previousMove.From.GetRank()) / 2;
                var enPassantSquare = state.GetBoard().GetSquare(previousMove.From.GetFile(), enPassantRank);
                return to == enPassantSquare;
            }

            return false;
        }

        private bool IsPromotion(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            bool isFinalRank = (GetPlayer() == 0 && to.GetRank() == state.GetBoard().GetHeight() - 1) ||
                (GetPlayer() == 1 && to.GetRank() == 0);

            return isFinalRank && (IsValidMoveDiagonal(state, from, to) || IsValidMoveForward(state, from, to));
        }

        private bool IsNormal(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            return IsValidMoveForward(state, from, to);
        }

        private bool IsCapture(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            return IsValidMoveDiagonal(state, from, to);
        }

        private bool IsValidMoveForward(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (to.GetPiece() != null)
            {
                return false;
            }

            if (from.GetFile() == to.GetFile())
            {
                if (Math.Abs(to.GetRank() - from.GetRank()) == 2)
                {
                    return !state.IsLineBlocked(from, to) && ((GetPlayer() == 0 && from.GetRank() == 1) ||
                    (GetPlayer() == 1 && from.GetRank() == 6));
                }

                return to.GetPiece() == null;
            }

            return false;
        }

        private bool IsValidMoveDiagonal(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if (from.GetFile() != to.GetFile())
            {
                return (to.GetPiece() != null && to.GetPiece().GetPlayer() != GetPlayer()) || IsEnPassant(state, to);
            }

            return false;
        }
    }
}
