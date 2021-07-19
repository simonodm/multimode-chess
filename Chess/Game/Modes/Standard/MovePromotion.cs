using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MovePromotion : ClassicMove
    {
        public MovePromotion(IGameRules rules) : base(rules)
        {
            AddOption("Queen");
            AddOption("Rook");
            AddOption("Bishop");
            AddOption("Knight");
        }

        public override StandardBoardState Process()
        {
            ClassicMove move;
            if (To.GetPiece() != null)
            {
                move = new MoveCapture(_rules)
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            else
            {
                move = new MoveNormal(_rules)
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            var newState = move.Process();
            var board = newState.GetBoard().RemovePiece(To).AddPiece(To, GetPieceFromSelectedOption());
            return new StandardBoardState(board, this);
        }

        private GamePiece GetPieceFromSelectedOption()
        {
            GamePiece piece;
            switch (SelectedOption.Id)
            {
                case 0:
                    piece = new Queen(Piece.GetPlayer());
                    break;
                case 1:
                    piece = new Rook(Piece.GetPlayer());
                    break;
                case 2:
                    piece = new Knight(Piece.GetPlayer());
                    break;
                case 3:
                    piece = new Bishop(Piece.GetPlayer());
                    break;
                default:
                    throw new Exception("Unrecognized option");
            }
            return piece;
        }
    }
}
