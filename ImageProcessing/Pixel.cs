using System;
using System.Drawing;
using System.Linq;

namespace ImageProcessing
{
    /// <summary>
    /// Représente un pixel R-G-B d'une image, composé de 3 nuances de couleurs : rouges, vertes et bleues
    /// </summary>
    public class Pixel
    {
        //Champs et propriétés

        private byte R;
        private byte G;
        private byte B;


        /// <summary>
        /// Composante rouge
        /// </summary>
        public byte GetR
        {
            get => this.R;
        }

        /// <summary>
        /// Composante verte
        /// </summary>
        public byte GetG
        {
            get => this.G;
        }

        /// <summary>
        /// Composante bleue
        /// </summary>
        public byte GetB
        {
            get => this.B;
        }


        /// <summary>
        /// Renvoie la moyenne des couleurs du Pixel
        /// </summary>
        public int GetMoyenne
        {
            get => (this.R + this.B + this.G) / 3;
        }


        /// <summary>
        /// Retourne <see langword="true"/> si les composants rouges, verts et bleus de cette instance ont la même valeur
        /// </summary>
        public bool IsGris
        {
            get => this.B == this.R && this.R == this.G;
        }

        /// <summary>
        /// Pixel de base, composantes à 0 = noir
        /// </summary>
        public static readonly Pixel Zero = new Pixel(0, 0, 0);


        //Constructeurs

        /// <summary>
        /// Initialises une nouvelle instance de la classe <see cref="Pixel"/> à partir de composants rouge vert et bleu
        /// </summary>
        /// <param name="R">rouge</param>
        /// <param name="G">vert</param>
        /// <param name="B">bleu</param>
        public Pixel(int R, int G, int B)
        {
            this.R = (byte)R;
            this.G = (byte)G;
            this.B = (byte)B;
        }

        /// <summary>
        /// Initialises une nouvelle instance de la classe <see cref="Pixel"/> à partir d'un tableau de taille 3 contenant les composants bleu puis vert puis rouge
        /// </summary>
        public Pixel(byte[] bgr)
        {
            if (bgr == null || bgr.Length != 3)
            {
                return;
            }
            this.R = bgr[2];
            this.G = bgr[1];
            this.B = bgr[0];
        }

        /// <summary>
        /// Initialises une nouvelle instance de la classe <see cref="Pixel"/> à partir d'un <see cref="Pixel"/>
        /// </summary>
        public Pixel(Pixel p) : this(p.R, p.G, p.B)
        {
        }

        /// <summary>
        /// Renvoie une nouvelle instance de la classe <see cref="Pixel"/> à partir de composants rouge vert et bleu
        /// </summary>
        public static Pixel FromRGB(int R, int G, int B)
        {
            return new Pixel(R, G, B);
        }

        /// <summary>
        /// Renvoie une nouvelle instance de la classe <see cref="Pixel"/> à partir d'une couleur <see cref="Couleurs"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Pixel FromColor(Couleurs color)
        {
            int[] rgb = Pixel.CouleursTableau[(int)color];

            return new Pixel(rgb[0], rgb[1], rgb[2]);
        }

        /// <summary>
        /// Renvoie une nouvelle instance de la classe <see cref="Pixel"/> à partir d'une <see cref="Color"/>
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Pixel FromColor(Color color)
        {
            return new Pixel(color.R, color.G, color.B);
        }

