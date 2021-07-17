using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Bishop : GamePiece
    {
        private (int, int)[] _possibleMoves;

        public Bishop(int player) : base(player)
        {
            _value = 3;
            _symbol = "B";
            _possibleMoves = new (int, int)[28];
            int current = 0;
            for (int i = -7; i < 8; i++)
            {
                if (i != 0)
                {
                    _possibleMoves[current++] = (i, i);
                    _possibleMoves[current++] = (i, -i);
                }
            }
        }
        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoves;
        }
    }
}
