using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TK_RTMS.CommonCS;
using TK_RTMS.Controls;
using TK_RTMS.Manager;

namespace TK_RTMS
{
    public partial class MainForm : Form
    {
        #region 변수

        public enum OPTION_TYPE { DOOR, LENDING, TYPE, MODEL, OPENING, MATERIALS }
        public enum OPTION_NO_TYPE { DOOR, LENDING, MODEL, OPENING, MATERIALS }

        Timer autoSearchTimer = null;

        bool autoViewFlag = false;
        bool autoSearchFlag = false;
        int autoSearchInterval = 0;

        Dictionary<string, string> _rItemDic = new Dictionary<string, string>();
        SortedTupleBag<string, string> _searchOptionDic = new SortedTupleBag<string, string>();
        IEnumerable<Tuple<string, string>> distinctDic;
        List<string> eqpidList = new List<string>();
        string[] eqpID = { "PP-L1", "PP-L2" };

        #endregion

        public MainForm()
        {
            InitializeComponent();

            //이벤트 선언
            InitEvent();

            //Judge ComboBox Default 값 OK 로 지정
            uiCb_Judge.Text = "OK";

            //Chart Label Angle
            uiCb_LabelAngle.Text = "45º";

            //차트에 보여질 데이터 개수 설정 MAX = 25개, MIN = 0개
            this.uiNum_Top.Maximum = 26;
            this.uiNum_Top.Minimum = -1;
        }

        #region 각종 메서드