        /// <summary>
        /// Renvoie une nouvelle instance de la classe <see cref="Pixel"/> à partir des composants TSV ou TSL (teinte, saturation, luminosité/valeur), HSL ou HSV en anglais
        /// </summary>
        /// <param name="hue">Teinte en degré % 360</param>
        /// <param name="sat">Saturation par rapport à 1</param>
        /// <param name="lum">Luminosité par rapport à 1</param>
        /// <returns></returns>
        public static Pixel FromHSL(int hue, float sat, float lum)
        {
            float r = 0, g = 0, b = 0;

            if (sat == 0)
            {
                r = g = b = lum;
            }
            else
            {
                float q = lum < 0.5f ? lum * (1 + sat) : lum + sat - lum * sat;
                float p = 2 * lum - q;
                r = FromHueToRGB(p, q, hue + (1f / 3));
                g = FromHueToRGB(p, q, hue);
                b = FromHueToRGB(p, q, hue - (1f / 3));
            }

            return new Pixel((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }


        /// <summary>
        /// Renvoie le <see cref="Pixel"/> sous forme d'un <see cref="Color"/> de transparence alpha = 255
        /// </summary>
        /// <returns></returns>
        internal Color ToColor()
        {
            return Color.FromArgb(this.R, this.G, this.B);
        }


        //Opérateurs

        /// <summary>
        /// Renvoie <see langword="true"/> si les 2 pixels ont les mêmes composantes R-G-B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Pixel a, Pixel b)
        {
            if ((a as object) == null && (b as object) == null)
                return true;
            if ((a as object) == null || (b as object) == null)
                return false;
            return a.R == b.R && a.G == b.G && a.B == b.B;
        }

        /// <summary>
        /// Renvoie <see langword="false"/> si les 2 pixels ont les mêmes composantes R-G-B
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator != (Pixel a, Pixel b)
        {
            return !(a == b);
        }


        // méthodes publiques

        /// <summary>
        /// Renvoie un pixel en son équivalent en gris
        /// </summary>
        /// <param name="intensité"></param>
        /// <returns></returns>
        public Pixel TransformationGris(int intensité)
        {
            int somme = this.GetMoyenne;
            if (somme > 255 / 2.0) //Moitié haute, on augmente l'intensité des composants du pixel
            {
                somme += 255 * intensité / 100;
            }
            else //En dessous on les baisses
            {
                somme -= 255 * intensité / 100;
            }
            if (somme > 255) somme = 255;
            if (somme < 0) somme = 0;
            return new Pixel(somme, somme, somme);
        }

        /// <summary>
        /// Renvoie un pixel en augmentant la luminosité en fonction d'un facteur intensité
        /// </summary>
        /// <param name="intensité"></param>
        /// <returns></returns>
        public Pixel TransformationLuminosité(int intensité)
        {
            int augmentation = 255 * intensité / 100;

            int red = this.R + augmentation, green = this.G + augmentation, blue = this.B + augmentation;
            red = Math.Min(Math.Max(red, 0), 255);
            green = Math.Min(Math.Max(green, 0), 255);
            blue = Math.Min(Math.Max(blue, 0), 255);

            return new Pixel(red, green, blue);
        }

        /// <summary>
        /// Renvoie le <see cref="Pixel"/> avec les couleurs inversées
        /// </summary>
        /// <returns></returns>
        public Pixel InversionCouleur()
        {
            return FromRGB(255 - this.R, 255 - this.G, 255 - this.B);
        }



        /// <summary>
        /// Renvoie un pixel en tonalités sépia
        /// </summary>
        /// <returns></returns>
        public Pixel TransformationSépia()
        {
            byte[] bgr = new byte[3];

            bgr[0] = (byte)Math.Min(Math.Max((int)(0.272 * this.R + 0.534 * this.G + 0.131 * this.B), 0), 255);  //Bleu
            bgr[1] = (byte)Math.Min(Math.Max((int)(0.349 * this.R + 0.686 * this.G + 0.168 * this.B), 0), 255);  //Vert
            bgr[2] = (byte)Math.Min(Math.Max((int)(0.393 * this.R + 0.769 * this.G + 0.189 * this.B), 0), 255);  //Rouge

            return new Pixel(bgr);
        }


        /// <summary>
        /// Ajoute une certaine transparence à un pixel
        /// </summary>
        /// <param name="alpha">Transparence, 0 à 1</param>
        /// <param name="backgroundPixel">Pixel de devant</param>
        /// <param name="foregroundPixel">Pixel de derrière</param>
        /// <returns></returns>
        public static Pixel AddTransparence(Pixel backgroundPixel, Pixel foregroundPixel, float alpha)
        {
            float inverse = 1.0f - alpha;
            return FromRGB((byte)(foregroundPixel.GetR * alpha + backgroundPixel.GetR * inverse),
                (byte)(foregroundPixel.GetG * alpha + backgroundPixel.GetG * inverse),
                (byte)(foregroundPixel.GetB * alpha + backgroundPixel.GetB * inverse));
        }


        /// <summary>
        /// Clone les données d'un pixel
        /// </summary>
        /// <returns></returns>
        public Pixel Clone()
        {
            return new Pixel(this);
        }



        /// <summary>
        /// Récupère la <see cref="Couleurs"/> la plus proche du <see cref="Pixel"/> entré en paramètre
        /// </summary>
        /// <param name="p">Pixel</param>
        /// <returns></returns>
        public static Couleurs GetCouleur(Pixel p)
        {
            int maxValue = Enum.GetValues(typeof(Couleurs)).Cast<int>().Max() + 1;
            int miniDistance = int.MaxValue;
            int indexMiniDistance = 0;
            for (int i = 0; i < maxValue; i++)
            {
                int r = CouleursTableau[i][0];
                int g = CouleursTableau[i][1];
                int b = CouleursTableau[i][2];
                int distance = (int)Math.Sqrt(((p.R - r) * (p.R - r)) + ((p.G - g) * (p.G - g)) + ((p.B - b) * (p.B - b)));
                if (distance < miniDistance)
                {
                    miniDistance = distance;
                    indexMiniDistance = i;
                }
            }

            return (Couleurs)indexMiniDistance;
        }



        /// <summary>
        /// Retourne la moyenne entre 2 pixels
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Pixel MoyennePixels(Pixel p1, Pixel p2)
        {
            return AddTransparence(p1, p2, 50);
        }



        /// <summary>
        /// Récupère la luminosité perçue du Pixel
        /// </summary>
        /// <returns></returns>
        public float GetBrightness()
        {
            return 0.2126f * this.R + 0.7152f * this.G + 0.0722f * this.B;
        }


        /// <summary>
        /// Récupère la teinte d'un Pixel
        /// </summary>
        /// <returns></returns>
        public int GetHue()
        {
            if (R == G && G == B)
                return 0;

            float r = (float)this.R / 255.0f;
            float g = (float)this.G / 255.0f;
            float b = (float)this.B / 255.0f;

            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);


            float hue = 0f;
            if (max == r)
            {
                hue = (g - b) / (max - min);

            }
            else if (max == g)
            {
                hue = 2f + ((b - r) / (max - min));

            }
            else
            {
                hue = 4f + ((r - g) / (max - min));
            }

            hue *= 60;
            if (hue < 0) hue += 360;

            return (int)Math.Round(hue);
        }

        /// <summary>
        /// Récupère la luminosité du Pixel
        /// </summary>
        /// <returns></returns>
        public float GetLightness()
        {
            float r = (float)R / 255.0f;
            float g = (float)G / 255.0f;
            float b = (float)B / 255.0f;

            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);

            return (max + min) / 2;
        }

