using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Pieces;

namespace Chess.Game.Modes
{
    class PawnOfTheDeadRules : ClassicRules
    {
        protected override BoardState HandleCapture(Move move)
        {
            return base.HandleCapture(move);
        }
    }
}
