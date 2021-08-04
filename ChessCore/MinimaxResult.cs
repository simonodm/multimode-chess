namespace ChessCore.Game
{
    public class MinimaxResult
    {
        public BoardState State { get; init; }
        public double Score { get; init; }
        public Move BestMove { get; init; }
        public bool IsGameOver { get; init; }
        public int Winner { get; init; }
        public MinimaxResult(BoardState state, double score, Move bestMove = null, bool isGameOver = false, int mateBy = 0)
        {
            State = state;
            Score = score;
            BestMove = bestMove;
            IsGameOver = isGameOver;
            Winner = mateBy;
        }
    }
}
