using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Pawn : IGamePiece
    {
        public int Value { get; } = 1;
        public (int, int)[] PossibleMoveOffsets { get; } =
        {
            (0, -2),
            (0, -1),
            (0, 1),
            (0, 2),
            (1, 1),
            (-1, 1),
            (1, -1),
            (-1, -1)
        };
        public BoardSquare? Square { get; set; }
        public int Player { get; set; }
        public int MoveCount { get; set; } = 0;
    }
}
