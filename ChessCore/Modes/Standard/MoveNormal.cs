namespace ChessCore.Modes.Standard
{
    /// <summary>
    /// A move type representing normal move without capture.
    /// </summary>
    internal class MoveNormal : StandardMove
    {
        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this);

            return new StandardBoardState(board, this);
        }
    }
}
