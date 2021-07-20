using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Knight : GamePiece
    {
        private (int, int)[] _possibleMoves =
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

        public Knight(int player) : base(player)
        {
            _value = 3;
            _symbol = "N";
        }

        public override List<BoardSquare> GetPossibleMoves(BoardState state, BoardSquare from)
        {
            var moves = new List<BoardSquare>();
            foreach(var move in _possibleMoves)
            {
                var target = GetTargetSquare(state, from, move);
                if(target != null)
                {
                    moves.Add((BoardSquare)target);
                }
            }
            return moves;
        }
        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoves;
        }
    }
}
