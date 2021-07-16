using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    interface IMoveFactory
    {
        public Move GetMove(BoardState state, BoardSquare from, BoardSquare to);
    }
}
