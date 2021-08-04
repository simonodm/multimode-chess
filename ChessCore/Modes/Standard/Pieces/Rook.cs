using System.Collections.Generic;

namespace ChessCore.Game.Modes.Standard.Pieces
{
    public class Rook : StandardPiece
    {
        public Rook(int player) : base(player)
        {
            _value = 5;
            _symbol = "R";
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
