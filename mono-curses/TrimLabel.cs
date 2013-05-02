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
	///   A label that can be trimmed to a given position
	/// </summary>
	/// <remarks>
	///   Just like a label, but it can be trimmed to a given
	///   position if the text being displayed overflows the
	///   specified width. 
	/// </remarks>
	public class TrimLabel : Label {
		string original;
		
		/// <summary>
		///   Public constructor.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public TrimLabel (int x, int y, int w, string s) : base (x, y, s)
		{
			original = s;

			SetString (w, s);
		}

		void SetString (int w, string s)
		{
			if ((Fill & Fill.Horizontal) != 0)
				w = Container.w - Container.Border * 2 - x;
			
			this.w = w;
			if (s.Length > w){
				if (w < 5)
					text = s.Substring (0, w);
				else {
					text = s.Substring (0, w/2-2) + "..." + s.Substring (s.Length - w/2+1);
				}
			} else
				text = s;
		}

		public override void DoSizeChanged ()
		{
			if ((Fill & Fill.Horizontal) != 0)
				SetString (0, original);
		}

		/// <summary>
		///   The text displayed by this widget.
		/// </summary>
		public override string Text {
			get {
				return original;
			}

			set {
				SetString (w, value);
				base.Text = text;
			}
		}
	}
}
