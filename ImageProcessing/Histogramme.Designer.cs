namespace ImageProcessing
{
    partial class Histogramme
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
            this.rempliCheckBox = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.largeurTB = new System.Windows.Forms.TextBox();
            this.hauteurTB = new System.Windows.Forms.TextBox();
            this.GoBut = new System.Windows.Forms.Button();
            this.AnnulerBut = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // rempliCheckBox
            // 
            this.rempliCheckBox.AutoSize = true;
            this.rempliCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rempliCheckBox.Location = new System.Drawing.Point(183, 24);
            this.rempliCheckBox.Name = "rempliCheckBox";
            this.rempliCheckBox.Size = new System.Drawing.Size(116, 22);
            this.rempliCheckBox.TabIndex = 0;
            this.rempliCheckBox.Text = "Remplissage";
            this.toolTip1.SetToolTip(this.rempliCheckBox, "Rempli le dessous des (de la) courbe(s)");
            this.rempliCheckBox.UseVisualStyleBackColor = true;
            this.rempliCheckBox.CheckedChanged += new System.EventHandler(this.rempliCheckBox_CheckedChanged);
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Items.AddRange(new object[] {
            "Histogramme de la couleur grise",
            "Histogramme des couleurs RGB"});
            this.listBox1.Location = new System.Drawing.Point(12, 76);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(312, 84);
            this.listBox1.TabIndex = 1;
            this.toolTip1.SetToolTip(this.listBox1, "Type d\'histogramme");
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Largeur";
            this.toolTip1.SetToolTip(this.label1, "Largeur en pixel d el\'histogramme à réaliser");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hauteur";
            this.toolTip1.SetToolTip(this.label2, "Hauteur en pixel d el\'histogramme à réaliser");
            // 
            // largeurTB
            // 
            this.largeurTB.Location = new System.Drawing.Point(92, 9);
            this.largeurTB.Name = "largeurTB";
            this.largeurTB.Size = new System.Drawing.Size(67, 22);
            this.largeurTB.TabIndex = 4;
            this.largeurTB.TextChanged += new System.EventHandler(this.largeurTB_TextChanged);
            this.largeurTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.largeurTB_KeyPress);
            // 
            // hauteurTB
            // 
            this.hauteurTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hauteurTB.Location = new System.Drawing.Point(92, 40);
            this.hauteurTB.Name = "hauteurTB";
            this.hauteurTB.Size = new System.Drawing.Size(67, 24);
            this.hauteurTB.TabIndex = 5;
            this.hauteurTB.TextChanged += new System.EventHandler(this.hauteurTB_TextChanged);
            this.hauteurTB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.hauteurTB_KeyPress);
            // 
            // GoBut
            // 
            this.GoBut.BackColor = System.Drawing.Color.LimeGreen;
            this.GoBut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.GoBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GoBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GoBut.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.GoBut.Location = new System.Drawing.Point(162, 157);
            this.GoBut.Margin = new System.Windows.Forms.Padding(0);
            this.GoBut.Name = "GoBut";
            this.GoBut.Size = new System.Drawing.Size(162, 96);
            this.GoBut.TabIndex = 6;
            this.GoBut.Text = "Créer \r\nl\'histogramme";
            this.GoBut.UseVisualStyleBackColor = false;
            this.GoBut.Click += new System.EventHandler(this.GoBut_Click);
            // 
            // AnnulerBut
            // 
            this.AnnulerBut.BackColor = System.Drawing.Color.Red;
            this.AnnulerBut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AnnulerBut.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AnnulerBut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AnnulerBut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AnnulerBut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.AnnulerBut.Location = new System.Drawing.Point(3, 157);
            this.AnnulerBut.Name = "AnnulerBut";
            this.AnnulerBut.Size = new System.Drawing.Size(156, 96);
            this.AnnulerBut.TabIndex = 7;
            this.AnnulerBut.Text = "Annuler";
            this.AnnulerBut.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label4.Location = new System.Drawing.Point(-5, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(339, 12);
            this.label4.TabIndex = 37;
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.White;
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label3.Location = new System.Drawing.Point(154, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 108);
            this.label3.TabIndex = 38;
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label5.Location = new System.Drawing.Point(0, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 108);
            this.label5.TabIndex = 39;
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label6.Location = new System.Drawing.Point(324, 145);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 120);
            this.label6.TabIndex = 40;
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Histogramme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 267);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.AnnulerBut);
            this.Controls.Add(this.GoBut);
            this.Controls.Add(this.hauteurTB);
            this.Controls.Add(this.largeurTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.rempliCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Histogramme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Histogramme";
            this.Load += new System.EventHandler(this.Histogramme_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox rempliCheckBox;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox largeurTB;
        private System.Windows.Forms.TextBox hauteurTB;
        private System.Windows.Forms.Button GoBut;
        private System.Windows.Forms.Button AnnulerBut;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}