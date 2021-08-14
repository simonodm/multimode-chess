using System.Collections.Generic;

namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// Represents a piece used in the standard game mode.
    /// </summary>
    public abstract class StandardPiece : GamePiece
    {
        /// <summary>
        /// Represents all the piece's possible moves as a set of (X, Y) position offsets.
        /// </summary>
        protected HashSet<(int, int)> PossibleMoves;

        protected StandardPiece(int player) : base(player) { }

        /// <summary>
        /// Retrieves all legal moves for the piece in the given board state from the given square.
        /// </summary>
        /// <param name="state">Board state to calculate legal moves for</param>
        /// <param name="from">Board square to calculate legal moves from</param>
        /// <returns>An IReadOnlyList of legal moves</returns>
        public virtual IReadOnlyList<StandardMove> GetLegalMoves(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = GetVisibleSquares(state, from);
            var legalMoves = new List<StandardMove>();
            foreach (var square in threatenedSquares)
            {
                var move = GenerateMove(state, from, square);
                if (move == null) continue;

                if (move.IsUserInputRequired)
                {
                    move.SelectOption(move.Options[0]); // Selected option does not affect check after promotion, so we can just select the first one
                }

                if (move.Process().IsInCheck(GetPlayer())) continue;

                if (move.SelectedOption != null)
                {
                    move.UnselectOption();
                }

                legalMoves.Add(move);
            }

            return legalMoves;
        }

        /// <summary>
        /// Retrieves the list of all squares visible by the piece in the given board state from the given square.
        /// </summary>
        /// <param name="state">Board square to calculate visibility for</param>
        /// <param name="from">Board square to calculate visibility from</param>
        /// <returns>An enumerable of visible board squares</returns>
        public virtual IEnumerable<BoardSquare> GetVisibleSquares(StandardBoardState state, BoardSquare from)
        {
            var threatenedSquares = new List<BoardSquare>();
            foreach (var possibleMove in PossibleMoves)
            {
                if (IsOutOfBounds(possibleMove, state.GetBoard(), from)) continue;

                var squareTo = GetTargetSquare(possibleMove, state, from);
                if (!state.IsLineBlocked(from, squareTo))
                {
                    threatenedSquares.Add(squareTo);
                }
            }
            return threatenedSquares;
        }

        /// <summary>
        /// Retrieves the list of all squares threatened by the piece in the given board state from the given square.
        /// </summary>
        /// <param name="state">Board square to calculate threats for</param>
        /// <param name="from">Board square to calculate threats from</param>
        /// <returns>An enumerable of threatened board squares</returns>
        public virtual IEnumerable<BoardSquare> GetThreatenedSquares(StandardBoardState state, BoardSquare from)
        {
            return GetVisibleSquares(state, from);
        }

        /// <summary>
        /// Determines whether the given (X, Y) offset is out of bounds of the given board, starting with the given square.
        /// </summary>
        /// <param name="possibleMove">(X, Y) move offset</param>
        /// <param name="board">Board</param>
        /// <param name="from">Starting square</param>
        /// <returns>true if the offset is out of bounds, false otherwise</returns>
        protected bool IsOutOfBounds((int offsetFile, int offsetRank) possibleMove, Board board, BoardSquare from)
        {
            int newFile = from.GetFile() + possibleMove.offsetFile;
            int newRank = from.GetRank() + possibleMove.offsetRank;
            return newFile < 0 || newFile >= board.GetWidth() || newRank < 0 || newRank >= board.GetHeight();
        }

        /// <summary>
        /// Generates the correct StandardMove type for the move between the supplied squares.
        /// </summary>
        /// <param name="state">Board state to calculate move for</param>
        /// <param name="from">Start square</param>
        /// <param name="to">End square</param>
        /// <returns>A move object if a move is legal, null otherwise</returns>
        protected virtual StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            StandardMove move = null;

            if (state.IsLineBlocked(from, to)) return move;

            if (to.GetPiece() != null && to.GetPiece().GetPlayer() != GetPlayer())
            {
                move = new MoveCapture()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if (to.GetPiece() == null)
            {
                move = new MoveNormal()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }

            return move;
        }

        /// <summary>
        /// Retrieves the target BoardSquare for the supplied (X, Y) offset from the start square.
        /// </summary>
        /// <param name="possibleMove">(X, Y) move offset</param>
        /// <param name="state">Board state to retrieve the target BoardSquare for</param>
        /// <param name="from">Start square</param>
        /// <returns>A BoardSquare object</returns>
        protected BoardSquare GetTargetSquare((int offsetFile, int offsetRank) possibleMove, BoardState state, BoardSquare from)
        {
            int newFile = from.GetFile() + possibleMove.offsetFile;
            int newRank = from.GetRank() + possibleMove.offsetRank;
            return state.GetBoard().GetSquare(newFile, newRank);
        }
    }
}
