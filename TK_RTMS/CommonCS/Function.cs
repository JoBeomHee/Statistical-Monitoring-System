using System;
using System.Windows.Forms;

namespace TK_RTMS.CommonCS
{
    public class Function
    {
        /// <summary>
        /// 엑셀 파일로 변환 저장
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="spread"></param>
        /// <returns></returns>
        public static bool ExportExcelFile(string fileName, FarPoint.Win.Spread.FpSpread spread)
        {
            try
            {
                for (int i = 0; i < spread.Sheets.Count; i++)
                    spread.Sheets[i].Protect = false;

                spread.SaveExcel(fileName, FarPoint.Win.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                //SaveExcelFromSheet(fileName, spread.Sheets[0]);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(new Form() { TopMost = true }, string.Format("Failed save to Excel\r\nReason : {0}", ex.Message), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 엑셀 파일 저장
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static SaveFileDialog GetExcelSaveFileDialog(string filename, string ext, string filter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.AddExtension = true;
            saveFileDialog.ValidateNames = true;
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //saveFileDialog.DefaultExt = ".xls";
            //saveFileDialog.Filter = "Microsoft Excel Workbook (*.xls)|*.xls";

            saveFileDialog.DefaultExt = ext;
            saveFileDialog.Filter = filter;
            saveFileDialog.FileName = filename;

            return saveFileDialog;
        }

        /// <summary>
        /// Copy Directory
        /// </summary>
        /// <param name="as_SourceName">Source Path</param>
        /// <param name="as_DeskName">Dest Path</param>
        public static void uf_DirectoryCopy(string as_SourceName, string as_DeskName)
        {
            string[] ls_Files = System.IO.Directory.GetFiles(as_SourceName); //Get All File In Path:as_SourceName
            string[] ls_Directorys = System.IO.Directory.GetDirectories(as_SourceName); //Get All Directorys in Path:as_SourceName
            if (!System.IO.Directory.Exists(as_DeskName))
                System.IO.Directory.CreateDirectory(as_DeskName); //Create Desk Directory

            //Copy File From Source to Dest

            foreach (string lo_EachFile in ls_Files)
            {
                string ls_FileName = lo_EachFile.Substring(lo_EachFile.LastIndexOf("\\") + 1);
                System.IO.File.Copy(lo_EachFile, as_DeskName + "\\" + ls_FileName, true);
            }
            if (ls_Directorys.Length == 0) //None Directorys 
                return;
            foreach (string lo_EachDirectory in ls_Directorys)
            {
                string ls_DirectoryName = lo_EachDirectory.Substring(lo_EachDirectory.LastIndexOf("\\") + 1);
                uf_DirectoryCopy(lo_EachDirectory, as_DeskName + "\\" + ls_DirectoryName);
            }
        }
    }
}
