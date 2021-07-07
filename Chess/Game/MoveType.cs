using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    enum MoveType
    {
        MOVE_NORMAL,
        MOVE_CAPTURE,
        MOVE_CASTLE,
        MOVE_PROMOTION,
        MOVE_SPECIAL,
        MOVE_ILLEGAL
    }
}
