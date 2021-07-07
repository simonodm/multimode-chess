using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Bishop : IGamePiece
    {
        public int Value { get; } = 3;
        public (int, int)[] PossibleMoveOffsets { get; }
        public BoardSquare? Square { get; set; }
        public int Player { get; set; }
        public int MoveCount { get; set; } = 0;

        public Bishop()
        {
            PossibleMoveOffsets = new (int, int)[28];
            int current = 0;
            for (int i = -7; i < 8; i++)
            {
                if (i != 0)
                {
                    PossibleMoveOffsets[current++] = (i, i);
                    PossibleMoveOffsets[current++] = (i, -i);
                }
            }
        }
    }
}
