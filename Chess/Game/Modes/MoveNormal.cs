using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    class MoveNormal : Move
    {
        public BoardState Handle()
        {
            BoardState newBoardState = BoardBefore.Move(this);

            return newBoardState;
        }

        public bool IsLegal()
        {
            if (To.Piece == null)
            {
                if (Piece is King)
                {
                    return Math.Abs(From.Rank - To.Rank) <= 1 && Math.Abs(From.File - To.File) <= 1;
                }
                if (Piece is Pawn)
                {
                    if (To.File != From.File)
                    {
                        return false;
                    }
                    if (Piece.Player == 0 && To.Rank - From.Rank < 0)
                    {
                        return false;
                    }
                    if (Piece.Player == 1 && From.Rank - To.Rank < 0)
                    {
                        return false;
                    }
                    if (To.Rank - From.Rank == 2)
                    {
                        return From.Rank == 1;
                    }
                    if (From.Rank - To.Rank == 2)
                    {
                        return From.Rank == 6;
                    }
                    return true;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }
}
