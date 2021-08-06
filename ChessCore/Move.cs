using ChessCore.Exceptions;
using ChessCore.Modes;
using System.Collections.Generic;

namespace ChessCore
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
                lock (_boardAfterLock)
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
        private Dictionary<int, int> _optionIdToIndexMap = new Dictionary<int, int>();

        public Move(IGameRules rules)
        {
            _rules = rules;
        }

        public void AddOption(Option option)
        {
            lock (_optionLock)
            {
                if (!IsUserInputRequired)
                {
                    IsUserInputRequired = true;
                    Options = new List<Option>();
                }
                if (_optionIdToIndexMap.ContainsKey(option.Id))
                {
                    throw new ChessCoreException("Duplicate option id added to Move.Options");
                }
                _optionIdToIndexMap[option.Id] = Options.Count;
                Options.Add(option);
            }
        }

        public void SelectOption(Option option)
        {
            if (option.Id != Options[_optionIdToIndexMap[option.Id]].Id)
            {
                throw new ChessCoreException("Unrecognized option selected.");
            }
            lock (_optionLock)
            {
                IsUserInputRequired = false;
                SelectedOption = option;
            }
        }

        public void UnselectOption()
        {
            if (SelectedOption == null)
            {
                throw new ChessCoreException("No option to unselect.");
            }
            SelectedOption = null;
            IsUserInputRequired = true;
        }

    }
}
