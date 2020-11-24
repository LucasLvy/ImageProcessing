using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Histogramme : Form
    {
        public int largeur = 1000;
        public int hauteur = 500;

        public bool remplissage = true;

        public bool color = true;

        public Histogramme()
        {
            InitializeComponent();
        }

        private void Histogramme_Load(object sender, EventArgs e)
        {
            this.listBox1.SelectedIndex = 1;
            this.rempliCheckBox.Checked = true;
            this.largeurTB.Text = this.largeur.ToString();
            this.hauteurTB.Text = this.hauteur.ToString();
        }

        private void largeurTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void largeurTB_TextChanged(object sender, EventArgs e)
        {
            this.largeur = int.TryParse((sender as TextBox).Text, out int value) ? value : 0;
        }

        private void hauteurTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        private void hauteurTB_TextChanged(object sender, EventArgs e)
        {
            this.hauteur = int.TryParse((sender as TextBox).Text, out int value) ? value : 0;
        }


        private void rempliCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.remplissage = this.rempliCheckBox.Checked;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.color = this.listBox1.SelectedIndex == 1;
        }


        private void GoBut_Click(object sender, EventArgs e)
        {
            if (this.largeur >= 50 && this.hauteur >= 20)
                this.DialogResult = DialogResult.Yes;
            else
            {
                MessageBox.Show("Vous ne pouvez pas créer un histogramme avec :\n-largeur < 50 \n-hauteur <20", "Erreur : image non chargée",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
