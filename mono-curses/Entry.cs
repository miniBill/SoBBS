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
    
    /// <summary>
    ///   Text data entry widget
    /// </summary>
    /// <remarks>
    ///   The Entry widget provides Emacs-like editing
    ///   functionality,  and mouse support.
    /// </remarks>
    public class Entry : Widget
    {
        string text, kill;
        int first, point;
        int color;
        bool used;
        
        /// <summary>
        ///   Changed event, raised when the text has clicked.
        /// </summary>
        /// <remarks>
        ///   Client code can hook up to this event, it is
        ///   raised when the text in the entry changes.
        /// </remarks>
        public event EventHandler Changed;
        
        /// <summary>
        ///   Public constructor.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Entry(int x, int y, int w, string s) : base (x, y, w, 1)
        {
            if (s == null)
                s = "";
            
            text = s;
            point = s.Length;
            first = point > w ? point - w : 0;
            CanFocus = true;
            Color = Application.ColorDialogFocus;
        }

        /// <summary>
        ///   Sets or gets the text in the entry.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                if (point > text.Length)
                    point = text.Length;
                first = point > w ? point - w : 0;
                Redraw();
            }
        }

        /// <summary>
        ///   Sets the secret property.
        /// </summary>
        /// <remarks>
        ///   This makes the text entry suitable for entering passwords. 
        /// </remarks>
        public bool Secret { get; set; }

        /// <summary>
        ///    The color used to display the text
        /// </summary>
        public int Color
        {
            get { return color; }
            set { color = value;
                Container.Redraw(); }
        }

        /// <summary>
        ///    The current cursor position.
        /// </summary>
        public int CursorPosition { get { return point; } }
        
        /// <summary>
        ///   Sets the cursor position.
        /// </summary>
        public override void PositionCursor()
        {
            Move(y, x + point - first);
        }
        
        public override void Redraw()
        {
            Curses.attrset(Color);
            Move(y, x);
            
            for (int i = 0; i < w; i++)
            {
                int p = first + i;

                if (p < text.Length)
                {
                    Curses.addch(Secret ? '*' : text [p]);
                }
                else
                    Curses.addch(' ');
            }
            PositionCursor();
        }

        void Adjust()
        {
            if (point < first)
                first = point;
            else if (first + point >= w)
                first = point - (w / 3);
            Redraw();
            Curses.refresh();
        }

        void SetText(string new_text)
        {
            text = new_text;
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }
        
        public override bool ProcessKey(int key)
        {
            switch (key)
            {
                case 127:
                case Curses.KeyBackspace:
                    if (point == 0)
                        return true;
                
                    SetText(text.Substring(0, point - 1) + text.Substring(point));
                    point--;
                    Adjust();
                    break;

                case Curses.KeyHome:
                case 1: // Control-a, Home
                    point = 0;
                    Adjust();
                    break;

                case Curses.KeyLeft:
                case 2: // Control-b, back character
                    if (point > 0)
                    {
                        point--;
                        Adjust();
                    }
                    break;

                case 4: // Control-d, Delete
                    if (point == text.Length)
                        break;
                    SetText(text.Substring(0, point) + text.Substring(point + 1));
                    Adjust();
                    break;
                
                case 5: // Control-e, End
                    point = text.Length;
                    Adjust();
                    break;

                case Curses.KeyRight:
                case 6: // Control-f, forward char
                    if (point == text.Length)
                        break;
                    point++;
                    Adjust();
                    break;

                case 11: // Control-k, kill-to-end
                    kill = text.Substring(point);
                    SetText(text.Substring(0, point));
                    Adjust();
                    break;

                case 25: // Control-y, yank
                    if (kill == null)
                        return true;
                
                    if (point == text.Length)
                    {
                        SetText(text + kill);
                        point = text.Length;
                    }
                    else
                    {
                        SetText(text.Substring(0, point) + kill + text.Substring(point));
                        point += kill.Length;
                    }
                    Adjust();
                    break;

                case (int) 'b' + Curses.KeyAlt:
                    int bw = WordBackward(point);
                    if (bw != -1)
                        point = bw;
                    Adjust();
                    break;

                case (int) 'f' + Curses.KeyAlt:
                    int fw = WordForward(point);
                    if (fw != -1)
                        point = fw;
                    Adjust();
                    break;
            
                default:
                // Ignore other control characters.
                    if (key < 32 || key > 255)
                        return false;

                    if (used)
                    {
                        if (point == text.Length)
                        {
                            SetText(text + (char)key);
                        }
                        else
                        {
                            SetText(text.Substring(0, point) + (char)key + text.Substring(point));
                        }
                        point++;
                    }
                    else
                    {
                        SetText("" + (char)key);
                        first = 0;
                        point = 1;
                    }
                    used = true;
                    Adjust();
                    return true;
            }
            used = true;
            return true;
        }

        int WordForward(int p)
        {
            if (p >= text.Length)
                return -1;

            int i = p;
            if (Char.IsPunctuation(text [p]) || Char.IsWhiteSpace(text [p]))
            {
                for (; i < text.Length; i++)
                {
                    if (Char.IsLetterOrDigit(text [i]))
                        break;
                }
                for (; i < text.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(text [i]))
                        break;
                }
            }
            else
            {
                for (; i < text.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(text [i]))
                        break;
                }
            }
            if (i != p)
                return i;
            return -1;
        }

        int WordBackward(int p)
        {
            if (p == 0)
                return -1;

            int i = p - 1;
            if (i == 0)
                return 0;
            
            if (Char.IsPunctuation(text [i]) || Char.IsSymbol(text [i]) || Char.IsWhiteSpace(text [i]))
            {
                for (; i >= 0; i--)
                {
                    if (Char.IsLetterOrDigit(text [i]))
                        break;
                }
                for (; i >= 0; i--)
                {
                    if (!Char.IsLetterOrDigit(text [i]))
                        break;
                }
            }
            else
            {
                for (; i >= 0; i--)
                {
                    if (!Char.IsLetterOrDigit(text [i]))
                        break;
                }
            }
            i++;
            
            if (i != p)
                return i;

            return -1;
        }
        
        public override void ProcessMouse(Curses.MouseEvent ev)
        {
            if ((ev.ButtonState & Curses.Event.Button1Clicked) == 0)
                return;

            Container.SetFocus(this);

            // We could also set the cursor position.
            point = first + (ev.X - x);
            if (point > text.Length)
                point = text.Length;
            if (point < first)
                point = 0;
            
            Container.Redraw();
            Container.PositionCursor();
        }
    }
}
