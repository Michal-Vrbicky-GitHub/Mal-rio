using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Obrazek : MapObject
    {
        public Image img;
        public Obrazek(Image img)
        {
            this.img = img;
        }
    }
}
