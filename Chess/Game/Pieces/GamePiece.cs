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
        public abstract (int, int)[] GetPossibleMoveOffsets();
    }
}
