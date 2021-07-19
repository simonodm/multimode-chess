using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveCastle : ClassicMove
    {
        public MoveCastle(IGameRules rules) : base(rules) { }

        public override StandardBoardState Process()
        {
            var rookSquare = GetRookSquare();

            var rookTargetSquare = To.GetFile() > From.GetFile() ?
                BoardBefore.GetBoard().GetSquare(From.GetFile() + 1, From.GetRank()) :
                BoardBefore.GetBoard().GetSquare(From.GetFile() - 1, From.GetRank());

            var rookMove = new MoveNormal(_rules)
            {
                From = rookSquare,
                To = rookTargetSquare,
                Piece = rookSquare.GetPiece()
            };

            var board = BoardBefore.GetBoard()
                .Move(this)
                .Move(rookMove);
            return new StandardBoardState(board, this);
        }

        private BoardSquare GetRookSquare()
        {
            if (To.GetFile() < From.GetFile())
            {
                return BoardBefore.GetBoard().GetSquare(0, From.GetRank());
            }
            else
            {
                return BoardBefore.GetBoard().GetSquare(7, From.GetRank());
            }
        }
    }
}
