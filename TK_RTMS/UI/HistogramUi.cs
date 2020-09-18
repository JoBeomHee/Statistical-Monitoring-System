using ChartFX.WinForms;
using ChartFX.WinForms.Statistical;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class HistogramUi : UserControl
    {
        public string RItem = string.Empty;
        public SearchOption opt = new SearchOption();

        public DataSet _totalDs = new DataSet();
        public DataSet _materialsDs = new DataSet();
        public DataSet _openingDs = new DataSet();

        private string backMaterials = string.Empty;

        public HistogramUi()
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
                uiChart_Histogram.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
            };

            uiChk_GapValue.CheckedChanged += (Object o, EventArgs e) =>
            {
                UpdateData();
            };

            //Panel 그라데이션 이벤트 선언
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Gradient);
            this.uiPan_Title.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Gradient);
        }

        /// <summary>
        /// Main Data 차트 조회 및 그리기
        /// </summary>
        public void UpdateData()
        {
            InitChart();
            _totalDs = DB_Process.Instance.Get_HistogramMainData(RItem, opt);
            UpdateChart(_totalDs);
        }

        /// <summary>
        /// Sub Data 차트 조회 및 그리기
        /// </summary>
        /// <param name="materials"></param>
        public void UpdateSubData(string materials)
        {
            InitChart();
            _materialsDs = DB_Process.Instance.Get_HistogramSubData(RItem, opt,materials);
            UpdateSubChart(_materialsDs,materials);

            backMaterials = materials;
        }

        /// <summary>
        /// Opening Data 차트 조회 및 그리기
        /// </summary>
        /// <param name="materials"></param>
        public void UpdateOpeningData(string opening, string materials)
        {
            InitChart();
            _openingDs = DB_Process.Instance.Get_HistogramOpeningData(RItem, opt, opening, materials);
            UpdateOpeningChart(_openingDs, opening, materials);
        }

        /// <summary>
        /// Back Data 차트 조회 및 그리기
        /// </summary>
        public void UpdateBackData()
        {
            InitChart();
            UpdateChart(_totalDs);
        }

        /// <summary>
        /// Back Data 차트 조회 및 그리기
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
            uiChart_Histogram.Data.Clear();
            uiChart_Histogram.LegendBox.Visible = false;

            uiChart_Histogram.Data.Series = 1;
            uiChart_Histogram.AxisX.AutoScale = true;
            uiChart_Histogram.AxisX.AutoScroll = true;
            
            uiChart_Histogram.AllSeries.Gallery = ChartFX.WinForms.Gallery.Lines;

            uiChart_Histogram.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;

            //Y축 제목 설정
            uiChart_Histogram.AxisY.Title.Text = "Cumulative count";
            uiChart_Histogram.AxisY.Title.TextColor = Color.Black;

            //X축 제목 설정
            uiChart_Histogram.AxisX.Title.Text = "Range of result data";
            uiChart_Histogram.AxisX.Title.TextColor = Color.Black;

            uiChart_Histogram.AllSeries.Color = System.Drawing.ColorTranslator.FromHtml("#46D2D2");
        }

        /// <summary>
        /// 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateChart(DataSet ds)
        {
            uiChart_Histogram.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 6 : 5;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                nPoints = uiChart_Histogram.Data.Points++;

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Histogram.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);

                }
            }

            //Histogram        
            uiChart_Histogram.Extensions.Clear();
            ChartFX.WinForms.Statistical.Statistics st = new ChartFX.WinForms.Statistical.Statistics();
            st.Chart = uiChart_Histogram;
            uiChart_Histogram.GalleryAttributes = st.Gallery.Histogram;

            //chart1.DataGrid.TextColorData = Color.Blue;

            uiChart_Histogram.ToolTipFormat = "%L\nPoint Value : %v";

            //null 값 처리 되어 입력 된 값이 없을 경우...
            if (uiChart_Histogram.Data.Series == 0) return;

            double range = (maxY + 25) - (minY - 25);
            st.Gallery.Histogram.AxisX.Step = range / 10;

            //차트 막대바 두께 설정
            st.Chart.AllSeries.Volume = 90;

            //차트 X축 라벨 기울기 각도 설정
            st.Gallery.Histogram.AxisX.LabelAngle = 0;

            st.Gallery.Histogram.DataMax = (double)(maxY + 25);
            st.Gallery.Histogram.DataMin = (double)(minY - 25);
            st.Gallery.Histogram.AxisX.LabelsFormat.Decimals = 1;

            st.Studies.Clear();
            st.Studies.AddTitle("\n\n");
            st.Studies.Add(AnalysisMulti.SampleSize);
            st.Studies.Add(Analysis.LowerQuartile);
            st.Studies.Add(Analysis.UpperQuartile);
            st.Studies.Add(Analysis.Mean);
            st.Studies.Add(Analysis.Lsl);
            st.Studies.Add(Analysis.Usl);
            st.Studies.Add(Analysis.Cp);
            st.Studies.Add(Analysis.Cpk);

            for (int i = 0; i < st.Studies.Count; ++i)
            {
                st.Studies[i].Decimals = 2;
            }

            st.Studies[1].Text = "Total Size";
            st.Studies[2].Text = "25%(L)";
            st.Studies[3].Text = "25%(U)";

            st.Calculators[0].Lsl = minY;
            st.Calculators[0].Usl = maxY;

            st.LegendBox.Visible = true;
            st.LegendBox.Dock = ChartFX.WinForms.DockArea.Right;
            st.LegendBox.AutoSize = false;
            st.LegendBox.Width = 135;
            st.LegendBox.Height = 300;

            st.Gallery.Histogram.ShowLimits = true;

            uiChart_Histogram.Series[0].Text = "Width";

            st.LegendBox.Visible = true;

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = "Capability Analysis";
            uiChart_Histogram.Titles.Add(td);
        }

        /// <summary>
        /// 서브 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSubChart(DataSet ds, string materials)
        {
            uiChart_Histogram.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 6 : 5;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                nPoints = uiChart_Histogram.Data.Points++;

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Histogram.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);

                }
            }

            //Histogram        
            uiChart_Histogram.Extensions.Clear();
            ChartFX.WinForms.Statistical.Statistics st = new ChartFX.WinForms.Statistical.Statistics();
            st.Chart = uiChart_Histogram;
            uiChart_Histogram.GalleryAttributes = st.Gallery.Histogram;

            //chart1.DataGrid.TextColorData = Color.Blue;

            uiChart_Histogram.ToolTipFormat = "%L\nPoint Value : %v";

            //null 값 처리 되어 입력 된 값이 없을 경우...
            if (uiChart_Histogram.Data.Series == 0) return;

            double range = (maxY + 25) - (minY - 25);
            st.Gallery.Histogram.AxisX.Step = range / 10;

            //차트 막대바 두께 설정
            st.Chart.AllSeries.Volume = 90;

            //차트 X축 라벨 기울기 각도 설정
            st.Gallery.Histogram.AxisX.LabelAngle = 0;

            st.Gallery.Histogram.DataMax = (double)(maxY + 25);
            st.Gallery.Histogram.DataMin = (double)(minY - 25);
            st.Gallery.Histogram.AxisX.LabelsFormat.Decimals = 1;

            st.Studies.Clear();
            st.Studies.AddTitle("\n\n");
            st.Studies.Add(AnalysisMulti.SampleSize);
            st.Studies.Add(Analysis.LowerQuartile);
            st.Studies.Add(Analysis.UpperQuartile);
            st.Studies.Add(Analysis.Mean);
            st.Studies.Add(Analysis.Lsl);
            st.Studies.Add(Analysis.Usl);
            st.Studies.Add(Analysis.Cp);
            st.Studies.Add(Analysis.Cpk);

            for (int i = 0; i < st.Studies.Count; ++i)
            {
                st.Studies[i].Decimals = 2;
            }

            st.Studies[1].Text = "Total Size";
            st.Studies[2].Text = "25%(L)";
            st.Studies[3].Text = "25%(U)";

            st.Calculators[0].Lsl = minY;
            st.Calculators[0].Usl = maxY;

            st.LegendBox.Visible = true;
            st.LegendBox.Dock = ChartFX.WinForms.DockArea.Right;
            st.LegendBox.AutoSize = false;
            st.LegendBox.Width = 135;
            st.LegendBox.Height = 300;

            st.Gallery.Histogram.ShowLimits = true;

            uiChart_Histogram.Series[0].Text = "Width";

            st.LegendBox.Visible = true;

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{materials}' - Capability Analysis";
            uiChart_Histogram.Titles.Add(td);
        }

        /// <summary>
        /// 서브 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateOpeningChart(DataSet ds, string opening, string materials)
        {
            uiChart_Histogram.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 6 : 5;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                nPoints = uiChart_Histogram.Data.Points++;

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Histogram.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);

                }
            }

            //Histogram        
            uiChart_Histogram.Extensions.Clear();
            ChartFX.WinForms.Statistical.Statistics st = new ChartFX.WinForms.Statistical.Statistics();
            st.Chart = uiChart_Histogram;
            uiChart_Histogram.GalleryAttributes = st.Gallery.Histogram;

            //chart1.DataGrid.TextColorData = Color.Blue;

            uiChart_Histogram.ToolTipFormat = "%L\nPoint Value : %v";

            //null 값 처리 되어 입력 된 값이 없을 경우...
            if (uiChart_Histogram.Data.Series == 0) return;

            double range = (maxY + 25) - (minY - 25);
            st.Gallery.Histogram.AxisX.Step = range / 10;

            //차트 막대바 두께 설정
            st.Chart.AllSeries.Volume = 90;

            //차트 X축 라벨 기울기 각도 설정
            st.Gallery.Histogram.AxisX.LabelAngle = 0;

            st.Gallery.Histogram.DataMax = (double)(maxY + 25);
            st.Gallery.Histogram.DataMin = (double)(minY - 25);
            st.Gallery.Histogram.AxisX.LabelsFormat.Decimals = 1;

            st.Studies.Clear();
            st.Studies.AddTitle("\n\n");
            st.Studies.Add(AnalysisMulti.SampleSize);
            st.Studies.Add(Analysis.LowerQuartile);
            st.Studies.Add(Analysis.UpperQuartile);
            st.Studies.Add(Analysis.Mean);
            st.Studies.Add(Analysis.Lsl);
            st.Studies.Add(Analysis.Usl);
            st.Studies.Add(Analysis.Cp);
            st.Studies.Add(Analysis.Cpk);

            for (int i = 0; i < st.Studies.Count; ++i)
            {
                st.Studies[i].Decimals = 2;
            }

            st.Studies[1].Text = "Total Size";
            st.Studies[2].Text = "25%(L)";
            st.Studies[3].Text = "25%(U)";

            st.Calculators[0].Lsl = minY;
            st.Calculators[0].Usl = maxY;

            st.LegendBox.Visible = true;
            st.LegendBox.Dock = ChartFX.WinForms.DockArea.Right;
            st.LegendBox.AutoSize = false;
            st.LegendBox.Width = 135;
            st.LegendBox.Height = 300;

            st.Gallery.Histogram.ShowLimits = true;

            uiChart_Histogram.Series[0].Text = "Width";

            st.LegendBox.Visible = true;

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{materials}'_'{opening}' - Capability Analysis";
            uiChart_Histogram.Titles.Add(td);
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
