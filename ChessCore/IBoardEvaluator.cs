namespace ChessCore
{
    public interface IBoardEvaluator
    {
        public double GetBoardScore(BoardState state);
    }
}
