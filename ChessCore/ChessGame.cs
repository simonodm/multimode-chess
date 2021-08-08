using ChessCore.Modes;
using System.Collections.Generic;
using System.Linq;

namespace ChessCore
{
    /// <summary>
    /// Represents a single chess game.
    /// </summary>
    public class ChessGame
    {
        private const int PLAYER_COUNT = 2;

        private readonly BoardState _defaultBoardState;
        private readonly IGameRules _rules;
        private readonly List<Move> _moveHistory;

        private BoardState _currentBoardState;
        private int _currentPlayer;

        private GameResult _gameResult;
        private bool _gameResultCurrent;

        private readonly object _gameStateLock = new object();

        internal ChessGame(IGameRules rules)
        {
            _rules = rules;
            _moveHistory = new List<Move>();
            _defaultBoardState = _rules.GetStartingBoardState();
            Reset();
        }

        internal ChessGame(IGameRules rules, Board board)
            : this(rules)
        {
            _defaultBoardState = _rules.GetStartingBoardState(board);
            Reset();
        }

        /// <summary>
        /// Resets the game.
        /// </summary>
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

        /// <summary>
        /// Processes the supplied move.
        /// </summary>
        /// <param name="move">Move to process</param>
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

        /// <summary>
        /// Retrieves the best possible (legal) move for the current board state as seen by minimax.
        /// </summary>
        /// <returns>A Move instance</returns>
        public Move GetNextBestMove()
        {
            lock (_gameStateLock)
            {
                return Evaluate(_currentBoardState).BestMove;
            }
        }

        /// <summary>
        /// Determines whether the game has finished.
        /// </summary>
        /// <returns>true if the game is over, false otherwise</returns>
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

        /// <summary>
        /// Retrieves the game result of the current board state.
        /// </summary>
        /// <returns>Game result</returns>
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

        /// <summary>
        /// Evaluates the supplied board state with a minimax algorithm.
        /// </summary>
        /// <param name="state">Board state to evaluate</param>
        /// <returns>A MinimaxResult instance</returns>
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

        /// <summary>
        /// Retrieves all the legal moves from the given square.
        /// </summary>
        /// <param name="square">Square to retrieve moves from</param>
        /// <returns>An enumerable of legal moves</returns>
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

        /// <summary>
        /// Retrieves the player who should play the next move.
        /// </summary>
        /// <returns>0 if white, 1 if black</returns>
        public int GetCurrentPlayer()
        {
            lock (_gameStateLock)
            {
                return _currentPlayer;
            }
        }

        /// <summary>
        /// Retrieves the game's move history up to current board state.
        /// </summary>
        /// <returns>A list of played moves</returns>
        public List<Move> GetMoveHistory()
        {
            lock (_gameStateLock)
            {
                return _moveHistory;
            }
        }

        /// <summary>
        /// Retrieves the current board state.
        /// </summary>
        /// <returns>Current board state</returns>
        public BoardState GetBoardState()
        {
            lock (_gameStateLock)
            {
                return _currentBoardState;
            }
        }

        /// <summary>
        /// Force ends the game with a draw.
        /// </summary>
        public void EndGame()
        {
            _gameResult = GameResult.DRAW;
            _gameResultCurrent = true;
        }

        /// <summary>
        /// Force ends the game with the specified winner
        /// </summary>
        /// <param name="winner">Game winner</param>
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
