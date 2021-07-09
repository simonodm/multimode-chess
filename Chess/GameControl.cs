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
        private ChessGame _game;
        
        public GameControl(ChessGame game)
        {
            _game = game;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _boardControl = new ChessBoardControl(_game)
            {
                Location = new Point(12, 12),
                Size = new Size(Height - 24, Height - 24)
            };

            Controls.Add(_boardControl);

            _moveHistory = new MoveHistoryControl(_game)
            {
                Location = new Point(Height, 12),
                Size = new Size(Width - Height - 12, Height - 24)
            };

            _moveHistory.SelectedMoveChanged += OnSelectedMoveChange;

            Controls.Add(_moveHistory);

            _boardControl.Moved += OnMove;
        }

        private void OnSelectedMoveChange(object sender, MoveEventArgs e)
        {
            var move = e.Move;
            _boardControl.UpdateBoard(move.BoardAfter);
        }

        private void OnMove(object sender, EventArgs e)
        {
            _moveHistory.Update();
        }
    }
}
