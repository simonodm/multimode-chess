﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    class MoveNormal : ClassicMove
    {
        public MoveNormal(IGameRules rules) : base(rules) { }

        public override StandardBoardState Process()
        {
            var board = BoardBefore.GetBoard()
                .Move(this);
            return new StandardBoardState(board, this);
        }
    }
}
