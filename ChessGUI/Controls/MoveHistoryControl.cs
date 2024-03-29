﻿using ChessCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGUI.Controls
{
    internal class MoveHistoryControl : UserControl
    {
        /// <summary>
        /// Occurs when the user selects a different move from history.
        /// </summary>
        public event MoveEventHandler SelectedMoveChanged;

        private ListBox _listBox;

        public MoveHistoryControl()
        {
            InitializeControls();
        }

        /// <summary>
        /// Adds a new move to history.
        /// </summary>
        /// <param name="move">Move to add</param>
        public void AddMove(Move move)
        {
            _listBox.Items.Add(move);
            _listBox.SelectedIndex = _listBox.Items.Count - 1;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _listBox.Location = new Point(0, 0);
            _listBox.Size = new Size(Width, Height);
        }

        protected virtual void listBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            var args = new MoveEventArgs()
            {
                Move = (Move)_listBox.SelectedItem
            };

            OnSelectedMoveChanged(args);
        }

        private void InitializeControls()
        {
            _listBox = new ListBox
            {
                IntegralHeight = false,
                DisplayMember = "Notation"
            };
            _listBox.SelectedIndexChanged += listBox_OnSelectedIndexChanged;

            Controls.Add(_listBox);
        }

        private void OnSelectedMoveChanged(MoveEventArgs e)
        {
            SelectedMoveChanged?.Invoke(this, e);
        }
    }
}
