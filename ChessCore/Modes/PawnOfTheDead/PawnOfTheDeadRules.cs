using ChessCore.Game.Modes.Standard;

namespace ChessCore.Game.Modes.PawnOfTheDead
{
    public class PawnOfTheDeadRules : StandardRules
    {
        public override BoardState Move(Move move)
        {
            if(move is MoveCapture)
            {
                var moveRecolor = new MoveRecolor
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
