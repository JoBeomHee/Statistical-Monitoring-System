namespace TK_RTMS.UI
{
    partial class MainUi
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uiPanel_Title = new System.Windows.Forms.Panel();
            this.uiLab_Title = new System.Windows.Forms.Label();
            this.uiTpl_Main = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.uiPanel_Title.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.uiPanel_Title, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.uiTpl_Main, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(845, 709);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uiPanel_Title
            // 
            this.uiPanel_Title.Controls.Add(this.uiLab_Title);
            this.uiPanel_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiPanel_Title.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.uiPanel_Title.Location = new System.Drawing.Point(4, 5);
            this.uiPanel_Title.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uiPanel_Title.Name = "uiPanel_Title";
            this.uiPanel_Title.Size = new System.Drawing.Size(837, 22);
            this.uiPanel_Title.TabIndex = 0;
            // 
            // uiLab_Title
            // 
            this.uiLab_Title.BackColor = System.Drawing.Color.Transparent;
            this.uiLab_Title.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiLab_Title.Font = new System.Drawing.Font("Microsoft JhengHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiLab_Title.ForeColor = System.Drawing.Color.DarkBlue;
            this.uiLab_Title.Location = new System.Drawing.Point(0, 0);
            this.uiLab_Title.Name = "uiLab_Title";
            this.uiLab_Title.Size = new System.Drawing.Size(837, 22);
            this.uiLab_Title.TabIndex = 0;
            this.uiLab_Title.Text = "Title";
            this.uiLab_Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiTpl_Main
            // 
            this.uiTpl_Main.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.uiTpl_Main.ColumnCount = 2;
            this.uiTpl_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTpl_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTpl_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTpl_Main.Location = new System.Drawing.Point(1, 32);
            this.uiTpl_Main.Margin = new System.Windows.Forms.Padding(0);
            this.uiTpl_Main.Name = "uiTpl_Main";
            this.uiTpl_Main.RowCount = 2;
            this.uiTpl_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTpl_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.uiTpl_Main.Size = new System.Drawing.Size(843, 676);
            this.uiTpl_Main.TabIndex = 1;
            // 
            // MainUi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MainUi";
            this.Size = new System.Drawing.Size(845, 709);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.uiPanel_Title.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel uiPanel_Title;
        private System.Windows.Forms.Label uiLab_Title;
        private System.Windows.Forms.TableLayoutPanel uiTpl_Main;
    }
}
