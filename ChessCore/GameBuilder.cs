using ChessCore.Game.Modes;
using ChessCore.Game.Modes.PawnOfTheDead;
using ChessCore.Game.Modes.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessCore.Game
{
    public class GameBuilder
    {
        private IGameRules _rules = GameModePool.Get<StandardRules>();
        private Board _board;

        public GameBuilder() { }

        private GameBuilder(IGameRules rules, Board board)
        {
            _rules = rules;
            _board = board;
        }

        public GameBuilder WithGameMode<TMode>() where TMode : IGameRules, new()
        {
            return new GameBuilder(GameModePool.Get<TMode>(), _board);
        }

        public GameBuilder WithGameMode(IGameRules rules)
        {
            return new GameBuilder(rules, _board);
        }

        public GameBuilder WithBoard(Board board)
        {
            return new GameBuilder(_rules, board);
        }

        public ChessGame Create()
        {
            if(_board != null)
            {
                return new ChessGame(_rules, _board);
            }
            return new ChessGame(_rules);
        }
    }
}
