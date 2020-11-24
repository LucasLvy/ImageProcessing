using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class FractaleForm : Form
    {
        public int height { get; private set; } = 500;
        public int width { get; private set; } = 500;
        public int maxTryColor { get; private set; } = 10;
        public int maxItération { get; private set; } = 200;
        public double Contraste { get; private set; } = 1;
        public int TailleMosaique { get; private set; } = 20;
        internal Fractale.Fractales type { get; private set; }
        internal Pixel backgroundColor { get; private set; } = Pixel.FromColor(Couleurs.Blanc);

        internal NombreComplex userComp = 0;

        public MyImage imageMosaique { get; private set; }


        private int selected = 0;

        private bool firstSelect = false;
        private bool secondSelect = false;


        public FractaleForm(MyImage image)
        {
            InitializeComponent();
            this.imageMosaique = image;

            if (this.imageMosaique != null)
            {
                AjoutImageBouton(this.imageMosaique);
            }
        }



        private void LB_equations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.firstSelect && !this.secondSelect)
            {
                this.firstSelect = true;
                this.TB_iteration.Enabled = true;
                this.TB_height.Enabled = true;
                this.TB_width.Enabled = true;
                this.SaveButton.Enabled = true;
            }

            if (this.secondSelect)
            {
                this.LB_récursiff.SetSelected(this.selected, false);
                this.secondSelect = false;
                this.firstSelect = true;
            }
            this.selected = this.LB_equations.SelectedIndex;

            this.TB_contraste.Enabled = true;

            this.But_imageMosaique.Enabled = false;
            this.TB_tryColor.Enabled = false;
            this.TB_mosaique.Enabled = false;
            this.TB_iteration.Text = "200";

            this.But_Color.Enabled = false;

            this.TB_Im.Enabled = this.TB_Re.Enabled = this.selected == 11;

            this.type = (Fractale.Fractales)this.selected;
        }

        private void LB_récursiff_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.firstSelect && !this.secondSelect)
            {
                this.secondSelect = true;
                this.TB_iteration.Enabled = true;
                this.TB_height.Enabled = true;
                this.TB_width.Enabled = true;
                this.SaveButton.Enabled = true;
            }

            if (this.firstSelect)
            {
                this.LB_equations.SetSelected(this.selected, false);
                this.firstSelect = false;
                this.secondSelect = true;
            }
            this.selected = this.LB_récursiff.SelectedIndex;
            
            if (this.selected == 10)
            {
                this.But_imageMosaique.Enabled = true;
                this.TB_mosaique.Text = "50";
                this.TB_height.Enabled = false;
                this.TB_width.Enabled = false;
            }
            else
            {
                this.But_imageMosaique.Enabled = false;
                this.TB_mosaique.Text = "30";
                this.TB_height.Enabled = true;
                this.TB_width.Enabled = true;
            }

            if (this.selected >= 8)
            {
                this.TB_iteration.Text = "25";
                this.But_Color.Enabled = false;
            }
            else
            {
                this.But_Color.Enabled = true;
                this.TB_iteration.Text = "6";
            }

            this.TB_tryColor.Enabled = this.selected == 10;

            this.TB_mosaique.Enabled = this.selected >= 9;
            this.TB_contraste.Enabled = false;

            this.type = (Fractale.Fractales)(this.selected + 12);
        }



        private void But_imageMosaique_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Program.ADRESSE_SAUVEGARDE;
            openFileDialog1.Filter = "Fichiers bitmap|*.bmp|Fichiers csv|*.csv|Fichiers bmp et csv|*.csv;*.bmp";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";


            DialogResult fichier = openFileDialog1.ShowDialog();
            string FileName = openFileDialog1.FileName;


            if (File.Exists(FileName) && fichier == DialogResult.OK)
            {
                this.imageMosaique = new MyImage(FileName);

                AjoutImageBouton(this.imageMosaique);
            }
        }


        private void AjoutImageBouton(MyImage image)
        {
            Cursor.Current = Cursors.WaitCursor;

            MyGraphics g = new MyGraphics(this.imageMosaique.GetCopie());
            g.Redimensionnement(this.But_imageMosaique.Height, this.But_imageMosaique.Width, InterpolationMode.Bilineaire);

            this.But_imageMosaique.BackgroundImage = g.GetMyImage.ToBitmap();


            Cursor.Current = Cursors.Default;
        }



        private void TB_height_TextChanged(object sender, EventArgs e)
        {
            this.height = int.TryParse(this.TB_height.Text, out int value) ? value : 0;
        }

        private void TB_width_TextChanged(object sender, EventArgs e)
        {
            this.width = int.TryParse(this.TB_width.Text, out int value) ? value : 0;
        }

        private void TB_Re_TextChanged(object sender, EventArgs e)
        {
            this.userComp.Re = float.TryParse(new string((from s in (sender as TextBox).Text.Replace(".", ",") where !char.IsLetter(s) select s).ToArray()), out float value) ? value : 0;
        }

        private void TB_Im_TextChanged(object sender, EventArgs e)
        {
            this.userComp.Im = float.TryParse(new string((from s in (sender as TextBox).Text.Replace(".", ",") where !char.IsLetter(s) select s).ToArray()), out float value) ? value : 0;
        }


        private void TB_contraste_TextChanged(object sender, EventArgs e)
        {
            string newS = (sender as TextBox).Text;
            newS = newS.Replace(".", ",");

            this.Contraste = float.TryParse(newS, out float value) ? value : 1;
        }

        private void TB_mosaique_TextChanged(object sender, EventArgs e)
        {
            this.TailleMosaique = int.TryParse(this.TB_mosaique.Text, out int value) ? value : 0;
        }

        private void TB_tryColor_TextChanged(object sender, EventArgs e)
        {
            this.maxTryColor = int.TryParse(this.TB_tryColor.Text, out int value) ? value : 0;
        }

        private void TB_iteration_TextChanged(object sender, EventArgs e)
        {
            this.maxItération = int.TryParse(this.TB_iteration.Text, out int value) ? value : 0;
        }



        private void TB_Im_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '-') && ((sender as TextBox).Text.LastIndexOf('-') != -1 && (sender as TextBox).Text.IndexOf('-') != -1))
            {
                e.Handled = true;
            }
            
        }

        private void TB_Re_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '-') && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            if ((e.KeyChar == '-') && ((sender as TextBox).Text.LastIndexOf('-') != -1 && (sender as TextBox).Text.IndexOf('-') != -1))
            {
                e.Handled = true;
            }
        }

        private void TB_height_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TB_width_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TB_mosaique_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TB_contraste_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void TB_tryColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void FractaleForm_Load(object sender, EventArgs e)
        {
            this.TB_height.Text = this.height.ToString();
            this.TB_width.Text = this.width.ToString();
            this.TB_tryColor.Text = this.maxTryColor.ToString();
            this.TB_contraste.Text = this.Contraste.ToString();
            this.TB_iteration.Text = this.maxItération.ToString();
            this.SaveButton.Enabled = false;
            this.But_Color.Enabled = false;
            this.But_Color.BackColor = this.backgroundColor.ToColor();
        }



        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (((this.height <= 10 || this.width <= 10) && this.type != Fractale.Fractales.Mosaïque_From_Image)|| this.maxItération <= 0
                || (this.type == Fractale.Fractales.Mosaïque_From_Image && this.imageMosaique == null))
            {
                MessageBox.Show("La largeur et la hauteur de l'image doivent être supérieures à 10 !\nL'itération doit aussi être supérieure à 0\n" +
                    "Il faut aussi charger une image avant d'en créer une mosaïque", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                this.DialogResult = DialogResult.Yes;
        }

        private void But_Color_Click(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.backgroundColor = Pixel.FromColor(this.colorDialog1.Color);
                this.But_Color.BackColor = this.colorDialog1.Color;
            }
        }
    }
}
