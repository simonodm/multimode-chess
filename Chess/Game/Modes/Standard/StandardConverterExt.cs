using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    static class StandardConverterExt
    {
        public static StandardBoardState ToStandardBoardState(this BoardState state)
        {
            return new StandardBoardState(state.GetBoard(), state.GetLastMove());
        }

        public static ClassicMove ToClassicMove(this Move move)
        {
            return ClassicMoveGenerator.GetMove(move.BoardBefore.ToStandardBoardState(), move);
        }
    }
}
