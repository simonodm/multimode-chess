﻿namespace ChessCore.Game
{
    public interface IBoardEvaluator
    {
        public double GetBoardScore(BoardState state);
    }
}
