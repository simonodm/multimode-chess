namespace ChessCore.Modes
{
    /// <summary>
    /// Abstracts away the creation, storage, and reuse of IGameRules instances. This is the preferred way of working with specific IGameRules instances.
    /// </summary>
    public static class GameModePool
    {
        private static class GameModeContainer<TRules> where TRules : IGameRules, new()
        {
            private static TRules _gameMode;
            public static TRules Get()
            {
                if (_gameMode == null)
                {
                    _gameMode = new TRules();
                }

                return _gameMode;
            }
        }

        /// <summary>
        /// Retrieves the stored TRules instance. Creates one on first call.
        /// </summary>
        /// <typeparam name="TRules">IGameRules implementation</typeparam>
        /// <returns>The stored TRules instance.</returns>
        public static TRules Get<TRules>() where TRules : IGameRules, new()
        {
            return GameModeContainer<TRules>.Get();
        }
    }
}
