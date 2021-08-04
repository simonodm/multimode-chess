using System;
using System.Collections.Generic;

namespace ChessCore.Game.Modes.Standard.Pieces
{
    public class King : StandardPiece
    {
        public King(int player) : base(player)
        {
            _value = 10;
            _symbol = "K";
            PossibleMoves = new HashSet<(int, int)>
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
        }

        protected override StandardMove GenerateMove(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if(IsCastle(state, from, to))
            {
                return new MoveCastle()
                {
                    Piece = this,
                    From = from,
                    To = to,
                    BoardBefore = state
                };
            }
            else if(Math.Abs(from.GetFile() - to.GetFile()) <= 1)
            {
                return base.GenerateMove(state, from, to);
            }

            return null;
        }

        private bool IsCastle(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if(Math.Abs(to.GetFile() - from.GetFile()) == 2)
            {
                if (IsCastlePositionValid(state, from))
                {
                    var rookSquare = GetRookSquare(state, from, to);
                    return IsCastleLegal(state, from, rookSquare);
                }
            }
            
            return false;
        }

        private bool IsCastlePositionValid(StandardBoardState state, BoardSquare from)
        {
            if((GetPlayer() == 0 && from.GetRank() == 0) || (GetPlayer() == 1 && from.GetRank() == state.GetBoard().GetHeight() - 1)) {
                return from.GetFile() == 4;
            }
            return false;
        }

        private bool IsCastleLegal(StandardBoardState state, BoardSquare from, BoardSquare rookSquare)
        {
            var rook = rookSquare.GetPiece();
            return GetMoveCount() == 0 &&
                   rook != null &&
                   rook.GetMoveCount() == 0 &&
                   !IsCastleLineBlocked(state, from, rookSquare);
        }

        private bool IsCastleLineBlocked(StandardBoardState state, BoardSquare kingSquare, BoardSquare rookSquare)
        {
            for (int file = Math.Min(rookSquare.GetFile(), kingSquare.GetFile()); file < Math.Max(rookSquare.GetFile(), kingSquare.GetFile()); file++)
            {
                var square = state.GetBoard().GetSquare(file, kingSquare.GetRank());
                if(square != kingSquare && square != rookSquare)
                {
                    if (square.GetPiece() != null || state.GetThreatMap().GetThreatCount(square, (GetPlayer() + 1) % 2) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private BoardSquare GetRookSquare(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            if(to.GetFile() == 6)
            {
                return state.GetBoard().GetSquare(7, from.GetRank());
            }
            else if(to.GetFile() == 2)
            {
                return state.GetBoard().GetSquare(0, from.GetRank());
            }
            throw new Exception("Invalid target square supplied.");
        }
    }
}
