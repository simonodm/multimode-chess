using ChessCore.Modes.Standard.Pieces;

namespace ChessCore.Modes.Standard
{
    public class StandardBoardState : BoardState
    {
        private object _threatMapLock = new object();

        private BoardSquare _whiteKingSquare;
        private BoardSquare _blackKingSquare;
        private ThreatMap _threatMap;

        public StandardBoardState(int width, int height, Move lastMove = null) : base(width, height, lastMove)
        {
            InitKings();
        }
        public StandardBoardState(Board board, Move lastMove = null) : base(board, lastMove)
        {
            InitKings();
        }

        internal ThreatMap GetThreatMap()
        {
            lock (_threatMapLock)
            {
                if (_threatMap == null)
                {
                    _threatMap = new ThreatMap(this);
                }
                return _threatMap;
            }
        }

        internal BoardSquare GetKingSquare(int player)
        {
            if (player == 0)
            {
                return _whiteKingSquare;
            }
            else
            {
                return _blackKingSquare;
            }
        }

        internal bool IsInCheck(int player)
        {
            var kingSquare = GetKingSquare(player);
            return GetThreatMap().GetThreatCount(kingSquare, (player + 1) % 2) > 0;
        }

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
