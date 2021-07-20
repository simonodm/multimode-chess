using Chess.Game.Modes.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Pieces
{
    abstract class GamePiece
    {
        protected int _value = 0;
        protected string _symbol = "";
        private int _player = 0;
        private int _moveCount = 0;
        
        public GamePiece(int player)
        {
            _player = player;
        }

        public int GetValue()
        {
            return _value;
        }

        public string GetSymbol()
        {
            return _symbol;
        }

        public int GetPlayer()
        {
            return _player;
        }
        public void SetPlayer(int player)
        {
            _player = player;
        }
        public int GetMoveCount()
        {
            return _moveCount;
        }
        public void SetMoveCount(int moveCount)
        {
            _moveCount = moveCount;
        }

        protected bool IsLineBlocked(BoardState state, BoardSquare from, BoardSquare to)
        {
            bool pieceFound = false;
            int startFile = from.GetFile();
            int endFile = to.GetFile();
            int startRank = from.GetRank();
            int endRank = to.GetRank();
            int i = startFile;
            int j = startRank;

            while (i != endFile || j != endRank)
            {
                if (i != startFile || j != startRank)
                {
                    var blockingPiece = state.GetBoard().GetSquare(i, j).GetPiece();
                    if (blockingPiece != null && blockingPiece != this)
                    {
                        pieceFound = true;
                    }
                    if (pieceFound)
                    {
                        return true;
                    }
                }
                if (startFile < endFile && i < endFile) i++;
                if (startFile > endFile && i > endFile) i--;
                if (startRank < endRank && j < endRank) j++;
                if (startRank > endRank && j > endRank) j--;
            }

            return false;
        }

        protected BoardSquare? GetTargetSquare(BoardState state, BoardSquare from, (int, int) offset)
        {
            int newFile = from.GetFile() + offset.Item1;
            int newRank = from.GetRank() + offset.Item2;
            if(newFile >= 0 && newFile < state.GetBoard().GetWidth() && newRank >= 0 && newRank < state.GetBoard().GetHeight())
            {
                return state.GetBoard().GetSquare(newFile, newRank);
            }
            return null;
        }

        public abstract List<BoardSquare> GetPossibleMoves(BoardState state, BoardSquare from);

        public abstract (int, int)[] GetPossibleMoveOffsets();
    }
}
