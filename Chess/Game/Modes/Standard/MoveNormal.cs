using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveNormal : ClassicMove
    {
        public MoveNormal(IGameRules rules) : base(rules) { }

        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this);
            return new StandardBoardState(board, this);
        }

        public static bool IsLegal(BoardSquare from, BoardSquare to)
        {
            if(from.GetPiece() is Pawn && to.GetFile() != from.GetFile())
            {
                return false;
            }
            if(from.GetPiece() is King && Math.Abs(to.GetFile() - from.GetFile()) == 2)
            {
                return false;
            }
            return to.GetPiece() == null;
            if (to.GetPiece() == null)
            {
                var movePiece = from.GetPiece();
                if (movePiece is King)
                {
                    return Math.Abs(from.GetRank() - to.GetRank()) == 1;
                }
                if (movePiece is Pawn)
                {
                    if (to.GetFile() != from.GetFile())
                    {
                        return false;
                    }
                    if (movePiece.GetPlayer() == 0 && to.GetRank() - from.GetRank() < 0)
                    {
                        return false;
                    }
                    if (movePiece.GetPlayer() == 1 && from.GetRank() - to.GetRank() < 0)
                    {
                        return false;
                    }
                    if (to.GetRank() - from.GetRank() == 2)
                    {
                        return from.GetRank() == 1;
                    }
                    if (from.GetRank() - to.GetRank() == 2)
                    {
                        return from.GetRank() == 6;
                    }
                    return true;
                }
                return true;
            }
            return false;
        }
    }
}
