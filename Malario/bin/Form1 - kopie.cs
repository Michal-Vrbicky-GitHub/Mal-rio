using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Malario
{
    public partial class Form1 : Form
    {
        int[] velikostObrazovky;
        int cas = 0;
        int timerInterval = 20;
        Color cerna = Color.FromArgb(42, 42, 42, 42);
        Color bila = Color.FromArgb(213,213,213,213);
        Brush br = new SolidBrush(Color.FromArgb(111, 111, 111, 111));
        //Pen   pe = new Pen(bila);
        public static string solutionPath = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static Image[] obry = new Image[7];


        public Form1()
        {
            InitializeComponent();
            Uvod();
            /*timer1.Start();// timer1.Enabled = true;
            pictureBox1.Paint += PictureBox1_Paint;*/
        }

        private void timer1_Tick_1(object sender, EventArgs e){
            pictureBox1.Invalidate();}

        void timer1_Tick_Uvod(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            /*//Graphics g2 = pictureBox2.CreateGraphics(); Tak na co má picturebox alfu, když odmítá bejt průhlednej?
            if(cas <= 170*20){
                pictureBox2.BackColor = Color.FromArgb(85+cas/20, 0, 0, 0);//127}*/
            Pen pe = new Pen(bila);
            pe.Width = 5;
            if (cas == 0)
            {
                this.Location = new Point((velikostObrazovky[0]-this.Width)/2, (velikostObrazovky[1]-this.Height)/2);
                g.DrawImage(obry[0], 80, 6);
            }
            if(cas <= 170*20){
                pictureBox1.BackColor = Color.FromArgb(85+cas/20, 0, 0, 0);
            }/*
            if(cas >= 100)
            {
                g1.DrawRectangle(pe, Width/6, Height-(int)(Height/2.3), (int)(Width*(2d/3.0)), 42);
                //g2.DrawRectangle(new Pen(Color.FromArgb(255, 255, 0, 111)), Width / 6, Height - (int)(Height / 2.3), 42, (int)(Width * (2d / 3.0)));
            }*/

            
            cas += timerInterval;
        }

        private void Uvod()
        {
            string[] nameOfImage = { "Transmission electron micrograph of a SARS-CoV-2 coronavirus covered in spike proteins.png",
                "Plasmodium_falciparum.PNG", "The_malaria-infected_red_blood_cell.PNG" };
            for(int i = 0; i < nameOfImage.Length; i++)
            {
                obry[i] = Image.FromFile(solutionPath + "\\Pictures\\" + nameOfImage[i]);
            }
            //pictureBox2.BackgroundImage = obry[0];
            //NastaveniPozadi("");
            //pictureBox1.BringToFront();
            pictureBox2.SendToBack(); 
            velikostObrazovky = VelikostObrazovky();
            timer1.Interval = timerInterval;
            timer1.Tick += new System.EventHandler(this.timer1_Tick_Uvod);
            timer1.Start();
            

            /*
            timer1.Stop();
            timer1.Tick -= timer1_Tick_Uvod;
            timer1.Tick += new System.EventHandler(timer1_Tick_1);*/
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        int[] VelikostObrazovky()// Převzato ze hry života
        {
            int X = -1;
            string tuto = "";
            string tohle = Screen.PrimaryScreen.Bounds.ToString();//{X=0,Y=0,Width=1536,Height=864}
            byte tohleto = 0;
            foreach (char to in tohle){
                if(to == 'h')
                {
                    tohleto = 1;
                }
                else if(tohleto == 1 && to == '=')
                {
                    tohleto = 2;
                }
                else if (tohleto == 2 && to != ',' && to != '}')
                {
                    tuto += to;
                }
                else if (tohleto == 2 && to == ','  ||  tohleto == 2 && to == '}')
                {
                    if(X==-1){
                        X = int.Parse(tuto);
                        tuto = "";
                        tohleto = 0;
                    }else{
                        return new int[] { X, int.Parse(tuto) };
                    }
                }
            }
            return new int[] {-1,-1,-1};
        }


        void NastaveniPozadi(string obr)
        {
            pictureBox2.BackgroundImage = Image.FromFile(solutionPath + "\\Pictures\\" + obr);
        }


    }
}
