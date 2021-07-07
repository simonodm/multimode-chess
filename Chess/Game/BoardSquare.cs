using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Pieces;

namespace Chess.Game
{
    struct BoardSquare
    {
        public int File;
        public int Rank;
        public IGamePiece Piece;

        public BoardSquare(int posX, int posY, IGamePiece piece)
        {
            File = posX;
            Rank = posY;
            Piece = piece;
        }

        public static bool operator ==(BoardSquare squareA, BoardSquare squareB)
        {
            return squareA.Equals(squareB);
        }

        public static bool operator !=(BoardSquare squareA, BoardSquare squareB)
        {
            return !squareA.Equals(squareB);
        }
    }
}
