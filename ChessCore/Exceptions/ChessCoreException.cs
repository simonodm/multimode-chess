using System;

namespace ChessCore.Exceptions
{
    public class ChessCoreException : Exception
    {
        public ChessCoreException(string message) : base(message) { }
    }
}
