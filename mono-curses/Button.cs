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
	///   Button widget
	/// </summary>
	/// <remarks>
	///   Provides a button that can be clicked, or pressed with
	///   the enter key and processes hotkeys (the first uppercase
	///   letter in the button becomes the hotkey).
	/// </remarks>
	public class Button : Widget {
		string text;
		string shown_text;
		char hot_key;
		int  hot_pos = -1;
		bool is_default;
		
		/// <summary>
		///   Clicked event, raised when the button is clicked.
		/// </summary>
		/// <remarks>
		///   Client code can hook up to this event, it is
		///   raised when the button is activated either with
		///   the mouse or the keyboard.
		/// </remarks>
		public event EventHandler Clicked;

		/// <summary>
		///   Public constructor, creates a button based on
		///   the given text at position 0,0
		/// </summary>
		/// <remarks>
		///   The size of the button is computed based on the
		///   text length.   This button is not a default button.
		/// </remarks>
		public Button (string s) : this (0, 0, s) {}
		
		/// <summary>
		///   Public constructor, creates a button based on
		///   the given text.
		/// </summary>
		/// <remarks>
		///   If the value for is_default is true, a special
		///   decoration is used, and the enter key on a
		///   dialog would implicitly activate this button.
		/// </remarks>
		public Button (string s, bool is_default) : this (0, 0, s, is_default) {}
		
		/// <summary>
		///   Public constructor, creates a button based on
		///   the given text at the given position.
		/// </summary>
		/// <remarks>
		///   The size of the button is computed based on the
		///   text length.   This button is not a default button.
		/// </remarks>
		public Button (int x, int y, string s) : this (x, y, s, false) {}
		
		/// <summary>
		///   The text displayed by this widget.
		/// </summary>
		public string Text {
			get {
				return text;
			}

			set {
				text = value;
				if (is_default)
					shown_text = "[< " + value + " >]";
				else
					shown_text = "[ " + value + " ]";

				hot_pos = -1;
				hot_key = (char) 0;
				int i = 0;
				foreach (char c in shown_text){
					if (Char.IsUpper (c)){
						hot_key = c;
						hot_pos = i;
						break;
					}
					i++;
				}
			}
		}

		/// <summary>
		///   Public constructor, creates a button based on
		///   the given text at the given position.
		/// </summary>
		/// <remarks>
		///   If the value for is_default is true, a special
		///   decoration is used, and the enter key on a
		///   dialog would implicitly activate this button.
		/// </remarks>
		public Button (int x, int y, string s, bool is_default)
			: base (x, y, s.Length + 4 + (is_default ? 2 : 0), 1)
		{
			CanFocus = true;

			this.is_default = is_default;
			Text = s;
		}

		public override void Redraw ()
		{
			Curses.attrset (HasFocus ? ColorFocus : ColorNormal);
			Move (y, x);
			Curses.addstr (shown_text);

			if (hot_pos != -1){
				Move (y, x + hot_pos);
				Curses.attrset (HasFocus ? ColorHotFocus : ColorHotNormal);
				Curses.addch (hot_key);
			}
		}

		public override void PositionCursor ()
		{
			Move (y, x + hot_pos);
		}

		bool CheckKey (int key)
		{
			if (Char.ToUpper ((char)key) == hot_key){
				Container.SetFocus (this);
				if (Clicked != null)
					Clicked (this, EventArgs.Empty);
				return true;
			}
			return false;
		}
			
		public override bool ProcessHotKey (int key)
		{
			int k = Curses.IsAlt (key);
			if (k != 0)
				return CheckKey (k);

			return false;
		}

		public override bool ProcessColdKey (int key)
		{
			if (is_default && key == '\n'){
				if (Clicked != null)
					Clicked (this, EventArgs.Empty);
				return true;
			}
			return CheckKey (key);
		}

		public override bool ProcessKey (int c)
		{
			if (c == '\n' || c == ' ' || Char.ToUpper ((char)c) == hot_key){
				if (Clicked != null)
					Clicked (this, EventArgs.Empty);
				return true;
			}
			return false;
		}

		public override void ProcessMouse (Curses.MouseEvent ev)
		{
			if ((ev.ButtonState & Curses.Event.Button1Clicked) != 0){
				Container.SetFocus (this);
				Container.Redraw ();
				if (Clicked != null)
					Clicked (this, EventArgs.Empty);
			}
		}
	}
}
