namespace ChessCore.Exceptions
{
    public class InvalidBoardException : ChessCoreException
    {
        public InvalidBoardException(string message) : base(message) { }
    }
}
