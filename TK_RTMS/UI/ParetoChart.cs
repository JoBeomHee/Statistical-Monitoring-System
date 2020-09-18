using ChartFX.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class ParetoChart : UserControl
    {
        #region 변수

        public string RItem = string.Empty;

        enum ChartType { TOTAL, MATERIALS, OPENING };

        private ChartType _CType = ChartType.TOTAL;
        private ChartType _PCType = ChartType.TOTAL;

        private string _TotalCode = string.Empty;
        private string _MaterialCode = string.Empty;

        DataSet totalDs = null;
        DataSet materialDs = null;

        public SearchOption opt = new SearchOption();

        //델리게이트 선언
        public delegate void ChartDoubleClickEventHandler(string value, string result);
        public delegate void PictureBackClickEventHandler();

        //이벤트 선언
        public event ChartDoubleClickEventHandler ChartDoubleClickEvent;
        public event PictureBackClickEventHandler PictureBackClickEvent;

        #endregion

        public ParetoChart()
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
                uiChart_Pareto.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
            };

            //차트 더블클릭 이벤트 선언
            this.uiChart_Pareto.MouseDoubleClick += Chart_MouseDouble_Click;

            //PictureBox 컨트롤 클릭 이벤트 선언
            this.uiPb_Back.Click += UiPic_Back_Click;

            //패널 그라데이션 이벤트 선언
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Gradient);
            this.uiPan_Title.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel_Gradient);
        }

        /// <summary>
        /// Main Data 차트 조회 및 그리기
        /// </summary>
        public void UpdateData()
        {
            _CType = ChartType.TOTAL;
            _PCType = ChartType.TOTAL;

            InitChart();
            totalDs = DB_Process.Instance.Get_ParetoChartMainData(RItem, opt);
            totalDs = DataTableValueChanged(totalDs);

            UpdateChart(totalDs);
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
        /// 차트 초기화
        /// </summary>
        private void InitChart()
        {
            //chart1.Reset();
            uiChart_Pareto.Data.Clear();
            uiChart_Pareto.LegendBox.Visible = false;

            //chart1.View3D.Enabled = true;
            //chart1.View3D.Cluster = true;

            uiChart_Pareto.AxisX.LabelAngle = Convert.ToInt16(opt.ANGLE.Replace("º", ""));

            uiChart_Pareto.Data.Series = 1;
            uiChart_Pareto.AxisX.AutoScale = true;
            uiChart_Pareto.AxisX.AutoScroll = false;

            //chart1.AxisY2.Visible = true;

            uiChart_Pareto.AllSeries.Gallery = ChartFX.WinForms.Gallery.Pareto;
            uiChart_Pareto.Series[0].Color = Color.FromArgb(139, 0, 255);

            uiChart_Pareto.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
        }

        /// <summary>
        /// Main Chart 표시
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateChart(DataSet ds)
        {
            InitChart();

            uiChart_Pareto.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = 3;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                //double stdVal = 0;

                nPoints = uiChart_Pareto.Data.Points++;

                uiChart_Pareto.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][0].ToString();

                uiChart_Pareto.Points[nPoints].Tag = ds.Tables[0].Rows[row][0].ToString();

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Pareto.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }
            }

            //chart1.AxisY.Min = minY * 0.5;
            uiChart_Pareto.AxisY.Min = 0;
            uiChart_Pareto.AxisY.Max = maxY * 1.5;
            uiChart_Pareto.AxisY.LabelsFormat.Decimals = 1;

            uiLab_ChartTitle.Text = string.Format("Total Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = "Pareto Chart";
            uiChart_Pareto.Titles.Add(td);
        }

        /// <summary>
        /// Sub Chart 표시
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSubChart(DataSet ds)
        {
            InitChart();

            uiChart_Pareto.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = 1;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                //double stdVal = 0;

                nPoints = uiChart_Pareto.Data.Points++;

                uiChart_Pareto.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][0].ToString();

                if (double.TryParse(ds.Tables[0].Rows[row][col].ToString(), out val) == true)
                {
                    uiChart_Pareto.Data[0, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }
            }

            //chart1.AxisY.Min = minY * 0.5;
            uiChart_Pareto.AxisY.Min = 0;
            uiChart_Pareto.AxisY.Max = maxY * 1.5;

            //chart1.AxisY.Min = 0;
            //chart1.AxisY.Max = 100;
            //chart1.AxisY.Step = 10;
            uiChart_Pareto.AxisY.LabelsFormat.Decimals = 1;

            uiLab_ChartTitle.Text = string.Format("Materials Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = "Pareto Chart";
            uiChart_Pareto.Titles.Add(td);
        }

        /// <summary>
        /// 차트 마우스 더블클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_MouseDouble_Click(object sender, ChartFX.WinForms.HitTestEventArgs e)
        {
            int point = 0;

            if (e.HitType == ChartFX.WinForms.HitType.Axis)
                point = (int)e.Value;

            if (e.HitType == ChartFX.WinForms.HitType.Point)
                point = e.Point;

            if (point >= 0)
            {
                if (uiChart_Pareto.Points[point].Tag == null) return;

                if (_CType == ChartType.MATERIALS) return;

                this.Cursor = Cursors.WaitCursor;

                this.ChartDoubleClickEvent?.Invoke(uiChart_Pareto.Points[point].Tag.ToString(), uiChart_Pareto.Data[0, point].ToString());

                CheckChartType(point, e);

                ChartDeepSearch(point, e);

                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 차트 타입 설정 메서드
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        private void CheckChartType(int point, ChartFX.WinForms.HitTestEventArgs e)
        {
            if (e.Series >= 0)
            {
                if (_CType == ChartType.TOTAL)
                    _TotalCode = uiChart_Pareto.Points[point].Tag.ToString();
                else if (_CType == ChartType.MATERIALS)
                    _MaterialCode = uiChart_Pareto.Points[point].Tag.ToString();
            }

            _PCType = _CType;

            if (_PCType == ChartType.TOTAL) _CType = ChartType.MATERIALS;
        }

        /// <summary>
        /// 차트 더블클릭 시
        /// Sub 차트 보여주는 메서드
        /// </summary>
        /// <param name="point"></param>
        /// <param name="e"></param>
        private void ChartDeepSearch(int point, ChartFX.WinForms.HitTestEventArgs e)
        {
            //Total 결과 화면에서 세부 소재 클릭 시
            if (_CType == ChartType.MATERIALS)
            {
                materialDs = new DataSet();
                materialDs = DB_Process.Instance.Get_ParetoChart_SubData(RItem, opt, _TotalCode);

                //UpdateSubChart(materialDs);
            }
        }

        /// <summary>
        /// Back 이미지 PictureBox 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPic_Back_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //PictureBox Back 클릭 이벤트 선언
            this.PictureBackClickEvent?.Invoke();

            if (_PCType == ChartType.MATERIALS)
            {
                _PCType = ChartType.TOTAL;
                _CType = ChartType.MATERIALS;

                UpdateSubChart(materialDs);
            }
            else if (_PCType == ChartType.TOTAL)
            {
                _CType = ChartType.TOTAL;

                UpdateChart(totalDs);
            }

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Panel 그라데이션 이벤트
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
        /// Panel 그라데이션 이벤트
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
