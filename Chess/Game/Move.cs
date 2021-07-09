using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game.Pieces;

namespace Chess.Game
{
    class Move
    {
        public IGamePiece Piece;
        public BoardSquare From;
        public BoardSquare To;
        public BoardState BoardBefore;
        public BoardState BoardAfter;
        public bool IsUserInputRequired = false;
        public List<Option> Options { get; private set; }
        public Option SelectedOption;

        public Move(){}

        public void AddOption(string optionName)
        {
            if(!IsUserInputRequired)
            {
                IsUserInputRequired = true;
                Options = new List<Option>();
            }
            Options.Add(new Option(Options.Count, optionName));
        }

        public void SelectOption(int optionId)
        {
            IsUserInputRequired = false;
            SelectedOption = Options[optionId];
        }

    }
}
