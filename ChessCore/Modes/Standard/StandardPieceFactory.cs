using ChessCore.Exceptions;
using ChessCore.Modes.Standard.Pieces;
using System.Collections.Generic;

namespace ChessCore.Modes.Standard
{
    class StandardPieceFactory : IPieceFactory
    {
        private string[] _pieces =
        {
            "Queen",
            "King",
            "Pawn",
            "Rook",
            "Knight",
            "Bishop"
        };

        public GamePiece GetPiece(int id, int player = 0)
        {
            switch (id)
            {
                case 0:
                    return new Queen(player);
                case 1:
                    return new King(player);
                case 2:
                    return new Pawn(player);
                case 3:
                    return new Rook(player);
                case 4:
                    return new Knight(player);
                case 5:
                    return new Bishop(player);
                default:
                    throw new ChessCoreException("Invalid standard piece id supplied.");
            }
        }

        public IEnumerable<Option> GetPieceOptions()
        {
            for (int i = 0; i < 6; i++)
            {
                yield return new Option(i, _pieces[i]);
            }
        }

        public IEnumerable<Option> GetPieceOptions(IEnumerable<int> ids)
        {
            foreach (int id in ids)
            {
                yield return new Option(id, _pieces[id]);
            }
        }
    }
}
