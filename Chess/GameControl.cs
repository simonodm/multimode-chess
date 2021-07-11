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
        private ChessBoardControl _boardControl;
        private MoveHistoryControl _moveHistory;
        private BoardScoreControl _scoreControl;
        private Label _winnerLabel;
        private ChessGame _game;
        
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
                Controls.Add(_boardControl);
            }
            
            if(_moveHistory == null)
            {
                _moveHistory = new MoveHistoryControl(_game)
                {
                    Location = new Point(Height, 12),
                    Size = new Size(Width - Height - 12, (Height / 2) - 24)
                };
                _moveHistory.SelectedMoveChanged += OnSelectedMoveChange;

                Controls.Add(_moveHistory);
            }

            if(_winnerLabel == null)
            {
                _winnerLabel = new Label()
                {
                    Size = new Size(Width - Height - 12, 24),
                    Location = new Point(Height, (Height / 2) - 12),
                    Text = "Winner: "
                };
                _winnerLabel.ForeColor = Color.White;

                Controls.Add(_winnerLabel);
            }
            
            if(_scoreControl == null)
            {
                _scoreControl = new BoardScoreControl()
                {
                    Size = new Size((Width - Height - 12) / 2, 24),
                    Location = new Point(Height, Height / 2 + 12)
                };
                Controls.Add(_scoreControl);
            }

        }

        private void OnSelectedMoveChange(object sender, MoveEventArgs e)
        {
            var move = e.Move;
            _boardControl.UpdateBoard(move.BoardAfter);
        }

        private void OnMove(object sender, EventArgs e)
        {
            _moveHistory.UpdateHistory();
            _scoreControl.SetScore(_game.Rules.GetBoardScore(_game.BoardState).ToString());
            if (_game.IsGameOver())
            {
                _winnerLabel.Text = $"Winner: {_game.Rules.GetGameResult().GetWinner()}";
            }
        }
    }
}
