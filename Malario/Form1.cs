using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
using System.Threading;
//using System.Threading.Tasks;
//using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
//using System.Runtime.InteropServices;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using Malario.MapObjects;
//using System.Net.Sockets;

namespace Malario
{
    public partial class Form1 : Form
    {
        int[] velikostObrazovky;
        int pruhlednostCasPozice = -1;//tato proměnná je velice všestranná a praktická
        int timer1Interval = 8;
        int skok;
        int poradi = 0;//05 
        readonly Color cerna = Color.FromArgb(42, 42, 42, 42);
        Color bila = Color.FromArgb(213, 213, 213, 213);
        Color belejsiBila = Color.FromArgb(255, 255, 255, 255);
        readonly Brush br = new SolidBrush(Color.FromArgb(111, 0, 0, 0));
        //Pen   pe = new Pen(bila);
        public static string solutionPath = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static Image[] obry = new Image[13];
        DataHandl dh = new DataHandl(solutionPath);
        int cisloHry = -8;//Gameslot
        int id_vyberu = 0;
        List<string> savy = new List<string>();
        public static int obtiznost;
        int pocetMap = 5;
        int pocetDokoncenychMap;
        string[] obtiznosti = { "ízy", "normal", "Ultra-Violence" };
        public static double[] pomerovyKoeficient;//pro úpravu velikostí a souřadnic v závislosti na rozměrech obrazovky
        //List<List<string>> veciNaMape = new List<List<string>>();
        int poziceX; int Xreferencni;
        int poziceY; int Yreferencni;
        //private HashSet<Keys> pressedKeys = new HashSet<Keys>();
        double speeed = 1, KoeficientZmenyY = 1;
        int zivoty;//pro ízy 3//
        int pokus; //Neukládam do savu, jenom počet pokusů souvisle za sebou, aby bylo co vykreslit
        Image[] malarioveFigury, DOOT, strely;
        bool[] muzuSeHnout = new bool[4];//vlevo, vpravo, nahoru, dolu
        bool muzuSeHnout3minule;
        int casRef;//int rychlostY;
        char naposledySemSel;
        bool nepadat, nahoru;
        bool atack, use;
        char atkForm;
        int[] dashe = new int[2];
        bool semNaZebriku, bylSemNaZebriku;
        sbyte nahorudolu;
        bool flymaggot, iddqd;
        List<MapObjects.Projectile> projectilesy = new List<MapObjects.Projectile>();
        //List<int[]> povrchProEmaky = new List<int[]>();
        bool poutokuSTOP;//42
        List<int[]> vysunutyBodaky = new List<int[]>();
        bool win, dead, begin;
        Image[] invulnerability_sphere = null;
        double nominalSpeed = 1.33;
        //Stopwatch Cas = new Stopwatch();
        ///*u*/long CasRef;
        bool blokKresleniHry;//Pro další provádění programu není dostatek paměti. AAAAAAAAAAAAAAAAA.
        int intervalHra = 36;//zvětšit, když se seká, ale všechno je vázaný na počet cyklů a ne na timeElapsed /*77*/
        bool testMode;
        int Yminule;
        Random rnd = new Random();
        System.Windows.Forms.Timer xP = new System.Windows.Forms.Timer { Interval = 42 };
        System.Windows.Forms.Timer xM = new System.Windows.Forms.Timer { Interval = 42 };
        System.Windows.Forms.Timer yPohyb = new System.Windows.Forms.Timer { Interval = 03 };
        System.Windows.Forms.Timer Mstrelba = new System.Windows.Forms.Timer { Interval = 200 };


        public Form1()
        {
            //testMode = true;//nesmrtelnost, bez kolize, větší rychlost
            InitializeComponent();
            pictureBox1.Paint += PictureBox1_Paint_Uvod;
            Uvod();
            this.Focus();
            KeyPreview = true;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }


        private void Uvod()
        {
            string[] nameOfImage = { "Transmission electron micrograph of a SARS-CoV-2 coronavirus covered in spike proteins.png",
                "Plasmodium_falciparum.PNG", "The_malaria-infected_red_blood_cell.PNG",
                "Clipboard01s_upscayl_4x_realesrgan-x4plus-animeAAA.Png", "Malário.PNG",
                "image-removebg-preview (šipky).png", "image-removebg-preview (entr).png", "image-removebg-preview (pr).png",
                "EASY.PNG", "NORMAL.PNG", "HARD.PNG",};
            for (int i = 0; i < nameOfImage.Length; i++)
            {
                obry[i] = Image.FromFile(solutionPath + "\\Pictures\\" + nameOfImage[i]);
            }
            pictureBox1.BackColor = Color.FromArgb(255, 42, 42, 42);
            pictureBox2.SendToBack();
            velikostObrazovky = VelikostObrazovky();
            timer1.Interval = (int)timer1Interval;
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Start();
            double pomer = 536 / 505.0;
            obry[1] = new Bitmap(obry[1], (int)(obry[1].Width * (2 / 3d)), (int)(obry[1].Height * (2 / 3d)));//resize 
            obry[2] = new Bitmap(obry[2], (int)(obry[2].Width * (1 / 2f)), (int)(obry[2].Height * (1 / 2f)));
            obry[3] = new Bitmap(obry[3], 545, 515);//Některý prostě moc moc chtějí blbnout
            obry[4] = new Bitmap(obry[4], (int)(obry[4].Width * pomer), (int)(obry[4].Height * pomer));
            savy = dh.NacteniSaveSlotu();
            pomer = 0.5;
            obry[6] = new Bitmap(obry[6], (int)(obry[6].Width * pomer), (int)(obry[6].Height * pomer));
            pomer = 1.44;
            for (int i = 8; i < 11; i++)
            {
                obry[i] = new Bitmap(obry[i], (int)(obry[i].Width * pomer), (int)(obry[i].Height * pomer) + 28);
            }
            speeed = nominalSpeed;
        }

