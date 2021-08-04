namespace ChessCore.Game.Modes.Standard
{
    class MoveCastle : StandardMove
    {
        public override StandardBoardState Process()
        {
            var rookSquare = GetRookSquare(BoardBefore, From, To);

            var rookTargetSquare = To.GetFile() > From.GetFile() ?
                BoardBefore.GetBoard().GetSquare(From.GetFile() + 1, From.GetRank()) :
                BoardBefore.GetBoard().GetSquare(From.GetFile() - 1, From.GetRank());

            var rookMove = new MoveNormal()
            {
                From = rookSquare,
                To = rookTargetSquare,
                Piece = rookSquare.GetPiece()
            };

            var board = BoardBefore.GetBoard()
                .Move(this)
                .Move(rookMove);

            return new StandardBoardState(board, this);
        }

        private static BoardSquare GetRookSquare(BoardState state, BoardSquare from, BoardSquare to)
        {
            return to.GetFile() < from.GetFile() ?
                state.GetBoard().GetSquare(0, from.GetRank()) :
                state.GetBoard().GetSquare(7, from.GetRank());
        }
    }
}
