#if !__MONO_CS__
using System.Windows.Forms;
using Sobbs.Cui.Forms;

#endif

namespace Sobbs.Cui
{
    public static class SoApplication
    {
#if __MONO_CS__
        public static void Run(Container container)
#else
        public static void Run(FormWidget container)
#endif
        {
#if __MONO_CS__
            Application.Run(container);
#else

            Form.Controls.Add(container);
            Application.Run(Form);
#endif
        }

        private readonly static Form Form;

        static SoApplication()
        {
            Form = new Form { Width = Width, Height = Height };
        }

#if __MONO_CS__
        public int Cols
        {
            get { return Application.Cols; }
        }

        public int Lines
        {
            get { return Application.Lines; }
        }
#else
        private const int Width = 800;
        private const int Height = 600;

        public const int Cols = 80;
        public const int Lines = 25;

        public const double Xprop = Width / Cols;
        public const double Yprop = Height / Lines;
#endif

        public static void Init()
        {
#if __MONO_CS__
            Application.Init();
#else
#endif
        }
    }
}