namespace Multi_Filter
{
    partial class frmMain
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
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.gdvLines = new System.Windows.Forms.DataGridView();
            this.tvSearchs = new System.Windows.Forms.TreeView();
            this.lblNumberOfLines = new System.Windows.Forms.Label();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvLines)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuExit});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(859, 24);
            this.mnuMain.TabIndex = 0;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuFile
            // 
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "File";
            this.mnuFile.Click += new System.EventHandler(this.mnuFile_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(37, 20);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // gdvLines
            // 
            this.gdvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gdvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gdvLines.Location = new System.Drawing.Point(284, 44);
            this.gdvLines.Name = "gdvLines";
            this.gdvLines.Size = new System.Drawing.Size(563, 473);
            this.gdvLines.TabIndex = 1;
            // 
            // tvSearchs
            // 
            this.tvSearchs.Location = new System.Drawing.Point(12, 44);
            this.tvSearchs.Name = "tvSearchs";
            this.tvSearchs.Size = new System.Drawing.Size(266, 472);
            this.tvSearchs.TabIndex = 2;
            this.tvSearchs.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvSearchs_MouseDown);
            // 
            // lblNumberOfLines
            // 
            this.lblNumberOfLines.AutoSize = true;
            this.lblNumberOfLines.Location = new System.Drawing.Point(281, 28);
            this.lblNumberOfLines.Name = "lblNumberOfLines";
            this.lblNumberOfLines.Size = new System.Drawing.Size(0, 13);
            this.lblNumberOfLines.TabIndex = 3;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 529);
            this.Controls.Add(this.lblNumberOfLines);
            this.Controls.Add(this.tvSearchs);
            this.Controls.Add(this.gdvLines);
            this.Controls.Add(this.mnuMain);
            this.Icon = global::Multi_Filter.Properties.Resources.search;
            this.MainMenuStrip = this.mnuMain;
            this.Name = "frmMain";
            this.Text = "Multi Filter";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gdvLines)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.DataGridView gdvLines;
        private System.Windows.Forms.TreeView tvSearchs;
        private System.Windows.Forms.Label lblNumberOfLines;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
    }
}

