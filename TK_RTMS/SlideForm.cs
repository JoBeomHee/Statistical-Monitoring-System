using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TK_RTMS
{
    public partial class SlideForm : Form
    {
        #region 변수

        List<SlideViewControl> ctlList = null;

        Timer slideTimer = new Timer();
        int idx = 0;

        public string result = string.Empty;

        //AutoView Interval 변수
        public int interval = 0;

        //각 차트별 UI Flag 변수
        public bool paretoFlag = false;
        public bool histogramFlag = false;
        public bool trendFlag = false;
        public bool yieldFlag = false;

        #endregion

        public SlideForm()
        {
            InitializeComponent();

            //이벤트 선언 및 호출
            InitEvent();
        }

        /// <summary>
        /// 윈폼 모든 컨트롤 더블 버퍼링 핸들 
        /// 이로 인해, 화면 그려지는거 부드러워짐
        /// (정확한 이유는 모름...)
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        /// <summary>
        /// 각종 이벤트 선언 및 호출 메서드
        /// </summary>
        public void InitEvent()
        {
            //각종 이벤트 선언
            this.Shown += SlideForm_Shown;
            this.FormClosing += SlideForm_Closing;
        }

        /// <summary>
        /// SlideForm Shown 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlideForm_Shown(object sender, EventArgs e)
        {
            result = result.Replace("'", "");

            if (result.Contains("ALL"))
            {
                result = "Width,Angle,Hole_L,Hole_C,Hole_R";
            }

            string[] resArr = result.Split(',');

            ctlList = new List<SlideViewControl>();

            //AutoView 에서 보고자 하는 UI 체크된 Viewer 
            //List에 Add
            for (int i = 0; i < resArr.Length; i++)
            {
                ControlFlag(resArr[i], ctlList);
            }

            slideTimer.Interval = interval * 1000;
            slideTimer.Tick += SlideTimer_Tick;

            if (ctlList.Count <= 0)
            {
                string msg = $"현재 체크된 Viewer 가 없습니다.";
                MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.Close();

                return;
            }

            this.Controls.Clear();
            this.Controls.Add(ctlList[idx].ctl);
            SetTitle(ctlList[idx].RItem, ctlList[idx].ctl);

            slideTimer.Start();

            if (ctlList.Count > 1)
                idx++;
        }

        /// <summary>
        /// Check 되어있는 Viewer
        /// List에 저장하는 메서드
        /// </summary>
        /// <param name="list"></param>
        private void ControlFlag(string result, List<SlideViewControl> list)
        {
            try
            {
                //Auto View 로 볼 Chart Check Flag를 List로 저장
                //Flag == true 이면, Auto View 볼 차트
                //Flag == false 이면, Auto Viwe 안할 차트
                List<bool> boolList = new List<bool>(){ paretoFlag, histogramFlag, trendFlag, yieldFlag };

                for(int idx = 0; idx < boolList.Count; idx++)
                {
                    if(boolList[idx] == true)
                    {
                        AddViewControl(result, idx, list);
                    }
                }
            }
            catch (KeyNotFoundException)
            {
                return;
            }
        }

        /// <summary>
        /// Chart Add 메서드
        /// </summary>
        /// <param name="result"></param>
        /// <param name="idx"></param>
        /// <param name="list"></param>
        public void AddViewControl(string result, int idx, List<SlideViewControl> list)
        {
            SlideViewControl obj = new SlideViewControl();
            obj.ctl = Common.uiList[result][idx];
            obj.RItem = result;
            list.Add(obj);
        }

        /// <summary>
        /// SlideTimer Tick 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlideTimer_Tick(object sender, EventArgs e)
        {
            this.Controls.Clear();

            if (ctlList.Count <= 0)
            {
                string msg = "현재 체크된 Viewer 가 없습니다.";
                MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                this.Close();

                return;
            }

            this.Controls.Add(ctlList[idx].ctl);
            SetTitle(ctlList[idx].RItem, ctlList[idx].ctl);

            //보고자 하는 Viewer를 하나만 선택했을 경우
            //idx 증가 하면 에러
            if (ctlList.Count > 1)
                idx++;

            if (idx >= ctlList.Count) idx = 0;
        }

        /// <summary>
        /// Viewer 별 Title 지정
        /// </summary>
        /// <param name="ctl"></param>
        private void SetTitle(string result, Control ctl)
        {
            if (ctl is UI.ParetoChart)
                this.Text = $"Auto View - [Pareto Chart][Result Item : {result}]";
            else if (ctl is UI.HistogramUi)
                this.Text = $"Auto View - [HistoGram Chart][Result Item : {result}]";
            else if (ctl is UI.TrendChartUi)
                this.Text = $"Auto View - [Trend Chart][Result Item : {result}]";
            else if (ctl is UI.YieldBarChart)
                this.Text = $"Auto View - [Yield Chart][Result Item : {result}]";
        }

        /// <summary>
        /// Slide Form Closing 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SlideForm_Closing(object sender, EventArgs e)
        {
            if (slideTimer != null)
                slideTimer.Stop();
        }
    }

    public class SlideViewControl
    {
        public Control ctl = new Control();
        public string RItem = string.Empty;
    }
}
