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
	///   A Dialog is a container that can also have a number of
	///   buttons at the bottom
	/// </summary>
	/// <remarks>
	///   <para>Dialogs are containers that can have a set of buttons at
	///   the bottom.   Dialogs are automatically centered on the
	///   screen, and on screen changes the buttons are
	///   relaid out.</para>
	/// <para>
	///   To make the dialog box run until an option has been
	///   executed, you would typically create the dialog box and
	///   then call Application.Run on the Dialog instance.
	/// </para>
	/// </remarks>
	public class Dialog : Frame {
		int button_len;
		List<Button> buttons;

		const int button_space = 3;
		
		/// <summary>
		///   Public constructor.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public Dialog (int w, int h, string title)
			: base ((Application.Cols - w) / 2, (Application.Lines-h)/3, w, h, title)
		{
			ContainerColorNormal = Application.ColorDialogNormal;
			ContainerColorFocus = Application.ColorDialogFocus;
			ContainerColorHotNormal = Application.ColorDialogHotNormal;
			ContainerColorHotFocus = Application.ColorDialogHotFocus;

			Border++;
		}

		/// <summary>
		///   Makes the default style for the dialog use the error colors.
		/// </summary>
		public void ErrorColors ()
		{
			ContainerColorNormal = Application.ColorError;
			ContainerColorFocus = Application.ColorErrorFocus;
			ContainerColorHotFocus = Application.ColorErrorHotFocus;
			ContainerColorHotNormal = Application.ColorErrorHot;
		}
		
		public override void Prepare ()
		{
			LayoutButtons ();
		}

		void LayoutButtons ()
		{
			if (buttons == null)
				return;
			
			int p = (w - button_len) / 2;
			
			foreach (Button b in buttons){
				b.x = p;
				b.y = h - 5;

				p += b.w + button_space;
			}
		}

		/// <summary>
		///   Adds a button to the dialog
		/// </summary>
		/// <remarks>
		/// </remarks>
		public void AddButton (Button b)
		{
			if (buttons == null)
				buttons = new List<Button> ();
			
			buttons.Add (b);
			button_len += b.w + button_space;

			Add (b);
		}
		
		public override void GetBase (out int row, out int col)
		{
			base.GetBase (out row, out col);
			row++;
			col++;
		}
		
		public override void ContainerMove (int row, int col)
		{
			base.ContainerMove (row + 1, col + 1);
		}

		public override void Redraw ()
		{
			Curses.attrset (ContainerColorNormal);
			Clear ();

			Widget.DrawFrame (x + 1, y + 1, w - 2, h - 2);
			Curses.move (y + 1, x + (w - Title.Length) / 2);
			Curses.addch (' ');
			Curses.attrset (Application.ColorDialogHotNormal);
			Curses.addstr (Title);
			Curses.addch (' ');
			RedrawChildren ();
		}

		public override bool ProcessKey (int key)
		{
			if (key == 27){
				Running = false;
				return true;
			}

			return base.ProcessKey (key);
		}

		public override void DoSizeChanged ()
		{
			base.DoSizeChanged ();
			
			x = (Application.Cols - w) / 2;
			y = (Application.Lines-h) / 3;

			LayoutButtons ();
		}
	}
}
