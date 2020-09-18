namespace TK_RTMS
{
    partial class AutoSearchViewer
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
            this.uiGb_Setting = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.uiNum_Interval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.uiBtn_Cancel = new System.Windows.Forms.Button();
            this.uiBtn_Start = new System.Windows.Forms.Button();
            this.uiGb_Setting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiNum_Interval)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGb_Setting
            // 
            this.uiGb_Setting.Controls.Add(this.label4);
            this.uiGb_Setting.Controls.Add(this.uiNum_Interval);
            this.uiGb_Setting.Controls.Add(this.label3);
            this.uiGb_Setting.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiGb_Setting.Location = new System.Drawing.Point(18, 13);
            this.uiGb_Setting.Name = "uiGb_Setting";
            this.uiGb_Setting.Size = new System.Drawing.Size(168, 56);
            this.uiGb_Setting.TabIndex = 1;
            this.uiGb_Setting.TabStop = false;
            this.uiGb_Setting.Text = "Auto Search Setting";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(130, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "sec";
            // 
            // uiNum_Interval
            // 
            this.uiNum_Interval.Location = new System.Drawing.Point(69, 22);
            this.uiNum_Interval.Name = "uiNum_Interval";
            this.uiNum_Interval.Size = new System.Drawing.Size(57, 23);
            this.uiNum_Interval.TabIndex = 4;
            this.uiNum_Interval.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Interval :";
            // 
            // uiBtn_Cancel
            // 
            this.uiBtn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiBtn_Cancel.ForeColor = System.Drawing.Color.Red;
            this.uiBtn_Cancel.Location = new System.Drawing.Point(111, 75);
            this.uiBtn_Cancel.Name = "uiBtn_Cancel";
            this.uiBtn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.uiBtn_Cancel.TabIndex = 2;
            this.uiBtn_Cancel.Text = "Cancel";
            this.uiBtn_Cancel.UseVisualStyleBackColor = true;
            // 
            // uiBtn_Start
            // 
            this.uiBtn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiBtn_Start.ForeColor = System.Drawing.Color.Blue;
            this.uiBtn_Start.Location = new System.Drawing.Point(30, 75);
            this.uiBtn_Start.Name = "uiBtn_Start";
            this.uiBtn_Start.Size = new System.Drawing.Size(75, 23);
            this.uiBtn_Start.TabIndex = 2;
            this.uiBtn_Start.Text = "Start";
            this.uiBtn_Start.UseVisualStyleBackColor = true;
            // 
            // AutoSearchViewer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(205, 109);
            this.Controls.Add(this.uiBtn_Start);
            this.Controls.Add(this.uiBtn_Cancel);
            this.Controls.Add(this.uiGb_Setting);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AutoSearchViewer";
            this.Text = "SetAutoForm";
            this.uiGb_Setting.ResumeLayout(false);
            this.uiGb_Setting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiNum_Interval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox uiGb_Setting;
        private System.Windows.Forms.Button uiBtn_Cancel;
        private System.Windows.Forms.Button uiBtn_Start;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown uiNum_Interval;
        private System.Windows.Forms.Label label3;
    }
}