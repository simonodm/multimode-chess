﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.Controls
{
    class ClockControl : Control
    {
        public event EventHandler RunOut;
        private Label _remainingTimeWhiteLabel;
        private Label _remainingTimeBlackLabel;
        private Timer _timer;

        private int _remainingTimeWhite;
        private int _remainingTimeBlack;
        private int _increment;
        private int _currentPlayer = 0;
        private bool _flipped;

        public ClockControl(int timeLimit, int increment, bool flipped = false)
        {
            _timer = new Timer()
            {
                Interval = 1000
            };
            _timer.Tick += timer_Tick;

            _remainingTimeWhite = timeLimit;
            _remainingTimeBlack = timeLimit;
            _increment = increment;
            _flipped = flipped;

            InitializeControls();
        }

        public void Switch()
        {
            Increment(_increment);
            _currentPlayer = (_currentPlayer + 1) % 2;
            UpdateTimes();
        }

        public void Start()
        {
            _timer.Start();
            UpdateTimes();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void InitializeControls()
        {
            _remainingTimeWhiteLabel = new Label()
            {
                Size = new Size(128, 24),
                ForeColor = Color.White
            };

            _remainingTimeBlackLabel = new Label()
            {
                Size = new Size(128, 24),
                ForeColor = Color.White
            };

            Controls.Add(_remainingTimeWhiteLabel);
            Controls.Add(_remainingTimeBlackLabel);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var topLocation = new Point(0, 0);
            var bottomLocation = new Point(0, Height - 48);
            _remainingTimeBlackLabel.Location = _flipped ? bottomLocation : topLocation;
            _remainingTimeWhiteLabel.Location = _flipped ? topLocation : bottomLocation;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Decrement();

            UpdateTimes();

            if (_remainingTimeWhite <= 0 || _remainingTimeBlack <= 0)
            {
                OnRunOut(new EventArgs());
            }
        }

        private void UpdateTimes()
        {
            TimeSpan whiteTime = new TimeSpan(0, 0, _remainingTimeWhite);
            TimeSpan blackTime = new TimeSpan(0, 0, _remainingTimeBlack);

            string timeFormat = @"mm\:ss";
            _remainingTimeWhiteLabel.Text = whiteTime.ToString(timeFormat);
            _remainingTimeBlackLabel.Text = blackTime.ToString(timeFormat);
        }

        private void Increment(int increment = 0)
        {
            if (_currentPlayer == 0)
            {
                _remainingTimeWhite += increment;
            }
            else
            {
                _remainingTimeBlack += increment;
            }
        }

        private void Decrement(int decrement = 1)
        {
            if (_currentPlayer == 0)
            {
                _remainingTimeWhite -= decrement;
            }
            else
            {
                _remainingTimeBlack -= decrement;
            }
        }

        private void OnRunOut(EventArgs e)
        {
            RunOut?.Invoke(this, e);
        }
    }
}