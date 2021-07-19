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
        private BoardState _boardState;
        private IGameRules _rules;
        private List<Move> _moveHistory;
        private Clock _clock;
        private int _currentPlayer;
        private bool _vsAi;
        private int _AIplayer = 1;

        internal ChessGame(IGameRules rules, int timeLimit = 600, int increment = 0, bool ai = false)
        {
            _rules = rules;
            _moveHistory = new List<Move>();
            _clock = new Clock(_rules.PlayerCount, timeLimit, increment);
            _vsAi = ai;
            Reset();
        }

        public void Reset()
        {
            _boardState = _rules.GetDefaultBoardState();
            _currentPlayer = 0;
            _clock.Reset();
            _moveHistory.Clear();
            _clock.Start();
        }

        public void ProcessMove(Move move)
        {
            if(_clock.GetRemainingTime(move.Piece.GetPlayer()) > 0)
            {
                if (_moveHistory.Count > 0) move.Previous = _moveHistory[_moveHistory.Count - 1];
                _boardState = _rules.Move(move);
                _moveHistory.Add(move);
                _clock.Switch();
                _currentPlayer = (_currentPlayer + 1) % _rules.PlayerCount;
                if(_vsAi && _currentPlayer == _AIplayer)
                {
                    _boardState.SetScore(Minimax.GetBoardScore(_rules, _boardState));
                    ProcessMove(_boardState.GetScore().BestMove);
                }
            }
        }

        public bool IsGameOver()
        {
            return DidClockRunOut() || _rules.IsGameOver(_boardState);
        }

        public GameResult GetGameResult()
        {
            if(!IsGameOver())
            {
                throw new Exception("The game is not over yet.");
            }
            var playersWithRemainingTime = GetAllPlayersWithRemainingTime();
            if(playersWithRemainingTime.Count == 1)
            {
                return new GameResult(playersWithRemainingTime[0]);
            }
            return _rules.GetGameResult(_boardState);
        }

        public double Evaluate(BoardState state)
        {
            if(state.GetScore() != null)
            {
                return state.GetScore().Score;
            }
            var score = Minimax.GetBoardScore(_rules, state);
            state.SetScore(score);
            return score.Score;
        }

        public IEnumerable<Move> GetLegalMoves(BoardSquare square, BoardState state)
        {
            return _rules.GetLegalMoves(square, state);
        }

        public Clock GetClock()
        {
            return _clock;
        }

        public int GetCurrentPlayer()
        {
            return _currentPlayer;
        }

        public List<Move> GetMoveHistory()
        {
            return _moveHistory;
        }

        public BoardState GetBoardState()
        {
            return _boardState;
        }

        private List<int> GetAllPlayersWithRemainingTime()
        {
            List<int> players = new List<int>();
            for (int i = 0; i < _rules.PlayerCount; i++)
            {
                if (_clock.GetRemainingTime(i) > 0)
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
