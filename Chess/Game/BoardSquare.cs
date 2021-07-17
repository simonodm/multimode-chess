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
        private int _file;
        private int _rank;
        private GamePiece _piece;

        public BoardSquare(int file, int rank, GamePiece piece)
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
    }
}
