using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    static class IntExt
    {
        public static char ConvertToChessFile(ref this int file)
        {
            if(file > 25)
            {
                throw new Exception("File too large.");
            }
            return (char)(97 + file);
        }
    }
}
