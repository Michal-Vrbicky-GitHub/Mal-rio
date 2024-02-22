using Malario.MapObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Malario
{
    internal class DataHandl
    {
        string cesta;
        string saveFileName;

        public bool uzMuzes;
        public int[] velikostMapy = new int[2];
        public int[] pocatecniSouradnice = new int[2];
        public MapObjects.Pozadi pozadi;
        public List<MapObjects.Obdélník>          rectanglesy       = new List<MapObjects.Obdélník> { };
        public List<MapObjects.ObdelnikBezKolize> obdelnikove       = new List<MapObjects.ObdelnikBezKolize> { };
        public List<MapObjects.Ladder>            zebraci           = new List<MapObjects.Ladder>();
        public List<MapObjects.Smileyface>        xychti            = new List<MapObjects.Smileyface>();
        public List<MapObjects.Obrazek>           obrazeky          = new List<MapObjects.Obrazek>();
        public List<MapObjects.Enemy>             enemaci           = new List<MapObjects.Enemy>();
        public List<MapObjects.Mina>              mini              = new List<MapObjects.Mina>();
        public List<MapObjects.Spike>             bodaky            = new List<MapObjects.Spike>();
        public List<MapObjects.Trap>              pasti             = new List<MapObjects.Trap>();
        public List<MapObjects.Otaznik>           otaznikovyCtverce = new List<MapObjects.Otaznik>();
        public List<MapObjects.OneTile>           ctverce           = new List<MapObjects.OneTile>();
        public List<MapObjects.InterestingPlaces> kraviny           = new List<MapObjects.InterestingPlaces>();
        public List<MapObjects.Napis>             kecy              = new List<MapObjects.Napis> { };
        public Image[] ruznyDalsiObry;
        public Image[] invulka;

        public List<string> NacteniSaveSlotu()//1. číslo: 0 je empty, 1-3 obtížnosti, 2. je poslední dokončená mapa
        {
            saveFileName = cesta+"\\Savy\\SaveFile";
            List<string> sloty = new List<string>();
            if (File.Exists(saveFileName))
            {
                using (StreamReader sr = new StreamReader(saveFileName))
                {
                    string radka;
                    while ((radka = sr.ReadLine()) != null)
                    {
                        sloty.Add(radka);
                    }
                }
            }
            else
            {
                using (StreamWriter ssw = new StreamWriter(saveFileName))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        ssw.WriteLine("00");
                        sloty.Add   ("00");
                    }
                }
            }
            return sloty;
            //return NacteniSaveSlotu();/**/hmmmmmmmmmmm
        }

        public void Save(string cisloObtiznostMapa)
        {
            string[] savy = File.ReadAllLines(saveFileName);
            savy[int.Parse(cisloObtiznostMapa[0].ToString())] = cisloObtiznostMapa[1].ToString()+cisloObtiznostMapa[2]+"";
            File.WriteAllLines(saveFileName, savy);
        }

        public void NacteniMapy(int lvl)
        {//MapObjects.MapObject mo = new MapObjects.MapObject();
            VymazniMapu();

            string soubor = cesta + "\\Mapy\\" + lvl.ToString() + ".map";
            if (File.Exists(soubor))
            {
                using (StreamReader sr = new StreamReader(soubor, Encoding.GetEncoding("Windows-1250")))
                {
                    bool loob = false;
                    string radek;
                    while ((radek = sr.ReadLine()) != null  &&  (radek != "" || !loob))
                    {
                        if (loob)
                        {
                            if (radek.ToCharArray()[0] == '*'){
                                continue;
                            }
                            else if (radek == "X"){
                                velikostMapy[0] = KouzleniSeSouradnicema(sr.ReadLine(), 'X');
                            }
                            else if (radek == "Y"){
                                velikostMapy[1] = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                            }
                            else if (radek == "pozadi"){//
                                Image i = NacteniObrazku("ObjektyNaMape\\" + sr.ReadLine());//"\\\\ObjektyNaMape\\" + sr.ReadLine()); vůbec mu neva víc lomen
                                double rat = Screen.PrimaryScreen.Bounds.Height / (double)i.Height;
                                pozadi = new MapObjects.Pozadi(new Bitmap(i, (int)((double)i.Width*rat), Screen.PrimaryScreen.Bounds.Height));//
                            }
                            else if (radek == "Malário"){
                                pocatecniSouradnice[0] = KouzleniSeSouradnicema(sr.ReadLine(), 'X');  
                                pocatecniSouradnice[1] = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                            }
                            else if (radek == "SolidBlok"){
                              //rectanglesy.Add(new MapObjects.Obdélník         (int.Parse(sr.ReadLine()), int.Parse(sr.ReadLine()), new int[] { int.Parse(sr.ReadLine()) , int.Parse(sr.ReadLine()) }, sr.ReadLine()));
                                rectanglesy.Add(new MapObjects.Obdélník         (KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y'), new int[] { KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y') }, sr.ReadLine()));
                            }
                            else if (radek == "OBD"){
                                obdelnikove.Add(new MapObjects.ObdelnikBezKolize(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y'), new int[] { KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y') }, sr.ReadLine(), bool.Parse(sr.ReadLine())));
                            }
                            else if (radek == "žebřik"){
                                zebraci.Add(new MapObjects.Ladder               ());
                                zebraci[zebraci.Count - 1].X     = KouzleniSeSouradnicema(sr.ReadLine(), 'X');
                                zebraci[zebraci.Count - 1].Y     = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                                zebraci[zebraci.Count - 1].height= KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                            }
                            else if (radek == "smajla"){
                                xychti.Add(new MapObjects.Smileyface());
                                xychti[xychti.Count - 1].X = KouzleniSeSouradnicema(sr.ReadLine(), 'X');
                                xychti[xychti.Count - 1].Y = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                            }
                            else if (radek == "img"){
                                int x = KouzleniSeSouradnicema(sr.ReadLine(), 'X'), y = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                                obrazeky.Add(new MapObjects.Obrazek(NacteniObrazku("ObjektyNaMape\\" + sr.ReadLine())));
                                obrazeky[obrazeky.Count - 1].X = x;
                                obrazeky[obrazeky.Count - 1].Y = y;
                            }
                            else if (radek == "SocksInSandals"){
                                enemaci.Add(new MapObjects.Enemy(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y'), MapObjects.Enemy.Typy.SWS,
                                                                 KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'X')));
                            }
                            else if (radek == "Tonk"){
                                enemaci.Add(new MapObjects.Enemy(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y'), MapObjects.Enemy.Typy.Tonk));
                            }
                            else if (radek == "Áďa"){
                                enemaci.Add(new MapObjects.Enemy(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y'), MapObjects.Enemy.Typy.Hitler));
                            }
                            else if (radek == "BOOM"){
                                mini  .Add(new MapObjects.Mina (KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y')));
                            }
                            else if (radek == "bodák"  ||  radek == "falešnej bodák"){
                                bodaky.Add(new MapObjects.Spike(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y')));
                                if(radek == "falešnej bodák")
                                    bodaky[bodaky.Count-1].fake = true;
                            }
                            else if (radek == "skrytej bodák"){
                                pasti .Add(new MapObjects.Trap (KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y')));
                            }
                            else if (radek == "???"){
                                int X = KouzleniSeSouradnicema(sr.ReadLine(), 'X'), Y = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                                otaznikovyCtverce.Add(new MapObjects.Otaznik(X, Y));
                                rectanglesy.Add(new MapObjects.Obdélník(KouzleniSeSouradnicema("110", 'X'), KouzleniSeSouradnicema("123", 'Y'), new int[] { X, Y }, "000,000,000,000"));
                            }
                            else if (radek == "ctverec"){
                                int X = KouzleniSeSouradnicema(sr.ReadLine(), 'X'), Y = KouzleniSeSouradnicema(sr.ReadLine(), 'Y');
                                ctverce.Add(new MapObjects.OneTile(X, Y));
                                rectanglesy.Add(new MapObjects.Obdélník(KouzleniSeSouradnicema("88", 'X'), KouzleniSeSouradnicema("99", 'Y'), new int[] { X-8, Y-8 }, "255,255,255,000"));
                            }
                            else if (radek == "INVULNERABILITY"  ||  radek == "heel" ||  radek == "kanec")
                            {
                                string objNaMape = "ObjektyNaMape\\";
                                kraviny.Add(new MapObjects.InterestingPlaces(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y')));
                                switch (radek)
                                {
                                    case "kanec":
                                        kraviny[kraviny.Count - 1].co = InterestingPlaces.veci.Vlajka;
                                        kraviny[kraviny.Count - 1].imagy = new Image[] { new Bitmap(NacteniObrazku(objNaMape+"F lag.png"), NacteniObrazku("ObjektyNaMape\\F lag.png").Width/3, NacteniObrazku($"{objNaMape}F lag.png").Height / 3) };//velice optimální
                                        break;
                                    case "heel":
                                        kraviny[kraviny.Count - 1].co = InterestingPlaces.veci.Heal;
                                        kraviny[kraviny.Count - 1].imagy = new Image[] {NacteniObrazku(objNaMape + "Hýl.png") };
                                        break;
                                    case "INVULNERABILITY":
                                        if(invulka==null)
                                            invulka = new Image[] { //dýlej
                                                NacteniObrazku(objNaMape+"frame_0_delay-0.2s.gif"), NacteniObrazku(objNaMape + "frame_1_delay-0.2s.gif"), 
                                                NacteniObrazku(objNaMape+"frame_2_delay-0.2s.gif"), NacteniObrazku(objNaMape + "frame_3_delay-0.2s.gif") };
                                        kraviny[kraviny.Count - 1].co = InterestingPlaces.veci.nezranitelnost;
                                        kraviny[kraviny.Count - 1].imagy = invulka;
                                        break;
                                }
                            }
                            else if (radek == "string"){
                                string rgb = sr.ReadLine();
                                int[] RGB = { int.Parse(rgb[0]+rgb[1].ToString()+rgb[2]), int.Parse(rgb[3] + rgb[4].ToString() + rgb[5]), int.Parse(rgb[6] + rgb[7].ToString() + rgb[8]) };
                                kecy.Add(new MapObjects.Napis(KouzleniSeSouradnicema(sr.ReadLine(), 'X'), KouzleniSeSouradnicema(sr.ReadLine(), 'Y'), sr.ReadLine(), Color.FromArgb(RGB[0], RGB[1], RGB[2]), KouzleniSeSouradnicema(sr.ReadLine(), 'Y')));
                            }
                            else if(radek == "obt2"  ||  radek == "obt3") { 
                                if (radek[3] == '2'  &&  Form1.obtiznost < 2   ||   radek[3] == '3'  &&  Form1.obtiznost < 3)
                                    break;
                            }
                            else
                            {
                                MessageBox.Show("Na mapě je nějakej bordel, co nevim co je: "+radek);//vINVULNERABILITY
                            }
                        }
                        else
                            if(radek=="*")
                                loob = true;
                    }
                }
                foreach(Enemy emak in enemaci)
                    switch(emak.typ){
                        case Enemy.Typy.SWS:
                            emak.health = 1;
                            break;
                        case Enemy.Typy.Tonk:
                            emak.health = 4;
                            break;
                        case Enemy.Typy.Hitler:
                            emak.health = 666;
                            break;
                    }
                uzMuzes = true;
            }
            else
            {
                MessageBox.Show("Nemam mapu");
                MessageBox.Show("Chybí "+soubor);
            }
            if(ruznyDalsiObry == null)
            {
                //string[] jmena_strel = { "killer_tomato_by_tarynsgate_d24mqj6-fullview.jpg__rmvbckgrnd", "shít", "XM1147 Advanced Multi-Purpose   (4)" };
                string[] dva = { "TROLL0.png", "TONK-removebg.png" , "Robohitler.png", "Anime_Pastel_Dream_explosion_in_8bit_game_6.jpeg image-removebg.png", "D E A D.png" };
                List<string> jmena = new List<string>{ "Minecraft_Ladder.png", "hi13.gif", "hi2.gif", "hi4.gif"};
                List<Image> imagesy = new List<Image>();
                for (int i = 1; i <= 9; i++){
                    if(i < 5)
                        jmena.Add($"Enemaci\\Troll{i}.PNG");
                    else
                        jmena.Add($"Enemaci\\{dva[i-5]}");
                }
                for (int i = 0; i < 4; i++){
                    jmena[i] = "ObjektyNaMape\\"+jmena[i];
                }                                                                   
                jmena.AddRange( new List<string> { "Srdicko.png", "Šipka.pngr" , "5e67e0b1df1e5e5e564f6799d656ffffd7da.png", "Explosion-8.png", "Mario_Džugašvili-removebg-preview.png", "PPSH-41.Png" });//\\
                /*for (int i = 0; i < jmena_strel.Length; i++){
                    jmena.Add(string.Format(@"Projektily\{0}.png", jmena_strel[i]));
                }*/jmena.AddRange( new List<string> { "ObjektyNaMape\\1Tile.png", "ObjektyNaMape\\OtaznikovejČtverec.Png", "ObjektyNaMape\\Špica.png"/*, "ObjektyNaMape\\"*/ });
                foreach (string jmeno in jmena)
                {
                    imagesy.Add(NacteniObrazku(jmeno));
                }
                imagesy[0] = new Bitmap(imagesy[0], new Size(imagesy[0].Width/4, imagesy[0].Height/4));//zapomněl sem ho zmenčit, tak tady
                imagesy[14].RotateFlip(RotateFlipType.RotateNoneFlipY);//zapomněl sem otočit
                imagesy[15] = new Bitmap(imagesy[15], new Size(imagesy[15].Width*11/6, imagesy[15].Height*11/6));//zvětšit
                ruznyDalsiObry = imagesy.ToArray();
            }
        }//
        //;

        private Image NacteniObrazku(string kdepa/*, bool resize*/)
        {
            double koe = 1;
            if (kdepa.Contains("_delay-0.2s.gif")){
                koe = 1 / 5.0;
                //kdepa += "ObjektyNaMape\\";
            }
            Image img = Image.FromFile(this.cesta + "\\Pictures\\Textury\\" + kdepa);
            return new Bitmap(img, (int)(img.Width*Form1.pomerovyKoeficient[0]*koe), (int)(img.Height*Form1.pomerovyKoeficient[1]*koe));
        }

        public Image[] NacteniObruDoMovementSchemyATakyDootaAJesteProjektilu(string M_nebo_D)
        {
            List<Image> mariove = new List<Image>();
            string[] nazvySouboru = Directory.GetFiles(this.cesta+"\\Pictures\\Textury\\"+M_nebo_D).Select(Path.GetFileName).ToArray();

            // Výpis názvů souborů
            foreach (string nazevSouboru in nazvySouboru)
            {
                mariove.Add(NacteniObrazku($"{M_nebo_D}\\{nazevSouboru}"));
            }
            return mariove.ToArray();
        }

        int KouzleniSeSouradnicema(string souradnice, char vyskaCiSirka)
        {
            int vIntu = int.Parse(souradnice);
            switch (vyskaCiSirka)
            {
                case 'X':
                    return (int)(vIntu*Form1.pomerovyKoeficient[0]);
                case 'Y':
                    return (int)(vIntu*Form1.pomerovyKoeficient[1]);
            }
            return -2147483647;
        }

        public void VymazniMapu()//
        {
            pozadi = null;// : base(nazev)
            rectanglesy.Clear(); obdelnikove.Clear(); zebraci.Clear(); xychti.Clear(); obrazeky.Clear(); enemaci.Clear();
            mini.Clear(); bodaky.Clear(); pasti.Clear(); otaznikovyCtverce.Clear(); ctverce.Clear(); kecy.Clear(); kraviny.Clear();
        }

        public DataHandl(string cesta)
        {
            this.cesta = cesta;
        }
    }
}
