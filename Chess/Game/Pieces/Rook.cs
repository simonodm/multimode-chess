using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Rook : GamePiece
    {
        private (int, int)[] _possibleMoveOffsets;
        public Rook(int player) : base(player)
        {
            _value = 5;
            _symbol = "R";
            _possibleMoveOffsets = new (int, int)[28];
            int current = 0;
            for (int i = -7; i < 8; i++)
            {
                if (i != 0)
                {
                    _possibleMoveOffsets[current++] = (0, i);
                    _possibleMoveOffsets[current++] = (i, 0);
                }
            }
        }

        public override List<BoardSquare> GetPossibleMoves(BoardState state, BoardSquare from)
        {
            var moves = new List<BoardSquare>();
            foreach(var move in _possibleMoveOffsets)
            {
                var target = GetTargetSquare(state, from, move);
                if(target != null && !IsLineBlocked(state, from, (BoardSquare)target))
                {
                    moves.Add((BoardSquare)target);
                }
            }
            return moves;
        }

        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoveOffsets;
        }
    }
}
