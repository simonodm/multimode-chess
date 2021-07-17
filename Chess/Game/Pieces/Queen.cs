using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Queen : IGamePiece
    {
        public int Value { get; } = 9;
        public (int, int)[] PossibleMoveOffsets { get; }
        public BoardSquare? Square { get; set; }
        public int Player { get; set; }
        public int MoveCount { get; set; } = 0;
        public string Symbol { get; } = "Q";

        public Queen()
        {
            PossibleMoveOffsets = new (int, int)[56];
            int current = 0;
            for(int i = -7; i < 8; i++)
            {
                if(i != 0)
                {
                    PossibleMoveOffsets[current++] = (0, i);
                    PossibleMoveOffsets[current++] = (i, 0);
                    PossibleMoveOffsets[current++] = (i, i);
                    PossibleMoveOffsets[current++] = (i, -i);
                }
            }
        }
    }
}
