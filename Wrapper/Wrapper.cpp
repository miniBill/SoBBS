#include "curses.h"

extern "C"
{
	_declspec(dllexport) int get_lines()
	{
		return LINES;
	}

	_declspec(dllexport) int get_cols()
	{
		return COLS;
	}
}