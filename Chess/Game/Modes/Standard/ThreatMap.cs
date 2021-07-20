using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class ThreatMap
    {
        private bool[,] _threatMapWhite;
        private bool[,] _threatMapBlack;
        private int _width;
        private int _height;

        public ThreatMap(int width, int height)
        {
            _threatMapWhite = new bool[width, height];
            _threatMapBlack = new bool[width, height];
            _width = width;
            _height = height;
        }

        public void ToggleThreat(int x, int y, int player)
        {
            if(player == 0)
            {
                _threatMapWhite[x, y] = !_threatMapWhite[x, y];
            }
            else
            {
                _threatMapBlack[x, y] = !_threatMapBlack[x, y];
            }
        }

        public bool IsThreatened(BoardSquare square, int byPlayer)
        {
            return IsThreatened(square.GetFile(), square.GetRank(), byPlayer);
        }

        public bool IsThreatened(int x, int y, int player)
        {
            if(player == 0)
            {
                return _threatMapWhite[x, y];
            }
            else
            {
                return _threatMapBlack[x, y];
            }
        }


    }
}