        private void PictureBox1_Paint_Uvod(object sender, PaintEventArgs e)
        {
            if (pruhlednostCasPozice == -1)
            {
                this.Location = new Point((velikostObrazovky[0] - this.Width) / 2, (velikostObrazovky[1] - this.Height) / 2);
                pruhlednostCasPozice = 0; //poradi = 4; skok = 1;                                                 //              tadyyyyyyyyyyyyyy
                return;
            }
            if (pruhlednostCasPozice / 16 > 255)
            {
                pruhlednostCasPozice = 0;
                Thread.Sleep(888);
                poradi++;
                if (poradi >= 3)
                {
                    skok = 1;
                }
            }
            int alpha = 0;
            if (poradi == 6)
            {
                alpha = -42;
            }
            int jezisikriste = alpha * (id_vyberu * 2 - 1);
            Brush br = new SolidBrush(Color.FromArgb(pruhlednostCasPozice / 16, 0, 0, 0));
            Font font1 = new Font("Tahoma", 66, FontStyle.Bold);
            Font font2 = new Font("Tahoma", 32, FontStyle.Bold);
            SolidBrush brush = new SolidBrush(Color.FromArgb(255 + alpha, 231, 231, 231));
            SolidBrush brum = new SolidBrush(Color.FromArgb(255 + alpha, 123 - jezisikriste, 123 - jezisikriste, 123 - jezisikriste));
            switch (poradi)
            {
                case 0:
                    e.Graphics.DrawImage(obry[0], (Width - obry[0].Width) / 2, 42);
                    //e.Graphics.DrawString("Covid Corps", new Font("Arial", 42, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 111, 11, 11)), Width-400, 380);
                    //e.Graphics.DrawString(napis, font, brush, x, 380);
                    Napis(e, "Covid Corps", new Font("Arial", 42, FontStyle.Bold), new SolidBrush(Color.FromArgb(255, 111, 11, 11)), 380);
                    //e.Graphics.DrawString(cas.ToString()+" "+timer1Interval, new Font("Arial", 42), new SolidBrush(Color.FromArgb(255, 255, 0, 0)), 0, 70);
                    break;
                case 1:
                    e.Graphics.DrawImage(obry[1], (Width - obry[1].Width) / 2 - 66, 42);
                    e.Graphics.DrawImage(obry[2], (Width - obry[2].Width) / 2 + 66, 159);
                    Font font = new Font("Courier New", 55, FontStyle.Bold);
                    brush = new SolidBrush(Color.FromArgb(255, 111, 111, 111));
                    Napis(e, "Zimnička", font, brush, 358);
                    Napis(e, "tropická", font, brush, 424);
                    break;
                case 2:
                    e.Graphics.DrawImage(obry[3], 0, 0);
                    brush = new SolidBrush(Color.FromArgb(255, 11, 222, 123));
                    string covidCorps = "Klub odpůrců repelentů";
                    font = new Font("Arial", 42, FontStyle.Regular);
                    int[] XY = { 198, 368 };
                    for (int i = 1; i < 3; i++)
                    {
                        Matrix originalTransform = e.Graphics.Transform.Clone();
                        e.Graphics.TranslateTransform(XY[0], XY[1]);
                        e.Graphics.RotateTransform(11 - i * 2);
                        e.Graphics.DrawString(covidCorps, font, brush, 0, 0);
                        e.Graphics.Transform = originalTransform;
                        if (i == 2) { break; }
                        covidCorps = "repelentů";
                        XY = new int[] { 202, 424 };
                    }
                    break;
                case 3:
                    font = new Font("Segoe UI", 100, FontStyle.Bold);//uvádějí
                    brush = new SolidBrush(Color.FromArgb(242, 6, 6, 6));
                    SizeF textSize = e.Graphics.MeasureString("AaA", font);
                    Napis(e, "Uvádějí", font, brush, (int)(Height - textSize.Height) / 2);
                    break;
                case 4:
                    {
                        int X = -160 + pruhlednostCasPozice;
                        if (X > 0) { X = 0; }
                        int Xx = -99 + (int)(pruhlednostCasPozice * 3.2);
                        if (Xx > 404) { Xx = 404; }
                        e.Graphics.DrawImage(obry[4], 0, X);
                        font = new Font("Verdana", 77, FontStyle.Bold);//MMM
                        brush = new SolidBrush(Color.FromArgb(255, 250, 111, 11));
                        Napis(e, "Malário", font, brush, Xx);
                        //e.Graphics.DrawString(timer1.Interval.ToString()+" "+timer1Interval+ " "+pruhlednostCasPozice, new Font("Arial", 42), new SolidBrush(Color.FromArgb(255, 255, 0, 0)), 0, 70);
                        if (pruhlednostCasPozice == 175)
                        {
                            poradi++;
                        }
                    }
                    break;
                case 5:
                    {
                        //timer1.Stop();
                        //cisloHry = VyberSlotu(e);
                        if (pruhlednostCasPozice != -2)
                        {
                            pruhlednostCasPozice = -2;
                            this.KeyDown += new KeyEventHandler(Form1_KeyDown_vyber);
                        }
                        //var g = pictureBox1.CreateGraphics();
                        Font font3 = new Font("Tahoma", 25, FontStyle.Regular);
                        Pen pe = new Pen(bila);
                        Pen ep = new Pen(belejsiBila);
                        pe.Width = /*ep.Width =*/ 5;
                        ep.Width = 8;
                        int widthSlotu = (int)(Width * (1.0 / 3.0));
                        int heightSlotu = 101;
                        int mezera = 27;
                        int x1 = (Width - 2 * widthSlotu - mezera) / 2;
                        int y1 = 160;
                        int[] X = { x1, x1 + widthSlotu + mezera };
                        int[] Y = { y1, y1 + heightSlotu + mezera };
                        Pen[] pens = { pe, pe, pe, pe };
                        pens[id_vyberu] = ep;//*
                        SolidBrush[] sldbrshs = { brum, brum, brum, brum };//Brum, brum brum, zatopíme medvědům
                        sldbrshs[id_vyberu] = brush;//*/
                        string obtiznost = "CHYBAAAAAAAAAAA";

                        Napis(e, "Vyber slot", font1, brum, 16);
                        for (int i = 0; i < 4; i++)
                        {
                            e.Graphics.DrawRectangle(pens[i], X[i % 2], Y[i / 2], widthSlotu, heightSlotu);
                            if (savy[i][0] != '0')
                            {
                                switch (savy[i][0])
                                {
                                    case '1':
                                        obtiznost = "Lehká";
                                        break;
                                    case '2':
                                        obtiznost = "Normal";
                                        break;
                                    case '3':
                                        obtiznost = "Těžká";
                                        break;
                                }
                                string map;
                                if (savy[i][1] == '0')
                                {
                                    map = "Tutorial";
                                }
                                else if (savy[i][1] == pocetMap.ToString().ToCharArray()[0])
                                {//No, anebo sem moh nechat převýst savy[i][1] na int, ale to by nebylo tak hezký.
                                    map = "Hotovo";
                                }
                                else
                                {
                                    map = "Mapa " + savy[i][1];
                                }
                                e.Graphics.DrawString(obtiznost, font2, sldbrshs[i], X[i % 2] + 4, Y[i / 2] + 3);
                                e.Graphics.DrawString(map, font2, sldbrshs[i], X[i % 2] + 4, Y[i / 2] + 3 + 42);
                            }
                            else
                            {
                                e.Graphics.DrawString("Prázdný", font2, sldbrshs[i], X[i % 2] - 5, Y[i / 2] + heightSlotu / 4);
                            }
                            e.Graphics.DrawImage(obry[5], 36, 400); e.Graphics.DrawString("Výběr", font3, brum, 18 + 16 + 11, 470);
                            e.Graphics.DrawImage(obry[6], 200, 400); e.Graphics.DrawString("Potvrdit", font3, brum, 205, 470);
                            e.Graphics.DrawImage(obry[7], 284, 370); e.Graphics.DrawString("Vymazat", font3, brum, 380, 469);
                        }/*e.Graphics.DrawRectangle(pens[3], x2, y2, widthSlotu, heightSlotu);*/
                        /*e.Graphics.DrawString(id_vyberu + 1 + "", new Font("Arial", 44), new SolidBrush(Color.FromArgb(242, 222, 222, 222)), 251, 242);*/
                        //e.Graphics.DrawString(id_vyberu+1 + "", new Font("Arial", 42), new SolidBrush(Color.FromArgb(200, 0, 0, 0)), 252, 242);//*/
                    }
                    break;
                case 6:
                    if (obtiznost == 0)//založení nové hry
                    {
                        string[] obtiznosti = {
                            "Enemáci střílejí jenom rovně do stran, moc se nesnažej a jsou docela blbý. Vem si těžší, bude ti jich líto.",
                            "Tohle je trapný, málo pastí, nepřátelé neumí střílet, i když se snaží. Jediná správná obtížnost je UV.",
                            "Agresivní a rychlí nepřátelé ve větším počtu, velice nefér. Tohle je moc těžký, bude lepčí to hrát na ízy." };
                        Font font3 = new Font("Tahoma", 25, FontStyle.Bold);
                        Font fontik = new Font("Helvetica", 26, FontStyle.Bold);
                        SolidBrush[] sldbrshs = Enumerable.Repeat(brum, 3).ToArray();//
                        sldbrshs[id_vyberu] = new SolidBrush(Color.FromArgb(222, 11, 111, 252));
                        brush = new SolidBrush(Color.FromArgb(255, 11, 111, 222)); ; ; ; ; ; ; ; ; ; ; ;//zajímavý
                        string radka = "";
                        string[] slova = obtiznosti[id_vyberu].Split(' ');
                        SizeF velikostRadky;
                        int radkaCislo = 0;
                        bool prvniNaRadce = true;
                        int y = 1;

                        e.Graphics.DrawImage(obry[8 + id_vyberu], 0, 0);
                        Napis(e, "Nová hra", font1, brum, -8);
                        Napis(e, "Výběr obtížnosti:", font3, brum, 82);
                        for (int i = 0; i < 3; i++)
                        {
                            Napis(e, this.obtiznosti[i], new Font("Helvetica", 42, FontStyle.Bold), sldbrshs[i], 107 + i * 50);
                        }
                        e.Graphics.DrawString(this.obtiznosti[id_vyberu] + ':', fontik, brush, 8, 270);
                        for (int i = 0; i < slova.Length; i++)
                        {
                            if (i + 1 == slova.Length)
                                y = 0;
                            if (prvniNaRadce)
                            {
                                radka += slova[i];
                                prvniNaRadce = false;
                            }
                            else
                                radka += " " + slova[i];
                            if ((velikostRadky = e.Graphics.MeasureString(radka + slova[i + y] + ' ' + ':' + '+' + '7', fontik)).Width >= this.Width || i + 1 == slova.Length)
                            {
                                Napis(e, radka, fontik, brush, 313 + radkaCislo * (int)velikostRadky.Height);
                                radkaCislo++;
                                radka = "";
                                prvniNaRadce = true;
                            }
                        }
                    }
                    break;
                case 7:
                    int yo = 253;
                    SolidBrush[] due = Enumerable.Repeat(brum, 2).ToArray();//
                    due[id_vyberu] = new SolidBrush(Color.FromArgb(225, 222, 111, 11));
                    Font f1 = new Font("Helvetica", 84, FontStyle.Bold);
                    Font f2 = new Font("Helvetica", 42, FontStyle.Bold);
                    if (id_vyberu == 1)
                    {
                        font1 = f1;
                        f1 = f2;
                        f2 = font1;
                    }
                    SizeF Ne = e.Graphics.MeasureString("Ne", f1);
                    SizeF Ano = e.Graphics.MeasureString("Ano", f2);
                    Napis(e, "Vymazat slot " + (obtiznost + 1) + '?', font2, brush, 123);
                    e.Graphics.DrawString("Ne", f1, due[0], 180 - (Ne.Width / 2), yo - (Ne.Height / 2), StringFormat.GenericTypographic);
                    e.Graphics.DrawString("Ano", f2, due[1], 363 - (Ano.Width / 2), yo - (Ano.Height / 2), StringFormat.GenericTypographic);
                    break;

            }
            if (poradi < 4)
            {
                e.Graphics.FillRectangle(br, 0, 0, Width, Height);//stmívání 
                skok = (int)(timer1Interval * Math.Exp(((double)pruhlednostCasPozice / (255 * 16)) * Math.Log(88 / timer1Interval)));//timer1.Interval = timerInterval + (int)(80 * progress);
            }
            else
            {
                //timer1.Stop();
                timer1.Interval++;
            }
            if (pruhlednostCasPozice != -2)
            {
                pruhlednostCasPozice += skok;
            }
        }

        int[] VelikostObrazovky()
        {
            int X = -1;
            string delkaStrany = "";
            string infoOObrazovce = Screen.PrimaryScreen.Bounds.ToString();//{X=0,Y=0,Width=800,Height=600}
            byte jestliUzCtuRozmer2 = 0;
            foreach (char to in infoOObrazovce)
            {
                if (to == 'h')
                {
                    jestliUzCtuRozmer2 = 1;
                }
                else if (jestliUzCtuRozmer2 == 1 && to == '=')
                {
                    jestliUzCtuRozmer2 = 2;
                }
                else if (jestliUzCtuRozmer2 == 2 && to != ',' && to != '}')
                {
                    delkaStrany += to;
                }
                else if (jestliUzCtuRozmer2 == 2 && to == ',' || jestliUzCtuRozmer2 == 2 && to == '}')
                {
                    if (X == -1)
                    {
                        X = int.Parse(delkaStrany);
                        delkaStrany = "";
                        jestliUzCtuRozmer2 = 0;
                    }
                    else
                    {
                        return new int[] { X, int.Parse(delkaStrany) };
                    }
                }
            }
            return new int[] { -1, -1, -1 };
        }


        void Napis(PaintEventArgs e, string napis, Font font, SolidBrush bru, int vyska)
        {
            SizeF textSize = e.Graphics.MeasureString(napis, font);
            float x = (Width - textSize.Width) / 2;
            e.Graphics.DrawString(napis, font, bru, x, vyska);
        }


