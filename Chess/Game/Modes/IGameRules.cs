using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes
{
    interface IGameRules
    {
        public int PlayerCount { get; }
        public int FileCount { get; }
        public int RankCount { get; }
        public int CurrentPlayer { get; }
        public BoardState Move(Move move);
        public bool IsGameOver(BoardState state);
        public List<Move> GetAllLegalMoves(BoardState state, int player);
        public List<Move> GetLegalMoves(BoardSquare square, BoardState state);
        public BoardState GetDefaultBoard();
        public GameResult GetGameResult(BoardState state);
        public BoardScore GetBoardScore(BoardState state);
        public IBoardEvaluator GetEvaluator();
        public string GetMoveNotation(Move move);
    }
}
