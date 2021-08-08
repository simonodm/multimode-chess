using ChessCore.Modes.Standard.Pieces;

namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// A BoardState override used for internal calculations of standard rules.
    /// </summary>
    public class StandardBoardState : BoardState
    {
        private readonly object _threatMapLock = new object();

        private BoardSquare _whiteKingSquare;
        private BoardSquare _blackKingSquare;
        private ThreatMap _threatMap;

        public StandardBoardState(Board board, Move lastMove = null) : base(board, lastMove)
        {
            InitKings();
        }

        /// <summary>
        /// Retrieves the <c>ThreatMap</c> for this board state. ThreatMap is instantiated lazily.
        /// </summary>
        /// <returns>Threat map for this state.</returns>
        internal ThreatMap GetThreatMap()
        {
            lock (_threatMapLock)
            {
                return _threatMap ??= new ThreatMap(this);
            }
        }

        /// <summary>
        /// Retrieves the square on which the supplied player's King is located.
        /// </summary>
        /// <param name="player">Player whose King to retrieve</param>
        /// <returns>A square on which the player's King is located.</returns>
        internal BoardSquare GetKingSquare(int player)
        {
            return player == 0 ? _whiteKingSquare : _blackKingSquare;
        }

        /// <summary>
        /// Checks whether the supplied player is in check.
        /// </summary>
        /// <param name="player">Player to determine check for.</param>
        /// <returns>true if the supplied player is in check, false otherwise</returns>
        internal bool IsInCheck(int player)
        {
            var kingSquare = GetKingSquare(player);
            return GetThreatMap().GetThreatCount(kingSquare, (player + 1) % 2) > 0;
        }

        /// <summary>
        /// Checks whether there is a piece located on the line between the supplied squares (excluded).
        /// </summary>
        /// <param name="from">First square</param>
        /// <param name="to">Second square</param>
        /// <returns>true if there is a piece found on the line, false otherwise</returns>
        internal bool IsLineBlocked(BoardSquare from, BoardSquare to)
        {
            bool pieceFound = false;
            int startFile = from.GetFile();
            int endFile = to.GetFile();
            int startRank = from.GetRank();
            int endRank = to.GetRank();
            int i = startFile;
            int j = startRank;

            while (i != endFile || j != endRank)
            {
                if (i != startFile || j != startRank)
                {
                    var blockingPiece = GetBoard().GetSquare(i, j).GetPiece();
                    if (blockingPiece != null && blockingPiece != from.GetPiece())
                    {
                        pieceFound = true;
                    }
                    if (pieceFound)
                    {
                        return true;
                    }
                }
                if (startFile < endFile && i < endFile) i++;
                if (startFile > endFile && i > endFile) i--;
                if (startRank < endRank && j < endRank) j++;
                if (startRank > endRank && j > endRank) j--;
            }

            return false;
        }

        private void InitKings()
        {
            var kings = FindPieces<King>();
            foreach (var kingSquare in kings)
            {
                var king = kingSquare.GetPiece();
                if (king.GetPlayer() == 0)
                {
                    _whiteKingSquare = kingSquare;
                }
                else
                {
                    _blackKingSquare = kingSquare;
                }
            }
        }
    }
}
