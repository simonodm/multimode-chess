using ChessCore.Exceptions;
using ChessCore.Modes;
using System.Collections.Generic;

namespace ChessCore
{
    /// <summary>
    /// Represents a single chess move.
    /// <remarks>
    /// A move might require user input to be processed properly. In that case, the IsUserInputRequired property is set to true, and an option from Options must be selected using the SelectOption() method
    /// before processing this move in the game.
    /// </remarks>
    /// </summary>
    public class Move
    {
        private readonly object _optionLock = new object();
        private readonly object _boardAfterLock = new object();

        /// <summary>
        /// The moving piece.
        /// </summary>
        public GamePiece Piece { get; init; }

        /// <summary>
        /// The starting square of the move.
        /// </summary>
        public BoardSquare From { get; init; }

        /// <summary>
        /// The target square of the move.
        /// </summary>
        public BoardSquare To { get; init; }

        /// <summary>
        /// The board state before this move.
        /// </summary>
        public BoardState BoardBefore { get; init; }

        /// <summary>
        /// The board state after this move if the move has already been processed.
        /// </summary>
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

        /// <summary>
        /// If this property is true, then an option from Options must be selected before processing this move. The option is selected by calling the SelectOption() method.
        /// </summary>
        public bool IsUserInputRequired { get; private set; }

        /// <summary>
        /// A list of available options if user input is required.
        /// </summary>
        public List<Option> Options { get; private set; }

        /// <summary>
        /// The selected option for this move. Only relevant if user input was required.
        /// </summary>
        public Option SelectedOption { get; private set; }

        /// <summary>
        /// The move's notation.
        /// </summary>
        public string Notation => Rules.GetMoveNotation(this);

        protected IGameRules Rules;
        private BoardState _boardAfter;
        private readonly Dictionary<int, int> _optionIdToIndexMap = new Dictionary<int, int>();

        public Move(IGameRules rules)
        {
            Rules = rules;
        }

        /// <summary>
        /// Adds an option to the list of available options. This sets IsUserInputRequired to true if it wasn't set to true already.
        /// </summary>
        /// <param name="option">The option to add</param>
        /// <exception cref="ChessCoreException">Thrown if a duplicate option id is detected</exception>
        internal void AddOption(Option option)
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

        /// <summary>
        /// Selects the given option.
        /// </summary>
        /// <param name="option">Option to select</param>
        /// <exception cref="ChessCoreException">Thrown if no user input is required or if option was not among the available options</exception>
        public void SelectOption(Option option)
        {
            if (!IsUserInputRequired)
            {
                throw new ChessCoreException("No user input required for this move.");
            }
            lock (_optionLock)
            {
                if (!_optionIdToIndexMap.ContainsKey(option.Id) || option.Id != Options[_optionIdToIndexMap[option.Id]].Id)
                {
                    throw new ChessCoreException("Unrecognized option selected.");
                }

                IsUserInputRequired = false;
                SelectedOption = option;
            }
        }

        /// <summary>
        /// Unselects the currently selected option
        /// <exception cref="ChessCoreException">Thrown if no option was selected</exception>
        /// </summary>
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
