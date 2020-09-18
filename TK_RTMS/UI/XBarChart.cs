using ChartFX.WinForms;
using ChartFX.WinForms.Statistical;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using TK_RTMS.CommonCS;

namespace TK_RTMS.UI
{
    public partial class XBarChart : UserControl
    {
        #region 변수

        public string RItem = string.Empty;
        public SearchOption opt = new SearchOption();

        public DataSet _totalDs = new DataSet();
        public DataSet _materialsDs = new DataSet();
        public DataSet _openingDs = new DataSet();

        private string backMaterials = string.Empty;

        public int count = 0;

        #endregion

        public XBarChart()
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
                uiChart_XBar.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
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
            _totalDs = DB_Process.Instance.Get_XChartMainData(RItem, opt);
            UpdateChart(_totalDs);
        }

        /// <summary>
        /// Sub Data 조회 및 Chart 그리기
        /// </summary>
        /// <param name="opening"></param>
        public void UpdateSubData(string materials)
        {
            InitChart();
            _materialsDs = DB_Process.Instance.Get_XChartSubData(RItem, opt, materials);
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
            _openingDs = DB_Process.Instance.Get_XChartOpeningData(RItem, opt, opening, materials);
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
            uiChart_XBar.Data.Clear();
            uiChart_XBar.LegendBox.Visible = false;

            uiChart_XBar.Data.Series = Convert.ToInt32(opt.TOPN);
            uiChart_XBar.AxisX.AutoScale = true;
            uiChart_XBar.AxisX.AutoScroll = true;

            uiChart_XBar.AllSeries.Gallery = ChartFX.WinForms.Gallery.Lines;

            uiChart_XBar.AllSeries.PointLabels.Visible = uiChk_ShowValue.Checked;
            uiChart_XBar.AllSeries.Color = System.Drawing.ColorTranslator.FromHtml("#FF5E00");

            //Y축 제목 설정
            uiChart_XBar.AxisY.Title.Text = "Range of result data";
            uiChart_XBar.AxisY.Title.TextColor = Color.Black;

            //X축 제목 설정
            uiChart_XBar.AxisX.Title.Text = "Result data";
            uiChart_XBar.AxisX.Title.TextColor = Color.Black;
        }

        /// <summary>
        /// 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateChart(DataSet ds)
        {
            uiChart_XBar.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            if(ds.Tables[0].Rows.Count < Convert.ToInt32(opt.TOPN))
            {
                uiChart_XBar.Data.Series = 1;
            }
            else
            {
                double avg = Math.Ceiling(Math.Round(ds.Tables[0].Rows.Count / Convert.ToDouble(opt.TOPN), 2));

                uiChart_XBar.Data.Series = avg > Convert.ToInt32(opt.TOPN) ? Convert.ToInt32(opt.TOPN) : Convert.ToInt32(avg);
            }

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 7 : 6;
            int k = 0;

            List<double> resultList = new List<double>();
            List<double> avgList = new List<double>();

            for (int idx = 0; idx < uiChart_XBar.Data.Series; idx++)
            {
                resultList.Clear();

                for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
                {
                    if ((row != 0 && row % uiChart_XBar.Data.Series == 0) || k == ds.Tables[0].Rows.Count)
                        break;

                    nPoints = 0;
                    double val = 0;

                    nPoints = uiChart_XBar.Data.Points++;

                    string date = ds.Tables[0].Rows[k][1].ToString();
                    uiChart_XBar.Series[idx].Text = date.Substring(date.Length - 6, 6).ToString();
                    uiChart_XBar.AxisX.LabelAngle = 45;

                    if (double.TryParse(ds.Tables[0].Rows[k][col].ToString(), out val) == true)
                    {
                        uiChart_XBar.Data[idx, nPoints] = val;

                        resultList.Add(val);

                        maxY = Math.Max(maxY, val);
                        minY = Math.Min(minY, val);
                    }
                    k++;
                }

                double avgValue = resultList.Average();
                avgList.Add(Math.Round(avgValue, 2));
            }

            double minValue = 0.0;
            double maxValue = 0.0;
            double gap = 0.0;

            if (uiChart_XBar.Data.Series == 1)
            {
                minValue = resultList.Min();
                maxValue = resultList.Max();
                gap = Math.Round(maxValue - minValue, 2);
            }
            else
            {
                minValue = avgList.Min();
                maxValue = avgList.Max();
                gap = Math.Round(maxValue - minValue, 2);
            }

            // X Chart
            uiChart_XBar.Extensions.Clear();
            ChartFX.WinForms.Statistical.Statistics st = new ChartFX.WinForms.Statistical.Statistics();
            st.Chart = uiChart_XBar;
            uiChart_XBar.GalleryAttributes = st.Gallery.XChart;

            st.Studies.Add(StudyGroup.Spc);

            if (gap < 5)
            {
                uiChart_XBar.AxisY.Min = minValue - gap;
                uiChart_XBar.AxisY.Max = maxValue + gap;
                uiChart_XBar.AxisY.LabelsFormat.Decimals = 1;
            }
            else
            {
                uiChart_XBar.AxisY.Min = minValue - (gap / 5);
                uiChart_XBar.AxisY.Max = maxValue + (gap / 5);
                uiChart_XBar.AxisY.LabelsFormat.Decimals = 1;
            }

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = "X̄ (X bar) Chart";
            uiChart_XBar.Titles.Add(td);
        }

