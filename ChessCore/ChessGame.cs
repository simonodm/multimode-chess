using ChessCore.Modes;
using System.Collections.Generic;
using System.Linq;

namespace ChessCore
{
    public class ChessGame
    {
        private const int PLAYER_COUNT = 2;

        private readonly BoardState _defaultBoardState;
        private readonly IGameRules _rules;
        private readonly List<Move> _moveHistory;

        private BoardState _currentBoardState;
        private int _currentPlayer;

        private GameResult _gameResult;
        private bool _gameResultCurrent = false;

        private object _gameStateLock = new object();

        internal ChessGame(IGameRules rules, int timeLimit = 600, int increment = 0)
        {
            _rules = rules;
            _moveHistory = new List<Move>();
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
            lock (_gameStateLock)
            {
                _currentBoardState = _defaultBoardState;
                _currentPlayer = 0;
                _moveHistory.Clear();
                _gameResult = GameResult.ONGOING;
                _gameResultCurrent = true;
            }
        }

        public void ProcessMove(Move move)
        {
            lock (_gameStateLock)
            {
                if (_gameResult == GameResult.ONGOING)
                {
                    _currentBoardState = _rules.Move(move);
                    move.BoardAfter = _currentBoardState;
                    _moveHistory.Add(move);
                    _currentPlayer = (_currentPlayer + 1) % PLAYER_COUNT;
                    _gameResultCurrent = false;
                }
            }
        }

        public Move GetNextBestMove()
        {
            lock (_gameStateLock)
            {
                return Evaluate(_currentBoardState).BestMove;
            }
        }

        public bool IsGameOver()
        {
            lock (_gameStateLock)
            {
                if (!_gameResultCurrent)
                {
                    _gameResult = GetGameResult();
                    _gameResultCurrent = true;
                }
                return _gameResult != GameResult.ONGOING;
            }
        }

        public GameResult GetGameResult()
        {
            lock (_gameStateLock)
            {
                if (!_gameResultCurrent)
                {
                    UpdateGameResult();
                }

                return _gameResult;
            }
        }

        public MinimaxResult Evaluate(BoardState state)
        {
            if (state.GetScore() == null)
            {
                var minimax = new Minimax(_rules, state);
                var result = minimax.Evaluate();
                state.SetScore(result);
            }

            return state.GetScore();
        }

        public IEnumerable<Move> GetLegalMoves(BoardSquare square)
        {
            lock (_gameStateLock)
            {
                if (_gameResult == GameResult.ONGOING)
                {
                    return _rules.GetLegalMoves(square, _currentBoardState, _currentPlayer);
                }
                return Enumerable.Empty<Move>();
            }
        }

        public int GetCurrentPlayer()
        {
            lock (_gameStateLock)
            {
                return _currentPlayer;
            }
        }

        public List<Move> GetMoveHistory()
        {
            lock (_gameStateLock)
            {
                return _moveHistory;
            }
        }

        public BoardState GetBoardState()
        {
            lock (_gameStateLock)
            {
                return _currentBoardState;
            }
        }

        public void EndGame()
        {
            _gameResult = GameResult.DRAW;
            _gameResultCurrent = true;
        }

        public void EndGame(int winner)
        {
            _gameResult = winner == 0 ? GameResult.WHITE_WIN : GameResult.BLACK_WIN;
            _gameResultCurrent = true;
        }

        private void UpdateGameResult()
        {
            _gameResult = _rules.GetGameResult(_currentBoardState);
            _gameResultCurrent = true;
        }
    }
}
