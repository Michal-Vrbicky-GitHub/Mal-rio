using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Malario.MapObjects
{
    internal class Enemy : Projectile
    {
        public new enum Typy
        {
            SWS,
            Tonk,
            Hitler,
            dying,
            mrkev
        }
        public new Typy typ;
        public int casStrelba;
        public new char smer;
        public int[] zarazky;
        //public bool 
        public int casHit;
        public int health;

        public Enemy(int pozXce, int pozYce, Typy vtip) : base(pozXce, pozYce, (Projectile.Typy)vtip)
        {
            this.X = pozXce;
            this.Y = pozYce;
            this.typ = /*(Projectile.Typy)*/vtip;// a tap
        }
        public Enemy(int pozXce, int pozYce, Typy type, int Xleva, int Xprava) : base(pozXce, pozYce, (Projectile.Typy)type)
        {
            this.X = pozXce;
            this.Y = pozYce;
            this.typ = type;
            zarazky = new int[]{Xleva, Xprava};
            smer = '→';
        }
    }
}
