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
        public Clock Clock;

        public ChessGame(IGameRules rules, int timeLimit = 600, int increment = 0)
        {
            Rules = rules;
            MoveHistory = new List<Move>();
            Clock = new Clock(timeLimit, increment);
            Reset();
        }

        public void Reset()
        {
            BoardState = Rules.GetDefaultBoard();
            Clock.Reset();
            MoveHistory.Clear();
            Clock.Start();
        }

        public void ProcessMove(Move move)
        {
            if(Clock.GetRemainingTime(move.Piece.Player) > 0)
            {
                BoardState = Rules.Move(move);
                MoveHistory.Add(move);
            }
        }

        public bool IsGameOver()
        {
            return IsClockRanOut() || Rules.IsGameOver(BoardState);
        }

        private bool IsClockRanOut()
        {
            int playersWithTime = 0;
            for(int i = 0; i < Rules.PlayerCount; i++)
            {
                if(Clock.GetRemainingTime(i) > 0)
                {
                    playersWithTime += 1;
                }
            }
            return playersWithTime == 1;
        }
    }
}
