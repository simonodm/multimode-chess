using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    interface IGamePiece
    {
        public int Value { get; }
        public (int, int)[] PossibleMoveOffsets { get; }
        public BoardSquare? Square { get; set; }
        public int Player { get; set; }
        public int MoveCount { get; set; }
        public string Symbol { get; }
    }
}
