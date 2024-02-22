using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Pozadi : Obrazek
    {
        //public Image img;

        public Pozadi(Image img) : base (img) { 
            this.img = img;
            this.X = 0;
            this.Y = 0;
        }
    }
}
