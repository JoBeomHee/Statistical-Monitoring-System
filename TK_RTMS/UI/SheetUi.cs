using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TK_RTMS.CommonCS;
using TK_RTMS.Controls;

namespace TK_RTMS.UI
{
    public partial class SheetUi : UserControl
    {
        #region 변수

        public enum COLUMNS { PLAN_DT, REG_DT, EQP_ID, JOB_NO, JOB_HG, MTR_CD, MTR_NM, DWG_NO, TOLERANCE, RESULT, GAP, INRANGE }

        public SearchOption opt = new SearchOption();

        public string RItem = string.Empty;

        public DataSet mainDs = new DataSet();

        #endregion

        public SheetUi()
        {
            InitializeComponent();

            this.uiPb_SaveExcel.Click += uiPb_SaveExcel_Click;
            this.uiPb_SaveExcel.MouseHover += UiPb_MouseHover_Event;
            this.uiPb_SaveExcel.MouseLeave += UiPb_MouseLeave_Event;
        }

        /// <summary>
        /// Sheet 데이터 조회 및 바인딩
        /// </summary>
        public void UpdateData()
        {
            string key = string.Empty;

            InitSheet();
            key = RItem.Replace("(", "_").Replace(")", ""); //Result Item을 Key 변수에 대입
            DataSet ds = DB_Process.Instance.Get_RawDataMainData(key, opt);
            mainDs = ds;
            UpdateSheet(ds);

            //Sheet Title 설정
            string title = $"Result Item : {key}";
            uiLb_Title.Text = title;
        }

