using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class ObdelnikBezKolize : Obdélník
    {
        public bool navrch;
        public ObdelnikBezKolize(int sirka, int vyska, int[] umisteni, string barva, bool navrch) : base(sirka, vyska,  umisteni, barva)//ajó
        {
            this.navrch = navrch;
        }
    }
}
