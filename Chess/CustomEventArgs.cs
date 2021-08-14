using ChessCore;
using System;
using System.Collections.Generic;

namespace ChessGUI
{
    class GameStartEventArgs : EventArgs
    {
        public ChessGame Game;
        public bool VersusAi;
        public int HumanPlayer;
        public int TimeLimit;
        public int Increment;
    }
    class LegalMovesEventArgs : EventArgs
    {
        public BoardSquare Square;
        public IEnumerable<Move> LegalMoves;
    }
    class MoveEventArgs : EventArgs
    {
        public Move Move;
    }
    class MultipleOptionEventArgs : EventArgs
    {
        public List<Option> Options;
        public Option PickedOption;
    }
}
