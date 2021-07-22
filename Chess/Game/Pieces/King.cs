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

        public override List<BoardSquare> GetPossibleMoveSquares(BoardState state, BoardSquare from)
        {
            var moves = new List<BoardSquare>();
            foreach(var move in _possibleMoves)
            {
                var target = GetTargetSquare(state, from, move);
                if(target != null)
                {
                    if(CheckLongMove(state, from, (BoardSquare)target))
                    {
                        moves.Add((BoardSquare)target);
                    }
                }
            }
            return moves;
        }

        private bool CheckLongMove(BoardState state, BoardSquare from, BoardSquare to)
        {
            if(Math.Abs(to.GetFile() - from.GetFile()) == 2)
            {
                return (from.GetRank() == 0 || from.GetRank() == state.GetBoard().GetHeight() - 1) &&
                    from.GetFile() == 4 &&
                    !IsLineBlocked(state, from, to) &&
                    to.GetPiece() == null;
            }
            return true;
        }

        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoves;
        }
    }
}
