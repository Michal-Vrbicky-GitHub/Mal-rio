using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Spike : Trap
    {
        public int CR;//Časová reference
        public int c;
        public bool fake;

        public Spike(int X, int Y) : base(X, Y) { }
    }
}
