using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    static class StandardUtils
    {
        public static bool IsMoveBlocked(Move move)
        {
            if (move.Piece is Knight)
            {
                return false;
            }

            bool pieceFound = false;
            int startFile = move.From.GetFile();
            int endFile = move.To.GetFile();
            int startRank = move.From.GetRank();
            int endRank = move.To.GetRank();
            int i = startFile;
            int j = startRank;

            while (i != endFile || j != endRank)
            {
                var piece = move.BoardBefore.GetBoard().GetSquare(i, j).GetPiece();
                if (piece != null && piece != move.Piece)
                {
                    pieceFound = true;
                }
                if (pieceFound)
                {
                    return true;
                }
                if (startFile < endFile && i < endFile) i++;
                if (startFile > endFile && i > endFile) i--;
                if (startRank < endRank && j < endRank) j++;
                if (startRank > endRank && j > endRank) j--;
            }

            if (move.To.GetPiece() != null && move.To.GetPiece().GetPlayer() == move.From.GetPiece().GetPlayer())
            {
                return true;
            }

            return false;
        }
    }
}
