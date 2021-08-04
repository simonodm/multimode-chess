namespace ChessCore.Game.Modes.Standard
{
    class MoveCapture : StandardMove
    {
        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this);
            return new StandardBoardState(board, this);
        }
    }
}
