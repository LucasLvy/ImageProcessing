﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class RognageImage : Form
    {
        private MyImage image;

        private bool b_sourisDown = false;

        private double rapportChgmtTaille = 1;

        private System.Drawing.Point origine = new System.Drawing.Point(-1, -1);
        private System.Drawing.Point dest = new System.Drawing.Point(-1, -1);

        public Point realOrigine = new Point(0, 0);
        public Point realDest;

        private Rectangle rognageAire = new Rectangle();


        public RognageImage(MyImage image)
        {
            this.image = image;
            this.realDest = new Point(image.GetHeight, image.GetWidth);
            InitializeComponent();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            dest.X = -1;
            dest.Y = -1;
            origine.X = -1;
            origine.Y = -1;

            this.b_sourisDown = true;
            
            origine.X = e.X;
            origine.Y = e.Y;

            this.realOrigine.X = (int)(origine.X / this.rapportChgmtTaille);
            this.realOrigine.Y = (int)(origine.Y / this.rapportChgmtTaille);
            

            dest.X = -1;
            dest.Y = -1;

            origineXPtLab.Text = "X :" + ((int)this.realOrigine.X).ToString();
            origineYPtLab.Text = "Y :" + ((int)this.realOrigine.Y).ToString();

            this.rognageAire = new Rectangle(new System.Drawing.Point(e.X, e.Y), new Size());

            Pen style = new Pen(Brushes.Black, 2)
            {
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dot
            };
            this.pictureBox1.CreateGraphics().DrawRectangle(style, rognageAire);
            this.pictureBox1.Refresh();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.b_sourisDown = false;
            
            if (Math.Abs(origine.X - dest.X) > 5 && Math.Abs(origine.Y - dest.Y) > 5 && dest.X != -1)
            {
                this.realDest = new Point((int)(e.Y / this.rapportChgmtTaille), (int)(e.X / this.rapportChgmtTaille));

                this.DialogResult = DialogResult.Yes;
            }
            
            dest.X = -1;
            dest.Y = -1;
            origine.X = -1;
            origine.Y = -1;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.b_sourisDown)
            {
                if (dest.X != -1)
                {
                    this.destinationXPtLab.Text = "X :" + ((int)this.realDest.X).ToString();
                    this.destinationYPtLab.Text = "Y :" + ((int)this.realDest.Y).ToString();
                }

                dest = new System.Drawing.Point(e.X, e.Y); ;
                this.realDest.X = dest.X / this.rapportChgmtTaille;
                this.realDest.Y = dest.Y / this.rapportChgmtTaille;

                if (e.X > origine.X && e.Y > origine.Y)
                {
                    this.rognageAire.Width = e.X - origine.X;

                    this.rognageAire.Height = e.Y - origine.Y;
                }
                else if (e.X < origine.X && e.Y > origine.Y)
                {
                    this.rognageAire.Width = origine.X - e.X;
                    this.rognageAire.Height = e.Y - origine.Y;
                    this.rognageAire.X = e.X;
                    this.rognageAire.Y = origine.Y;
                }
                else if (e.X > origine.X && e.Y < origine.Y)
                {
                    this.rognageAire.Width = e.X - origine.X;
                    this.rognageAire.Height = origine.Y - e.Y;

                    this.rognageAire.X = origine.X;
                    this.rognageAire.Y = e.Y;
                }
                else
                {
                    this.rognageAire.Width = origine.X - e.X;

                    this.rognageAire.Height = origine.Y - e.Y;
                    this.rognageAire.X = e.X;
                    this.rognageAire.Y = e.Y;
                }
                Brush brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));

                Pen style = new Pen(Brushes.Black, 1)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot,
                    DashCap = System.Drawing.Drawing2D.DashCap.Flat,
                    DashOffset = 0.5f
                };

                this.pictureBox1.Refresh();

                this.pictureBox1.CreateGraphics().FillRectangle(brush, rognageAire);
                this.pictureBox1.CreateGraphics().DrawRectangle(style, rognageAire);

            }
        }



        private void RognageImage_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            this.destinationXPtLab.Text = "X :" + (this.image.GetWidth - 1).ToString();
            this.destinationYPtLab.Text = "Y :" + (this.image.GetHeight - 1).ToString();

            if (this.image.GetHeight != this.pictureBox1.Height || this.image.GetWidth != this.pictureBox1.Width) //Change de taille l'image pour "fit" la picturebox
            {
                double ratioW = (double)this.pictureBox1.Width / this.image.GetWidth;
                double ratioH = (double)this.pictureBox1.Height / this.image.GetHeight;

                this.rapportChgmtTaille = ratioH > ratioW ? ratioW : ratioH;

                int hauteurEnTrop = (int)Math.Ceiling(this.pictureBox1.Height - this.image.GetHeight * rapportChgmtTaille);
                int largeurEnTrop = (int)Math.Ceiling(this.pictureBox1.Width - this.image.GetWidth * rapportChgmtTaille);

                this.Size = new Size(this.Width - largeurEnTrop, this.Height - hauteurEnTrop); //On réduit la taille du Form en largeur ou en hauteur

                MyGraphics g = new MyGraphics(this.image.GetCopie())
                {
                    Quality = InterpolationMode.Bicubique
                };
                g.Redimensionnement(rapportChgmtTaille); //On change la taille de l'image

                this.pictureBox1.Image = g.GetMyImage.ToBitmap(); 
            }
            else
                this.pictureBox1.Image = this.image.ToBitmap();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Cursor.Current = Cursors.Default;
        }
    }
}
