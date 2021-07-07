using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Game
{
    class Option
    {
        public int Id { get; }
        public string Name { get; }

        public Option(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
