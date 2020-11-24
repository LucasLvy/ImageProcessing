using System;

namespace ImageProcessing
{
    /// <summary>
    /// Représente un nombre complexe à valeur flottante
    /// </summary>
    internal struct NombreComplex
    {
        //Champs et propriétés

        private float réel, imag;

        /// <summary>
        /// Valeur réelle
        /// </summary>
        public float Re
        {
            get => réel;
            set => réel = value;
        }

        /// <summary>
        /// Valeur imaginaire
        /// </summary>
        public float Im
        {
            get => imag;
            set => imag = value;
        }


        //Constructeur

        /// <summary>
        /// Déclare une nouvelle variable <see cref="NombreComplex"/> avec les paramètres spécifiés
        /// </summary>
        /// <param name="reel"></param>
        /// <param name="imag"></param>
        public NombreComplex(double reel, double imag)
        {
            this.réel = (float)reel;
            this.imag = (float)imag;
        }


        //Opérateurs

        /// <summary>
        /// Renvoie le résultat de l'addition entre 2 <see cref="NombreComplex"/> 
        /// </summary>
        public static NombreComplex operator +(NombreComplex j, NombreComplex z)
        {
            return new NombreComplex(j.Re + z.Re, j.Im + z.Im);
        }

        /// <summary>
        /// Renvoie le résultat de la soustraction entre 2 <see cref="NombreComplex"/> 
        /// </summary>
        public static NombreComplex operator -(NombreComplex j, NombreComplex z)
        {
            return new NombreComplex(j.Re - z.Re, j.Im - z.Im);
        }

        /// <summary>
        /// Renvoie le résultat de la multiplication entre 2 <see cref="NombreComplex"/> 
        /// </summary>
        public static NombreComplex operator *(NombreComplex j, NombreComplex z)
        {
            return new NombreComplex(j.Re * z.Re - j.Im * z.Im, j.Re * z.Im + z.Re * j.Im);
        }

        /// <summary>
        /// Renvoie le résultat de la division entre 2 <see cref="NombreComplex"/> 
        /// </summary>
        public static NombreComplex operator /(NombreComplex j, NombreComplex z)
        {
            j *= GetConjugué(z);
            z *= GetConjugué(z);

            return new NombreComplex(j.Re / z.Re, j.Im / z.Re);
        }

        /// <summary>
        /// Renvoie le résultat d'un <see cref="NombreComplex"/> élevé à la puissance spécifiée
        /// </summary>
        public static NombreComplex operator ^(NombreComplex j, double Pow)
        {
            Pow = (int)Pow;
            if (Pow == 0)
            {
                return 1;
            }
            else if (Pow < 0)
            {
                return 1 / (j ^ -Pow);
            }

            NombreComplex result = j;
            
            for (int i = 1; i < Pow; i++)
            {
                result *= j;
            }

            return result;
        }

        /// <summary>
        /// Renvoie un nouveau <see cref="NombreComplex"/> initialisé avec un nombre
        /// </summary>
        public static implicit operator NombreComplex(double x)
        {
            return new NombreComplex(x, 0);
        }
        

        public static bool operator ==(NombreComplex j, NombreComplex z)
        {
            return j.Re == z.Re && z.Im == j.Im;
        }
        public static bool operator !=(NombreComplex j, NombreComplex z)
        {
            return !(j == z);
        }


        //Méthodes publiques

        /// <summary>
        /// Renvoie le sinus du <see cref="NombreComplex"/> indiqué
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public static NombreComplex Sin(NombreComplex j)
        {
            return new NombreComplex(Cosh(j.Im) * Math.Sin(j.Re), Sinh(j.Im) * Math.Cos(j.Re));
        }

        /// <summary>
        /// Renvoie le cosinus du <see cref="NombreComplex"/> indiqué
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        public static NombreComplex Cos(NombreComplex j)
        {
            return new NombreComplex(Cosh(j.Re) * Math.Cos(j.Re), -Sinh(j.Im) * Math.Sin(j.Im));
        }

        private static double Cosh(double teta)
        {
            return (Math.Exp(teta) + Math.Exp(-teta)) / 2;
        }
        
        private static double Sinh(double theta)
        {
            return (Math.Exp(theta) - Math.Exp(-theta)) / 2;
        }



        private static NombreComplex GetConjugué(NombreComplex z)
        {
            return new NombreComplex(z.Re, -z.Im);
        }


        /// <summary>
        /// Renvoie le module d'un <see cref="NombreComplex"/> passé en paramètre
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public static float GetModule(NombreComplex z)
        {
            return (float)Math.Sqrt(Math.Pow(z.Re, 2) + Math.Pow(z.Im, 2));
        }

        /// <summary>
        /// Renvoie le module du <see cref="NombreComplex"/> 
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        public float GetModule()
        {
            return GetModule(this);
        }


        /// <summary>
        /// Renvoie la distance entre 2 <see cref="NombreComplex"/>
        /// </summary>
        /// <param name="z"></param>
        /// <param name="j"></param>
        /// <returns></returns>
        public static float GetDistance(NombreComplex z, NombreComplex j)
        {
            return Math.Abs(z.GetModule() - j.GetModule());
        }

        /// <summary>
        /// Renvoie la distance entre 2 <see cref="NombreComplex"/>
        /// </summary>
        public float GetDistance(NombreComplex z)
        {
            return GetDistance(this, z);
        }



        public override string ToString()
        {
            return $" {this.Re} {(this.Im < 0 ? "-" : "+")} {Math.Abs(this.Im)}i ";
        }
    }
}
