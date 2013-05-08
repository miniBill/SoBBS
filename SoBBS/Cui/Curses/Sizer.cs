using Sobbs.Config.Sizes;

namespace Sobbs.Cui.Curses
{
    public static class Sizer
    {
        public static int X(Size x, int pwidth)
        {
            return Project(x, pwidth);
        }

        private static int Project(Size size, int cols)
        {
            return size.Either(i => i, percent => (int)(percent.Value * cols / 100.0), star => cols);
        }

        public static int X(Size x, Size width, int pwidth)
        {
            var xproj = Project(x, pwidth);
            return width.Either(i => xproj + i, percent => xproj + (int)(percent.Value * pwidth / 100.0), star => pwidth) - 1;
        }

        public static int Y(Size y, int pheight)
        {
            return Project(y, pheight);
        }

        public static int Y(Size y, Size height, int pheight)
        {
            var yproj = Project(y, pheight);
            return height.Either(i => yproj + i, percent => yproj + (int)(percent.Value * pheight / 100.0), star => pheight) - 1;
        }
    }
}