        /// <summary>
        /// 각종 컨트롤 이벤트 선언 메서드
        /// </summary>
        private void InitEvent()
        {
            //메인폼 Shown 이벤트 선언
            this.Shown += MainForm_Shown;

            //각종 PictureBox 클릭 이벤트 선언
            this.uiPb_Data_LookUp.Click += UiPb_Search_Click;
            this.uiPb_Auto_View.Click += UiPb_AutoView_Click;
            this.uiPb_Save_Excel.Click += UiPb_SaveExcel_Click;
            this.uiPb_ShowOption_Condition.Click += UiPb_ShowMainCondition_Click;
            this.uiPb_Raw_Data.Click += UiPb_RawData_Click;
            this.uiPb_Auto_Search.Click += UiPb_AutoSearch_Click;

            //각종 PictureBox MouseHover 이벤트 선언
            this.uiPb_Data_LookUp.MouseHover += UiPb_MouseHover_Event;
            this.uiPb_Auto_View.MouseHover += UiPb_MouseHover_Event;
            this.uiPb_Save_Excel.MouseHover += UiPb_MouseHover_Event;
            this.uiPb_ShowOption_Condition.MouseHover += UiPb_MouseHover_Event;
            this.uiPb_Raw_Data.MouseHover += UiPb_MouseHover_Event;
            this.uiPb_Auto_Search.MouseHover += UiPb_MouseHover_Event;

            //각종 PictureBox MouseLeave 이벤트 선언
            this.uiPb_Data_LookUp.MouseLeave += UiPb_MouseLeave_Event;
            this.uiPb_Auto_View.MouseLeave += UiPb_MouseLeave_Event;
            this.uiPb_Save_Excel.MouseLeave += UiPb_MouseLeave_Event;
            this.uiPb_ShowOption_Condition.MouseLeave += UiPb_MouseLeave_Event;
            this.uiPb_Raw_Data.MouseLeave += UiPb_MouseLeave_Event;
            this.uiPb_Auto_Search.MouseLeave += UiPb_MouseLeave_Event;

            //DateTimePicker 컨트롤 변경 이벤트 선언
            this.uiDT_Start.ValueChanged += UiDT_ValueChanged;
            this.uiDT_End.ValueChanged += UiDT_ValueChanged;

            //Textbox 컨트롤 변경 이벤트 선언
            this.uiTxt_MaterialFilter.TextChanged += UiTxt_TextChanged;

            //NumericUpDown 컨트롤 값 변경 이벤트 선언
            this.uiNum_Top.ValueChanged += UiNum_Top_ValueChanged;
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
        /// Shown 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //Connect to DB
            if (SqlDBManager.Instance.GetConnection() == false)
            {
                string msg = $"Failed to Connect to Database";
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            InitFrm();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 자동 조회 Timer 설정
        /// </summary>
        private void AutoSearchTimerSetting()
        {
            this.UiPb_Search_Click(null, null);

            autoSearchTimer = new Timer(); //타이머 객체 생성
            autoSearchTimer.Interval = autoSearchInterval * 1000; //Interval 설정
            autoSearchTimer.Tick += AutoSearchTimer_Tick; //Tick 이벤트 선언
            autoSearchTimer.Start(); //타이머 시작
        }

        /// <summary>
        /// 자동 조회 Timer Tick 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoSearchTimer_Tick(object sender, EventArgs e)
        {
            AutoSearchViewer_Run();
        }

        /// <summary>
        /// 자동조회 Timer 동작 메서드
        /// </summary>
        public void AutoSearchViewer_Run()
        {
            for (int i = 0; i < uiTab_Main.TabPages.Count; i++)
            {
                UI.MainUi maunUi = uiTab_Main.TabPages[i].Controls[0] as UI.MainUi;
                maunUi.UpdateChart();
            }
        }

        /// <summary>
        /// 조회기간 설정 메서드
        /// </summary>
        private void InitFrm()
        {
            //Search Option (조회옵션) 리스트 초기화
            ClearList();

            //StartTime, EndTime 포맷 설정
            this.uiDT_Start.CustomFormat = "yyyyMMdd_HHmmss";
            this.uiDT_End.CustomFormat = "yyyyMMdd_HHmmss";

            string start = DateTime.Now.AddDays(0).ToString("yyyyMMdd_000000");
            string end = DateTime.Now.AddDays(+1).ToString("yyyyMMdd_000000");

#if DEBUG
            start = "20200706_000000";
            end = "20200708_000000";
#endif

            this.uiDT_Start.Value = DateTime.ParseExact(start, "yyyyMMdd_HHmmss", null);
            this.uiDT_End.Value = DateTime.ParseExact(end, "yyyyMMdd_HHmmss", null);

            //EqupipmentSearchOptingSetting();
            SearchOptoingSetting();

            InitCondition();
        }

        /// <summary>
        /// 조회조건 날짜에 맞게 설정
        /// </summary>
        private void SearchOptoingSetting()
        {
            ClearList();

            DataSet searchDs = new DataSet();

            //Database에서 해당 조회 날짜에 맞는 MTR_NM 컬럼 데이터 값 불러오기
            searchDs = DB_Process.Instance.Get_SearchOptionData(AddSearchOption());

            for (int row = 0; row < searchDs.Tables[0].Rows.Count; row++)
            {
                string[] arr = searchDs.Tables[0].Rows[row]["SearchOption"].ToString().Split(' ');

                //Type이 있는 경우
                if (arr[(int)OPTION_TYPE.TYPE].ToString().Contains("FP"))
                {
                    _searchOptionDic.Add("DOOR", arr[(int)OPTION_TYPE.DOOR]);
                    _searchOptionDic.Add("LENDING", $"#{arr[(int)OPTION_TYPE.LENDING]}");
                    _searchOptionDic.Add("TYPE", arr[(int)OPTION_TYPE.TYPE]);
                    _searchOptionDic.Add("MODEL", arr[(int)OPTION_TYPE.MODEL]);
                    _searchOptionDic.Add("OPENING", arr[(int)OPTION_TYPE.OPENING]);
                    _searchOptionDic.Add("MATERIALS", string.Join(" ", arr, (int)OPTION_TYPE.MATERIALS, arr.Length - (int)OPTION_TYPE.MATERIALS));
                }
                else
                {
                    _searchOptionDic.Add("DOOR", arr[(int)OPTION_NO_TYPE.DOOR]);
                    _searchOptionDic.Add("LENDING", $"#{arr[(int)OPTION_NO_TYPE.LENDING]}");
                    _searchOptionDic.Add("MODEL", arr[(int)OPTION_NO_TYPE.MODEL]);
                    _searchOptionDic.Add("OPENING", arr[(int)OPTION_NO_TYPE.OPENING]);
                    _searchOptionDic.Add("MATERIALS", string.Join(" ", arr, (int)OPTION_NO_TYPE.MATERIALS, arr.Length - (int)OPTION_NO_TYPE.MATERIALS));
                }
            }

            //Materials 리스트 에서 숫자 제거
            DeleteNumberString();

            //Search Option Tuple 중복 검사
            DistinctTuple();
        }

        /// <summary>
        /// 조회조건 날짜에 맞게 EQPID 데이터 조회 설정
        /// </summary>
        private void EqupipmentSearchOptingSetting()
        {
            //EQPID 리스트 초기화
            eqpidList.Clear();

            DataSet eqpIdDs = new DataSet();

            //Database에서 해당 조회 날짜에 맞는 PROD_GB 컬럼 데이터 설비 값 불러오기
            eqpIdDs = DB_Process.Instance.Get_EquipmentSearchOptionData(AddSearchOption());

            for (int row = 0; row < eqpIdDs.Tables[0].Rows.Count; row++)
            {
                eqpidList.Add(eqpIdDs.Tables[0].Rows[row]["PROD_GB"].ToString());
            }
        }

        /// <summary>
        /// Materials 리스트에서 맨 마지막 소수점 제거
        /// </summary>
        private void DeleteNumberString()
        {
            List<string> _list = new List<string>();

            foreach (var tuple in _searchOptionDic)
            {
                if (tuple.Item1.Contains("MATERIALS") && tuple.Item2.Contains("."))
                {
                    _list.Add(tuple.Item2.Substring(0, tuple.Item2.LastIndexOf('.') - 1));
                }
                else if (tuple.Item1.Contains("MATERIALS"))
                {
                    _list.Add(tuple.Item2.ToString());
                }
            }

            for (int idx = 0; idx < _list.Count; idx++)
            {
                _searchOptionDic.Add("MATERIALS_DECIMAL_DELETE", _list[idx].ToString());
            }
        }

        /// <summary>
        /// Search Option 튜플 중복 제거 메서드
        /// </summary>
        private void DistinctTuple()
        {
            distinctDic = _searchOptionDic.Distinct();
        }

        /// <summary>
        /// Search Option 리스트 초기화 메서드
        /// </summary>
        private void ClearList()
        {
            _searchOptionDic.Clear();
        }

        /// <summary>
        /// Search Option 콤보박스 초기화 메서드
        /// </summary>
        private void ClearComboBox()
        {
            uiCcmb_Type.Clear();
            uiCcmb_Model.Clear();
            uiCcmb_Opening.Clear();
            uiCcmb_Materials.Clear();
        }

        /// <summary>
        /// 조회옵션 설정 메서드
        /// </summary>
        private void InitCondition()
        {
            //ComboBox 컨트롤 초기화
            ClearComboBox();

            string start = uiDT_Start.Value.ToString("yyyyMMdd_000000");
            string end = uiDT_End.Value.ToString("yyyyMMdd_000000");

            //각 조회 조건 초기화
            uiCcmb_Door.Reset(GetArray("DOOR"));
            uiCcmb_LR.Reset(GetArray("LENDING"));
            uiCcmb_Type.Reset(GetArray("TYPE"));
            uiCcmb_Model.Reset(GetArray("MODEL"));
            uiCcmb_Opening.Reset(GetArray("OPENING"));
            uiCcmb_Materials.Reset(GetArray("MATERIALS_DECIMAL_DELETE"));
            uiCcmb_EqpID.Reset(eqpID);

            //Result 옵션
            uiChk_Width.Checked = true;
        }

        /// <summary>
        /// SearchOption 분류하는 메서드
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string[] GetArray(string searchType)
        {
            var reaultArray = distinctDic
                             .Where(x => x.Item1.Contains(searchType))
                             .Select(s => s.Item2).ToArray();

            return reaultArray;
        }

        /// <summary>
        /// 조회조건 정보 저장 메서드
        /// </summary>
        public SearchOption AddSearchOption()
        {
            //조회조건 SearchOption 클래스 객체에 저장
            SearchOption opt = new SearchOption
            {
                START_TIME = uiDT_Start.Text.ToString(),
                END_TIME = uiDT_End.Text.ToString(),
                DOOR = uiCcmb_Door.Text,
                LR = uiCcmb_LR.Text,
                TYPE = uiCcmb_Type.Text,
                MODEL = uiCcmb_Model.Text,
                OPENING = uiCcmb_Opening.Text,
                MATERIALS = uiCcmb_Materials.Text,
                JUDGE = uiCb_Judge.Text,
                EQPID = uiCcmb_EqpID.Text,
                TOPN = uiNum_Top.Value.ToString(),
                ANGLE = uiCb_LabelAngle.Text,
                RESULT_WIDTH = uiChk_Width.Checked,
                RESULT_ANGLE = uiChk_Width.Checked,
                RESULT_HOLE_L = uiChk_Hole_L.Checked,
                RESULT_HOLE_C = uiChk_Hole_C.Checked,
                RESULT_HOLE_R = uiChk_Hole_R.Checked
            };

            return opt;
        }

        /// <summary>
        /// Search Option 숨기기
        /// </summary>
        /// <param name="pb"></param>
        private void HideSearchCondition(PictureBox pb)
        {
            uiTlp_Main.RowStyles[1].Height = 100;
            pb.Text = "Hide Condition";
            pb.ForeColor = Color.Red;
            pb.Image = Properties.Resources.ShowOption;
        }

        /// <summary>
        /// Search Option 보여주기
        /// </summary>
        /// <param name="btn"></param>
        private void ShowSearchCondition(PictureBox pb)
        {
            uiTlp_Main.RowStyles[1].Height = 0;
            pb.Text = "Show Condition";
            pb.ForeColor = Color.Blue;
            pb.Image = Properties.Resources.HideOption;
        }

        /// <summary>
        /// 조회된 결과 화면 보여주는 메서드
        /// </summary>
        private void ResultViewer()
        {
            uiTab_Main.Controls.Clear();

            Common.uiList = new Dictionary<string, List<Control>>();

            foreach (KeyValuePair<string, string> item in this._rItemDic)
            {
                //탭 페이지 생성
                TabPage tp = new TabPage(item.Value);
                tp.AutoScroll = true;

                UI.MainUi mainUi = new UI.MainUi();

                mainUi.opt = AddSearchOption(); //조회조건 정보 저장
                mainUi.Width = uiTab_Main.Width - 20;
                mainUi.RItem = item.Value;
                mainUi.Dock = DockStyle.Fill;

                //Tab Page 추가
                tp.Controls.Add(mainUi);

                uiTab_Main.Controls.Add(tp);
            }
        }

        /// <summary>
        /// SetAutoForm 객체 선언 및 이벤트 선언
        /// </summary>
        private void CreateSetAutoForm()
        {
            SetAutoForm frm = new SetAutoForm();
            frm.StartEvent += StartButton_Event;
            frm.CancelEvent += CancelButton_Event;
            frm.ClosingEvent += ClosingForm_Event;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        /// <summary>
        /// AutoSearchForm 객체 선언 및 이벤트 선언
        /// </summary>
        private void CreateAutoSearchForm()
        {
            AutoSearchViewer frm = new AutoSearchViewer();
            frm.StartButtonClickEvent += StartButton_ClickEvent;
            frm.CancelButtonClickEvent += CancelButton_ClickEvent;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        /// <summary>
        /// 자동 조회동안 나머지 Button 비활성화
        /// </summary>
        private void PictureBoxEnableFalse()
        {
            uiPb_Auto_View.Enabled = false;
            uiPb_Save_Excel.Enabled = false;
            uiPb_Data_LookUp.Enabled = false;
            uiPb_ShowOption_Condition.Enabled = false;
        }

        /// <summary>
        /// 자동조회 끝나거나 취소 시
        /// 나머지 Button 활성화
        /// </summary>
        private void PictureBoxEnableTrue()
        {
            uiPb_Auto_View.Enabled = true;
            uiPb_Save_Excel.Enabled = true;
            uiPb_Data_LookUp.Enabled = true;
            uiPb_ShowOption_Condition.Enabled = true;
        }

        /// <summary>
        /// 툴팁 이름 설정 메서드
        /// </summary>
        /// <param name="pb"></param>
        private void CreateTooltipName(PictureBox pb)
        {
            //ToolTip 객체 생성
            ToolTip tt = new ToolTip();
            tt.IsBalloon = true;

            //ToolTip 이름 배열로 선언
            string[] nameArr = { "ShowOption_Condition", "Auto_View", "Auto_Search", "Raw_Data", "Save_Excel", "Data_LookUp" };

            string name = string.Empty;

            var result = nameArr.Where(s => pb.Name.Contains(s)).ToList();

            name = result[0].Replace("_", " ").ToString();
            tt.SetToolTip(pb, name);
        }

        #endregion

        #region 각종 이벤트

        /// <summary>
        /// 조회 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_Search_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            //최종적으로 Check 되어있는 Result Item 딕셔너리에 저장
            ResultItemAdd();

            //결과 화면 출력
            ResultViewer();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 최종적으로 보여줄 Result Item Dictionary 컬렉션에 저장
        /// </summary>
        private void ResultItemAdd()
        {
            //Dictionary 초기화
            this._rItemDic.Clear();

            List<CheckBox> checkBoxList = uiGb_ResultItem.Controls.Cast<CheckBox>().ToList();

            //리스트 역순으로 정렬
            checkBoxList.Reverse();

            //리스트 반복 진행
            foreach (var cb in checkBoxList)
            {
                if (cb.Checked)
                {
                    this._rItemDic.Add(cb.Text, cb.Text);
                }
            }
        }

        /// <summary>
        /// Auto View 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_AutoView_Click(object sender, EventArgs e)
        {
            //결과 데이터가 조회되지 않는 화면은
            //AutoView 기능 사용 못함. 최소 결과하면 1개 이상은 있어야 함.
            if (uiTab_Main.TabPages[0].Controls.Count <= 0)
            {
                string msg = $"현재 조회 된 결과가 없습니다.";
                MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //SetAutoForm 객체 선언 및 이벤트 호출
            CreateSetAutoForm();
        }

        /// <summary>
        /// AutoView 폼 Start 버튼 이벤트 핸들러
        /// </summary>
        private void StartButton_Event()
        {
            autoViewFlag = true;
        }

        /// <summary>
        /// AutoView 폼 Closing 이벤트 핸들러
        /// </summary>
        private void CancelButton_Event()
        {
            if (autoViewFlag == true)
            {
                //결과 화면 출력
                ResultViewer();

                autoViewFlag = false;
            }
        }

        /// <summary>
        /// AutoView 폼 종료 이벤트 핸들러
        /// </summary>
        private void ClosingForm_Event()
        {
            if (autoViewFlag == true)
            {
                //결과 화면 출력
                ResultViewer();

                autoViewFlag = false;
            }
        }

        /// <summary>
        /// Hide, Show Condition 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_ShowMainCondition_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (pb.Text == "Show Condition")
            {
                HideSearchCondition(pb); //Search Option 숨기기
            }
            else
            {
                ShowSearchCondition(pb); //Search Option 보여주기
            }
        }

        /// <summary>
        /// RawData 보기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_RawData_Click(object sender, EventArgs e)
        {
            //최종적으로 Check 되어있는 Result Item 딕셔너리에 저장
            ResultItemAdd();

            //RawData Viewer 호출
            RawDataViwer frm = new RawDataViwer();
            frm.opt = AddSearchOption();
            frm.rDic = this._rItemDic;
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        /// <summary>
        /// 자동 조회 설정 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_AutoSearch_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;

            if (pb.Text == "Auto Start")
            {
                //남은 Button 들 비활성화
                PictureBoxEnableTrue();

                pb.Text = "Auto Stop";
                pb.ForeColor = Color.Red;

                if (autoSearchTimer != null)
                {
                    autoSearchTimer.Stop();
                    autoSearchTimer = null;
                }

                uiTlp_Main.RowStyles[1].Height = 100;

                autoSearchFlag = false;

                uiPb_Auto_Search.Image = Properties.Resources.AutoStop;
            }
            else
            {
                //비활성화 Button 다시 비활성화
                PictureBoxEnableFalse();

                pb.Text = "Auto Start";
                pb.ForeColor = Color.Blue;

                //AutoSearchForm 객체 선언 및 이벤트 호출
                CreateAutoSearchForm();

                //자동조회 하려고 자동조회 버튼 클릭은 했으나
                //자동조회 화면에서 Cancel 버튼을 클릭했을 경우
                //결국, 자동조회 하지 않음.
                if (autoSearchFlag == false)
                {
                    //남은 Button 들 활성화
                    PictureBoxEnableTrue();

                    pb.Text = "Auto Stop";
                    pb.ForeColor = Color.Red;
                    uiTlp_Main.RowStyles[1].Height = 100; //= uiTpl_Main_Height;

                    return;
                }

                //타이머 시작
                if (autoSearchFlag != false)
                {
                    AutoSearchTimerSetting();
                }

                uiTlp_Main.RowStyles[1].Height = 0;

                uiPb_Auto_Search.Image = Properties.Resources.AutoStart;
            }
        }

        /// <summary>
        /// 자동 조회 Start 버튼 클릭 이벤트
        /// </summary>
        /// <param name="interval"></param>
        private void StartButton_ClickEvent(decimal interval)
        {
            autoSearchFlag = true;
            autoSearchInterval = Convert.ToInt32(interval);
        }

        /// <summary>
        /// 자동 조회 Cancel 버튼 클릭 이벤트
        /// </summary>
        private void CancelButton_ClickEvent()
        {
            autoSearchFlag = false;
            InitFrm();
            InitCondition();
        }

        /// <summary>
        /// PictureBox MouseHover 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_MouseHover_Event(object sender, EventArgs e)
        {
            //PictureBox 객체 정보 가져오기
            PictureBox pb = sender as PictureBox;

            //ToolTip 생성
            CreateTooltipName(pb);

            pb.BackColor = Color.Orange;
        }

        /// <summary>
        /// PictureBox MouseLeave Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_MouseLeave_Event(object sender, EventArgs e)
        {
            //PictureBox 객체 정보 가져오기
            PictureBox pb = sender as PictureBox;

            pb.BackColor = Color.Transparent;
        }

        /// <summary>
        /// DateTimePicker 변경 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiDT_ValueChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            if (sender == uiDT_Start)
            {
                uiDT_End.Value = uiDT_Start.Value.AddDays(1);
                return;
            }

            if (sender == uiDT_End)
            {
                if (uiDT_End.Value < uiDT_Start.Value)
                {
                    string msg = $"시작 시간은 종료 시간보다 길 수 없습니다.";
                    JKMessageBox.Show(msg, "Warining", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    uiDT_End.Value = uiDT_Start.Value.AddDays(1); ;
                }
            }

            //EqupipmentSearchOptingSetting();
            SearchOptoingSetting();
            InitCondition();

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Filter 텍스트박스 변경 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiTxt_TextChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            List<string> list = new List<string>();

            var arr = uiTxt_MaterialFilter.Text.ToUpper().Replace(" ", "").Split(new string[] { ",", ";", "." }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (new string[] { ",", ";", "." }.Any(s => uiTxt_MaterialFilter.Text.ToUpper().Contains(s)))
            {
                foreach (var item in arr)
                {
                    var result = FilterLinq(item);
                    list.AddRange(result);
                }
            }
            else
            {
                var result = FilterLinq(uiTxt_MaterialFilter.Text.ToUpper());
                list.AddRange(result);
            }

            //최종적으로 Filter된 List 데이터 CheckComboBox에 다시 데이터 바인딩
            uiCcmb_Materials.Reset(list.Distinct().ToArray());

            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Filter 기능 Linq 질의문 생성 메서드
        /// </summary>
        /// <param name="filterString"></param>
        /// <returns></returns>
        private List<string> FilterLinq(string filterString)
        {
            var result = (from dic in distinctDic
                          where dic.Item1.Contains("MATERIALS_DECIMAL_DELETE") &&
                                dic.Item2.ToUpper().Contains(filterString)
                          select dic.Item2).ToList();

            return result;
        }

        /// <summary>
        /// Numeric Up Down 컨트롤 최대값, 최소값 설정 이벤트 핸들러
        /// </summary>
        /// <param name="sencer"></param>
        /// <param name="e"></param>
        private void UiNum_Top_ValueChanged(object sencer, EventArgs e)
        {
            //Max는 20
            if (this.uiNum_Top.Value > 20)
            {
                this.uiNum_Top.Value = 0;
            }
            if (this.uiNum_Top.Value < 0)
            {
                this.uiNum_Top.Value = this.uiNum_Top.Maximum - 1;
            }
        }

        /// <summary>
        /// 엑셀로 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UiPb_SaveExcel_Click(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"BeomBeomJoJo_RTMS_{date}.xls";
            string ext = ".xls";
            string filter = "Microsoft Excel Workbook (*.xls)|*.xls";

            using (SaveFileDialog saveFileDialog = Function.GetExcelSaveFileDialog(fileName, ext, filter))
            {
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;

                    FarPoint.Win.Spread.FpSpread fs = new FarPoint.Win.Spread.FpSpread();
                    fs.Sheets.Count = uiTab_Main.Controls.Count;
                    FarPoint.Win.Spread.CellType.ImageCellType imgCell = new FarPoint.Win.Spread.CellType.ImageCellType();
                    imgCell.Style = FarPoint.Win.RenderStyle.Stretch;

                    for (int row = 0; row < uiTab_Main.Controls.Count; ++row)
                    {
                        UI.MainUi ui = uiTab_Main.TabPages[row].Controls[0] as UI.MainUi;

                        fs.Sheets[row].Columns.Count = 1;
                        fs.Sheets[row].Columns[0].CellType = imgCell;

                        fs.Sheets.Add(ui);

                        if (ui == null) continue;

                        using (Graphics g = ui.CreateGraphics())
                        {
                            Bitmap memBitMap = new Bitmap(ui.Width, ui.Height);
                            ui.DrawToBitmap(memBitMap, new Rectangle(0, 0, ui.Width, ui.Height));
                            g.DrawImageUnscaled(memBitMap, 0, 0);

                            fs.Sheets[row].Rows.Count = 1;
                            fs.Sheets[row].Columns[0].Width = 1500;
                            fs.Sheets[row].Rows[0].Height = 800;
                            fs.Sheets[row].Cells[0, 0].Value = memBitMap.Clone();

                            memBitMap.Dispose();
                        }

                        fs.Sheets[row].SheetName = ui.RItem;
                    }

                    if (Function.ExportExcelFile(saveFileDialog.FileName, fs) == true)
                        JKMessageBox.ShowExcel(this, saveFileDialog.FileName);

                    this.Cursor = Cursors.Default;
                }
            }
        }

        #endregion
    }

    public class SortedTupleBag<TKey, TValue> : SortedSet<Tuple<TKey, TValue>> where TKey : IComparable
    {
        private class TupleComparer : Comparer<Tuple<TKey, TValue>>
        {
            public override int Compare(Tuple<TKey, TValue> x, Tuple<TKey, TValue> y)
            {
                if (x == null || y == null) return 0;
                return x.Item1.Equals(y.Item1) ? 1 : Comparer<TKey>.Default.Compare(x.Item1, y.Item1);
            }
        }
        public SortedTupleBag() : base(new TupleComparer()) { }
        public void Add(TKey key, TValue value)
        {
            Add(new Tuple<TKey, TValue>(key, value));
        }
    }
}
