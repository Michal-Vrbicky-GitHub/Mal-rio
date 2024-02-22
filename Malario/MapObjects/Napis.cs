using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Napis : MapObject
    {
        public Color  clr { get; set; }
        public string txt { get; set; }
        public int    sze { get; set; }

        public Napis(int V, int S, string txt, Color clr, int size)
        {
            this.clr = clr;
            this.txt = txt;
            sze = size;
            X=V; Y=S;
        }
    }
}
