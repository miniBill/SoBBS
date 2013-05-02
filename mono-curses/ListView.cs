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

	/// <summary>
	///   A Listview widget.
	/// </summary>
	/// <remarks>
	///   This widget renders a list of data.   The actual
	///   rendering is implemented by an instance of the class
	///   IListProvider that must be supplied at construction time.
	/// </remarks>
	public class ListView : Widget {
		int items;
		int top;
		int selected;
		bool allow_mark;
		IListProvider provider;
		
		/// <summary>
		///   Public constructor.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public ListView (int x, int y, int w, int h, IListProvider provider) : base (x, y, w, h)
		{
			CanFocus = true;

			this.provider = provider;
			provider.SetListView (this);
			items = provider.Items;
			allow_mark = provider.AllowMark;
		}
		
		/// <summary>
		///   This method can be invoked by the model to
		///   notify the view that the contents of the model
		///   have changed.
		/// </summary>
		/// <remarks>
		///   Invoke this method to invalidate the contents of
		///   the ListView and force the ListView to repaint
		///   the contents displayed.
		/// </remarks>
		public void ProviderChanged ()
		{
			if (provider.Items != items){
				items = provider.Items;
				if (top > items){
					if (items > 1)
						top = items-1;
					else
						top = 0;
				}
				if (selected > items){
					if (items > 1)
						selected = items - 1;
					else
						selected = 0;
				}
			}
			Redraw ();
		}

		void SelectedChanged ()
		{
			provider.SelectedChanged ();
		}
		
		public override bool ProcessKey (int c)
		{
			int n;

			switch (c){
			case 16: // Control-p
			case Curses.KeyUp:
				if (selected > 0){
					selected--;
					if (selected < top)
						top = selected;
					SelectedChanged ();
					Redraw ();
					return true;
				} else
					return false;

			case 14: // Control-n
			case Curses.KeyDown:
				if (selected+1 < items){
					selected++;
					if (selected >= top + h){
						top++;
					}
					SelectedChanged ();
					Redraw ();
					return true;
				} else 
					return false;

			case 22: //  Control-v
			case Curses.KeyNPage:
				n = (selected + h);
				if (n > items)
					n = items-1;
				if (n != selected){
					selected = n;
					if (items >= h)
						top = selected;
					else
						top = 0;
					SelectedChanged ();
					Redraw ();
				}
				return true;
				
			case Curses.KeyPPage:
				n = (selected - h);
				if (n < 0)
					n = 0;
				if (n != selected){
					selected = n;
					top = selected;
					SelectedChanged ();
					Redraw ();
				}
				return true;

			default:
				return provider.ProcessKey (c);
			}
		}

		public override void PositionCursor ()
		{
			Move (y + (selected-top), x);
		}

		public override void Redraw ()
		{
			for (int l = 0; l < h; l++){
				Move (y + l, x);
				int item = l + top;

				if (item >= items){
					Curses.attrset (ColorNormal);
					for (int c = 0; c < w; c++)
						Curses.addch (' ');
					continue;
				}

				bool marked = allow_mark ? provider.IsMarked (item) : false;

				if (item == selected){
					if (marked)
						Curses.attrset (ColorHotNormal);
					else
						Curses.attrset (ColorFocus);
				} else {
					if (marked)
						Curses.attrset (ColorHotFocus);
					else
						Curses.attrset (ColorNormal);
				}
				provider.Render (y + l, x, w, item);
			}
			PositionCursor ();
		}

		/// <summary>
		///   Returns the index of the currently selected item.
		/// </summary>
		public int Selected {
			get {
				if (items == 0)
					return -1;
				return selected;
			}

			set {
				if (value >= items)
					throw new ArgumentException ("value");

				selected = value;
				Redraw ();
			}
		}
		
		public override void ProcessMouse (Curses.MouseEvent ev)
		{
			if ((ev.ButtonState & Curses.Event.Button1Clicked) == 0)
				return;

			ev.X -= x;
			ev.Y -= y;

			if (ev.Y < 0)
				return;
			if (ev.Y+top >= items)
				return;
			selected = ev.Y - top;
			SelectedChanged ();
			
			Redraw ();
		}
	}
}
