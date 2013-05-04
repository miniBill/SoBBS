using System.Windows.Forms;

namespace Sobbs.Cui.Forms
{
    public class FormWidget : Control, IWidget
    {
        protected FormWidget(WidgetInfo info)
        {
            Left = (int)(info.X * SoApplication.Xprop);
            Top = (int)(info.Y * SoApplication.Yprop);
            Width = (int)(info.Width * SoApplication.Xprop);
            w = info.Width;
            Height = (int)(info.Height * SoApplication.Yprop);
            h = info.Height;
        }

        public event KeyPressedEventHandler OnProcessHotKey;
        public int w { get; private set; }
        public int h { get; private set; }
    }
}