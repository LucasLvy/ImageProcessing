using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class CouleurFiltre : Form
    {
        public int GrisIntensité = 0;
        public int LumChgmtIntensité = 50;
        public int CouleurChgmtIntensité = 100;

        public bool NoirEtBlanc = false;
        public bool Rouge = false;
        public bool Vert = false;
        public bool Bleu = false;
        public bool Sépia = false;
        public bool transfoLum = false;
        public bool transfoCouleur = false;

        public bool inversion = false;

        /// <summary>
        /// Détermine si c'est bien l'utilisateur qui a changé les index
        /// </summary>
        private bool userListBoxChange = false;

        private int indexTrackbarTab = 0;


        public CouleurFiltre()
        {
            InitializeComponent();
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int lastSelectedIndex = (int)typeof(ListBox).GetProperty("FocusedIndex", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this.listBox1, null);

            if (this.listBox1.SelectedIndices.Count == 0)
            {
                this.OkButton.Enabled = false;
            }
            else if (this.userListBoxChange)
            {
                this.OkButton.Enabled = true;
                this.userListBoxChange = false; //Sinon on rentre dans une boucle infinie à chaque chgmt de selections

                if (lastSelectedIndex == 0 && this.listBox1.GetSelected(0))
                {
                    this.listBox1.SetSelected(1, false);
                    this.listBox1.SetSelected(2, false);
                    this.listBox1.SetSelected(3, false);
                    this.listBox1.SetSelected(0, true);
                }
                else if (lastSelectedIndex < 4 || !this.listBox1.GetSelected(0))
                {
                    this.listBox1.SetSelected(0, false);
                }
            }
            this.transfoLum = this.listBox1.GetSelected(6);
            this.NoirEtBlanc = this.listBox1.GetSelected(0);
            this.transfoCouleur = this.listBox1.GetSelected(7);

            if (this.listBox1.GetSelected(6) && lastSelectedIndex == 6)
                LuminoIndex();
            if (this.listBox1.GetSelected(0) && lastSelectedIndex == 0)
                GrisIndex();
            if (this.listBox1.GetSelected(7) && lastSelectedIndex == 7)
                CouleurIndex();

            this.gristrackbar.Enabled = this.indexTrackbarTab == 0 ? this.NoirEtBlanc : this.indexTrackbarTab == 1 ? this.transfoLum : this.transfoCouleur;
            this.textBoxGris.Enabled = this.indexTrackbarTab == 0 ? this.NoirEtBlanc : this.indexTrackbarTab == 1 ? this.transfoLum : this.transfoCouleur;
            this.lab0.Enabled = this.indexTrackbarTab == 0 ? this.NoirEtBlanc : this.indexTrackbarTab == 1 ? this.transfoLum : this.transfoCouleur;
            this.lab100.Enabled = this.indexTrackbarTab == 0 ? this.NoirEtBlanc : this.indexTrackbarTab == 1 ? this.transfoLum : this.transfoCouleur;
            this.labIntensité.Enabled = this.indexTrackbarTab == 0 ? this.NoirEtBlanc : this.indexTrackbarTab == 1 ? this.transfoLum : this.transfoCouleur;
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this.userListBoxChange = true;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.gristrackbar.Value = int.TryParse(this.textBoxGris.Text, out int value) ? value < 0 ? 0 : value > 100 ? 100 : value : 0;
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.textBoxGris.Text = this.gristrackbar.Value.ToString();

            if (this.indexTrackbarTab == 0)
                this.GrisIntensité = this.gristrackbar.Value;
            else if (this.indexTrackbarTab == 1)
                this.LumChgmtIntensité = this.gristrackbar.Value;
            else if(this.indexTrackbarTab == 2)
                this.CouleurChgmtIntensité = this.gristrackbar.Value;
        }


        private void CouleurFiltre_Load(object sender, EventArgs e)
        {
            this.OkButton.Enabled = false;
            this.icoLab.Image = Bitmap.FromHicon(SystemIcons.Information.Handle);
            this.gristrackbar.Enabled = false;
            this.textBoxGris.Enabled = false;
            this.lab0.Enabled = false;
            this.lab100.Enabled = false;
            this.labIntensité.Enabled = false;
        }


        private void OkButton_Click(object sender, EventArgs e)
        {
            this.NoirEtBlanc = this.listBox1.GetSelected(0);
            this.Rouge = this.listBox1.GetSelected(1);
            this.Vert = this.listBox1.GetSelected(2);
            this.Bleu = this.listBox1.GetSelected(3);
            this.inversion = this.listBox1.GetSelected(4);

            this.Sépia = this.listBox1.GetSelected(5);
            this.transfoLum = this.listBox1.GetSelected(6);
        }

        private void OkButton_EnabledChanged(object sender, EventArgs e)
        {
            this.OkButton.BackColor = this.OkButton.BackColor == Color.LimeGreen ? Color.FromArgb(80, 100, 80) : Color.LimeGreen;
        }



        private void button1_Click(object sender, EventArgs e) //forwards
        {
            if (this.indexTrackbarTab == 0)
                LuminoIndex();
            else if (this.indexTrackbarTab == 1)
                CouleurIndex();
        }
        

        private void GrisIndex()
        {
            this.indexTrackbarTab = 0;
            this.buttonBack.Enabled = false;
            this.buttonForward.Enabled = true;
            this.labIntensité.Text = "Noir et Blanc intensité";
            this.textBoxGris.Text = this.GrisIntensité.ToString();

            this.toolTip1.SetToolTip(this.gristrackbar, "Transforme les pixels en des nuances de gris, de noir et de blanc " +
                "en fonction d'un facteur %.\r\n100 % revient à changer la photo qu'en noir et qu'en blanc," +
                "\r\n0 % revient à mettre la photo en nuance de gris normalement");

            this.toolTip1.SetToolTip(this.textBoxGris, "Transforme les pixels en des nuances de gris, de noir et de blanc " +
                "en fonction d'un facteur %.\r\n100 % revient à changer la photo qu'en noir et qu'en blanc," +
                "\r\n0 % revient à mettre la photo en nuance de gris normalement");

            this.gristrackbar.Enabled = this.NoirEtBlanc;
            this.textBoxGris.Enabled = this.NoirEtBlanc;
            this.lab0.Enabled = this.NoirEtBlanc;
            this.lab100.Enabled = this.NoirEtBlanc;
            this.labIntensité.Enabled = this.NoirEtBlanc;
        }

        private void LuminoIndex()
        {
            this.indexTrackbarTab = 1;
            this.buttonForward.Enabled = true;
            this.buttonBack.Enabled = true;
            this.labIntensité.Text = "  Luminosité intensité";
            this.textBoxGris.Text = this.LumChgmtIntensité.ToString();

            this.toolTip1.SetToolTip(this.gristrackbar, "Transforme la luminosité de l'image.\r\nOn définit 50% comme " +
                "étant la luminosité actuelle.\r\n0 % revient à mettre l'image en noir.\r\n100 % revient à la changer en blanc.");

            this.toolTip1.SetToolTip(this.textBoxGris, "Transforme la luminosité de l'image.\r\nOn définit 50% comme " +
                "étant la luminosité actuelle.\r\n0 % revient à mettre l'image en noir.\r\n100 % revient à la changer en blanc.");

            this.gristrackbar.Enabled = this.transfoLum;
            this.textBoxGris.Enabled = this.transfoLum;
            this.lab0.Enabled = this.transfoLum;
            this.lab100.Enabled = this.transfoLum;
            this.labIntensité.Enabled = this.transfoLum;
        }

        private void CouleurIndex()
        {
            this.indexTrackbarTab = 2;
            this.buttonBack.Enabled = true;
            this.buttonForward.Enabled = false;
            this.labIntensité.Text = "    Couleurs intensité";
            this.textBoxGris.Text = this.CouleurChgmtIntensité.ToString();

            this.toolTip1.SetToolTip(this.gristrackbar, "Atténue les couleurs d'une image en fonction d'un %.\n" +
                "100% est l'intensité de couleur actuelle, 0% est l'image en noir et blanc d'intensité 0% (cf 'Noir et Blanc')");

            this.toolTip1.SetToolTip(this.textBoxGris, "Atténue les couleurs d'une image en fonction d'un %.\n" +
                "100% est l'intensité de couleur actuelle, 0% est l'image en noir et blanc d'intensité 0% (cf 'Noir et Blanc')");

            this.gristrackbar.Enabled = this.transfoCouleur;
            this.textBoxGris.Enabled = this.transfoCouleur;
            this.lab0.Enabled = this.transfoCouleur;
            this.lab100.Enabled = this.transfoCouleur;
            this.labIntensité.Enabled = this.transfoCouleur;
        }


        private void button2_Click(object sender, EventArgs e) //backwards
        {
            if (this.indexTrackbarTab == 2)
                LuminoIndex();
            else if (this.indexTrackbarTab == 1)
                GrisIndex();
        }
    }
}
