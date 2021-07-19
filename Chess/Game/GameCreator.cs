using Chess.Game.Modes;
using Chess.Game.Modes.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    static class GameCreator
    {
        public static ChessGame CreateGame<TRules>(int timeLimit = 600, int increment = 0, bool vsAi = false)
            where TRules : IGameRules, new()
        {
            var rules = new TRules();
            return new ChessGame(rules, timeLimit, increment, vsAi);
        }

        public static ChessGame CreateFromModeId(int modeId, int timeLimit = 600, int increment = 0, bool vsAi = false)
        {
            switch (modeId)
            {
                case 1:
                    return CreateGame<PawnOfTheDeadRules>(timeLimit, increment, vsAi);
                case 0:
                default:
                    return CreateGame<ClassicRules>(timeLimit, increment, vsAi);
            }
        }
    }
}
