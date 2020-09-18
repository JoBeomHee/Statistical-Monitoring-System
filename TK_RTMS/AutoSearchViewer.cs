using System;
using System.Windows.Forms;

namespace TK_RTMS
{
    public partial class AutoSearchViewer : Form
    {
        //델리게이트 선언
        public delegate void StartButtonEventHandler(decimal interval);
        public event StartButtonEventHandler StartButtonClickEvent;

        //이벤트 선언
        public delegate void CancelButtonEventHandler();
        public event CancelButtonEventHandler CancelButtonClickEvent;

        public AutoSearchViewer()
        {
            InitializeComponent();

            this.uiBtn_Cancel.Click += UiBtn_Cancel_Click;
            this.uiBtn_Start.Click += UiBtn_Start_Click;
        }

        /// <summary>
        /// Cancel 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiBtn_Cancel_Click(object sender, EventArgs e)
        {
            this.CancelButtonClickEvent?.Invoke();
            this.Close();
        }

        /// <summary>
        /// Start 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiBtn_Start_Click(object sender, EventArgs e)
        {
            this.StartButtonClickEvent?.Invoke(uiNum_Interval.Value);
            this.Close();
        }
    }
}
