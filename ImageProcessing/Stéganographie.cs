using System;
namespace ImageProcessing
{
    public class Stéganographie//public pour les tests unitaires
    {

        /// <summary>
        /// Cache une <see cref="MyImage"/> dans une autre <see cref="MyImage"/> visible et renvoie le résultat. <para/>
        /// Si l'image à cacher est plus petite que l'image visible, alors on cache l'image au milieu de l'image visible.<para/>
        /// Si l'image à cacher est plus grande que l'image visible, alors on rogne les bords des 4 côtés de l'image à cacher pour que les 2 images soient de même taille.
        /// </summary>
        /// <param name="imageVisible">Image visible</param>
        /// <param name="imageToHide">Image à cacher</param>
        /// <param name="nbBitsToHide">Nombre de bits à cacher sur 1 pixel</param>
        public static MyImage CacherImage(MyImage imageVisible, MyImage imageToHide, int nbBitsToHide = 4)
        {
            nbBitsToHide = nbBitsToHide < 0 ? 0 : nbBitsToHide > 8 ? 8 : nbBitsToHide;

            if (nbBitsToHide == 0)
                return imageVisible.GetCopie();


            int startHeight = (imageToHide.GetHeight - imageVisible.GetHeight) / 2;
            int startWidth = (imageToHide.GetWidth - imageVisible.GetWidth) / 2;


            byte décallage = (byte)(8 - nbBitsToHide);

            byte bits_visibles = (byte)(0b1111_1111 << nbBitsToHide);

            MyImage addition = new MyImage(imageVisible.GetHeight, imageVisible.GetWidth);

            for (int i = 0; i < imageVisible.GetHeight; i++)
            {
                for (int j = 0; j < imageVisible.GetWidth; j++)
                {
                    Pixel visible = imageVisible[i, j];

                    if (i + startHeight >= 0 && j + startWidth >= 0 && i + startHeight < imageToHide.GetHeight && j + startWidth < imageToHide.GetWidth)
                    {
                        Pixel caché = imageToHide[i + startHeight, j + startWidth];

                        byte[] bgr = new byte[3];
                        bgr[0] = (byte)((visible.GetB & bits_visibles) + (caché.GetB >> décallage));
                        bgr[1] = (byte)((visible.GetG & bits_visibles) + (caché.GetG >> décallage));
                        bgr[2] = (byte)((visible.GetR & bits_visibles) + (caché.GetR >> décallage));

                        addition[i, j] = new Pixel(bgr);
                    }
                    else
                    {
                        addition[i, j] = visible;
                    }
                }
            }


            return addition;
        }

        /// <summary>
        /// Renvoie une image cachée dans une autre
        /// </summary>
        /// <param name="imageVisible">Image à décoder</param>
        /// <param name="nbBitsHidden">Nombre de bits cachés sur 1 pixel</param>
        /// <returns></returns>
        public static MyImage GetImageCachée(MyImage imageVisible, int nbBitsHidden = 4)
        {
            MyImage imageCachée = new MyImage(imageVisible.GetHeight, imageVisible.GetWidth);

            nbBitsHidden = nbBitsHidden < 0 ? 0 : nbBitsHidden > 8 ? 8 : nbBitsHidden;

            if (nbBitsHidden == 0 || nbBitsHidden == 8)
                return imageVisible.GetCopie();

            byte décallage = (byte)(8 - nbBitsHidden);

            byte bits_cachés = (byte)(0b1111_1111 >> décallage);

            for (int i = 0; i < imageCachée.GetHeight; i++)
            {
                for (int j = 0; j < imageCachée.GetWidth; j++)
                {
                    Pixel addition = imageVisible[i, j];

                    byte[] bgr = new byte[3];
                    bgr[0] = (byte)((addition.GetB & bits_cachés) << décallage);
                    bgr[1] = (byte)((addition.GetG & bits_cachés) << décallage);
                    bgr[2] = (byte)((addition.GetR & bits_cachés) << décallage);

                    imageCachée[i, j] = new Pixel(bgr);
                }
            }

            return imageCachée;
        }



