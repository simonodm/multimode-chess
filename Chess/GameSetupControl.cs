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
        private Label _gameModeLabel;
        private ComboBox _comboBoxModes;
        private Label _opponentLabel;
        private CheckBox _checkBoxOpponent;
        private Label _timeLimitLabel;
        private NumericUpDown _timeLimit;
        private Label _incrementLabel;
        private NumericUpDown _increment;
        private Button _startButton;
        private event GameStartEventHandler _onGameStart;

        public GameSetupControl()
        {
            _gameModeLabel = GenerateLabel("Select game mode");
            _opponentLabel = GenerateLabel("Play vs AI?");
            _timeLimitLabel = GenerateLabel("Select time limit per player");
            _incrementLabel = GenerateLabel("Select increment per player");
            _comboBoxModes = GenerateGameModeDropdown();
            _timeLimit = GenerateTimeLimitControl();
            _increment = GenerateIncrementControl();
            _startButton = GenerateStartButton();
            _checkBoxOpponent = GenerateOpponentCheckbox();
            Controls.Add(_comboBoxModes);
            Controls.Add(_gameModeLabel);
            Controls.Add(_opponentLabel);
            Controls.Add(_timeLimitLabel);
            Controls.Add(_incrementLabel);
            Controls.Add(_timeLimit);
            Controls.Add(_increment);
            Controls.Add(_startButton);
            Controls.Add(_checkBoxOpponent);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _comboBoxModes.Size = new Size(Width / 6, Height / 8);
            _gameModeLabel.Location = new Point((Width - _gameModeLabel.Size.Width) / 2, 12);
            _opponentLabel.Location = new Point((Width - _opponentLabel.Size.Width) / 2, 76);
            _timeLimitLabel.Location = new Point((Width - _timeLimitLabel.Size.Width) / 2, 140);
            _incrementLabel.Location = new Point((Width - _incrementLabel.Size.Width) / 2, 204);
            _comboBoxModes.Location = new Point(Width / 2 - Width / 12, 36);
            _checkBoxOpponent.Location = new Point((Width - _checkBoxOpponent.Size.Width) / 2, 100);
            _timeLimit.Location = new Point((Width - _timeLimit.Size.Width) / 2, 164);
            _increment.Location = new Point((Width - _increment.Size.Width) / 2, 228);
            _startButton.Location = new Point((Width - _startButton.Size.Width) / 2, 292);
        }

        private ComboBox GenerateGameModeDropdown()
        {
            var gameModeOptions = new List<Option>();
            gameModeOptions.Add(new Option(0, "Classic"));
            gameModeOptions.Add(new Option(1, "Pawn of the Dead"));
            var comboBoxModes = new ComboBox();
            comboBoxModes.ValueMember = "Id";
            comboBoxModes.DisplayMember = "Text";
            comboBoxModes.DataSource = gameModeOptions;
            return comboBoxModes;
        }

        private CheckBox GenerateOpponentCheckbox()
        {
            var opponentCheckbox = new CheckBox();
            opponentCheckbox.AutoSize = true;
            return opponentCheckbox;
        }
        private NumericUpDown GenerateTimeLimitControl()
        {
            var timeLimit = new NumericUpDown
            {
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
                Size = new Size(128, 64)
            };
            startButton.Text = "Start game";
            startButton.ForeColor = Color.White;
            startButton.Click += startButton_OnClick;
            return startButton;
        }

        private Label GenerateLabel(string text)
        {
            var label = new Label
            {
                Size = new Size(200, 20)
            };
            label.Text = text;
            label.ForeColor = Color.White;
            label.AutoSize = true;
            return label;
        }

        private void startButton_OnClick(object sender, EventArgs e)
        {
            var args = new GameStartEventArgs
            {
                Game = GameCreator.CreateFromModeId((int)_comboBoxModes.SelectedValue, (int)_timeLimit.Value, (int)_increment.Value, _checkBoxOpponent.Checked)
            };
            OnGameStart(args);
        }

        private void OnGameStart(GameStartEventArgs e)
        {
            _onGameStart?.Invoke(this, e);
        }
    }
}
