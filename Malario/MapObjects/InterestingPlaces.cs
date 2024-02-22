using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class InterestingPlaces : MapObject
    {
        public enum veci
        {
            Vlajka,
            Heal,
            nezranitelnost
        }
        public veci co = veci.Vlajka;
        public bool sezrany = false;
        public Image[] imagy = null;

        public InterestingPlaces(int Xé, int Ý)
        {
            this.X = Xé;
            this.Y = Ý ;
        }
    }
}
