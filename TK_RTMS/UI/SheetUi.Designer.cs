namespace TK_RTMS.UI
{
    partial class SheetUi
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
            this.uiTlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.uiPanel_Title = new System.Windows.Forms.Panel();
            this.uiPb_SaveExcel = new System.Windows.Forms.PictureBox();
            this.uiLb_Title = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.fpSpread1 = new FarPoint.Win.Spread.FpSpread();
            this.fpSpread1_Sheet1 = new FarPoint.Win.Spread.SheetView();
            this.uiTlp_Main.SuspendLayout();
            this.uiPanel_Title.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPb_SaveExcel)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).BeginInit();
            this.SuspendLayout();
            // 
            // uiTlp_Main
            // 
            this.uiTlp_Main.ColumnCount = 1;
            this.uiTlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uiTlp_Main.Controls.Add(this.uiPanel_Title, 0, 0);
            this.uiTlp_Main.Controls.Add(this.panel2, 0, 1);
            this.uiTlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTlp_Main.Location = new System.Drawing.Point(0, 0);
            this.uiTlp_Main.Name = "uiTlp_Main";
            this.uiTlp_Main.RowCount = 2;
            this.uiTlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.uiTlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.uiTlp_Main.Size = new System.Drawing.Size(471, 550);
            this.uiTlp_Main.TabIndex = 0;
            // 
            // uiPanel_Title
            // 
            this.uiPanel_Title.Controls.Add(this.uiPb_SaveExcel);
            this.uiPanel_Title.Controls.Add(this.uiLb_Title);
            this.uiPanel_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel_Title.Location = new System.Drawing.Point(3, 3);
            this.uiPanel_Title.Name = "uiPanel_Title";
            this.uiPanel_Title.Size = new System.Drawing.Size(465, 38);
            this.uiPanel_Title.TabIndex = 0;
            // 
            // uiPb_SaveExcel
            // 
            this.uiPb_SaveExcel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiPb_SaveExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(160)))), ((int)(((byte)(234)))));
            this.uiPb_SaveExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.uiPb_SaveExcel.Image = global::TK_RTMS.Properties.Resources.Excel;
            this.uiPb_SaveExcel.Location = new System.Drawing.Point(415, 0);
            this.uiPb_SaveExcel.Name = "uiPb_SaveExcel";
            this.uiPb_SaveExcel.Size = new System.Drawing.Size(50, 38);
            this.uiPb_SaveExcel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.uiPb_SaveExcel.TabIndex = 1;
            this.uiPb_SaveExcel.TabStop = false;
            // 
            // uiLb_Title
            // 
            this.uiLb_Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(160)))), ((int)(((byte)(234)))));
            this.uiLb_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLb_Title.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiLb_Title.ForeColor = System.Drawing.Color.White;
            this.uiLb_Title.Location = new System.Drawing.Point(0, 0);
            this.uiLb_Title.Name = "uiLb_Title";
            this.uiLb_Title.Size = new System.Drawing.Size(465, 38);
            this.uiLb_Title.TabIndex = 0;
            this.uiLb_Title.Text = "Title";
            this.uiLb_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.fpSpread1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(465, 500);
            this.panel2.TabIndex = 1;
            // 
            // fpSpread1
            // 
            this.fpSpread1.AccessibleDescription = "";
            this.fpSpread1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fpSpread1.Location = new System.Drawing.Point(0, 0);
            this.fpSpread1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fpSpread1.Name = "fpSpread1";
            this.fpSpread1.Sheets.AddRange(new FarPoint.Win.Spread.SheetView[] {
            this.fpSpread1_Sheet1});
            this.fpSpread1.Size = new System.Drawing.Size(465, 500);
            this.fpSpread1.TabIndex = 1;
            // 
            // fpSpread1_Sheet1
            // 
            this.fpSpread1_Sheet1.Reset();
            fpSpread1_Sheet1.SheetName = "Sheet1";
            // 
            // SheetUi
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.uiTlp_Main);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SheetUi";
            this.Size = new System.Drawing.Size(471, 550);
            this.uiTlp_Main.ResumeLayout(false);
            this.uiPanel_Title.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPb_SaveExcel)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fpSpread1_Sheet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel uiTlp_Main;
        private System.Windows.Forms.Panel uiPanel_Title;
        private System.Windows.Forms.Panel panel2;
        private FarPoint.Win.Spread.FpSpread fpSpread1;
        private FarPoint.Win.Spread.SheetView fpSpread1_Sheet1;
        private System.Windows.Forms.Label uiLb_Title;
        private System.Windows.Forms.PictureBox uiPb_SaveExcel;
    }
}
