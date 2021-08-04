using ChessCore.Game.Modes.Standard.Pieces;
using System;

namespace ChessCore.Game.Modes.Standard
{
    class MovePromotion : StandardMove
    {
        public MovePromotion() : base()
        {
            AddOption("Queen");
            AddOption("Rook");
            AddOption("Knight");
            AddOption("Bishop");
        }

        public override StandardBoardState Process()
        {
            StandardMove move;
            if (To.GetPiece() != null)
            {
                move = new MoveCapture()
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }
            else
            {
                move = new MoveNormal()
                {
                    BoardBefore = BoardBefore,
                    From = From,
                    To = To,
                    Piece = Piece
                };
            }

            var newState = move.Process();
            var board = newState.GetBoard()
                .RemovePiece(To)
                .AddPiece(To, GetPieceFromSelectedOption());

            return new StandardBoardState(board, this);
        }

        private GamePiece GetPieceFromSelectedOption()
        {
            if (SelectedOption == null)
            {
                return new Queen(Piece.GetPlayer());
            }

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
