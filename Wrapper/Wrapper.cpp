extern "C"
{
#ifdef __linux__
#include <ncursesw/curses.h>
#define EXT extern
#else
#include "curses.h"
#define EXT _declspec(dllexport)
#endif
#include <stdio.h>
#include <locale.h>

	EXT int get_lines()
	{
		return LINES;
	}

	EXT int get_cols()
	{
		return COLS;
	}

	EXT int get_colors()
	{
		return COLORS;
	}

	EXT int get_color_pairs()
	{
		return COLOR_PAIRS;
	}

	EXT unsigned int color_pair(short index)
	{
		return COLOR_PAIR(index);
	}

	cchar_t convert(unsigned int ch, short color_pair)
	{
		cchar_t toret;
		wchar_t container[2];
		container[0] = ch;
		container[1] = 0;
		setcchar(&toret, container, 0, color_pair, NULL);
		return toret;
	}

	EXT int mvaddwch(int y, int x, unsigned int ch, short color_pair)
	{
		cchar_t toprint = convert(ch, color_pair);
		return mvadd_wch(y, x, &toprint);
	}

	EXT int mvinswch(int y, int x, unsigned int ch, short color_pair)
	{
		cchar_t toprint = convert(ch, color_pair);
		return mvins_wch(y, x, &toprint);
	}
}
