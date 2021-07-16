using Chess.Game.Pieces;
using Chess.Game.Modes;
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
        public IBoardEvaluator Evaluator;
        public int CurrentPlayer;

        public static ChessGame CreateGame<TRules>(int timeLimit = 600, int increment = 0)
            where TRules : IGameRules, new()
        {
            var rules = new TRules();
            return new ChessGame(rules, timeLimit, increment);
        }

        public static ChessGame CreateFromModeId(int modeId, int timeLimit = 600, int increment = 0)
        {
            switch(modeId)
            {
                case 1:
                    return CreateGame<PawnOfTheDeadRules>(timeLimit, increment);
                case 0:
                default:
                    return CreateGame<ClassicRules>(timeLimit, increment);
            }
        }

        protected ChessGame(IGameRules rules, int timeLimit = 600, int increment = 0)
        {
            Rules = rules;
            MoveHistory = new List<Move>();
            Clock = new Clock(Rules.PlayerCount, timeLimit, increment);
            Reset();
        }

        public void Reset()
        {
            BoardState = Rules.GetDefaultBoard();
            CurrentPlayer = 0;
            Clock.Reset();
            MoveHistory.Clear();
            Clock.Start();
        }

        public void ProcessMove(Move move)
        {
            if(Clock.GetRemainingTime(move.Piece.Player) > 0)
            {
                if (MoveHistory.Count > 0) move.Previous = MoveHistory[MoveHistory.Count - 1];
                BoardState = Rules.Move(move);
                MoveHistory.Add(move);
                Clock.Switch();
                CurrentPlayer = (CurrentPlayer + 1) % Rules.PlayerCount;
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

        public double Evaluate(BoardState state)
        {
            return Minimax.GetBoardScore(Rules, state);
        }

        public IBoardEvaluator GetEvaluator()
        {
            return Rules.GetEvaluator();
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