        private void Form1_KeyDown_vyber(object sender, KeyEventArgs e)
        {
            if (cisloHry == -8)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if (id_vyberu % 2 == 1)
                            id_vyberu--;
                        break;
                    case Keys.Up:
                        if (id_vyberu >= 2)
                            id_vyberu -= 2;
                        break;
                    case Keys.Right:
                        if (id_vyberu % 2 == 0)
                            id_vyberu++;
                        break;
                    case Keys.Down:
                        if (id_vyberu < 2)
                            id_vyberu += 2;
                        break;
                    case Keys.Enter:
                        cisloHry = id_vyberu;
                        obtiznost = int.Parse(savy[id_vyberu][0].ToString());
                        pocetDokoncenychMap = int.Parse(savy[id_vyberu][1] + "");
                        if (obtiznost == 0)
                        {
                            poradi++;
                            id_vyberu = 0;
                        }
                        else
                        {
                            ZacniUzKonecneHru();
                        }
                        break;
                    case Keys.Delete:
                        if (savy[id_vyberu][0] != '0')
                        {
                            obtiznost = id_vyberu;
                            id_vyberu = 0;
                            poradi += 2;
                            cisloHry = 64;
                        }
                        break;
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Left:
                        if (id_vyberu != 0)
                            id_vyberu--;
                        break;
                    case Keys.Down:
                    case Keys.Right:
                        if (id_vyberu < 2 && cisloHry != 64 || id_vyberu < 1 && cisloHry == 64)
                            id_vyberu++;
                        break;
                    case Keys.Enter:
                        if (cisloHry == 64)
                        {
                            if (id_vyberu == 1)
                            {
                                dh.Save(obtiznost + "00");
                                savy = dh.NacteniSaveSlotu();
                            }
                            poradi = 5;
                            cisloHry = -8;
                            id_vyberu = obtiznost;
                        }
                        else
                        {
                            obtiznost = id_vyberu + 1;
                            ZacniUzKonecneHru();
                            dh.Save(cisloHry + obtiznost.ToString() + '0');
                        }
                        break;
                    case Keys.Escape:
                        id_vyberu = 0;
                        poradi--;
                        cisloHry = -8;
                        if (cisloHry == 64) { poradi--; }
                        break;
                }
            }
            //e.Handled = true;
        }


        void ZpetDoMenu(bool odemknoutDalsiMapu)
        {
            if (odemknoutDalsiMapu)
                dh.Save(cisloHry + obtiznost + "" + (pocetDokoncenychMap + 1));
            this.pictureBox1.Paint -= PbPaintHra;
            KeyDown -= new KeyEventHandler(KlavesyHra);
            KeyUp -= new KeyEventHandler(KlavesyHraAleJenomDvaStopy);//KlavesyHraAleSMinusama
            KeyDown += new KeyEventHandler(OdchytKlavesProMenu);

            ObrazkovyPrehazovac();
            pruhlednostCasPozice = 0;
            poradi = 0;
            id_vyberu = 0;
            this.pictureBox1.Paint -= PbPaintHra;
            pictureBox1.Paint += PbPainMenu;
            nahoru = false;
        }

        void ZacniUzKonecneHru()
        {
            timer1.Enabled = false;
            ObrazkovyPrehazovac();
            var g = pictureBox1.CreateGraphics();/*
            pictureBox1.Paint -= (sender, e) => { };
            timer1.Tick       -= (sender, e) => { };
            this.KeyDown      -= (sender, e) => { };*///AAAAAAAAAAAAAAAAAA
            pictureBox1.Paint -= PictureBox1_Paint_Uvod;
            timer1.Tick -= new System.EventHandler(timer1_Tick);
            this.KeyDown -= Form1_KeyDown_vyber;
            this.WindowState = FormWindowState.Normal;//iiFS
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            pictureBox1.Width = this.Bounds.Width;
            pictureBox1.Height = this.Bounds.Height;
            pictureBox1.BackColor = Color.DimGray;
            ZpetDoMenu(false);
            timer1.Tick += new System.EventHandler(timer1_Tick);
            timer1.Enabled = true;
            pomerovyKoeficient = new double[] { velikostObrazovky[0] / 1536.0, velikostObrazovky[1] / 864.0 };//odzkoušeno na moji voubrazouce né
            xM.Tick += new EventHandler(Xmius);
            xP.Tick += new EventHandler(Xplus);
            yPohyb.Tick += new EventHandler(Ykalk);
            malarioveFigury = dh.NacteniObruDoMovementSchemyATakyDootaAJesteProjektilu("Malario");
            timer1.Interval = intervalHra;//TADY SE NASTAVUJE INTERVAL PRO CHOD HRY
            Cursor.Hide();
            strely = dh.NacteniObruDoMovementSchemyATakyDootaAJesteProjektilu("Projektily");
            strely[3] = new Bitmap(strely[3], new Size(strely[3].Width / 4, strely[3].Height / 4));//další zapomenutej resize
            Mstrelba.Tick += new System.EventHandler(Pepeska);
            Mstrelba.Interval = 42 + obtiznost*5;
        }

        void ObrazkovyPrehazovac()
        {
            obry = new Image[2];
            Image img = null;
            double pomer;
            string[] namesOfImages = { "Zámek....pNg", "Malario_upscayl_16x_ultrasharp_33%_DigiArtText+⧸.PNG.png", "Plasmodium_lifecycle_PHIL_3405_lores.jpg" };//obry[0] = new Bitmap(Image.FromFile(piscturesPath + namesOfImages[1]), (int)(obry[5 + i].Width * pomer), (int)(obry[5 + i].Height * pomer));
            string piscturesPath = solutionPath + "\\Pictures\\";//AAAAA webp pánovi necutná, ale musí říct že je mimo paměť
            int kdyzNeniDokonceno0KdyzJeDokonceno1 = 0;
            for (int i = 0; i < namesOfImages.Length - 1; i++)
            {
                if (pocetDokoncenychMap == pocetMap && i == 1)
                {
                    kdyzNeniDokonceno0KdyzJeDokonceno1 = 1;
                    i++;
                    belejsiBila = Color.FromArgb(202, 69, 69, 69);//na bilym podkladu
                    bila = Color.FromArgb(255, 159, 159, 159);
                }
                img = Image.FromFile(piscturesPath + namesOfImages[i]);
                if (i == 0)
                    pomer = 0.42 * velikostObrazovky[0] / 1536.0 * 70.1 / 66.6;
                else
                    pomer = velikostObrazovky[1] / (double)img.Height;
                obry[i - kdyzNeniDokonceno0KdyzJeDokonceno1] = new Bitmap(img, (int)(img.Width * pomer), (int)(img.Height * pomer));
            }
        }

        private void OdchytKlavesProMenu(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (id_vyberu != 0)
                        id_vyberu--;
                    else
                        if (pocetDokoncenychMap != 0)
                        id_vyberu = pocetDokoncenychMap;
                    if (id_vyberu == 5)
                        id_vyberu--;
                    break;
                case Keys.Right:
                    if (id_vyberu < pocetDokoncenychMap)
                        id_vyberu++;
                    else
                        id_vyberu = 0;
                    if (id_vyberu == 5)
                        id_vyberu = 0;
                    break;
                case Keys.Down:
                    if (pruhlednostCasPozice != -1)
                        pruhlednostCasPozice--;
                    /*if (id_vyberu < Math.Ceiling((pocetMap/2.0)))
                        id_vyberu = 0;
                    else
                        id_vyberu = 1;*/
                    break;
                case Keys.Up:
                    if (pruhlednostCasPozice == -1)
                        pruhlednostCasPozice++;
                    /*if (id_vyberu == 1)
                        id_vyberu = pocetMap-1;*/
                    break;
                case Keys.Enter:
                    if (pruhlednostCasPozice == -1)
                        this.Close();
                    else
                    {
                        this.pictureBox1.Paint -= PbPainMenu;
                        KeyDown -= new KeyEventHandler(OdchytKlavesProMenu);
                        dh.uzMuzes = false;
                        dh.NacteniMapy(id_vyberu);
                        poziceY = dh.pocatecniSouradnice[1];
                        poziceX = dh.pocatecniSouradnice[0];
                        this.pictureBox1.Paint += PbPaintHra;
                        KeyDown += new KeyEventHandler(KlavesyHra);
                        KeyUp += new KeyEventHandler(KlavesyHraAleJenomDvaStopy);
                        muzuSeHnout = Enumerable.Repeat(true, 4).ToArray();
                        pruhlednostCasPozice = 0;
                        // *
                        //Xreferencni = poziceX;
                        //xP.Start();
                        // *
                        dashe[0] = id_vyberu + 1;
                        pokus = 0;
                        zivoty = 4 - obtiznost + id_vyberu;
                        win = false;
                        timer1Interval = -6;
                        nahoru = false;
                        muzuSeHnout3minule = false;
                        begin = true;
                        dead = false;
                        blokKresleniHry = false;
                        //Cas.Start();
                        speeed = nominalSpeed;
                    }
                    break;
            }
            e.Handled = true;
        }

        private void PbPainMenu(object sender, PaintEventArgs e)
        {
            //var g = e.Graphics();
            string ksa = "Odejít"; SolidBrush solitbruch = new SolidBrush(belejsiBila); Font front = new Font("Helvetica", (int)(64 * pomerovyKoeficient[0]), FontStyle.Bold);
            if (pocetDokoncenychMap == pocetMap)
                ksa = "A to je vše, přátelé!";
            int strana = (int)((66 + 3 + 2) * pomerovyKoeficient[0]);
            int mezera = (int)(32 * pomerovyKoeficient[0]);
            int x = (Width - (pocetMap + 2) * strana - (pocetMap - 1) * mezera) / 2;
            int y = (int)(500 * pomerovyKoeficient[1]);                                                //Screen.PrimaryScreen.Bounds, velikostObrazovky, this.Width AAAAAAA
            int y2 = (int)(y + 55 * pomerovyKoeficient[1]);
            int[] xy = { (Width - (int)e.Graphics.MeasureString(ksa, front).Width) / 2/*/-42-64/*/, y + 100 - 20 };
            e.Graphics.DrawImage(obry[1], (velikostObrazovky[0] - obry[1].Width) / 2, 0);
            Pen[] peni = Enumerable.Repeat(new Pen(bila, 6), pocetMap).ToArray();
            SolidBrush[] solidniBrushe = Enumerable.Repeat(new SolidBrush(bila), pocetMap).ToArray();
            if (pruhlednostCasPozice == 0)
            {
                e.Graphics.DrawString(ksa, front, solidniBrushe[0], xy[0], xy[1]);
                peni[id_vyberu] = new Pen(belejsiBila, 6);//*
                solidniBrushe[id_vyberu] = solitbruch;
            }
            else if (pruhlednostCasPozice == -1)
                e.Graphics.DrawString(ksa, front, solitbruch, xy[0], xy[1]);
            for (int i = 0; i < peni.Length; i++)
            {
                string txt = "Compiler Error CS0165 Use of unassigned local variable 'name'";
                int offsetCisel = (int)(10 * pomerovyKoeficient[0]);
                int offTuto = 0;
                int str;
                if (i == 0)
                {
                    str = 3 * strana;//int dlouhejTutorial = 1; 
                    txt = "Tutorial";
                    offsetCisel = 0;
                    offTuto = 5;
                }
                else
                {
                    str = strana;
                    txt = i.ToString();
                }
                e.Graphics.DrawString(txt, new Font("Helvetica", (int)(42 * pomerovyKoeficient[0]), FontStyle.Bold), solidniBrushe[i], x + offsetCisel - offTuto, y);
                e.Graphics.DrawLine(peni[i], x + offsetCisel + (int)(3 * pomerovyKoeficient[0]), y2, x + str - offsetCisel - (int)(3 * pomerovyKoeficient[0]), y2);
                e.Graphics.DrawRectangle(peni[i], x, y, str, strana);//zámky čísla
                if (i > pocetDokoncenychMap)
                {
                    e.Graphics.DrawImage(obry[0], x, y);
                }
                x += str + mezera;
            }
            Font nack = new Font("Helvetica", (int)(11 * pomerovyKoeficient[0]));
            e.Graphics.DrawString("Obtížnost: " + obtiznosti[obtiznost - 1], nack, solitbruch, 0, Height - (int)(20 * pomerovyKoeficient[1]));
            e.Graphics.DrawString("Číslo hry: " + (cisloHry + 1), nack, solitbruch, 0, Height - (int)(40 * pomerovyKoeficient[1]));
        }


        void PbPaintHra(object sender, PaintEventArgs e)
        {
            pruhlednostCasPozice++;
            if (dh.uzMuzes && !blokKresleniHry)
            {
                List<ObdelnikBezKolize> obdelnikBezKolizes = new List<ObdelnikBezKolize>();
                Image Malario = null;
                int nulaJeNaStreduX = -poziceX + velikostObrazovky[0] / 2;
                int nulaJeNaStreduY = (int)(-poziceY + velikostObrazovky[1] / 2 + 100 * pomerovyKoeficient[1]); //y je o něco níž
                int divider = (int)(77 * pomerovyKoeficient[0]);//vzd na krok
                int M = 337 + 01;
                int minus = 697;
                bool stuj = false;
                int nulaJeVejs = (int)(nulaJeNaStreduY - 80 * pomerovyKoeficient[1]);

                e.Graphics.DrawImage(dh.pozadi.img, new Point(-(int)((poziceX / (double)dh.velikostMapy[0]) * ((double)dh.pozadi.img.Width - Width)), dh.pozadi.Y));
                //**/e.Graphics.DrawString("poziceX: " + poziceX + "", new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(bila), 0, 0);
                if (poziceX > dh.velikostMapy[0])
                    poziceX = dh.velikostMapy[0];
                else if (poziceX == dh.velikostMapy[0])
                    muzuSeHnout[1] = false;
                else if (poziceX < 0)
                    poziceX = 0;
                else if (poziceX == 0)
                    muzuSeHnout[0] = false;
                else
                    muzuSeHnout[0] = muzuSeHnout[1] = true;
                muzuSeHnout[3] = true;
                muzuSeHnout[2] = true;
                semNaZebriku = false;

                /*
                if (pruhlednostCasPozice > casRef + 500 && nepadat)
                {
                    muzuSeHnout[3] = false;
                    yPohyb.Stop();
                }
                if (yPohyb.Enabled && !muzuSeHnout[3])
                {
                    poziceY -= 300;
                    yPohyb.Start();
                    casRef = pruhlednostCasPozice;
                }*/

                foreach (MapObjects.Ladder zerpsik in dh.zebraci)
                {
                    if (NeniMimoObraz(zerpsik.X, zerpsik.Y, dh.ruznyDalsiObry[0].Width, zerpsik.height)) {
                        for (int i = 0; i < zerpsik.height / dh.ruznyDalsiObry[0].Height; i++)
                        {
                            e.Graphics.DrawImage(dh.ruznyDalsiObry[0], zerpsik.X + nulaJeNaStreduX, zerpsik.Y + dh.ruznyDalsiObry[0].Height * i + nulaJeNaStreduY);
                        }//if(KolizeSvisla(zerpsik.X, dh.ruznyDalsiObry[0].Height, zerpsik.Y, true))
                        if (zerpsik.X <= poziceX && poziceX <= zerpsik.X + dh.ruznyDalsiObry[0].Width && poziceY >= zerpsik.Y && zerpsik.Y + zerpsik.height >= poziceY)
                        {
                            semNaZebriku = true;
                            stuj = true;
                        }
                    }
                }
                foreach (MapObjects.ObdelnikBezKolize obdik in dh.obdelnikove)
                {
                    if (!ObdelnikMimoObraz(obdik.X, obdik.Y, obdik.sirka, obdik.vyska)) {
                        if (!obdik.navrch)
                            e.Graphics.FillRectangle(new SolidBrush(obdik.barva), obdik.X + nulaJeNaStreduX, obdik.Y + nulaJeNaStreduY, obdik.sirka, obdik.vyska);
                        else
                            obdelnikBezKolizes.Add(obdik);
                    }
                }
                //obd
                foreach (MapObjects.Obdélník obdik in dh.rectanglesy)
                {
                    if (!ObdelnikMimoObraz(obdik.X, obdik.Y, obdik.sirka, obdik.vyska)) {
                        e.Graphics.FillRectangle(new SolidBrush(obdik.barva), obdik.X + nulaJeNaStreduX, obdik.Y + nulaJeNaStreduY, obdik.sirka, obdik.vyska);
                        KolizeSvisla(obdik.X, obdik.sirka, obdik.Y);//if(KolizeSvisla(obdik.X, obdik.sirka, obdik.Y)){if (!nahoru){poziceY = obdik.Y;yPohyb.Stop();}muzuSeHnout[3] = false;
                        KolizeVodorovna(obdik.Y, obdik.vyska, obdik.X, obdik.sirka);
                    }
                }
                foreach (MapObjects.Smileyface welcome in dh.xychti)
                {
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[welcome.IDofPicture(pruhlednostCasPozice)], welcome.X + nulaJeNaStreduX, welcome.Y + nulaJeNaStreduY);
                    //kolido eneme
                }
                foreach (MapObjects.Obrazek obr in dh.obrazeky)
                {
                    if (!ObdelnikMimoObraz(obr.X, obr.Y, obr.img.Width, obr.img.Width))
                        e.Graphics.DrawImage(obr.img, obr.X + nulaJeNaStreduX, obr.Y + nulaJeNaStreduY);
                }
                foreach (MapObjects.InterestingPlaces vec in dh.kraviny)
                {//ca cas koskq
                    if (vec.X - vec.imagy[0].Width < poziceX + Width / 2 && vec.X + vec.imagy[0].Width > poziceX - Width / 2 &&
                        vec.Y - vec.imagy[0].Height < poziceY + Height / 2 && vec.Y + vec.imagy[0].Height > poziceY - Height / 2) {
                        if (!vec.sezrany)
                        {
                            int ani  = 0;
                            int Yfix = 0;
                            if(vec.co != InterestingPlaces.veci.Vlajka)
                                Yfix = -vec.imagy[0].Height/2;
                            Rectangle obvod = new Rectangle(
                                vec.X,
                                vec.Y+Yfix,
                                vec.imagy[0].Width,
                                vec.imagy[0].Height);
                            if (vec.co == MapObjects.InterestingPlaces.veci.nezranitelnost)
                                ani = pruhlednostCasPozice / 5 % 4;//já si tam dam svůj dýlej ty famfulo
                            if (obvod.Contains(new Point(poziceX, poziceY)))
                            {
                                if (vec.co != MapObjects.InterestingPlaces.veci.Vlajka) //{ }
                                    vec.sezrany = true;
                                switch (vec.co)
                                {
                                    case MapObjects.InterestingPlaces.veci.Vlajka:
                                        win = true;
                                        break;
                                    case MapObjects.InterestingPlaces.veci.nezranitelnost:
                                        iddqd = true;//godmode
                                        skok = pruhlednostCasPozice;
                                        break;
                                    case MapObjects.InterestingPlaces.veci.Heal:
                                        zivoty++;
                                        break;
                                }
                            }
                            e.Graphics.DrawImage(vec.imagy[ani], vec.X + nulaJeNaStreduX, vec.Y + nulaJeVejs);//e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(42, Color.OrangeRed)), vec.X + nulaJeNaStreduX, vec.Y + nulaJeVejs, vec.imagy[0].Width, vec.imagy[0].Height);
                        }
                    }
                }




                foreach (MapObjects.Otaznik blok in dh.otaznikovyCtverce)
                {
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[20], new Point(blok.X + nulaJeNaStreduX, blok.Y + nulaJeNaStreduY));
                }
                foreach (MapObjects.OneTile blok in dh.ctverce)
                {
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[19], new Point(blok.X + nulaJeNaStreduX, blok.Y + nulaJeNaStreduY));
                }


                //pictureBox1.SizeMode = PictureBoxSizeMode.Clip; Omlouvám se za zmatení. Můj omyl, PictureBoxSizeMode.Clip není platná hodnota pro vlastnost SizeMode u PictureBox. Omlouvám se za nesprávnou informaci.  Proč GPT nikdy neprotestuje? Protože vždycky souhlasí!
                foreach (MapObjects.Enemy n in dh.enemaci)//tvl to je dálka sem
                {
                    int obr = 888;
                    switch (n.typ)
                    {
                        case Enemy.Typy.SWS:
                            obr = 8;
                            break;
                        case Enemy.Typy.Tonk:
                            obr = 9;
                            break;
                        case Enemy.Typy.Hitler:
                            obr = 10;
                            break;
                        case Enemy.Typy.dying
                            : obr = 11;
                            break;
                        case Enemy.Typy.mrkev: obr = 12;
                            break;
                    }
                    if (NeniMimoObraz(n.X, n.Y, dh.ruznyDalsiObry[obr].Width, dh.ruznyDalsiObry[obr].Height)) { //scaežoakn zincaěajka
                        //int ubratTonkuZleva = 0, ubratTonkuZprava = 0; jeden dává přes druhýho
                        int casy = 99;
                        if (obr > 8 && obr < 11) {
                            if (n.X > poziceX + dh.ruznyDalsiObry[obr].Width / 2)
                                n.smer = '←';
                            else n.smer = '→';
                        }
                        if (n.typ == MapObjects.Enemy.Typy.SWS)
                        {//e.Graphics.DrawString(n.smer+n.casStrelba.ToString()+":"+pruhlednostCasPozice, new Font("Helvetica", 42, FontStyle.Bold), new SolidBrush(Color.FromArgb(210, 210, 210, 0)), 0, 500);
                            if (n.X > n.zarazky[1] && n.smer != 'L') {
                                n.smer = '←';
                            }
                            else if (n.X < n.zarazky[0] && n.smer != 'P') {
                                n.smer = '→';
                            }
                            if (n.smer != '\0')
                                obr = 4 + (pruhlednostCasPozice / 7) % 4;
                            if (n.smer == '→')
                                n.X += 3;
                            else if (n.smer == '←')
                                n.X -= 3;
                            if (n.casStrelba < pruhlednostCasPozice - casy)
                                n.casStrelba = pruhlednostCasPozice + casy;
                            else if (n.casStrelba < pruhlednostCasPozice + casy * 2 / 3) {
                                if (n.smer == 'P')
                                    n.smer = '→';
                                else if (n.smer == 'L')
                                    n.smer = '←';
                            }
                            else {
                                if (!(n.X > poziceX && n.smer == '→' || n.X < poziceX && n.smer == '←') || obtiznost == 3) {// } else { 
                                    obr = 8; 
                                    if (n.smer == '→')
                                        n.smer = 'P';
                                    else if (n.smer == '←')
                                        n.smer = 'L';
                                    if (n.casStrelba == pruhlednostCasPozice + casy * 5/6)
                                        Strelba(n.X, n.Y, Projectile.Typy.Rajče, n.typ, n.smer);//"Attack of the Killer Tomatoes!"
                                }
                            }
                        }
                        else if (n.typ == MapObjects.Enemy.Typy.Tonk)//projektil má info pro koho je
                        {//obr = 9; //e.Graphics.DrawRectangle(new Pen(cerna, 16), n.X - dh.ruznyDalsiObry[obr].Width / 2 - Width / 2 + nulaJeNaStreduX, n.Y - dh.ruznyDalsiObry[obr].Height / 2 - Height / 2 + nulaJeNaStreduY, Width, Height);
                            for (int HP = 0; HP < n.health-1; HP++)
                            {
                                e.Graphics.DrawImage(strely[2], -(int)(pomerovyKoeficient[0] * 202) + HP * dh.ruznyDalsiObry[2].Width / 1 + n.X + nulaJeNaStreduX, n.Y - dh.ruznyDalsiObry[0].Height - (int)(pomerovyKoeficient[1] * 88) + nulaJeNaStreduY);
                            }
                            //flymaggot = true;iddqd = true;e.Graphics.DrawString(((Math.Atan((poziceY - n.Y) / (double)(poziceX - n.X))*180/Math.PI)%360).ToString(), new Font("Helvetica", 42, FontStyle.Bold), new SolidBrush(Color.FromArgb(210, 210, 210, 0)), 0, 500);
                            if (n.casStrelba < pruhlednostCasPozice - casy/obtiznost)
                                n.casStrelba = pruhlednostCasPozice + casy/obtiznost;
                                if (n.casStrelba == pruhlednostCasPozice)
                                    Strelba(n.X, n.Y, Projectile.Typy.XM1147, n.typ, n.smer);
                        }
                        else if (n.typ == MapObjects.Enemy.Typy.Hitler)
                        {
                            if (n.smer == '←')//Áďa trucuje
                                n.smer = '→';
                            else if (n.smer == '→')
                                n.smer = '←';
                            int R = (int)(n.health*255/(double)666);
                            if (R < 0)
                                R = 0;
                            Napis(e, "Robohitler", new Font("Impact", (int)(42*pomerovyKoeficient[0]), FontStyle.Bold), new SolidBrush(Color.FromArgb(255, R, 000,000)), (int)(-2*pomerovyKoeficient[1]));//, Roboto
                            //rectangel
                            
                            if (n.casStrelba < pruhlednostCasPozice)
                                n.casStrelba = pruhlednostCasPozice+400;
                            else if (n.casStrelba == pruhlednostCasPozice+21  ||  n.casStrelba == pruhlednostCasPozice+42  ||  n.casStrelba == pruhlednostCasPozice+221  ||  n.casStrelba == pruhlednostCasPozice+242) {
                              //int yShift = -dh.ruznyDalsiObry[obr].Height* 8/11;
                                int xShift = -dh.ruznyDalsiObry[10].Width * 13/16;
                                int yShift;
                                if(n.casStrelba != pruhlednostCasPozice +42  &&  n.casStrelba != pruhlednostCasPozice+242)
                                    xShift += (int)(404*pomerovyKoeficient[0]);
                                for (int j = 0; j < 2; j++)
                                {
                                    yShift = -dh.ruznyDalsiObry[obr].Height* 8/11;
                                    for (int i = 0; i < 5; i++)
                                    {
                                        projectilesy.Add(new Projectile(n.X +xShift, n.Y +yShift, Projectile.Typy.Rajče));
                                        projectilesy[projectilesy.Count - 1].smer = Math.PI;
                                        yShift += (int)(32*pomerovyKoeficient[1]);
                                    }
                                    xShift += (int)(16 *pomerovyKoeficient[0]);
                                }
                            }
                            else if (n.casStrelba == pruhlednostCasPozice+121  ||  n.casStrelba == pruhlednostCasPozice+142  ||  n.casStrelba == pruhlednostCasPozice+321  ||  n.casStrelba == pruhlednostCasPozice+342){
                                int yShift = -dh.ruznyDalsiObry[obr].Height* 6/13;
                                int xShift = -dh.ruznyDalsiObry[10].Width * 10/16;
                                for (int j = 0; j < 2; j++)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        projectilesy.Add(new Projectile(n.X +xShift, n.Y +yShift, Projectile.Typy.XM1147));
                                        projectilesy[projectilesy.Count - 1].smer = Math.PI;
                                        xShift += (int)(36 *pomerovyKoeficient[0]);
                                    }
                                    yShift += (int)(42 * pomerovyKoeficient[1]*5);
                                }
                            }
                            else if ((n.casStrelba == pruhlednostCasPozice +190  ||  n.casStrelba == pruhlednostCasPozice +390)  &&  obtiznost > 1){
                                Strelba(
                                    n.X - dh.ruznyDalsiObry[obr].Width *10/16, 
                                    n.Y -dh.ruznyDalsiObry[10].Width*10/16, 
                                    Projectile.Typy.Tokarev7_62_X_25mm, Enemy.Typy.Hitler, (char)21328);
                            }
                            else if (n.casStrelba == pruhlednostCasPozice +90  &&  obtiznost == 3){
                                MessageBox.Show("Pro další provádění programu není dostatek paměti. To je smůla.");//Zákeřný útok
                            }
                            for (int i = 0; i < projectilesy.Count(); i++)
                            {
                                if (new Rectangle(n.X - dh.ruznyDalsiObry[obr].Width * 9 / 16,
                                n.Y - dh.ruznyDalsiObry[obr].Height*9/10,
                                666, (int)(666*pomerovyKoeficient[1]))
                                .Contains(projectilesy[i].X, projectilesy[i].Y)  &&
                                projectilesy[i].typ == Projectile.Typy.Tokarev7_62_X_25mm) { 
                                        n.health--;
                                        projectilesy.RemoveAt(i);
                                        i--;
                                    }
                            }//n.health-=2;
                            //e.Graphics.DrawRectangle(new Pen(Color.Red, 16), n.X + nulaJeNaStreduX - dh.ruznyDalsiObry[obr].Width*9/16, n.Y + (int)(-poziceY + velikostObrazovky[1] / 2 + 159 * pomerovyKoeficient[1]) - dh.ruznyDalsiObry[obr].Height, 666, 666);
                            if (n.health <= 0)
                            {
                                win = true;
                                n.typ = Enemy.Typy.dying;
                                n.casHit = pruhlednostCasPozice;
                                dh.mini.Add(new Mina(n.X - dh.ruznyDalsiObry[obr].Width *5/8, n.Y - dh.ruznyDalsiObry[obr].Height*2/3));
                                dh.mini[dh.mini.Count - 1].boomnuta = true;
                                dh.mini[dh.mini.Count - 1].CR = pruhlednostCasPozice + 8;
                                dh.mini.Add(new Mina(n.X - dh.ruznyDalsiObry[obr].Width *7/16, n.Y - dh.ruznyDalsiObry[obr].Height*7/10*2/3));
                                dh.mini[dh.mini.Count - 1].boomnuta = true;
                                dh.mini[dh.mini.Count - 1].CR = pruhlednostCasPozice + 8;
                                n.X = n.X - (int)(222*pomerovyKoeficient[0]);
                                n.Y = n.Y - (int)(142*pomerovyKoeficient[1]);
                            }
                        }
                        Image img = new Bitmap(dh.ruznyDalsiObry[obr]);
                        if (n.typ == MapObjects.Enemy.Typy.dying)
                        {
                            double pomer = 0;
                            if (n.casHit > pruhlednostCasPozice - 8)
                            {
                                pomer = (pruhlednostCasPozice - n.casHit) / (double)8;
                            }
                            else if (n.casHit > pruhlednostCasPozice - 32)
                            {
                                pomer = 1;
                            }
                            else { 
                                n.typ = MapObjects.Enemy.Typy.mrkev;
                                n.X -= (int)(64 * pomerovyKoeficient[0]);
                            }
                            int heh = (int)(dh.ruznyDalsiObry[obr].Height * pomer);
                            if (heh > 0)
                                img = new Bitmap(img, img.Width, heh);
                        }
                        else if (n.typ == MapObjects.Enemy.Typy.mrkev)
                        {

                        }
                        if ((n.X - dh.ruznyDalsiObry[obr].Width < poziceX && n.X > poziceX && n.Y - dh.ruznyDalsiObry[obr].Height < poziceY && n.Y > poziceY - 101) && obr <= 10)
                        {
                            if (!atack/*  &&  */)
                            {
                                if (n.casHit < pruhlednostCasPozice - 100) {
                                    n.casHit = pruhlednostCasPozice;
                                    if (!iddqd) {
                                        if(obtiznost != 1)
                                            zivoty--;
                                        if (n.typ == Enemy.Typy.Hitler)
                                            zivoty -= 4;
                                    }
                                }
                            }
                            else {
                                n.health--;
                                atack = false;
                                speeed = nominalSpeed;
                                n.casHit = pruhlednostCasPozice - 88; //fix hitu při hitu, aby nehitoval, když je hitnutej
                                if (atkForm == '↓') {
                                    if(obtiznost < 3)
                                        n.health -= 3;//↓ zabije tonk
                                    else
                                        n.health -= 2;//nezabije tonk
                                } else
                                    nahoru = true;
                                Yminule = 999999;
                                atkForm = '\0';
                                if (n.health <= 0) {
                                    n.typ = Enemy.Typy.dying;
                                    n.casHit = pruhlednostCasPozice;
                                    nahoru = true;
                                    n.X += (int)(84 * pomerovyKoeficient[0]);
                                }
                            }

                        }
                        if (n.smer == '←' || n.smer == 'L')
                            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        e.Graphics.DrawImage(img, n.X + nulaJeNaStreduX - dh.ruznyDalsiObry[obr].Width, n.Y + (int)(-poziceY + velikostObrazovky[1] / 2 + 123 * pomerovyKoeficient[1]) - img.Height);

                    }
                }//↑enemáci, střely↓
                /////////////*foreach (Projectile/*Hele! Tile*/ projectiles/*bez lesa, jako*/ in/*Ni!*/ projectilesy/*ČR*/) {
                ////////////    if (NeniMimoObraz(/*teď neni důležitý*/)true) {
                ////////////        //něco, já nevim ještě ty jo
                
                ////////////    }
                ////////////    else {AAAAAAAAAAAAAAAAAAAAAA tohle se muší fórem, Název AAAAAAAAAAAAAAAAAAAAAA v aktuálním kontextu neexistuje., Typ nebo název oboru názvů AAAAAAAAAAAAAAAAAAAAAA se nenašel. (Nechybí direktiva using nebo odkaz na sestavení?)
                ////////////        projectiles.bouchlej = true;/////tak jede někam do píče asi né, tak tam už nemuší bejt
                //ctrlK a pak ctrlC, odkokotentit je na U
                ////////////    }
                ////////////}*/
                /*Rus, Američan a Čech ztroskotají na pustym vostrově. Chytnou je domorodci – kanibalové. Předvedou je před náčelníka. Náčelník jen sedí na bambusovym trůnu, jednim loktem opřenej o opěrák a v druhý ruce dvě skleněný koule, se kterejma si točí na dlani a po chvíli řiká: "Když mě a mému kmeni předvedete s těmito koulemi něco, co jsme ještě neviděli, pustíme vás!" Tak Rusák a Amík cvičí celej tejden, až nadejde osudnej den. První to zkusí Rus. Prohazuje koulema pod rukama, nohama a náčelník řekne: "...sem viděl!" Sežrat!" Druhý to zkusí Amerikán. Koulema háže na nohou jak s hakisákem a náčelník jen: "...sem viděl! Sežrat!" Čech přijde k náčelníkovi, nakloní se a něco mu pošeptá do ucha a náčelník se začne smát a pak řiká: "Toho pusťte!" Když v klidu Čech odejde, tak se ho ostatní z kmene ptaj, co že mu to jako řek, že ho pustil?! A náčelník řiká: " Ten blb mi řekl, že jednu kouli rozbil a tu druhou ztratil."*/for (int i = 0; i < projectilesy.Count(); i++){//dsf
                    if (/*/*/NeniMimoObraz/**//*ObdelnikMimoObraz*/(projectilesy[i].X, projectilesy[i].Y+strely[2].Height/2, strely[3].Width, strely[3].Height)) {//NeniMi/*mino*/moObraz() no a proč ne AAAAAAAA
                        Image mgi = null;
                        int bulletSpeed = -32;
                        //int xfix = 0+0+0+0+0+0+0+0+0+0+0+0+0+0+0+0+0+0-(int)0.0000000000000;
                        switch (projectilesy[i].typ)
                        {
                            case Projectile.Typy.Rajče:
                                mgi = new Bitmap(strely[0]);
                                bulletSpeed = 8;
                                break;
                            case Projectile.Typy.Tokarev7_62_X_25mm:
                                mgi = new Bitmap(strely[1]);
                                bulletSpeed = 13-obtiznost+4;
                                break;
                            case Projectile.Typy.XM1147:
                                mgi = new Bitmap(strely[3]);
                                bulletSpeed = 10+obtiznost*2;
                                break;
                        }
                        int xfix = mgi.Width;
                        if (projectilesy[i].smer >Math.PI/2  ||  projectilesy[i].smer <= -Math.PI/2){
                            mgi.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            xfix = 0;
                        }
                        int yfix = 0;
                        if(projectilesy[i].typ == Projectile.Typy.XM1147){
                            yfix = (int)(-10*pomerovyKoeficient[1]);
                        }
                        else if(projectilesy[i].typ == Projectile.Typy.Tokarev7_62_X_25mm){
                            yfix = (int)(-18*pomerovyKoeficient[1]);
                        }
                        e.Graphics.DrawImage(mgi, projectilesy[i].X + nulaJeNaStreduX, projectilesy[i].Y- mgi.Height+nulaJeNaStreduY);
                        projectilesy[i].X+=(int)(bulletSpeed*Math.Cos(projectilesy[i].smer)*pomerovyKoeficient[0]);
                        projectilesy[i].Y+=(int)(bulletSpeed*Math.Sin(projectilesy[i].smer)*pomerovyKoeficient[1]);
                        if (!new Rectangle(poziceX -malarioveFigury[0].Width/2, poziceY -malarioveFigury[0].Height/4 -mgi.Height +yfix, malarioveFigury[0].Width, malarioveFigury[0].Height/*7/8/*2/3*/+yfix)
                            .Contains(projectilesy[i].X+xfix, projectilesy[i].Y))
                            continue;
                        else
                            if(!iddqd)
                                zivoty--;
                            //Vybuch(projectilesy[i].X+xfix, projectilesy[i].Y, projectilesy[i].typ)
                            //Otres(2)
                    }
                    projectilesy.RemoveAt(i);//remůvnu projectiles z projectilesů, když doprojectil svůj peoect
                    i--;
                }


                if (flymaggot)
                {
                    semNaZebriku = true;
                    speeed = 5.55;
                    stuj = true;
                    muzuSeHnout[0] = muzuSeHnout[1] = muzuSeHnout[3] = muzuSeHnout[2] = true;
                }
                if (!muzuSeHnout[0] && xM.Enabled || !muzuSeHnout[1] && xP.Enabled)
                    stuj = true;
                if ((xM.Enabled && xP.Enabled || !xM.Enabled && !xP.Enabled || poziceX == dh.velikostMapy[0] || poziceX == 0) || stuj)
                {
                    Xreferencni = poziceX;
                    M = 3;
                }
                else if (xM.Enabled)
                {
                    if (naposledySemSel == '→')
                        Xreferencni = poziceX;
                    minus = +1;
                    naposledySemSel = '←';
                }
                else if (xP.Enabled)
                {
                    if (naposledySemSel == '←')
                        Xreferencni = poziceX;
                    minus = -1;
                    naposledySemSel = '→';
                }
                if (M != 3 && !muzuSeHnout[3])
                {
                    int posun = minus * (poziceX * minus - Xreferencni * minus);
                    int tni = -minus * posun % divider;
                    if (tni < divider / 3)
                        M = 2;
                    else if (tni < 2 * divider / 3)
                        M = 0;
                    else
                        M = 1;
                }
                else if (((pruhlednostCasPozice % (3333 / 18) > 42) && M == 3 && !semNaZebriku) || (semNaZebriku && poziceX % (84) > 42))
                    minus = -minus;
                if (semNaZebriku && poziceY % 105 > 55)///////
                    minus = -minus;
                if (naposledySemSel == '←' && M == 3)
                    minus = -minus;
                if (muzuSeHnout[3])
                {
                    if (atack && !semNaZebriku  &&  id_vyberu != 4)
                    {
                        bool vypni = false;
                        muzuSeHnout3minule = false;
                        nahoru = false;
                        if (atack && ((xM.Enabled || xP.Enabled) && atkForm == '\0' && dashe[0] > dashe[1]) || atkForm == '↘')
                        {
                            if (atkForm == '\0')
                            {
                                Xreferencni = poziceX;
                                atkForm = '↘';
                                dashe[1]++;
                            }
                            M = 5;
                            speeed = 5;
                            if (naposledySemSel == '→')
                            {
                                //xP.Enabled = true;
                                if (poziceX > Xreferencni + 421 * pomerovyKoeficient[0])
                                {
                                    muzuSeHnout[0] = false;
                                    vypni = true;
                                    if (poutokuSTOP)
                                    {
                                        poutokuSTOP = false;
                                        xP.Stop();
                                    }
                                }
                            }
                            else if (naposledySemSel == '←')
                            {
                                //xM.Enabled = true;
                                if (poziceX < Xreferencni - 421 * pomerovyKoeficient[0])
                                {
                                    muzuSeHnout[1] = false;
                                    vypni = true;
                                    if (poutokuSTOP)
                                    {
                                        poutokuSTOP = false;
                                        xM.Stop();
                                    }
                                }
                            }
                            if (vypni)
                            {
                                speeed = nominalSpeed;
                                atkForm = '\0';
                                atack = false;
                            }
                        }
                        else
                        {
                            atkForm = '↓';
                            muzuSeHnout[1] = muzuSeHnout[0] = xM.Enabled = xP.Enabled = false;
                            M = 6;//if neni mapa 4
                        }
                    }/*
                    else if (atack  &&  id_vyberu == 4)
                    {
                        Strelba(poziceX, poziceY, Projectile.Typy.Tokarev7_62_X_25mm, Enemy.Typy.mrkev, naposledySemSel);//dyš střílí mrkev, střílí Mario
                    }*/
                    else
                        M = 4;
                    yPohyb.Start();
                }
                else
                    //if(id_vyberu != 4)
                    //    atack = false;
                if (bylSemNaZebriku && !semNaZebriku)
                {
                    Yreferencni = poziceY;
                    bylSemNaZebriku = false;
                    muzuSeHnout3minule = false;
                }
                if (semNaZebriku)
                {
                    int lezeni = (int)(5 * pomerovyKoeficient[1]);
                    yPohyb.Stop();
                    M = 4;
                    if (nahorudolu == 1)
                        poziceY -= lezeni;
                    else if (nahorudolu == -1)
                        poziceY += lezeni;
                    atack = nahoru = false;
                    atkForm = '\0';
                    speeed = nominalSpeed;
                    bylSemNaZebriku = muzuSeHnout[2] = true;
                }
                Malario = new Bitmap(malarioveFigury[M]);
                if (minus > 0)
                    Malario.RotateFlip(RotateFlipType.RotateNoneFlipX);
                if (dead)
                    Malario = malarioveFigury[7];
                e.Graphics.DrawImage(Malario, new Point((Width - Malario.Width) / 2, (int)((Height - Malario.Height + 100 * pomerovyKoeficient[1]) / 2)));//Životy pokusy
                /*
                e.Graphics.DrawImage(malarioveFigury[M], new Point(420, 420));
                e.Graphics.DrawString("Zivoty:", new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(bila), 0, 0);
                e.Graphics.DrawString(zivoty.ToString(), new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(bila), 0, 0);
                */
                //pruhlednostCasPozice++;
                //if (pruhlednostCasPozice > 2147483000)
                //    pruhlednostCasPozice = 0;//co kdyby
                //e.Graphics.DrawString("poziceX: " + poziceX + "", new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(bila), 0, 0);
                //e.Graphics.DrawString(pruhlednostCasPozice + "", new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(Color.FromArgb(255, 0, 0, 0)), 0, 0);
                //x_v_minulem_cyklu = poziceX;
                /*
                //1536.0, velikostObrazovky[1] / 864
                e.Graphics.DrawLine(new Pen(bila, 6), 0, (int)(864 / 2 * pomerovyKoeficient[1]), Width, (int)(864 / 2 * pomerovyKoeficient[1])); //flymaggot = true;
                e.Graphics.DrawLine(new Pen(bila, 6), 0, (int)(864 / 4 * pomerovyKoeficient[1]), Width, (int)(864 / 4 * pomerovyKoeficient[1])); e.Graphics.DrawLine(new Pen(bila, 1), 0, (int)((864+100) / 2 * pomerovyKoeficient[1]), Width, (int)((964) / 2 * pomerovyKoeficient[1])); 
                e.Graphics.DrawLine(new Pen(bila, 6), (int)(1536 / 2 * pomerovyKoeficient[0]), 0, (int)(1536 / 2 * pomerovyKoeficient[0]), Height);
                e.Graphics.DrawLine(new Pen(bila, 6), (int)(1536 / 4 * pomerovyKoeficient[0]), 0, (int)(1536 / 4 * pomerovyKoeficient[0]), Height);//*/
                //e.Graphics.DrawString(poziceY+" "+Yreferencni+" "+pruhlednostCasPozice+" "+KoeficientZmenyY, new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(belejsiBila), 0, 0);
                //e.Graphics.DrawString(casRef.ToString()+nahoru, new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(Color.DarkBlue), 0, 0);
                if (!muzuSeHnout[3] && atkForm != '\0')
                {
                    atkForm = '\0'; speeed = nominalSpeed; dashe[1] = 0; atack = false;/*xP.Stop(); xM.Stop(); xM.Enabled = false; xP.Enabled = false;*/
                }
                if (!muzuSeHnout[3]) dashe[1] = 0;//e.Graphics.DrawString(dashe[0] + dashe[1].ToString() + atkForm + atack/* + Xreferencni.ToString()*/, new Font("Helvetica", (int)(111 * pomerovyKoeficient[0])), new SolidBrush(Color.DarkBlue), 0, 0);
                if (poutokuSTOP && atkForm == '\0')
                {
                    xP.Stop();
                    xM.Stop();
                    poutokuSTOP = false;
                }

                foreach (MapObjects.Trap trp in dh.pasti)
                {
                    int[] soura = new int[] { trp.X, trp.Y };
                    int[] DANGER = { trp.X - dh.ruznyDalsiObry[21].Width / 2, trp.Y };
                    bool jeTamUzSpike = false;
                    Rectangle dosah = new Rectangle(DANGER[0], DANGER[1] - Malario.Height / 3, dh.ruznyDalsiObry[21].Width, dh.ruznyDalsiObry[21].Width + Malario.Height / 3);
                    for (int i = 0; i < vysunutyBodaky.Count; i++)
                    {
                        if (vysunutyBodaky[i][0] == trp.X && vysunutyBodaky[i][1] == trp.Y)
                        {
                            jeTamUzSpike = true;
                        }
                    }
                    if (dosah.Contains(poziceX, poziceY) && !jeTamUzSpike)
                    {
                        dh.bodaky.Add(new Spike(trp.X, trp.Y));
                        vysunutyBodaky.Add(soura);
                    }
                }
                foreach (MapObjects.Spike trn in dh.bodaky)
                {
                    if (NeniMimoObraz(trn.X, trn.Y, dh.ruznyDalsiObry[21].Width, dh.ruznyDalsiObry[21].Height)) {
                        
                        int vysouvani = 8;
                        int[] DANGER = { trn.X - dh.ruznyDalsiObry[21].Width / 2, trn.Y };
                        int posun = dh.ruznyDalsiObry[21].Height;//musí bejt takle, jinak blikne
                        if (!trn.fake) { 
                            //  e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(42, Color.OrangeRed)), DANGER[0] + nulaJeNaStreduX, DANGER[1] + nulaJeVejs - Malario.Height / 3, dh.ruznyDalsiObry[24].Width, dh.ruznyDalsiObry[24].Height+Malario.Height / 3);
                            Rectangle dosah = new Rectangle(DANGER[0], DANGER[1] - Malario.Height / 2/*3*/, dh.ruznyDalsiObry[21].Width, dh.ruznyDalsiObry[21].Width + Malario.Height / 3);
                            if (dosah.Contains(poziceX, poziceY) && trn.c < pruhlednostCasPozice - 25)
                            {
                                trn.c = pruhlednostCasPozice;
                                if (!iddqd)
                                    zivoty--;
                            }
                            if (trn.CR == 0)
                                trn.CR = pruhlednostCasPozice + vysouvani;
                            else if (trn.CR >= pruhlednostCasPozice)
                                posun = -(int)(((pruhlednostCasPozice - trn.CR) / (double)vysouvani) * dh.ruznyDalsiObry[21].Height);
                            else
                                posun = 0;
                            //pruhlednostCasPozice += vysouvani / 3;
                        }
                        else
                            posun = 0;
                        e.Graphics.DrawImage(dh.ruznyDalsiObry[21], DANGER[0] + nulaJeNaStreduX, DANGER[1] + nulaJeVejs + posun);
                    }
                }
                foreach (MapObjects.ObdelnikBezKolize obdik in obdelnikBezKolizes)
                    if (!ObdelnikMimoObraz(obdik.X, obdik.Y, obdik.sirka, obdik.vyska)) 
                        e.Graphics.FillRectangle(new SolidBrush(obdik.barva), obdik.X + nulaJeNaStreduX, obdik.Y + nulaJeNaStreduY, obdik.sirka, obdik.vyska);
                if(id_vyberu == 3)
                    e.Graphics.DrawImage(new Bitmap(Image.FromFile(solutionPath + "\\Pictures\\Textury\\ObjektyNaMape\\Flag_of_the_Soviet_Union.svg.png"), 
                        (int)(152*pomerovyKoeficient[0]), (/*u*/int)(76 * pomerovyKoeficient[1])), 
                        (float)(1107*pomerovyKoeficient[0] + nulaJeNaStreduX), (float)(726 *pomerovyKoeficient[1] + nulaJeNaStreduY));
                else if(id_vyberu == 4)
                {
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[15], poziceX + nulaJeNaStreduX-(int)(39*pomerovyKoeficient[0]), poziceY + nulaJeNaStreduY-(int)(139*pomerovyKoeficient[0]));
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[18], poziceX + nulaJeNaStreduX-(int)(52*pomerovyKoeficient[0]), poziceY + nulaJeNaStreduY-(int)(69*pomerovyKoeficient [0]));
                    dashe[0] = 0;
                }
                foreach (MapObjects.Napis text in dh.kecy)
                {
                    e.Graphics.DrawString(text.txt, new Font(FontFamily.GenericMonospace, text.sze, FontStyle.Bold), new SolidBrush(text.clr), new Point(text.X + nulaJeNaStreduX/**3/2*/, text.Y + nulaJeNaStreduY/*/2*/));
                }
                foreach (MapObjects.Mina LM in dh.mini)
                {/*
                    poziceX = LM.X;
                    poziceY = LM.Y;*///e.Graphics.DrawRectangle(new Pen(cerna, 8), (LM.X-Malario.Width)+nulaJeNaStreduX, (LM.Y-Malario.Height)+nulaJeNaStreduY, Malario.Width, Malario.Height); (int)(-poziceY + velikostObrazovky[1] / 2
                    int boomTime = 8;
                    int[] DANGER = { LM.X - Malario.Width / 2, LM.Y + Malario.Height / 2 };
                    if (obtiznost == 2)//nápověda pro miny
                        e.Graphics.DrawRectangle(new Pen(Color.FromArgb(cerna.A / 3, cerna), 2), DANGER[0] + nulaJeNaStreduX, DANGER[1] + (int)(nulaJeNaStreduY - 100 * pomerovyKoeficient[1]), Malario.Width, Malario.Height);
                    else if (obtiznost == 1)
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(42, Color.OrangeRed)), DANGER[0] + nulaJeNaStreduX, DANGER[1] + (int)(nulaJeNaStreduY - 100 * pomerovyKoeficient[1]), Malario.Width, Malario.Height);
                    Rectangle dosah = new Rectangle(
                    DANGER[0],
                    DANGER[1] - (int)(50 * pomerovyKoeficient[1]),
                    Malario.Width,
                    Malario.Height);
                    if (LM.c < 13 && dosah.Contains(poziceX, poziceY) && !LM.boomnuta)
                    {
                        //if (LM.c < 13  &&  poziceX >= LM.X-Malario.Width/2  &&  poziceX <= LM.X+Malario.Width/2  &&  poziceY >= LM.Y-Malario.Height/2  &&  poziceY <= LM.Y+Malario.Height/2  &&  !LM.boomnuta) { 
                        LM.CR = pruhlednostCasPozice + boomTime;
                        LM.boomnuta = true;
                        if (!iddqd)
                            zivoty--;
                    }
                    if (LM.boomnuta && LM.c < 16 + 5)
                    {
                        Image bum = null;
                        double pomer = 42;
                        if (LM.CR > pruhlednostCasPozice && LM.c == 0)
                        {
                            pomer = (pruhlednostCasPozice - LM.CR) / (double)boomTime + 1.3;
                            //pruhlednostCasPozice += 400;
                        }
                        else
                        {
                            if (LM.CR >= pruhlednostCasPozice)
                            {
                                LM.CR = pruhlednostCasPozice + boomTime / 2;
                                if (LM.c % 2 == 1)
                                    pomer = 1.4;
                                else
                                    pomer = 1.2;
                                LM.c++;
                            }
                        }
                        int w = (int)(dh.ruznyDalsiObry[16].Width * pomer), h = (int)(dh.ruznyDalsiObry[16].Height * pomer);
                        if (w > 0 && h > 0)
                            bum = new Bitmap(dh.ruznyDalsiObry[16], w, h);
                        if (bum != null)
                            e.Graphics.DrawImage(bum, LM.X + nulaJeNaStreduX - bum.Width / 2, LM.Y + nulaJeNaStreduY - bum.Height / 2);
                    }
                    else
                    {
                        if (obtiznost == 3)
                        {
                            LM.boomnuta = false;
                            LM.c = 0;
                        }
                    }
                }

                for (int dshs = dashe[0] - dashe[1] - 1; dshs >= 0; dshs--)
                {
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[14], this.Width - (dshs + 1) * dh.ruznyDalsiObry[14].Width / 2 - (int)(pomerovyKoeficient[0] * 55), this.Height - dh.ruznyDalsiObry[14].Height + (int)(pomerovyKoeficient[1] * 32));
                }
                for (int HP = 0; HP < zivoty; HP++)
                {
                    e.Graphics.DrawImage(dh.ruznyDalsiObry[13], -(int)(pomerovyKoeficient[0] * 18) + HP * dh.ruznyDalsiObry[13].Width / 3 * 2, this.Height - dh.ruznyDalsiObry[14].Height + (int)(pomerovyKoeficient[1] * 8));
                }
                string[] asd = { "Pokus ", "Obtížnost " };
                Font F = new Font("Helvetica", (int)(18 * pomerovyKoeficient[0]));
                SolidBrush SB = new SolidBrush(Color.DarkBlue);
                SizeF SF = e.Graphics.MeasureString(asd[1], F);
                e.Graphics.DrawString(asd[1] + obtiznosti[obtiznost - 1], F, SB, 0, 0);
                e.Graphics.DrawString(asd[0] + (pokus + 1), F, SB, 0, SF.Height - (int)(6 * pomerovyKoeficient[0]));
                //e.Graphics.DrawString(poziceX+":"+poziceY+nahoru+muzuSeHnout[3], F, SB, 0, (SF.Height - (int)(6 * pomerovyKoeficient[0]))*2);

                if (testMode) { 
                    iddqd = flymaggot = true; nominalSpeed = 8;}
                if (dh.invulka != null && invulnerability_sphere == null)
                    invulnerability_sphere = dh.invulka;
                if (iddqd && skok < pruhlednostCasPozice - 555 && !testMode)
                    iddqd = false;
                if (iddqd)
                    if(invulnerability_sphere != null)
                        e.Graphics.DrawImage(invulnerability_sphere[pruhlednostCasPozice / 5 % 4], Width - invulnerability_sphere[0].Height - 16, 16);
                if (Yminule < poziceY-6*pomerovyKoeficient[0]  &&  muzuSeHnout[3]/*  &&  !atack*/)
                    nahoru = false;
                Yminule = poziceY;
            }
            if (win)
            {
                int velikostPicma = (int)(423 * pomerovyKoeficient[0]);
                int pozicw = (int)(-69 * pomerovyKoeficient[0]);
                muzuSeHnout = Enumerable.Repeat(false, 4).ToArray();
                xP.Enabled = xM.Enabled = false;
                if (timer1Interval < 0)
                    timer1Interval = pruhlednostCasPozice + 100;
                else if (timer1Interval >= pruhlednostCasPozice)
                {
                    Matrix originalTransform = e.Graphics.Transform.Clone();
                    e.Graphics.TranslateTransform(0, 0);
                    e.Graphics.RotateTransform(42 * (timer1Interval - pruhlednostCasPozice) / 100);
                    e.Graphics.DrawString("Hurá!", new Font("Helvetica", velikostPicma, FontStyle.Bold), new SolidBrush(Color.FromArgb(210, 210, 210, 0)), pozicw, 0);
                    e.Graphics.Transform = originalTransform;
                }
                else if (timer1Interval > pruhlednostCasPozice-73)
                    e.Graphics.DrawString("Hurá!", new Font("Helvetica", velikostPicma, FontStyle.Bold), new SolidBrush(Color.FromArgb(210, 210, 210, 0)), pozicw, 0);
                else
                {
                    this.pictureBox1.Paint -= PbPaintHra;
                    KeyDown -= new KeyEventHandler(KlavesyHra);
                    KeyUp -= new KeyEventHandler(KlavesyHraAleJenomDvaStopy);
                    this.pictureBox1.Paint += PbPainMenu;
                    KeyDown += new KeyEventHandler(OdchytKlavesProMenu);
                    if (id_vyberu == pocetDokoncenychMap  &&  pocetDokoncenychMap != 5)
                    {
                        string sss = (cisloHry + obtiznost.ToString() + (id_vyberu + 1));
                        dh.Save(cisloHry + obtiznost.ToString() + (id_vyberu + 1));
                        savy = dh.NacteniSaveSlotu();
                        pocetDokoncenychMap++;
                    }
                    id_vyberu = 0;
                    pruhlednostCasPozice = 0;
                    muzuSeHnout3minule = false;
                    atack = false;
                    speeed = nominalSpeed;
                    iddqd = false;
                    //ZpetDoMenu(true);
                    ObrazkovyPrehazovac();
                }
            }
            else if (dead)
            {
                timer1.Interval = 30;
                muzuSeHnout = Enumerable.Repeat(false, 4).ToArray();
                xP.Enabled = xM.Enabled = false;
                if (timer1Interval < 0) {
                    timer1Interval = pruhlednostCasPozice + 225;//
                }
                else if (timer1Interval > pruhlednostCasPozice)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(-(int)((timer1Interval-pruhlednostCasPozice-225) / (double)225 * 255), 0, 0, 0)), 0, 0, Width, Height);
                else if (timer1Interval == pruhlednostCasPozice) { 
                    skok = pruhlednostCasPozice+101;
                    blokKresleniHry = true;
                    dh.VymazniMapu();
                    DOOT = dh.NacteniObruDoMovementSchemyATakyDootaAJesteProjektilu("Smrt");
                    for (int i = 0; i < DOOT.Length; i++)
                        DOOT[i] = new Bitmap(DOOT[i], DOOT[i].Width * 2, DOOT[i].Height * 2);
                }
                else if(skok >= pruhlednostCasPozice)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, Width, Height);
                    e.Graphics.DrawImage(DOOT[(pruhlednostCasPozice-skok+101) / 11 % DOOT.Length], Width/2-DOOT[0].Width/2, Height-DOOT[0].Height);
                }
                else { 
                    poziceY = dh.pocatecniSouradnice[1];
                    poziceX = dh.pocatecniSouradnice[0];
                    muzuSeHnout = Enumerable.Repeat(true, 4).ToArray();
                    begin = true;
                    pokus++;
                    dead = false;
                    zivoty = 4 - obtiznost + id_vyberu;
                    timer1Interval = -6;
                    dh.NacteniMapy(id_vyberu);
                    muzuSeHnout3minule = false;
                    DOOT = null;
                    nahoru = false;
                    blokKresleniHry = false;
                    atack = false;
                    timer1.Interval = intervalHra;
                    vysunutyBodaky = new List<int[]>();
                    skok = -2;
                    speeed = nominalSpeed;
                }
            }
            if(zivoty<=0){/*
                DOOT = dh.NacteniObruDoMovementSchemyATakyDootaAJesteProjektilu("Smrt");
                for (int i = 0; i < DOOT.Length; i++) mimo hru AAA
                    DOOT[i] = new Bitmap(DOOT[i], DOOT[i].Width * 2, DOOT[i].Height * 2);*/
                dead = true;
            }
            if (begin){
                if (timer1Interval < 0)
                    timer1Interval = pruhlednostCasPozice + 88;
                else if (timer1Interval >= pruhlednostCasPozice)
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255 * (timer1Interval - pruhlednostCasPozice) / 88, 0, 0, 0)), 0, 0, Width, Height);
                else { 
                    begin = false;
                    timer1Interval = -5;
                }
            }
        }



        private void KlavesyHra(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    ZpetDoMenu(false);
                    iddqd = atack = xP.Enabled = xM.Enabled = false;
                    speeed = nominalSpeed;
                    win = dead = false;
                    break;
                case Keys.Right://bool an pomdínku pro držení
                    //timer1.Tick += new EventHandler(Xplus);
                    //Xreferencni = poziceX;
                    if (!atack  ||  id_vyberu == 4)
                        xP.Start();
                    break;
                case Keys.Up:
                case Keys.Space:
                    nahorudolu = 1;
                    if(!semNaZebriku)
                    yPohyb.Start();
                    break;
                case Keys.Left:
                    //poziceX--;
                    if (!atack  ||  id_vyberu == 4)
                        xM.Start();
                    break;
                case Keys.A:
                case Keys.Down:
                case Keys.Menu:
                    nahorudolu = -1;
                    if (muzuSeHnout[3]  ||  id_vyberu == 4)
                        atack = true;
                    if(id_vyberu == 4)
                        Mstrelba.Enabled = true;
                    break;
                case Keys.S:
                    if (!use)
                        use = true;
                    else
                        use = false;
                    break;
                case Keys.D:
                    break;
            }
        }
        private void KlavesyHraAleJenomDvaStopy(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Right:
                    //timerxp.Stop();
                    //timer1.Tick -= new EventHandler(Xplus);
                    if(!atack  ||  id_vyberu == 4)
                        xP.Stop();
                    else
                        poutokuSTOP = true;
                    break;
                case Keys.Left:
                    if (!atack  ||  id_vyberu == 4)
                        xM.Stop();
                    else
                        poutokuSTOP = true;
                    break;
                case Keys.A:
                case Keys.Up:
                case Keys.Down:
                case Keys.Space:
                nahorudolu = 0;
                break;
            }
            switch(e.KeyCode)
            {
                case Keys.A:
                case Keys.Down:
                case Keys.Menu:
                    Mstrelba.Enabled = false;
                break;
            }
        }
        void Xplus(object sender, EventArgs e)
        {
            if (muzuSeHnout[1])
                poziceX += (int)(5 * speeed * pomerovyKoeficient[0]);
        }
        void Xmius(object sender, EventArgs e)
        {
            if (muzuSeHnout[0])
                poziceX -= (int)(5 * speeed * pomerovyKoeficient[0]);
        }

        void Ykalk(object sender, EventArgs e)
        {
            double skokFix  = (double)30/intervalHra;//protože sem měl původně timer interval na 30ms
            double priUtoku = 14;// *1;
            int plnaRychlostPaduPoDosazeniPoctuCyklu = (int)(36*skokFix);
            double KoeficientZmenyYMax = 9.9 /* speeed)*/;
            if (muzuSeHnout[3])
            {
                if (!muzuSeHnout3minule)
                {
                    //CasRef = Cas.ElapsedMilliseconds;
                    casRef = pruhlednostCasPozice;
                    muzuSeHnout3minule = true;
                }
                if (!atack  ||  id_vyberu == 4)
                    poziceY += (int)((KoeficientZmenyY) * pomerovyKoeficient[1]);
                else
                {
                    if (xM.Enabled || xP.Enabled)//xM.Enabled || xP.Enabled
                        priUtoku /= 4.2;
                    poziceY += (int)(priUtoku * pomerovyKoeficient[1]);
                }
                if (nahoru)
                    poziceY -= (int)((KoeficientZmenyYMax - KoeficientZmenyY) * pomerovyKoeficient[1]);
                if ((casRef + plnaRychlostPaduPoDosazeniPoctuCyklu) / (pruhlednostCasPozice + 2) < 1)
                    KoeficientZmenyY = KoeficientZmenyYMax;
                else
                  //KoeficientZmenyY = Math.Pow(KoeficientZmenyYMax, (double)(Cas.ElapsedMilliseconds - CasRef) / (plnaRychlostPaduPoDosazeniPoctuCyklu/3)/100);
                  KoeficientZmenyY = Math.Pow(KoeficientZmenyYMax, (double)(pruhlednostCasPozice - casRef) / (plnaRychlostPaduPoDosazeniPoctuCyklu));
                if (KoeficientZmenyYMax - KoeficientZmenyY < 0.001)
                    nahoru = false;
            }
            else if (muzuSeHnout[2])
            {
                if (!muzuSeHnout[3] && !nahoru)
                {
                    nahoru = true;//nepadat = true; casRef = pruhlednostCasPozice; Yreferencni = poziceY;
                }
            }
            else
                atkForm = '\0';
        }

        void Pepeska(object sender, EventArgs e)
        {
            if(!dead)
                Strelba(poziceX, poziceY, Projectile.Typy.Tokarev7_62_X_25mm, Enemy.Typy.mrkev, naposledySemSel);
        }

        bool KolizeSvisla(int zacatek, int delka, int hladina)
        {
            int tolerance = (int)(42 * pomerovyKoeficient[1]);
            if (zacatek < poziceX && zacatek + delka > poziceX)
                if (!(hladina - poziceY > tolerance && poziceY - hladina < tolerance) && hladina >= poziceY)//obdik.Y == poziceY, dá se propadnout, if(poziceY > hladina) skáče
                {
                    if (!nahoru/*  &&  !zebrik*/&&!semNaZebriku  &&  !flymaggot)//noclip
                    {
                        poziceY = hladina;
                        yPohyb.Stop();
                        muzuSeHnout[3] = false;
                        muzuSeHnout3minule = false;
                        KoeficientZmenyY = 0;//V jednom ze 142 případů tam zůstane zaseklá a při skoku je na 25ms rychlá ve směru dolu, než dojde k přepočtení v metodě Ykalk, což vede k propadnutí, jež není žádoucím jevem při zmáčknutí klávesy skoku.
                    }/*
                    if (povrchProEmaky == null)
                        povrchProEmaky.Add(new int[] { zacatek, delka, hladina });*/
                    return true;
                }
            return false;
        }

        void KolizeVodorovna(int zacatek, int delka, int pozice, int sirka)
        {
            uint vetsiTolerancePriVyssiRychlosti = 1;
            if (atack)
                vetsiTolerancePriVyssiRychlosti = 2;
            int tolerance = (int)(16 * pomerovyKoeficient[0] * vetsiTolerancePriVyssiRychlosti);
            if (zacatek < poziceY && zacatek + delka > poziceY)
            {
                if ((!(pozice - poziceX > tolerance && poziceX - pozice < tolerance) && pozice >= poziceX)&&!flymaggot)
                {
                    if (poziceX > pozice)
                        poziceX = pozice;
                    muzuSeHnout[1] = false;
                }
                else if ((!(pozice + sirka - poziceX > tolerance && poziceX - pozice - sirka < tolerance) && pozice + sirka >= poziceX)&&!flymaggot)
                {
                    if (poziceX < pozice)
                        poziceX = pozice + sirka;
                    muzuSeHnout[0] = false;
                }
            }
        }

        bool NeniMimoObraz (int x, int y, int s, int d)
        {
            if (id_vyberu !=4){
                if(x-s < poziceX + Width/2   &&  x+s > poziceX - Width/2  &&
                   y-d < poziceY + Height/2  &&  y+d > poziceY - Height/2)
                    return true;}
            else 
                if(x-s < poziceX + Width   &&  x+s > poziceX - Width/2  &&
                   y-d < poziceY + Height  &&  y+d > poziceY - Height)
                    return true;
            return false;
        }
        bool ObdelnikMimoObraz(int x, int y, int s, int d)
        {
            if(x > poziceX + Width/2   &&  x+s < poziceX - Width/2  &&
               y > poziceY + Height/2  &&  y+d < poziceY - Height/2)
                return true;
            return false;
        }


        void Strelba(int xE, int yE, Projectile.Typy typMunice, Enemy.Typy kdoStrili, char smeu)
        {
            int Xstrely = '4', Ystrely = "2".ToCharArray().ToString()[0];//ble ble ble ble ble Chyba	CS0165	Použila se nepřiřazená lokální proměnná Xstrely.	Malario	D:\jako takový něco\VOŠ\2\Z to je ten co začíná v létě\AAAAAA furt se to ňák divně rozesírá\Malario\Malario\Form1.cs	1582	Aktivní, aby ses neposral třeba.
            //v Polsku zakážou domácí úkoly
            switch (kdoStrili)
            {
                case Enemy.Typy.SWS:
                    Ystrely = (int)(yE - 52 * pomerovyKoeficient[1]);
                    if (smeu == 'L')
                        Xstrely = (int)(xE - 180 * pomerovyKoeficient[0]);
                    else if (smeu == 'P')
                        Xstrely = (int)(xE - 77 * pomerovyKoeficient[0]);
                    projectilesy.Add(new Projectile(Xstrely, Ystrely, Projectile.Typy.Rajče ));
                break;
                case Enemy.Typy.Tonk:
                    Ystrely = (int)(yE - 64 * pomerovyKoeficient[1]);
                    if (smeu == '←')
                        Xstrely = (int)(xE - 230 * pomerovyKoeficient[0]);
                    else if (smeu == '→')
                        Xstrely = (int)(xE - 84 * pomerovyKoeficient[0]);
                    projectilesy.Add(new Projectile(Xstrely, Ystrely, Projectile.Typy.XM1147));
                break;
                case Enemy.Typy.Hitler:
                    Xstrely = xE; Ystrely = yE;
                    projectilesy.Add(new Projectile(xE, yE, typMunice));
                    kdoStrili = Enemy.Typy.SWS;
                break;
                case Enemy.Typy.mrkev:
                    projectilesy.Add(new Projectile((int)(xE +42*pomerovyKoeficient[0]), (int)(yE -(42+6)*pomerovyKoeficient[1]), typMunice));
                    projectilesy[projectilesy.Count - 1].smer = rnd.Next(-16, 16) *Math.PI/180;//pálí furt vpravo
                return;
            }
            if (obtiznost == 1)
            {
                if (smeu == '←' || smeu == 'L')                          //   90
                    projectilesy[projectilesy.Count - 1].smer = Math.PI;//180    0
                else                                                   //    -90
                    projectilesy[projectilesy.Count - 1].smer = 0;
            }
            else { 
                int komora = (int)(poziceY-42*pomerovyKoeficient[1]);
                double uhel = Math.Atan2((komora-Ystrely),(double)(poziceX-Xstrely))*180/Math.PI; //uhlík 90-95%, voda nižší než 5%, popel nižší než 10%, síra nižší než 1%, 34,2 MJ/kg
                if (obtiznost == 2)
                {
                    if(kdoStrili == Enemy.Typy.Tonk)
                        if (uhel<160 && uhel>90  ||  uhel>-160 && uhel<-90)
                            uhel = 180;
                        else if (uhel<-20 && uhel>-90  ||  uhel>20 && uhel<90)//přesně kolmo projde :D
                            uhel = 0;
                    projectilesy[projectilesy.Count - 1].smer = uhel * Math.PI/180;
                }
                else
                {
                    projectilesy[projectilesy.Count-1].smer = uhel * Math.PI / 180;
                    if(kdoStrili == Enemy.Typy.SWS) { 
                        int predmireni = 404;//neb
                        if (xM.Enabled)
                            predmireni = -predmireni;
                        else if (xM.Enabled && xP.Enabled  ||  !xM.Enabled && !xP.Enabled)
                            predmireni = 0;
                        if(kdoStrili == Enemy.Typy.SWS){
                            Xstrely += (int)(24*pomerovyKoeficient[0]); Ystrely -= (int)(16*pomerovyKoeficient[1]);
                            projectilesy.Add(new Projectile(Xstrely, Ystrely, typMunice));
                            projectilesy[projectilesy.Count - 1].smer = Math.Atan2((komora-Ystrely), (double)(poziceX-Xstrely+predmireni));
                            Xstrely -= (int)(16*pomerovyKoeficient[0]); Ystrely -= (int)(16*pomerovyKoeficient[1]);
                            projectilesy.Add(new Projectile(Xstrely, Ystrely, typMunice));
                            projectilesy[projectilesy.Count - 1].smer = Math.Atan2((komora - Ystrely), (double)(poziceX - Xstrely + predmireni/2));
                        }
                    }
                }
            }
        }


    }
}
