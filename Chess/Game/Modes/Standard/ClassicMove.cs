using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game.Modes.Standard
{
    abstract class ClassicMove : Move
    {
        public new StandardBoardState BoardBefore;
        public new StandardBoardState BoardAfter;

        public ClassicMove(IGameRules rules) : base(rules) { }

        public abstract StandardBoardState Process();
    }
}
