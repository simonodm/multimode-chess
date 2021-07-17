using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class King : IGamePiece
    {
        public int Value { get; } = 10;
        public (int, int)[] PossibleMoveOffsets { get; private set; } =
        {
            (-2, 0),
            (-1, -1),
            (-1, 0),
            (-1, 1),
            (0, -1),
            (0, 1),
            (1, -1),
            (1, 0),
            (1, 1),
            (2, 0)
        };
        public BoardSquare? Square { get; set; }
        public int Player { get; set; }
        public int MoveCount { get; set; } = 0;
    }
}
