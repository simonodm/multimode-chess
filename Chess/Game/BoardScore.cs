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
        public BoardScore(int score)
        {
            _score = score;
        }

        public string ToString()
        {
            return _score.ToString();
        }
    
    }
}
