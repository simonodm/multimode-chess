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
        private List<Move> _moveHistory;

        public ChessGame(IGameRules rules)
        {
            Rules = rules;
            _moveHistory = new List<Move>();
            Reset();
        }

        public void Reset()
        {
            BoardState = Rules.GetDefaultBoard();
            _moveHistory.Clear();
        }

        public void ProcessMove(Move move)
        {
            BoardState = Rules.Move(move);
            _moveHistory.Add(move);
        }

        public bool IsGameOver()
        {
            return Rules.IsGameOver(BoardState);
        }
    }
}
