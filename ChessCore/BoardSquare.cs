namespace ChessCore
{
    /// <summary>
    /// Represents a single board square.
    /// </summary>
    public struct BoardSquare
    {
        private readonly int _file;
        private readonly int _rank;
        private GamePiece _piece;

        public BoardSquare(int file, int rank, GamePiece piece = null)
        {
            _file = file;
            _rank = rank;
            _piece = piece;
        }

        /// <summary>
        /// Retrieves the square's file.
        /// </summary>
        /// <returns>Square file</returns>
        public int GetFile()
        {
            return _file;
        }

        /// <summary>
        /// Retrieves the square's rank.
        /// </summary>
        /// <returns>Square rank</returns>
        public int GetRank()
        {
            return _rank;
        }

        /// <summary>
        /// Retrieves the piece present on the square.
        /// </summary>
        /// <returns>An instance of GamePiece if there is a piece present, null otherwise</returns>
        public GamePiece GetPiece()
        {
            return _piece;
        }

        /// <summary>
        /// Updates the piece on the square. Previous piece is discarded.
        /// </summary>
        /// <param name="piece">New piece to add to the square</param>
        public void SetPiece(GamePiece piece)
        {
            _piece = piece;
        }

        public static bool operator ==(BoardSquare squareA, BoardSquare squareB)
        {
            return squareA.Equals(squareB);
        }

        public static bool operator !=(BoardSquare squareA, BoardSquare squareB)
        {
            return !squareA.Equals(squareB);
        }

        public override bool Equals(object obj)
        {
            if (obj is not BoardSquare)
            {
                return false;
            }

            var otherSquare = (BoardSquare)obj;
            return otherSquare._file == _file &&
                otherSquare._rank == _rank &&
                otherSquare._piece == _piece;
        }

        public override int GetHashCode()
        {
            int hash = 19;

            hash = (hash * 13) + _file.GetHashCode();
            hash = (hash * 13) + _rank.GetHashCode();
            hash = (hash * 13) + _piece.GetHashCode();

            return hash;
        }
    }
}
