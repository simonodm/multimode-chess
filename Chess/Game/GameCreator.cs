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
        // TODO: change to builder
        public static ChessGame CreateGame<TRules>(Board board = null, int timeLimit = 600, int increment = 0, bool vsAi = false)
            where TRules : IGameRules, new()
        {
            var rules = new TRules();
            return board == null ?
                new ChessGame(rules, timeLimit, increment, vsAi) :
                new ChessGame(rules, board, timeLimit, increment, vsAi);
        }

        public static ChessGame CreateFromModeId(int modeId, Board board = null, int timeLimit = 600, int increment = 0, bool vsAi = false)
        {
            switch (modeId)
            {
                case 1:
                    return CreateGame<PawnOfTheDeadRules>(board, timeLimit, increment, vsAi);
                case 0:
                default:
                    return CreateGame<ClassicRules>(board, timeLimit, increment, vsAi);
            }
        }
    }
}
