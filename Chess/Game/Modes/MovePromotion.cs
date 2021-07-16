using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    class MovePromotion : Move
    {
        public BoardState Handle()
        {
            Move move;
            if(To.Piece != null)
            {
                move = new MoveCapture
                {
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            else
            {
                move = new MoveNormal
                {
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            //move.Handle();
            return BoardBefore.RemoveAt(To).AddPiece(To, GetPieceFromSelectedOption());
        }

        public bool IsLegal()
        {
            return SelectedOption != null;
        }

        private IGamePiece GetPieceFromSelectedOption()
        {
            IGamePiece piece;
            switch (SelectedOption.Id)
            {
                case 0:
                    piece = new Queen
                    {
                        Square = To,
                        Player = Piece.Player
                    };
                    break;
                case 1:
                    piece = new Rook
                    {
                        Square = To,
                        Player = Piece.Player
                    };
                    break;
                case 2:
                    piece = new Knight
                    {
                        Square = To,
                        Player = Piece.Player
                    };
                    break;
                case 3:
                    piece = new Bishop
                    {
                        Square = To,
                        Player = Piece.Player
                    };
                    break;
                default:
                    throw new Exception("Unrecognized option");
            }
            return piece;
        }
    }
}
