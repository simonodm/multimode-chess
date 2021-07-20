using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class StandardBoardState : BoardState
    {
        private BoardSquare _whiteKing;
        private BoardSquare _blackKing;
        private Dictionary<BoardSquare, List<Move>> _cachedNonBlockedMoves
            = new Dictionary<BoardSquare, List<Move>>();

        public StandardBoardState(int width, int height, Move lastMove = null) : base(width, height, lastMove)
        {
            InitKings();
        }
        public StandardBoardState(Board board, Move lastMove = null) : base(board, lastMove)
        {
            InitKings();
        }

        public bool IsSquareUnderThreat(BoardSquare square, int byPlayer)
        {
            // TODO: holy shit just refactor this
            return IsSquareUnderThreatFrom(square, new Pawn((byPlayer + 1) % 2)) ||
                IsSquareUnderThreatFrom(square, new Knight((byPlayer + 1) % 2)) ||
                IsSquareUnderThreatFrom(square, new Bishop((byPlayer + 1) % 2)) ||
                IsSquareUnderThreatFrom(square, new Rook((byPlayer + 1) % 2)) ||
                IsSquareUnderThreatFrom(square, new Queen((byPlayer + 1) % 2)) ||
                IsSquareUnderThreatFrom(square, new King((byPlayer + 1) % 2));
        }
        public BoardSquare GetPlayerKing(int player)
        {
            if(player == 0)
            {
                return _whiteKing;
            }
            else
            {
                return _blackKing;
            }
        }

        public List<Move> GetNonBlockedMoves(BoardSquare square)
        {
            if(_cachedNonBlockedMoves.ContainsKey(square))
            {
                return _cachedNonBlockedMoves[square];
            }
            var moveList = new List<Move>();
            if(square.GetPiece() != null)
            {
                foreach(var targetSquare in square.GetPiece().GetPossibleMoves(this, square))
                {
                    moveList.Add(new Move(GameModePool.Get<ClassicRules>())
                    {
                        BoardBefore = this,
                        From = square,
                        To = targetSquare,
                        Piece = square.GetPiece()
                    });
                }
            }

            _cachedNonBlockedMoves[square] = moveList;
            return moveList;
        }

        public bool IsInCheck(int player)
        {
            return IsSquareUnderThreat(GetPlayerKing(player), (player + 1) % 2);
        }

        private bool IsSquareUnderThreatFrom(BoardSquare square, GamePiece piece)
        {
            var squareMock = new BoardSquare(square.GetFile(), square.GetRank(), piece);
            var moves = GetNonBlockedMoves(squareMock);
            foreach (var move in moves)
            {
                // TODO: pawn and king special moves
                if (move.To.GetPiece() != null && move.To.GetPiece().GetType() == piece.GetType() && move.To.GetPiece().GetPlayer() != piece.GetPlayer())
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsLineBlocked(GamePiece piece, BoardSquare from, BoardSquare to)
        {
            if (piece is Knight)
            {
                return false;
            }

            bool pieceFound = false;
            int startFile = from.GetFile();
            int endFile = to.GetFile();
            int startRank = from.GetRank();
            int endRank = to.GetRank();
            int i = startFile;
            int j = startRank;

            while (i != endFile || j != endRank)
            {
                if(i != startFile || j != startRank)
                {
                    var blockingPiece = GetBoard().GetSquare(i, j).GetPiece();
                    if (blockingPiece != null && blockingPiece != piece)
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

            if (to.GetPiece() != null && to.GetPiece().GetPlayer() == from.GetPiece().GetPlayer())
            {
                return true;
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
                    _whiteKing = kingSquare;
                }
                else
                {
                    _blackKing = kingSquare;
                }
            }
        }
    }
}
