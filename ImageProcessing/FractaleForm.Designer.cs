using System;
using System.Windows.Forms;

namespace ImageProcessing
{
    partial class FractaleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FractaleForm));
            this.SaveButton = new System.Windows.Forms.Button();
            this.Clear = new System.Windows.Forms.Button();
            this.LB_equations = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_Re = new System.Windows.Forms.TextBox();
            this.TB_Im = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.LB_récursiff = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.TB_height = new System.Windows.Forms.TextBox();
            this.TB_width = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.TB_iteration = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.But_imageMosaique = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.TB_tryColor = new System.Windows.Forms.TextBox();
            this.TB_contraste = new System.Windows.Forms.TextBox();
            this.TB_mosaique = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label19 = new System.Windows.Forms.Label();
            this.But_Color = new System.Windows.Forms.Button();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(217)))), ((int)(((byte)(32)))));
            this.SaveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SaveButton.Location = new System.Drawing.Point(607, 337);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(298, 74);
            this.SaveButton.TabIndex = 11;
            this.SaveButton.Text = "Créer";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Clear
            // 
            this.Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Clear.BackColor = System.Drawing.Color.Red;
            this.Clear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Clear.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Clear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Clear.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Clear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Clear.Location = new System.Drawing.Point(301, 337);
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(296, 74);
            this.Clear.TabIndex = 12;
            this.Clear.Text = "Annuler";
            this.Clear.UseVisualStyleBackColor = false;
            // 
            // LB_equations
            // 
            this.LB_equations.BackColor = System.Drawing.Color.DimGray;
            this.LB_equations.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_equations.ForeColor = System.Drawing.SystemColors.Window;
            this.LB_equations.FormattingEnabled = true;
            this.LB_equations.HorizontalScrollbar = true;
            this.LB_equations.ItemHeight = 20;
            this.LB_equations.Items.AddRange(new object[] {
            "Mandelbrot                           (z = z² +c)",
            "Lapin                                  (z = z² - 0.123 + 0.745i)",
            "Dentrite                              (z = z² -i)",
            "Dentrite division                  (z = z² - 0.1 - i)",
            "Ilots                                     (z = z² + 0.1 + 0.65i)",
            "Ilots serpents                        (z = z² - 0.231 + 0.785i)",
            "Escargot                              (z = z² - 0.777 + 0.111i)",
            "Escargot révolution               (z = z² + 0.3 + 0.5i)",
            "Forme 1                               (z = z² - 0.39 - 0.59i)",
            "Forme 2                               (z = z² - 0.1 - 0.65i)",
            "Forme 3                                (z = z² - 3/4)",
            "Créer votre propre équation"});
            this.LB_equations.Location = new System.Drawing.Point(19, 45);
            this.LB_equations.Name = "LB_equations";
            this.LB_equations.Size = new System.Drawing.Size(266, 304);
            this.LB_equations.TabIndex = 13;
            this.LB_equations.SelectedIndexChanged += new System.EventHandler(this.LB_equations_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(12, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(240, 36);
            this.label1.TabIndex = 14;
            this.label1.Text = "Choisissez une forme fractale\r\ngénérée à partir d\'une équation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(32, 349);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "Créer votre propre équation :";
            // 
            // TB_Re
            // 
            this.TB_Re.Enabled = false;
            this.TB_Re.Location = new System.Drawing.Point(36, 389);
            this.TB_Re.Name = "TB_Re";
            this.TB_Re.Size = new System.Drawing.Size(90, 22);
            this.TB_Re.TabIndex = 16;
            this.TB_Re.Text = "0";
            this.TB_Re.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_Re.TextChanged += new System.EventHandler(this.TB_Re_TextChanged);
            this.TB_Re.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_Re_KeyPress);
            // 
            // TB_Im
            // 
            this.TB_Im.Enabled = false;
            this.TB_Im.Location = new System.Drawing.Point(159, 389);
            this.TB_Im.Name = "TB_Im";
            this.TB_Im.Size = new System.Drawing.Size(94, 22);
            this.TB_Im.TabIndex = 17;
            this.TB_Im.Text = "0 i";
            this.TB_Im.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_Im.TextChanged += new System.EventHandler(this.TB_Im_TextChanged);
            this.TB_Im.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_Im_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(65, 369);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 18);
            this.label3.TabIndex = 18;
            this.label3.Text = "Re :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(191, 368);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 18);
            this.label4.TabIndex = 19;
            this.label4.Text = "Im :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(295, 2);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(280, 36);
            this.label5.TabIndex = 20;
            this.label5.Text = "Ou choisissez des formes générées\r\nde manière récursive";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LB_récursiff
            // 
            this.LB_récursiff.BackColor = System.Drawing.Color.DimGray;
            this.LB_récursiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LB_récursiff.ForeColor = System.Drawing.SystemColors.Window;
            this.LB_récursiff.FormattingEnabled = true;
            this.LB_récursiff.HorizontalScrollbar = true;
            this.LB_récursiff.ItemHeight = 20;
            this.LB_récursiff.Items.AddRange(new object[] {
            "Triangle de Sierpinski",
            "Tapis de Sierpinski",
            "Flocon de Koch",
            "Flocon de Koch en profondeur",
            "Arbre récursif",
            "Arbre récursif 4 côtés",
            "Cercle récursif",
            "Cercle récursif 2",
            "Kaléïdoscope",
            "Mosaïque de kaléïdoscope",
            "Mosaïque à partir d\'une image"});
            this.LB_récursiff.Location = new System.Drawing.Point(298, 45);
            this.LB_récursiff.Name = "LB_récursiff";
            this.LB_récursiff.Size = new System.Drawing.Size(299, 284);
            this.LB_récursiff.TabIndex = 21;
            this.LB_récursiff.SelectedIndexChanged += new System.EventHandler(this.LB_récursiff_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label6.Location = new System.Drawing.Point(607, -1);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 332);
            this.label6.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label7.Location = new System.Drawing.Point(607, 321);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(298, 10);
            this.label7.TabIndex = 23;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.LightGray;
            this.label8.Location = new System.Drawing.Point(617, -1);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(288, 322);
            this.label8.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(895, -1);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(10, 332);
            this.label9.TabIndex = 25;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label10.Location = new System.Drawing.Point(607, -1);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(298, 10);
            this.label10.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.LightGray;
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label11.Location = new System.Drawing.Point(700, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(108, 22);
            this.label11.TabIndex = 27;
            this.label11.Text = "Paramètres";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TB_height
            // 
            this.TB_height.Enabled = false;
            this.TB_height.Location = new System.Drawing.Point(635, 67);
            this.TB_height.Name = "TB_height";
            this.TB_height.Size = new System.Drawing.Size(100, 22);
            this.TB_height.TabIndex = 28;
            this.TB_height.Text = "1000";
            this.TB_height.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.TB_height, "Hauteur de l\'image à créer");
            this.TB_height.TextChanged += new System.EventHandler(this.TB_height_TextChanged);
            this.TB_height.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_height_KeyPress);
            // 
            // TB_width
            // 
            this.TB_width.Enabled = false;
            this.TB_width.Location = new System.Drawing.Point(772, 67);
            this.TB_width.Name = "TB_width";
            this.TB_width.Size = new System.Drawing.Size(100, 22);
            this.TB_width.TabIndex = 29;
            this.TB_width.Text = "1000";
            this.TB_width.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.TB_width, "Largeur de l\'image à créer");
            this.TB_width.TextChanged += new System.EventHandler(this.TB_width_TextChanged);
            this.TB_width.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_width_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.LightGray;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label12.Location = new System.Drawing.Point(654, 46);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(60, 18);
            this.label12.TabIndex = 30;
            this.label12.Text = "Hauteur";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.LightGray;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label13.Location = new System.Drawing.Point(792, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 18);
            this.label13.TabIndex = 31;
            this.label13.Text = "Largeur";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TB_iteration
            // 
            this.TB_iteration.Enabled = false;
            this.TB_iteration.Location = new System.Drawing.Point(783, 104);
            this.TB_iteration.Name = "TB_iteration";
            this.TB_iteration.Size = new System.Drawing.Size(100, 22);
            this.TB_iteration.TabIndex = 32;
            this.TB_iteration.Text = "200";
            this.TB_iteration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_iteration.TextChanged += new System.EventHandler(this.TB_iteration_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.LightGray;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label14.Location = new System.Drawing.Point(645, 105);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(111, 18);
            this.label14.TabIndex = 33;
            this.label14.Text = "Max d\'itération :";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label14, "Nombre d\'itération pour la génération d\'une forme récursive\r\nMax d\'itération pour" +
        " la génération d\'une forme fractale de julia\r\nPour un kaléidoscope, c\'est le nom" +
        "bre de forme générées");
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.LightGray;
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label15.Location = new System.Drawing.Point(805, 186);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(58, 20);
            this.label15.TabIndex = 34;
            this.label15.Text = " Image ";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label15, "Image pour créer une mosaïque");
            // 
            // But_imageMosaique
            // 
            this.But_imageMosaique.BackColor = System.Drawing.Color.DimGray;
            this.But_imageMosaique.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_imageMosaique.Enabled = false;
            this.But_imageMosaique.Font = new System.Drawing.Font("MS Reference Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.But_imageMosaique.ForeColor = System.Drawing.Color.White;
            this.But_imageMosaique.Location = new System.Drawing.Point(783, 209);
            this.But_imageMosaique.Name = "But_imageMosaique";
            this.But_imageMosaique.Size = new System.Drawing.Size(109, 109);
            this.But_imageMosaique.TabIndex = 50;
            this.But_imageMosaique.Text = "CLIQUE ICI";
            this.But_imageMosaique.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolTip1.SetToolTip(this.But_imageMosaique, "Image pour créer une mosaïque");
            this.But_imageMosaique.UseVisualStyleBackColor = false;
            this.But_imageMosaique.Click += new System.EventHandler(this.But_imageMosaique_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.LightGray;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label16.Location = new System.Drawing.Point(620, 211);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(139, 18);
            this.label16.TabIndex = 51;
            this.label16.Text = "Nombre mosaïque :";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label16, "Nombre de mosaïque en largeur sur l\'image à créer");
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.LightGray;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label17.Location = new System.Drawing.Point(654, 165);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(81, 18);
            this.label17.TabIndex = 52;
            this.label17.Text = "Contraste :";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.LightGray;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label18.Location = new System.Drawing.Point(630, 141);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(147, 18);
            this.label18.TabIndex = 53;
            this.label18.Text = "Max d\'essai couleur :";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label18, resources.GetString("label18.ToolTip"));
            // 
            // TB_tryColor
            // 
            this.TB_tryColor.Enabled = false;
            this.TB_tryColor.Location = new System.Drawing.Point(783, 141);
            this.TB_tryColor.Name = "TB_tryColor";
            this.TB_tryColor.Size = new System.Drawing.Size(100, 22);
            this.TB_tryColor.TabIndex = 54;
            this.TB_tryColor.Text = "10";
            this.TB_tryColor.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_tryColor.TextChanged += new System.EventHandler(this.TB_tryColor_TextChanged);
            this.TB_tryColor.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_tryColor_KeyPress);
            // 
            // TB_contraste
            // 
            this.TB_contraste.Enabled = false;
            this.TB_contraste.Location = new System.Drawing.Point(648, 186);
            this.TB_contraste.Name = "TB_contraste";
            this.TB_contraste.Size = new System.Drawing.Size(100, 22);
            this.TB_contraste.TabIndex = 55;
            this.TB_contraste.Text = "1";
            this.TB_contraste.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TB_contraste.TextChanged += new System.EventHandler(this.TB_contraste_TextChanged);
            this.TB_contraste.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_contraste_KeyPress);
            // 
            // TB_mosaique
            // 
            this.TB_mosaique.Enabled = false;
            this.TB_mosaique.Location = new System.Drawing.Point(648, 232);
            this.TB_mosaique.Name = "TB_mosaique";
            this.TB_mosaique.Size = new System.Drawing.Size(100, 22);
            this.TB_mosaique.TabIndex = 56;
            this.TB_mosaique.Text = "30";
            this.TB_mosaique.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.TB_mosaique, "Nombre de kaléidoscope en largeur");
            this.TB_mosaique.TextChanged += new System.EventHandler(this.TB_mosaique_TextChanged);
            this.TB_mosaique.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TB_mosaique_KeyPress);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.LightGray;
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.SystemColors.MenuText;
            this.label19.Location = new System.Drawing.Point(617, 267);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(145, 18);
            this.label19.TabIndex = 57;
            this.label19.Text = "Couleur arrière plan :";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip1.SetToolTip(this.label19, "Couleur d\'arrière plan pour les formes générées récursivement.\r\nCliquez sur le bo" +
        "uton ci-dessous pour changer la couleur");
            // 
            // But_Color
            // 
            this.But_Color.BackColor = System.Drawing.Color.White;
            this.But_Color.Cursor = System.Windows.Forms.Cursors.Hand;
            this.But_Color.Enabled = false;
            this.But_Color.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.But_Color.FlatAppearance.BorderSize = 5;
            this.But_Color.Location = new System.Drawing.Point(620, 288);
            this.But_Color.Name = "But_Color";
            this.But_Color.Size = new System.Drawing.Size(157, 30);
            this.But_Color.TabIndex = 58;
            this.toolTip1.SetToolTip(this.But_Color, "Cliquez ici pour choisir la couleur de remplissage ");
            this.But_Color.UseVisualStyleBackColor = false;
            this.But_Color.Click += new System.EventHandler(this.But_Color_Click);
            // 
            // FractaleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(913, 423);
            this.Controls.Add(this.But_Color);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.TB_mosaique);
            this.Controls.Add(this.TB_contraste);
            this.Controls.Add(this.TB_tryColor);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.But_imageMosaique);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.TB_iteration);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.TB_width);
            this.Controls.Add(this.TB_height);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.LB_récursiff);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TB_Im);
            this.Controls.Add(this.TB_Re);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LB_equations);
            this.Controls.Add(this.Clear);
            this.Controls.Add(this.SaveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FractaleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FRACTALES";
            this.Load += new System.EventHandler(this.FractaleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button Clear;
        private System.Windows.Forms.ListBox LB_equations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TB_Re;
        private System.Windows.Forms.TextBox TB_Im;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox LB_récursiff;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TB_height;
        private System.Windows.Forms.TextBox TB_width;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox TB_iteration;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button But_imageMosaique;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox TB_tryColor;
        private System.Windows.Forms.TextBox TB_contraste;
        private System.Windows.Forms.TextBox TB_mosaique;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private ToolTip toolTip1;
        private Label label19;
        private Button But_Color;
        private ColorDialog colorDialog1;
    }
}