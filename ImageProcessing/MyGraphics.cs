using System;
using System.Collections.Generic;
using System.Linq;

namespace ImageProcessing
{

    /// <summary>
    /// Méthodes d'interpolation utilisées pour l'aggrandissement, le rétrécissement et la rotation d'une <see cref="MyImage"/>
    /// </summary>
    public enum InterpolationMode
    {
        /// <summary>
        /// Interpolation du voisin le plus proche. Avantage : aucune perte d'information lorsqu'on augmente la taille, rapide à process
        /// </summary>
        NearestNeighbour,

        /// <summary>
        /// On réalise une moyenne des pixels les plus proche (4) du pixel d'arrivée en fonction de leur distance à celui-ci. Avantage : précis, résultat plus proche de ce qui est attendu en général lorsqu'on zoom sur une image
        /// </summary>
        Bilineaire,

        /// <summary>
        /// Interpolation bicubique, prend en compte les 16 pixels les plus proches. Plus précis que l'interpolation bilinéaire
        /// </summary>
        Bicubique
    }


    /// <summary>
    /// Fournit des méthodes pour modifier une <see cref="MyImage"/>. Les changements sont effectués directement sur l'instance du <see cref="MyImage"/> entré en paramètre
    /// </summary>
    public class MyGraphics
    {
        //Champs et propriétés

        private MyImage image;

        private Random rand = new Random(Guid.NewGuid().GetHashCode());


        /// <summary>
        /// Récupère l'instance <see cref="MyImage"/> utilisée, la même que celle utilisée pour créer cette instance
        /// </summary>
        public MyImage GetMyImage => image;

        /// <summary>
        /// Contrôle le type de méthode utilisée pour le redimensionnement et la rotation d'une <see cref="MyImage"/>
        /// </summary>
        public InterpolationMode Quality { get; set; }

        /// <summary>
        /// Lors d'un changement de taille de l'image, contrôle le respect ou non du ratio Hauteur/Largeur de l'image d'origine. Les bords vides sont alors remplis par <see cref="PixelRemplissage"/><para/>
        /// Lors d'une rotation de l'image, contrôle le respect de la taille de l'image d'origine. Les bords vides sont alors remplis par <see cref="PixelRemplissage"/><para/>
        /// Lors d'une pixélisation d'une image, contrôle le respect ou non de la taille d'un nouveau pixel pour que les bords n'aient pas de "petits" pixels
        /// </summary>
        public bool KeepAspectRatio { get; set; }

        /// <summary>
        /// Pixel qui occasionnellement remplira les trous laissés par une image tournée ou redimensionnée
        /// </summary>
        public Pixel PixelRemplissage { get; set; }


        //Constructeur

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MyGraphics"/>. Réalise des opérations sur l'instance du <see cref="MyImage"/> entré en paramètre
        /// </summary>Instance sur laquelle les opérations sont effectuées
        /// <param name="image">Image sur laquelle réaliser des opérations</param>
        public MyGraphics(MyImage image)
        {
            this.image = image;
            this.KeepAspectRatio = true;
            this.PixelRemplissage = Pixel.Zero;
            this.Quality = InterpolationMode.NearestNeighbour;
        }


