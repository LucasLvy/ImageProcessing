using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ImageProcessing
{
    /// <summary>
    /// Permet de lire et de sauvegarder un fichier .bmp ou .csv (au format bitmap)
    /// </summary>
    public class MyImage
    {
        //Constantes faisant reférence aux index des différentes infos dans un fichier .bmp

        private const byte BMP_HEADER_INFO = 14;
        private const byte BMP_HEADER_INFO_WIDTH = 4;
        private const byte BMP_HEADER_INFO_HEIGHT = 8;
        private const byte BMP_HEADER_INFO_DATA_OFFSET = 10;
        private const byte BMP_HEADER_INFO_BPP = 14;
        private const byte BMP_HEADER_INFO_COMPRESSION = 16;

        private const byte BMP_PIXEL_ARRAY = 54;


        //Champs et propriétés

        private int errorCount = 0;

        /// <summary>
        /// Matrice contenant tous les pixels de cette instance de <see cref="MyImage"/>
        /// </summary>
        private Pixel[,] pixels;

        private int width;
        private int height;

        private Extension ext;

        /// <summary>
        /// Récupère la largeur de l'image 
        /// </summary>
        public int GetWidth => width;

        /// <summary>
        /// Récupère la hauteur de l'image
        /// </summary>
        public int GetHeight => height;

        /// <summary>
        /// Controle la validité de l'image
        /// </summary>
        public bool BitmapValide { get; private set; } = true;


        /// <summary>
        /// Renvoie et set le <see cref="Pixel"/> aux coordonnées spécifiées dans la matrice de <see cref="Pixel"/>
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public Pixel this[int row, int column]
        {
            get
            {
                try
                {
                    return this.pixels[row, column];
                }
                catch (Exception e)
                {
                    if (this.errorCount++ <= 2)
                    {
                        MessageBox.Show($"Erreur interne : {e.Message} \nmaxWidth = { this.width } , maxHeight = { this.height}\n" +
                          $"  ligne : {row} ,  colonne : {column}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    throw new Exception();
                }
            }

            set
            {
                try
                {
                    this.pixels[row, column] = value ?? throw new ArgumentNullException();
                }
                catch (Exception e)
                {
                    if (this.errorCount++ <= 2)
                    {
                        MessageBox.Show($"Erreur interne : {e.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    throw new Exception();
                }
            }
        }


        //Constructeurs

        /// <summary>
        /// Initialise une nouvelle instance de la class <see cref="MyImage"/> à partir du fichier au chemin indiqué.
        /// </summary>
        /// <param name="filename">Chemin du fichier</param>
        public MyImage(string filename)
            : this(filename, GetExtension(filename, true))
        {
        }

        /// <summary>
        /// Crée une nouvelle instance de la classe <see cref="MyImage"/> à partir du chemin indiqué au format <see cref="MyImage.Extension"/> spécifié
        /// </summary>
        /// <param name="filename">Chemin</param>
        /// <param name="ext">Extension</param>
        public MyImage(string filename, Extension ext)
        {
            if (ext == 0)
                return;
            filename = GetFilenameWithCorrectExtension(filename, ext);
            this.ext = GetExtension(filename, true);
            GetBitMapFromImage(GetFilenameWithCorrectExtension(filename, this.ext));
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MyImage"/> à partir de nouvelles dimensions de hauteur et de largeur
        /// </summary>
        /// <param name="height">Hauteur</param>
        /// <param name="width">Largeur</param>
        public MyImage(int height, int width)
        {
            this.width = width;
            this.height = height;

            this.pixels = new Pixel[height, width];
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MyImage"/> à partir de nouvelles dimensions de hauteur et de largeur. 
        /// La remplit avec la couleur spécifiée
        /// </summary>
        /// <param name="height">Hauteur</param>
        /// <param name="width">Largeur</param>
        /// <param name="couleurRemplissage">Pixel de remplissage de l'image</param>
        public MyImage(int height, int width, Pixel couleurRemplissage)
            :this(height, width)
        {
            new MyGraphics(this).Remplissage(couleurRemplissage);
        }


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MyImage"/> à partir d'une <see cref="MyImage"/> à redimensionner selon 
        /// de nouveaux paramètres de hauteur et de largeur.
        /// </summary>
        /// <param name="image">Image </param>
        /// <param name="targetHeight">Hauteur</param>
        /// <param name="targetWidth">Largeur</param>
        public MyImage(MyImage image, int targetHeight, int targetWidth)
        {
            MyGraphics g = new MyGraphics(image.GetCopie())
            {
                KeepAspectRatio = false,
                Quality = InterpolationMode.Bicubique
            };
            g.Redimensionnement(targetHeight, targetWidth);

            ClonePixels(g.GetMyImage.pixels);
        }


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MyImage"/> à partir d'une matrice de <see cref="Pixel"/>
        /// </summary>
        public MyImage(Pixel[,] p)
            :this(p.GetLength(0), p.GetLength(1))
        {
            ClonePixels(p);
        }
        

        /// <summary>
        /// Retourne une nouvelle copie d'une <see cref="MyImage"/>
        /// </summary>
        public MyImage GetCopie()
        {
            return new MyImage(this.pixels);
        }


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MyImage"/> à partir d'une <see cref="Bitmap"/>
        /// </summary>
        public static MyImage FromBitmap(Bitmap bmp)
        {
            MyImage myImage = new MyImage(bmp.Height, bmp.Width);

            for (int i = 0; i < myImage.height; i++)
            {
                for (int j = 0; j < myImage.width; j++)
                {
                    myImage.pixels[i, j] = Pixel.FromColor(bmp.GetPixel(j, i));
                }
            }
            return myImage;
        }
        
        /// <summary>
        /// Renvoie la <see cref="MyImage"/> sous forme d'un <see cref="Bitmap"/>
        /// </summary>
        /// <returns></returns>
        public Bitmap ToBitmap()
        {
            Bitmap bmp = new Bitmap(this.width, this.height);

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    bmp.SetPixel(j, i, this[i, j].ToColor());
                }
            }

            return bmp;
        }
        

        //Opérateurs 

        /// <summary>
        /// Renvoie <see langword="true"></see> si tous les Pixels des 2 images sont les mêmes
        /// </summary>
        /// <param name="a">image 1</param>
        /// <param name="b">image 2</param>
        /// <returns></returns>
        public static bool operator ==(MyImage a, MyImage b)
        {
            if ((a as object) == null && (b as object) == null) // 'as object' pour éviter de rentrer dans une boucle infinie
                return true;
            if ((a as object) == null || (b as object) == null)
                return false;

            if (a.GetHeight != b.GetHeight || a.width != b.GetWidth)
                return false;

            for (int i = 0; i < a.height; i++)
            {
                for (int j = 0; j < a.width; j++)
                {
                    if (a[i, j] != b[i, j])
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Renvoie <see langword="false"></see> si tous les Pixels des 2 images sont les mêmes
        /// </summary>
        /// <param name="a">image 1</param>
        /// <param name="b">image 2</param>
        /// <returns></returns>
        public static bool operator !=(MyImage a, MyImage b)
        {
            return !(a == b);
        }


        //Création et sauvegarde d'un fichier

        #region Création bitmap à partir d'un fichier

        private void GetBitMapFromImage(string filename)
        {
            if (filename == null || !File.Exists(filename))
            {
                this.BitmapValide = false;
                return;
            }

            byte[] allBytes = GetByteArrayFromFile(filename);
            byte[] header = ExtractByteArrayFromByteArray(allBytes, 0, BMP_HEADER_INFO);
            byte[] headerInfo = ExtractByteArrayFromByteArray(allBytes, BMP_HEADER_INFO, BMP_PIXEL_ARRAY);

            ConformitéFichierTest(headerInfo, header);

            GetWidthAndHeightFromHeaderInfo(headerInfo, out this.height, out this.width);
            this.pixels = new Pixel[this.height, this.width];


            byte[] rgb = ExtractByteArrayFromByteArray(allBytes, BMP_PIXEL_ARRAY, allBytes.Length);

            GetBitMapFromRGBArray(rgb);

            BitmapValide = CheckValiditéImage();
        }

        /// <summary>
        /// Récupère le fichier sous forme d'un tableau de <see cref="byte"/>
        /// </summary>
        /// <param name="filename">Chemin d'accès au fichier</param>
        /// <returns></returns>
        private byte[] GetByteArrayFromFile(string filename)
        {
            byte[] returnBytes = null;

            if (this.ext == Extension.csv)
            {
                string chars = new StreamReader(filename).ReadToEnd();
                List<int> onlyDigit = new List<int>();

                string num = "";

                foreach (char c in chars)
                {
                    if (!char.IsDigit(c) && num != string.Empty)
                    {
                        onlyDigit.Add(Convert.ToInt32(num));
                        num = "";
                    }
                    else if (char.IsDigit(c))
                    {
                        num += c;
                    }
                }
                returnBytes = new byte[onlyDigit.Count];
                for (int i = 0; i < onlyDigit.Count; i++)
                {
                    returnBytes[i] = (byte)onlyDigit[i];
                }
            }
            else
            {
                returnBytes = File.ReadAllBytes(filename);
            }
            return returnBytes;
        }
        
        /// <summary>
        /// Extrait un tableau de <see cref="byte"/> d'un tableau de <see cref="byte"/> entre 2 limites
        /// </summary>
        /// <param name="bytes">Tableau de <see cref="byte"/></param>
        /// <param name="début">Index de début</param>
        /// <param name="fin">Index de fin (inclus)</param>
        /// <returns></returns>
        private static byte[] ExtractByteArrayFromByteArray(byte[] bytes, int début, int fin)
        {
            if (bytes == null || début > fin || début < 0 || fin > bytes.Length)
            {
                return null;
            }

            byte[] toReturn = new byte[fin - début];
            for (int indexArrayToExtract = début, indexArray = 0; indexArray < toReturn.Length; indexArray++, indexArrayToExtract++)
            {
                toReturn[indexArray] = bytes[indexArrayToExtract];
            }

            return toReturn;
        }

        private static void GetWidthAndHeightFromHeaderInfo(byte[] headerInfo, out int height, out int width)
        {
            byte[] _height = ExtractByteArrayFromByteArray(headerInfo, BMP_HEADER_INFO_HEIGHT, BMP_HEADER_INFO_HEIGHT + 4);
            byte[] _width = ExtractByteArrayFromByteArray(headerInfo, BMP_HEADER_INFO_WIDTH, BMP_HEADER_INFO_WIDTH + 4);

            height = ConvertEndianToInt(_height);
            width = ConvertEndianToInt(_width);
        }


        /// <summary>
        /// Remplit la matrice de <see cref="Pixel"/> à partir d'un tableau de <see cref="byte"/> au format bgr
        /// </summary>
        /// <param name="rgbs">Tableau contenant les infos liées aux pixels de l'image</param>
        private void GetBitMapFromRGBArray(byte[] rgbs)
        {
            int indexArray = 0;
            int padding = GetPaddingPixel(this.width);

            for (int i = this.height - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.width; j++)
                {
                    byte[] bgr = new byte[3];
                    for (int k = 0; k < bgr.Length; k++)
                    {
                        bgr[k] = rgbs[indexArray++];
                    }
                    this[i, j] = new Pixel(bgr);
                }
                indexArray += padding;
            }

        }

        private static int GetPaddingPixel(int width)
        {
            int padding = 0;
            while ((width * 3 + padding) % 4 != 0)
                padding++;
            return padding;
        }


        /// <summary>
        /// Non conforme si le nombre de bits par pixels est != 24, si le fichier est compressé ou si le début des données des pixels ne commence pas au bon endroit
        /// </summary>
        private bool ConformitéFichierTest(byte[] headerInfo, byte[] header)
        {
            int bpp = ExtractByteArrayFromByteArray(headerInfo, BMP_HEADER_INFO_BPP, BMP_HEADER_INFO_BPP + 2)[0];

            if (bpp != 24)
                throw new NotSupportedException("La méthode ne prend en charge que des images 24 bits pour le moment");

            bool compression = ExtractByteArrayFromByteArray(headerInfo, BMP_HEADER_INFO_COMPRESSION, BMP_HEADER_INFO_COMPRESSION + 4)[0] != 0;

            if (compression)
                throw new NotSupportedException("La méthode actuelle ne prend pas en charge les fichiers compressés");

            int débutData = ExtractByteArrayFromByteArray(header, BMP_HEADER_INFO_DATA_OFFSET, BMP_HEADER_INFO_DATA_OFFSET + 4)[0];

            if (débutData != 54)
                throw new NotSupportedException("Le début des informations liées aux pixels ne commencent pas à l'endroit attendu");

            return true;
        }

        #endregion

        #region Sauvegarde

        /// <summary>
        /// Sauvegarde cette <see cref="MyImage"/> dans le fichier sélectionné
        /// </summary>
        /// <param name="filename">Chemin de sauvegarde</param>
        public void Save(string filename)
        {
            Save(filename, GetExtension(filename));
        }

        /// <summary>
        /// Sauvegarde cette <see cref="MyImage"/> dans le fichier sélectionné au format indiqué. Overwrite si le fichier existe déjà
        /// </summary>
        /// <param name="filename">Chemin de sauvegarde</param>
        /// <param name="ext">Extension à appliquer au chemin de sauvegarde</param>
        public void Save(string filename, Extension ext)
        {
            if (filename == null || ext == 0)
                MessageBox.Show($"Impossible de sauvegarder, le format n'est pas pris en charge", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            this.ext = ext;

            filename = GetFilenameWithCorrectExtension(filename, ext);

            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                MessageBox.Show($"Impossible de sauvegarder, le dossier de sauvegarde n'existe pas", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] pixels = GetPixelsByteArray();
            byte[] header = GetHeaderANDheaderInfo(pixels.Length);

            byte[] fichierBMP = new byte[header.Length + pixels.Length];

            int indexArray = 0;
            for (int i = 0; i < header.Length; i++, indexArray++)
            {
                fichierBMP[indexArray] = header[i];
            }

            for (int i = 0; i < pixels.Length; i++, indexArray++)
            {
                fichierBMP[indexArray] = pixels[i];
            }

            if (ext == Extension.bmp)
            {
                File.WriteAllBytes(filename, fichierBMP);
            }
            else if (ext == Extension.csv)
            {
                /*
                string fichier = "";
                for (int i = 0; i < 54; i++)
                {
                    fichier += fichierBMP[i] + ";";
                    if (i == BMP_HEADER_INFO - 1)
                        fichier += ";;\r\n";
                }
                fichier += ";;;;\r\n";
                for (int i = 54; i < fichierBMP.Length; i++)
                {
                    fichier += fichierBMP[i].ToString() + ";";
                }
                File.WriteAllText(filename, fichier);
                */
                File.WriteAllText(filename, string.Join(";", fichierBMP) + ";"); //Bcp plus rapide
            }

            //Console.WriteLine($@"La sauvegarde a été une réussite !{Environment.NewLine}Image à retrouver :"" {filename} """);
        }


        private byte[] GetPixelsByteArray()
        {
            byte padding = (byte)GetPaddingPixel(this.width);
            int tailleArray = (this.width * this.height * 3) + this.height * padding;
            byte[] corps = new byte[tailleArray];

            int indexArray = 0;
            for (int i = this.height - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.width; j++)
                {
                    corps[indexArray] = this[i, j].GetB;
                    indexArray++;

                    corps[indexArray] = this[i, j].GetG;
                    indexArray++;

                    corps[indexArray] = this[i, j].GetR;
                    indexArray++;
                }
                for (int j = 0; j < padding; j++, indexArray++)  //Padding fin de ligne
                {
                    corps[indexArray] += 0;
                }
            }
            return corps;
        }

        private byte[] GetHeaderANDheaderInfo(int tailleArrayPixels) 
        {
            byte[] header = new byte[BMP_PIXEL_ARRAY];

            //Header 

            header[0] = 66;
            header[1] = 77;

            int sizePixels = BMP_PIXEL_ARRAY + tailleArrayPixels;

            byte[] size = ConvertIntToEndian(sizePixels);

            for (int i = 0; i < 4; i++)
            {
                header[i + 2] = size[i];
            }

            for (int i = 6; i < 10; i++)
            {
                header[i] = 0;
            }

            header[10] = BMP_PIXEL_ARRAY;
            for (int i = 11; i < 14; i++)
            {
                header[i] = 0;
            }

            //Fin Header

            int infoOffset = 14;

            //Header Info

            header[infoOffset] = 40; //size info header
            for (int i = infoOffset + 1; i < infoOffset + 3; i++)
                header[i] = 0;

            size = ConvertIntToEndian(this.width); //Width

            infoOffset += 4;
            for (int i = 0; i < 4; i++)
            {
                header[i + infoOffset] = size[i];
            }
            infoOffset += 4;

            size = ConvertIntToEndian(this.height);  //Height

            for (int i = 0; i < 4; i++)
            {
                header[i + infoOffset] = size[i];
            }
            infoOffset += 4;

            header[infoOffset++] = 1;  //Planes
            header[infoOffset++] = 0;

            header[infoOffset++] = 24;  //BPP
            header[infoOffset++] = 0;

            for (int i = 0; i < 4; i++)   //Compression
                header[i + infoOffset] = 0;
            infoOffset += 4;


            size = ConvertIntToEndian(tailleArrayPixels);  //Raw size, number of pixels bytes
            for (int i = 0; i < 4; i++)
            {
                header[i + infoOffset] = size[i];
            }
            infoOffset += 4;

            for (int i = infoOffset; i < BMP_PIXEL_ARRAY; i++)  //Reste, que des 0, on n'écrit pas la résolution elle n'est pas nécessaire
                header[i] = 0;

            //Fin Header Info

            return header;
        }

        #endregion


        /// <summary>
        /// Convertit un <see cref="int"/> en un tableau de <see cref="byte"/> au format endian
        /// </summary>
        /// <param name="value">Valeur à convertir</param>
        /// <returns></returns>
        public static byte[] ConvertIntToEndian(int value)
        {
            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)(value >> 8 * i);
            }

            return bytes;
        } //public pour les tests unitaires

        /// <summary>
        /// Convertit un tableau de <see cref="byte"/> au format endian en <see cref="int"/>
        /// </summary>
        /// <param name="bt">Tableau de <see cref="byte"/></param>
        /// <returns></returns>
        public static int ConvertEndianToInt(byte[] bt)
        {
            int value = 0;

            for (int i = 0; i < bt.Length; i++)
            {
                value |= bt[i] << (8 * i);
            }

            return value;
        } //public pour les tests unitaires
        

        /// <summary>
        /// Regarde si tous les pixels de la matrice de <see cref="Pixel"/> sont intialisés
        /// </summary>
        /// <returns></returns>
        private bool CheckValiditéImage()
        {
            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    if (this[i, j] == null)
                    {
                        MessageBox.Show($"Image pas bien chargée, choisissez une autre image", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            Console.WriteLine("Le chargement a été un succès !");
            return true;
        }
        

        //Extensions de fichier

        /// <summary>
            /// Récupère un chemin et l'extension <see cref="Extension"/> qui va avec et retourne ce chemin avec l'extension correcte
            /// </summary>
            /// <param name="filename">Chemin jusqu'au fichier</param>
            /// <param name="ext">Extension à ajouter au chemin</param>
            /// <returns></returns>
        private static string GetFilenameWithCorrectExtension(string filename, Extension ext)
        {
            return Path.GetDirectoryName(filename) + @"\" + Path.GetFileNameWithoutExtension(filename) + (ext == Extension.bmp ? ".bmp" : ".csv");
        }
        
        /// <summary>
        /// Récupère l'extension sur le chemin d'un fichier
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="chargement">Si on charge une image et que l'extension n'est pas précisée, on regarde dans le dossier de sauvegarde quelle image pourrait correspondre</param>
        /// <returns></returns>
        private static Extension GetExtension(string filename, bool chargement = false)
        {
            string ext = Path.GetExtension(filename);
            if (ext != null && ext != string.Empty)
            {
                switch (ext)
                {
                    case ".bmp":
                        if ((chargement && File.Exists(GetFilenameWithCorrectExtension(filename, Extension.bmp))) || !chargement) //On check avant de charger si le fichier existe bien
                            return Extension.bmp;
                        break;
                    case ".csv":
                        if ((chargement && File.Exists(GetFilenameWithCorrectExtension(filename, Extension.csv))) || !chargement)
                            return Extension.csv;
                        break;
                    default:
                        return 0;     //Extension non gérée
                }
            }
            if (chargement) //Pas de précision sur le type, on regarde dans le dossier
            {
                if (File.Exists(GetFilenameWithCorrectExtension(filename, Extension.bmp)))
                    return Extension.bmp;
                if (File.Exists(GetFilenameWithCorrectExtension(filename, Extension.csv)))
                    return Extension.csv;
            }
            else
            {
                return Extension.bmp;  //Pas de précision sur le type, on sauvegarde en .bmp par défaut
            }
            return 0;
        }


        /// <summary>
        /// Différents types d'extension de fichier supportées
        /// </summary>
        public enum Extension
        {
            /// <summary>
            /// Format Bitmap ou .bmp
            /// </summary>
            bmp = 1,

            /// <summary>
            /// Format d'image bitmap sous format .csv (comma-separated values) -> lisible par Excel 
            /// </summary>
            csv = 2,
        }


        //Autres
        
        private void ClonePixels(Pixel[,] bp)
        {
            this.pixels = new Pixel[bp.GetLength(0), bp.GetLength(1)];

            this.height = bp.GetLength(0);
            this.width = bp.GetLength(1);

            for (int i = 0; i < this.height; i++)
            {
                for (int j = 0; j < this.width; j++)
                {
                    this[i, j] = bp[i, j];
                }
            }
        }

        /// <summary>
        /// Permet de changer la taille d'une instance <see cref="MyImage"/> sans en changer la référence. Réinitialise le tableau de <see cref="Pixel"/>
        /// </summary>
        /// <param name="height"></param>
        /// <param name="width"></param>
        public void ChangeSize(int height, int width)
        {
            this.height = height;
            this.width = width;

            this.pixels = new Pixel[height, width];
        }

    }
}
