using System;
using System.Drawing;
using System.Windows.Forms;

namespace Sobbs.Cui.Forms
{
    public class FormFrame : FormContainer, IFrame
    {
        public string Title { get; private set; }

        public FormFrame(FrameInfo info)
            : base(info)
        {
            Title = info.Title;
            //  -16777216 = 0xff000000
            BackColor = Color.FromArgb(-16777216 | Title.GetHashCode());
            var titleLabel = new Label
                {
                    Text = Title,
                    Top = (int)(SoApplication.Yprop * 0.2),
                    Left = (int)(SoApplication.Xprop * 0.2),
                    Height = (int)(SoApplication.Yprop * 0.8)
                };
            Controls.Add(titleLabel);
        }

        public event EventHandler OnUpdate;
    }
}