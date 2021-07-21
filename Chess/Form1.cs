using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Game;

namespace Chess
{
    public partial class Form1 : Form
    {
        private ChessGame _game;
        private GameSetupControl _gameSetupView;
        private GameControl _gameView;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _gameSetupView = new GameSetupControl
            {
                Size = new Size(ClientRectangle.Width, ClientRectangle.Height),
                Location = new Point(0, 0)
            };
            _gameSetupView.GameStart += OnGameStart;
            _gameSetupView.OptionPickRequired += OnMultipleOptionInput;
            Controls.Add(_gameSetupView);
        }

        private void OnGameStart(object sender, GameStartEventArgs e)
        {
            Controls.Clear();
            _gameView = GenerateGameControl(e.Game);
            Controls.Add(_gameView);           
        }

        private void OnMultipleOptionInput(object sender, MultipleOptionEventArgs e)
        {
            using (var optionForm = new OptionPickerModalForm(e.Options))
            {
                optionForm.ShowDialog();
                if(optionForm.DialogResult == DialogResult.OK)
                {
                    e.PickedOption = optionForm.PickedOption;
                }
            }
        }

        private GameControl GenerateGameControl(ChessGame game)
        {
            var gameControl = new GameControl(game)
            {
                Size = new Size(ClientRectangle.Width, ClientRectangle.Height),
                Location = new Point(0, 0)
            };
            gameControl.OptionPickRequired += OnMultipleOptionInput;
            return gameControl;
        }
    }
}
