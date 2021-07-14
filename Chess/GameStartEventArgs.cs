using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.Game;

namespace Chess
{
    class GameStartEventArgs : EventArgs
    {
        public ChessGame Game;
    }
}
