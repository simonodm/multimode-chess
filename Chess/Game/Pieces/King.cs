using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class King : GamePiece
    {
        private (int, int)[] _possibleMoves =
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

        public King(int player) : base(player)
        {
            _value = 10;
            _symbol = "K";
        }

        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoves;
        }
    }
}
