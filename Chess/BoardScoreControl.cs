using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    class BoardScoreControl : Control
    {
        private Button _toggleButton;
        private Label _scoreLabel;
        private bool _shown = false;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(_toggleButton == null)
            {
                _toggleButton = new Button()
                {
                    Location = new Point(0, 0),
                    Size = new Size(Width / 2, Height)
                };
                _toggleButton.Click += OnToggle;
                Controls.Add(_toggleButton);
            }

            if(_scoreLabel == null)
            {
                _scoreLabel = new Label()
                {
                    Location = new Point(Width / 2 + 12, 0),
                    Size = new Size(Width / 2 - 12, Height)
                };
                _scoreLabel.ForeColor = Color.White;
                _scoreLabel.Text = "0.00";
                Controls.Add(_scoreLabel);
            }
        }

        public void SetScore(string score)
        {
            _scoreLabel.Text = score.ToString();
        }

        private void OnToggle(object sender, EventArgs e)
        {
            if(_shown)
            {
                _shown = false;
                _scoreLabel.Hide();
            }
            else
            {
                _shown = true;
                _scoreLabel.Show();
            }
        }
    }
}
