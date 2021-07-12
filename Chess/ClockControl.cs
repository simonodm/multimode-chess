using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Game;

namespace Chess
{
    class ClockControl : Control
    {
        private Label _remainingTimeWhite;
        private Label _remainingTimeBlack;
        private Clock _clock;
        private Timer _timer;

        public ClockControl(Clock clock)
        {
            _clock = clock;
            _timer = new Timer();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if(_remainingTimeWhite == null)
            {
                _remainingTimeWhite = new Label()
                {
                    Location = new Point(0, Height - 48),
                    Size = new Size(128, 24),
                    ForeColor = Color.White
                };
                Controls.Add(_remainingTimeWhite);
            }
            
            if(_remainingTimeBlack == null)
            {
                _remainingTimeBlack = new Label()
                {
                    Location = new Point(0, 0),
                    Size = new Size(128, 24),
                    ForeColor = Color.White
                };
                Controls.Add(_remainingTimeBlack);
            }

            _timer.Tick += timer_Tick;

            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan whiteTime = new TimeSpan(0, 0, _clock.GetRemainingTime(0));
            TimeSpan blackTime = new TimeSpan(0, 0, _clock.GetRemainingTime(1));
            _remainingTimeWhite.Text = whiteTime.ToString(@"mm\:ss");
            _remainingTimeBlack.Text = blackTime.ToString(@"mm\:ss");
        }

    }
}
