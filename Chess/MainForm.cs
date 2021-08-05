using Chess.Controls;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public partial class MainForm : Form
    {
        private GameSetupControl _gameSetupView;
        private GameControl _gameView;

        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _gameSetupView = GenerateSetupControl();
            Controls.Add(_gameSetupView);
        }

        private void OnGameStart(object sender, GameStartEventArgs e)
        {
            Controls.Remove(_gameSetupView);
            _gameSetupView.Dispose();
            _gameSetupView = null;

            _gameView = GenerateGameControl(e);

            Controls.Add(_gameView);
        }

        private void OnMultipleOptionInput(object sender, MultipleOptionEventArgs e)
        {
            using (var optionForm = new OptionPickerModalForm(e.Options))
            {
                optionForm.ShowDialog();
                if (optionForm.DialogResult == DialogResult.OK)
                {
                    e.PickedOption = optionForm.PickedOption;
                }
            }
        }

        private GameSetupControl GenerateSetupControl()
        {
            var gameSetupControl = new GameSetupControl
            {
                Location = new Point(0, 0),
                Size = new Size(ClientRectangle.Width, ClientRectangle.Height)
            };
            gameSetupControl.GameStart += OnGameStart;
            gameSetupControl.OptionPickRequired += OnMultipleOptionInput;

            return gameSetupControl;
        }

        private GameControl GenerateGameControl(GameStartEventArgs startArgs)
        {
            var gameControl = new GameControl(startArgs)
            {
                Location = new Point(0, 0),
                Size = new Size(ClientRectangle.Width, ClientRectangle.Height)
            };
            gameControl.OptionPickRequired += OnMultipleOptionInput;
            gameControl.GameCancelled += OnGameCancel;

            return gameControl;
        }

        private void OnGameCancel(object sender, EventArgs e)
        {
            Controls.Clear();
            _gameView.Dispose();
            _gameView = null;
            _gameSetupView = GenerateSetupControl();
            Controls.Add(_gameSetupView);
        }
    }
}
