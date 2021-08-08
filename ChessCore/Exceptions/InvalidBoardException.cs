namespace ChessCore.Exceptions
{
    /// <summary>
    /// An exception for board-related errors.
    /// </summary>
    public class InvalidBoardException : ChessCoreException
    {
        public InvalidBoardException(string message) : base(message) { }
    }
}
