using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Pawn : GamePiece
    {
        private (int, int)[] _possibleMoves =
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

        public Pawn(int player) : base(player)
        {
            _value = 1;
            _symbol = "";
        }

        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoves;
        }
    }
}
