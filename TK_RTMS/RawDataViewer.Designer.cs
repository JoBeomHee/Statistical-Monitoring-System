namespace TK_RTMS
{
    partial class RawDataViwer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RawDataViwer));
            this.uiPan_Main = new System.Windows.Forms.Panel();
            this.uiTab_Main = new System.Windows.Forms.TabControl();
            this.uiPan_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiPan_Main
            // 
            this.uiPan_Main.Controls.Add(this.uiTab_Main);
            this.uiPan_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPan_Main.Location = new System.Drawing.Point(0, 0);
            this.uiPan_Main.Name = "uiPan_Main";
            this.uiPan_Main.Size = new System.Drawing.Size(1434, 586);
            this.uiPan_Main.TabIndex = 0;
            // 
            // uiTab_Main
            // 
            this.uiTab_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab_Main.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiTab_Main.Location = new System.Drawing.Point(0, 0);
            this.uiTab_Main.Name = "uiTab_Main";
            this.uiTab_Main.SelectedIndex = 0;
            this.uiTab_Main.Size = new System.Drawing.Size(1434, 586);
            this.uiTab_Main.TabIndex = 0;
            // 
            // RawDataViwer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1434, 586);
            this.Controls.Add(this.uiPan_Main);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RawDataViwer";
            this.uiPan_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel uiPan_Main;
        private System.Windows.Forms.TabControl uiTab_Main;
    }
}