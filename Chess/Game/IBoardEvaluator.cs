using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Modes;

namespace Chess.Game
{
    interface IBoardEvaluator
    {
        public double GetBoardScore(BoardState state);
    }
}
