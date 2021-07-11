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
        public delegate void MoveEventHandler(object sender, MoveEventArgs e); 
        public event MoveEventHandler SelectedMoveChanged
        {
            add
            {
                onSelectedMoveChanged += value;
            }
            remove
            {
                onSelectedMoveChanged -= value;
            }
        }
        private ChessGame _game;
        private ListBox _listBox;
        private event MoveEventHandler onSelectedMoveChanged;

        public MoveHistoryControl(ChessGame game)
        {
            _game = game;
        }

        public void UpdateHistory()
        {
            for(int i = _listBox.Items.Count; i < _game.MoveHistory.Count; i++)
            {
                _listBox.Items.Add(_game.Rules.GetMoveNotation(_game.MoveHistory[i]));
            }
            _listBox.SelectedIndex = _listBox.Items.Count - 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(_listBox == null)
            {
                _listBox = new ListBox()
                {
                    Location = new System.Drawing.Point(0, 0),
                    Size = new System.Drawing.Size(Width, Height)
                };
                foreach (var move in _game.MoveHistory)
                {
                    _listBox.Items.Add(_game.Rules.GetMoveNotation(move));
                }

                _listBox.SelectedIndexChanged += listBox_OnSelectedIndexChanged;

                Controls.Add(_listBox);
            }
            
        }

        protected virtual void listBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var args = new MoveEventArgs()
            {
                Move = _game.MoveHistory[_listBox.SelectedIndex]
            };
            OnSelectedMoveChanged(args);
        }

        private void OnSelectedMoveChanged(MoveEventArgs e)
        {
            onSelectedMoveChanged?.Invoke(this, e);
        }
    }
}
