using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Game;

namespace Chess
{
    class MoveHistoryControl : Control
    {
        ChessGame _game;
        ListBox _listView;
        public MoveHistoryControl(ChessGame game)
        {
            _game = game;
        }

        public void Update()
        {
            for(int i = _listView.Items.Count; i < _game.MoveHistory.Count; i++)
            {
                _listView.Items.Add(_game.Rules.GetMoveNotation(_game.MoveHistory[i]));
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _listView = new ListBox()
            {
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(Width, Height)
            };
            foreach(var move in _game.MoveHistory)
            {
                _listView.Items.Add(_game.Rules.GetMoveNotation(move));
            }

            Controls.Add(_listView);
        }
    }
}
