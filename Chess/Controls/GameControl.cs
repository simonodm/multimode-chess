using ChessCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGUI.Controls
{
    /// <summary>
    /// A control which displays a chess game and enables the user to play it.
    /// Contains the board, move history, players' clocks, board score, game result, and
    /// an exit button.
    /// </summary>
    internal class GameControl : UserControl
    {
        private const string CALCULATING_STRING = "Calculating...";

        // Lock to prevent race condition in SafeInvoke between dispose check and Control.Invoke().
        // Must be locked every time a child control is being disposed.
        private readonly object _disposeLock = new object();

        /// <summary>
        /// Occurs when the game requires user input to continue.
        /// </summary>
        public event MultipleOptionEventHandler UserInputRequired;

        /// <summary>
        /// Occurs when user exits the game.
        /// </summary>
        public event EventHandler GameCancelled;

        private PlayableChessBoardControl _boardControl;
        private MoveHistoryControl _moveHistory;
        private BoardScoreControl _scoreControl;
        private ClockControl _clockControl;
        private Label _winnerLabel;
        private Button _cancelButton;

        private readonly ChessGame _game;
        private readonly bool _versusAi;
        private readonly int _humanPlayer;
        private readonly HashSet<BoardState> _evaluatedStates = new HashSet<BoardState>();
        private readonly TaskFactory _taskFactory = new TaskFactory();
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        private Board _currentGameBoard;

        public GameControl(GameStartEventArgs startArgs)
        {
            _game = startArgs.Game;
            _versusAi = startArgs.VersusAi;
            _humanPlayer = startArgs.HumanPlayer;
            _currentGameBoard = _game.GetBoardState().GetBoard();

            bool boardFlipped = startArgs.HumanPlayer == 1;
            InitializeControls(startArgs.TimeLimit, startArgs.Increment, boardFlipped);
            _clockControl.Start();
        }

        #region Controls

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _boardControl.Location = new Point(12, 12);
            _boardControl.Size = new Size(Height - 24, Height - 24);

            _moveHistory.Location = new Point(Height + 96, 12);
            _moveHistory.Size = new Size(Width - Height - 108, (Height / 2) - 24);

            _clockControl.Location = new Point(Height, 12);
            _clockControl.Size = new Size(72, Height);

            _winnerLabel.Location = new Point(Height + 96, (Height / 2) - 12);
            _winnerLabel.Size = new Size(Width - Height - 108, 24);

            _scoreControl.Location = new Point(Height + 96, Height / 2 + 12);
            _scoreControl.Size = new Size(Width - Height - 108, 24);

            _cancelButton.Location = new Point(Width - _cancelButton.Size.Width - 12, Height - _cancelButton.Size.Height - 12);
        }

        private void InitializeControls(int timeLimit, int increment, bool boardFlipped)
        {
            _boardControl = GenerateBoardControl(boardFlipped);
            _moveHistory = GenerateMoveHistoryControl();
            _scoreControl = GenerateScoreControl();
            _clockControl = GenerateClockControl(timeLimit, increment, boardFlipped);
            _winnerLabel = GenerateWinnerLabel();
            _cancelButton = GenerateCancelButton();

            Controls.Add(_boardControl);
            Controls.Add(_moveHistory);
            Controls.Add(_scoreControl);
            Controls.Add(_clockControl);
            Controls.Add(_winnerLabel);
            Controls.Add(_cancelButton);
        }

        private PlayableChessBoardControl GenerateBoardControl(bool boardFlipped)
        {
            var boardControl = new PlayableChessBoardControl(_game.GetBoardState().GetBoard(), boardFlipped);
            boardControl.MovePlayed += board_OnMove;
            boardControl.MoveInputRequested += board_OnMoveInputRequired;
            boardControl.LegalMovesRequested += board_LegalMovesRequested;

            return boardControl;
        }

        private MoveHistoryControl GenerateMoveHistoryControl()
        {
            var moveHistory = new MoveHistoryControl();
            moveHistory.SelectedMoveChanged += moveHistory_OnSelectedMoveChange;

            return moveHistory;
        }

        private ClockControl GenerateClockControl(int timeLimit, int increment, bool flipped = false)
        {
            var clockControl = new ClockControl(timeLimit, increment, flipped);

            clockControl.RunOut += clock_OnRunOut;

            return clockControl;
        }

        private Label GenerateWinnerLabel()
        {
            var winnerLabel = new Label
            {
                ForeColor = Color.White
            };

            return winnerLabel;
        }

        private BoardScoreControl GenerateScoreControl()
        {
            var scoreControl = new BoardScoreControl();

            return scoreControl;
        }

        private Button GenerateCancelButton()
        {
            var button = new Button()
            {
                Text = "Exit",
                ForeColor = Color.White
            };
            button.Click += OnGameCancelled;

            return button;
        }

        #endregion
        #region Events
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (_game.IsGameOver())
            {
                OnGameFinish(this, e);
            }
            else if (_humanPlayer == 1)
            {
                RunCancellableTask(ProcessAiMove);
            }
        }

        protected override void Dispose(bool disposing)
        {
            lock (_disposeLock)
            {
                base.Dispose(disposing);
            }
        }

        private void moveHistory_OnSelectedMoveChange(object sender, MoveEventArgs e)
        {
            var move = e.Move;
            bool isBoardCurrent = move.BoardAfter.GetBoard() == _currentGameBoard;
            _boardControl.UpdateBoard(move.BoardAfter.GetBoard(), isBoardCurrent);

            if (!_evaluatedStates.Contains(move.BoardAfter))
            {
                _evaluatedStates.Add(move.BoardAfter);

                RunCancellableTask(() => EvaluateStateJob(move));
            }
            else if (move.BoardAfter.GetScore() != null)
            {
                _scoreControl.SetScore(GetScoreString(move.BoardAfter.GetScore()));
            }
            else
            {
                _scoreControl.SetScore(CALCULATING_STRING);
            }
        }

        private void board_OnMove(object sender, MoveEventArgs e)
        {
            RunCancellableTask(() => ProcessMoveJob(e));
        }

        private void board_OnMoveInputRequired(object sender, MoveEventArgs e)
        {
            var args = new MultipleOptionEventArgs { Options = e.Move.Options };
            OnOptionPickRequired(this, args);
            e.Move.SelectOption(args.PickedOption);
        }

        private void board_LegalMovesRequested(object sender, LegalMovesEventArgs e)
        {
            e.LegalMoves = _game.GetLegalMoves(e.Square);
        }

        private void clock_OnRunOut(object sender, EventArgs e)
        {
            _game.EndGame((_game.GetCurrentPlayer() + 1) % 2);
            OnGameFinish(sender, e);
        }

        private void OnOptionPickRequired(object sender, MultipleOptionEventArgs e)
        {
            UserInputRequired?.Invoke(this, e);
        }

        private void OnGameFinish(object sender, EventArgs e)
        {
            var gameResult = _game.GetGameResult();
            string resultString = gameResult switch
            {
                GameResult.WHITE_WIN => "Winner: White",
                GameResult.BLACK_WIN => "Winner: Black",
                GameResult.DRAW => "Draw",
                _ => ""
            };
            SafeInvoke(_winnerLabel, () =>
            {
                _winnerLabel.Text = resultString;
            });
            SafeInvoke(_clockControl, () =>
            {
                _clockControl.Stop();
            });
        }

        private void OnGameCancelled(object sender, EventArgs e)
        {
            if (GameCancelled == null) return;

            GameCancelled.Invoke(this, e);
            _cancellationSource.Cancel();
            _cancellationSource.Dispose();
        }
        #endregion
        #region Workers
        private void RunCancellableTask(Action action)
        {
            try
            {
                _taskFactory.StartNew(action, _cancellationSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
            catch (AggregateException ae)
            {
                foreach (Exception e in ae.InnerExceptions)
                {
                    if (e is not TaskCanceledException)
                    {
                        throw e;
                    }
                }
            }
        }

        private string GetScoreString(MinimaxResult score)
        {
            if (score.GameOverCanBeForced)
            {
                return score.ForcedWinner == 0 ? "Forced mate by white" : "Forced mate by black";
            }
            return score.Score.ToString("F2");
        }

        private void EvaluateStateJob(Move move)
        {
            SafeInvoke(_scoreControl, () =>
            {
                _scoreControl.SetScore(CALCULATING_STRING);
            });

            var score = _game.Evaluate(move.BoardAfter);
            if (move.BoardAfter.GetBoard() == _currentGameBoard)
            {
                SafeInvoke(_scoreControl, () =>
                {
                    _scoreControl.SetScore(GetScoreString(score));
                });
            }
        }

        private void ProcessMoveJob(MoveEventArgs e)
        {
            if (e.Move == null)
            {
                throw new Exception("Null move supplied.");
            }

            SafeInvoke(_boardControl, () => _boardControl.Disable());
            ProcessMove(e.Move);
            if (_versusAi && !_game.IsGameOver())
            {
                ProcessAiMove();
            }

            SafeInvoke(_boardControl, () => _boardControl.Enable());
        }

        private void ProcessAiMove()
        {
            var aiMove = _game.GetNextBestMove();
            if (aiMove == null)
            {
                throw new Exception("No AI move returned by the engine.");
            }

            ProcessMove(aiMove);
        }

        private void ProcessMove(Move move)
        {
            _game.ProcessMove(move);
            _currentGameBoard = move.BoardAfter.GetBoard();

            SafeInvoke(_moveHistory, () =>
            {
                _moveHistory.AddMove(move);
            });
            SafeInvoke(_clockControl, () =>
            {
                _clockControl.Switch();
            });

            bool isGameOver = _game.IsGameOver();
            if (isGameOver)
            {
                OnGameFinish(this, new EventArgs());
            }
        }

        private void SafeInvoke(Control control, Action action)
        {
            lock (_disposeLock)
            {
                if (!control.IsDisposed && control.IsHandleCreated)
                {
                    Invoke(new Action(() => control.Invoke(action)));
                }
                else if (!control.IsDisposed && !control.IsHandleCreated)
                {
                    control.HandleCreated += (sender, e) => control.Invoke(action);
                }
            }
        }

        #endregion
    }
}
