namespace ChessCore
{
    public interface IBoardEvaluator
    {
        /// <summary>
        /// Returns the position's score based on players' total piece value and threat coverage.
        /// </summary>
        /// <param name="state"></param>
        /// <returns>Position's evaluated score</returns>
        public double GetBoardScore(BoardState state);
    }
}
