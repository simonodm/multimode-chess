namespace ChessCore.Modes
{
    public static class GameModePool
    {
        private static class GameModeContainer<T> where T : IGameRules, new()
        {
            private static T _gameMode;
            public static T Get()
            {
                if (_gameMode == null)
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
