using ChessCore;
using ChessCore.Exceptions;
using ChessCore.Modes;
using ChessCore.Modes.PawnOfTheDead;
using ChessCore.Modes.Standard;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess.Controls
{
    /// <summary>
    /// A control which allows the user to configure the game before start.
    /// </summary>
    internal class GameSetupControl : UserControl
    {
        /// <summary>
        /// Occurs when the setup requires additional user input to continue.
        /// </summary>
        public event MultipleOptionEventHandler UserInputRequired;

        /// <summary>
        /// Occurs when the user starts the game.
        /// </summary>
        public event GameStartEventHandler GameStart;

        private ComboBox _comboBoxModes;
        private ComboBox _comboBoxPlayer;
        private CheckBox _checkBoxOpponent;
        private NumericUpDown _timeLimit;
        private NumericUpDown _increment;
        private ConfigurableChessBoardControl _chessBoardSetup;

        private Label _gameModeLabel;
        private Label _opponentLabel;
        private Label _playerLabel;
        private Label _timeLimitLabel;
        private Label _incrementLabel;
        private Label _errorLabel;

        private Button _startButton;
        private Button _clearBoardButton;

        private IGameRules _rules;

        public GameSetupControl()
        {
            InitializeControls();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _comboBoxModes.Size = new Size(Width / 6, Height / 8);
            _chessBoardSetup.Size = new Size(Width / 4, Width / 4);
            _gameModeLabel.Location = new Point((Width - _gameModeLabel.Size.Width) / 2, 12);
            _opponentLabel.Location = new Point((Width - _opponentLabel.Size.Width) / 2, 76);
            _playerLabel.Location = new Point((Width - _playerLabel.Size.Width) / 2, 140);
            _errorLabel.Location = new Point((Width - _errorLabel.Size.Width) / 2, _chessBoardSetup.Location.Y + _chessBoardSetup.Size.Height + 12);
            _timeLimitLabel.Location = new Point((Width - _timeLimitLabel.Size.Width) / 2, 204);
            _incrementLabel.Location = new Point((Width - _incrementLabel.Size.Width) / 2, 268);
            _comboBoxModes.Location = new Point(Width / 2 - Width / 12, 36);
            _checkBoxOpponent.Location = new Point((Width - _checkBoxOpponent.Size.Width) / 2, 100);
            _comboBoxPlayer.Location = new Point((Width - _comboBoxPlayer.Size.Width) / 2, 164);
            _timeLimit.Location = new Point((Width - _timeLimit.Size.Width) / 2, 228);
            _increment.Location = new Point((Width - _increment.Size.Width) / 2, 292);
            _chessBoardSetup.Location = new Point((Width - _chessBoardSetup.Size.Width) / 2, 356);
            _startButton.Location = new Point((Width - _startButton.Size.Width) / 2, Height - _startButton.Size.Height - 12);
            _clearBoardButton.Location = new Point((Width - _chessBoardSetup.Size.Width) / 2 - 160, 356 + _chessBoardSetup.Size.Height / 2 - _clearBoardButton.Size.Height / 2);
        }

        private void InitializeControls()
        {
            _gameModeLabel = GenerateLabel("Select game mode", Color.White);
            _opponentLabel = GenerateLabel("Play vs AI?", Color.White);
            _playerLabel = GenerateLabel("Select piece color", Color.White);
            _timeLimitLabel = GenerateLabel("Select time limit per player", Color.White);
            _incrementLabel = GenerateLabel("Select increment per player", Color.White);
            _errorLabel = GenerateLabel("", Color.Red);
            _comboBoxModes = GenerateGameModeDropdown();
            _comboBoxPlayer = GeneratePlayerDropdown();
            _timeLimit = GenerateTimeLimitControl();
            _increment = GenerateIncrementControl();
            _chessBoardSetup = GenerateChessBoardControl();
            _startButton = GenerateButton("Start game", startButton_OnClick);
            _clearBoardButton = GenerateButton("Clear board", clearButton_OnClick);
            _checkBoxOpponent = GenerateOpponentCheckbox();

            Controls.Add(_comboBoxModes);
            Controls.Add(_comboBoxPlayer);
            Controls.Add(_gameModeLabel);
            Controls.Add(_opponentLabel);
            Controls.Add(_playerLabel);
            Controls.Add(_errorLabel);
            Controls.Add(_timeLimitLabel);
            Controls.Add(_incrementLabel);
            Controls.Add(_timeLimit);
            Controls.Add(_increment);
            Controls.Add(_chessBoardSetup);
            Controls.Add(_startButton);
            Controls.Add(_clearBoardButton);
            Controls.Add(_checkBoxOpponent);
        }

        private ComboBox GenerateGameModeDropdown()
        {
            var gameModeOptions = new List<Option>
            {
                new Option(0, "Classic"),
                new Option(1, "Pawn of the Dead")
            };
            var comboBoxModes = new ComboBox
            {
                ValueMember = "Id",
                DisplayMember = "Text",
                DataSource = gameModeOptions
            };
            comboBoxModes.SelectedIndexChanged += gameModeComboBox_OnSelectedIndexChanged;
            _rules = GameModePool.Get<StandardRules>();

            return comboBoxModes;
        }

        private ComboBox GeneratePlayerDropdown()
        {
            var playerOptions = new List<Option>
            {
                new Option(0, "White"),
                new Option(1, "Black")
            };
            var comboBoxPlayer = new ComboBox
            {
                DataSource = playerOptions,
                ValueMember = "Id",
                DisplayMember = "Text",
                Enabled = false
            };
            comboBoxPlayer.SelectedIndexChanged += playerComboBox_OnSelectedIndexChanged;

            return comboBoxPlayer;
        }

        private CheckBox GenerateOpponentCheckbox()
        {
            var opponentCheckbox = new CheckBox
            {
                AutoSize = true
            };
            opponentCheckbox.CheckedChanged += opponentCheckbox_OnCheckedChanged;
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

        private Button GenerateButton(string text, EventHandler clickHandler)
        {
            var button = new Button
            {
                Size = new Size(128, 64),
                Text = text,
                ForeColor = Color.White
            };
            button.Click += clickHandler;

            return button;
        }

        private Label GenerateLabel(string text, Color foreColor)
        {
            var label = new Label
            {
                Size = new Size(200, 20),
                Text = text,
                ForeColor = foreColor,
                AutoSize = true
            };

            return label;
        }

        private ConfigurableChessBoardControl GenerateChessBoardControl()
        {
            var board = _rules.GetStartingBoardState().GetBoard();
            var configurableBoard = new ConfigurableChessBoardControl(board.GetWidth(), board.GetHeight(), _rules);

            configurableBoard.UpdateBoard(board);
            configurableBoard.UserInputRequired += OnOptionPickRequired;

            return configurableBoard;
        }

        private void startButton_OnClick(object sender, EventArgs e)
        {
            ChessGame game;
            try
            {
                game = new GameBuilder()
                .WithGameMode(_rules)
                .WithBoard(_chessBoardSetup.GetBoard())
                .Create();
            }
            catch (InvalidBoardException ex)
            {
                DisplayError($"Invalid board: {ex.Message}");
                return;
            }

            var args = new GameStartEventArgs
            {
                Game = game,
                VersusAi = _checkBoxOpponent.Checked,
                HumanPlayer = _checkBoxOpponent.Checked ? (int)_comboBoxPlayer.SelectedValue : 0,
                TimeLimit = (int)_timeLimit.Value,
                Increment = (int)_increment.Value
            };

            OnGameStart(args);
        }

        private void opponentCheckbox_OnCheckedChanged(object sender, EventArgs e)
        {
            _comboBoxPlayer.Enabled = _checkBoxOpponent.Checked;
        }

        private void clearButton_OnClick(object sender, EventArgs e)
        {
            var board = _rules.GetStartingBoardState().GetBoard();
            var clearBoard = new Board(board.GetWidth(), board.GetHeight());
            _chessBoardSetup.UpdateBoard(clearBoard);
        }

        private void gameModeComboBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            switch (_comboBoxModes.SelectedIndex)
            {
                case 0:
                    _rules = GameModePool.Get<StandardRules>();
                    break;
                case 1:
                    _rules = GameModePool.Get<PawnOfTheDeadRules>();
                    break;
                default:
                    throw new Exception("Invalid game mode id supplied.");
            }
            _chessBoardSetup.SetRules(_rules);
        }

        private void playerComboBox_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            _chessBoardSetup.Flip();
        }

        private void OnGameStart(GameStartEventArgs e)
        {
            GameStart?.Invoke(this, e);
        }

        private void OnOptionPickRequired(object sender, MultipleOptionEventArgs e)
        {
            UserInputRequired?.Invoke(this, e);
        }

        private void DisplayError(string errorMessage)
        {
            _errorLabel.Text = errorMessage;
            _errorLabel.Location = new Point((Width - _errorLabel.Size.Width) / 2, _errorLabel.Location.Y);
        }
    }
}
