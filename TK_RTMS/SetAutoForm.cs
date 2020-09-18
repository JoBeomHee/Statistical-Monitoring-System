using System;
using System.Windows.Forms;

namespace TK_RTMS
{
    public partial class SetAutoForm : Form
    {
        #region 변수

        //대리자 생성
        public delegate void FormCancelEventHandler();
        public delegate void FormStartEventHandler();
        public delegate void FormClosingEventHandler();

        //이벤트 생성
        public event FormCancelEventHandler CancelEvent;
        public event FormStartEventHandler StartEvent;
        public event FormClosingEventHandler ClosingEvent;

        //최종 Target Data 배열
        string[] resultArr = { "Width", "Angle", "Hole_L", "Hole_C", "Hole_R" };

        #endregion

        public SetAutoForm()
        {
            InitializeComponent();

            //각종 이벤트 선언
            InitEvent();
        }

        /// <summary>
        /// 이벤트 선언 메서드
        /// </summary>
        public void InitEvent()
        {
            //Shown 이벤트 선언
            this.Shown += SetAutoForm_Shown;

            //버튼 클릭 이벤트 선언
            this.uiBtn_Cancel.Click += UiBtn_Cancel_Click;
            this.uiBtn_Start.Click += UiBtn_Start_Click;

            //폼 닫기 이벤트 선언
            this.FormClosing += AutoForm_Closing;
        }

        /// <summary>
        /// Shown 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetAutoForm_Shown(object sender, EventArgs e)
        {
            InitCheck();
            uiCcmb_Result.Reset(resultArr);
        }

        /// <summary>
        /// CheckBox Load 시
        /// 일괄적으로 모두 Check = True 상태로
        /// </summary>
        private void InitCheck()
        {
            uiCkb_Pareto.Checked = true;
            uiCkb_Histogram.Checked = true;
            uiCkb_Trend.Checked = true;
            uiCkb_Yield.Checked = true;
        }

        /// <summary>
        /// Slide 폼 객체 및 이벤트 선언 메서드
        /// </summary>
        private void CreateSlideForm()
        {
            SlideForm frm = new SlideForm();

            frm.result = uiCcmb_Result.Text;
            frm.paretoFlag = uiCkb_Pareto.Checked;
            frm.histogramFlag = uiCkb_Histogram.Checked;
            frm.trendFlag = uiCkb_Trend.Checked;
            frm.yieldFlag = uiCkb_Yield.Checked;

            frm.interval = Convert.ToInt32(uiNum_Interval.Value);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.WindowState = FormWindowState.Maximized;
            frm.ShowDialog();
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiBtn_Cancel_Click(object sender, EventArgs e)
        {
            this.CancelEvent?.Invoke();
            this.Close();
        }

        /// <summary>
        /// 시작 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiBtn_Start_Click(object sender, EventArgs e)
        {
            this.StartEvent?.Invoke();

            //Slide 폼 객체 및 이벤트 선언 메서드
            CreateSlideForm();
        }

        /// <summary>
        /// 폼 닫기 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AutoForm_Closing(object sender, EventArgs e)
        {
            this.ClosingEvent?.Invoke();
        }
    }
}
