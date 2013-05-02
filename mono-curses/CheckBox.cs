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

namespace Mono.Terminal
{
    public class CheckBox : Widget
    {
        string text;
        int hot_pos = -1;
        char hot_key;
        
        /// <summary>
        ///   Toggled event, raised when the CheckButton is toggled.
        /// </summary>
        /// <remarks>
        ///   Client code can hook up to this event, it is
        ///   raised when the checkbutton is activated either with
        ///   the mouse or the keyboard.
        /// </remarks>
        public event EventHandler Toggled;

        /// <summary>
        ///   Public constructor, creates a CheckButton based on
        ///   the given text at the given position.
        /// </summary>
        /// <remarks>
        ///   The size of CheckButton is computed based on the
        ///   text length. This CheckButton is not toggled.
        /// </remarks>
        public CheckBox(int x, int y, string s) : this (x, y, s, false)
        {
        }

        /// <summary>
        ///   Public constructor, creates a CheckButton based on
        ///   the given text at the given position and a state.
        /// </summary>
        /// <remarks>
        ///   The size of CheckButton is computed based on the
        ///   text length. 
        /// </remarks>
        public CheckBox(int x, int y, string s, bool is_checked) : base (x, y, s.Length + 4, 1)
        {
            Checked = is_checked;
            Text = s;

            CanFocus = true;
        }

        /// <summary>
        ///    The state of the checkbox.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        ///   The text displayed by this widget.
        /// </summary>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;

                int i = 0;
                hot_pos = -1;
                hot_key = (char)0;
                foreach (char c in text)
                {
                    if (Char.IsUpper(c))
                    {
                        hot_key = c;
                        hot_pos = i;
                        break;
                    }
                    i++;
                }
            }
        }
        
        public override void Redraw()
        {
            Curses.attrset(ColorNormal);
            Move(y, x);
            Curses.addstr(Checked ? "[X] " : "[ ] ");
            Curses.attrset(HasFocus ? ColorFocus : ColorNormal);
            Move(y, x + 3);
            Curses.addstr(Text);
            if (hot_pos != -1)
            {
                Move(y, x + 3 + hot_pos);
                Curses.attrset(HasFocus ? ColorHotFocus : ColorHotNormal);
                Curses.addch(hot_key);
            }
            PositionCursor();
        }

        public override void PositionCursor()
        {
            Move(y, x + 1);
        }

        public override bool ProcessKey(int c)
        {
            if (c == ' ')
            {
                Checked = !Checked;

                if (Toggled != null)
                    Toggled(this, EventArgs.Empty);

                Redraw();
                return true;
            }
            return false;
        }

        public override void ProcessMouse(Curses.MouseEvent ev)
        {
            if ((ev.ButtonState & Curses.Event.Button1Clicked) != 0)
            {
                Container.SetFocus(this);
                Container.Redraw();

                Checked = !Checked;
                
                if (Toggled != null)
                    Toggled(this, EventArgs.Empty);
                Redraw();
            }
        }
    }
}
