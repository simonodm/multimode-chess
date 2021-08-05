namespace ChessCore
{
    public abstract class GamePiece
    {
        protected int _value = 0;
        protected string _symbol = "";
        private int _player = 0;
        private int _moveCount = 0;
        private object _moveCountLock = new object();

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
            lock (_moveCountLock)
            {
                return _moveCount;
            }
        }
        public void SetMoveCount(int moveCount)
        {
            lock (_moveCountLock)
            {
                _moveCount = moveCount;
            }
        }
    }
}
