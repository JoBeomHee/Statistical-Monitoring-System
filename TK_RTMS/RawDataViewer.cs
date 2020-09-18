using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TK_RTMS.CommonCS;
using TK_RTMS.UI;

namespace TK_RTMS
{
    public partial class RawDataViwer : Form
    {
        #region 변수

        private SheetUi sheetUi = null;
        public SearchOption opt = new SearchOption();
        public Dictionary<string, string> rDic = new Dictionary<string, string>();

        #endregion

        public RawDataViwer()
        {
            InitializeComponent();

            //폼 Shown 이벤트 선언
            this.Shown += RawDataViwerForm_Shown;
            this.FormClosing += RawDataViwerForm_ClosingEvent;
            Common.dicSheet = new Dictionary<string, FarPoint.Win.Spread.SheetView>();
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
        /// Form Shown 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RawDataViwerForm_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            foreach (KeyValuePair<string, string> item in this.rDic)
            {
                //탭 페이지 생성
                TabPage tp = new TabPage(item.Value);
                tp.AutoScroll = true;

                sheetUi = new SheetUi();
                sheetUi.RItem = item.Value;
                sheetUi.opt = opt;
                sheetUi.Dock = DockStyle.Fill;

                //탭 페이지에 시트 추가
                tp.Controls.Add(sheetUi);

                //탭 컨트롤에 탭 페이지 추가
                uiTab_Main.Controls.Add(tp);

                sheetUi.UpdateData();
            }

            foreach (Control c in uiTab_Main.Controls)
            {
                c.Dock = DockStyle.Fill;
            }

            //Common.uiList = new Dictionary<string, List<Control>>();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// RawData 폼 종료 이벤트 핸드러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RawDataViwerForm_ClosingEvent(object sender, EventArgs e)
        {
            if(Common.dicSheet != null)
            {
                Common.dicSheet.Clear();
            }
        }
    }
}
