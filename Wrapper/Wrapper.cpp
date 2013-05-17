#include "curses.h"
#include <iostream>

using namespace std;

extern "C"
{

#ifdef __linux__
#define EXT extern
#else
#define EXT _declspec(dllexport)
#endif

	EXT int get_lines()
	{
		return LINES;
	}

	EXT int get_cols()
	{
		return COLS;
	}

	EXT int wrap_mvinsch(int y, int x, unsigned int character)
	{
		cerr << "mvinsch(" << y << ", " << x << ", " << character << "(" << (char)character << "))\n";
		return mvinsch(y, x, character);
	}

	EXT int wrap_mvaddch(int y, int x, unsigned int character)
	{
		cerr << "mvaddch(" << y << ", " << x << ", " << character << "(" << (char)character << "))\n";
		return mvaddch(y, x, character);
	}
}
