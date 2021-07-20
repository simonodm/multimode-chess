using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    class Queen : GamePiece
    {
        private (int, int)[] _possibleMoveOffsets { get; }

        public Queen(int player) : base(player)
        {
            _value = 9;
            _symbol = "Q";
            _possibleMoveOffsets = new (int, int)[56];
            int current = 0;
            for(int i = -7; i < 8; i++)
            {
                if(i != 0)
                {
                    _possibleMoveOffsets[current++] = (0, i);
                    _possibleMoveOffsets[current++] = (i, 0);
                    _possibleMoveOffsets[current++] = (i, i);
                    _possibleMoveOffsets[current++] = (i, -i);
                }
            }
        }
        public override List<BoardSquare> GetPossibleMoves(BoardState state, BoardSquare from)
        {
            var moves = new List<BoardSquare>();
            foreach (var move in _possibleMoveOffsets)
            {
                var target = GetTargetSquare(state, from, move);
                if (target != null && !IsLineBlocked(state, from, (BoardSquare)target))
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
