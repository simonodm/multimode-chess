using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.Controls
{
    class BoardScoreControl : Control
    {
        private Button _toggleButton;
        private Label _scoreLabel;
        private bool _scoreShown;

        public BoardScoreControl()
        {
            InitializeControls();
        }

        public void SetScore(string score)
        {
            _scoreLabel.Text = score;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _toggleButton.Size = new Size(Width / 2, Height);
            _toggleButton.Location = new Point(0, 0);
            _scoreLabel.Size = new Size(Width / 2, _scoreLabel.Font.Height);
            _scoreLabel.Location = new Point(Width / 2 + 12, (_toggleButton.Size.Height - _scoreLabel.Size.Height) / 2);
        }

        private void InitializeControls()
        {
            _toggleButton = GenerateToggleButton();
            _scoreLabel = GenerateScoreLabel();

            Controls.Add(_toggleButton);
            Controls.Add(_scoreLabel);
        }

        private Button GenerateToggleButton()
        {
            var toggleButton = new Button
            {
                Text = "Toggle board score",
                ForeColor = Color.White
            };
            toggleButton.Click += toggleButton_OnClick;

            return toggleButton;
        }

        private Label GenerateScoreLabel()
        {
            var scoreLabel = new Label
            {
                ForeColor = Color.White,
                Text = "0.00"
            };
            if (!_scoreShown)
            {
                scoreLabel.Hide();
            }

            return scoreLabel;
        }

        private void toggleButton_OnClick(object sender, EventArgs e)
        {
            if (_scoreShown)
            {
                _scoreShown = false;
                _scoreLabel.Hide();
            }
            else
            {
                _scoreShown = true;
                _scoreLabel.Show();
            }
        }
    }
}
