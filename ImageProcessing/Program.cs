using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessing
{
    static class Program
    {
        public static readonly string ADRESSE_SAUVEGARDE =    //Insérer chemin jusqu'au dossier de sauvegarde contenant les fichiers
            Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).FullName;

        
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Photoshop3000());
        }
    }
}
