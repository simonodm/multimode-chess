using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Queen : GamePiece
    {
        private (int, int)[] _possibleMoves { get; }

        public Queen(int player) : base(player)
        {
            _value = 9;
            _symbol = "Q";
            _possibleMoves = new (int, int)[56];
            int current = 0;
            for(int i = -7; i < 8; i++)
            {
                if(i != 0)
                {
                    _possibleMoves[current++] = (0, i);
                    _possibleMoves[current++] = (i, 0);
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
