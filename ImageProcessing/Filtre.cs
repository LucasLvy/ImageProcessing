using System;

namespace ImageProcessing
{
    /// <summary>
    /// Classe effectuant des opérations de filtrage sur des copies d'instances de <see cref="MyImage"/>
    /// </summary>
    internal class Filtre
    {
        //Champs et propriétés

        private MyImage matrixFiltrée;

        /// <summary>
        /// Addition du total des éléments dans la matrice de convolution de cette instance
        /// </summary>
        private int sommeTotale;

        /// <summary>
        /// Valeur ajoutée à chaque valeur de pixel lors du calcul matriciel
        /// </summary>
        private int correctif;

        /// <summary>
        /// Si <see langword="true"/>, le filtre est appliquée en 2 fois en divisant la matrice de convolution. Plus rapide. Pas implémenté
        /// </summary>
        private bool séparation;

        /// <summary>
        /// Récupère l'instance <see cref="MyImage"/> résultante du filtrage
        /// </summary>
        public MyImage GetMyImage => matrixFiltrée;
        

        private int GetTotal(float[][] convMat)
        {
            float value = 0;
            for (int i = 0; i < convMat.Length; i++)
            {
                for (int j = 0; j < convMat[i].Length; j++)
                {
                    value += convMat[i][j];
                }
            }
            if (value == 0) value = 1;
            return (int)value;
        }


        //Constructeurs

        /// <summary>
        /// Applique à une copie d'une instance <see cref="MyImage"/> le filtre <see cref="ConvolutionMatrix"/> spécifié
        /// </summary>
        /// <param name="bmp">Instance <see cref="MyImage"/> contenant les informations liées à l'image</param>
        /// <param name="convMat">Matrice de convolution</param>
        public Filtre(MyImage imageToFilter, ConvolutionMatrix convMat)
            : this(imageToFilter, convMat.GetMatrix(), convMat.Correctif, convMat.Séparation)
        {
        }

        /// <summary>
        /// Applique à une copie d'une instance <see cref="MyImage"/> le filtre spécifié
        /// </summary>
        /// <param name="bmp">Instance <see cref="MyImage"/> contenant les informations liées à l'image</param>
        /// <param name="convMat">Matrice de convolution</param>
        /// <param name="correctif">Valeur à ajouter à chaque pixel</param>
        /// <param name="séparation">Si <see langword="true"/>, le filtre est appliquée en 2 fois en divisant la matrice de convolution. Plus rapide.</param>
        public Filtre(MyImage imageToFilter, float[][] convMat, int correctif = 0, bool séparation = false)
        {
            this.sommeTotale = GetTotal(convMat);
            this.correctif = correctif;
            this.séparation = séparation;

            this.matrixFiltrée = new MyImage(imageToFilter.GetHeight, imageToFilter.GetWidth);

            ApplicationFiltre(imageToFilter, convMat);
        }


        //Méthodes de calculs

        /// <summary>
        /// Multiplie chaque pixel d'une matrice et les pixels autours de ce pixel par une matrice de convolution
        /// </summary>
        /// <param name="imageToFilter"></param>
        /// <param name="convMatrix"></param>
        private void ApplicationFiltre(MyImage imageToFilter, float[][] convMatrix)
        {
            for (int i = 0; i < imageToFilter.GetHeight; i++)
            {
                for (int j = 0; j < imageToFilter.GetWidth; j++)
                {
                    byte[] bgr = new byte[3];

                    for (int k = 0; k < 3; k++)
                    {
                        bgr[k] = GetValue(imageToFilter, convMatrix, new Point(i, j ), k);
                    }

                    this.matrixFiltrée[i, j] = new Pixel(bgr);
                }
            }

        }

        private byte GetValue(MyImage imageToFilter, float[][] convMatrix, Point position, int couleur)
        {
            int autourIndex = convMatrix.Length / 2;  //Nombre de case à prendre en compte autour de la case position[], 1 si la mat est de taille 3, 2 si de taille 5

            int minY = (int)position.Y - autourIndex;
            int minX = (int)position.X - autourIndex;

            int maxY = (int)position.Y + autourIndex;
            int maxX = (int)position.X + autourIndex;

            double value = 0;

            for (int i = minY; i <= maxY; i++)
            {
                for (int j = minX; j <= maxX; j++)
                {
                    int x = j, y = i;

                    //On gère les contours en prenant les pixels miroirs à l'intérieur de l'image. 
                    //Par ex pour un pixel situé à 2 unités de distances en dehors de l'image on prendra le pixel à 2 unités à l'intérieur de l'image

                    if (x >= imageToFilter.GetWidth)
                        x = imageToFilter.GetWidth - 1 - (x - imageToFilter.GetWidth);
                    else if (x < 0)
                    {
                        x = -x;
                    }
                    if (y >= imageToFilter.GetHeight)
                        y = imageToFilter.GetHeight - 1 - (y - imageToFilter.GetHeight);
                    else if (y < 0)
                    {
                        y = -y;
                    }

                    byte color = couleur == 0 ? imageToFilter[y, x].GetB : couleur == 1 ? imageToFilter[y, x].GetG : imageToFilter[y, x].GetR;

                    value += color * convMatrix[i - minY][j - minX];
                }
            }

            return (byte)Math.Min(Math.Max(value / this.sommeTotale + this.correctif, 0), 255);
        }

    }
}