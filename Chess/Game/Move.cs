using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Pieces;

namespace Chess.Game
{
    struct Move
    {
        public IGamePiece Piece;
        public BoardSquare From;
        public BoardSquare To;
        public BoardState BoardState;

        public static bool operator ==(Move moveA, Move moveB)
        {
            return moveA.Equals(moveB);
        }

        public static bool operator !=(Move moveA, Move moveB)
        {
            return !moveA.Equals(moveB);
        }
    }
}
