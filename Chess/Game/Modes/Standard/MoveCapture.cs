using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveCapture : ClassicMove
    {
        public MoveCapture(IGameRules rules) : base(rules) { }

        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this);
            return new StandardBoardState(board, this);
        }

        public static bool IsLegal(BoardSquare from, BoardSquare to)
        {
            var movePiece = from.GetPiece();
            if (movePiece is Pawn)
            {
                if (from.GetFile() == to.GetFile() ||
                   (movePiece.GetPlayer() == 0 && to.GetRank() < from.GetRank()) ||
                   (movePiece.GetPlayer() == 1 && to.GetRank() > from.GetRank()))
                {
                    return false;
                }
            }

            if (to.GetPiece() != null && to.GetPiece().GetPlayer() != from.GetPiece().GetPlayer())
            {
                return true;
            }

            return false;
        }
    }
}
