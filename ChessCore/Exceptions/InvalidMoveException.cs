namespace ChessCore.Exceptions
{
    /// <summary>
    /// An exception for move-related errors.
    /// </summary>
    public class InvalidMoveException : ChessCoreException
    {
        public InvalidMoveException(string message) : base(message) { }
    }
}
