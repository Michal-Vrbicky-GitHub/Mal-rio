using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Obdélník : MapObject
    {
        public int sirka;
        public int vyska;
        public Color barva ;

        public Obdélník(int sirka, int vyska, int[] umisteni, string barva)
        {
            this.sirka = sirka;
            this.vyska = vyska;
            this.X = umisteni[0];
            this.Y = umisteni[1];//šikula šroubovák
            int R = 111;
            int G = 252;
            int B = 000;
            int alfa=42;
            string odkladaci = "";
            for (int i = 0; i < 15; i++)
            {
                odkladaci += barva[i];
                if(i==2) {
                    R = int.Parse(odkladaci);
                }
                else if (i == 6){
                    G = int.Parse(odkladaci);
                }
                else if (i == 10){
                    B = int.Parse(odkladaci);
                }
                else if (i == 14){
                    alfa = int.Parse(odkladaci);
                }
                if(i==3 || i==7 || i==11){
                    odkladaci = "";
                }
            }
            this.barva = Color.FromArgb(alfa, R, G, B);
        }
    }
}
