using ChessCore.Game.Modes;
using System.Collections.Generic;

namespace ChessCore.Game
{
    public class Move
    {
        private object _optionLock = new object();
        private object _boardAfterLock = new object();

        public GamePiece Piece { get; init; }
        public BoardSquare From { get; init; }
        public BoardSquare To { get; init; }
        public BoardState BoardBefore { get; init; }
        public BoardState BoardAfter
        {
            get => _boardAfter;
            set
            {
                lock(_boardAfterLock)
                {
                    _boardAfter = value;
                }
            }
        }
        public bool IsUserInputRequired { get; private set; } = false;
        public List<Option> Options { get; private set; }
        public Option SelectedOption { get; private set; }
        public string Notation
        {
            get
            {
                return _rules.GetMoveNotation(this);
            }
        }

        protected IGameRules _rules;
        private BoardState _boardAfter;
        public Move(IGameRules rules)
        {
            _rules = rules;
        }

        public void AddOption(string optionName)
        {
            lock (_optionLock)
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
            lock (_optionLock)
            {
                IsUserInputRequired = false;
                SelectedOption = Options[optionId];
            }
        }

    }
}
