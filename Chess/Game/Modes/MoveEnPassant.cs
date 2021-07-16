using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    class MoveEnPassant : Move
    {
        public BoardState Handle()
        {
            return BoardBefore.Move(this).RemoveAt(GetEnPassantSquare());
        }

        public bool IsLegal()
        {
            if(Previous.Piece is Pawn && Math.Abs(Previous.To.Rank - Previous.From.Rank) == 2 && GetEnPassantSquare() == To)
            {
                return true;
            }
            return false;
        }

        private BoardSquare GetEnPassantSquare()
        {
            var enPassantFile = Previous.From.File;
            var enPassantRank = Previous.From.Rank + (Previous.To.Rank - Previous.From.Rank);
            var square = BoardBefore.GetSquare(enPassantFile, enPassantRank);
            return square;
        }
    }
}
