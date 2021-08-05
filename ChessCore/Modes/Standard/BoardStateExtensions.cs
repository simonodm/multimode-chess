namespace ChessCore.Modes.Standard
{
    static class BoardStateExtensions
    {
        public static StandardBoardState ToStandardBoardState(this BoardState state)
        {
            if (state is StandardBoardState)
            {
                return (StandardBoardState)state;
            }
            return new StandardBoardState(state.GetBoard(), state.GetLastMove());
        }
    }
}
