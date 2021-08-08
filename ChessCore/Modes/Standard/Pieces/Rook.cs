using System.Collections.Generic;

namespace ChessCore.Modes.Standard.Pieces
{
    /// <summary>
    /// Represents a standard rook.
    /// </summary>
    public class Rook : StandardPiece
    {
        public Rook(int player) : base(player)
        {
            Value = 5;
            Symbol = "R";
            PossibleMoves = new HashSet<(int, int)>();
            for (int i = -7; i < 8; i++)
            {
                if (i != 0)
                {
                    PossibleMoves.Add((0, i));
                    PossibleMoves.Add((i, 0));
                }
            }
        }
    }
}
