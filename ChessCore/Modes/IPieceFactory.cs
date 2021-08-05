using System.Collections.Generic;

namespace ChessCore.Modes
{
    public interface IPieceFactory
    {
        public GamePiece GetPiece(int id, int player);
        public IEnumerable<Option> GetPieceOptions();
        public IEnumerable<Option> GetPieceOptions(IEnumerable<int> ids);
    }
}
