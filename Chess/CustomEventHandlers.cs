using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    delegate void GameStartEventHandler(object sender, GameStartEventArgs e);
    delegate void MoveEventHandler(object sender, MoveEventArgs e);
    delegate void LegalMovesEventHandler(object sender, LegalMovesEventArgs e);
    delegate void BoardEventHandler(object sender, BoardEventArgs e);
    delegate void MultipleOptionEventHandler(object sender, MultipleOptionEventArgs e);
}
