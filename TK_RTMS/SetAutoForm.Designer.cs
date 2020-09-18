namespace TK_RTMS
{
    partial class SetAutoForm
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
            this.uiBtn_Start = new System.Windows.Forms.Button();
            this.uiBtn_Cancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.uiCkb_Yield = new System.Windows.Forms.CheckBox();
            this.uiCkb_Trend = new System.Windows.Forms.CheckBox();
            this.uiCkb_Histogram = new System.Windows.Forms.CheckBox();
            this.uiCkb_Pareto = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.uiNum_Interval = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.uiCcmb_Result = new TK_RTMS.Controls.CheckComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiNum_Interval)).BeginInit();
            this.SuspendLayout();
            // 
            // uiBtn_Start
            // 
            this.uiBtn_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiBtn_Start.ForeColor = System.Drawing.Color.Blue;
            this.uiBtn_Start.Location = new System.Drawing.Point(227, 133);
            this.uiBtn_Start.Name = "uiBtn_Start";
            this.uiBtn_Start.Size = new System.Drawing.Size(75, 23);
            this.uiBtn_Start.TabIndex = 5;
            this.uiBtn_Start.Text = "Start";
            this.uiBtn_Start.UseVisualStyleBackColor = true;
            // 
            // uiBtn_Cancel
            // 
            this.uiBtn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiBtn_Cancel.ForeColor = System.Drawing.Color.Red;
            this.uiBtn_Cancel.Location = new System.Drawing.Point(308, 133);
            this.uiBtn_Cancel.Name = "uiBtn_Cancel";
            this.uiBtn_Cancel.Size = new System.Drawing.Size(75, 23);
            this.uiBtn_Cancel.TabIndex = 4;
            this.uiBtn_Cancel.Text = "Cancel";
            this.uiBtn_Cancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uiCkb_Yield);
            this.groupBox1.Controls.Add(this.uiCkb_Trend);
            this.groupBox1.Controls.Add(this.uiCkb_Histogram);
            this.groupBox1.Controls.Add(this.uiCkb_Pareto);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.uiNum_Interval);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.uiCcmb_Result);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupBox1.Location = new System.Drawing.Point(21, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 112);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Auto View Setting";
            // 
            // uiCkb_Yield
            // 
            this.uiCkb_Yield.AutoSize = true;
            this.uiCkb_Yield.Location = new System.Drawing.Point(280, 57);
            this.uiCkb_Yield.Name = "uiCkb_Yield";
            this.uiCkb_Yield.Size = new System.Drawing.Size(52, 16);
            this.uiCkb_Yield.TabIndex = 9;
            this.uiCkb_Yield.Text = "Yield";
            this.uiCkb_Yield.UseVisualStyleBackColor = true;
            // 
            // uiCkb_Trend
            // 
            this.uiCkb_Trend.AutoSize = true;
            this.uiCkb_Trend.Location = new System.Drawing.Point(214, 57);
            this.uiCkb_Trend.Name = "uiCkb_Trend";
            this.uiCkb_Trend.Size = new System.Drawing.Size(57, 16);
            this.uiCkb_Trend.TabIndex = 8;
            this.uiCkb_Trend.Text = "Trend";
            this.uiCkb_Trend.UseVisualStyleBackColor = true;
            // 
            // uiCkb_Histogram
            // 
            this.uiCkb_Histogram.AutoSize = true;
            this.uiCkb_Histogram.Location = new System.Drawing.Point(131, 57);
            this.uiCkb_Histogram.Name = "uiCkb_Histogram";
            this.uiCkb_Histogram.Size = new System.Drawing.Size(81, 16);
            this.uiCkb_Histogram.TabIndex = 7;
            this.uiCkb_Histogram.Text = "Histogram";
            this.uiCkb_Histogram.UseVisualStyleBackColor = true;
            // 
            // uiCkb_Pareto
            // 
            this.uiCkb_Pareto.AutoSize = true;
            this.uiCkb_Pareto.Location = new System.Drawing.Point(68, 57);
            this.uiCkb_Pareto.Name = "uiCkb_Pareto";
            this.uiCkb_Pareto.Size = new System.Drawing.Size(60, 16);
            this.uiCkb_Pareto.TabIndex = 6;
            this.uiCkb_Pareto.Text = "Pareto";
            this.uiCkb_Pareto.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(128, 86);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "sec";
            // 
            // uiNum_Interval
            // 
            this.uiNum_Interval.Location = new System.Drawing.Point(67, 81);
            this.uiNum_Interval.Name = "uiNum_Interval";
            this.uiNum_Interval.Size = new System.Drawing.Size(57, 21);
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
            this.label3.Location = new System.Drawing.Point(9, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Interval :";
            // 
            // uiCcmb_Result
            // 
            this.uiCcmb_Result.BackColor = System.Drawing.Color.SkyBlue;
            this.uiCcmb_Result.DropDownHeight = 1;
            this.uiCcmb_Result.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uiCcmb_Result.DropDownWidth = 1;
            this.uiCcmb_Result.EventArgs_Checked = null;
            this.uiCcmb_Result.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.uiCcmb_Result.FormattingEnabled = true;
            this.uiCcmb_Result.IntegralHeight = false;
            this.uiCcmb_Result.Location = new System.Drawing.Point(67, 25);
            this.uiCcmb_Result.Name = "uiCcmb_Result";
            this.uiCcmb_Result.Size = new System.Drawing.Size(266, 20);
            this.uiCcmb_Result.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "UI :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Result :";
            // 
            // SetAutoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(406, 166);
            this.Controls.Add(this.uiBtn_Start);
            this.Controls.Add(this.uiBtn_Cancel);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetAutoForm";
            this.Text = "SetAutoForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiNum_Interval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button uiBtn_Start;
        private System.Windows.Forms.Button uiBtn_Cancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown uiNum_Interval;
        private System.Windows.Forms.Label label3;
        private TK_RTMS.Controls.CheckComboBox uiCcmb_Result;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox uiCkb_Yield;
        private System.Windows.Forms.CheckBox uiCkb_Trend;
        private System.Windows.Forms.CheckBox uiCkb_Histogram;
        private System.Windows.Forms.CheckBox uiCkb_Pareto;
    }
}