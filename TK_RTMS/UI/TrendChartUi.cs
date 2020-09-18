using ChartFX.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class TrendChartUi : UserControl
    {
        #region 변수

        public string RItem = string.Empty;
        public SearchOption opt = new SearchOption();

        public DataSet _totalDs = new DataSet();
        public DataSet _materialsDs = new DataSet();

        #endregion

        public TrendChartUi()
        {
            InitializeComponent();

            // 이벤트 선언 및 호출
            InitEvent();
        }

        /// <summary>
        /// 각종 이벤트 선언 및 호출
        /// </summary>
        public void InitEvent()
        {
            uiChk_ShowValue.CheckedChanged += (Object o, EventArgs e) =>
            {
                uiChart_Trend.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
            };

            uiChk_GapValue.CheckedChanged += (Object o, EventArgs e) =>
            {
                UpdateData();
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
            _totalDs = DB_Process.Instance.Get_TrendChartMainData(RItem, opt);
            UpdateChart(_totalDs);
        }

        /// <summary>
        /// Sub Data 조회 및 Chart 그리기
        /// </summary>
        /// <param name="materials"></param>
        public void UpdateSubData(string materials)
        {
            InitChart();
            _materialsDs = DB_Process.Instance.Get_TrendChartSubData(RItem, opt, materials);
            UpdateSubChart(_materialsDs, materials);
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
        /// 차트 설정
        /// </summary>
        private void InitChart()
        {
            //chart1.Reset();
            uiChart_Trend.Data.Clear();
            uiChart_Trend.LegendBox.Visible = false;

            uiChart_Trend.Data.Series = 2;
            uiChart_Trend.AxisX.AutoScale = true;
            uiChart_Trend.AxisX.AutoScroll = true;

            uiChart_Trend.AllSeries.Gallery = ChartFX.WinForms.Gallery.Lines;

            uiChart_Trend.Series[1].MarkerSize = 1;

            uiChart_Trend.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;

            //Y축 제목 설정
            uiChart_Trend.AxisY.Title.Text = "Range of result data";
            uiChart_Trend.AxisY.Title.TextColor = Color.Black;

            //X축 제목 설정
            uiChart_Trend.AxisX.Title.Text = "Result data";
            uiChart_Trend.AxisX.Title.TextColor = Color.Black;

        }

        /// <summary>
        /// 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateChart(DataSet ds)
        {
            uiChart_Trend.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 7 : 6;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                double stdVal = 0;

                nPoints = uiChart_Trend.Data.Points++;

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Trend.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }

                if (double.TryParse(ds.Tables[0].Rows[row][5].ToString(), out stdVal) == true)
                {
                    uiChart_Trend.Data[1, nPoints] = stdVal;
                }
            }

            uiChart_Trend.AxisY.Min = minY - 20;
            uiChart_Trend.AxisY.Max = maxY + 20;
            uiChart_Trend.AxisY.LabelsFormat.Decimals = 1;

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = "Trend Chart";
            uiChart_Trend.Titles.Add(td);
        }

        /// <summary>
        /// 서브 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSubChart(DataSet ds, string materials)
        {
            uiChart_Trend.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 7 : 6;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                double stdVal = 0;

                nPoints = uiChart_Trend.Data.Points++;

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Trend.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }

                if (double.TryParse(ds.Tables[0].Rows[row][5].ToString(), out stdVal) == true)
                {
                    uiChart_Trend.Data[1, nPoints] = stdVal;
                }
            }

            uiChart_Trend.AxisY.Min = minY - 20;
            uiChart_Trend.AxisY.Max = maxY + 20;
            uiChart_Trend.AxisY.LabelsFormat.Decimals = 1;

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{materials}' - Trend Chart";
            uiChart_Trend.Titles.Add(td);
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
