namespace TK_RTMS.UI
{
    partial class HistogramUi
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            ChartFX.WinForms.Adornments.SolidBackground solidBackground1 = new ChartFX.WinForms.Adornments.SolidBackground();
            this.uiTlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.uiPan_Title = new System.Windows.Forms.Panel();
            this.uiChk_GapValue = new System.Windows.Forms.CheckBox();
            this.uiChk_ShowValue = new System.Windows.Forms.CheckBox();
            this.uiChart_Histogram = new ChartFX.WinForms.Chart();
            this.uiTlp_Main.SuspendLayout();
            this.uiPan_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiChart_Histogram)).BeginInit();
            this.SuspendLayout();
            // 
            // uiTlp_Main
            // 
            this.uiTlp_Main.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.OutsetPartial;
            this.uiTlp_Main.ColumnCount = 1;
            this.uiTlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTlp_Main.Controls.Add(this.uiPan_Title, 0, 0);
            this.uiTlp_Main.Controls.Add(this.uiChart_Histogram, 0, 1);
            this.uiTlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTlp_Main.Location = new System.Drawing.Point(0, 0);
            this.uiTlp_Main.Name = "uiTlp_Main";
            this.uiTlp_Main.RowCount = 2;
            this.uiTlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.uiTlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTlp_Main.Size = new System.Drawing.Size(630, 289);
            this.uiTlp_Main.TabIndex = 1;
            // 
            // uiPan_Title
            // 
            this.uiPan_Title.BackColor = System.Drawing.Color.DarkTurquoise;
            this.uiPan_Title.Controls.Add(this.uiChk_GapValue);
            this.uiPan_Title.Controls.Add(this.uiChk_ShowValue);
            this.uiPan_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPan_Title.Location = new System.Drawing.Point(6, 6);
            this.uiPan_Title.Name = "uiPan_Title";
            this.uiPan_Title.Size = new System.Drawing.Size(618, 34);
            this.uiPan_Title.TabIndex = 0;
            // 
            // uiChk_GapValue
            // 
            this.uiChk_GapValue.AutoSize = true;
            this.uiChk_GapValue.BackColor = System.Drawing.Color.Transparent;
            this.uiChk_GapValue.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiChk_GapValue.ForeColor = System.Drawing.Color.Black;
            this.uiChk_GapValue.Location = new System.Drawing.Point(3, 7);
            this.uiChk_GapValue.Name = "uiChk_GapValue";
            this.uiChk_GapValue.Size = new System.Drawing.Size(86, 19);
            this.uiChk_GapValue.TabIndex = 0;
            this.uiChk_GapValue.Text = "Gap Value";
            this.uiChk_GapValue.UseVisualStyleBackColor = false;
            // 
            // uiChk_ShowValue
            // 
            this.uiChk_ShowValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiChk_ShowValue.AutoSize = true;
            this.uiChk_ShowValue.BackColor = System.Drawing.Color.Transparent;
            this.uiChk_ShowValue.Checked = true;
            this.uiChk_ShowValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uiChk_ShowValue.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiChk_ShowValue.ForeColor = System.Drawing.Color.White;
            this.uiChk_ShowValue.Location = new System.Drawing.Point(524, 7);
            this.uiChk_ShowValue.Name = "uiChk_ShowValue";
            this.uiChk_ShowValue.Size = new System.Drawing.Size(91, 19);
            this.uiChk_ShowValue.TabIndex = 0;
            this.uiChk_ShowValue.Text = "Point Value";
            this.uiChk_ShowValue.UseVisualStyleBackColor = false;
            // 
            // uiChart_Histogram
            // 
            this.uiChart_Histogram.AxisX.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Number;
            this.uiChart_Histogram.AxisX.Title.Text = "X-Axis";
            this.uiChart_Histogram.AxisY.LabelsFormat.Decimals = 2;
            this.uiChart_Histogram.AxisY.Title.Text = "Y Axis";
            this.uiChart_Histogram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            solidBackground1.AssemblyName = "ChartFX.WinForms.Adornments";
            this.uiChart_Histogram.Background = solidBackground1;
            this.uiChart_Histogram.Border = new ChartFX.WinForms.Adornments.SimpleBorder(ChartFX.WinForms.Adornments.SimpleBorderType.Color, System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(133))))));
            this.uiChart_Histogram.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiChart_Histogram.Location = new System.Drawing.Point(6, 49);
            this.uiChart_Histogram.Name = "uiChart_Histogram";
            this.uiChart_Histogram.Palette = "ChartFX6.ModernBusiness";
            this.uiChart_Histogram.PlotAreaColor = System.Drawing.Color.Transparent;
            this.uiChart_Histogram.Size = new System.Drawing.Size(618, 234);
            this.uiChart_Histogram.TabIndex = 0;
            // 
            // HistogramUi
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiTlp_Main);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "HistogramUi";
            this.Size = new System.Drawing.Size(630, 289);
            this.uiTlp_Main.ResumeLayout(false);
            this.uiTlp_Main.PerformLayout();
            this.uiPan_Title.ResumeLayout(false);
            this.uiPan_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiChart_Histogram)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel uiTlp_Main;
        private System.Windows.Forms.Panel uiPan_Title;
        private System.Windows.Forms.CheckBox uiChk_ShowValue;
        private System.Windows.Forms.CheckBox uiChk_GapValue;
        private ChartFX.WinForms.Chart uiChart_Histogram;
    }
}