        /// <summary>
        /// Sheet 설정
        /// </summary>
        private void InitSheet()
        {
            FarPoint.Win.Spread.SheetView sheet = fpSpread1.Sheets[0];

            sheet.SelectionUnit = FarPoint.Win.Spread.Model.SelectionUnit.Row;
            sheet.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;

            fpSpread1.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
            fpSpread1.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.Always;

            sheet.Columns.Count = System.Enum.GetValues(typeof(COLUMNS)).Length - 1;
            sheet.Rows.Count = 0;
            sheet.OperationMode = FarPoint.Win.Spread.OperationMode.Normal;
            sheet.ColumnHeader.Rows[0].Height = 35;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.PLAN_DT)].Value = "PLAN_DT";
            sheet.Columns[(int)(COLUMNS.PLAN_DT)].Width = 150;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.REG_DT)].Value = "REG_DT";
            sheet.Columns[(int)(COLUMNS.REG_DT)].Width = 150;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.EQP_ID)].Value = "EQP_ID";
            sheet.Columns[(int)(COLUMNS.EQP_ID)].Width = 100;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.JOB_NO)].Value = "JOB_NO";
            sheet.Columns[(int)(COLUMNS.JOB_NO)].Width = 110;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.JOB_HG)].Value = "JOB_HG";
            sheet.Columns[(int)(COLUMNS.JOB_HG)].Width = 60;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.MTR_CD)].Value = "MTR_CD";
            sheet.Columns[(int)(COLUMNS.MTR_CD)].Width = 120;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.MTR_NM)].Value = "MTR_NM";
            sheet.Columns[(int)(COLUMNS.MTR_NM)].Width = 250;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.DWG_NO)].Value = "DWG_NO";
            sheet.Columns[(int)(COLUMNS.DWG_NO)].Width = 110;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.TOLERANCE)].Value = "Tolerance";
            sheet.Columns[(int)(COLUMNS.TOLERANCE)].Width = 100;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.RESULT)].Value = "Result";
            sheet.Columns[(int)(COLUMNS.RESULT)].Width = 100;

            sheet.ColumnHeader.Cells[0, (int)(COLUMNS.GAP)].Value = "Gap";
            sheet.Columns[(int)(COLUMNS.GAP)].Width = 80;

            sheet.Columns[0, (int)(COLUMNS.GAP)].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            sheet.Columns[0, (int)(COLUMNS.GAP)].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
            sheet.Columns[0, (int)(COLUMNS.GAP)].Locked = true;
        }

        /// <summary>
        /// Sheet에 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSheet(DataSet ds)
        {
            FarPoint.Win.Spread.SheetView sheet = fpSpread1.Sheets[0];

            sheet.Rows.Count = 0;

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            sheet.Rows.Count = ds.Tables[0].Rows.Count;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                for (int col = 0; col < ds.Tables[0].Columns.Count - 1; ++col)
                {
                    sheet.Cells[row, col].Value = ds.Tables[0].Rows[row][col].ToString();
                }

                double a = 0;
                double b = 0;

                double.TryParse(ds.Tables[0].Rows[row][(int)(COLUMNS.GAP)].ToString(), out a);
                double.TryParse(ds.Tables[0].Rows[row][(int)(COLUMNS.INRANGE)].ToString(), out b);

                if (Math.Abs(a) > b)
                {
                    sheet.Cells[row, (int)(COLUMNS.GAP)].ForeColor = Color.Red;
                }
            }

            sheet.Rows[0, sheet.Rows.Count - 1].Height = 30;

            Common.dicSheet.Add(RItem, sheet);
        }

        /// <summary>
        /// 툴팁 이름 설정 메서드
        /// </summary>
        /// <param name="pb"></param>
        private void CreateTooltipName(PictureBox pb)
        {
            //ToolTip 객체 생성
            ToolTip tt = new ToolTip();
            tt.IsBalloon = true;

            //ToolTip 이름 배열로 선언
            string[] nameArr = { "SaveExcel"};
            string name = string.Empty;

            var result = nameArr.Where(s => pb.Name.Contains(s)).ToList();

            name = result[0].Replace("_", " ").ToString();
            tt.SetToolTip(pb, name);

        }
        /// <summary>
        /// PictureBox MouseHover 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_MouseHover_Event(object sender, EventArgs e)
        {
            //PictureBox 객체 정보 가져오기
            PictureBox pb = sender as PictureBox;

            //ToolTip 생성
            CreateTooltipName(pb);

            pb.BackColor = Color.Orange;
        }

        /// <summary>
        /// PictureBox MouseLeave Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_MouseLeave_Event(object sender, EventArgs e)
        {
            //PictureBox 객체 정보 가져오기
            PictureBox pb = sender as PictureBox;

            pb.BackColor = Color.FromArgb(2, 160, 234);
        }

        /// <summary>
        /// RawData 엑셀로 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uiPb_SaveExcel_Click(object sender, EventArgs e)
        {
            if (mainDs == null || mainDs.Tables.Count == 0)
                return;

            string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"BeomBeomJoJo_RawData_{date}.xls";
            string ext = ".xls";
            string filter = "Microsoft Excel Workbook (*.xls)|*.xls";

            using (SaveFileDialog saveFileDialog = GetExcelSaveFileDialog(fileName, ext, filter))
            {
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    FarPoint.Win.Spread.FpSpread spread = new FarPoint.Win.Spread.FpSpread();
                    List<string> sheetNames = new List<string>();

                    int row = 0;

                    foreach(KeyValuePair<string, FarPoint.Win.Spread.SheetView> items in Common.dicSheet)
                    {
                        spread.Sheets.Add(items.Value);
                        sheetNames.Add(items.Key);
                        spread.Sheets[row].SheetName = items.Key;
                        row++;
                    }

                    if (Function.ExportExcelFile(saveFileDialog.FileName, spread) == true)
                        JKMessageBox.ShowExcel(this, saveFileDialog.FileName);

                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 엑셀 파일 저장
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SaveFileDialog GetExcelSaveFileDialog(string filename, string ext, string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.ValidateNames = true;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            saveFileDialog.DefaultExt = ext;
            saveFileDialog.Filter = filter;
            saveFileDialog.FileName = filename;

            return saveFileDialog;
        }
    }
}
