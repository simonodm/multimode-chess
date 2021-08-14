using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessGUI.Controls
{
    /// <summary>
    /// A control which displays and updates the players' remaining times.
    /// </summary>
    internal class ClockControl : UserControl
    {
        /// <summary>
        /// Occurs when one of the clocks runs out.
        /// </summary>
        public event EventHandler RunOut;

        private Label _remainingTimeWhiteLabel;
        private Label _remainingTimeBlackLabel;
        private readonly Timer _timer;

        private int _remainingTimeWhite;
        private int _remainingTimeBlack;
        private readonly int _increment;
        private int _currentPlayer;
        private readonly bool _flipped;

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

        /// <summary>
        /// Switches the currently running timer to the other player.
        /// </summary>
        public void Switch()
        {
            Increment(_increment);
            _currentPlayer = (_currentPlayer + 1) % 2;
            UpdateTimes();
        }

        /// <summary>
        /// Starts the timers.
        /// </summary>
        public void Start()
        {
            _timer.Start();
            UpdateTimes();
        }

        /// <summary>
        /// Stops the timers.
        /// </summary>
        public void Stop()
        {
            _timer.Stop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var topLocation = new Point(0, 0);
            var bottomLocation = new Point(0, Height - 48);
            _remainingTimeBlackLabel.Location = _flipped ? bottomLocation : topLocation;
            _remainingTimeWhiteLabel.Location = _flipped ? topLocation : bottomLocation;
        }

        private void InitializeControls()
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
            var whiteTime = new TimeSpan(0, 0, _remainingTimeWhite);
            var blackTime = new TimeSpan(0, 0, _remainingTimeBlack);

            const string timeFormat = @"mm\:ss";
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
