using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;
using Chess.Game;

namespace Chess
{
    class GameControl : Control
    {
        public delegate void MultipleOptionEventHandler(object sender, MultipleOptionEventArgs e);
        public event MultipleOptionEventHandler OptionPickRequired
        {
            add
            {
                _onOptionPickRequired += value;
            }
            remove
            {
                _onOptionPickRequired -= value;
            }
        }
        private PlayableChessBoardControl _boardControl;
        private MoveHistoryControl _moveHistory;
        private BoardScoreControl _scoreControl;
        private ClockControl _clockControl;
        private Label _winnerLabel;
        private ChessGame _game;
        private event MultipleOptionEventHandler _onOptionPickRequired;
        private bool _versusAi;
        private HashSet<BoardState> _evaluatedStates = new HashSet<BoardState>();

        public GameControl(ChessGame game, bool versusAi)
        {
            _game = game;
            _versusAi = versusAi;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(_boardControl == null)
            {
                _boardControl = new PlayableChessBoardControl(_game.GetBoardState().GetBoard())
                {
                    Location = new Point(12, 12),
                    Size = new Size(Height - 24, Height - 24)
                };
                _boardControl.MovePlayed += OnMove;
                _boardControl.MoveInputRequested += board_OnMoveInputRequired;
                _boardControl.LegalMovesRequested += board_LegalMovesRequested;
                _boardControl.BoardRequested += board_CurrentBoardRequested;
                Controls.Add(_boardControl);
            }
            
            if(_moveHistory == null)
            {
                _moveHistory = new MoveHistoryControl()
                {
                    Location = new Point(Height + 96, 12),
                    Size = new Size(Width - Height - 108, (Height / 2) - 24)
                };
                _moveHistory.SelectedMoveChanged += OnSelectedMoveChange;

                Controls.Add(_moveHistory);
            }

            if(_clockControl == null)
            {
                _clockControl = new ClockControl(_game)
                {
                    Location = new Point(Height, 12),
                    Size = new Size(72, Height)
                };

                _clockControl.RunOut += OnGameFinish;

                Controls.Add(_clockControl);
            }

            if(_winnerLabel == null)
            {
                _winnerLabel = new Label()
                {
                    Size = new Size(Width - Height - 108, 24),
                    Location = new Point(Height + 96, (Height / 2) - 12),
                    Text = "Winner: "
                };
                _winnerLabel.ForeColor = Color.White;

                Controls.Add(_winnerLabel);
            }
            
            if(_scoreControl == null)
            {
                _scoreControl = new BoardScoreControl()
                {
                    Size = new Size((Width - Height - 108) / 2, 24),
                    Location = new Point(Height + 96, Height / 2 + 12)
                };
                Controls.Add(_scoreControl);
            }

        }

        private void OnSelectedMoveChange(object sender, MoveEventArgs e)
        {
            var move = e.Move;
            _boardControl.UpdateBoard(move.BoardAfter.GetBoard());
            if(!_evaluatedStates.Contains(move.BoardAfter))
            {
                _evaluatedStates.Add(move.BoardAfter);
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    _scoreControl.Invoke(new Action(() =>
                    {
                        _scoreControl.SetScore("Calculating...");
                    }));
                    var score = _game.Evaluate(move.BoardAfter);
                    if(move.BoardAfter == _game.GetBoardState())
                    {
                        _scoreControl.Invoke(new Action(() =>
                        {
                            _scoreControl.SetScore(score.Score.ToString());
                        }));
                    }
                });
            }
            else if(move.BoardAfter.GetScore() != null)
            {
                _scoreControl.SetScore(move.BoardAfter.GetScore().Score.ToString());
            }
            else
            {
                _scoreControl.SetScore("Calculating...");
            }
        }

        private void OnMove(object sender, MoveEventArgs e)
        {
            ThreadPool.QueueUserWorkItem((state) =>
            {
                _boardControl.Invoke(new Action(() =>
                {
                    _boardControl.Disable();
                }));
                _game.ProcessMove(e.Move);
                if (_game.IsGameOver())
                {
                    OnGameFinish(this, e);
                }
                _moveHistory.Invoke(new Action(() =>
                {
                    _moveHistory.AddMove(e.Move);
                }));
                if (_versusAi)
                {
                    var aiMove = _game.GetBestMove();
                    _game.ProcessMove(aiMove);
                    if (_game.IsGameOver())
                    {
                        OnGameFinish(this, e);
                    }
                    _moveHistory.Invoke(new Action(() =>
                    {
                        _moveHistory.AddMove(aiMove);
                    }));
                }
                _boardControl.Invoke(new Action(() =>
                {
                    _boardControl.Enable();
                }));
            });
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

        private void OnOptionPickRequired(object sender, MultipleOptionEventArgs e)
        {
            _onOptionPickRequired?.Invoke(this, e);

        }

        private void OnGameFinish(object sender, EventArgs e)
        {
            _winnerLabel.Invoke(new Action(() =>
            {
                _winnerLabel.Text = $"Winner: {_game.GetGameResult().GetWinnerString()}";
            }));
        }
    }
}
