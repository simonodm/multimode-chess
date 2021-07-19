using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class ThreatMap
    {
        private bool[,] _threatMap;
        private int _width;
        private int _height;

        public ThreatMap(int width, int height)
        {
            _threatMap = new bool[width, height];
            _width = width;
            _height = height;
        }

        public void ToggleThreat(int x, int y)
        {
            _threatMap[x, y] = !_threatMap[x, y];
        }

        public bool IsThreatened(int x, int y)
        {
            return _threatMap[x, y];
        }


    }
}
