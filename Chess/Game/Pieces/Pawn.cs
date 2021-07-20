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

        public override List<BoardSquare> GetPossibleMoves(BoardState state, BoardSquare from)
        {
            var moves = new List<BoardSquare>();
            foreach(var move in _possibleMoves)
            {
                var target = GetTargetSquare(state, from, move);
                if(target != null)
                {
                    if(ValidateMove(state, from, (BoardSquare)target))
                    {
                        moves.Add((BoardSquare)target);
                    }
                }
            }

            return moves;
        }

        public override (int, int)[] GetPossibleMoveOffsets()
        {
            return _possibleMoves;
        }

        private bool ValidateMove(BoardState state, BoardSquare from, BoardSquare to)
        {
            return CheckDirection(from, to) && CheckLongMove(from, to) && CheckCollision(state, from, to);
        }

        private bool CheckDirection(BoardSquare from, BoardSquare to)
        {
            if(GetPlayer() == 0)
            {
                return to.GetRank() > from.GetRank();
            }
            else
            {
                return to.GetRank() < from.GetRank();
            }
        }

        private bool CheckLongMove(BoardSquare from, BoardSquare to)
        {
            if(Math.Abs(from.GetRank() - to.GetRank()) == 2)
            {
                return (GetPlayer() == 0 && from.GetRank() == 1) || (GetPlayer() == 1 && from.GetRank() == 6);
            }
            return true;
        }

        private bool CheckCollision(BoardState state, BoardSquare from, BoardSquare to)
        {
            if(to.GetFile() == from.GetFile())
            {
                return !IsLineBlocked(state, from, to) && to.GetPiece() == null;
            }
            return true;
        }
    }
}
