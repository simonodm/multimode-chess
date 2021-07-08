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
        private ChessGame _game;
        
        public GameControl(ChessGame game)
        {
            _game = game;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            ChessBoardControl boardControl = new ChessBoardControl(_game)
            {
                Location = new Point(12, 12),
                Size = new Size(Height - 24, Height - 24)
            };

            Controls.Add(boardControl);
        }
    }
}
