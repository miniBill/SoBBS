#include "curses.h"

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
}
