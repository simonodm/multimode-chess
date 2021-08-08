using System.Collections.Generic;

namespace ChessCore.Modes.Standard.Pieces
{
    /// <summary>
    /// Represents a standard bishop.
    /// </summary>
    public class Bishop : StandardPiece
    {
        public Bishop(int player) : base(player)
        {
            Value = 3;
            Symbol = "B";
            PossibleMoves = new HashSet<(int, int)>();
            for (int i = -7; i < 8; i++)
            {
                if (i != 0)
                {
                    PossibleMoves.Add((i, i));
                    PossibleMoves.Add((i, -i));
                }
            }
        }
    }
}
