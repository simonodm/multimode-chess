namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// Represents a standard rules board evaluator. This class only calculates the immediate position's score (it does not minimax).
    /// </summary>
    internal class StandardBoardEvaluator : IBoardEvaluator
    {
        private readonly IGameRules _rules;

        public StandardBoardEvaluator(IGameRules rules)
        {
            _rules = rules;
        }

        /// <inheritdoc cref="IBoardEvaluator"/>
        public double GetBoardScore(BoardState state)
        {
            double score = 0;
            var convertedState = state.ToStandardBoardState();

            var gameResult = _rules.GetGameResult(convertedState);
            if (gameResult != GameResult.ONGOING)
            {
                if (gameResult == GameResult.WHITE_WIN)
                {
                    return double.MaxValue;
                }
                if (gameResult == GameResult.BLACK_WIN)
                {
                    return double.MinValue;
                }
                return 0;
            }

            foreach (var square in convertedState.GetBoard().GetAllSquares())
            {
                score += CalculatePieceValue(square);
                score += CalculateThreatScore(convertedState, square);
            }

            return score;
        }

        private int CalculatePieceValue(BoardSquare square)
        {
            if (square.GetPiece() == null) return 0;
            if (square.GetPiece().GetPlayer() == 0)
            {
                return square.GetPiece().GetValue();
            }
            else
            {
                return -square.GetPiece().GetValue();
            }
        }

        private double CalculateThreatScore(StandardBoardState state, BoardSquare square)
        {
            const double threatMultiplier = 0.1;

            double whiteThreatScore = state.GetThreatMap().GetThreatCount(square, 0);
            if (square.GetPiece() != null && square.GetPiece().GetPlayer() == 1)
            {
                whiteThreatScore *= square.GetPiece().GetValue();
            }

            double blackThreatScore = state.GetThreatMap().GetThreatCount(square, 1);
            if (square.GetPiece() != null && square.GetPiece().GetPlayer() == 0)
            {
                blackThreatScore *= square.GetPiece().GetValue();
            }

            whiteThreatScore *= threatMultiplier;
            blackThreatScore *= threatMultiplier;

            return whiteThreatScore - blackThreatScore;
        }

    }
}
