using System.Collections.Generic;

namespace ChessCore.Modes.Standard.Pieces
{
    public class Queen : StandardPiece
    {
        public Queen(int player) : base(player)
        {
            _value = 9;
            _symbol = "Q";
            PossibleMoves = new HashSet<(int, int)>();
            for (int i = -7; i < 8; i++)
            {
                if (i != 0)
                {
                    PossibleMoves.Add((0, i));
                    PossibleMoves.Add((i, 0));
                    PossibleMoves.Add((i, i));
                    PossibleMoves.Add((i, -i));
                }
            }
        }
    }
}
