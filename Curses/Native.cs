using System;
using System.Runtime.InteropServices;

namespace MinCurses
{
    internal static class Native
    {
        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr initscr();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int curs_set(int visibility);

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int noecho();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int refresh();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int endwin();

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mvinswch(int y, int x, uint character, short colorPair);

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern int mvaddwch(int y, int x, uint character, short colorPair);

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool has_colors();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int wgetch(IntPtr window);

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int start_color();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int init_pair(short index, short fg, short bg);

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int echo();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int nocbreak();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int cbreak();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int erase();

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_lines();

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_cols();

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_colors();

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern int get_color_pairs();

        [DllImport("pdcurses", CallingConvention = CallingConvention.Cdecl)]
        public static extern int attron(uint attribute);

        [DllImport("Wrapper", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint color_pair(short index);
    }
}
