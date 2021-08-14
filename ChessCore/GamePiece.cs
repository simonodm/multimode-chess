namespace ChessCore
{
    /// <summary>
    /// The base class for a game piece.
    /// </summary>
    public abstract class GamePiece
    {
        protected int Value = 0;
        protected string Symbol = "";
        private readonly int _player;
        private int _moveCount;
        private readonly object _moveCountLock = new object();

        /// <summary>
        /// Initializes the piece for the given player.
        /// </summary>
        /// <param name="player">Player to own the piece</param>
        protected GamePiece(int player)
        {
            _player = player;
        }

        /// <summary>
        /// Retrieves the piece's value.
        /// </summary>
        /// <returns>An integer representing the piece's absolute value</returns>
        public int GetValue()
        {
            return Value;
        }

        /// <summary>
        /// Retrieves the piece's symbol in the game's move notation.
        /// </summary>
        /// <returns>The piece's symbol</returns>
        public string GetSymbol()
        {
            return Symbol;
        }

        /// <summary>
        /// Retrieves the player who owns the piece.
        /// </summary>
        /// <returns>0 if white, 1 if black</returns>
        public int GetPlayer()
        {
            return _player;
        }

        /// <summary>
        /// Retrieves the amount of times this piece has moved.
        /// </summary>
        /// <returns>An integer representing the move count</returns>
        public int GetMoveCount()
        {
            lock (_moveCountLock)
            {
                return _moveCount;
            }
        }

        /// <summary>
        /// Updates the piece's move count to the specified value.
        /// </summary>
        /// <param name="moveCount">New move count</param>
        public void SetMoveCount(int moveCount)
        {
            lock (_moveCountLock)
            {
                _moveCount = moveCount;
            }
        }
    }
}
