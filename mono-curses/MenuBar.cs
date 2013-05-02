//
// gui.cs: Simple curses-based GUI toolkit, core
//
// Authors:
//   Miguel de Icaza (miguel.de.icaza@gmail.com)
//
// Copyright (C) 2007-2011 Novell (http://www.novell.com)
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

using System;
using System.Collections;
using System.Collections.Generic;
using Mono;
using System.IO;

namespace Mono.Terminal {

	public class MenuBar : Container {
		public MenuBarItem [] Menus { get; set; }
		int selected;
		Action action;
		
		public MenuBar (MenuBarItem [] menus) : base (0, 0, Application.Cols, 1)
		{
			Menus = menus;
			CanFocus = false;
			selected = -1;
		}

		/// <summary>
		///   Activates the menubar
		/// </summary>
		public void Activate (int idx)
		{
			if (idx < 0 || idx > Menus.Length)
				throw new ArgumentException ("idx");

			action = null;
			selected = idx;

			foreach (var m in Menus)
				m.Current = 0;
			
			Application.Run (this);
			selected = -1;
			Container.Redraw ();
			
			if (action != null)
				action ();
		}

		void DrawMenu (int idx, int col, int line)
		{
			int max = 0;
			var menu = Menus [idx];

			if (menu.Children == null)
				return;

			foreach (var m in menu.Children){
				if (m == null)
					continue;
				
				if (m.Width > max)
					max = m.Width;
			}
			max += 4;
			DrawFrame (col + x, line, max, menu.Children.Length + 2, true);
			for (int i = 0; i < menu.Children.Length; i++){
				var item = menu.Children [i];

				Move (line + 1 + i, col + 1);
				Curses.attrset (item == null ? Application.ColorFocus : i == menu.Current ? Application.ColorMenuSelected : Application.ColorMenu);
				for (int p = 0; p < max - 2; p++)
					Curses.addch (item == null ? Curses.ACS_HLINE : ' ');

				if (item == null)
					continue;
				
				Move (line + 1 + i, col + 2);
				DrawHotString (item.Title,
					       i == menu.Current ? Application.ColorMenuHotSelected : Application.ColorMenuHot,
					       i == menu.Current ? Application.ColorMenuSelected : Application.ColorMenu);

				// The help string
				var l = item.Help.Length;
				Move (line + 1 + i, col + x + max - l - 2);
				Curses.addstr (item.Help);
			}
		}
		
		public override void Redraw ()
		{
			Move (y, 0);
			Curses.attrset (Application.ColorFocus);
			for (int i = 0; i < Application.Cols; i++)
				Curses.addch (' ');

			Move (y, 1);
			int pos = 0;
			for (int i = 0; i < Menus.Length; i++){
				var menu = Menus [i];
				if (i == selected){
					DrawMenu (i, pos, y+1);
					Curses.attrset (Application.ColorMenuSelected);
				} else
					Curses.attrset (Application.ColorFocus);

				Move (y, pos);
				Curses.addch (' ');
				Curses.addstr (menu.Title);
				Curses.addch (' ');
				if (HasFocus && i == selected)
					Curses.attrset (Application.ColorMenuSelected);
				else
					Curses.attrset (Application.ColorFocus);
				Curses.addstr ("  ");
				
				pos += menu.Title.Length + 4;
			}
			PositionCursor ();
		}

		public override void PositionCursor ()
		{
			int pos = 0;
			for (int i = 0; i < Menus.Length; i++){
				if (i == selected){
					pos++;
					Move (y, pos);
					return;
				} else {
					pos += Menus [i].Title.Length + 4;
				}
			}
			Move (y, 0);
		}

		void Selected (MenuItem item)
		{
			Running = false;
			action = item.Action;
		}
		
		public override bool ProcessKey (int key)
		{
			switch (key){
			case Curses.KeyUp:
				if (Menus [selected].Children == null)
					return false;

				int current = Menus [selected].Current;
				do {
					current--;
					if (current < 0)
						current = Menus [selected].Children.Length-1;
				} while (Menus [selected].Children [current] == null);
				Menus [selected].Current = current;
				
				Redraw ();
				Curses.refresh ();
				return true;
				
			case Curses.KeyDown:
				if (Menus [selected].Children == null)
					return false;

				do {
					Menus [selected].Current = (Menus [selected].Current+1) % Menus [selected].Children.Length;
				} while (Menus [selected].Children [Menus [selected].Current] == null);
				
				Redraw ();
				Curses.refresh ();
				break;
				
			case Curses.KeyLeft:
				selected--;
				if (selected < 0)
					selected = Menus.Length-1;
				break;
			case Curses.KeyRight:
				selected = (selected + 1) % Menus.Length;
				break;

			case '\n':
				if (Menus [selected].Children == null)
					return false;

				Selected (Menus [selected].Children [Menus [selected].Current]);
				break;

			case 27:
			case 3:
				Running = false;
				break;

			default:
				if ((key >= 'a' && key <= 'z') || (key >= 'A' && key <= 'Z') || (key >= '0' && key <= '9')){
					char c = Char.ToUpper ((char)key);

					if (Menus [selected].Children == null)
						return false;
					
					foreach (var mi in Menus [selected].Children){
						int p = mi.Title.IndexOf ('_');
						if (p != -1 && p+1 < mi.Title.Length){
							if (mi.Title [p+1] == c){
								Selected (mi);
								return true;
							}
						}
					}
				}
				    
				return false;
			}
			Container.Redraw ();
			Curses.refresh ();
			return true;
		}
	}
}
