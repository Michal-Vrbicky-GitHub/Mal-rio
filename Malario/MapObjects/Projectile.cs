using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Projectile : MapObject
    {
        public enum Typy
        {
            Rajče,
            Tokarev7_62_X_25mm,
            XM1147
        }
        public Typy typ;
        public double smer;

        public Projectile(int pozXce, int pozYce, Typy vtyp)
        {
            this.X = pozXce;
            this.Y = pozYce;
            this.typ = vtyp;
        }
    }
}
