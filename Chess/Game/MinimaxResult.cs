using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class MinimaxResult
    {
        public BoardState State { get; private set; }
        public double Score { get; private set; }
        public Move? BestMove { get; private set; }
        public MinimaxResult(BoardState state, double score, Move? bestMove = null)
        {
            State = state;
            Score = score;
            BestMove = bestMove;
        }
    }
}
