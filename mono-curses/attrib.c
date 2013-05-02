//
// attrib.c: Generates some C-level glue
//
// Authors:
//   Miguel de Icaza (miguel.de.icaza@gmail.com)
//
// Copyright (C) 2007 Novell (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
#include <stdio.h>
#include <ncurses.h>

#define put(x) fprintf (OUT, "\tpublic const int " #x " = unchecked((int)0x%x);\n", x)
#define pute(k,x) fprintf (OUT, "\t\t%s = unchecked((int)0x%x),\n", k, x)
#define put2(s,x) fprintf (OUT, "\tpublic const int Key%s = unchecked((int)0x%x);\n", s, x)

int
main (int argc, char *argv [])
{
	FILE *OUT = fopen (argv [1], "w");
	
	int diff;

	initscr ();
	fprintf (OUT, "/*\n * This file is autogenerated by the attrib.c program, do not edit \n */\n\n");
	fprintf (OUT, "using System;\n\n"
		"namespace Mono.Terminal {\n"
		"public partial class Curses {\n");
	put (A_NORMAL);
	put (A_STANDOUT);
	put (A_UNDERLINE);
	put (A_REVERSE);
	put (A_BLINK);
	put (A_DIM);
	put (A_BOLD);
	put (A_PROTECT);
	put (A_INVIS);

	put (ACS_LLCORNER);
	put (ACS_LRCORNER);
	put (ACS_HLINE);
	put (ACS_ULCORNER);
	put (ACS_URCORNER);
	put (ACS_VLINE);

	put (COLOR_BLACK);
	put (COLOR_RED);
	put (COLOR_GREEN);
	put (COLOR_YELLOW);
	put (COLOR_BLUE);
	put (COLOR_MAGENTA);
	put (COLOR_CYAN);
	put (COLOR_WHITE);

	fprintf (OUT, "public enum Event : long {\n");
	pute ("Button1Pressed", BUTTON1_PRESSED);
	pute ("Button1Released", BUTTON1_RELEASED);
	pute ("Button1Clicked", BUTTON1_CLICKED);
	pute ("Button1DoubleClicked", BUTTON1_DOUBLE_CLICKED);
	pute ("Button1TripleClicked", BUTTON1_TRIPLE_CLICKED);
	pute ("Button2Pressed", BUTTON2_PRESSED);
	pute ("Button2Released", BUTTON2_RELEASED);
	pute ("Button2Clicked", BUTTON2_CLICKED);
	pute ("Button2DoubleClicked", BUTTON2_DOUBLE_CLICKED);
	pute ("Button2TrippleClicked", BUTTON2_TRIPLE_CLICKED);
	pute ("Button3Pressed", BUTTON3_PRESSED);
	pute ("Button3Released", BUTTON3_RELEASED);
	pute ("Button3Clicked", BUTTON3_CLICKED);
	pute ("Button3DoubleClicked", BUTTON3_DOUBLE_CLICKED);
	pute ("Button3TripleClicked", BUTTON3_TRIPLE_CLICKED);
	pute ("Button4Pressed", BUTTON4_PRESSED);
	pute ("Button4Released", BUTTON4_RELEASED);
	pute ("Button4Clicked", BUTTON4_CLICKED);
	pute ("Button4DoubleClicked", BUTTON4_DOUBLE_CLICKED);
	pute ("Button4TripleClicked", BUTTON4_TRIPLE_CLICKED);
	pute ("ButtonShift", BUTTON_SHIFT);
	pute ("ButtonCtrl", BUTTON_CTRL);
	pute ("ButtonAlt", BUTTON_ALT);
	pute ("ReportMousePosition", REPORT_MOUSE_POSITION);
	pute ("AllEvents", ALL_MOUSE_EVENTS);
	fprintf (OUT, "}\n");
	put (ERR);

	put2 ("Backspace", KEY_BACKSPACE);
	put2 ("Up",    KEY_UP);
	put2 ("Down",  KEY_DOWN);
	put2 ("Left",  KEY_LEFT);
	put2 ("Right",  KEY_RIGHT);
	put2 ("NPage", KEY_NPAGE);
	put2 ("PPage", KEY_PPAGE);
	put2 ("Home",  KEY_HOME);
	put2 ("Mouse", KEY_MOUSE);
	put2 ("End",   KEY_END);
	put2 ("DeleteChar", KEY_DC);
	put2 ("InsertChar", KEY_IC);
	put2 ("F1", KEY_F(1));
	put2 ("F2", KEY_F(2));
	put2 ("F3", KEY_F(3));
	put2 ("F4", KEY_F(4));
	put2 ("F5", KEY_F(5));
	put2 ("F6", KEY_F(6));
	put2 ("F7", KEY_F(7));
	put2 ("F8", KEY_F(8));
	put2 ("F9", KEY_F(9));
	put2 ("F10", KEY_F(10));
	put2 ("Resize", KEY_RESIZE);
	
	diff = COLOR_PAIR (1) - COLOR_PAIR(0);
	fprintf (OUT, "\n\n\tstatic public int ColorPair(int n){\n"
		"\t\treturn %d + n * %d;\n"
		"\t}\n\n", COLOR_PAIR (0), diff);
	fprintf (OUT,"}\n}\n");
	fclose (OUT);
	endwin ();
	return 0;
}