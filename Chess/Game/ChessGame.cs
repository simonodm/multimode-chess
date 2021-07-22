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
        private object _gameStateLock = new object();

        private BoardState _boardState;
        private IGameRules _rules;
        private List<Move> _moveHistory;
        private Clock _clock;
        private int _currentPlayer;
        private BoardState _defaultBoardState;

        internal ChessGame(IGameRules rules, int timeLimit = 600, int increment = 0)
        {
            _rules = rules;
            _moveHistory = new List<Move>();
            _clock = new Clock(_rules.PlayerCount, timeLimit, increment);
            _defaultBoardState = _rules.GetStartingBoardState();
            Reset();
        }

        internal ChessGame(IGameRules rules, Board board, int timeLimit = 600, int increment = 0)
            : this(rules, timeLimit, increment)
        {
            _defaultBoardState = _rules.GetStartingBoardState(board);
            Reset();
        }

        public void Reset()
        {
            _boardState = _defaultBoardState;
            lock (_gameStateLock)
            {
                _currentPlayer = 0;
                _clock.Reset();
                _moveHistory.Clear();
                _clock.Start();
            }
        }

        public void ProcessMove(Move move)
        {
            lock (_gameStateLock)
            {
                if (GetRemainingTime(move.Piece.GetPlayer()) > 0)
                {
                    if (_moveHistory.Count > 0) move.Previous = _moveHistory[_moveHistory.Count - 1];
                    _boardState = _rules.Move(move);
                    move.BoardAfter = _boardState;
                    _moveHistory.Add(move);
                    _clock.Switch();
                    _currentPlayer = (_currentPlayer + 1) % _rules.PlayerCount;
                }
            }
        }

        public Move GetBestMove()
        {
            lock(_gameStateLock)
            {
                return Evaluate(_boardState).BestMove;
            }
        }

        public bool IsGameOver()
        {
            lock(_gameStateLock)
            {
                return DidClockRunOut() || _rules.IsGameOver(_boardState);
            }
        }

        public GameResult GetGameResult()
        {
            
            if (!IsGameOver())
            {
                throw new Exception("The game is not over yet.");
            }
            lock (_gameStateLock)
            {
                var playersWithRemainingTime = GetAllPlayersWithRemainingTime();
                if (playersWithRemainingTime.Count == 1)
                {
                    return new GameResult(playersWithRemainingTime[0]);
                }
                return _rules.GetGameResult(_boardState);
            }            
        }

        public MinimaxResult Evaluate(BoardState state)
        {
            lock(_gameStateLock)
            {
                if (state.GetScore() == null)
                {
                    lock (state)
                    {
                        var result = Minimax.GetBoardScore(_rules, state);
                        state.SetScore(result);
                    }
                }
                return state.GetScore();
            }
        }

        public IEnumerable<Move> GetLegalMoves(BoardSquare square)
        {
            lock(_gameStateLock)
            {
                return _rules.GetLegalMoves(square, _boardState, _currentPlayer);
            }
        }

        public int GetRemainingTime(int player)
        {
            return _clock.GetRemainingTime(player);
        }

        public int GetCurrentPlayer()
        {
            lock(_gameStateLock)
            {
                return _currentPlayer;
            }
        }

        public List<Move> GetMoveHistory()
        {
            lock(_gameStateLock)
            {
                return _moveHistory;
            }
        }

        public BoardState GetBoardState()
        {
            lock(_gameStateLock)
            {
                return _boardState;
            }
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