        /// <summary>
        /// Cache du texte au format 8 bits (256 premiers chars du format UTF-8) dans les bits les moins importants d'une copie d'une image et renvoie le résultat. <para/>
        /// Les pixels sont choisis pseudo-aléatoirement à partir d'un mot de passe. Les charactères ne faisant pas parti du format 8 bits sont ignorés. <para/>
        /// Ne pas utiliser la même image pour cacher 2 textes car l'un pourrait empiéter sur l'autre et corrompre certaines parties du texte. <para/>
        /// La longueur du texte par rapport à la taille de l'image n'influe pas sur le temps de calcul
        /// </summary>
        /// <param name="image">Image dans laquelle cacher du texte</param>
        /// <param name="txt">Le texte à cacher au format 8 bits. Si la longueur du texte sera inconnue lors du décodage, le terminer par \u0003 </param>
        /// <param name="password">Mot de passe pour retrouver le texte par la suite. Pas de format particulier à respecter</param>
        /// <param name="nbreBitsCachés">Nombre de bits sur lesquels cacher le texte sur une couleur (rgb) d'un pixel. 1, 2, 4 ou 8</param>
        /// <returns></returns>
        public static MyImage CacherTexte(MyImage image, string txt, string password, int nbreBitsCachés = 2)
        {
            Random rand = new Random(password.GetHashCode()); //On génère une classe Rand à partir du hashcode du mdp. On pourrait générer le hashcode à la main pour plus de sécu

            MyImage copieImage = image.GetCopie();

            byte décallage = (byte)(nbreBitsCachés = nbreBitsCachés < 0 ? 0 : nbreBitsCachés > 8 ? 8 : nbreBitsCachés);
            if (décallage == 0)
                return copieImage;

            byte bitsVisibles = (byte)(0b1111_1111 << décallage);

            byte bitsInvisibles = (byte)(0b1111_1111 >> (8 - décallage));

            byte[] textByte = GetByteArrayFrom8BitsString(Get8BitsString(txt));

            int nombrePixelsToChange = textByte.Length * (int)Math.Ceiling((double)(8 / décallage));

            if (nombrePixelsToChange > copieImage.GetHeight * copieImage.GetWidth * 3)
            {
                Console.WriteLine("L'image est trop petite pour cacher tout le texte");
                return copieImage;
            }

            //Point[] trackingPositions = new Point[nombrePixelsToChange]; //Toutes les positions dont il faut changer les pixels. Dans l'ordre . Ancien algo
            //int[] trackingPixelsUsedByPosition = new int[nombrePixelsToChange];   //Tous les pixels associés aux positions dont il faut changer le pixel

            //Init tabs des coordonnées
            int totalCoord = image.GetWidth * image.GetHeight * 3;
            int[] tableauDesCoordonnéesExistentes = new int[totalCoord];
            int[] indexDesCoordonnéesDispo = new int[totalCoord];

            for (int k = 0; k < totalCoord; k++)
            {
                tableauDesCoordonnéesExistentes[k] = k;
                indexDesCoordonnéesDispo[k] = k;
            }


            int indexChar = 0;
            int indexDansChar = 0;

            for (int i = 0; i < nombrePixelsToChange; i++)
            {
                int index = rand.Next(tableauDesCoordonnéesExistentes.Length - i); //Index excluant la fin du tab d'index, index déjà utilisés

                int position = tableauDesCoordonnéesExistentes[indexDesCoordonnéesDispo[index]];
                int temp = indexDesCoordonnéesDispo[tableauDesCoordonnéesExistentes.Length - 1 - i];
                indexDesCoordonnéesDispo[tableauDesCoordonnéesExistentes.Length - 1 - i] = index;
                indexDesCoordonnéesDispo[index] = temp;

                int rgb = position % 3 == 0 ? 0 : position % 2 == 0 ? 1 : 2;

                position /= 3;
                int X = position % image.GetWidth;
                int Y = position / image.GetWidth;

                /* Ancien algorithme : on choisit au hasard des coord jusqu'à ce que la coord choisie n'ait pas déjà été utilisée (on utilise alors
                 * un tableau contenant toutes les coord déjà utilisées et on y test chaque nouvelle coord choisie).
                 * Problème, moins il y a de coord dispo plus cela va prendre du temps d'en trouver une dispo. Le nouvel algo gère ce problème,
                 * beaucoup plus efficace mais plus couteux en mémoire, on met dans 2 tabs toutes les coord possibles de l'image, on cherche un 
                 * index aléatoirement dans le 1er tab, cet index correspond à une coord dans le 2ème tab à partir de laquelle on déduit la position
                 * et la composante du pixel à utiliser. Ensuite cet index est placé à la fin du 1er tab (on échange les 2 coord dans le tab) et on
                 * ne tire donc un nombre aléatoire qu'entre le début du 1er tab et la limite à partir de laquelle les coord ont déjà été utilisées
                 * Chaque index tiré est donc unique et correspond à une coord unique dans le 2ème tab
                
                Ancien algo :
                int Y = rand.Next(copieImage.GetHeight);  //Pos Y
                int X = rand.Next(copieImage.GetWidth);   //Pos X
                int rgb = rand.Next(0, 3);                //Composante du pixel à changer
                
                Point pos = new Point(Y, X);
                
                //Plus le rapport 'taille de l'image / longueur du texte' est petit, plus l'algorithme mettre du temps à trouver des cases à changer
                //
                while (!PositionAndPixelUnique(trackingPositions, pos, trackingPixelsUsedByPosition, rgb))
                {
                    Y = rand.Next(copieImage.GetHeight);
                    X = rand.Next(copieImage.GetWidth);
                    rgb = rand.Next(0, 3);
                    pos = new Point(Y, X);
                }
                
                trackingPositions[i] = pos;
                trackingPixelsUsedByPosition[i] = rgb;
                */
                Pixel pixel = copieImage[Y, X];

                byte bytesTexte = (byte)((textByte[indexChar] & (bitsInvisibles << indexDansChar)) >> indexDansChar);

                if (rgb == 0)
                {
                    byte àChanger = (byte)((pixel.GetR & bitsVisibles) + bytesTexte);
                    pixel = new Pixel(àChanger, pixel.GetG, pixel.GetB);
                }
                else if (rgb == 1)
                {
                    byte àChanger = (byte)((pixel.GetG & bitsVisibles) + bytesTexte);
                    pixel = new Pixel(pixel.GetR, àChanger, pixel.GetB);
                }
                else
                {
                    byte àChanger = (byte)((pixel.GetB & bitsVisibles) + bytesTexte);
                    pixel = new Pixel(pixel.GetR, pixel.GetG, àChanger);
                }

                copieImage[Y, X] = pixel;

                if (indexDansChar + nbreBitsCachés >= 8) //On change de char à cacher
                {
                    indexChar++;
                    indexDansChar = 0;
                }
                else
                    indexDansChar += nbreBitsCachés;  //On change les bits à cacher dans le char
            }

            return copieImage;
        }

