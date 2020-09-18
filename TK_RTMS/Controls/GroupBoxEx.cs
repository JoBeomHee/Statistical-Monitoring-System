using System.Drawing;
using System.Windows.Forms;

namespace TK_RTMS.Controls
{
    public class GroupBoxEX : GroupBox
    {
        #region Field

        /// <summary>
        /// 테두리 색상
        /// </summary>
        /// 
        private Color borderColor;

        #endregion

        #region 테두리 색상 - BorderColor

        /// <summary>
        /// 테두리 색상
        /// </summary>

        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
                Invalidate();
            }
        }

        #endregion

        #region 생성자 - GroupBoxEX()

        /// <summary>
        /// 생성자
        /// </summary>
        public GroupBoxEX()
        {
            this.borderColor = Color.Black;
        }

        #endregion

        #region 페인트시 처리하기 - OnPaint(e)

        /// <summary>
        /// 페인트시 처리하기
        /// </summary>
        /// <param name="e">이벤트 인자</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            Size textSize = TextRenderer.MeasureText(Text, Font);
            Rectangle clientRectangle = e.ClipRectangle;

            clientRectangle.Y += textSize.Height / 2;
            clientRectangle.Height -= textSize.Height / 2;

            ControlPaint.DrawBorder(e.Graphics, clientRectangle, this.borderColor, ButtonBorderStyle.Solid);
            Rectangle textRectangle = e.ClipRectangle;

            textRectangle.X += 6;
            textRectangle.Width = textSize.Width + 1;
            textRectangle.Height = textSize.Height;

            e.Graphics.FillRectangle(new SolidBrush(BackColor), textRectangle);
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textRectangle);
        }

        #endregion

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }
    }
}
