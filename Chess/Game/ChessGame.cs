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
        public int CurrentPlayer;
        private bool _vsAi;
        private int _AIplayer = 1;

        public static ChessGame CreateGame<TRules>(int timeLimit = 600, int increment = 0, bool vsAi = false)
            where TRules : IGameRules, new()
        {
            var rules = new TRules();
            return new ChessGame(rules, timeLimit, increment, vsAi);
        }

        public static ChessGame CreateFromModeId(int modeId, int timeLimit = 600, int increment = 0, bool vsAi = false)
        {
            switch(modeId)
            {
                case 1:
                    return CreateGame<PawnOfTheDeadRules>(timeLimit, increment, vsAi);
                case 0:
                default:
                    return CreateGame<ClassicRules>(timeLimit, increment, vsAi);
            }
        }

        protected ChessGame(IGameRules rules, int timeLimit = 600, int increment = 0, bool ai = false)
        {
            Rules = rules;
            MoveHistory = new List<Move>();
            Clock = new Clock(Rules.PlayerCount, timeLimit, increment);
            _vsAi = ai;
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
                if(_vsAi && CurrentPlayer == _AIplayer)
                {
                    BoardState.Score = Minimax.GetBoardScore(Rules, BoardState);
                    ProcessMove(BoardState.Score.BestMove);
                }
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
            return Rules.GetGameResult(BoardState);
        }

        public double Evaluate(BoardState state)
        {
            if(state.Score != null)
            {
                return state.Score.Score;
            }
            var score = Minimax.GetBoardScore(Rules, state);
            state.Score = score;
            return score.Score;
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
