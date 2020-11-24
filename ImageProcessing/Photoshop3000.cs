using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ImageProcessing
{
    public partial class Photoshop3000 : Form
    {
        private List<MyImage> imagesParOnglet = new List<MyImage>();
        private List<Panel> panelsParOnglet = new List<Panel>();

        private string FileName;

        private bool imageChargée = false;
        private bool realSize = false;
        private bool modeNuit = false;

        private int currentIndexImage = 0;

        private int numéroModif = 1;


        public Photoshop3000()
        {
            InitializeComponent();
            this.menuStrip1.Renderer = new ToolStripProfessionalRenderer(new CouleurMenuDéroulant(this.modeNuit));
            this.Originale.BackgroundImageLayout = ImageLayout.Center;
            this.Originale.BackgroundImage = ImageHelper();
        }
        

        #region Open et Save file

        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //openFileDialog1.InitialDirectory = Program.ADRESSE_SAUVEGARDE;
            openFileDialog1.Filter = "Fichiers bitmap|*.bmp|Fichiers csv|*.csv|Fichiers bmp et csv|*.csv;*.bmp";
            openFileDialog1.FilterIndex = 3;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";


            DialogResult fichier = openFileDialog1.ShowDialog();
            FileName = openFileDialog1.FileName;


            if (File.Exists(FileName) && fichier == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                MyImage im = new MyImage(FileName);

                if (!im.BitmapValide || im.GetHeight < 2 || im.GetWidth < 2)
                {
                    MessageBox.Show("L'image est trop petite, doit etre supérieur à 2 pixels en largeur et hauteur", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (this.Tabs.TabPages.Count == 1)
                {
                    this.imagesParOnglet.Clear();
                    ClearEverything("");
                    this.imageChargée = false;
                    this.imagesParOnglet.Add(new MyImage(FileName));
                    ModificationActive();
                }
                else
                {
                    if (ClearEverything("Voulez vous vraiment tout supprimer ?") != DialogResult.Yes)
                    {
                        return;
                    }
                    this.imagesParOnglet.Add(new MyImage(FileName));
                    ModificationActive();
                }
                this.imageChargée = true;
                this.sauvegarderToolStripMenuItem.Enabled = true;
                this.Originale.BackgroundImage = null;
                Cursor.Current = Cursors.Default;
            }
        }

        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
                SaveDialog(this.imagesParOnglet[Tabs.TabPages.Count - 1]); //Le dernier
        }
        private void SaveDialog(MyImage toSave)
        {
            saveFileDialog1.InitialDirectory = Program.ADRESSE_SAUVEGARDE;
            saveFileDialog1.Filter = "Fichiers bitmap|*.bmp|Fichiers csv|*.csv|Fichiers bmp et csv|*.bmp;*.csv;";
            saveFileDialog1.FilterIndex = 3;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = Path.GetFileNameWithoutExtension(this.FileName) + " Modif " + (this.numéroModif - 1).ToString();

            DialogResult SaveFile = saveFileDialog1.ShowDialog();


            if (SaveFile == DialogResult.OK && Directory.Exists(Path.GetDirectoryName(saveFileDialog1.FileName)))
            {
                Cursor.Current = Cursors.WaitCursor;
                toSave.Save(saveFileDialog1.FileName);
                Cursor.Current = Cursors.Default;
            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
                SaveDialog(this.imagesParOnglet[this.Tabs.SelectedIndex]);
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!imageChargée || MessageBox.Show("Ëtes-vous sûr de vouloir quitter l'application ?", "Quitter",
                       MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                this.Dispose();
                Environment.Exit(0);
            }
        }


        #endregion



        #region Filtre

        private int pixélisation = 10;
        private bool adaptationTaille = true;


        private void ApplicationMéthodes(int méthode)
        {
            if (this.imageChargée)
            {
                Cursor.Current = Cursors.WaitCursor;
                MyGraphics g = new MyGraphics(this.imagesParOnglet[Tabs.SelectedIndex].GetCopie());
                switch (méthode)
                {
                    case 2:
                        g.FiltreSobel();
                        break;
                    case 3:
                        g.FiltreCanny();
                        break;

                    #region Filtres de matrice de convolution

                    case 1:
                        g.FiltreDetectionContours();
                        break;
                    case 4:
                        g.FiltreRenforcementContours();
                        break;
                    case 5:
                        g.FiltreAffaiblissementContours();
                        break;
                    case 6:
                        g.FiltreDessin(false);
                        break;
                    case 7:
                        g.FiltreGravure();
                        break;
                    case 8:
                        g.FiltreRepoussageFaible();
                        break;
                    case 9:
                        g.FiltreRepoussageFort();
                        break;
                    case 10:
                        g.FiltreAiguisage();
                        break;
                    case 11:
                        g.FiltreContraste();
                        break;
                    case 12:
                        g.FiltreFlou(false);
                        break;
                    case 13:
                        g.FiltreFlou(true);
                        break;
                    case 14:
                        g.FiltreFlouGaussien();
                        break;
                    case 15:
                        g.FiltreLissage();
                        break;

                    #endregion

                    #region bruit numérique

                    case 16:
                        g.AddNoise(20);
                        break;
                    case 17:
                        g.AddPepperNoise(10);
                        break;
                    case 18:
                        g.AddShotNoise(10);
                        break;

                    #endregion

                    case 19:
                        g.KeepAspectRatio = this.adaptationTaille;
                        g.Pixélisation(this.pixélisation);
                        break;

                    #region effet miroir

                    case 20:
                        g.EffetMiroir();
                        break;

                    case 21:
                        g.EffetMiroirDoubleOuQuadruple(true, false);
                        break;

                    case 22:
                        g.EffetMiroirDoubleOuQuadruple(false, true);
                        break;

                    case 23:
                        g.EffetMiroirDoubleOuQuadruple(true, true);
                        break;

                    #endregion

                    case 24:
                        g.CouleurPeinture();
                        break;

                    case 25:
                        g.DécallageSin();
                        break;

                }

                this.imagesParOnglet.Add(g.GetMyImage);
                ModificationActive(Tabs.TabPages.Count);
            }
            else
            {
                MessageBox.Show("Ouvrez d'abord une image avant de la modifier !", "Erreur : image non chargée",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void détectionDesContoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(1);
        }

        private void sobelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(2);
        }

        private void cannyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(3);
        }

        private void renforcementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(4);
        }

        private void affaiblissementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(5);
        }

        private void dessinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(6);
        }

        private void gravureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(7);
        }

        private void faibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(8);
        }

        private void fortToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(9);
        }

        private void aiguisageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(10);
        }

        private void contrasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(11);
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(12);
        }

        private void mouvementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(13);
        }

        private void gaussienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(14);
        }

        private void lissageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(15);
        }
        

        private void couleursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
            {
                using (CouleurFiltre colorFilter = new CouleurFiltre())
                {
                    if (colorFilter.ShowDialog() == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        MyGraphics g = new MyGraphics(this.imagesParOnglet[this.currentIndexImage].GetCopie());

                        if (colorFilter.NoirEtBlanc)
                        {
                            g.TransformationNoirEtBlanc(colorFilter.GrisIntensité);
                        }
                        else if (colorFilter.Rouge || colorFilter.Vert || colorFilter.Bleu)
                        {
                            g.TransformationRGB(colorFilter.Rouge, colorFilter.Vert, colorFilter.Bleu);
                        }

                        if (colorFilter.inversion)
                            g.InversionCouleurs();

                        if (colorFilter.Sépia)
                            g.TransformationSépia();

                        if (colorFilter.transfoLum)
                            g.TransformationLuminositéPerçue(colorFilter.LumChgmtIntensité);

                        if (colorFilter.transfoCouleur)
                            g.TransformationCouleurIntensité(100 - colorFilter.CouleurChgmtIntensité);

                        this.imagesParOnglet.Add(g.GetMyImage);
                        ModificationActive(this.Tabs.TabPages.Count);

                    }

                    colorFilter.Dispose();
                }
            }
            else
            {
                MessageBox.Show("Ouvrez d'abord une image avant de la modifier !", "Erreur : image non chargée",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region Fractale

        private void fractalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FractaleForm frac = new FractaleForm(this.imageChargée ? this.imagesParOnglet[this.currentIndexImage] : null))
            {
                if (frac.ShowDialog() == DialogResult.Yes)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    Fractale fractale;
                    if (frac.type == Fractale.Fractales.Mosaïque_From_Image)
                    {
                        fractale = new Fractale(frac.imageMosaique)
                        {
                            MaxItération = frac.maxItération,
                            MaxTryColor = frac.maxTryColor,
                            TailleMosaique = frac.TailleMosaique,
                        };
                    }
                    else
                    {
                        fractale = new Fractale(frac.height, frac.width, frac.type)
                        {
                            UserEquation = frac.userComp,
                            Contraste = frac.Contraste,
                            MaxItération = frac.maxItération,
                            MaxTryColor = frac.maxTryColor,
                            TailleMosaique = frac.TailleMosaique,
                            BackgroundColor = frac.backgroundColor
                        };
                    }
                    fractale.CreateFractale();

                    this.imagesParOnglet.Add(fractale.GetMyImage);

                    if (this.imageChargée)
                    {
                        ModificationActive(Tabs.TabPages.Count);
                    }
                    else
                    {
                        ModificationActive();
                        this.imageChargée = true;
                        this.sauvegarderToolStripMenuItem.Enabled = true;
                        this.Originale.BackgroundImage = null;
                        Cursor.Current = Cursors.Default;
                    }

                }

                frac.Dispose();
            }
        }

        #endregion


        #region Création

        private void effetPeintureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(24);
        }


        private void dessinsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Dessin drawing = new Dessin(this.imageChargée ? this.imagesParOnglet[this.currentIndexImage].GetCopie() : null))
            {
                if (drawing.ShowDialog() == DialogResult.Yes)
                {
                    if (drawing.imageOnLoad.BitmapValide)
                    {
                        this.imagesParOnglet.Add(drawing.imageOnLoad);

                        if (this.imageChargée)
                        {
                            ModificationActive(Tabs.TabPages.Count);
                        }
                        else
                        {
                            ModificationActive();
                            this.imageChargée = true;
                            this.sauvegarderToolStripMenuItem.Enabled = true;
                            this.Originale.BackgroundImage = null;
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
        }


        private void copieImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
            {
                using (CopieImageForm copie = new CopieImageForm(this.imagesParOnglet[this.currentIndexImage]))
                {
                    if (copie.ShowDialog() == DialogResult.Yes)
                    {
                        MyImage copieRealSize = new MyImage(copie.GetImage, copie.HeightImage, copie.WidthImage);

                        MyGraphics g = new MyGraphics(this.imagesParOnglet[this.currentIndexImage].GetCopie());
                        g.AddImage(copieRealSize, copie.GetPointOrigine, copie.Opacité);
                        
                        this.imagesParOnglet.Add(g.GetMyImage);
                        ModificationActive(Tabs.TabPages.Count);
                    }

                    copie.Dispose();
                }
            }
            else
                MessageBox.Show("Ouvrez d'abord une image !", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void pixélisationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
            {
                using (Pixélisation pixel = new Pixélisation(this.imagesParOnglet[this.currentIndexImage].GetWidth))
                {
                    if (pixel.ShowDialog() == DialogResult.Yes)
                    {
                        this.pixélisation = pixel.taillePixel;
                        this.adaptationTaille = pixel.adaptateur;
                        ApplicationMéthodes(19);
                    }
                }
            }
            else
            {
                MessageBox.Show("Ouvrez d'abord une image avant de la modifier !", "Erreur : image non chargée",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DécallageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(25);
        }

        //Bruit numérique
        private void coloréToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(16);
        }

        private void selEtPoivreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(17);
        }

        private void grenailleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(18);
        }


        //Miroir
        private void simpleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(20);
        }

        private void doubleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(21);
        }

        private void doubleHauteurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(22);
        }

        private void quadrupleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplicationMéthodes(23);
        }

        #endregion


        #region stéganographie

        private void stéganographieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
            {
                using (StéganographieForm ste = new StéganographieForm())
                {
                    if (ste.ShowDialog() == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        MyImage myImage = new MyImage(this.imagesParOnglet[this.currentIndexImage].GetHeight, this.imagesParOnglet[this.currentIndexImage].GetWidth);
                        MyGraphics g = new MyGraphics(this.imagesParOnglet[this.currentIndexImage].GetCopie());

                        if (ste.HideImage)
                        {
                            if (ste.AdapatSize)
                            {
                                double ratioW = (double)myImage.GetWidth / ste.imageToHide.GetWidth;
                                double ratioH = (double)myImage.GetHeight / ste.imageToHide.GetHeight;

                                double rapportChgmtTaille = ratioH > ratioW ? ratioW : ratioH;

                                new MyGraphics(ste.imageToHide).Redimensionnement(rapportChgmtTaille);
                            }
                            g.CacherImage(ste.imageToHide, ste.BitsToHide);
                        }
                        else if (ste.GetImage)
                        {
                            g.GetImageCachée(ste.BitsToHide);
                        }
                        else if (ste.HideTexte)
                        {
                            g.CacherTexte(ste.texteToHide, ste.mdp, ste.BitsToHide);

                            if (this.imagesParOnglet[this.currentIndexImage] == g.GetMyImage) //La fct renvoie la meme image si txt trop long
                            {
                                Cursor.Current = Cursors.Default;
                                MessageBox.Show("Le texte est trop long pour le cacher dans l'image !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        else if (ste.GetTexte)
                        {
                            string s = g.GetTexteCaché(ste.mdp, ste.longueurTexte, ste.BitsToHide);

                            Cursor.Current = Cursors.Default;
                            MessageBox.Show(s, "Texte trouvé", MessageBoxButtons.OK, MessageBoxIcon.None);
                            return;
                        }

                        this.imagesParOnglet.Add(g.GetMyImage);
                        ModificationActive(Tabs.TabPages.Count);
                    }
                }
            }
            else
                MessageBox.Show("Ouvrez d'abord une image !", "ERREUR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        #endregion


        #region Histo

        private void infosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (imageChargée)
            {
                using (Histogramme histoForm = new Histogramme())
                {
                    if (histoForm.ShowDialog() == DialogResult.Yes)
                    {
                        Cursor.Current = Cursors.WaitCursor;

                        MyImageStatistiques histo = new MyImageStatistiques(this.imagesParOnglet[this.currentIndexImage], histoForm.hauteur, histoForm.largeur);
                        HistogrammeMode mode = histoForm.color ? HistogrammeMode.Echelle_Couleurs : HistogrammeMode.Echelle_Gris;

                        this.imagesParOnglet.Add(histo.CreateHistogramme(mode, histoForm.remplissage));

                        ModificationActive(Tabs.TabPages.Count);
                    }
                }
            }
            else
                MessageBox.Show("Ouvrez d'abord une image avant d'en créer un histogramme !", "Erreur : image non chargée",
                       MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion


        #region outils

        private void paramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ModifImage modif = new ModifImage(this.imageChargée ? this.imagesParOnglet[this.currentIndexImage] : null))
            {
                DialogResult d = modif.ShowDialog();
                if (d == DialogResult.Yes && this.imageChargée)
                {
                    Cursor.Current = Cursors.WaitCursor;

                    int newHeight = modif.newHeight;
                    int newWidth = modif.newWidth;

                    double angle = modif.angleRotation;

                    Point topLeft = modif.topLeft;
                    Point bottomRight = modif.bottomRight;

                    MyGraphics graph = new MyGraphics(this.imagesParOnglet[this.currentIndexImage].GetCopie())
                    {
                        KeepAspectRatio = modif.b_KeepAspectRatio,
                        Quality = modif.quality,
                        PixelRemplissage = modif.remplissage
                    };

                    bool changementEffectué = false;

                    if (topLeft != new Point(0, 0) || bottomRight != new Point(this.imagesParOnglet[this.currentIndexImage].GetHeight - 1,
                        this.imagesParOnglet[this.currentIndexImage].GetWidth - 1))
                    {
                        graph.Rognage(topLeft, bottomRight);
                        changementEffectué = true;
                    }

                    if (newHeight != 0 && newWidth != 0 && (newHeight != this.imagesParOnglet[this.currentIndexImage].GetHeight ||
                            newWidth != this.imagesParOnglet[this.currentIndexImage].GetWidth))
                    {
                        graph.Redimensionnement(newHeight, newWidth);
                        changementEffectué = true;
                    }

                    if (angle != 0)
                    {
                        graph.Rotation(angle, modif.BordsRemplis);
                        changementEffectué = true;
                    }

                    if (changementEffectué)
                    {
                        this.imagesParOnglet.Add(graph.GetMyImage);
                        ModificationActive(Tabs.TabPages.Count);
                    }
                    else
                    {
                        MessageBox.Show("Aucune modification n'a été effectué sur l'image !", "Pas de modification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else if (!imageChargée && d == DialogResult.Yes)
                {
                    MessageBox.Show("Vous devez ouvrir une image avant d'y effectuer des opérations !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                modif.Dispose();
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.imageChargée)
            {
                Cursor.Current = Cursors.WaitCursor;

                int hue = MyImageStatistiques.GetAverageHue(this.imagesParOnglet[this.currentIndexImage]);
                double light = MyImageStatistiques.GetAverageLightness(this.imagesParOnglet[this.currentIndexImage]);
                double satur = MyImageStatistiques.GetAverageSaturation(this.imagesParOnglet[this.currentIndexImage]);
                double bright = MyImageStatistiques.GetAverageBrightness(this.imagesParOnglet[this.currentIndexImage]);
                Pixel colorAverage = MyImageStatistiques.GetAverageColor(this.imagesParOnglet[this.currentIndexImage]);

                string infos = $"Informations sur l'image :\n" +
                    $"    - Taille           : Largeur : {this.imagesParOnglet[this.currentIndexImage].GetWidth}  |  Hauteur :" +
                    $" {this.imagesParOnglet[this.currentIndexImage].GetHeight}\n\n" +
                    $"    - Teinte           : {hue.ToString()}° (0-360°)\n" +
                    $"    - Luminosité  : {(light * 100).ToString("0")}%\n" +
                    $"    - Saturation   : {(satur * 100).ToString("0")}%\n\n" +
                    $"    - Luminance   : {bright.ToString("0")}\n\n" +
                    $"    - Pixel moyen : {colorAverage.ToString()}\n" +
                    $"                              = {Pixel.GetCouleur(colorAverage).ToString()}";

                Cursor.Current = Cursors.Default;

                MessageBox.Show(infos, "Informations", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Il n'y a pas d'image chargée !", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void aideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pour modifier une image, mettez-vous dans l'onglet de l'image à modifier puis choisissez une méthode " +
                "dans les menu déroulants pour la modifier. Un nouvel onglet apparaitra avec l'image changée.\n\n" +
                "Pour sauvegarder une image en particulier appuyez sur le bouton 'SAVE' dans l'onglet de l'image à sauvegarder.\n" +
                @"Vous pouvez vous déplacer dans une image si la case ""Real size"" est cochée", "Aide",
                       MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion



        #region Paramètres internes

        private void ModificationActive(int onglet = 0)
        {
            if (this.imageChargée)
            {
                TabPage page = new TabPage("Modification Image n°" + numéroModif++ + "     ")
                {
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(3, 3, 3, 3),
                    Padding = new Padding(2, 2, 2, 2),
                    BackColor = Color.Gainsboro,
                    AutoScroll = true,
                };

                this.Tabs.TabPages.Add(page);
                this.Tabs.SelectedTab = this.Tabs.TabPages[onglet];
            }

            this.PictureBoxParOnglet.Add(new PictureBox());
            this.PictureBoxParOnglet[onglet].BackColor = System.Drawing.Color.Gainsboro;
            this.PictureBoxParOnglet[onglet].Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBoxParOnglet[onglet].Location = new System.Drawing.Point(3, 3);
            this.PictureBoxParOnglet[onglet].Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right));
            this.PictureBoxParOnglet[onglet].Name = Tabs.TabPages.Count.ToString();
            this.PictureBoxParOnglet[onglet].Size = new System.Drawing.Size(689, 383);
            this.PictureBoxParOnglet[onglet].SizeMode = this.realSize ? PictureBoxSizeMode.AutoSize : PictureBoxSizeMode.Zoom;
            this.PictureBoxParOnglet[onglet].TabIndex = onglet;
            this.PictureBoxParOnglet[onglet].TabStop = false;
            this.PictureBoxParOnglet[onglet].Image = this.imagesParOnglet[onglet].ToBitmap();

            this.PictureBoxParOnglet[onglet].MouseDown += this.pictureBox1_MouseDown;
            this.PictureBoxParOnglet[onglet].MouseMove += this.pictureBox1_MouseMove;
            this.PictureBoxParOnglet[onglet].MouseUp += this.pictureBox1_MouseUp;

            this.panelsParOnglet.Add(new Panel());
            this.panelsParOnglet[onglet].Size = this.Tabs.SelectedTab.Size;
            this.panelsParOnglet[onglet].Anchor = ((AnchorStyles)(((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right));
            this.panelsParOnglet[onglet].AutoScroll = true;
            this.panelsParOnglet[onglet].Controls.Add(this.PictureBoxParOnglet[onglet]);


            this.Tabs.TabPages[onglet].Controls.Add(this.panelsParOnglet[onglet]);
            this.currentIndexImage = onglet;

            if (!this.CheckLBSettings.GetItemChecked(1))
            {
                this.realSize = true;
                this.CheckLBSettings.SetSelected(1, true);
                this.CheckLBSettings.SetSelected(1, false);
            }


            Cursor.Current = Cursors.Default;
        }


        private void clear_Click(object sender, EventArgs e)
        {
            ClearEverything("Vous avez plusieurs onglets ouverts, voulez-vous vraiment tous les supprimer ?");
        }

        private DialogResult ClearEverything(string MessageErreur)
        {
            DialogResult result = DialogResult.None;
            if (Tabs.TabCount > 1)
            {
                result = MessageBox.Show("Vous avez plusieurs onglets ouverts ! Voulez-vous vraiment tous les supprimer ?", "Attention !",
                       MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    this.imagesParOnglet = new List<MyImage>();
                    for (int i = Tabs.TabCount - 1; i > 0; i--)
                    {
                        this.PictureBoxParOnglet.RemoveAt(i);
                        this.panelsParOnglet.RemoveAt(i);
                    }
                    SupprimerOnglets();
                    this.PictureBoxParOnglet[0].Image = ImageHelper();
                    this.sauvegarderToolStripMenuItem.Enabled = false;
                    this.imageChargée = false;
                }
            }
            else if (this.imageChargée)
            {
                this.imagesParOnglet = new List<MyImage>();
                SupprimerOnglets();
                for (int i = 0; i < Tabs.TabCount; i++)
                {
                    this.PictureBoxParOnglet.RemoveAt(i);
                    this.panelsParOnglet.RemoveAt(i);
                    this.Tabs.TabPages[i].Controls.Clear();
                }
                this.Tabs.TabPages[0].BackgroundImage = ImageHelper();
                this.sauvegarderToolStripMenuItem.Enabled = false;
                this.imageChargée = false;
            }

            return result;
        }

        private void SupprimerOnglets()
        {
            for (int i = Tabs.TabCount - 1; i > 0; i--)
            {
                this.Tabs.TabPages.RemoveAt(i);
            }
            this.currentIndexImage = 0;
            this.numéroModif = 1;
        }


        private void Settings_Click(object sender, EventArgs e)
        {
            this.CheckLBSettings.Visible = !this.CheckLBSettings.Visible;
            this.SaveButton.Location = new System.Drawing.Point(this.SaveButton.Location.X, this.SaveButton.Location.Y + (this.CheckLBSettings.Height * (this.CheckLBSettings.Visible ? 1 : -1)));

            this.SaveButton.Height = this.SaveButton.Height + this.CheckLBSettings.Height * (this.CheckLBSettings.Visible ? -1 : 1);
        }
        
        private void CheckLBSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CheckLBSettings.GetItemChecked(0) && this.CheckLBSettings.GetSelected(0) && !this.modeNuit)
            {
                menuStrip1.Renderer = new ToolStripProfessionalRenderer(new CouleurMenuDéroulant(true));
                this.Originale.BackColor = Color.Black;

                foreach (var pb in PictureBoxParOnglet)
                    pb.BackColor = Color.FromArgb(31, 31, 31);

                this.menuStrip1.BackColor = Color.FromArgb(31, 31, 31);
                this.BackColor = Color.FromArgb(31, 31, 31);
                this.Settings.BackColor = Color.FromArgb(61, 61, 61);
                this.CheckLBSettings.BackColor = Color.FromArgb(61, 61, 61);
                this.menuStrip1.ForeColor = Color.White;
            }
            else if (this.CheckLBSettings.GetSelected(0) && this.modeNuit)
            {
                menuStrip1.Renderer = new ToolStripProfessionalRenderer(new CouleurMenuDéroulant(false));
                this.Originale.BackColor = Color.White;

                foreach (var pb in PictureBoxParOnglet)
                    pb.BackColor = Color.Gainsboro;

                this.menuStrip1.BackColor = Color.WhiteSmoke;
                this.BackColor = Color.WhiteSmoke;
                this.Settings.BackColor = Color.DimGray;
                this.CheckLBSettings.BackColor = Color.DimGray;
                this.menuStrip1.ForeColor = Color.Black;
            }

            this.modeNuit = this.CheckLBSettings.GetItemChecked(0);

            if (this.CheckLBSettings.GetItemChecked(1) && this.CheckLBSettings.GetSelected(1) && !this.realSize)
            {
                foreach (var pb in PictureBoxParOnglet)
                {
                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                }
                this.Tabs.Refresh();
            }
            else if (this.CheckLBSettings.GetSelected(1) && this.realSize)
            {
                for (int i = 0; i < this.PictureBoxParOnglet.Count; i++)
                {
                    this.PictureBoxParOnglet[i].Size = this.panelsParOnglet[i].Size;
                    this.PictureBoxParOnglet[i].SizeMode = PictureBoxSizeMode.Zoom;
                    this.PictureBoxParOnglet[i].Left = 0;
                    this.PictureBoxParOnglet[i].Top = 0;
                }
                this.Tabs.Refresh();
            }

            this.realSize = this.CheckLBSettings.GetItemChecked(1);
        }


        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)//Chemin pour image à changer
        {
            Brush couleurTexte = new SolidBrush(Color.Black);

            if (!modeNuit)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Gainsboro), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(31, 31, 31)), e.Bounds);
                couleurTexte = new SolidBrush(Color.Gainsboro);
            }

            var tabPage = this.Tabs.TabPages[e.Index];
            var tabRect = this.Tabs.GetTabRect(e.Index);
            tabRect.Inflate(-2, -2);
            if (e.Index != 0) 
            {
                MyImage closeImage = MyImage.FromBitmap(Properties.Resources.CroixRouge); //Croix rouge
                new MyGraphics(closeImage).Redimensionnement(20, 20);

                e.Graphics.DrawImage(closeImage.ToBitmap(),
                    (tabRect.Right - closeImage.GetWidth),
                    tabRect.Top + (tabRect.Height - closeImage.GetHeight) / 2);

                TextRenderer.DrawText(e.Graphics, tabPage.Text, tabPage.Font,
                    tabRect, tabPage.ForeColor, TextFormatFlags.Left);
            }
            else
            {
                Rectangle bordures = e.Bounds;
                bordures.Inflate(-2, -2);
                e.Graphics.DrawString(this.Tabs.TabPages[e.Index].Text, this.Font, couleurTexte, bordures);
            }

        } //Croix rouge à la fin d'un onglet
        
        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (var i = 1; i < this.Tabs.TabPages.Count; i++)
            {
                Rectangle tabRect = this.Tabs.GetTabRect(i);
                tabRect.Inflate(-2, -2);

                Rectangle imageRect = new Rectangle(tabRect.Right - 20, tabRect.Top + (tabRect.Height - 20) / 2, 20, 20);

                if (imageRect.Contains(e.Location))
                {
                    this.Tabs.TabPages.RemoveAt(i);
                    this.PictureBoxParOnglet.RemoveAt(i);
                    this.panelsParOnglet.RemoveAt(i);
                    this.imagesParOnglet.RemoveAt(i);
                    this.Tabs.SelectedTab = this.Tabs.TabPages[this.currentIndexImage > this.Tabs.TabCount - 1 ? this.currentIndexImage - 1 : this.currentIndexImage]; ;
                    if (this.Tabs.TabCount == 1)
                        this.numéroModif = 1;
                    break;
                }
            }
            this.currentIndexImage = this.Tabs.SelectedIndex;
        } //Chgmt d'onglet ou supression de l'onglet si on est sur une croix rouge



        //Déplcament dans une image affichée avec la taille réelle et qui dépasse des bords de la picturebox
        private bool ImageMoving;

        private System.Drawing.Point imageLocation = new System.Drawing.Point();

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && this.realSize)
            {
                imageLocation = e.Location;
                ImageMoving = true;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Control c = sender as Control;
            if (ImageMoving && c != null)
            {
                int top = e.Y - imageLocation.Y;
                int left = e.X - imageLocation.X;

                if (this.PictureBoxParOnglet[this.currentIndexImage].Image.Width + c.Left + left > this.panelsParOnglet[this.currentIndexImage].Width && c.Left + left < 0)
                {
                    c.Left += left;
                }
                if (this.PictureBoxParOnglet[this.currentIndexImage].Image.Height + c.Top + top > this.panelsParOnglet[this.currentIndexImage].Height && c.Top + top < 0)
                {
                    c.Top += top;
                }
                this.Tabs.TabPages[this.currentIndexImage].Refresh();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            ImageMoving = false;
        }


        //Quand changement de taille et que RealSize activé
        private Size sizeForm;

        private void Photoshop3000_sizebegin(object sender, EventArgs e)
        {
            this.sizeForm = this.Size;
        }

        private void Photoshop3000_SizeChange(object sender, EventArgs e)
        {
            int diffX = this.Size.Width - this.sizeForm.Width;
            int diffY = this.Size.Height - this.sizeForm.Height;

            if (this.imageChargée)
            {
                foreach (var pb in PictureBoxParOnglet)
                {
                    int origineX = pb.Left + diffX;
                    int origineY = pb.Top + diffY;

                    if (diffX > 0 && pb.Left + pb.Image.Width <= this.panelsParOnglet[this.currentIndexImage].Width)
                    {
                        if (origineX > 0)
                            pb.Left = 0;
                        else
                            pb.Left += diffX;
                    }

                    if (diffY > 0 && pb.Top + pb.Image.Height <= this.panelsParOnglet[this.currentIndexImage].Height)
                    {
                        if (origineY > 0)
                            pb.Top = 0;
                        else
                            pb.Top += diffY;
                    }

                }
            }
            this.Tabs.Refresh();
        }


        #endregion



        /// <summary>
        /// Ajoute du texte pour indiquer à l'utilisateur que faire
        /// </summary>
        /// <returns></returns>
        private Bitmap ImageHelper()
        {
            int height = this.PictureBoxParOnglet.Count != 0 ? this.PictureBoxParOnglet[0].Height : this.Originale.Height - this.Originale.Padding.Vertical;
            int width = this.PictureBoxParOnglet.Count != 0 ? this.PictureBoxParOnglet[0].Width : this.Originale.Width - this.Originale.Padding.Horizontal;

            Bitmap bmp = new Bitmap(width, height);
            Graphics.FromImage(bmp).DrawString(" Veuillez charger\n\rune image depuis\r\n le menu 'fichier'",
                new Font("Times New Roman", 40, FontStyle.Bold), Brushes.Black, new System.Drawing.Point(65, 70));
            Graphics.FromImage(bmp).DrawString("ou créez-en une depuis les menus\r\n 'Fractales' et 'Création > Dessins'",
                new Font("Times New Roman", 17, FontStyle.Regular), Brushes.Black, new System.Drawing.Point(110, 270));

            return bmp;
        }

    }
}
