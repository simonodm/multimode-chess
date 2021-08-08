namespace ChessCore
{
    /// <summary>
    /// Represents the result of a single minimax evaluation of a state.
    /// </summary>
    public class MinimaxResult
    {
        /// <summary>
        /// The state which was evaluated.
        /// </summary>
        public BoardState State { get; init; }

        /// <summary>
        /// The raw score of the state. Positive score means that the max (first) player has the advantage, negative score means that the min (second) player has the advantage.
        /// </summary>
        public double Score { get; init; }

        /// <summary>
        /// The best move for the currently playing player according to evaluation.
        /// </summary>
        public Move BestMove { get; init; }

        /// <summary>
        /// True if a game end can be forced, false otherwise.
        /// </summary>
        public bool GameOverCanBeForced { get; init; }

        /// <summary>
        /// The player who can force the game over. This property is only relevant if GameOverCanBeForced is true.
        /// </summary>
        public int ForcedWinner { get; init; }
        internal MinimaxResult(BoardState state, double score, Move bestMove = null, bool gameOverCanBeForced = false, int mateBy = 0)
        {
            State = state;
            Score = score;
            BestMove = bestMove;
            GameOverCanBeForced = gameOverCanBeForced;
            ForcedWinner = mateBy;
        }
    }
}