        /// <summary>
        /// Sub 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateSubChart(DataSet ds, string materials)
        {
            uiChart_XBar.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;

            double avg = Math.Ceiling(Math.Round(count / Convert.ToDouble(opt.TOPN), 2));

            uiChart_XBar.Data.Series = avg > Convert.ToInt32(opt.TOPN) ? Convert.ToInt32(opt.TOPN) : Convert.ToInt32(avg);

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 7 : 6;
            int k = 0;
            List<double> resultList = new List<double>();
            List<double> avgList = new List<double>();

            for (int idx = 0; idx < uiChart_XBar.Data.Series; idx++)
            {
                resultList.Clear();

                for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
                {
                    if ((uiChart_XBar.Data.Series != 1 && row != 0 && row % uiChart_XBar.Data.Series == 0)
                        || k == ds.Tables[0].Rows.Count)
                        break;

                    nPoints = 0;
                    double val = 0;

                    nPoints = uiChart_XBar.Data.Points++;

                    string date = ds.Tables[0].Rows[k][1].ToString();
                    uiChart_XBar.Series[idx].Text = date.Substring(date.Length - 6, 6).ToString();
                    uiChart_XBar.AxisX.LabelAngle = 45;

                    if (double.TryParse(ds.Tables[0].Rows[k][col].ToString(), out val) == true)
                    {
                        uiChart_XBar.Data[idx, nPoints] = val;

                        resultList.Add(val);

                        maxY = Math.Max(maxY, val);
                        minY = Math.Min(minY, val);
                    }
                    k++;
                }

                double avgValue = resultList.Average();
                avgList.Add(Math.Round(avgValue, 2));
            }

            double minValue = 0.0;
            double maxValue = 0.0;
            double gap = 0.0;

            if (uiChart_XBar.Data.Series == 1)
            {
                minValue = resultList.Min();
                maxValue = resultList.Max();
                gap = Math.Round(maxValue - minValue, 2);
            }
            else
            {
                minValue = avgList.Min();
                maxValue = avgList.Max();
                gap = Math.Round(maxValue - minValue, 2);
            }

            // X Chart
            uiChart_XBar.Extensions.Clear();
            ChartFX.WinForms.Statistical.Statistics st = new ChartFX.WinForms.Statistical.Statistics();
            st.Chart = uiChart_XBar;
            uiChart_XBar.GalleryAttributes = st.Gallery.XChart;

            st.Studies.Add(StudyGroup.Spc);

            if (gap < 5)
            {
                uiChart_XBar.AxisY.Min = minValue - gap;
                uiChart_XBar.AxisY.Max = maxValue + gap;
                uiChart_XBar.AxisY.LabelsFormat.Decimals = 1;
            }
            else
            {
                uiChart_XBar.AxisY.Min = minValue - (gap / 5);
                uiChart_XBar.AxisY.Max = maxValue + (gap / 5);
                uiChart_XBar.AxisY.LabelsFormat.Decimals = 1;
            }

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{materials}' - X̄ (X bar) Chart";
            uiChart_XBar.Titles.Add(td);
        }

