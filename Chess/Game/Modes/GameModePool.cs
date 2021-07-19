using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    static class GameModePool
    {
        private static class GameModeContainer<T> where T : IGameRules, new()
        {
            private static T _gameMode;
            public static T Get()
            {
                if(_gameMode == null)
                {
                    _gameMode = new T();
                }
                return _gameMode;
            }
        }

        public static T Get<T>() where T : IGameRules, new()
        {
            return GameModeContainer<T>.Get();
        }
    }
}
