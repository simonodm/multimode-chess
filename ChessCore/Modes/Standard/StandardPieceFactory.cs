using ChessCore.Exceptions;
using ChessCore.Modes.Standard.Pieces;
using System.Collections.Generic;
using System.Linq;

namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// Represents the piece factory for the standard mode. Can be used to retrieve the options for the standard game mode.
    /// </summary>
    internal class StandardPieceFactory : IPieceFactory
    {
        private readonly string[] _pieces =
        {
            "Queen",
            "King",
            "Pawn",
            "Rook",
            "Knight",
            "Bishop"
        };

        /// <inheritdoc cref="IPieceFactory.GetPiece"/>
        /// <exception cref="ChessCoreException">Thrown if an invalid id is supplied</exception>
        public GamePiece GetPiece(int id, int player = 0)
        {
            return id switch
            {
                0 => new Queen(player),
                1 => new King(player),
                2 => new Pawn(player),
                3 => new Rook(player),
                4 => new Knight(player),
                5 => new Bishop(player),
                _ => throw new ChessCoreException("Invalid standard piece id supplied.")
            };
        }

        public IEnumerable<Option> GetPieceOptions()
        {
            for (int i = 0; i < 6; i++)
            {
                yield return new Option(i, _pieces[i]);
            }
        }

        /// <inheritdoc cref="IPieceFactory.GetPieceOptions(IEnumerable&lt;int&gt;)"/>
        /// <exception cref="ChessCoreException">Thrown if an invalid id is supplied</exception>
        public IEnumerable<Option> GetPieceOptions(IEnumerable<int> ids)
        {
            var enumeratedIds = ids as int[] ?? ids.ToArray();

            foreach (int id in enumeratedIds)
            {
                if (id < 0 || id > 5)
                {
                    throw new ChessCoreException($"Invalid id supplied: {id}");
                }
            }
            return enumeratedIds.Select(id => new Option(id, _pieces[id]));
        }
    }
}