        /// <summary>
        /// Opening 차트 데이터 바인딩
        /// </summary>
        /// <param name="ds"></param>
        private void UpdateOpeningChart(DataSet ds, string opening, string materials)
        {
            uiChart_XBar.Titles.Clear();

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0) return;
            
            double avg = Math.Ceiling(Math.Round(count / Convert.ToDouble(opt.TOPN), 2));
            
            uiChart_XBar.Data.Series = avg > Convert.ToInt32(opt.TOPN) ? Convert.ToInt32(opt.TOPN) : Convert.ToInt32(avg);

            int nPoints = 0;

            double minY = double.MaxValue;
            double maxY = double.MinValue;

            int col = uiChk_GapValue.Checked ? 7 : 6;
            int k = 0;
            List<double> resultList = new List<double>();
            List<double> avgList = new List<double>();

            for (int idx = 0; idx < uiChart_XBar.Data.Series; idx++)
            {
                resultList.Clear();

                for (int row = 0; row < ds.Tables[0].Rows.Count; ++row)
                {
                    if ((uiChart_XBar.Data.Series != 1 &&row != 0 && row % uiChart_XBar.Data.Series == 0) 
                        || k == ds.Tables[0].Rows.Count)
                        break;

                    nPoints = 0;
                    double val = 0;

                    nPoints = uiChart_XBar.Data.Points++;

                    string date = ds.Tables[0].Rows[k][1].ToString();
                    uiChart_XBar.Series[idx].Text = date.Substring(date.Length - 6, 6).ToString();
                    uiChart_XBar.AxisX.LabelAngle = 45;

                    if (double.TryParse(ds.Tables[0].Rows[k][col].ToString(), out val) == true)
                    {
                        uiChart_XBar.Data[idx, nPoints] = val;

                        resultList.Add(val);

                        maxY = Math.Max(maxY, val);
                        minY = Math.Min(minY, val);
                    }
                    k++;
                }

                double avgValue = resultList.Average();
                avgList.Add(Math.Round(avgValue, 2));
            }

            double minValue = 0.0;
            double maxValue = 0.0;
            double gap = 0.0;

            if (uiChart_XBar.Data.Series == 1)
            {
                minValue = resultList.Min();
                maxValue = resultList.Max();
                gap = Math.Round(maxValue - minValue, 2);
            }
            else
            {
                minValue = avgList.Min();
                maxValue = avgList.Max();
                gap = Math.Round(maxValue - minValue, 2);
            }

            // X Chart
            uiChart_XBar.Extensions.Clear();
            ChartFX.WinForms.Statistical.Statistics st = new ChartFX.WinForms.Statistical.Statistics();
            st.Chart = uiChart_XBar;
            uiChart_XBar.GalleryAttributes = st.Gallery.XChart;

            st.Studies.Add(StudyGroup.Spc);

            if (gap < 5)
            {
                uiChart_XBar.AxisY.Min = minValue - gap;
                uiChart_XBar.AxisY.Max = maxValue + gap;
                uiChart_XBar.AxisY.LabelsFormat.Decimals = 1;
            }
            else
            {
                uiChart_XBar.AxisY.Min = minValue - (gap / 5);
                uiChart_XBar.AxisY.Max = maxValue + (gap / 5);
                uiChart_XBar.AxisY.LabelsFormat.Decimals = 1;
            }

            //차트 Title 설정       
            TitleDockable td = new TitleDockable();
            td.Font = new Font("Arial", 11, FontStyle.Bold);
            td.TextColor = Color.DarkBlue;
            td.Text = $"'{materials}'_'{opening}' - X̄ (X bar) Chart";
            uiChart_XBar.Titles.Add(td);
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
