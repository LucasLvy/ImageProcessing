using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class CopieImageForm : Form
    {
        //Champs

        private bool userChangeLocation = true;

        private MyImage imageMove;
        private MyImage imageStatic;

        private int pbStaticHeight;
        private int pbStaticWidth;

        private double rapportImageStatic;
        private double rapportImageMove;

        private int imageStaticHeight;
        private int imageStaticWidth;

        private Point origineImage;
        private Point destinationImage;

        private int pbMoveHeight; //aussi = à la taille de l'image
        private int pbMoveWidth;

        private bool stretch = false;

        private int opacité = 100;

        //Propriété

        public int HeightImage { get => (int)(this.destinationImage.Y - this.origineImage.Y); }
        public int WidthImage { get => (int)(this.destinationImage.X - this.origineImage.X); }
        internal Point GetPointOrigine { get => this.origineImage; }
        internal MyImage GetImage { get => this.imageMove; }
        public int Opacité { get => this.opacité; }

        //Constructeur

        public CopieImageForm(MyImage im)
        {
            Cursor.Current = Cursors.WaitCursor;

            InitializeComponent();

            this.pB_Moving.Parent = this.pB_static;

            this.pB_Moving.Location = this.PointToScreen(this.pB_Moving.Location);

            this.pbStaticHeight = this.pB_static.Height;
            this.pbStaticWidth = this.pB_static.Width;

            this.imageStatic = im;
            this.pB_static.Image = im.ToBitmap();


        }


        //Image move init
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Program.ADRESSE_SAUVEGARDE;
            openFileDialog1.Filter = "Fichiers bitmap|*.bmp|Fichiers csv|*.csv|Fichiers bmp et csv|*.csv;*.bmp";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";


            DialogResult fichier = openFileDialog1.ShowDialog();
            string FileName = openFileDialog1.FileName;

            Cursor.Current = Cursors.WaitCursor;


            if (File.Exists(FileName) && fichier == DialogResult.OK)
            {
                this.imageMove = new MyImage(FileName);

                //Redimensionnement picturebox
                double ratioW = (double)this.pB_Moving.Width / this.imageMove.GetWidth;
                double ratioH = (double)this.pB_Moving.Height / this.imageMove.GetHeight;

                this.rapportImageMove = ratioH > ratioW ? ratioW : ratioH;

                int hauteurEnTrop = (int)Math.Ceiling(this.pB_Moving.Height - this.imageMove.GetHeight * this.rapportImageMove);
                int largeurEnTrop = (int)Math.Ceiling(this.pB_Moving.Width - this.imageMove.GetWidth * this.rapportImageMove);

                this.pB_Moving.Size = new Size(this.pB_Moving.Width - largeurEnTrop, this.pB_Moving.Height - hauteurEnTrop); //On réduit la taille du Form en largeur ou en hauteur

                int posX = (this.pB_static.Width - this.pB_Moving.Width) / 2; //this.pB_static.Location.X + 
                int posY = (this.pB_static.Height - this.pB_Moving.Height) / 2; //this.pB_static.Location.Y + 
                this.pB_Moving.Location = new System.Drawing.Point(posX, posY);

                this.changeLocation();

                this.pB_Moving.Visible = true;

                this.pB_Moving.Image = this.imageMove.ToBitmap();

                this.SaveButton.Enabled = this.buttonCentrer.Enabled = this.checkBoxEtirer.Enabled = this.textBoxOrigineX.Enabled =
                    this.textBoxDestinX.Enabled = this.textBoxDestinY.Enabled = this.textBoxOrigineY.Enabled = 
                    this.trackBarOpacité.Enabled = this.labelOpacité.Enabled = true;
                
            }


            Cursor.Current = Cursors.Default;
        }


        private System.Drawing.Point locationMouse = System.Drawing.Point.Empty;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.locationMouse = new System.Drawing.Point(e.X, e.Y);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.locationMouse != System.Drawing.Point.Empty)
            {
                this.pictureBoxChangeLocation(e.X - this.locationMouse.X, e.Y - this.locationMouse.Y);
                this.changeLocation();
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this.locationMouse = System.Drawing.Point.Empty;
        }


        private void pictureBoxChangeLocation(int x, int y)
        {
            System.Drawing.Point newLocation = this.pB_Moving.Location;

            int hauteurEnTrop = (int)((this.pbStaticHeight - this.imageStaticHeight * this.rapportImageStatic) / 2);
            int largeurEnTrop = (int)((this.pbStaticWidth - this.imageStaticWidth * this.rapportImageStatic) / 2);

            newLocation.X += x;

            if (newLocation.X >= largeurEnTrop && newLocation.X < this.pbStaticWidth - this.pbMoveWidth - largeurEnTrop)
            {
                this.pB_Moving.Location = newLocation;
            }
            else
                newLocation.X -= x;

            newLocation.Y += y;
            if (newLocation.Y >= hauteurEnTrop && newLocation.Y <= this.pbStaticHeight - this.pbMoveHeight - hauteurEnTrop)
            {
                this.pB_Moving.Location = newLocation;
            }
            
        }


        private void pictureboxMove_changesize(object sender, EventArgs e)
        {
            this.pbMoveHeight = this.pB_Moving.Height;
            this.pbMoveWidth = this.pB_Moving.Width;

            if (this.imageMove != null)
            {
                double ratioW = (double)this.pB_Moving.Width / this.imageMove.GetWidth;
                double ratioH = (double)this.pB_Moving.Height / this.imageMove.GetHeight;

                this.rapportImageMove = ratioH < ratioW ? ratioW : ratioH;

                if (!this.stretch)
                {
                    int hauteurEnTrop = (int)Math.Ceiling(this.pB_Moving.Height - this.imageMove.GetHeight * this.rapportImageMove);
                    int largeurEnTrop = (int)Math.Ceiling(this.pB_Moving.Width - this.imageMove.GetWidth * this.rapportImageMove);

                    this.pB_Moving.Size = new Size(this.pB_Moving.Width - largeurEnTrop, this.pB_Moving.Height - hauteurEnTrop); //On réduit la taille du Form en largeur ou en hauteur

                }

                this.changeLocation();
            }

        }

        private void pictureboxStatic_changesize(object sender, EventArgs e)
        {
            this.pbStaticHeight = this.pB_static.Height;
            this.pbStaticWidth = this.pB_static.Width;

            if (this.imageStatic != null)
            {
                double ratioW = (double)this.pB_static.Width / this.imageStatic.GetWidth;
                double ratioH = (double)this.pB_static.Height / this.imageStatic.GetHeight;

                this.rapportImageStatic = ratioH > ratioW ? ratioW : ratioH;
                this.changeLocation();
            }

        }


        private void checkBoxEtirer_CheckedChanged(object sender, EventArgs e)
        {
            this.stretch = this.checkBoxEtirer.Checked;

            if (!this.stretch)
            {
                int hauteurEnTrop = (int)Math.Ceiling(this.pB_Moving.Height - this.imageMove.GetHeight * this.rapportImageMove);
                int largeurEnTrop = (int)Math.Ceiling(this.pB_Moving.Width - this.imageMove.GetWidth * this.rapportImageMove);

                this.pB_Moving.Size = new Size(this.pB_Moving.Width - largeurEnTrop, this.pB_Moving.Height - hauteurEnTrop); //On réduit la taille du Form en largeur ou en hauteur
            }
            else
            {
                this.pB_Moving.SizeMode = PictureBoxSizeMode.StretchImage;
            }

        }


        private void trackBarOpacité_Scroll(object sender, EventArgs e)
        {
            this.opacité = this.trackBarOpacité.Value;
            this.labelOpacité.Text = "Opacité : " + this.opacité.ToString();
            Bitmap bmp = this.imageMove.ToBitmap();

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    bmp.SetPixel(i, j, Color.FromArgb(this.opacité * 255 / 100, bmp.GetPixel(i, j)));
                }
            }
            
            this.pB_Moving.Image = bmp;
            
        }


        private void changeLocation()
        {
            this.userChangeLocation = false;

            if (this.imageMove != null)
            {
                this.origineImage.X = (this.pB_Moving.Location.X - ((this.pbStaticWidth - this.imageStaticWidth * this.rapportImageStatic) / 2)) / this.rapportImageStatic;
                this.origineImage.Y = (this.pB_Moving.Location.Y - ((this.pbStaticHeight - this.imageStaticHeight * this.rapportImageStatic) / 2)) / this.rapportImageStatic;

                this.destinationImage.X = (this.pB_Moving.Location.X - ((this.pbStaticWidth - this.imageStaticWidth * this.rapportImageStatic) / 2) + this.pbMoveWidth) / this.rapportImageStatic;
                this.destinationImage.Y = (this.pB_Moving.Location.Y - ((this.pbStaticHeight - this.imageStaticHeight * this.rapportImageStatic) / 2) + this.pbMoveHeight) / this.rapportImageStatic;

                this.textBoxOrigineX.Text = ((int)this.origineImage.X).ToString();
                this.textBoxOrigineY.Text = ((int)this.origineImage.Y).ToString();
                this.textBoxDestinX.Text = ((int)this.destinationImage.X).ToString();
                this.textBoxDestinY.Text = ((int)this.destinationImage.Y).ToString();
            }

            this.userChangeLocation = true;
        }


        private void CopieImageForm_Load(object sender, EventArgs e)
        {
            double ratioW = (double)this.pB_static.Width / this.imageStatic.GetWidth;
            double ratioH = (double)this.pB_static.Height / this.imageStatic.GetHeight;

            this.rapportImageStatic = ratioH > ratioW ? ratioW : ratioH;

            this.imageStaticHeight = this.imageStatic.GetHeight;
            this.imageStaticWidth = this.imageStatic.GetWidth;

            this.labelHeight.Text += this.imageStatic.GetHeight.ToString();
            this.labelWidth.Text += this.imageStatic.GetWidth.ToString();

            this.userChangeLocation = false;
            this.textBoxOrigineX.Text = this.textBoxDestinX.Text = this.textBoxDestinY.Text = this.textBoxOrigineY.Text = "///";
            this.userChangeLocation = true;

            this.buttonHelper.ImageAlign = ContentAlignment.MiddleCenter;
            this.buttonHelper.Image = System.Drawing.Bitmap.FromHicon(System.Drawing.SystemIcons.Information.Handle);


            Cursor.Current = Cursors.Default;
        }


        #region Text

        private void textBoxOrigineX_TextChanged(object sender, EventArgs e)
        {
            if (this.userChangeLocation)
            {
                TextChangeLocation();
            }
        }

        private void textBoxOrigineY_TextChanged(object sender, EventArgs e)
        {
            if (this.userChangeLocation)
            {
                TextChangeLocation();
            }
        }

        private void textBoxDestinX_TextChanged(object sender, EventArgs e)
        {
            if (this.userChangeLocation)
            {
                TextChangeLocation();
            }
        }

        private void textBoxDestinY_TextChanged(object sender, EventArgs e)
        {
            if (this.userChangeLocation)
            {
                TextChangeLocation();
            }
        }


        private void TextChangeLocation()
        {
            int posUserX = int.TryParse(this.textBoxOrigineX.Text, out int val) ? (int)((val + 1) * this.rapportImageStatic + ( (this.pbStaticWidth - this.imageStaticWidth * this.rapportImageStatic) / 2)) : this.pB_Moving.Location.X;
            int posUserY = int.TryParse(this.textBoxOrigineY.Text, out int val2) ? (int)((val2 + 1) * this.rapportImageStatic + ( (this.pbStaticHeight - this.imageStaticHeight * this.rapportImageStatic) / 2)) : this.pB_Moving.Location.Y;

            double x = -this.pB_Moving.Location.X + posUserX;
            double y = -this.pB_Moving.Location.Y + posUserY;

            this.pictureBoxChangeLocation((int)x, (int)y);

            this.origineImage.X = (this.pB_Moving.Location.X - ( (this.pbStaticWidth - this.imageStaticWidth * this.rapportImageStatic) / 2)) / this.rapportImageStatic;
            this.origineImage.Y = (this.pB_Moving.Location.Y - ( (this.pbStaticHeight - this.imageStaticHeight * this.rapportImageStatic) / 2)) / this.rapportImageStatic;

            this.destinationImage.X = (this.pB_Moving.Location.X - ( (this.pbStaticWidth - this.imageStaticWidth * this.rapportImageStatic) / 2) + this.pbMoveWidth) / this.rapportImageStatic;
            this.destinationImage.Y = (this.pB_Moving.Location.Y - ( (this.pbStaticHeight - this.imageStaticHeight * this.rapportImageStatic) / 2) + this.pbMoveHeight) / this.rapportImageStatic;


            this.userChangeLocation = false;

            this.textBoxOrigineX.Text = ((int)this.origineImage.X).ToString();
            this.textBoxOrigineY.Text = ((int)this.origineImage.Y).ToString();
            this.textBoxDestinX.Text = ((int)this.destinationImage.X).ToString();
            this.textBoxDestinY.Text = ((int)this.destinationImage.Y).ToString();
            
            this.userChangeLocation = true;
        }


        private void taille_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion


        private void buttonHelper_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pour copier une image dans l'image ci-contre :\n\n" +
                "    - Choisissez une image à copier en cliquant sur le bouton pour ouvrir une image (vous pouvez changer d'image à copier à volonté)\n" +
                "    - L'image va apparaitre en petit au dessus de l'image sur laquelle la copier\n" +
                "    - Vous pouvez déplacer l'image à copier en cliquant dessus ainsi qu'en modifier la taille\n" +
                "    - Pour modifier la taille : déplacer le curseur en bas à droite de l'image à copier (le mode d'aggrandissement par" +
                @" défaut conserve les proportions de l'image, vous pouvez modifier cela en cochant la case ""Etirer l'image"")" + "\n\n" +
                @"    - Une fois que l'emplacement vous satisfait, cliquez sur le bouton ""Copier"" pour activer les changements",
                "Comment copier une image dans une autre ?", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void buttonCentrer_Click(object sender, EventArgs e)
        {
            int posX = (this.pB_static.Width - this.pB_Moving.Width) / 2;
            int posY = (this.pB_static.Height - this.pB_Moving.Height) / 2;

            this.pB_Moving.Location = new System.Drawing.Point(posX, posY);

            this.changeLocation();
        }

    }
}
