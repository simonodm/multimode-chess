using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Modes;
using Chess.Game.Pieces;

namespace Chess.Game
{
    class Move
    {
        private object _optionLock = new object();

        public GamePiece Piece;
        public BoardSquare From;
        public BoardSquare To;
        public BoardState BoardBefore;
        public BoardState BoardAfter;
        public Move Previous;
        public bool IsUserInputRequired = false;
        public List<Option> Options { get; private set; }
        public Option SelectedOption;
        public string Notation
        {
            get
            {
                return _rules.GetMoveNotation(this);
            }
        }

        protected IGameRules _rules;

        public Move(IGameRules rules)
        {
            _rules = rules;
        }

        public void AddOption(string optionName)
        {
            lock(_optionLock)
            {
                if (!IsUserInputRequired)
                {
                    IsUserInputRequired = true;
                    Options = new List<Option>();
                }
                Options.Add(new Option(Options.Count, optionName));
            }           
        }

        public void SelectOption(int optionId)
        {
            lock(_optionLock)
            {
                IsUserInputRequired = false;
                SelectedOption = Options[optionId];
            }
        }

    }
}
