namespace ChessCore.Modes.Standard
{
    internal static class BoardStateExtensions
    {
        /// <summary>
        /// Converts the given BoardState to StandardBoardState.
        /// </summary>
        /// <param name="state">A BoardState object to convert</param>
        /// <returns>state if it's already an instance of StandardBoardState, a newly initialized StandardBoardState otherwise</returns>
        public static StandardBoardState ToStandardBoardState(this BoardState state)
        {
            if (state is StandardBoardState boardState)
            {
                return boardState;
            }
            return new StandardBoardState(state.GetBoard(), state.GetLastMove());
        }
    }
}
