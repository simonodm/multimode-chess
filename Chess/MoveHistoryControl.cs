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
        private BindingSource _moveSource;
        private List<Move> _moveHistory;

        public MoveHistoryControl(ChessGame game)
        {
            _game = game;
            _moveSource = new BindingSource();
            _moveHistory = game.GetMoveHistory();
            _moveSource.DataSource = _moveHistory;
        }

        public void UpdateHistory()
        {
            _moveSource.ResetBindings(false);
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
                _listBox.DataSource = _moveSource;
                _listBox.DisplayMember = "Notation";
                
                _listBox.SelectedIndexChanged += listBox_OnSelectedIndexChanged;
                //_listBox.DisplayMember += listBox_OnDataSourceChange;

                Controls.Add(_listBox);
            }
            
        }

        protected virtual void listBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var args = new MoveEventArgs()
            {
                Move = _moveHistory[_listBox.SelectedIndex]
            };
            OnSelectedMoveChanged(args);
        }

        private void OnSelectedMoveChanged(MoveEventArgs e)
        {
            onSelectedMoveChanged?.Invoke(this, e);
        }
    }
}
