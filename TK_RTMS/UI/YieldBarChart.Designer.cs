namespace TK_RTMS.UI
{
    partial class YieldBarChart
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
            this.uiTlb_Main = new System.Windows.Forms.TableLayoutPanel();
            this.uiPan_Title = new System.Windows.Forms.Panel();
            this.uiLab_ChartTitle = new System.Windows.Forms.Label();
            this.uiChart_Yield = new ChartFX.WinForms.Chart();
            this.uiChk_ShowValue = new System.Windows.Forms.CheckBox();
            this.uiTlb_Main.SuspendLayout();
            this.uiPan_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiChart_Yield)).BeginInit();
            this.SuspendLayout();
            // 
            // uiTlb_Main
            // 
            this.uiTlb_Main.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.uiTlb_Main.ColumnCount = 1;
            this.uiTlb_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTlb_Main.Controls.Add(this.uiPan_Title, 0, 0);
            this.uiTlb_Main.Controls.Add(this.uiChart_Yield, 0, 1);
            this.uiTlb_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTlb_Main.Location = new System.Drawing.Point(0, 0);
            this.uiTlb_Main.Name = "uiTlb_Main";
            this.uiTlb_Main.RowCount = 2;
            this.uiTlb_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.uiTlb_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTlb_Main.Size = new System.Drawing.Size(527, 276);
            this.uiTlb_Main.TabIndex = 2;
            // 
            // uiPan_Title
            // 
            this.uiPan_Title.BackColor = System.Drawing.Color.DarkTurquoise;
            this.uiPan_Title.Controls.Add(this.uiChk_ShowValue);
            this.uiPan_Title.Controls.Add(this.uiLab_ChartTitle);
            this.uiPan_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPan_Title.Location = new System.Drawing.Point(5, 5);
            this.uiPan_Title.Name = "uiPan_Title";
            this.uiPan_Title.Size = new System.Drawing.Size(517, 34);
            this.uiPan_Title.TabIndex = 0;
            // 
            // uiLab_ChartTitle
            // 
            this.uiLab_ChartTitle.AutoSize = true;
            this.uiLab_ChartTitle.BackColor = System.Drawing.Color.Transparent;
            this.uiLab_ChartTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiLab_ChartTitle.ForeColor = System.Drawing.Color.Black;
            this.uiLab_ChartTitle.Location = new System.Drawing.Point(3, 10);
            this.uiLab_ChartTitle.Name = "uiLab_ChartTitle";
            this.uiLab_ChartTitle.Size = new System.Drawing.Size(56, 15);
            this.uiLab_ChartTitle.TabIndex = 3;
            this.uiLab_ChartTitle.Text = "No Data";
            // 
            // uiChart_Yield
            // 
            this.uiChart_Yield.AllSeries.Gallery = ChartFX.WinForms.Gallery.Bar;
            this.uiChart_Yield.AxisX.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Number;
            this.uiChart_Yield.AxisX.Title.Text = "X-Axis";
            this.uiChart_Yield.AxisY.LabelsFormat.Decimals = 2;
            this.uiChart_Yield.AxisY.Title.Text = "Y Axis";
            this.uiChart_Yield.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            solidBackground1.AssemblyName = "ChartFX.WinForms.Adornments";
            this.uiChart_Yield.Background = solidBackground1;
            this.uiChart_Yield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiChart_Yield.Location = new System.Drawing.Point(5, 47);
            this.uiChart_Yield.Name = "uiChart_Yield";
            this.uiChart_Yield.Palette = "Schemes2.Autumn";
            this.uiChart_Yield.PlotAreaColor = System.Drawing.Color.Transparent;
            this.uiChart_Yield.Size = new System.Drawing.Size(517, 224);
            this.uiChart_Yield.TabIndex = 0;
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
            this.uiChk_ShowValue.Location = new System.Drawing.Point(423, 9);
            this.uiChk_ShowValue.Name = "uiChk_ShowValue";
            this.uiChk_ShowValue.Size = new System.Drawing.Size(91, 19);
            this.uiChk_ShowValue.TabIndex = 6;
            this.uiChk_ShowValue.Text = "Point Value";
            this.uiChk_ShowValue.UseVisualStyleBackColor = false;
            // 
            // YieldBarChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiTlb_Main);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "YieldBarChart";
            this.Size = new System.Drawing.Size(527, 276);
            this.uiTlb_Main.ResumeLayout(false);
            this.uiTlb_Main.PerformLayout();
            this.uiPan_Title.ResumeLayout(false);
            this.uiPan_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiChart_Yield)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel uiTlb_Main;
        private System.Windows.Forms.Panel uiPan_Title;
        private ChartFX.WinForms.Chart uiChart_Yield;
        private System.Windows.Forms.Label uiLab_ChartTitle;
        private System.Windows.Forms.CheckBox uiChk_ShowValue;
    }
}
