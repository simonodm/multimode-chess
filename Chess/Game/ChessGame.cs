using Chess.Game.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class ChessGame
    {
        public BoardState BoardState;
        public IGameRules Rules;
        public List<Move> MoveHistory;

        public ChessGame(IGameRules rules)
        {
            Rules = rules;
            MoveHistory = new List<Move>();
            Reset();
        }

        public void Reset()
        {
            BoardState = Rules.GetDefaultBoard();
            MoveHistory.Clear();
        }

        public void ProcessMove(Move move)
        {
            BoardState = Rules.Move(move);
            MoveHistory.Add(move);
        }

        public bool IsGameOver()
        {
            return Rules.IsGameOver(BoardState);
        }
    }
}
