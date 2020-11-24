using System;

namespace ImageProcessing
{

    /// <summary>
    /// Différents types d'histogramme possible 
    /// </summary>
    public enum HistogrammeMode
    {
        /// <summary>
        /// Histogramme d'échelle de gris
        /// </summary>
        Echelle_Gris,

        /// <summary>
        /// Histogramme des 3 couleurs R-G-B
        /// </summary>
        Echelle_Couleurs,

        /// <summary>
        /// TSV, pas implémenté
        /// </summary>
        HSV
    }


    /// <summary>
    /// Fournit des méthodes pour analyser une <see cref="MyImage"/> et afficher des statistiques sous forme d'image ou de texte.
    /// </summary>
    internal class MyImageStatistiques
    {
        //Champs

        private MyGraphics grapheHisto;
        private MyImage histo;
        private int[][] pixelsParIntensité;
        private int realWidth;
        private Pixel[,] couleursRGB;


        //Constructeurs

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="MyImageStatistiques"/> avec une <see cref="MyImage"/>
        /// </summary>
        /// <param name="image">Image</param>
        public MyImageStatistiques(MyImage image)
            : this(image, 512, 1024)
        {
        }

        /// <summary>
        /// Initialise une nouvelle instance de <see cref="MyImageStatistiques"/> avec une <see cref="MyImage"/> et les dimensions de l'histogramme à réaliser
        /// </summary>
        /// <param name="image">Image</param>
        /// <param name="height">Hauteur de l'histogramme en nombre pixel</param>
        /// <param name="width">Largeur de l'histogramme en nombre pixel</param>
        public MyImageStatistiques(MyImage image, int height, int width)
        {
            this.realWidth = width;
            //On est obligé de choisir une taille d'image correspondant au nombre de pixel avec lesquels créer l'histo sinon on laisse des trous
            //de manière irrégulière dans l'image. On change donc la taille mais on redimensionne l'image avec la taille spécifiée par l'utilisateur à la fin
            if (width < 256)
            {
                width = 256;
            }
            else if (width % 256 != 0)
            {
                double diff = width / 256.0;

                int incre = diff % 1 < 0.5 ? -1 : 1;
                while (width % 256 != 0)
                {
                    width += incre;
                }
            }
            width += 10;
            if (height < 10)
                height = 10;

            this.histo = new MyImage(height, width, Pixel.FromColor(Couleurs.Blanc));
            this.grapheHisto = new MyGraphics(histo);

            couleursRGB = new Pixel[3, 2];
            couleursRGB[0, 0] = Pixel.FromRGB(255, 80, 80);
            couleursRGB[0, 1] = Pixel.FromRGB(50, 20, 20);
            couleursRGB[1, 0] = Pixel.FromRGB(100, 255, 100);
            couleursRGB[1, 1] = Pixel.FromRGB(20, 50, 20);
            couleursRGB[2, 0] = Pixel.FromRGB(80, 80, 255);
            couleursRGB[2, 1] = Pixel.FromRGB(20, 20, 50);

            this.pixelsParIntensité = new int[3][];
            for (int i = 0; i < 3; i++)
            {
                this.pixelsParIntensité[i] = IntensiteParCouleur(image, i);
            }
        }


        //Méthodes statiques

        /// <summary>
        /// Renvoie un <see cref="Pixel"/> avec la couleur moyenne de l'image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Pixel GetAverageColor(MyImage image)
        {
            if (image == null)
                return Pixel.Zero;
            int[] colors = new int[3];

            int height = image.GetHeight;
            int width = image.GetWidth;

            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    colors[0] += image[i, j].GetR;
                    colors[1] += image[i, j].GetG;
                    colors[2] += image[i, j].GetB;
                }
            }
            int total = height * width;

