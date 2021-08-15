using System.Collections.Generic;
using System.Linq;

namespace ChessCore
{
    /// <summary>
    /// Represents a board state.
    /// </summary>
    public class BoardState
    {
        private readonly object _scoreLock = new object();

        private readonly Move _lastMove;
        private readonly Board _board;
        private MinimaxResult _score;

        public BoardState(Board board, Move lastMove = null)
        {
            _board = board;
            _lastMove = lastMove;
        }

        /// <summary>
        /// Retrieves the state's board.
        /// </summary>
        /// <returns>A Board instance</returns>
        public Board GetBoard()
        {
            return _board;
        }

        /// <summary>
        /// Retrieves the move which lead to this state.
        /// </summary>
        /// <returns>The previous move if this state is not the initial state, null otherwise</returns>
        public Move GetLastMove()
        {
            return _lastMove;
        }

        /// <summary>
        /// Retrieves the state's minimax score. The score must be set externally prior to calling this method.
        /// </summary>
        /// <returns>A MinimaxResult instance if a score was set, null otherwise</returns>
        public MinimaxResult GetScore()
        {
            lock (_scoreLock)
            {
                return _score;
            }
        }

        /// <summary>
        /// Updates the state's minimax score
        /// </summary>
        /// <param name="score">Minimax score</param>
        internal void SetScore(MinimaxResult score)
        {
            lock (_scoreLock)
            {
                _score = score;
            }
        }

        /// <summary>
        /// Enumerates all the pieces of the given type found on the board.
        /// </summary>
        /// <typeparam name="TPiece">A piece type</typeparam>
        /// <returns>An enumerable of board squares</returns>
        public IEnumerable<BoardSquare> FindPieces<TPiece>() where TPiece : GamePiece
        {
            return _board.GetAllSquares().Where(square => square.GetPiece() is TPiece);
        }
    }
}