        /// <summary>
        /// Clone les pixels d'une <see cref="MyImage"/> dans l'instance de la <see cref="MyImage"/> de ce <see cref="MyGraphics"/>
        /// </summary>
        /// <param name="myImage"></param>
        private void CloneImage(MyImage myImage)
        {
            if (this.image.Equals(myImage)) //meme ref, si on réinitialise le tab de pixel de l'un on réinitialise aussi l'autre, donc crash
                return;

            this.image.ChangeSize(myImage.GetHeight, myImage.GetWidth);

            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    this.image[i, j] = myImage[i, j];
                }
            }
        }


        //Méthodes de Rognage

        #region Rognage

        /// <summary>
        /// Effectue un rognage dans l'image entre les coordonnées spécifiées. N'aggrandie pas la nouvelle image
        /// </summary>
        /// <param name="hautGauche">Coin en haut à gauche de la nouvelle image</param>
        /// <param name="basDroit">Coin en bas à droite de la nouvelle image</param>
        public void Rognage(Point hautGauche, Point basDroit)
        {
            MyImage im = InternalRognage(this.image, hautGauche, basDroit);

            if (im != null)
                CloneImage(im);
        }

        /// <summary>
        /// Renvoie un rognage de l'image entre les coordonnées spécifiées
        /// </summary>
        /// <param name="hautGauche">Coin en haut à gauche de la nouvelle image</param>
        /// <param name="basDroit">Coin en bas à droite de la nouvelle image</param>
        public MyImage GetRognage(Point hautGauche, Point basDroit)
        {
            return InternalRognage(this.image, hautGauche, basDroit);
        }


        private static MyImage InternalRognage(MyImage image, Point hautGauche, Point basDroit)
        {
            if (hautGauche.X > basDroit.X && hautGauche.Y > basDroit.Y)
            {
                Point temp = basDroit;
                basDroit = hautGauche;
                hautGauche = temp;
            }
            if (hautGauche.X > basDroit.X && hautGauche.Y < basDroit.Y)
            {
                double tempX = hautGauche.X;
                hautGauche.X = basDroit.X;
                basDroit.X = tempX;
            }
            if (hautGauche.X < basDroit.X && hautGauche.Y > basDroit.Y)
            {
                double tempY = hautGauche.Y;
                hautGauche.Y = basDroit.Y;
                basDroit.Y = tempY;
            }

            if (hautGauche.X < 0 || hautGauche.X >= image.GetWidth ||
                hautGauche.Y < 0 || hautGauche.Y >= image.GetHeight ||
                hautGauche.X > basDroit.X || hautGauche.Y > basDroit.Y)
            {
                return null;
            }

            int différenceHeight = (int)hautGauche.Y;
            int différenceWidth = (int)hautGauche.X;

            int maxX = (int)Math.Min(basDroit.X - hautGauche.X, image.GetWidth - hautGauche.X);
            int maxY = (int)Math.Min(basDroit.Y - hautGauche.Y, image.GetHeight - hautGauche.Y);


            MyImage newMyImage = new MyImage(maxY, maxX);


            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                {
                    newMyImage[i, j] = image[i + différenceHeight, j + différenceWidth].Clone();
                }
            }
            return newMyImage;
        }

        #endregion


        //Rotation et redimensionnement

        #region Rotation + Redimensionnement

        /// <summary>
        /// Tourne une matrice avec l'angle en degré spécifié et le mode d'interpolation pré-définie. La taille de l'image est adaptée ou non selon <see cref="KeepAspectRatio"/>
        /// </summary>
        /// <param name="degré">Angle de rotation, sens inverse du sens trigo</param>
        /// <param name="bordsCouleurIm">Gère la couleur des trous laissés par l'image. <see langword="false"/> = remplissage par <see cref="PixelRemplissage"/>. Sinon remplissage par la couleur des bords</param>
        public void Rotation(double degré, bool bordsCouleurIm = false)
        {
            Rotation(degré, bordsCouleurIm, this.Quality);
        }

        /// <summary>
        /// Tourne une matrice avec l'angle en degré spécifié et le mode d'interpolation pré-définie. La taille de l'image est adaptée ou non selon <see cref="KeepAspectRatio"/>
        /// </summary>
        /// <param name="degré">Angle de rotation, sens inverse du sens trigo</param>
        /// <param name="bordsCouleurIm">Gère la couleur des trous laissés par l'image. <see langword="false"/> = remplissage par <see cref="PixelRemplissage"/>. Sinon remplissage par la couleur des bords</param>
        /// <param name="quality">Type de méthode utilisée pour l'interpolation d'un pixel</param>
        public void Rotation(double degré, bool bordsCouleurIm, InterpolationMode quality)
        {
            this.Quality = quality;

            double rad = Math.PI * (degré % 360) / 180;

            double sin = Math.Sin(rad);
            double cos = Math.Cos(rad);


            double targetHeight = 0;
            double targetWidth = 0;

            if (this.KeepAspectRatio)
            {
                targetHeight = this.image.GetHeight;
                targetWidth = this.image.GetWidth;
            }
            else
            {
                targetHeight = Math.Abs(this.image.GetHeight * cos) + Math.Abs(this.image.GetWidth * sin);
                targetWidth = Math.Abs(this.image.GetWidth * cos) + Math.Abs(this.image.GetHeight * sin);
            }

            MyImage newImage = new MyImage((int)targetHeight, (int)targetWidth);

            sin = -sin;  //On inverse le sinus pour avoir la rotation dans le sens inverse du sens trigo

            int hwidth = (int)((targetWidth) / 2.0);
            int hheight = (int)((targetHeight) / 2.0);

            for (int i = 0; i < newImage.GetHeight; i++)
            {
                for (int j = 0; j < newImage.GetWidth; j++)
                {
                    double rapportY = i - hheight; //On se place par rapport au centre de la nouvelle image
                    double rapportX = j - hwidth;

                    double x = cos * rapportX - sin * rapportY + (this.image.GetWidth - 1) / 2.0;  //Position du pixel tourné
                    double y = sin * rapportX + cos * rapportY + (this.image.GetHeight - 1) / 2.0;

                    if (x < 0 && x * (-1000) < 1) //imprecisions de Math.Cos et Math.Sin
                    {
                        x = 0;
                    }
                    if (y < 0 && y * (-1000) < 1)
                    {
                        y = 0;
                    }

                    if (!bordsCouleurIm && (y < 0 || x < 0 || x >= this.image.GetWidth || y >= this.image.GetHeight)) //En dehors du cadre
                    {
                        newImage[i, j] = this.PixelRemplissage;
                    }
                    else
                    {
                        newImage[i, j] = GetPixelInterpolé(new Point(y, x)); //On interpole en fct du Mode pré-défini
                    }

                }
            }

            CloneImage(newImage);
        }


        /// <summary>
        /// Adapte l'image en fonction d'un rapport d'aggrandissement ou de rétrécissement. Avec 1, la taille de l'image de base
        /// </summary>
        /// <param name="rapport">Rapport de changement de la taille d'une image avec 1 la taille actuelle de l'image</param>
        public void Redimensionnement(double rapport)
        {
            double newHeight = this.image.GetHeight * rapport;
            double newWidth = this.image.GetWidth * rapport;

            Redimensionnement((int)newHeight, (int)newWidth);
        }

        /// <summary>
        /// Adapte l'image en fonction de nouveaux paramètres de hauteur et de largeur
        /// </summary>
        /// <param name="targetHeight">Nouvelle taille de l'image en hauteur</param>
        /// <param name="targetWidth">Nouvelle taille de l'image en largeur</param>
        public void Redimensionnement(int targetHeight, int targetWidth)
        {
            Redimensionnement(targetHeight, targetWidth, this.Quality);
        }

        /// <summary>
        /// Adapte l'image en fonction de nouveaux paramètres de hauteur et de largeur et d'une méthode d'interpolation spécifiée
        /// </summary>
        /// <param name="targetHeight">Nouvelle taille de l'image en hauteur</param>
        /// <param name="targetWidth">Nouvelle taille de l'image en largeur</param>
        /// <param name="quality">Mode d'interpolation</param>
        public void Redimensionnement(int targetHeight, int targetWidth, InterpolationMode quality)
        {
            this.Quality = quality;

            int imageTargetHeight = targetHeight, imageTargetWidth = targetWidth;

            //On garde les dimensions de l'image d'origine. L'image aggrandie sera au milieu (soit en largeur soit en hauteur) d'une image avec les dimensions demandées par l'utilisateur
            if (this.KeepAspectRatio)
            {
                double ratioW = (double)targetWidth / this.image.GetWidth;
                double ratioH = (double)targetHeight / this.image.GetHeight;

                if (ratioH > ratioW)
                {
                    imageTargetHeight = (int)(this.image.GetHeight * ratioW);
                }
                else
                {
                    imageTargetWidth = (int)(this.image.GetWidth * ratioH);
                }
            }

            double rapportHeight = 0;
            double rapportWidth = 0;

            if (quality == InterpolationMode.Bicubique || quality == InterpolationMode.Bilineaire) //???
            {
                rapportHeight = (double)imageTargetHeight / (this.image.GetHeight - 1);
                rapportWidth = (double)imageTargetWidth / (this.image.GetWidth - 1);
            }
            else
            {
                rapportHeight = (double)imageTargetHeight / (this.image.GetHeight);
                rapportWidth = (double)imageTargetWidth / (this.image.GetWidth);
            }

            MyImage newImage = new MyImage(imageTargetHeight, imageTargetWidth);

            for (int i = 0; i < newImage.GetHeight; i++)
            {
                for (int j = 0; j < newImage.GetWidth; j++)
                {
                    newImage[i, j] = GetPixelInterpolé(new Point(i / rapportHeight, j / rapportWidth)); //Interpolation de chaque nvx pixel
                }
            }

            if (this.KeepAspectRatio) //On crée une nouvelle image avec l'image dont on vient de changer la taille au centre
            {
                MyImage imageCentrée = new MyImage(targetHeight, targetWidth);
                int milieuY = Math.Abs((targetHeight - imageTargetHeight) / 2);
                int milieuX = Math.Abs((targetWidth - imageTargetWidth) / 2);

                for (int i = 0; i < targetHeight; i++)
                {
                    for (int j = 0; j < targetWidth; j++)
                    {
                        if (i < milieuY || i >= imageTargetHeight + milieuY || j < milieuX || j >= imageTargetWidth + milieuX)
                        {
                            imageCentrée[i, j] = this.PixelRemplissage;
                        }
                        else
                        {
                            imageCentrée[i, j] = newImage[i - milieuY, j - milieuX];
                        }

                    }
                }
                CloneImage(imageCentrée);
            }
            else
            {
                CloneImage(newImage);
            }
        }



        /// <summary>
        /// Crée un pixel en fonction d'une position d'origine dans une image et d'une méthode <see cref="InterpolationMode"/>
        /// </summary>
        /// <param name="origine">Position à partir de la quelle créer un nouveau <see cref="Pixel"/></param>
        /// <returns></returns>
        private Pixel GetPixelInterpolé(Point origine)
        {
            int diffMin = this.Quality == InterpolationMode.Bicubique ? 1 : 0;
            int diffMax = this.Quality == InterpolationMode.Bicubique ? 3 : 1;

            int minY = (int)origine.Y - diffMin;
            int minX = (int)origine.X - diffMin;

            int maxY = minY + diffMax;
            int maxX = minX + diffMax;


            if (this.Quality == InterpolationMode.NearestNeighbour || this.Quality == InterpolationMode.Bilineaire)
            {
                List<Point> points = new List<Point>();
                for (int i = minY; i <= maxY; i++)
                {
                    for (int j = minX; j <= maxX; j++)
                    {
                        double posY = i;
                        double posX = j;

                        if (this.Quality == InterpolationMode.Bilineaire)
                        {
                            if (posY >= this.image.GetHeight)
                                posY = this.image.GetHeight - 2;
                            if (posX >= this.image.GetWidth)
                                posX = this.image.GetWidth - 2;
                        }
                        else
                        {
                            posY += 0.5;
                            posX += 0.5;
                            if (posY < 0) posY = 0.5f;
                            if (posY >= this.image.GetHeight) posY = this.image.GetHeight - 0.5;
                            if (posX < 0) posX = 0.5f;
                            if (posX >= this.image.GetWidth) posX = this.image.GetWidth - 0.5;
                        }

                        points.Add(new Point(posY, posX));
                    }
                }

                if (this.Quality == InterpolationMode.NearestNeighbour)
                {
                    Point nearest = GetNearestPoint(origine, points);
                    if (nearest == Point.Zero)
                        return this.PixelRemplissage;
                    return this.image[(int)nearest.Y, (int)nearest.X];
                }
                else
                {
                    return GetInterpolationBilineaire(origine, points);
                }
            }
            else  //Méthode d'Hermite
            {
                Pixel[,] voisins = new Pixel[4, 4]; //Voisins du pixel

                for (int i = 0; i < voisins.GetLength(0); i++)
                {
                    for (int j = 0; j < voisins.GetLength(1); j++)
                    {
                        int posY = minY + i;
                        int posX = minX + j;

                        if (posY < 0) posY = 0;
                        if (posY >= this.image.GetHeight) posY = this.image.GetHeight - 1;
                        if (posX < 0) posX = 0;
                        if (posX >= this.image.GetWidth) posX = this.image.GetWidth - 1;

                        voisins[i, j] = this.image[posY, posX];
                    }
                }

                double xFrac = origine.X - (int)origine.X;
                double yFrac = origine.Y - (int)origine.Y;

                byte[] moyenne = new byte[3];

                //On récupère la valeur d'intensité de pixel pour chaque colonne et enfin on détermine la solution de l'equation générée avec ces 4 intensités en ligne

                float colonne0 = GetInterpolationBicubique(voisins[0, 0].GetR, voisins[1, 0].GetR, voisins[2, 0].GetR, voisins[3, 0].GetR, (float)yFrac);
                float colonne1 = GetInterpolationBicubique(voisins[0, 1].GetR, voisins[1, 1].GetR, voisins[2, 1].GetR, voisins[3, 1].GetR, (float)yFrac);
                float colonne2 = GetInterpolationBicubique(voisins[0, 2].GetR, voisins[1, 2].GetR, voisins[2, 2].GetR, voisins[3, 2].GetR, (float)yFrac);
                float colonne3 = GetInterpolationBicubique(voisins[0, 3].GetR, voisins[1, 3].GetR, voisins[2, 3].GetR, voisins[3, 3].GetR, (float)yFrac);
                float valeurPixel = GetInterpolationBicubique(colonne0, colonne1, colonne2, colonne3, (float)xFrac);
                moyenne[0] = (byte)Math.Max(0, Math.Min(valeurPixel, 255));

                colonne0 = GetInterpolationBicubique(voisins[0, 0].GetG, voisins[1, 0].GetG, voisins[2, 0].GetG, voisins[3, 0].GetG, (float)yFrac);
                colonne1 = GetInterpolationBicubique(voisins[0, 1].GetG, voisins[1, 1].GetG, voisins[2, 1].GetG, voisins[3, 1].GetG, (float)yFrac);
                colonne2 = GetInterpolationBicubique(voisins[0, 2].GetG, voisins[1, 2].GetG, voisins[2, 2].GetG, voisins[3, 2].GetG, (float)yFrac);
                colonne3 = GetInterpolationBicubique(voisins[0, 3].GetG, voisins[1, 3].GetG, voisins[2, 3].GetG, voisins[3, 3].GetG, (float)yFrac);
                valeurPixel = GetInterpolationBicubique(colonne0, colonne1, colonne2, colonne3, (float)xFrac);
                moyenne[1] = (byte)Math.Max(0, Math.Min(valeurPixel, 255));

                colonne0 = GetInterpolationBicubique(voisins[0, 0].GetB, voisins[1, 0].GetB, voisins[2, 0].GetB, voisins[3, 0].GetB, (float)yFrac);
                colonne1 = GetInterpolationBicubique(voisins[0, 1].GetB, voisins[1, 1].GetB, voisins[2, 1].GetB, voisins[3, 1].GetB, (float)yFrac);
                colonne2 = GetInterpolationBicubique(voisins[0, 2].GetB, voisins[1, 2].GetB, voisins[2, 2].GetB, voisins[3, 2].GetB, (float)yFrac);
                colonne3 = GetInterpolationBicubique(voisins[0, 3].GetB, voisins[1, 3].GetB, voisins[2, 3].GetB, voisins[3, 3].GetB, (float)yFrac);
                valeurPixel = GetInterpolationBicubique(colonne0, colonne1, colonne2, colonne3, (float)xFrac);
                moyenne[2] = (byte)Math.Max(0, Math.Min(valeurPixel, 255));


                return new Pixel(moyenne[0], moyenne[1], moyenne[2]);
            }
        }


        /// <summary>
        /// Renvoie la solution de f(t) avec t compris entre 0 et 1. Les autres paramètres sont les positions en x ou y des pixels à interpoler de gauche à droite. Basé sur le polynôme d'Hermite
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="C"></param>
        /// <param name="D"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static float GetInterpolationBicubique(float A, float B, float C, float D, float t)
        {
            //On a f(t) = at^3 + bt² + ct + d, le but est de trouver a, b, c et d pour ensuite trouver la solution de l'equation f(t)
            //On sait que f(0) = d, f(1) = a + b + c + d, f'(0) = c et f'(1) = 3at² + 2bt + c
            //On a alors a = 2f(0) − 2f(1) + f′(0) + f′(1)  et  b = −3f(0) + 3f(1) − 2f′(0) − f′(1), on sait que f(0) = (x0, B) et f(1) = (x1, C) donc f(0) = B et f(1) = C
            //Il vient alors f'(0) = (C - A) / 2  et f'(1) = (D - B) / 2 donc :

            float a = -(A / 2.0f) + (3.0f * B / 2.0f) - (3.0f * C / 2.0f) + (D / 2.0f);
            float b = A - (5.0f * B / 2.0f) + (2.0f * C) - (D / 2.0f);
            float c = -(A / 2.0f) + (C / 2.0f);
            float d = B;

            return a * t * t * t + b * t * t + c * t + d; //Equation polynomiale du 3ème degré
        }

        /// <summary>
        /// Compare la distance d'une <see cref="List{T}"/> de <see cref="Point"/> avec un <see cref="Point"/> d'origine et retourne le <see cref="Point"/> le plus proche dans cette <see cref="List{T}"/>
        /// </summary>
        private static Point GetNearestPoint(Point origine, List<Point> points)
        {
            int miniIndex = 0;
            double miniDistance = double.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                double newDist = Math.Sqrt(Math.Pow(points[i].X - origine.X, 2) + Math.Pow(points[i].Y - origine.Y, 2));
                if (miniDistance > newDist)
                {
                    miniIndex = i;
                    miniDistance = newDist;
                }
            }
            return points[miniIndex];
        }

        /// <summary>
        /// Renvoie le pixel moyen déterminé en fonction de la distance des pixels au point d'origine
        /// </summary>
        /// <param name="origine"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private Pixel GetInterpolationBilineaire(Point origine, List<Point> points)
        {
            if (points == null || points.Count == 0) //Sécurité, mais inutile les exceptions sont gérées dans les autres méthodes
                return this.PixelRemplissage;

            double x = origine.X;
            double y = origine.Y;

            double x1 = points[0].X;
            double x2 = points[1].X;
            double y1 = points[0].Y;
            double y2 = points[2].Y;

            //En dehors des limites, on renvoie noir
            if (x1 >= this.image.GetWidth || x2 >= this.image.GetWidth || y1 >= this.image.GetHeight || y2 >= this.image.GetHeight)
                return this.PixelRemplissage;


            int posX1 = (int)x1;
            int posX2 = (int)x2;
            int posY2 = (int)y2;
            int posY1 = (int)y1;


            double R1 = (x2 - x) / (x2 - x1) * this.image[posY1, posX1].GetR
                      + (x - x1) / (x2 - x1) * this.image[posY1, posX2].GetR;

            double R2 = (x2 - x) / (x2 - x1) * this.image[posY2, posX1].GetR
                      + (x - x1) / (x2 - x1) * this.image[posY2, posX2].GetR;

            double Red = ((y2 - y) / (y2 - y1) * R1) + ((y - y1) / (y2 - y1) * R2);



            double G1 = (x2 - x) / (x2 - x1) * this.image[posY1, posX1].GetG
                      + (x - x1) / (x2 - x1) * this.image[posY1, posX2].GetG;

            double G2 = (x2 - x) / (x2 - x1) * this.image[posY2, posX1].GetG
                      + (x - x1) / (x2 - x1) * this.image[posY2, posX2].GetG;

            double Green = ((y2 - y) / (y2 - y1) * G1) + ((y - y1) / (y2 - y1) * G2);



            double B1 = (x2 - x) / (x2 - x1) * this.image[posY1, posX1].GetB
                      + (x - x1) / (x2 - x1) * this.image[posY1, posX2].GetB;

            double B2 = (x2 - x) / (x2 - x1) * this.image[posY2, posX1].GetB
                      + (x - x1) / (x2 - x1) * this.image[posY2, posX2].GetB;

            double Blue = ((y2 - y) / (y2 - y1) * B1) + ((y - y1) / (y2 - y1) * B2);




            return new Pixel((byte)Math.Max(Math.Min(Math.Round(Red), 255), 0), (byte)Math.Max(Math.Min(Math.Round(Green), 255), 0), (byte)Math.Max(Math.Min(Math.Round(Blue), 255), 0));
        }

        #endregion


        //Transformations des couleurs

        #region Transformations couleurs

        /// <summary>
        /// Inverse les couleurs des pixels d'un <see cref="MyImage"/>
        /// </summary>
        public void InversionCouleurs()
        {
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    this.image[i, j] = this.image[i, j].InversionCouleur();
                }
            }
        }


        /// <summary>
        /// Transforme une image en nuance de gris
        /// </summary>
        public void TransformationGris()
        {
            TransformationNoirEtBlanc(0);
        }

        /// <summary>
        /// Transforme les pixels d'un <see cref="MyImage"/> en des nuances de gris, de noir et de blanc en fonction d'un facteur %. <para/>100% revient à changer la photo qu'en noir et qu'en blanc, 0 revient à mettre la photo en nuance de gris normalement
        /// </summary>
        public void TransformationNoirEtBlanc(int intensitéPourcentage)
        {
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    this.image[i, j] = this.image[i, j].TransformationGris(intensitéPourcentage / 2);
                }
            }
        }

        /// <summary>
        /// Transforme une image en sépia
        /// </summary>
        public void TransformationSépia()
        {
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    this.image[i, j] = this.image[i, j].TransformationSépia();
                }
            }
        }

        /// <summary>
        /// Modifie la luminosité de l'image en fonction d'un facteur pourcentage : 50 = luminosité actuelle, 100 = tout blanc, 0 = tout noir
        /// </summary>
        /// <param name="brightnessPourcentage"></param>
        public void TransformationLuminositéPerçue(int brightnessPourcentage)
        {
            brightnessPourcentage -= (100 - brightnessPourcentage); //On passe le pourcentage de 0->100 à -100 -> 100 pour simplifier les calculs
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    this.image[i, j] = this.image[i, j].TransformationLuminosité(brightnessPourcentage);
                }
            }
        }


        /// <summary>
        /// Atténue les couleurs d'une image
        /// </summary>
        /// <param name="couleurChgmtIntensité">Pourcentage d'atténuation (0% = noir et blanc, 100% = couleurs normales)</param>
        public void TransformationCouleurIntensité(int couleurChgmtIntensité)
        {
            MyGraphics g = new MyGraphics(this.image.GetCopie());
            g.TransformationGris();

            this.AddImage(g.GetMyImage, Point.Zero, couleurChgmtIntensité);
        }


        /// <summary>
        /// Transforme l'image en ne retenant que le composant rouge 
        /// </summary>
        public void TransformationRouge()
        {
            TransformationRGB(true, false, false);
        }

        /// <summary>
        /// Transforme l'image en ne retenant que le composant vert
        /// </summary>
        public void TransformationVert()
        {
            TransformationRGB(false, true, false);
        }

        /// <summary>
        /// Transforme l'image en ne retenant que le composant bleu
        /// </summary>
        public void TransformationBleu()
        {
            TransformationRGB(false, false, true);
        }


        /// <summary>
        /// Transforme l'image en ne retenant que les composants RGB sélectionnés
        /// </summary>
        public void TransformationRGB(bool rouge, bool vert, bool bleu)
        {
            if (rouge && vert && bleu)
                return;
            if (!rouge && !vert && !bleu)
            {
                CloneImage(new MyImage(this.image.GetHeight, this.image.GetWidth, Pixel.Zero));
                return;
            }

            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    Pixel p = this.image[i, j];

                    int r = p.GetR, v = p.GetG, b = p.GetB;

                    if (!rouge)
                        r = 0;
                    if (!vert)
                        v = 0;
                    if (!bleu)
                        b = 0;

                    this.image[i, j] = new Pixel(r, v, b);
                }
            }
        }

        #endregion


        //Effet miroir

        #region Effet miroir

        /// <summary>
        /// Inverse les pixels de droite à gauche d'un <see cref="MyImage"/> par rapport au milieu
        /// </summary>
        public void EffetMiroir()
        {
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth / 2; j++)
                {
                    int X = this.image.GetWidth - j - 1;
                    int Y = i;

                    Pixel pixelTemp = this.image[i, j];

                    this.image[i, j] = this.image[Y, X];
                    this.image[Y, X] = pixelTemp;
                }
            }
        }


        /// <summary>
        /// L'image de l'instance est ajoutée à côté d'elle même et/ou sous elle de manière symétrique.
        /// </summary>
        /// <param name="effetQuadruple">L'image est démultipliée 4 fois au lieu de 2</param>
        public void EffetMiroirDoubleOuQuadruple(bool effetLargeur, bool effetHauteur)
        {
            MyImage myImage = new MyImage(this.image.GetHeight * (effetHauteur ? 2 : 1), this.image.GetWidth * (effetLargeur ? 2 : 1));

            for (int i = 0; i < myImage.GetHeight; i++)
            {
                for (int j = 0; j < myImage.GetWidth; j++)
                {
                    int targetThisY = 0;
                    int targetThisX = 0;

                    if (i < this.image.GetHeight)
                        targetThisY = i;
                    else
                        targetThisY = myImage.GetHeight - i - 1;

                    if (j < this.image.GetWidth)
                        targetThisX = j;
                    else
                        targetThisX = myImage.GetWidth - j - 1;

                    myImage[i, j] = image[targetThisY, targetThisX].Clone();
                }
            }

            CloneImage(myImage);
        }

        #endregion


        //Filtres

        #region Filtres

        /// <summary>
        /// Filtre de Sobel pour détecter les contours
        /// </summary>
        public void FiltreDetectionContours()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Bords_Détection)).GetMyImage);
        }


        /// <summary>
        /// Transforme une image pour détecter les contours avec la méthode de Sobel (gris -> flou -> filtre mat verticale + filtre mat horizontale -> somme)
        /// </summary>
        public void FiltreSobel()
        {
            this.TransformationGris();
            this.FiltreFlouGaussien();
            MyImage copie = new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Sobel_Vertical)).GetMyImage;
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Sobel_Horizontal)).GetMyImage);

            InternalSobelCanny(copie, this.image.GetCopie(), false);
        }

        /// <summary>
        /// Transforme une image avec le filtre de detection de contour avec la méthode de Canny
        /// </summary>
        /// <param name="intensitéMax">Intensité de pixel (0-255) à partir de laquelle on est sur qu'un pixel fait parti d'une ligne de contour</param>
        /// <param name="intensitéMin">Intensité de pixel (0-255) en dessous de laquelle on est sur qu'un pixel ne fait pas parti d'une ligne de contour</param>
        public void FiltreCanny(int intensitéMax = 200, int intensitéMin = 50)
        {
            //1.  - Application du filtre de Sobel à l'image
            //2.  - On rétrécit la taille des bords : pour cela on regarde la direction des bords de l'image, en fonction de ça on élimine les pixels 
            //   les moins importants de sorte qu'il ne reste qu'1 ou 2 pixel pour un bord. Direction Teta = tan-1(Gy / Gx)
            // Teta = 67.5-112.5 % 180 = haut/bas    |     Teta = 112.5-157.5 % 180 = diagonale basGauche/hautDroite, 22.5-67.5 % 180 = autres diago
            //      = 0-22.5 && 180-157.5 % 180 = gauche/droite
            //3.  - On élimine le bruit crée par des lignes de contours indésirables en éliminant les pixels en dessous d'une intensité X et en gardant 
            //   les pixels au dessus d'une intensité Y. Pour les pixels à l'intensité entre X et Y on regarde si ces pixels font parti d'une ligne
            //   de bord forte ou non, si oui on les garde sinon on les élimine.

            this.TransformationGris();
            this.FiltreFlouGaussien();
            MyImage copie = new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Sobel_Vertical)).GetMyImage;
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Sobel_Horizontal)).GetMyImage);

            InternalSobelCanny(copie, this.image, true, intensitéMax, intensitéMin);
        }

        private void InternalSobelCanny(MyImage x, MyImage y, bool canny, int intensitéMax = 200, int intensitéMin = 50)
        {
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    int gris = Math.Min((int)Math.Sqrt(y[i, j].GetMoyenne * y[i, j].GetMoyenne + x[i, j].GetMoyenne * x[i, j].GetMoyenne), 255);
                    this.image[i, j] = new Pixel(gris, gris, gris);
                }
            }
            if (!canny)
                return;

            for (int i = 1; i < this.image.GetHeight - 1; i++)
            {
                for (int j = 1; j < this.image.GetWidth - 1; j++)
                {
                    int mgntd = Math.Min((int)Math.Sqrt(y[i, j].GetMoyenne * y[i, j].GetMoyenne + x[i, j].GetMoyenne * x[i, j].GetMoyenne), 255);

                    double theta = Math.Atan(y[i, j].GetMoyenne / (x[i, j].GetMoyenne == 0 ? 1 : x[i, j].GetMoyenne)) % 180;

                    if (mgntd != 0)
                    {
                        int increX = 0;
                        int increY = 0;

                        if (theta >= 67.5 && theta < 112.5) //haut bas   |
                        {
                            increY = 1;
                        }
                        else if (theta >= 112.5 && theta < 157.5)  //Diago /
                        {
                            increX = -1;
                            increY = 1;
                        }
                        else if (theta >= 22.5 && theta < 67.5)   // Diago \
                        {
                            increX = 1;
                            increY = 1;
                        }
                        else if ((theta >= 0 && theta < 22.5) || (theta >= 157.5 && theta < 180))  //Gauche droite --
                        {
                            increX = 1;
                        }

                        int mgntdBas = Math.Min((int)Math.Sqrt(y[i + increY, j + increX].GetMoyenne * y[i + increY, j + increX].GetMoyenne + x[i + increY, j + increX].GetMoyenne * x[i + increY, j + increX].GetMoyenne), 255);
                        int mgntdHaut = Math.Min((int)Math.Sqrt(y[i - increY, j - increX].GetMoyenne * y[i - increY, j - increX].GetMoyenne + x[i - increY, j - increX].GetMoyenne * x[i - increY, j - increX].GetMoyenne), 255);

                        if (mgntdHaut >= mgntd)
                        {
                            this.image[i + increY, j + increX] = Pixel.Zero;
                        }
                        else if (mgntdBas >= mgntd)
                        {
                            this.image[i - increY, j - increX] = Pixel.Zero;
                        }
                        else
                        {
                            this.image[i - increY, j - increX] = this.image[i + increY, j + increX] = Pixel.Zero;
                        }
                    }
                }
            }


            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    if (this.image[i, j].GetMoyenne < intensitéMin)
                        this.image[i, j] = Pixel.Zero;
                    if (this.image[i, j].GetMoyenne < intensitéMax)
                    {
                        int minX = i < 2 ? 0 : i - 2;
                        int maxX = i >= this.image.GetHeight - 2 ? this.image.GetHeight - 1 : i + 2;
                        int minY = j < 2 ? 0 : j - 2;
                        int maxY = j >= this.image.GetWidth - 2 ? this.image.GetWidth - 1 : j + 2;
                        bool voisinBord = false;
                        for (int k = minX; k < maxX && !voisinBord; k++)
                        {
                            for (int l = minY; l < maxY; l++)
                            {
                                if (this.image[k, l].GetMoyenne > intensitéMin && (k != i || l != j))
                                {
                                    voisinBord = true;
                                    break;
                                }
                            }
                        }
                        if (!voisinBord)
                            this.image[i, j] = Pixel.Zero;
                    }
                }
            }

        }


        /// <summary>
        /// Filtre de renforcement des bords
        /// </summary>
        public void FiltreRenforcementContours()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Bords_Renforcement)).GetMyImage);
        }

        /// <summary>
        /// Filtre d'affaiblissement des bords
        /// </summary>
        public void FiltreAffaiblissementContours()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Bords_Affaiblissement)).GetMyImage);
        }



        /// <summary>
        /// Transforme une image en un dessin, les couleurs de l'image peuvent être inversées pour se rapprocher du résultat attendu 
        /// (paysage entre autres, à éviter sur les portraits). <para/>Ajouter du bruit peut également améliorer le résultat.
        /// </summary>
        /// <param name="inversion">Inversion des couleurs de l'image ou non</param>
        public void FiltreDessin(bool inversion)
        {
            this.AddNoise(20);
            this.TransformationGris();
            if (inversion)
                this.InversionCouleurs();
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.FlouGaussien)).GetMyImage);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Dessin)).GetMyImage);
        }

        /// <summary>
        /// Transforme une image en son équivalent en style gravure
        /// </summary>
        public void FiltreGravure()
        {
            //Un peu long à process
            this.TransformationNoirEtBlanc(0);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Contraste)).GetMyImage);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Repoussage_Fort)).GetMyImage);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Dessin)).GetMyImage);
        }



        /// <summary>
        /// Transforme une image avec le filtre de repoussage faible
        /// </summary>
        public void FiltreRepoussageFaible()
        {
            this.TransformationNoirEtBlanc(0);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Repoussage_Léger)).GetMyImage);
        }

        /// <summary>
        /// Transforme une image avec le filtre de repoussage fort
        /// </summary>
        public void FiltreRepoussageFort()
        {
            this.TransformationNoirEtBlanc(0);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Repoussage_Fort)).GetMyImage);
        }



        /// <summary>
        /// Aiguise une image
        /// </summary>
        public void FiltreAiguisage()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Aiguiser)).GetMyImage);
        }

        /// <summary>
        /// Augmente le contraste d'une image
        /// </summary>
        public void FiltreContraste()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Contraste)).GetMyImage);
        }



        /// <summary>
        /// Rend une image floue (flou de mouvement)
        /// </summary>
        /// <param name="flouDeMouvement">Filtre de mouvement ou filtre normal</param>
        public void FiltreFlou(bool flouDeMouvement)
        {
            //this.Resize(0.5);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(flouDeMouvement ? ConvolutionMatrix.ImageFiltre.Flou_De_Mouvement : ConvolutionMatrix.ImageFiltre.Flou)).GetMyImage);
            //this.Resize(2);
        }

        /// <summary>
        /// Rend une image floue (flou Gaussien)
        /// </summary>
        public void FiltreFlouGaussien()
        {
            //this.Resize(0.5);
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.FlouGaussien)).GetMyImage);
            //this.Resize(2);
        }

        /// <summary>
        /// Filtre de lissage
        /// </summary>
        public void FiltreLissage()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Lissage)).GetMyImage);
        }


        /// <summary>
        /// Test
        /// </summary>
        public void FiltreTest()
        {
            CloneImage(new Filtre(this.image, new ConvolutionMatrix(ConvolutionMatrix.ImageFiltre.Test)).GetMyImage);
        }

        #endregion


        //Pixélisation

        #region Pixélisation

        /// <summary>
        /// Rend une <see cref="MyImage"/> pixélisée à partir de la taille d'un nouveau gros Pixel définie par rapport à la largeur de l'image. Dans un gros pixel, tous les petits pixels ont la même couleur.<para/>
        /// 100% revient à pixéliser une image avec un nouveau pixel de la taille maximale en largeur.
        /// </summary>
        /// <param name="rapport"></param>
        public void Pixélisation(double rapport)
        {
            Pixélisation((int)(rapport * this.image.GetWidth / 100.0));
        }

        /// <summary>
        /// Rend une <see cref="MyImage"/> pixélisée à partir d'une taille d'un nouveau gros Pixel. Dans un gros pixel, tous les petits pixels ont la même couleur. <para/>
        /// Si <see cref="KeepAspectRatio"/> est set à <see langword="true"/> les gros pixels auront tous les même taille. <para/>
        /// Si <see langword="false"/> les gros pixels auront la taille indiquée, certains pourraient donc être plus petits sur les bords de l'image
        /// </summary>
        /// <param name="taillePixel">Taille d'un gros pixel avec 1 la taille d'un pixel d'origine</param>
        public void Pixélisation(int taillePixel)
        {
            int max = Math.Min(this.image.GetHeight, this.image.GetWidth);
            taillePixel = Math.Max(Math.Min(max, taillePixel), 0);

            int taillePixelWidth = taillePixel;
            int taillePixelHeight = taillePixel;

            if (this.KeepAspectRatio)  //On change la taille des nouveaux gros pixels pour ne pas avoir de pixels d'une taille trop inférieur sur les bords
            {
                double MaxDifférence = 5.0 / taillePixel;  // 5 pixels max d'écart pour les bords

                double resteWidth = (double)this.image.GetWidth / taillePixelWidth % 1;
                double resteHeight = (double)this.image.GetHeight / taillePixelHeight % 1;

                bool inversion = false;
                bool incrémentationDecimal = (double)this.image.GetWidth / taillePixelWidth % 1 > 0.5; //On va au plus proche d'abord

                while (!(resteWidth < MaxDifférence / 1.5 || resteWidth > 1 - MaxDifférence))//On préfère avoir un pixel d'une taille presque égale aux autres qu'un tout petit pixel d'une taille très inférieur pour les bords
                {
                    if (!inversion && ((taillePixelWidth < this.image.GetWidth && incrémentationDecimal) || (taillePixelWidth > 1 && !incrémentationDecimal))) //1ere étape, on augmente la taille si on peut
                    {
                        if (incrémentationDecimal)
                            taillePixelWidth++;
                        else
                            taillePixelWidth--;
                    }
                    else if (!inversion) //2ème étape
                    {
                        taillePixelWidth = taillePixel;
                        inversion = true;
                    }
                    else if ((taillePixelWidth > 1 && incrémentationDecimal) || (taillePixelWidth < this.image.GetWidth && !incrémentationDecimal)) //3ème étape, dans l'autre sens
                    {
                        if (incrémentationDecimal)
                            taillePixelWidth--;
                        else
                            taillePixelWidth++;
                    }
                    else //Fin
                    {
                        taillePixelWidth = taillePixel; //Echec, on part sur la taille donnée par l'utilisateur
                        inversion = false;
                        break;
                    }
                    resteWidth = (double)this.image.GetWidth / taillePixelWidth % 1;
                }

                incrémentationDecimal = (double)this.image.GetHeight / taillePixelHeight % 1 > 0.5;

                while (resteHeight > MaxDifférence / 1.5 && resteHeight < 1 - MaxDifférence) //On préfère avoir un pixel d'une taille presque égale aux autres qu'un tout petit pixel d'une taille très inférieur pour les bords
                {
                    if (!inversion && ((taillePixelHeight < this.image.GetHeight && incrémentationDecimal) || (taillePixelHeight > 1 && !incrémentationDecimal))) //1ere étape, on augmente la taille si on peut
                    {
                        if (incrémentationDecimal)
                            taillePixelHeight++;
                        else
                            taillePixelHeight--;
                    }
                    else if (!inversion) //2ème étape
                    {
                        taillePixelHeight = taillePixel;
                        inversion = true;
                    }
                    else if ((taillePixelHeight > 1 && incrémentationDecimal) || (taillePixelHeight < this.image.GetHeight && !incrémentationDecimal)) //3ème étape, dans l'autre sens
                    {
                        if (incrémentationDecimal)
                            taillePixelHeight--;
                        else
                            taillePixelHeight++;
                    }
                    else //Fin
                    {
                        taillePixelHeight = taillePixel; //Echec, on part sur la taille donnée par l'utilisateur
                        inversion = false;
                        break;
                    }
                    resteHeight = (double)this.image.GetHeight / taillePixelHeight % 1;
                }
            }

            int height = (int)Math.Ceiling((double)this.image.GetHeight / taillePixelHeight);
            int width = (int)Math.Ceiling((double)this.image.GetWidth / taillePixelWidth);

            //Pixélisation
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int X = taillePixelWidth * j;
                    int Y = taillePixelHeight * i;

                    Point coinHautGauche = new Point(Y, X);

                    MyImage im = this.GetRognage(coinHautGauche, new Point(Y + taillePixelHeight, X + taillePixelWidth)); //On rogne l'image pour isoler les couleurs

                    //On remplit l'image par la couleur moyenne de cette image

                    this.AddImage(new MyImage(im.GetHeight, im.GetWidth, MyImageStatistiques.GetAverageColor(im)), coinHautGauche); //On copie cette image au point determiné préalablement dans l'image
                }
            }
        }

        #endregion


        //Décallage

        #region décallage

        /// <summary>
        /// Décalle tous les pixels d'une image en fonction de la fonction sinus
        /// </summary>
        public void DécallageSin()
        {
            MyImage imageDécallée = new MyImage(this.image.GetHeight, this.image.GetWidth, Pixel.Zero);

            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    double sin = j + Math.Sin(j * 180 / this.image.GetWidth) * (this.image.GetWidth - 1);

                    if (sin > this.image.GetWidth - 1)
                        sin = sin - this.image.GetWidth - 1;
                    if (sin < 0)
                        sin = this.image.GetWidth - 1 + sin;

                    imageDécallée[i, (int)sin] = this.image[i, j];
                }
            }

            CloneImage(imageDécallée);
        }

        #endregion


        //Bruit numérique

        #region Bruit num

        /// <summary>
        /// Ajoute du bruit coloré à une <see cref="MyImage"/>
        /// </summary>
        /// <param name="quantité">Montant à ajouter ou à enlever à chaque pixel</param>
        public void AddNoise(int quantité)
        {
            quantité = Math.Abs(quantité);
            for (int y = 0; y < this.image.GetHeight; ++y)
            {
                for (int x = 0; x < this.image.GetWidth; ++x)
                {
                    Pixel currentPixel = this.image[y, x];

                    int R = currentPixel.GetR + this.rand.Next(-quantité, quantité + 1);

                    int G = currentPixel.GetG + this.rand.Next(-quantité, quantité + 1);

                    int B = currentPixel.GetB + this.rand.Next(-quantité, quantité + 1);

                    R = R > 255 ? 255 : R;
                    R = R < 0 ? 0 : R;

                    G = G > 255 ? 255 : G;
                    G = G < 0 ? 0 : G;

                    B = B > 255 ? 255 : B;
                    B = B < 0 ? 0 : B;

                    this.image[y, x] = Pixel.FromRGB(R, G, B);

                }
            }
        }

        /// <summary>
        /// Ajoute du bruit de type "sel et poivre" à une <see cref="MyImage"/>
        /// </summary>
        /// <param name="pourcentage">pourcentage de pixels rendus blancs ou noirs</param>
        public void AddPepperNoise(int pourcentage)
        {
            InternalAddNoise(false, pourcentage);
        }

        /// <summary>
        /// Ajoute du bruit de Grenaille à une <see cref="MyImage"/>
        /// </summary>
        public void AddShotNoise(int pourcentage)
        {
            InternalAddNoise(true, pourcentage);
        }

        /// <summary>
        /// Note : On ne teste pas si le pixel a déjà été changé pour accélérer le processus
        /// </summary>
        /// <param name="blackOnly"></param>
        /// <param name="pourcentage"></param>
        private void InternalAddNoise(bool blackOnly, int pourcentage)
        {
            pourcentage = Math.Min(Math.Max(pourcentage, 0), 100);

            int totalPixelAChanger = this.image.GetHeight * this.image.GetWidth * pourcentage / 100;
            for (int i = 0; i < totalPixelAChanger; i++)
            {
                int Y = this.rand.Next(0, this.image.GetHeight);
                int X = this.rand.Next(0, this.image.GetWidth);

                bool poivre = blackOnly || rand.Next(0, 2) == 1;

                this.image[Y, X] = Pixel.FromColor(poivre ? Couleurs.Noir : Couleurs.Blanc);
            }
        }

        #endregion


        //Remplissage d'une image par une couleur

        #region remplissage

        /// <summary>
        /// Remplit une image avec une couleur pré-définie
        /// </summary>
        public void Remplissage()
        {
            Remplissage(this.PixelRemplissage);
        }

        /// <summary>
        /// Remplit une image par la <see cref="Couleurs"/> spécifiée
        /// </summary>
        /// <param name="color">Couleur</param>
        public void Remplissage(Couleurs color)
        {
            Remplissage(Pixel.FromColor(color));
        }

        /// <summary>
        /// Remplit une image par le <see cref="Pixel"/> spécifié
        /// </summary>
        /// <param name="color">Pixel</param>
        public void Remplissage(Pixel color)
        {
            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    this.image[i, j] = color;
                }
            }
        }

        #endregion


        //Copie d'une image dans une image

        #region Ajout image dans image

        /// <summary>
        /// Copie un <see cref="MyImage"/> au milieu du <see cref="MyImage"/> de ce <see cref="MyGraphics"/>.
        /// </summary>
        /// <param name="imagePremierPlan">Image</param>
        public void AddImage(MyImage imagePremierPlan)
        {
            AddImage(imagePremierPlan, Point.Zero, 100);
        }


        /// <summary>
        /// Copie un <see cref="MyImage"/> sur le <see cref="MyImage"/> de ce <see cref="MyGraphics"/> à partir d'un <see cref="Point"/> et d'une opacité spécifiés.
        /// </summary>
        /// <param name="imagePremierPlan">Image à copier à partir du point spécifié</param>
        /// <param name="opacité">Opacité de l'image à copier</param>
        /// <param name="topLeft">Point à partir duquel l'image est copiée</param>
        public void AddImage(MyImage imagePremierPlan, Point topLeft, int opacité = 100)
        {
            if (topLeft == Point.Zero)  //On centre l'image
            {
                int diffX = this.image.GetWidth - imagePremierPlan.GetWidth;
                int diffY = this.image.GetHeight - imagePremierPlan.GetHeight;

                topLeft.X = diffX / 2;
                topLeft.Y = diffY / 2;
            }
            //On accepte les images qui dépassent des bords, il suffit donc de calculer à partir de quelles limites on copie 
            //l'image et jusqu'à quelles limites sup

            int minX = (int)topLeft.X;
            int minY = (int)topLeft.Y;

            int offSetX = 0;
            int offSetY = 0;

            if (minX < 0)
            {
                offSetX = -minX;
                minX = 0;
            }
            if (minY < 0)
            {
                offSetY = -minY;
                minY = 0;
            }

            int maxX = Math.Min(minX + imagePremierPlan.GetWidth - offSetX, this.image.GetWidth); //limites non comprises
            int maxY = Math.Min(minY + imagePremierPlan.GetHeight - offSetY, this.image.GetHeight);

            for (int i = minY; i < maxY; i++)
            {
                for (int j = minX; j < maxX; j++)
                {
                    if (this.image[i, j] != null)
                    {
                        this.image[i, j] = Pixel.AddTransparence(this.image[i, j], imagePremierPlan[i - minY + offSetY, j - minX + offSetX], (float)opacité / 100);
                    }
                    else
                        this.image[i, j] = imagePremierPlan[i - minY + offSetY, j - minX + offSetX];
                }

            }
        }

        #endregion


        //Stéganographie

        #region stégano

        /// <summary>
        /// Cache une <see cref="MyImage"/> dans une autre <see cref="MyImage"/> visible.<para/>
        /// Si l'image à cacher est plus petite que l'image visible, alors on cache l'image au milieu de l'image visible.<para/>
        /// Si l'image à cacher est plus grande que l'image visible, alors on rogne les bords pour que les 2 images soient de même taille.
        /// </summary>
        /// <param name="imageToHide">Image à cacher</param>
        /// <param name="nbBitsToHide">Nombre de bits à cacher</param>
        public void CacherImage(MyImage imageToHide, int nbBitsToHide = 4)
        {
            CloneImage(Stéganographie.CacherImage(this.image, imageToHide, nbBitsToHide));
        }

        /// <summary>
        /// Trouve une image cachée dans une image
        /// </summary>
        /// <param name="nbBitsHidden">Nombre de bits de décallage</param>
        /// <returns></returns>
        public void GetImageCachée(int nbBitsHidden = 4)
        {
            CloneImage(Stéganographie.GetImageCachée(this.image, nbBitsHidden));
        }

        /// <summary>
        /// Cache du texte au format 8 bits (256 premiers chars du format UTF-8) dans les bits les moins importants d'une copie d'une image et renvoie le résultat. <para/>
        /// Les pixels sont choisis pseudo-aléatoirement à partir d'un mot de passe. Les charactères ne faisant pas parti du format 8 bits sont ignorés. <para/>
        /// Ne pas utiliser la même image pour cacher 2 textes car l'un pourrait empiéter sur l'autre et corrompre certaines parties du texte. <para/>
        /// La longueur du texte par rapport à la taille de l'image n'influe pas sur le temps de calcul
        /// </summary>
        /// <param name="txt">Le texte à cacher au format 8 bits. Si la longueur du texte sera inconnue lors du décodage, le terminer par \u0003 </param>
        /// <param name="mdp">Mot de passe pour retrouver le texte par la suite. Pas de format particulier à respecter</param>
        /// <param name="nbreBitsCachés">Nombre de bits sur lesquels cacher le texte sur une couleur (rgb) d'un pixel. 1, 2, 4 ou 8</param>
        /// <returns></returns>
        public void CacherTexte(string txt, string mdp, int nbreBitsCachés = 2)
        {
            CloneImage(Stéganographie.CacherTexte(this.image, txt, mdp, nbreBitsCachés));
        }

        /// <summary>
        /// Récupère un texte caché à partir dans une <see cref="MyImage"/> à partir d'un mot de passe et de la longueur du texte à trouver (pas obligé mais le texte doit alors finir par \u0003 pour signifier la fin).
        /// </summary>
        /// <param name="mdp">Mot de passe</param>
        /// <param name="tailleMessage">Taille du message à retrouver. Si inconnue, mettre à 0, la longueur de texte maximale cherchée sera alors de 10 000 char. Le texte renvoyé s'arrête alors au char 'u\0003' ou jusqu'à la taille max</param>
        /// <param name="nbreBitsCachés">Nombre de bits utilisés pour cacher le message. 1, 2, 4 ou 8</param>
        /// <returns></returns>
        public string GetTexteCaché(string mdp, int tailleMessage = 0, int nbreBitsCachés = 2)
        {
            return Stéganographie.GetTexteCaché(this.image, mdp, tailleMessage, nbreBitsCachés);
        }


        /// <summary>
        /// Méthode qui change les couleurs d'une image !
        /// </summary>
        /// <param name="nbBitsToRetirer">Bits a retirer</param>
        /// <param name="moinsImportants">Retirer les bits les moins importants ou plus importants</param>
        public void RetirerBitsMyImage(int nbBitsToRetirer, bool moinsImportants = true)
        {
            nbBitsToRetirer = nbBitsToRetirer < 0 ? 0 : nbBitsToRetirer > 8 ? 8 : nbBitsToRetirer;

            byte invisible = 0b1111_1111;

            if (moinsImportants)
                invisible <<= nbBitsToRetirer;
            else
                invisible >>= nbBitsToRetirer;

            for (int i = 0; i < this.image.GetHeight; i++)
            {
                for (int j = 0; j < this.image.GetWidth; j++)
                {
                    Pixel p = this.image[i, j];

                    int r = p.GetR & invisible;
                    int g = p.GetG & invisible;
                    int b = p.GetB & invisible;

                    this.image[i, j] = new Pixel(r, g, b);
                }
            }
        } //Pas implémenter dans le form


        /// <summary>
        /// Méthode qui intensifie les couleurs d'une image, donne un peu un effet peinture
        /// </summary>
        public void CouleurPeinture()
        {
            for (int i = 7; i > 0; i--)
                this.CacherImage(this.image, i);
        }

        #endregion


        //Méthodes de dessin

        #region Dessin

        /// <summary>
        /// Dessine une ligne entre 2 points par la couleur <see cref="MyGraphics.PixelRemplissage"/>
        /// </summary>
        public void DrawLineBresenham(Point départ, Point arrivée)
        {
            DrawLineBresenham(départ, arrivée, this.PixelRemplissage);
        }

        /// <summary>
        /// Dessine une ligne entre 2 points avec la couleur spécifiée. Basé sur l'algorithme de Bresenham.
        /// </summary>
        public void DrawLineBresenham(Point départ, Point arrivée, Pixel color)
        {
            bool dép = TestPoint(départ);
            bool arriv = TestPoint(arrivée);

            if (!dép && !arriv)
                return;
            if (!dép)
            {
                départ = PointCollisionImage(départ, arrivée);
            }
            if (!arriv)
            {
                arrivée = PointCollisionImage(arrivée, départ);
            }

            //On change les coordonnées en int
            int x0 = (int)Math.Round(départ.X);
            int y0 = (int)Math.Round(départ.Y);
            int x1 = (int)Math.Round(arrivée.X);
            int y1 = (int)Math.Round(arrivée.Y);


            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                {
                    LigneBasBresenham(x1, y1, x0, y0, color);
                }
                else
                {
                    LigneBasBresenham(x0, y0, x1, y1, color);
                }
            }
            else
            {
                if (y0 > y1)
                {
                    LigneHautBresenham(x1, y1, x0, y0, color);
                }
                else
                {
                    LigneHautBresenham(x0, y0, x1, y1, color);
                }
            }
        }


        private void LigneBasBresenham(int x0, int y0, int x1, int y1, Pixel color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;

            if (dy < 0)
            {
                yi = -1;
                dy = -dy;
            }

            int d = 2 * dy - dx;
            int y = y0;

            for (int x = x0; x <= x1; x++)
            {
                //Pas opti mais aucune possibilité de crash. Le mieux serait de déterminer le point en bordure d'image 
                //avant de dessiner au lieu de tester si le point est dans l'image à chaque itération  -> fixé
                if (TestPoint(new Point(y, x)))
                {
                    this.image[y, x] = color;
                }

                if (d > 0)
                {
                    y += yi;
                    d += -2 * dx;
                }
                d += 2 * dy;
            }
        }
        private void LigneHautBresenham(int x0, int y0, int x1, int y1, Pixel color)
        {
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;

            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }

            int d = 2 * dx - dy;
            int x = x0;

            for (int y = y0; y <= y1; y++)
            {
                //Pas opti mais aucune possibilité de crash. Le mieux serait de déterminer le point en bordure d'image 
                //avant de dessiner au lieu de tester si le point est dans l'image à chaque itération --> c'est fait
                if (TestPoint(new Point(y, x)))
                {
                    this.image[y, x] = color;
                }

                if (d > 0)
                {
                    x += xi;
                    d += -2 * dy;
                }
                d += 2 * dx;
            }
        }



        /// <summary>
        /// Dessine une ligne entre 2 points par la couleur <see cref="PixelRemplissage"/> avec la méthode de Xiaolin Wu (anticrénelage)
        /// </summary>
        public void DrawLineWu(Point départ, Point arrivée)
        {
            DrawLineWu(départ, arrivée, this.PixelRemplissage);
        }

        /// <summary>
        /// Dessine une ligne entre 2 points par la couleur spécifiée avec la méthode de Xiaolin Wu (anticrénelage)
        /// </summary>
        public void DrawLineWu(Point départ, Point arrivée, Pixel color)
        {
            bool dép = TestPoint(départ);
            bool arriv = TestPoint(arrivée);

            if (!dép && !arriv)
                return;
            if (!dép)
            {
                départ = PointCollisionImage(départ, arrivée);
            }
            if (!arriv)
            {
                arrivée = PointCollisionImage(arrivée, départ);
            }

            int x0 = (int)départ.X;
            int x1 = (int)arrivée.X;
            int y0 = (int)départ.Y;
            int y1 = (int)arrivée.Y;

            if (Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                {
                    LigneWu(x1, y1, x0, y0, color);
                }
                else
                {
                    LigneWu(x0, y0, x1, y1, color);
                }
            }
            else
            {
                if (y0 > y1)
                {
                    LigneWu(x1, y1, x0, y0, color);
                }
                else
                {
                    LigneWu(x0, y0, x1, y1, color);
                }
            }
        }


        private void LigneWu(int x0, int y0, int x1, int y1, Pixel color)
        {
            InternalDrawPointWu(y0, x0, 1, color);
            InternalDrawPointWu(y1, x1, 1, color);

            float dx = x1 - x0;
            float dy = y1 - y0;
            float pente = dy / dx;
            if (Math.Abs(pente) <= 1f)
            {
                float y = y0 + pente;
                for (int x = x0 + 1; x < x1; x++)
                {
                    InternalDrawPointWu((int)y, x, 1 - (y - (int)y), color);
                    if ((int)y + 1 < this.image.GetHeight)
                    {
                        InternalDrawPointWu((int)y + 1, x, y - (int)y, color);
                    }
                    y += pente;
                }
            }
            else
            {
                pente = 1f / pente;
                float x = x0 + pente;
                for (int y = y0 + 1; y < y1; y++)
                {
                    InternalDrawPointWu(y, (int)x, 1 - (x - (int)x), color);
                    if (x + 1 < this.image.GetWidth)
                    {
                        InternalDrawPointWu(y, (int)x + 1, x - (int)x, color);
                    }
                    x += pente;
                }
            }
        }

        private void InternalDrawPointWu(int y, int x, float intensité, Pixel color)
        {
            this.image[y, x] = Pixel.AddTransparence(this.image[y, x], color, intensité);
        }



        /// <summary>
        /// Dessine un cercle avec le point central et le rayon spécifiés par la couleur <see cref="MyGraphics.PixelRemplissage"/>
        /// </summary>
        public void DrawCircleBresenham(Point centre, int rayon)
        {
            DrawCircleBresenham(centre, rayon, this.PixelRemplissage);
        }

        /// <summary>
        /// Dessine un cercle avec le point central et le rayon spécifiés de la couleur spécifiée. Basé sur l'algorithme de Bresenham.
        /// </summary>
        public void DrawCircleBresenham(Point centre, int rayon, Pixel color)
        {
            int d = (5 - rayon * 4) / 4;
            int y = 0;
            int x = rayon;

            do
            {
                if (centre.Y + y >= 0 && centre.Y + y <= this.image.GetHeight - 1 && centre.X + x >= 0 && centre.X + x <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y + y), (int)Math.Round(centre.X + x)] = color;
                if (centre.Y + y >= 0 && centre.Y + y <= this.image.GetHeight - 1 && centre.X - x >= 0 && centre.X - x <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y + y), (int)Math.Round(centre.X - x)] = color;
                if (centre.Y - y >= 0 && centre.Y - y <= this.image.GetHeight - 1 && centre.X + x >= 0 && centre.X + x <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y - y), (int)Math.Round(centre.X + x)] = color;
                if (centre.Y - y >= 0 && centre.Y - y <= this.image.GetHeight - 1 && centre.X - x >= 0 && centre.X - x <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y - y), (int)Math.Round(centre.X - x)] = color;
                if (centre.Y + x >= 0 && centre.Y + x <= this.image.GetHeight - 1 && centre.X + y >= 0 && centre.X + y <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y + x), (int)Math.Round(centre.X + y)] = color;
                if (centre.Y + x >= 0 && centre.Y + x <= this.image.GetHeight - 1 && centre.X - y >= 0 && centre.X - y <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y + x), (int)Math.Round(centre.X - y)] = color;
                if (centre.Y - x >= 0 && centre.Y - x <= this.image.GetHeight - 1 && centre.X + y >= 0 && centre.X + y <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y - x), (int)Math.Round(centre.X + y)] = color;
                if (centre.Y - x >= 0 && centre.Y - x <= this.image.GetHeight - 1 && centre.X - y >= 0 && centre.X - y <= this.image.GetWidth - 1) this.image[(int)Math.Round(centre.Y - x), (int)Math.Round(centre.X - y)] = color;

                if (d < 0)
                {
                    d += 2 * y + 1;
                }
                else
                {
                    d += 2 * (y - x) + 1;
                    x--;
                }
                y++;

            } while (y <= x);
        }



        /// <summary>
        /// Remplit un cercle formé par son centre et son rayon par la couleur indiquée. Basé sur l'algorithme de Bresenham
        /// </summary>
        /// <param name="centre">Centre du cercle</param>
        /// <param name="rayon">Rayon du cercle</param>
        /// <param name="color">Couleur de remplissage</param>
        public void FillCircle(Point centre, int rayon, Pixel color)
        {
            double x = 0;
            double y = rayon;
            double balance = -rayon;

            while (x <= y)
            {
                double p0 = centre.X - x;
                double p1 = centre.X - y;

                double w0 = x + x;
                double w1 = y + y;

                DrawLineBresenham(new Point(centre.Y + y, p0), new Point(centre.Y + y, p0 + w0), color);
                DrawLineBresenham(new Point(centre.Y - y, p0), new Point(centre.Y - y, p0 + w0), color);

                DrawLineBresenham(new Point(centre.Y + x, p1), new Point(centre.Y + x, p1 + w1), color);
                DrawLineBresenham(new Point(centre.Y - x, p1), new Point(centre.Y - x, p1 + w1), color);

                if ((balance += x++ + x) >= 0)
                {
                    balance -= --y + y;
                }
            }

        }




        /// <summary>
        /// Dessine un carré de la taille indiquée avec la couleur spécifiée à l'endroit donné
        /// </summary>
        /// <param name="hautGauche">Coin en haut à gauche du carré à partir duquel le carré est dessiné</param>
        /// <param name="taille">Taille du carré</param>
        /// <param name="color">Couleur de remplissage</param>
        public void DrawCarré(Point milieu, int taille, Pixel color)
        {
            DrawRectangle(new Point(milieu.Y - taille / 2, milieu.X - taille / 2), new Point(milieu.Y + taille / 2, milieu.X + taille / 2), color);
        }

        /// <summary>
        /// Remplit un carré de la taille indiquée avec la couleur spécifiée à l'endroit donné
        /// </summary>
        /// <param name="hautGauche">Coin en haut à gauche du carré à partir duquel le carré est remplit</param>
        /// <param name="taille">Taille du carré</param>
        /// <param name="color">Couleur de remplissage</param>
        public void FillCarré(Point milieu, int taille, Pixel color)
        {
            FillRectangle(new Point(milieu.Y - taille / 2, milieu.X - taille / 2), new Point(milieu.Y + taille / 2, milieu.X + taille / 2), color);
        }



        /// <summary>
        /// Dessine les contours d'un rectangle dont les coordonnées sont spécifiées par 2 coins d'en haut à gauche et d'en bas à droite, par la couleur spécifiée
        /// </summary>
        /// <param name="hautGauche">Coin en haut à gauche à partir duquel le rectangle est dessiné</param>
        /// <param name="basDroit">Coin en bas à droite. Limite du rectangle</param>
        /// <param name="color">Couleur de remplissage</param>
        public void DrawRectangle(Point hautGauche, Point basDroit, Pixel color)
        {
            Point hautDroit = new Point(hautGauche.Y, basDroit.X);
            Point basGauche = new Point(basDroit.Y, hautGauche.X);

            DrawLineBresenham(hautGauche, hautDroit, color);
            DrawLineBresenham(hautDroit, basDroit, color);
            DrawLineBresenham(basDroit, basGauche, color);
            DrawLineBresenham(basGauche, hautGauche, color);
        }

        /// <summary>
        /// Remplit un rectangle avec la couleur spécifiée à partir des coins à gauche en haut et à droite en bas
        /// </summary>
        /// <param name="hautGauche">Coin en haut à gauche à partir duquel le rectangle est remplit</param>
        /// <param name="basDroit">Coin en bas à droite. Limite du rectangle</param>
        /// <param name="color">Couleur de remplissage</param>
        public void FillRectangle(Point hautGauche, Point basDroit, Pixel color)
        {
            FillForme_4(hautGauche, basDroit, new Point(hautGauche.Y, basDroit.X), new Point(basDroit.Y, hautGauche.X), color);
        }




        /// <summary>
        /// Remplit le triangle formé par les <see cref="Point"/> indiqués par la couleur <see cref="MyGraphics.PixelRemplissage"/>
        /// </summary>
        public void FillTriangle(Point haut, Point droite, Point gauche)
        {
            FillTriangle(haut, droite, gauche, this.PixelRemplissage);
        }

        /// <summary>
        /// Remplit par la couleur spécifiée le triangle formé par les <see cref="Point"/> indiqués
        /// </summary>
        public void FillTriangle(Point haut, Point droite, Point gauche, Pixel color)
        {
            //Trie par hauteur puis par largeur les points

            List<Point> points = new List<Point>() { haut, droite, gauche };

            points = points.OrderBy(x => x.Y).ThenBy(x => x.X).ToList();


            //On appelle des méthodes en fonction des points

            if (points[2].Y == points[1].Y)  //Plat en bas
            {
                FillTrianglePlatBottom(points[0], points[2], points[1], color);
            }
            else if (points[0].Y == points[1].Y)  //Plat en haut
            {
                FillTrianglePlatTop(points[2], points[1], points[0], color);
            }
            else  //Autres formes, on divise le triangle en 2 pour travailler avec 2 triangles plats
            {
                Point intersection = new Point
                {
                    Y = points[1].Y,
                    X = points[0].X + ((points[1].Y - points[0].Y) / (points[2].Y - points[0].Y) * (points[2].X - points[0].X))
                };

                FillTrianglePlatBottom(points[0], points[1], intersection, color);
                FillTrianglePlatTop(points[2], intersection, points[1], color);

            }

        }


        private void FillTrianglePlatBottom(Point haut, Point droite, Point gauche, Pixel color)
        {
            //this.DrawLineWu(haut, droite, color);
            //this.DrawLineWu(droite, gauche, color);
            //this.DrawLineWu(haut, gauche, Pixel.FromColor(Couleurs.Noir));

            float penteHaut = (float)((gauche.X - haut.X) / (gauche.Y - haut.Y));
            float penteBas = (float)((droite.X - haut.X) / (droite.Y - haut.Y));

            float YHaut = (float)haut.X;
            float YBas = (float)haut.X;

            int minGauche = (int)Math.Ceiling(gauche.Y);

            for (int scanlineY = (int)Math.Ceiling(haut.Y); scanlineY <= minGauche; scanlineY++)
            {
                DrawLineBresenham(new Point(scanlineY, YHaut), new Point(scanlineY, YBas), color);
                YHaut += penteHaut;
                YBas += penteBas;
            }

            //this.DrawLineWu(haut, droite, color);
            //this.DrawLineWu(droite, gauche, color);
            //this.DrawLineWu(haut, gauche, color);

            Point p1 = new Point(haut.Y, haut.X - 1);
            Point p2 = new Point(gauche.Y, gauche.X - 1);
            if (TestPoint(p1) && TestPoint(p2))
            {
                //this.DrawLineWu(p1, p2, color);
            }

        }
        private void FillTrianglePlatTop(Point bas, Point droite, Point gauche, Pixel color)
        {
            float penteGauche = (float)((bas.X - gauche.X) / (bas.Y - gauche.Y));
            float penteDroite = (float)((bas.X - droite.X) / (bas.Y - droite.Y));

            float xGauche = (float)bas.X;
            float xDroite = (float)bas.X;

            int minGauche = (int)Math.Ceiling(gauche.Y);

            for (int scanlineY = (int)Math.Ceiling(bas.Y); scanlineY >= minGauche; scanlineY--)
            {
                DrawLineBresenham(new Point(scanlineY, xGauche), new Point(scanlineY, xDroite), color);
                xGauche -= penteGauche;
                xDroite -= penteDroite;
            }

            //this.DrawLineWu(droite, gauche, color);
            //this.DrawLineWu(bas, gauche, color);

            Point p1 = new Point(bas.Y, bas.X - 1);
            Point p2 = new Point(droite.Y, droite.X - 1);
            if (TestPoint(p1) && TestPoint(p2))
            {
                //this.DrawLineWu(p1, p2, color);
            }
        }



        /// <summary>
        /// Remplit par la couleur spécifiée le quadrilatère formé par les <see cref="Point"/> indiqués. L'ordre n'importe pas
        /// </summary>
        public void FillForme_4(Point p1, Point p2, Point p3, Point p4, Pixel color)
        {
            //On trie les points par distance au point le plus haut puis le plus à gauche

            List<Point> points = new List<Point>() { p1, p2, p3, p4 }.OrderBy(x => x.Y).ThenBy(x => x.X).ToList();


            FillTriangle(points[0], points[1], points[2], color);
            FillTriangle(points[1], points[2], points[3], color);
        }

        /// <summary>
        /// Remplit par la couleur spécifiée le pentagone formé par les <see cref="Point"/> indiqués. L'ordre n'importe pas
        /// </summary>
        public void FillForme_5(Point p1, Point p2, Point p3, Point p4, Point p5, Pixel color)
        {
            //On trie les points par distance au point le plus haut puis le plus à gauche

            List<Point> points = new List<Point>() { p1, p2, p3, p4, p5 }.OrderBy(x => x.Y).ThenBy(x => x.X).ToList();


            FillForme_4(points[0], points[1], points[2], points[3], color);
            FillTriangle(points[2], points[3], points[4], color);
        }

        /// <summary>
        /// Remplit par la couleur spécifiée l'hexagone formé par les <see cref="Point"/> indiqués. L'ordre n'importe pas
        /// </summary>
        public void FillForme_6(Point p1, Point p2, Point p3, Point p4, Point p5, Point p6, Pixel color)
        {
            //On trie les points par distance au point le plus haut puis le plus à gauche

            List<Point> points = new List<Point>() { p1, p2, p3, p4, p5, p6 }.OrderBy(x => x.Y).ThenBy(x => x.X).ToList();


            FillForme_5(points[0], points[1], points[2], points[3], points[4], color);
            FillTriangle(points[3], points[4], points[5], color);
        }



        /// <summary>
        /// Remplit un hexagone parfait à partir du point central indiqué, de la taille indiquée avec la couleur spécifiée
        /// </summary>
        /// <param name="centre">Centre de l'hexagone</param>
        /// <param name="size">Taille de l'hexagone (taille d'un coté)</param>
        /// <param name="color">Couleur de remplissage</param>
        public void FillHexagone(Point centre, int size, Pixel color)
        {
            InternalHexagone(centre, size, color, true);
        }

        /// <summary>
        /// Dessine le contour d'un hexagone parfait à partir du point central indiqué, de la taille indiquée avec la couleur spécifiée
        /// </summary>
        /// <param name="centre">Centre de l'hexagone</param>
        /// <param name="size">Taille de l'hexagone (taille d'un coté)</param>
        /// <param name="color">Couleur de remplissage</param>
        public void DrawHexagone(Point centre, int size, Pixel color)
        {
            InternalHexagone(centre, size, color, false);
        }


        private void InternalHexagone(Point centre, int size, Pixel color, bool remplissage)
        {
            Point pLeft = new Point(centre.Y, centre.X - size);
            Point pRight = new Point(centre.Y, centre.X + size);

            double ri = Math.Sqrt(3) / 2 * size;

            Point pTopLeft = new Point(centre.Y - ri, centre.X - size / 2);
            Point pTopRight = new Point(pTopLeft.Y, pTopLeft.X + size);
            Point pBotLeft = new Point(centre.Y + ri, centre.X - size / 2);
            Point pBotRight = new Point(pBotLeft.Y, pBotLeft.X + size);

            if (remplissage)
                FillForme_6(pLeft, pRight, pTopLeft, pTopRight, pBotLeft, pBotRight, color);
            else
            {
                DrawLineBresenham(pLeft, pTopLeft, color);
                DrawLineBresenham(pTopLeft, pTopRight, color);
                DrawLineBresenham(pTopRight, pRight, color);
                DrawLineBresenham(pRight, pBotRight, color);
                DrawLineBresenham(pBotRight, pBotLeft, color);
                DrawLineBresenham(pBotLeft, pLeft, color);
            }
        }



        /// <summary>
        /// Dessine le contour d'un pentagone parfait à partir du point central indiqué, de la taille indiquée avec la couleur spécifiée
        /// </summary>
        /// <param name="centre">Centre du pentagone</param>
        /// <param name="size">Taille du pentagone (taille d'un coté)</param>
        /// <param name="color">Couleur de remplissage</param>
        public void DrawPentagone(Point centre, int size, Pixel color)
        {
            InternalPentagone(centre, size, color, false);
        }

        /// <summary>
        /// Remplit un pentagone parfait à partir du point central indiqué, de la taille indiquée avec la couleur spécifiée
        /// </summary>
        /// <param name="centre">Centre du pentagone</param>
        /// <param name="size">Taille du pentagone (taille d'un coté)</param>
        /// <param name="color">Couleur de remplissage</param>
        public void FillPentagone(Point centre, int size, Pixel color)
        {
            InternalPentagone(centre, size, color, true);
        }


        private void InternalPentagone(Point centre, int size, Pixel color, bool remplissage)
        {
            double circons = 1 / Math.Sqrt(3 - ((1 + Math.Sqrt(5)) / 2));
            double apoth = 1 / Math.Tan(Math.PI / 5) / 2;
            double angle = 105 * Math.PI / 180;

            double rCirc = size * circons;
            double rInsc = size * apoth;

            Point top = new Point(centre.Y - rCirc, centre.X);
            Point botLeft = new Point(centre.Y + rInsc, centre.X - size / 2);
            Point botRight = new Point(botLeft.Y, botLeft.X + size);

            Point left = new Point(botLeft.Y - size * Math.Sin(angle), botLeft.X + size * Math.Cos(angle));
            Point right = new Point(botRight.Y - size * Math.Sin(angle), botRight.X - size * Math.Cos(angle));

            if (remplissage)
                FillForme_5(top, botLeft, botRight, left, right, color);
            else
            {
                DrawLineBresenham(top, left, color);
                DrawLineBresenham(left, botLeft, color);
                DrawLineBresenham(botLeft, botRight, color);
                DrawLineBresenham(botRight, right, color);
                DrawLineBresenham(right, top, color);
            }
        }



        /// <summary>
        /// Teste si un <see cref="Point"/> est présent dans l'image
        /// </summary>
        private bool TestPoint(Point p1)
        {
            if (p1.X < 0)
                return false;
            else if (p1.X > this.image.GetWidth - 1)
                return false;

            if (p1.Y < 0)
                return false;
            else if (p1.Y > this.image.GetHeight - 1)
                return false;

            return true;
        }

        /// <summary>
        /// Renvoie le Point de l'intersection d'une droite entre 2 Point et les bords de l'image
        /// </summary>
        /// <param name="pointToChange">Point à changer</param>
        /// <param name="départ">Point de départ à partir duquel tracer une droite vers le point à changer</param>
        /// <returns></returns>
        private Point PointCollisionImage(Point pointToChange, Point départ)
        {
            double pente = (pointToChange.Y - départ.Y) / (pointToChange.X - départ.X);
            //f(x) = ax + b, f(départ.X) = pente * départ.X + b <=> b = départ.Y - pente * départ.X
            double origineY = départ.Y - pente * départ.X;

            if (pointToChange.X == départ.X && pointToChange.Y == départ.Y)
            {
                return Point.Zero;
            }

            if ((pointToChange.X >= this.image.GetWidth && pointToChange.Y >= this.image.GetHeight) ||
                (pointToChange.X < 0 && pointToChange.Y < 0))
            {
                //On regarde si le futur point changé est sur l'axe des abscisses de l'image ou des ordonnées 
                //f(maxWidth) = pente * maxWidth + origineY
                if (pointToChange.Y >= 0)
                {
                    double posY = pente * (this.image.GetWidth - 1) + origineY;
                    if (posY <= this.image.GetHeight - 1)
                    {
                        pointToChange.X = this.image.GetWidth - 1;
                        pointToChange.Y = origineY + pente * (this.image.GetWidth - 1);
                    }
                    else
                    {
                        pointToChange.Y = this.image.GetHeight - 1;
                        pointToChange.X = pente == 0 ? origineY : (this.image.GetHeight - 1 - origineY) / pente;
                    }
                }
                else
                {
                    if (origineY < 0)
                    {
                        pointToChange.Y = 0;
                        pointToChange.X = pente == 0 ? 0 : -origineY / pente;
                    }
                    else
                    {
                        pointToChange.X = 0;
                        pointToChange.Y = origineY;
                    }
                }
            }
            else if (pointToChange.Y >= this.image.GetHeight || pointToChange.Y < 0) //Y
            {
                if (pointToChange.Y < 0)
                {
                    pointToChange.Y = 0;
                    pointToChange.X = pente == 0 ? 0 : -origineY / pente;
                }
                else
                {
                    pointToChange.Y = this.image.GetHeight - 1;
                    pointToChange.X = pente == 0 ? -origineY : (this.image.GetHeight - origineY) / pente;
                }
            }
            else //X
            {
                if (pointToChange.X < 0)
                {
                    pointToChange.X = 0;
                    pointToChange.Y = origineY;
                }
                else
                {
                    pointToChange.X = this.image.GetWidth - 1;
                    pointToChange.Y = origineY + pente * (this.image.GetWidth - 1);
                }
            }

            return pointToChange;
        }

        #endregion
    }
}