        /// <summary>
        /// Récupère un texte caché à partir dans une <see cref="MyImage"/> à partir d'un mot de passe et de la longueur du texte à trouver (pas obligé mais le texte doit alors finir par \u0003 pour signifier la fin).
        /// </summary>
        /// <param name="image">Image dans laquelle retrouver le message caché</param>
        /// <param name="password">Mot de passe</param>
        /// <param name="tailleMessage">Taille du message à retrouver. Si inconnue, mettre à 0, la longueur de texte maximale cherchée sera alors de 10 000 char.</param>
        /// <param name="nbreBitsCachés">Nombre de bits utilisés pour cacher le message sur 1 pixel. 1, 2, 4 ou 8</param>
        /// <returns></returns>
        public static string GetTexteCaché(MyImage image, string password, int tailleMessage = 0, int nbreBitsCachés = 2)
        {
            Random rand = new Random(password.GetHashCode()); //On génère une classe Rand à partir du hashcode du mdp. On pourrait générer le hashcode à la main pour plus de sécu

            const int tailleMaxTexteSiTailleInconnue = 10000;

            byte décallage = (byte)(nbreBitsCachés = nbreBitsCachés < 0 ? 0 : nbreBitsCachés > 8 ? 8 : nbreBitsCachés);
            if (décallage == 0)
                return string.Empty;

            byte bitsVisibles = (byte)(0b1111_1111 << décallage);

            byte bitsInvisibles = (byte)(0b1111_1111 >> (8 - décallage));


            int nombrePixelsToChange = 0;
            if (tailleMessage <= 0) //Si on connait pas la taille du texte à récupérer
            {
                //Nb max de char. Prendre le nombre max possible 
                int max = image.GetWidth * image.GetHeight * 3;
                nombrePixelsToChange = tailleMaxTexteSiTailleInconnue > max ? max : tailleMaxTexteSiTailleInconnue;
            }
            else
            {
                nombrePixelsToChange = tailleMessage * (int)Math.Ceiling((double)(8 / décallage));

                int max = image.GetWidth * image.GetHeight * 3;
                nombrePixelsToChange = nombrePixelsToChange > max ? max : nombrePixelsToChange;
            }

            byte[] chars = new byte[tailleMessage];
            if (tailleMessage <= 0)
            {
                chars = new byte[tailleMaxTexteSiTailleInconnue / (int)Math.Ceiling((double)(8 / décallage))];
            }
            else
            {
                chars = new byte[tailleMessage];
            }

            //Init tabs des coordonnées
            int posTotal = image.GetWidth * image.GetHeight * 3;
            int[] positionsAvailable = new int[posTotal];
            int[] positionsUsed = new int[posTotal];

            for (int k = 0; k < posTotal; k++)
            {
                positionsAvailable[k] = k;
                positionsUsed[k] = k;
            }


            int tempChar = 0; //Variable dans laquelle on enregistre les bits récupérés pour 1 char
            int indexChar = 0;   //Position dans le tableau de char
            int indexDansChar = 0;  //Position dans le byte du char du tableau de char

            for (int i = 0; i < nombrePixelsToChange; i++)
            {
                int index = rand.Next(positionsAvailable.Length - i);

                int position = positionsAvailable[positionsUsed[index]];
                int temp = positionsUsed[positionsAvailable.Length - 1 - i];
                positionsUsed[positionsAvailable.Length - 1 - i] = index;
                positionsUsed[index] = temp;

                //Une position prend 3 espaces dans le tableau pour chaque composante du pixel : les nb aux index 0, 1, 2 rpz la même pos
                //L'ordre des couleurs n'importe pas tant que le même algo a été utilisé pour cacher le texte
                int rgb = position % 3 == 0 ? 0 : position % 2 == 0 ? 1 : 2;

                position /= 3;
                int X = position % image.GetWidth;
                int Y = position / image.GetWidth;

                Pixel pixel = image[Y, X];

                byte byteARetrouver = 0;
                if (rgb == 0)
                {
                    byteARetrouver = pixel.GetR;
                }
                else if (rgb == 1)
                {
                    byteARetrouver = pixel.GetG;
                }
                else
                {
                    byteARetrouver = pixel.GetB;
                }

                tempChar += (byte)((byteARetrouver & bitsInvisibles) << indexDansChar);

                if (indexDansChar + nbreBitsCachés >= 8)
                {
                    if (tempChar == '\u0003') //Fin du texte si on ne connaissait pas la taille du texte
                    {
                        byte[] realSizeChars = new byte[indexChar + 1];
                        for (int k = 0; k < realSizeChars.Length; k++)
                        {
                            realSizeChars[k] = chars[k];
                        }
                        return Get8BitsStringFromByteArray(realSizeChars);
                    }
                    chars[indexChar] = (byte)tempChar;
                    tempChar = 0;
                    indexChar++;
                    indexDansChar = 0;
                }
                else
                    indexDansChar += nbreBitsCachés;
            }

            return Get8BitsStringFromByteArray(chars);
        }


        private static byte[] GetByteArrayFrom8BitsString(string text)
        {
            byte[] bytes = new byte[text.Length];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)text[i];
            }

            return bytes;
        }

        private static string Get8BitsStringFromByteArray(byte[] array)
        {
            string txt = string.Empty;
            for (int i = 0; i < array.Length; i++)
            {
                txt += ((char)array[i]).ToString();
            }
            return txt;
        }


        private static string Get8BitsString(string text)
        {
            string realText = "";

            for (int i = 0; i < text.Length; i++)
            {
                if (Is8Bits(text[i]))
                    realText += text[i];
                else
                    realText += '?';
            }

            return realText;
        }

        public static bool Is8Bits(char c)
        {
            return c <= 255;
        }//public pour le test unitaire
    }
}
