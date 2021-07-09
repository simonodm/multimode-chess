using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Pieces;

namespace Chess.Game
{
    class PawnOfTheDeadRules : ClassicRules
    {
        protected override BoardState HandleCapture(Move move)
        {
            if(move.From.Piece.Player == 0)
            {
                return base.HandleCapture(move);
            }
            
            var piece = move.To.Piece;
            var newPiece = (IGamePiece)Activator.CreateInstance(piece.GetType());
            newPiece.Player = 1;
            return move.BoardState
                .RemoveAt(move.To)
                .AddPiece(move.To, newPiece);
        }
    }
}
