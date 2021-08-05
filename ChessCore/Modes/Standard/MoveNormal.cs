namespace ChessCore.Modes.Standard
{
    class MoveNormal : StandardMove
    {
        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this);

            return new StandardBoardState(board, this);
        }
    }
}
