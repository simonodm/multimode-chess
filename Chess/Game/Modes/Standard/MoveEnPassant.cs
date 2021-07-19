using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveEnPassant : ClassicMove
    {
        public MoveEnPassant(IGameRules rules) : base(rules) { }
        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this)
                .RemovePiece(GetEnPassantSquare());
            return new StandardBoardState(board, this);
        }

        private BoardSquare GetEnPassantSquare()
        {
            var enPassantFile = BoardBefore.GetLastMove().From.GetFile();
            var enPassantRank = BoardBefore.GetLastMove().From.GetRank() + (BoardBefore.GetLastMove().To.GetRank() - BoardBefore.GetLastMove().From.GetRank());
            var square = BoardBefore.GetBoard().GetSquare(enPassantFile, enPassantRank);
            return square;
        }
    }
}
