using System;
using System.Runtime.Serialization;

namespace Chess.Controls
{
    [Serializable]
    internal class ChessCoreException : Exception
    {
        public ChessCoreException()
        {
        }

        public ChessCoreException(string message) : base(message)
        {
        }

        public ChessCoreException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChessCoreException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}