        /// <summary>
        /// Récupère la saturation du Pixel
        /// </summary>
        /// <returns></returns>
        public float GetSaturation()
        {
            float r = (float)this.R / 255.0f;
            float g = (float)this.G / 255.0f;
            float b = (float)this.B / 255.0f;

            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);

            if (min == max)
                return 0f;

            if ((max + min) / 2 <= .5f)
            {
                return (max - min) / (max + min);
            }
            else
            {
                return (max - min) / (2f - max - min);
            }
        }


        private static float FromHueToRGB(float p, float q, float t)
        {
            if (t < 0) t += 1;
            if (t > 1) t -= 1;
            if (t < 1f / 6) return p + (q - p) * 6 * t;
            if (t < 1f / 2) return q;
            if (t < 2f / 3) return p + (q - p) * (2f / 3 - t) * 6;
            return p;
        }


        /// <summary>
        /// Renvoie une chaine de charactère décrivant le <see cref="Pixel"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"| R({R}) G({G}) B({B}) |";
        }


        /// <summary>
        /// R-G-B
        /// </summary>
        private static int[][] CouleursTableau = new int[][]
        {
            //Blanc
            new int[]
            {
                255, 255, 255
            },
            
            //Gris clair
            new int[]
            {
                224, 224, 224
            },

            //Argent
            new int[]
            {
                192, 192, 192
            },

            //Gris
            new int[]
            {
                128, 128, 128
            },
            
            //Gris foncé
            new int[]
            {
                70, 70, 70
            },

            //Gris très foncé
            new int[]
            {
                34, 34, 34
            },
            
            //Noir
            new int[]
            {
                0, 0, 0
            },
            
            //Violet
            new int[]
            {
                138, 43, 226
            },

            //Violet2
            new int[]
            {
                102, 0, 51
            },

            //Indigo
            new int[]
            {
                46, 0, 79
            },
            
            //Bleu marine
            new int[]
            {
                0, 0, 128
            },
            
            //Bleu
            new int[]
            {
                0, 0, 255
            },

            //Bleu 2
            new int[]
            {
                15, 69, 165
            },

            //Cyan
            new int[]
            {
                0, 255, 255
            },

            //Bleu ciel 2
            new int[]
            {
                80, 106, 193
            },

            //Bleu ciel
            new int[]
            {
                119, 180, 254
            },

            //Bleu clair
            new int[]
            {
                153, 204, 254
            },
            
            //Bleu canard
            new int[]
            {
                0, 128, 128
            },
            
            //Vert 2
            new int[]
            {
                41, 120, 0
            },

            //Vert foncé
            new int[]
            {
                0, 128, 0
            },

            //Vert 
            new int[]
            {
                0, 255, 0
            },

            //Vert clair
            new int[]
            {
                144, 238, 178
            },

            //Vert jaune
            new int[]
            {
                229, 255, 204
            },
            
            //Beige
            new int[]
            {
                255, 255, 190
            },

            //Jaune
            new int[]
            {
                255, 255, 0
            },

            //Jaune Kaki
            new int[]
            {
                102, 102, 0
            },

            //Jaune orangé
            new int[]
            {
                255, 204, 0
            },

            //Orange Foncé
            new int[]
            {
                204, 102, 0
            },

            //Orange
            new int[]
            {
                255, 102, 0
            },
            
            //Orange Rosé
            new int[]
            {
                255, 204, 253
            },
            
            //Rose
            new int[]
            {
                255, 102, 102
            },
            
            //Marron
            new int[]
            {
                128, 0, 0
            },
            
            //Magenta
            new int[]
            {
                255, 0, 255
            },
            
            //Rouge clair
            new int[]
            {
                250, 180, 170
            },

            //Rouge
            new int[]
            {
                255, 0, 0
            },

            //Rouge Foncé
            new int[]
            {
                153, 0, 0
            },

        };

    }

    /// <summary>
    /// Types de couleurs de base. 35 couleurs disponibles
    /// </summary>
    public enum Couleurs
    {
        Blanc,

        Gris_Clair,
        Argent,
        Gris,
        Gris_Foncé,
        Noir_Clair,

        Noir,

        Violet,
        Violet2,
        Indigo,
        Bleu_Marine,
        Bleu,
        Bleu2,
        Cyan,
        Bleu_ciel2,
        Bleu_Ciel,
        Bleu_Clair,
        Bleu_Canard,
        Vert2,
        Vert_Foncé,
        Vert,
        Vert_Clair,
        Vert_Jaune,
        Beige,
        Jaune,
        Jaune_Kaki,
        Jaune_Orangé,
        Orange_Foncé,
        Orange,
        Orange_Rosé,
        Rose,
        Marron,
        Magenta,
        Rouge_Clair,
        Rouge,
        Rouge_Foncé
    }
}
