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
            Clock = new Clock(Rules.PlayerCount, timeLimit, increment);
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
                Clock.Switch();
            }
        }

        public bool IsGameOver()
        {
            return DidClockRunOut() || Rules.IsGameOver(BoardState);
        }

        public GameResult GetGameResult()
        {
            if(!IsGameOver())
            {
                throw new Exception("No game result available yet.");
            }
            var playersWithRemainingTime = GetAllPlayersWithRemainingTime();
            if(playersWithRemainingTime.Count == 1)
            {
                return new GameResult(playersWithRemainingTime[0]);
            }
            return Rules.GetGameResult();
        }

        private List<int> GetAllPlayersWithRemainingTime()
        {
            List<int> players = new List<int>();
            for (int i = 0; i < Rules.PlayerCount; i++)
            {
                if (Clock.GetRemainingTime(i) > 0)
                {
                    players.Add(i);
                }
            }
            return players;
        }

        private bool DidClockRunOut()
        {
            return GetAllPlayersWithRemainingTime().Count == 1;
        }
    }
}
