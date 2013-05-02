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
	///   Framed-container widget.
	/// </summary>
	/// <remarks>
	///   A container that provides a frame around its children,
	///   and an optional title.
	/// </remarks>
	public class Frame : Container {
		public string Title;

		/// <summary>
		///   Creates an empty frame, with the given title
		/// </summary>
		/// <remarks>
		/// </remarks>
		public Frame (string title) : this (0, 0, 0, 0, title)
		{
		}
		
		/// <summary>
		///   Public constructor, a frame, with the given title.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public Frame (int x, int y, int w, int h, string title) : base (x, y, w, h)
		{
			Title = title;
			Border++;
		}

		public override void GetBase (out int row, out int col)
		{
			row = 1;
			col = 1;
		}
		
		public override void ContainerMove (int row, int col)
		{
			base.ContainerMove (row + 1, col + 1);
		}

		public override void Redraw ()
		{
			Curses.attrset (ContainerColorNormal);
			Clear ();
			
			Widget.DrawFrame (x, y, w, h);
			Curses.attrset (Container.ContainerColorNormal);
			Curses.move (y, x + 1);
			if (HasFocus)
				Curses.attrset (Application.ColorDialogNormal);
			if (Title != null){
				Curses.addch (' ');
				Curses.addstr (Title);
				Curses.addch (' ');
			}
			RedrawChildren ();
		}

		public override void Add (Widget w)
		{
			base.Add (w);
		}
	}
}
