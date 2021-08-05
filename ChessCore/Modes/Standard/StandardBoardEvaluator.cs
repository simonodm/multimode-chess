namespace ChessCore.Modes.Standard
{
    class StandardBoardEvaluator : IBoardEvaluator
    {
        private IGameRules _rules;

        public StandardBoardEvaluator(IGameRules rules)
        {
            _rules = rules;
        }

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
                else if (gameResult == GameResult.BLACK_WIN)
                {
                    return double.MinValue;
                }
                else
                {
                    return 0;
                }
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
            if (square.GetPiece() != null)
            {
                if (square.GetPiece().GetPlayer() == 0)
                {
                    return square.GetPiece().GetValue();
                }
                else
                {
                    return -square.GetPiece().GetValue();
                }
            }

            return 0;
        }

        private double CalculateThreatScore(StandardBoardState state, BoardSquare square)
        {
            const double THREAT_MULTIPLIER = 0.1;

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

            whiteThreatScore *= THREAT_MULTIPLIER;
            blackThreatScore *= THREAT_MULTIPLIER;

            return whiteThreatScore - blackThreatScore;
        }

    }
}
