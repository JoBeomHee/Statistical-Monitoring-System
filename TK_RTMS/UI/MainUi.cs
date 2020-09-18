using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class MainUi : UserControl
    {
        #region 변수

        public string RItem = string.Empty;

        private BarLineChart barLineChart = null;
        private XBarChart xChart = null;
        private HistogramUi histogram = null;
        private YieldBarChart yieldChart = null;

        public SearchOption opt = new SearchOption();

        public string ChartName = string.Empty;

        #endregion

        public MainUi()
        {
            InitializeComponent();

            //이벤트 선언
            InitEvent();
        }

        /// <summary>
        /// 각종 컨트롤 이벤트 선언 및 호출
        /// </summary>
        public void InitEvent()
        {
            this.Load += MainUi_Load;

            //Panel 그라데이션 이벤트 선언
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Gradient);
            this.uiPanel_Title.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Gradient);
        }
        
        /// <summary>
        /// MainUi 유저컨트롤 Load 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainUi_Load(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            CreateViewerSetting();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Pareto Chart, Trend Chart, Histogram Chart, Yield Chart
        /// 4개 결과 화면 객체 생성 및 데이터 설정
        /// </summary>
        private void CreateViewerSetting()
        {
            //Target Data 로 Title 설정
            uiLab_Title.Text = $"Result Item : {this.RItem}";

            barLineChart = new BarLineChart() { RItem = this.RItem };
            xChart = new XBarChart() { RItem = this.RItem };
            histogram = new HistogramUi() { RItem = this.RItem };
            yieldChart = new YieldBarChart() { RItem = this.RItem };

            //Pareto Chart DoubleClick 이벤트 선언
            barLineChart.ChartDoubleClickEvent += ChartDoubleClick_Event;
            barLineChart.PictureBackClickEvent += PictureBackClick_Event;

            //조회조건(Search Option) 저장
            barLineChart.opt = opt;
            xChart.opt = opt;
            histogram.opt = opt;
            yieldChart.opt = opt;

            //TableLayoutPanel에 Viewer 넣어주기
            uiTpl_Main.Controls.Add(barLineChart, 0, 0);
            uiTpl_Main.Controls.Add(xChart, 0, 1);
            uiTpl_Main.Controls.Add(histogram, 1, 0);
            uiTpl_Main.Controls.Add(yieldChart, 1, 1);

            //4개 결과화면 Panel에 Dock.Fill 로 설정
            foreach (Control c in uiTpl_Main.Controls)
            {
                c.Dock = DockStyle.Fill;
            }

            //각 차트 데이터 갱신
            UpdateChart();

            Common.uiList.Add(RItem, new List<Control>() { barLineChart, histogram, xChart, yieldChart });
        }

        /// <summary>
        /// 각 차트별 데이터 갱신
        /// </summary>
        public void UpdateChart()
        {
            barLineChart.UpdateData();
            histogram.UpdateData();
            xChart.UpdateData();
            yieldChart.UpdateData();
        }

        /// <summary>
        /// Deep Search 이후 다시
        /// 이전 데이터로 돌아가는 메서드
        /// </summary>
        public void UpdateBackChart()
        {
            histogram.UpdateBackData();
            xChart.UpdateBackData();
            yieldChart.UpdateBackData();
        }

        /// <summary>
        /// Opening Deep Search 이후 다시
        /// 이전 데이터로 돌아가는 메서드
        /// </summary>
        public void UpdateMaterialsBackChart()
        {
            histogram.UpdateMaterialsBackData();
            xChart.UpdateMaterialsBackData();
            yieldChart.UpdateMaterialsBackData();
        }

        /// <summary>
        /// Pareto Chart 더블 클릭 했을 경우
        /// </summary>
        /// <param name="value"></param>
        public void ChartDoubleClick_Event(string chartType, string value, string result)
        {
            this.ChartName = value;
            xChart.count = Convert.ToInt32(result);
            string materials = barLineChart.material;

            if (chartType.Contains("TOTAL"))
            {
                histogram.UpdateSubData(this.ChartName);
                xChart.UpdateSubData(this.ChartName);
                yieldChart.UpdateSubData(this.ChartName);
            }

            if (chartType.Contains("MATERIALS"))
            {
                histogram.UpdateOpeningData(this.ChartName, materials);
                xChart.UpdateOpeningData(this.ChartName, materials);
                yieldChart.UpdateOpeningData(this.ChartName, materials);
            }
        }

        /// <summary>
        /// Pareto Chart PictureBox Back 버튼 클릭 이벤트
        /// </summary>
        public void PictureBackClick_Event(string chartType)
        {
            if(chartType.Contains("OPENING"))
            {
                UpdateMaterialsBackChart();
            }
            else if(chartType.Contains("MATERIALS"))
            {
                UpdateBackChart();
            }
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
            Color startColor = System.Drawing.ColorTranslator.FromHtml("#e0c3fc");
            Color middleColor = System.Drawing.ColorTranslator.FromHtml("#8ec5fc");
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
