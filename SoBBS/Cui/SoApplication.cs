#if __MONO_CS__

#else
using System.Windows.Forms;
using Sobbs.Cui.Forms;
#endif
/*
namespace Sobbs.Cui
{
    public static class SoApplication
    {
#if __MONO_CS__
        public static void Run<T>(CursesContainer<T> container) where T : Container
#else
        public static void Run(FormWidget container)
#endif
        {
#if __MONO_CS__
            Application.Run(container.Implementation);
#else

            Form.Controls.Add(container);
            Application.Run(Form);
#endif
        }

#if !__MONO_CS__
        private readonly static Form Form;
#endif

        static SoApplication()
        {
#if !__MONO_CS__
            Form = new Form { Width = Width, Height = Height };
#endif
        }

#if __MONO_CS__
        public static int Cols
        {
            get { return Application.Cols; }
        }

        public static int Lines
        {
            get { return Application.Lines; }
        }

        public const double Xprop = 0;
        public const double Yprop = 0;
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
            Application.Init(false);
#else
#endif
        }
    }
}
*/