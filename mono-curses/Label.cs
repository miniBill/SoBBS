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
	///   Label widget, displays a string at a given position.
	/// </summary>
	public class Label : Widget {
		protected string text;
		public int Color = -1;
		
		/// <summary>
		///   Public constructor: creates a label at the given
		///   coordinate with the given string.
		/// </summary>
		public Label (int x, int y, string s) : base (x, y, s.Length, 1)
		{
			text = s;
		}

		public Label (int x, int y, string s, params object [] args) : base (x, y, String.Format (s, args).Length, 1)
		{
			text = String.Format (s, args);
		}
		
		public override void Redraw ()
		{
			if (Color != -1)
				Curses.attrset (Color);
			else
				Curses.attrset (ColorNormal);

			Move (y, x);
			Curses.addstr (text);
		}

		/// <summary>
		///   The text displayed by this widget.
		/// </summary>
		public virtual string Text {
			get {
				return text;
			}
			set {
				Curses.attrset (ColorNormal);
				Move (y, x);
				for (int i = 0; i < text.Length; i++)
					Curses.addch (' ');
				text = value;
				Redraw ();
			}
		}
	}
}
