using ChessCore.Modes;
using ChessCore.Modes.Standard;

namespace ChessCore
{
    /// <summary>
    /// A fluent API for building a chess game with the specified parameters.
    /// </summary>
    public class GameBuilder
    {
        private readonly IGameRules _rules = GameModePool.Get<StandardRules>();
        private readonly Board _board;

        public GameBuilder() { }

        private GameBuilder(IGameRules rules, Board board)
        {
            _rules = rules;
            _board = board;
        }

        /// <summary>
        /// Sets the game mode.
        /// </summary>
        /// <param name="rules">IGameRules implementation</param>
        /// <returns>A new GameBuilder with the specified game mode set</returns>
        public GameBuilder WithGameMode(IGameRules rules)
        {
            return new GameBuilder(rules, _board);
        }

        /// <summary>
        /// Sets the game starting board.
        /// </summary>
        /// <param name="board">Starting board</param>
        /// <returns>A new GameBuilder with the specified board set</returns>
        public GameBuilder WithBoard(Board board)
        {
            return new GameBuilder(_rules, board);
        }

        /// <summary>
        /// Creates the game.
        /// </summary>
        /// <returns>A ChessGame instance with the configured parameters.</returns>
        public ChessGame Create()
        {
            if (_board != null)
            {
                return new ChessGame(_rules, _board);
            }
            return new ChessGame(_rules);
        }
    }
}
