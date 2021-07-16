using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    struct BoardScore
    {
        private double _score;
        public BoardScore(double score)
        {
            _score = score;
        }

        public double GetScore()
        {
            return _score;
        }

        public string ToString()
        {
            return _score.ToString();
        }
        
        public static bool operator >(BoardScore first, BoardScore second)
        {
            return first.GetScore() > second.GetScore();
        }

        public static bool operator <(BoardScore first, BoardScore second)
        {
            return first.GetScore() < second.GetScore();
        }

        public static bool operator >=(BoardScore first, BoardScore second)
        {
            return first.GetScore() >= second.GetScore();
        }

        public static bool operator <=(BoardScore first, BoardScore second)
        {
            return first.GetScore() <= second.GetScore();
        }

        public static bool operator ==(BoardScore first, BoardScore second)
        {
            return first.GetScore() == second.GetScore();
        }

        public static bool operator !=(BoardScore first, BoardScore second)
        {
            return first.GetScore() != second.GetScore();
        }
    
    }
}
