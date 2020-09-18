using ChartFX.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class BarLineChart : UserControl
    {
        #region 변수
        enum ChartType { TOTAL, MATERIALS, OPENING };
        private enum Type { LINES, BAR };

        public string RItem = string.Empty;

        private ChartType _CType = ChartType.TOTAL;
        private ChartType _PCType = ChartType.TOTAL;

        private string _TotalCode = string.Empty;
        private string _MaterialCode = string.Empty;
        private string _OpeningCode = string.Empty;
        public string material = string.Empty;

        DataSet totalDs = null;
        DataSet materialDs = null;
        DataSet openingDs = null;

        public SearchOption opt = new SearchOption();

        //델리게이트 선언
        public delegate void ChartDoubleClickEventHandler(string chartType, string value, string result);
        public delegate void PictureBackClickEventHandler(string chartType);

        //이벤트 선언
        public event ChartDoubleClickEventHandler ChartDoubleClickEvent;
        public event PictureBackClickEventHandler PictureBackClickEvent;

        #endregion

        public BarLineChart()
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
                uiChart_BarLine.Series[(int)Type.BAR].PointLabels.Visible = uiChk_ShowValue.Checked;
            };

            //차트 더블클릭 이벤트 선언
            this.uiChart_BarLine.MouseDoubleClick += Chart_MouseDouble_Click;

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
            uiChart_BarLine.Data.Clear();
            uiChart_BarLine.LegendBox.Visible = false;

            uiChart_BarLine.AxisX.LabelAngle = Convert.ToInt16(opt.ANGLE.Replace("º", ""));

            uiChart_BarLine.Data.Series = 2;
            uiChart_BarLine.AxisX.AutoScale = true;
            uiChart_BarLine.AxisX.AutoScroll = false;

            // 왼쪽 Y측
            uiChart_BarLine.AxisY.Visible = true;
            uiChart_BarLine.AxisY.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular);
            uiChart_BarLine.AxisY.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.None;

            // 오른쪽 Y측
            uiChart_BarLine.AxisY2.Visible = true;
            uiChart_BarLine.AxisY2.Font = new Font("Microsoft YaHei", 9F, FontStyle.Regular);
            uiChart_BarLine.AxisY2.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.None;

            //1번 Series 라인 그래프
            uiChart_BarLine.Series[(int)Type.LINES].Gallery = ChartFX.WinForms.Gallery.Lines;
            uiChart_BarLine.Series[(int)Type.LINES].Color = System.Drawing.ColorTranslator.FromHtml("#FF007F");

            //2번 Series 막대 그래프
            uiChart_BarLine.Series[(int)Type.BAR].Gallery = ChartFX.WinForms.Gallery.Bar;
            uiChart_BarLine.Series[(int)Type.BAR].Color = System.Drawing.ColorTranslator.FromHtml("#1EA4FF");

            uiChart_BarLine.Series[(int)Type.BAR].PointLabels.Visible = uiChk_ShowValue.Checked;

            //Y1, Y2 제목 설정
            uiChart_BarLine.AxisY.Title.Text = "Number by materials";
            uiChart_BarLine.AxisY.Title.TextColor = Color.Black;

            //X축 제목 설정
            uiChart_BarLine.AxisX.Title.Text = "Materials";
            uiChart_BarLine.AxisX.Title.TextColor = Color.Black;
        }

        /// <summary>
        /// Main Chart 표시
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateChart(DataSet ds)
        {
            InitChart();

            uiChart_BarLine.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double sum = 0.0;

            int resultValue = 1;
            int share = 3;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                double stdVal = 0;

                nPoints = uiChart_BarLine.Data.Points++;

                uiChart_BarLine.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][0].ToString();

                uiChart_BarLine.Points[nPoints].Tag = ds.Tables[0].Rows[row][0].ToString();

                uiChart_BarLine.Series[(int)Type.LINES].Color = System.Drawing.ColorTranslator.FromHtml("#FF007F");
                uiChart_BarLine.Series[(int)Type.LINES].AxisY = uiChart_BarLine.AxisY2;

                if (double.TryParse(ds.Tables[0].Rows[row][resultValue].ToString(), out val) == true)
                {
                    uiChart_BarLine.Data[(int)Type.BAR, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }

                if (double.TryParse(ds.Tables[0].Rows[row][share].ToString(), out stdVal) == true)
                {
                    sum += Math.Round(stdVal / 100, 4);
                    uiChart_BarLine.Data[(int)Type.LINES, nPoints] = sum;

                    //항상 마지막 점유율의 총 합은 100이다
                    if (row == ds.Tables[0].Rows.Count - 1)
                    {
                        sum = Math.Round(100.0 / 100, 4);;
                        uiChart_BarLine.Data[(int)Type.LINES, nPoints] = sum;
                    }
                }
            }

            uiChart_BarLine.AxisY.Min = 0;
            uiChart_BarLine.AxisY.Max = maxY * 1.5;
            uiChart_BarLine.AxisY.LabelsFormat.Decimals = 0;

            uiChart_BarLine.AxisY2.DataFormat.Format = ChartFX.WinForms.AxisFormat.Percentage;
            uiChart_BarLine.AxisY2.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Percentage;

            uiChart_BarLine.AxisY2.Min = 0;
            uiChart_BarLine.AxisY2.Max = 1 * 1.1;
            uiChart_BarLine.AxisY2.LabelsFormat.Decimals = 0;

            uiLab_ChartTitle.Text = string.Format("Total Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = "Materials Pareto Chart";
            uiChart_BarLine.Titles.Add(td);
        }

        /// <summary>
        /// Sub Chart 표시
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSubChart(DataSet ds, string materials)
        {
            InitChart();

            uiChart_BarLine.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double sum = 0.0;

            int resultValue = 1;
            int share = 3;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                double stdVal = 0;

                nPoints = uiChart_BarLine.Data.Points++;

                uiChart_BarLine.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][0].ToString();

                uiChart_BarLine.Points[nPoints].Tag = ds.Tables[0].Rows[row][0].ToString();

                uiChart_BarLine.Series[(int)Type.LINES].Color = System.Drawing.ColorTranslator.FromHtml("#FF007F");
                uiChart_BarLine.Series[(int)Type.LINES].AxisY = uiChart_BarLine.AxisY2;

                if (double.TryParse(ds.Tables[0].Rows[row][resultValue].ToString(), out val) == true)
                {
                    uiChart_BarLine.Data[(int)Type.BAR, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }

                if (double.TryParse(ds.Tables[0].Rows[row][share].ToString(), out stdVal) == true)
                {
                    sum += Math.Round(stdVal / 100, 4);
                    uiChart_BarLine.Data[(int)Type.LINES, nPoints] = sum;

                    //항상 마지막 점유율의 총 합은 100이다
                    if (row == ds.Tables[0].Rows.Count - 1)
                    {
                        sum = Math.Round(100.0 / 100, 4); ;
                        uiChart_BarLine.Data[(int)Type.LINES, nPoints] = sum;
                    }
                }
            }

            uiChart_BarLine.AxisY.Min = 0;
            uiChart_BarLine.AxisY.Max = maxY * 1.5;
            uiChart_BarLine.AxisY.LabelsFormat.Decimals = 0;

            uiChart_BarLine.AxisY2.DataFormat.Format = ChartFX.WinForms.AxisFormat.Percentage;
            uiChart_BarLine.AxisY2.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Percentage;

            uiChart_BarLine.AxisY2.Min = 0;
            uiChart_BarLine.AxisY2.Max = 1;
            uiChart_BarLine.AxisY2.LabelsFormat.Decimals = 0;

            uiLab_ChartTitle.Text = string.Format("Materials Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{material}' -  Pareto Chart";
            uiChart_BarLine.Titles.Add(td);
        }

        /// <summary>
        /// Sub Chart 표시
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateOpeningChart(DataSet ds, string materials, string opening)
        {
            InitChart();

            uiChart_BarLine.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double sum = 0.0;

            int resultValue = 1;
            int share = 3;

            for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
            {
                double val = 0;
                double stdVal = 0;

                nPoints = uiChart_BarLine.Data.Points++;

                uiChart_BarLine.AxesX[0].Labels[nPoints] = ds.Tables[0].Rows[row][0].ToString();

                uiChart_BarLine.Points[nPoints].Tag = ds.Tables[0].Rows[row][0].ToString();

                uiChart_BarLine.Series[(int)Type.LINES].Color = System.Drawing.ColorTranslator.FromHtml("#FF007F");
                uiChart_BarLine.Series[(int)Type.LINES].AxisY = uiChart_BarLine.AxisY2;

                if (double.TryParse(ds.Tables[0].Rows[row][resultValue].ToString(), out val) == true)
                {
                    uiChart_BarLine.Data[(int)Type.BAR, nPoints] = val;

                    maxY = Math.Max(maxY, val);
                    minY = Math.Min(minY, val);
                }

                if (double.TryParse(ds.Tables[0].Rows[row][share].ToString(), out stdVal) == true)
                {
                    sum += Math.Round(stdVal / 100, 4);
                    uiChart_BarLine.Data[(int)Type.LINES, nPoints] = sum;

                    //항상 마지막 점유율의 총 합은 100이다
                    if (row == ds.Tables[0].Rows.Count - 1)
                    {
                        sum = Math.Round(100.0 / 100, 4); ;
                        uiChart_BarLine.Data[(int)Type.LINES, nPoints] = sum;
                    }
                }
            }

            uiChart_BarLine.AxisY.Min = 0;
            uiChart_BarLine.AxisY.Max = maxY * 1.5;
            uiChart_BarLine.AxisY.LabelsFormat.Decimals = 0;

            uiChart_BarLine.AxisY2.DataFormat.Format = ChartFX.WinForms.AxisFormat.Percentage;
            uiChart_BarLine.AxisY2.LabelsFormat.Format = ChartFX.WinForms.AxisFormat.Percentage;

            uiChart_BarLine.AxisY2.Min = 0;
            uiChart_BarLine.AxisY2.Max = 1;
            uiChart_BarLine.AxisY2.LabelsFormat.Decimals = 0;

            uiLab_ChartTitle.Text = string.Format("Materials Information Chart");

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{material}'_'{opening}' Pareto Chart";
            uiChart_BarLine.Titles.Add(td);
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
                if (uiChart_BarLine.Points[point].Tag == null) return;

                if (_CType == ChartType.OPENING) return;

                this.Cursor = Cursors.WaitCursor;

                this.ChartDoubleClickEvent?.Invoke(_CType.ToString(), uiChart_BarLine.Points[point].Tag.ToString(), uiChart_BarLine.Data[(int)Type.BAR, point].ToString());

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
                {
                    _TotalCode = uiChart_BarLine.Points[point].Tag.ToString();
                    material = _TotalCode;
                }
                else if (_CType == ChartType.MATERIALS)
                {
                    _MaterialCode = uiChart_BarLine.Points[point].Tag.ToString();
                }
                else if (_CType == ChartType.OPENING)
                {
                    _OpeningCode = uiChart_BarLine.Points[point].Tag.ToString();
                }
            }

            _PCType = _CType;

            if (_PCType == ChartType.TOTAL)
                _CType = ChartType.MATERIALS;
            else if (_PCType == ChartType.MATERIALS)
                _CType = ChartType.OPENING;
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

                UpdateSubChart(materialDs, _MaterialCode);
            }
            //Materials 결과 화면에서 세부 소재 클릭 시
            if (_CType == ChartType.OPENING)
            {
                openingDs = new DataSet();
                openingDs = DB_Process.Instance.Get_ParetoChart_OpeningData(RItem, opt, _TotalCode, _MaterialCode);

                UpdateOpeningChart(openingDs, _TotalCode, _MaterialCode);
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
            this.PictureBackClickEvent?.Invoke(_CType.ToString());

            if (_PCType == ChartType.OPENING)
            {
                _PCType = ChartType.MATERIALS;
                _CType = ChartType.OPENING;

                UpdateOpeningChart(openingDs, _TotalCode, _MaterialCode);
            }
            else if (_PCType == ChartType.MATERIALS)
            {
                _PCType = ChartType.TOTAL;
                _CType = ChartType.MATERIALS;

                UpdateSubChart(materialDs, _TotalCode);
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
