using ChessCore.Modes.Standard;

namespace ChessCore.Modes.PawnOfTheDead
{
    /// <summary>
    /// Represents the Pawn of the Dead game mode.
    /// </summary>
    public class PawnOfTheDeadRules : StandardRules
    {
        /// <inheritdoc cref="StandardRules.Move"/>
        public override BoardState Move(Move move)
        {
            if (move is MoveCapture)
            {
                var moveRecolor = new MoveConvert
                {
                    Piece = move.Piece,
                    From = move.From,
                    To = move.To,
                    BoardBefore = move.BoardBefore
                };
                return moveRecolor.Process();
            }
            return base.Move(move);
        }
    }
}
