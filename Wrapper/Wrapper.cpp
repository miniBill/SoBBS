#include "curses.h"

extern "C"
{
	PDCEX int get_lines()
	{
		return LINES;
	}

	PDCEX int get_cols()
	{
		return COLS;
	}
}
