using ChessCore.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.Controls
{
    class GameControl : Control
    {
        private const string CALCULATING_STRING = "Calculating...";

        public event MultipleOptionEventHandler OptionPickRequired;

        private PlayableChessBoardControl _boardControl;
        private MoveHistoryControl _moveHistory;
        private BoardScoreControl _scoreControl;
        private ClockControl _clockControl;
        private Label _winnerLabel;

        private ChessGame _game;
        private bool _versusAi;
        private int _humanPlayer;
        private HashSet<BoardState> _evaluatedStates = new HashSet<BoardState>();
        private TaskFactory _taskFactory = new TaskFactory();

        public GameControl(GameStartEventArgs startArgs)
        {
            _game = startArgs.Game;
            _versusAi = startArgs.VersusAi;
            _humanPlayer = startArgs.HumanPlayer;

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
        }

        private void InitializeControls(int timeLimit, int increment, bool boardFlipped)
        {
            _boardControl = GenerateBoardControl(boardFlipped);
            _moveHistory = GenerateMoveHistoryControl();
            _scoreControl = GenerateScoreControl();
            _clockControl = GenerateClockControl(timeLimit, increment, boardFlipped);
            _winnerLabel = GenerateWinnerLabel();

            Controls.Add(_boardControl);
            Controls.Add(_moveHistory);
            Controls.Add(_scoreControl);
            Controls.Add(_clockControl);
            Controls.Add(_winnerLabel);
        }

        private PlayableChessBoardControl GenerateBoardControl(bool boardFlipped)
        {
            var boardControl = new PlayableChessBoardControl(_game.GetBoardState().GetBoard(), boardFlipped);
            boardControl.MovePlayed += board_OnMove;
            boardControl.MoveInputRequested += board_OnMoveInputRequired;
            boardControl.LegalMovesRequested += board_LegalMovesRequested;
            boardControl.BoardRequested += board_CurrentBoardRequested;

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
            var winnerLabel = new Label();
            winnerLabel.ForeColor = Color.White;

            return winnerLabel;
        }

        private BoardScoreControl GenerateScoreControl()
        {
            var scoreControl = new BoardScoreControl();

            return scoreControl;
        }

        #endregion
        #region Events
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if(_humanPlayer == 1)
            {
                _taskFactory.StartNew(() =>
                {
                    ProcessAiMove();
                });
            }
        }

        private void moveHistory_OnSelectedMoveChange(object sender, MoveEventArgs e)
        {
            var move = e.Move;
            _boardControl.UpdateBoard(move.BoardAfter.GetBoard());

            if (!_evaluatedStates.Contains(move.BoardAfter))
            {
                _evaluatedStates.Add(move.BoardAfter);

                _taskFactory.StartNew(() => EvaluateStateJob(move), TaskCreationOptions.LongRunning);
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
            var thread = new Thread(() => ProcessMoveJob(e));
            thread.Start();
        }

        private void board_OnMoveInputRequired(object sender, MoveEventArgs e)
        {
            var args = new MultipleOptionEventArgs { Options = e.Move.Options };
            OnOptionPickRequired(this, args);
            e.Move.SelectOption(args.PickedOption.Id);
        }

        private void board_LegalMovesRequested(object sender, LegalMovesEventArgs e)
        {
            e.LegalMoves = _game.GetLegalMoves(e.Square);
        }

        private void board_CurrentBoardRequested(object sender, BoardEventArgs e)
        {
            e.Board = _game.GetBoardState().GetBoard();
        }

        private void clock_OnRunOut(object sender, EventArgs e)
        {
            _game.EndGame((_game.GetCurrentPlayer() + 1) % 2);
            OnGameFinish(sender, e);
        }

        private void OnOptionPickRequired(object sender, MultipleOptionEventArgs e)
        {
            OptionPickRequired?.Invoke(this, e);

        }

        private void OnGameFinish(object sender, EventArgs e)
        {
            var gameResult = _game.GetGameResult();
            string resultString = "";
            switch(gameResult)
            {
                case GameResult.WHITE_WIN:
                    resultString = "Winner: White";
                    break;
                case GameResult.BLACK_WIN:
                    resultString = "Winner: Black";
                    break;
                case GameResult.DRAW:
                    resultString = "Draw";
                    break;
            }
            _winnerLabel.Invoke(new Action(() =>
            {
                _winnerLabel.Text = resultString;
            }));
            _clockControl.Invoke(new Action(() =>
            {
                _clockControl.Stop();
            }));
        }

        #endregion
        #region Workers

        private string GetScoreString(MinimaxResult score)
        {
            if (score.IsGameOver)
            {
                return score.Winner == 0 ? $"Forced mate by white" : "Forced mate by black";
            }
            return score.Score.ToString("F2");
        }

        private void EvaluateStateJob(Move move)
        {
            _scoreControl.Invoke(new Action(() =>
            {
                _scoreControl.SetScore(CALCULATING_STRING);
            }));

            var score = _game.Evaluate(move.BoardAfter);
            if (move.BoardAfter == _game.GetBoardState())
            {
                _scoreControl.Invoke(new Action(() =>
                {
                    _scoreControl.SetScore(GetScoreString(score));
                }));
            }
        }

        private void ProcessMoveJob(MoveEventArgs e)
        {
            _boardControl.Invoke(new Action(() =>
            {
                _boardControl.Disable();
            }));

            _game.ProcessMove(e.Move);
            _moveHistory.Invoke(new Action(() =>
            {
                _moveHistory.AddMove(e.Move);
            }));

            bool isGameOver = _game.IsGameOver();
            if (isGameOver)
            {
                OnGameFinish(this, e);
            }
            else
            {
                _clockControl.Invoke(new Action(() => {
                    _clockControl.Switch();
                }));

                if(_versusAi)
                {
                    ProcessAiMove();
                }

                _boardControl.Invoke(new Action(() =>
                {
                    _boardControl.Enable();
                }));
            }          
        }

        private void ProcessAiMove()
        {
            var aiMove = _game.GetNextBestMove();
            if(aiMove == null)
            {
                throw new Exception("No AI move found.");
            }
            _game.ProcessMove(aiMove);
            _clockControl.Invoke(new Action(() => {
                _clockControl.Switch();
            }));

            if (_game.IsGameOver())
            {
                OnGameFinish(this, new EventArgs());
            }
            _moveHistory.Invoke(new Action(() =>
            {
                _moveHistory.AddMove(aiMove);
            }));
        }
        #endregion
    }
}
