namespace TK_RTMS.UI
{
    partial class XBarChart
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
            this.uiChk_GapValue = new System.Windows.Forms.CheckBox();
            this.uiChk_ShowValue = new System.Windows.Forms.CheckBox();
            this.uiChart_XBar = new ChartFX.WinForms.Chart();
            this.uiTlb_Main.SuspendLayout();
            this.uiPan_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiChart_XBar)).BeginInit();
            this.SuspendLayout();
            // 
            // uiTlb_Main
            // 
            this.uiTlb_Main.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.uiTlb_Main.ColumnCount = 1;
            this.uiTlb_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTlb_Main.Controls.Add(this.uiPan_Title, 0, 0);
            this.uiTlb_Main.Controls.Add(this.uiChart_XBar, 0, 1);
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
            this.uiPan_Title.Controls.Add(this.uiChk_GapValue);
            this.uiPan_Title.Controls.Add(this.uiChk_ShowValue);
            this.uiPan_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPan_Title.Location = new System.Drawing.Point(5, 5);
            this.uiPan_Title.Name = "uiPan_Title";
            this.uiPan_Title.Size = new System.Drawing.Size(517, 34);
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
            this.uiChk_ShowValue.Location = new System.Drawing.Point(423, 7);
            this.uiChk_ShowValue.Name = "uiChk_ShowValue";
            this.uiChk_ShowValue.Size = new System.Drawing.Size(91, 19);
            this.uiChk_ShowValue.TabIndex = 0;
            this.uiChk_ShowValue.Text = "Point Value";
            this.uiChk_ShowValue.UseVisualStyleBackColor = false;
            // 
            // uiChart_XBar
            // 
            this.uiChart_XBar.AxisX.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Number;
            this.uiChart_XBar.AxisX.Title.Text = "X-Axis";
            this.uiChart_XBar.AxisY.LabelsFormat.Decimals = 2;
            this.uiChart_XBar.AxisY.Title.Text = "Y Axis";
            this.uiChart_XBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            solidBackground1.AssemblyName = "ChartFX.WinForms.Adornments";
            this.uiChart_XBar.Background = solidBackground1;
            this.uiChart_XBar.Border = new ChartFX.WinForms.Adornments.SimpleBorder(ChartFX.WinForms.Adornments.SimpleBorderType.Color, System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(233)))), ((int)(((byte)(216))))));
            this.uiChart_XBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiChart_XBar.Location = new System.Drawing.Point(5, 47);
            this.uiChart_XBar.Name = "uiChart_XBar";
            this.uiChart_XBar.Palette = "Schemes2.Autumn";
            this.uiChart_XBar.PlotAreaColor = System.Drawing.Color.Transparent;
            this.uiChart_XBar.Size = new System.Drawing.Size(517, 224);
            this.uiChart_XBar.TabIndex = 0;
            // 
            // XBarChart
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiTlb_Main);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Name = "XBarChart";
            this.Size = new System.Drawing.Size(527, 276);
            this.uiTlb_Main.ResumeLayout(false);
            this.uiTlb_Main.PerformLayout();
            this.uiPan_Title.ResumeLayout(false);
            this.uiPan_Title.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiChart_XBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel uiTlb_Main;
        private System.Windows.Forms.Panel uiPan_Title;
        private System.Windows.Forms.CheckBox uiChk_GapValue;
        private System.Windows.Forms.CheckBox uiChk_ShowValue;
        private ChartFX.WinForms.Chart uiChart_XBar;
    }
}
