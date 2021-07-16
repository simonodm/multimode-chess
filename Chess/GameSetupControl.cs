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
    class GameSetupControl : Control
    {
        public delegate void GameStartEventHandler(object sender, GameStartEventArgs e);
        public event GameStartEventHandler GameStart
        {
            add
            {
                _onGameStart += value;
            }
            remove
            {
                _onGameStart -= value;
            }
        }
        private ComboBox _comboBoxModes;
        private CheckBox _checkBoxOpponent;
        private NumericUpDown _timeLimit;
        private NumericUpDown _increment;
        private Button _startButton;
        private event GameStartEventHandler _onGameStart;

        public GameSetupControl()
        {
            _comboBoxModes = GenerateGameModeDropdown();
            _timeLimit = GenerateTimeLimitControl();
            _increment = GenerateIncrementControl();
            _startButton = GenerateStartButton();
            _checkBoxOpponent = GenerateOpponentCheckbox();
            Controls.Add(_comboBoxModes);
            Controls.Add(_timeLimit);
            Controls.Add(_increment);
            Controls.Add(_startButton);
            Controls.Add(_checkBoxOpponent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private ComboBox GenerateGameModeDropdown()
        {
            var gameModeOptions = new List<Option>();
            gameModeOptions.Add(new Option(0, "Classic"));
            gameModeOptions.Add(new Option(1, "Pawn of the Dead"));
            var comboBoxModes = new ComboBox
            {
                Location = new Point(12, 12)
            };
            comboBoxModes.ValueMember = "Id";
            comboBoxModes.DisplayMember = "Text";
            comboBoxModes.DataSource = gameModeOptions;
            return comboBoxModes;
        }

        private CheckBox GenerateOpponentCheckbox()
        {
            var opponentCheckbox = new CheckBox
            {
                Location = new Point(12, 48)
            };
            return opponentCheckbox;
        }
        private NumericUpDown GenerateTimeLimitControl()
        {
            var timeLimit = new NumericUpDown
            {
                Location = new Point(12, 84),
                Minimum = 1,
                Maximum = 60 * 60 * 24,
                Increment = 5,
                Value = 600
            };
            return timeLimit;
        }

        private NumericUpDown GenerateIncrementControl()
        {
            var increment = new NumericUpDown
            {
                Location = new Point(12, 120),
                Minimum = 0,
                Maximum = 60,
                Increment = 1,
                Value = 0
            };
            return increment;
        }

        private Button GenerateStartButton()
        {
            var startButton = new Button
            {
                Location = new Point(12, 156)
            };
            startButton.Text = "Start game";
            startButton.Click += startButton_OnClick;
            return startButton;
        }

        private void startButton_OnClick(object sender, EventArgs e)
        {
            var args = new GameStartEventArgs
            {
                Game = ChessGame.CreateFromModeId((int)_comboBoxModes.SelectedValue, (int)_timeLimit.Value, (int)_increment.Value, _checkBoxOpponent.Checked)
            };
            OnGameStart(args);
        }

        private void OnGameStart(GameStartEventArgs e)
        {
            _onGameStart?.Invoke(this, e);
        }
    }
}
