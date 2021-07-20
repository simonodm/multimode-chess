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
            AddOption("Knight");
            AddOption("Bishop");
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

        public static bool IsLegal(StandardBoardState state, BoardSquare from, BoardSquare to)
        {
            return from.GetPiece() is Pawn &&
                (to.GetPiece() != null && to.GetFile() != from.GetFile() || (to.GetPiece() == null && to.GetFile() == from.GetFile())) &&
                ((from.GetRank() == 6 && to.GetRank() == state.GetBoard().GetHeight() - 1) ||
                (from.GetRank() == 1 && to.GetRank() == 0));
        }

        private GamePiece GetPieceFromSelectedOption()
        {
            if(SelectedOption == null)
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
