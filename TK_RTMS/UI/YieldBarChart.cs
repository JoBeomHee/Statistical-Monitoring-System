using ChartFX.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class YieldBarChart : UserControl
    {
        #region 변수

        private enum MAIN_COLUMNS { MATERIAS, OK_TOTAL_CNT, NG_TOTAL_CNT, OK_CNT, NG_CNT, YIELD }

        public string RItem = string.Empty;
        public SearchOption opt = new SearchOption();

        public DataSet _totalDs = new DataSet();
        public DataSet _materialsDs = new DataSet();
        public DataSet _openingDs = new DataSet();

        private string backMaterials = string.Empty;

        #endregion

        public YieldBarChart()
        {
            InitializeComponent();

            //이벤트 선언 및 호출
            InitEvent();
        }

        /// <summary>
        /// 각종 이벤트 선언 및 호출
        /// </summary>
        public void InitEvent()
        {
            uiChk_ShowValue.CheckedChanged += (Object o, EventArgs e) =>
            {
                uiChart_Yield.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
            };

            //패널 그라데이션 이벤트 선언
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Gradient);
            this.uiPan_Title.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Gradient);
        }

        /// <summary>
        /// Main Data 조회 및 Chart 그리기
        /// </summary>
        public void UpdateData()
        {
            InitChart();
            _totalDs = DB_Process.Instance.Get_YieldMainData(RItem, opt);
            _totalDs = DataTableValueChanged(_totalDs);

            UpdateChart(_totalDs);
        }

        /// <summary>
        /// Materials 컬럼 값들에서 맨 뒤의 소수점 숫자 제거 메서드
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public DataSet DataTableValueChanged(DataSet ds)
        {
            DataSet convertDs = ds;

            //DataTable 특정 컬럼 값 변경하기
            foreach (DataRow dr in convertDs.Tables[0].Rows)
            {
                if (dr["MATERIALS"].ToString().Contains("."))
                {
                    dr["MATERIALS"] = dr["MATERIALS"].ToString().Substring(0, dr["MATERIALS"].ToString().LastIndexOf('.') - 1);
                }
            }

            return convertDs;
        }

        /// <summary>
        /// Sub Data 조회 및 Chart 그리기
        /// </summary>
        /// <param name="opening"></param>
        public void UpdateSubData(string materials)
        {
            InitChart();
            _materialsDs = DB_Process.Instance.Get_YieldSubData(RItem, opt, materials);

            UpdateSubChart(_materialsDs, materials);

            backMaterials = materials;
        }

        /// <summary>
        /// Opening Data 조회 및 Chart 그리기
        /// </summary>
        /// <param name="opening"></param>
        public void UpdateOpeningData(string opening, string materials)
        {
            InitChart();
            _openingDs = DB_Process.Instance.Get_YieldOpeningData(RItem, opt, opening, materials);

            UpdateOpeningChart(_openingDs, opening, materials);
        }

        /// <summary>
        /// Back Data 조회 및 Chart 그리기
        /// </summary>
        public void UpdateBackData()
        {
            InitChart();
            UpdateChart(_totalDs);
        }

        /// <summary>
        /// Materials Back Data 조회 및 Chart 그리기
        /// </summary>
        public void UpdateMaterialsBackData()
        {
            InitChart();
            UpdateSubChart(_materialsDs, backMaterials);
        }

        /// <summary>
        /// 차트 설정
        /// </summary>
        private void InitChart()
        {
            //chart1.Reset();
            uiChart_Yield.Data.Clear();
            uiChart_Yield.LegendBox.Visible = false;

            //chart1.View3D.Enabled = true;
            //chart1.View3D.Cluster = true;

            uiChart_Yield.AxisX.LabelAngle = Convert.ToInt16(opt.ANGLE.Replace("º", "")); 

            uiChart_Yield.Data.Series = 1;
            uiChart_Yield.AxisX.AutoScale = true;
            uiChart_Yield.AxisX.AutoScroll = false;

            //chart1.AxisY2.Visible = true;

            uiChart_Yield.AllSeries.Gallery = ChartFX.WinForms.Gallery.Bar;
            uiChart_Yield.Series[0].Color = Color.FromArgb(0, 103, 163);

            //TextLabel "%" 문구 붙이기
            uiChart_Yield.AllSeries.PointLabels.Format = "%T%%";
            uiChart_Yield.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;

            //Y축 제목 설정
            uiChart_Yield.AxisY.Title.Text = "Quantity rate by materials";
            uiChart_Yield.AxisY.Title.TextColor = Color.Black;

            //X축 제목 설정
            uiChart_Yield.AxisX.Title.Text = "Materials";
            uiChart_Yield.AxisX.Title.TextColor = Color.Black;
        }

        /// <summary>
        /// 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateChart(DataSet ds)
        {
            uiChart_Yield.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double totatCnt = 0;
            double okTotalCnt = 0;
            double ngTotalCnt = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = (int)MAIN_COLUMNS.YIELD;

            //Total 양품률, 불량률 구하기
            okTotalCnt = Convert.ToInt32(ds.Tables[0].Rows[0][(int)MAIN_COLUMNS.OK_TOTAL_CNT].ToString());
            ngTotalCnt = Convert.ToInt32(ds.Tables[0].Rows[0][(int)MAIN_COLUMNS.NG_TOTAL_CNT].ToString());

            //Total Count 계산
            totatCnt = okTotalCnt + ngTotalCnt;

            nPoints = uiChart_Yield.Data.Points++;
            uiChart_Yield.AxesX[0].Labels[nPoints] = "TOTAL";

            if(opt.JUDGE.Contains("OK"))
            {
                uiChart_Yield.Data[0, nPoints] = Math.Round((okTotalCnt / totatCnt) * 100);
            }
            else
            {
                uiChart_Yield.Data[0, nPoints] = Math.Round((ngTotalCnt / totatCnt) * 100);
            }
            
            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;

                nPoints = uiChart_Yield.Data.Points++;

                uiChart_Yield.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][(int)MAIN_COLUMNS.MATERIAS].ToString();

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                { 
                    uiChart_Yield.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }
            }

            uiChart_Yield.AxisY.Min = 0;
            uiChart_Yield.AxisY.Max = 100;
            uiChart_Yield.AxisY.Step = 10;

            uiChart_Yield.AxisY.LabelsFormat.Decimals = 0;

            uiLab_ChartTitle.Text = string.Format("Yield Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 12, FontStyle.Bold);

            if(opt.JUDGE.Contains("OK"))
            {
                uiChart_Yield.Series[0].Color = System.Drawing.ColorTranslator.FromHtml("#C8C8FF");
                td.Text = "OK - Yield Chart";
                td.TextColor = Color.DarkBlue;
            }
            else
            {
                uiChart_Yield.Series[0].Color = System.Drawing.ColorTranslator.FromHtml("#FF607F");
                td.Text = "NG - Yield Chart";
                td.TextColor = Color.Red;
            }
            uiChart_Yield.Titles.Add(td);
        }

        /// <summary>
        /// Sub 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSubChart(DataSet ds, string materials)
        {
            uiChart_Yield.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double totatCnt = 0;
            double okTotalCnt = 0;
            double ngTotalCnt = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = (int)MAIN_COLUMNS.YIELD;

            //Total 양품률, 불량률 구하기
            okTotalCnt = Convert.ToInt32(ds.Tables[0].Rows[0][(int)MAIN_COLUMNS.OK_TOTAL_CNT].ToString());
            ngTotalCnt = Convert.ToInt32(ds.Tables[0].Rows[0][(int)MAIN_COLUMNS.NG_TOTAL_CNT].ToString());

            //Total Count 계산
            totatCnt = okTotalCnt + ngTotalCnt;

            nPoints = uiChart_Yield.Data.Points++;
            uiChart_Yield.AxesX[0].Labels[nPoints] = "TOTAL";

            if (opt.JUDGE.Contains("OK"))
            {
                uiChart_Yield.Data[0, nPoints] = Math.Round((okTotalCnt / totatCnt) * 100);
            }
            else
            {
                uiChart_Yield.Data[0, nPoints] = Math.Round((ngTotalCnt / totatCnt) * 100);
            }

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                //double stdVal = 0;

                nPoints = uiChart_Yield.Data.Points++;

                uiChart_Yield.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][(int)MAIN_COLUMNS.MATERIAS].ToString();

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Yield.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }
            }

            uiChart_Yield.AxisY.Min = 0;
            uiChart_Yield.AxisY.Max = 100;
            uiChart_Yield.AxisY.Step = 10;

            uiChart_Yield.AxisY.LabelsFormat.Decimals = 0;

            uiLab_ChartTitle.Text = string.Format("Yield Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);

            if (opt.JUDGE.Contains("OK"))
            {
                uiChart_Yield.Series[0].Color = System.Drawing.ColorTranslator.FromHtml("#C8C8FF");
                td.Text = $"'{materials}' - OK Yield Chart";
                td.TextColor = Color.DarkBlue;
            }
            else
            {
                uiChart_Yield.Series[0].Color = System.Drawing.ColorTranslator.FromHtml("#FF607F");
                td.Text = $"'{materials}' - NG Yield Chart";
                td.TextColor = Color.Red;
            }
            uiChart_Yield.Titles.Add(td);
        }

        /// <summary>
        /// Opening 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateOpeningChart(DataSet ds, string opening, string materials)
        {
            uiChart_Yield.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double totatCnt = 0;
            double okTotalCnt = 0;
            double ngTotalCnt = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = (int)MAIN_COLUMNS.YIELD;

            //Total 양품률, 불량률 구하기
            okTotalCnt = Convert.ToInt32(ds.Tables[0].Rows[0][(int)MAIN_COLUMNS.OK_TOTAL_CNT].ToString());
            ngTotalCnt = Convert.ToInt32(ds.Tables[0].Rows[0][(int)MAIN_COLUMNS.NG_TOTAL_CNT].ToString());

            //Total Count 계산
            totatCnt = okTotalCnt + ngTotalCnt;

            nPoints = uiChart_Yield.Data.Points++;
            uiChart_Yield.AxesX[0].Labels[nPoints] = "TOTAL";

            if (opt.JUDGE.Contains("OK"))
            {
                uiChart_Yield.Data[0, nPoints] = Math.Round((okTotalCnt / totatCnt) * 100);
            }
            else
            {
                uiChart_Yield.Data[0, nPoints] = Math.Round((ngTotalCnt / totatCnt) * 100);
            }

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                //double stdVal = 0;

                nPoints = uiChart_Yield.Data.Points++;

                uiChart_Yield.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][(int)MAIN_COLUMNS.MATERIAS].ToString();

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Yield.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }
            }

            uiChart_Yield.AxisY.Min = 0;
            uiChart_Yield.AxisY.Max = 100;
            uiChart_Yield.AxisY.Step = 10;

            uiChart_Yield.AxisY.LabelsFormat.Decimals = 0;

            uiLab_ChartTitle.Text = string.Format("Yield Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);

            if (opt.JUDGE.Contains("OK"))
            {
                uiChart_Yield.Series[0].Color = System.Drawing.ColorTranslator.FromHtml("#C8C8FF");
                td.Text = $"'{materials}'_'{opening}' - OK Yield Chart";
                td.TextColor = Color.DarkBlue;
            }
            else
            {
                uiChart_Yield.Series[0].Color = System.Drawing.ColorTranslator.FromHtml("#FF607F");
                td.Text = $"'{materials}'_'{opening}' - NG Yield Chart";
                td.TextColor = Color.Red;
            }
            uiChart_Yield.Titles.Add(td);
        }

        /// <summary>
        /// Panel 컨트롤 그라데이션 효과
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Form_Gradient(object sender, PaintEventArgs e)
        {
            LinearGradientBrush br = new LinearGradientBrush(this.ClientRectangle,
                                                             Color.DarkTurquoise,
                                                             Color.DarkTurquoise,
                                                             0,
                                                             false);
            e.Graphics.FillRectangle(br, this.ClientRectangle);
        }

        /// <summary>
        /// Panel 컨트롤 그라데이션 효과
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Panel_Gradient(object sender, PaintEventArgs e)
        {
            Color startColor = System.Drawing.ColorTranslator.FromHtml("#0acffe");
            Color middleColor = System.Drawing.ColorTranslator.FromHtml("#495aff");
            Color endColor = Color.FromArgb(0, 0, 0);

            LinearGradientBrush br = new LinearGradientBrush(this.ClientRectangle,
                                                 System.Drawing.Color.Black,
                                                 System.Drawing.Color.Black,
                                                 0,
                                                 false);

            ColorBlend cb = new ColorBlend();
            cb.Positions = new[] { 0, 1 / 2f, 1 };
            cb.Colors = new[] { startColor, middleColor, endColor };

            br.InterpolationColors = cb;
            br.RotateTransform(45);
            e.Graphics.FillRectangle(br, this.ClientRectangle);
        }
    }
}
