using System.Collections.Generic;

namespace ChessCore.Modes
{
    public interface IGameRules
    {
        public BoardState Move(Move move);
        public IReadOnlyList<Move> GetAllLegalMoves(BoardState state, int player);
        public IReadOnlyList<Move> GetLegalMoves(BoardSquare square, BoardState state, int player);
        public BoardState GetStartingBoardState();
        public BoardState GetStartingBoardState(Board board);
        public GameResult GetGameResult(BoardState state);
        public IBoardEvaluator GetEvaluator();
        public IPieceFactory GetPieceFactory();
        public string GetMoveNotation(Move move);
    }
}
