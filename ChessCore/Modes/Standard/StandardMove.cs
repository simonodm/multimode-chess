namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// A Move override used for internal calculations of standard rules.
    /// </summary>
    public abstract class StandardMove : Move
    {
        protected StandardMove() : base(GameModePool.Get<StandardRules>()) { }

        /// <summary>
        /// Processes the move according to standard rules.
        /// </summary>
        /// <returns>Board state after processing the move</returns>
        public abstract StandardBoardState Process();
    }
}
