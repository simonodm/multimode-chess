using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class Clock
    {
        private object _clockSwitchLock = new object();

        private int _limit = 600;
        private int _increment = 0;
        private int[] _remainingTimes;
        private int _currentPlayer = 0;
        private DateTime _currentClockStart;
        private DateTime _gameStart;
        

        public Clock(int playerCount, int limit = 600, int increment = 0)
        {
            _limit = limit;
            _increment = increment;
            _remainingTimes = new int[playerCount];
            for(int i = 0; i < playerCount; i++)
            {
                _remainingTimes[i] = limit;
            }
        }

        public void Start()
        {
            lock(_clockSwitchLock)
            {
                _gameStart = DateTime.Now;
                _currentClockStart = _gameStart;
                _currentPlayer = 0;
            }
        }

        public void Switch()
        {
            lock(_clockSwitchLock)
            {
                _remainingTimes[_currentPlayer] = GetRemainingTime(_currentPlayer) + _increment;
                _currentClockStart = DateTime.Now;
                _currentPlayer = (_currentPlayer + 1) % _remainingTimes.Length;
            }
        }

        public void Reset()
        {
            lock(_clockSwitchLock)
            {
                for (int i = 0; i < _remainingTimes.Length; i++)
                {
                    _remainingTimes[i] = _limit;
                }
            }
        }

        public int GetRemainingTime(int player)
        {
            lock(_clockSwitchLock)
            {
                if (player == _currentPlayer)
                {
                    TimeSpan currentMoveTime = DateTime.Now - _currentClockStart;
                    return Math.Max(0, _remainingTimes[_currentPlayer] - currentMoveTime.Seconds);
                }
                else
                {
                    return Math.Max(0, _remainingTimes[player]);
                }
            }
        }

    }
}