            return new Pixel(colors[0] / total, colors[1] / total, colors[2] / total);
        }


        /// <summary>
        /// Renvoie la luminosité moyenne perçue par rapport à 255
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns></returns>
        public static int GetAverageBrightness(MyImage image)
        {
            double lumi = 0;

            for (int i = 0; i < image.GetHeight; ++i)
            {
                for (int j = 0; j < image.GetWidth; ++j)
                {
                    lumi += image[i, j].GetBrightness();
                }
            }
            lumi /= image.GetHeight * image.GetWidth;

            return (int)lumi;
        }


        /// <summary>
        /// Renvoie la teinte moyenne en degré % 360. 
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns></returns>
        public static int GetAverageHue(MyImage image)
        {
            double hue = 0;

            for (int i = 0; i < image.GetHeight; ++i)
            {
                for (int j = 0; j < image.GetWidth; ++j)
                {
                    hue += image[i, j].GetHue();
                }
            }
            hue /= image.GetHeight * image.GetWidth;

            return (int)hue;
        }

        /// <summary>
        /// Renvoie la luminosité moyenne par rapport à 1
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns></returns>
        public static float GetAverageLightness(MyImage image)
        {
            float lumi = 0;

            for (int i = 0; i < image.GetHeight; ++i)
            {
                for (int j = 0; j < image.GetWidth; ++j)
                {
                    lumi += image[i, j].GetLightness();
                }
            }
            lumi /= image.GetHeight * image.GetWidth;

            return lumi;
        }

        /// <summary>
        /// Renvoie la saturation moyenne par rapport à 1
        /// </summary>
        /// <param name="image">Image</param>
        /// <returns></returns>
        public static float GetAverageSaturation(MyImage image)
        {
            float lumi = 0;

            for (int i = 0; i < image.GetHeight; ++i)
            {
                for (int j = 0; j < image.GetWidth; ++j)
                {
                    lumi += image[i, j].GetSaturation();
                }
            }
            lumi /= image.GetHeight * image.GetWidth;

            return lumi;
        }


        private static int[] IntensiteParCouleur(MyImage image, int couleur)
        {
            int[] données = new int[256];
            for (int i = 0; i < image.GetHeight; i++)
            {
                for (int j = 0; j < image.GetWidth; j++)
                {
                    if (couleur == 0)
                    {
                        données[image[i, j].GetR] += 1;
                    }
                    else if (couleur == 1)
                    {
                        données[image[i, j].GetG] += 1;
                    }
                    else
                    {
                        données[image[i, j].GetB] += 1;
                    }

                }
            }

            return données;
        }


        private static int Max(int[] tab)
        {
            int max = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] > max)
                    max = tab[i];
            }
            return max;
        }


        //Histogramme

        /// <summary>
        /// Crée un histogramme de l'image du nombre de pixels en fonction de l'intensité (0-255) selon un <see cref="HistogrammeMode"/> et des paramètres pré-définis
        /// </summary>
        /// <param name="mode">Type d'histogramme</param>
        /// <param name="remplissage">Les lignes crées sont remplies jusqu'en bas de l'image pour une meilleure visualisation</param>
        public MyImage CreateHistogramme(HistogrammeMode mode, bool remplissage = false)
        {
            int hauteur = this.histo.GetHeight;
            const int décallageW = 4;
            const int décallageH = -2;

            if (mode == HistogrammeMode.Echelle_Couleurs)
            {
                int max = Math.Max(Max(pixelsParIntensité[0]), Math.Max(Max(pixelsParIntensité[1]), Max(pixelsParIntensité[2])));

                if (remplissage) //Remplit par la couleur spécifiée le dessous de la courbe d'intensité
                {
                    for (int j = 0; j < 3; j++)
                    {
                        int incrementation = (int)Math.Floor((double)this.histo.GetWidth / this.pixelsParIntensité[j].Length);
                        for (int i = 0; i < pixelsParIntensité[j].Length - 1; i++)
                        {
                            Point hautGauche = new Point((double)hauteur + décallageH - (double)pixelsParIntensité[j][i] * (hauteur + 2 * décallageH) / max, incrementation * i + décallageW);
                            Point hautDroite = new Point((double)hauteur + décallageH - (double)pixelsParIntensité[j][i + 1] * (hauteur + 2 * décallageH) / max, incrementation * (i + 1) + décallageW);
                            Point basGauche = new Point(this.histo.GetHeight - 1 + décallageH, hautGauche.X);
                            Point basDroite = new Point(this.histo.GetHeight - 1 + décallageH, hautDroite.X);

                            double rapportOrigine = (hautGauche.X - décallageW) / this.histo.GetWidth; //Distance par rapport à l'axe des ordonnées à l'origine du repère + décallageW

                            byte couleurR = (byte)Math.Min(Math.Max(0, this.couleursRGB[j, 0].GetR * rapportOrigine + this.couleursRGB[j, 1].GetR * (1 - rapportOrigine)) + 10, 255);

                            byte couleurG = (byte)Math.Min(Math.Max(0, this.couleursRGB[j, 0].GetG * rapportOrigine + this.couleursRGB[j, 1].GetG * (1 - rapportOrigine)) + 10, 255);

                            byte couleurB = (byte)Math.Min(Math.Max(0, this.couleursRGB[j, 0].GetB * rapportOrigine + this.couleursRGB[j, 1].GetB * (1 - rapportOrigine)) + 10, 255);

                            Pixel color = new Pixel(couleurR, couleurG, couleurB);

                            this.grapheHisto.FillRectangle(hautGauche, basDroite, color);

                            if (i == this.pixelsParIntensité[0].Length - 2) //Dernier
                            {
                                hautGauche = hautDroite;
                                basDroite = new Point(this.histo.GetHeight - 1 + décallageH, hautGauche.X + incrementation);

                                this.grapheHisto.FillRectangle(hautGauche, basDroite, color);
                            }

                        }
                    }
                }

                for (int j = 2; j >= 0; j--)
                {
                    int incrementation = this.histo.GetWidth / this.pixelsParIntensité[j].Length;
                    Couleurs color = j == 0 ? Couleurs.Rouge_Foncé : j == 1 ? Couleurs.Vert_Foncé : Couleurs.Bleu_Marine;

                    for (int i = 0; i < pixelsParIntensité[j].Length - 1; i++)
                    {
                        Point hautGauche = new Point((double)hauteur - 1 + décallageH - (double)pixelsParIntensité[j][i] * (hauteur + 2 * décallageH) / max, incrementation * (i + 0.5) + décallageW);
                        Point hautDroite = new Point((double)hauteur - 1 + décallageH - (double)pixelsParIntensité[j][i + 1] * (hauteur + 2 * décallageH) / max, incrementation * (i + 1.5) + décallageW);

                        for (int k = 0; k < 3; k++) //Bug, parfois des lignes traversent toute l'image
                        {
                            this.grapheHisto.DrawLineWu(hautGauche, hautDroite, Pixel.FromColor(color));

                            hautGauche = new Point(hautGauche.Y + 1, hautGauche.X + 0.25);
                            hautDroite = new Point(hautDroite.Y + 1, hautDroite.X + 0.25);
                        }

                    }
                }
            }
            else if (mode == HistogrammeMode.Echelle_Gris)
            {
                int incrementation = (int)Math.Floor((double)this.histo.GetWidth / this.pixelsParIntensité[0].Length);
                int max = (Max(pixelsParIntensité[0]) + Max(pixelsParIntensité[1]) + Max(pixelsParIntensité[2])) / 3;
                for (int i = 1; i < this.pixelsParIntensité[0].Length - 1; i++)
                {
                    int moyenne = (this.pixelsParIntensité[0][i] + this.pixelsParIntensité[1][i] + this.pixelsParIntensité[2][i]) / 3;
                    Point hautGauche = new Point((double)hauteur + décallageH - (moyenne * (hauteur + 2.0 * décallageH) / max), incrementation * (i + 0.5) + décallageW);
                    moyenne = (this.pixelsParIntensité[0][i + 1] + this.pixelsParIntensité[1][i + 1] + this.pixelsParIntensité[2][i + 1]) / 3;
                    Point hautDroite = new Point((double)hauteur + décallageH - (moyenne * (hauteur + 2.0 * décallageH) / max), incrementation * (i + 1.5) + décallageW);

                    double rapportOrigine = (hautGauche.X - décallageW) / this.histo.GetWidth; //Distance par rapport à l'axe des ordonnées à l'origine du repère + décallageW

                    byte couleur = (byte)Math.Min(Math.Max(0, Pixel.FromColor(Couleurs.Gris_Clair).GetMoyenne * rapportOrigine + Pixel.FromColor(Couleurs.Noir_Clair).GetMoyenne * (1 - rapportOrigine)) + 10, 255);

                    if (remplissage)
                    {
                        hautGauche.Y += 1;
                        hautDroite.Y += 1;
                        Point basDroite = new Point((double)this.histo.GetHeight - 1 + décallageH, hautDroite.X);

                        this.grapheHisto.FillRectangle(hautGauche, basDroite, Pixel.FromRGB(couleur, couleur, couleur));
                    }

                    for (int k = 0; k < 3; k++) //Bug, parfois y'a des lignes qui traversent toute l'image
                    {
                        this.grapheHisto.DrawLineBresenham(hautGauche, hautDroite, Pixel.FromColor(Couleurs.Noir));

                        hautGauche = new Point(hautGauche.Y + 1, hautGauche.X + 0.25);
                        hautDroite = new Point(hautDroite.Y + 1, hautDroite.X + 0.25);
                    }

                    if (remplissage && i == this.pixelsParIntensité[0].Length - 2)
                    {
                        hautGauche = hautDroite;
                        Point basDroite = new Point((double)this.histo.GetHeight - 1 + décallageH, hautGauche.X + incrementation);

                        this.grapheHisto.FillRectangle(hautGauche, basDroite, Pixel.FromRGB(couleur, couleur, couleur));
                    }
                }

            }

            if (this.realWidth != this.histo.GetWidth) //On redimensionne l'image vers ce que l'utilisateur a choisi 
            {
                this.grapheHisto.KeepAspectRatio = false;
                this.grapheHisto.Quality = InterpolationMode.Bicubique;
                this.grapheHisto.Redimensionnement(this.histo.GetHeight, this.realWidth);
            }


            //Cadre

            Point hautGaucheLigneNoire = new Point(4, 4);
            Point basDroiteLigneNoire = new Point(this.histo.GetHeight - 1, this.histo.GetWidth - 5);

            this.grapheHisto.DrawRectangle(hautGaucheLigneNoire, basDroiteLigneNoire, Pixel.FromColor(Couleurs.Noir_Clair));
            this.grapheHisto.DrawRectangle(new Point(hautGaucheLigneNoire.Y - 1, hautGaucheLigneNoire.X - 1),
                new Point(basDroiteLigneNoire.Y, basDroiteLigneNoire.X + 1), Pixel.FromColor(Couleurs.Noir_Clair));
            this.grapheHisto.DrawRectangle(new Point(hautGaucheLigneNoire.Y - 2, hautGaucheLigneNoire.X - 2),
                new Point(basDroiteLigneNoire.Y, basDroiteLigneNoire.X + 2), Pixel.FromColor(Couleurs.Noir_Clair));


            return this.histo;
        }


        /// <summary>
        /// Teste si un <see cref="Point"/> est présent dans l'image
        /// </summary>
        private bool TestPoint(Point p1)
        {
            if (p1.X < 0)
                return false;
            else if (p1.X > this.histo.GetWidth - 1)
                return false;

            if (p1.Y < 0)
                return false;
            else if (p1.Y > this.histo.GetHeight - 1)
                return false;

            return true;
        }

    }
}
