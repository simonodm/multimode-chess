using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class GameResult
    {
        private int _winner;
        public GameResult(int winner)
        {
            _winner = winner;
        }

        public int GetWinner()
        {
            return _winner;
        }
        public string GetWinnerString()
        {
            switch (_winner)
            {
                case 0:
                    return "White";
                case 1:
                    return "Black";
                default:
                    return _winner.ToString();
            }
        }
    }
}
