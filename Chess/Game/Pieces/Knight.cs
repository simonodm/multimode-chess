using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Knight : IGamePiece
    {
        public int Value { get; } = 3;
        public (int, int)[] PossibleMoveOffsets { get; } =
        {
            (-2, -1),
            (-2, +1),
            (+2, -1),
            (+2, +1),
            (-1, -2),
            (-1, +2),
            (+1, -2),
            (+1, +2)
        };
        public BoardSquare? Square { get; set; }
        public int Player { get; set; }
        public int MoveCount { get; set; } = 0;
        public string Symbol { get; } = "N";
    }
}
