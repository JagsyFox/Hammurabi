namespace Hammurabi
{
    partial class LoadingScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingScreen));
            this.btnNew = new System.Windows.Forms.Button();
            this.lstScores = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Copperplate Gothic Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(380, 136);
            this.btnNew.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(133, 49);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "Start New Game";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // lstScores
            // 
            this.lstScores.Font = new System.Drawing.Font("Copperplate Gothic Light", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstScores.FormattingEnabled = true;
            this.lstScores.ItemHeight = 11;
            this.lstScores.Location = new System.Drawing.Point(46, 28);
            this.lstScores.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lstScores.Name = "lstScores";
            this.lstScores.Size = new System.Drawing.Size(214, 257);
            this.lstScores.TabIndex = 1;
            // 
            // LoadingScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.lstScores);
            this.Controls.Add(this.btnNew);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "LoadingScreen";
            this.Text = "LoadingScreen";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.ListBox lstScores;
    }
}