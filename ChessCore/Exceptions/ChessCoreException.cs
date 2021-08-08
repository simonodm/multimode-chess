using System;

namespace ChessCore.Exceptions
{
    /// <summary>
    /// A base class for all engine-related exceptions.
    /// </summary>
    public class ChessCoreException : Exception
    {
        public ChessCoreException(string message) : base(message) { }
    }
}
