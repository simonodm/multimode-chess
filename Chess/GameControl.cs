using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private ChessBoardControl _boardControl;
        private MoveHistoryControl _moveHistory;
        private BoardScoreControl _scoreControl;
        private ClockControl _clockControl;
        private Label _winnerLabel;
        private ChessGame _game;
        private event MultipleOptionEventHandler _onOptionPickRequired;

        public GameControl(ChessGame game)
        {
            _game = game;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(_boardControl == null)
            {
                _boardControl = new ChessBoardControl(_game)
                {
                    Location = new Point(12, 12),
                    Size = new Size(Height - 24, Height - 24)
                };
                _boardControl.MovePlayed += OnMove;
                _boardControl.MoveInputRequired += board_OnMoveInputRequired;
                Controls.Add(_boardControl);
            }
            
            if(_moveHistory == null)
            {
                _moveHistory = new MoveHistoryControl(_game)
                {
                    Location = new Point(Height + 96, 12),
                    Size = new Size(Width - Height - 108, (Height / 2) - 24)
                };
                _moveHistory.SelectedMoveChanged += OnSelectedMoveChange;

                Controls.Add(_moveHistory);
            }

            if(_clockControl == null)
            {
                _clockControl = new ClockControl(_game.Clock)
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
            _boardControl.UpdateBoard(move.BoardAfter);
        }

        private void OnMove(object sender, MoveEventArgs e)
        {
            _moveHistory.AddMove(e.Move);
            _scoreControl.SetScore(_game.Rules.GetBoardScore(_game.BoardState).ToString());
            if (_game.IsGameOver())
            {
                OnGameFinish(this, e);
            }
        }

        private void board_OnMoveInputRequired(object sender, MoveEventArgs e)
        {
            var args = new MultipleOptionEventArgs { Options = e.Move.Options };
            OnOptionPickRequired(this, args);
            e.Move.SelectOption(args.PickedOption.Id);
        }

        private void OnOptionPickRequired(object sender, MultipleOptionEventArgs e)
        {
            _onOptionPickRequired?.Invoke(this, e);

        }

        private void OnGameFinish(object sender, EventArgs e)
        {
            _winnerLabel.Text = $"Winner: {_game.GetGameResult().GetWinner()}"; 
        }
    }
}
