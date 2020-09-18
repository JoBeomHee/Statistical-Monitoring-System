using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TK_RTMS
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //중복 실행 방지 폼
            try
            {
                int cnt = 0;

                Process[] procs = Process.GetProcesses();

                foreach(Process p in procs)
                {
                    if(p.ProcessName.Equals("TK_RTMS") == true)
                    {
                        cnt++;
                    }
                }

                if( cnt > 1 )
                {
                    string msg = $"프로그램이 이미 실행중입니다.";
                    MessageBox.Show(msg);
                    return;
                }
                else
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Program - Error");
            }
        }
    }
}
