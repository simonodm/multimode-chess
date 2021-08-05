namespace ChessCore.Exceptions
{
    public class InvalidMoveException : ChessCoreException
    {
        public InvalidMoveException(string message) : base(message) { }
    }
}
