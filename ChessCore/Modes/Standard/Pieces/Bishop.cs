using System.Collections.Generic;

namespace ChessCore.Modes.Standard.Pieces
{
    public class Bishop : StandardPiece
    {
        public Bishop(int player) : base(player)
        {
            _value = 3;
            _symbol = "B";
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
