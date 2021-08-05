namespace ChessCore
{
    public struct BoardSquare
    {
        private int _file;
        private int _rank;
        private GamePiece _piece;

        public BoardSquare(int file, int rank, GamePiece piece = null)
        {
            _file = file;
            _rank = rank;
            _piece = piece;
        }

        public int GetFile()
        {
            return _file;
        }

        public int GetRank()
        {
            return _rank;
        }

        public GamePiece GetPiece()
        {
            return _piece;
        }

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
