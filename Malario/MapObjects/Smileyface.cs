using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Smileyface : MapObject
    {
        public int IDofPicture(int time)
        {
            int zb = time % 44;
            if(zb < 11)
                return 1;
            else if (zb < 22)
                return 2;
            else if (zb < 33)
                return 1;
            return 3;
        }
    }
}
