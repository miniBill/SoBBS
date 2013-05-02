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
    ///   Container widget, can host other widgets.
    /// </summary>
    /// <remarks>
    ///   This implements the foundation for other containers
    ///   (like Dialogs and Frames) that can host other widgets
    ///   inside their boundaries.   It provides focus handling
    ///   and event routing.
    /// </remarks>
    public class Container : Widget, IEnumerable<Widget>
    {
        List<Widget> widgets = new List<Widget>();
        Widget focused = null;
        public bool Running;
        public int ContainerColorNormal;
        public int ContainerColorFocus;
        public int ContainerColorHotNormal;
        public int ContainerColorHotFocus;
        public int Border;
        
        static Container()
        {
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Widget> GetEnumerator()
        {
            return widgets.GetEnumerator();
        }

        /// <summary>
        ///   Public constructor.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Container(int x, int y, int w, int h) : base (x, y, w, h)
        {
            ContainerColorNormal = Application.ColorNormal;
            ContainerColorFocus = Application.ColorFocus;
            ContainerColorHotNormal = Application.ColorHotNormal;
            ContainerColorHotFocus = Application.ColorHotFocus;
        }

        /// <summary>
        ///   Called on top-level container before starting up.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void Prepare()
        {
        }

        /// <summary>
        ///   Used to redraw all the children in this container.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void RedrawChildren()
        {
            foreach (Widget w in widgets)
            {
                // Poor man's clipping.
                if (w.x >= this.w - Border * 2)
                    continue;
                if (w.y >= this.h - Border * 2)
                    continue;
                
                w.Redraw();
            }
        }
        
        public override void Redraw()
        {
            RedrawChildren();
        }
        
        public override void PositionCursor()
        {
            if (focused != null)
                focused.PositionCursor();
        }

        /// <summary>
        ///   Focuses the specified widget in this container.
        /// </summary>
        /// <remarks>
        ///   Focuses the specified widge, taking the focus
        ///   away from any previously focused widgets.   This
        ///   method only works if the widget specified
        ///   supports being focused.
        /// </remarks>
        public void SetFocus(Widget w)
        {
            if (!w.CanFocus)
                return;
            if (focused == w)
                return;
            if (focused != null)
                focused.HasFocus = false;
            focused = w;
            focused.HasFocus = true;
            Container wc = w as Container;
            if (wc != null)
                wc.EnsureFocus();
            focused.PositionCursor();
        }

        /// <summary>
        ///   Focuses the first possible focusable widget in
        ///   the contained widgets.
        /// </summary>
        public void EnsureFocus()
        {
            if (focused == null)
                FocusFirst();
        }
        
        /// <summary>
        ///   Focuses the first widget in the contained widgets.
        /// </summary>
        public void FocusFirst()
        {
            foreach (Widget w in widgets)
            {
                if (w.CanFocus)
                {
                    SetFocus(w);
                    return;
                }
            }
        }

        /// <summary>
        ///   Focuses the last widget in the contained widgets.
        /// </summary>
        public void FocusLast()
        {
            for (int i = widgets.Count; i > 0;)
            {
                i--;

                Widget w = (Widget)widgets [i];
                if (w.CanFocus)
                {
                    SetFocus(w);
                    return;
                }
            }
        }

        /// <summary>
        ///   Focuses the previous widget.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public bool FocusPrev()
        {
            if (focused == null)
            {
                FocusLast();
                return true;
            }
            int focused_idx = -1;
            for (int i = widgets.Count; i > 0;)
            {
                i--;
                Widget w = (Widget)widgets [i];

                if (w.HasFocus)
                {
                    Container c = w as Container;
                    if (c != null)
                    {
                        if (c.FocusPrev())
                            return true;
                    }
                    focused_idx = i;
                    continue;
                }
                if (w.CanFocus && focused_idx != -1)
                {
                    focused.HasFocus = false;

                    Container c = w as Container;
                    if (c != null && c.CanFocus)
                    {
                        c.FocusLast();
                    } 
                    SetFocus(w);
                    return true;
                }
            }
            if (focused != null)
            {
                focused.HasFocus = false;
                focused = null;
            }
            return false;
        }

        /// <summary>
        ///   Focuses the next widget.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public bool FocusNext()
        {
            if (focused == null)
            {
                FocusFirst();
                return focused != null;
            }
            int n = widgets.Count;
            int focused_idx = -1;
            for (int i = 0; i < n; i++)
            {
                Widget w = (Widget)widgets [i];

                if (w.HasFocus)
                {
                    Container c = w as Container;
                    if (c != null)
                    {
                        if (c.FocusNext())
                            return true;
                    }
                    focused_idx = i;
                    continue;
                }
                if (w.CanFocus && focused_idx != -1)
                {
                    focused.HasFocus = false;

                    Container c = w as Container;
                    if (c != null && c.CanFocus)
                    {
                        c.FocusFirst();
                    } 
                    SetFocus(w);
                    return true;
                }
            }
            if (focused != null)
            {
                focused.HasFocus = false;
                focused = null;
            }
            return false;
        }

        /// <summary>
        ///   Returns the base position for child widgets to
        ///   paint on.   
        /// </summary>
        /// <remarks>
        ///   This method is typically overwritten by
        ///   containers that want to have some padding (like
        ///   Frames or Dialogs).
        /// </remarks>
        public virtual void GetBase(out int row, out int col)
        {
            row = 0;
            col = 0;
        }
        
        public virtual void ContainerMove(int row, int col)
        {
            if (Container != Application.EmptyContainer && Container != null)
                Container.ContainerMove(row + y, col + x);
            else
                Curses.move(row + y, col + x);
        }
        
        public virtual void ContainerBaseMove(int row, int col)
        {
            if (Container != Application.EmptyContainer && Container != null)
                Container.ContainerBaseMove(row + y, col + x);
            else
                Curses.move(row + y, col + x);
        }
        
        /// <summary>
        ///   Adds a widget to this container.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void Add(Widget w)
        {
            widgets.Add(w);
            w.Container = this;
            if (w.CanFocus)
                this.CanFocus = true;
        }

        /// <summary>
        ///   Removes all the widgets from this container.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void RemoveAll()
        {
            foreach (Widget w in widgets)
                Remove(w);
        }

        /// <summary>
        ///   Removes a widget from this container.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public virtual void Remove(Widget w)
        {
            if (w == null)
                return;
            
            widgets.Remove(w);
            w.Container = null;
            
            if (widgets.Count < 1)
                this.CanFocus = false;
        }

        public override bool ProcessKey(int key)
        {
            if (focused != null)
            {
                if (focused.ProcessKey(key))
                    return true;
            }
            return false;
        }

        public override bool ProcessHotKey(int key)
        {
            if (focused != null)
            if (focused.ProcessHotKey(key))
                return true;
            
            foreach (Widget w in widgets)
            {
                if (w == focused)
                    continue;
                
                if (w.ProcessHotKey(key))
                    return true;
            }
            return false;
        }

        public override bool ProcessColdKey(int key)
        {
            if (focused != null)
            if (focused.ProcessColdKey(key))
                return true;
            
            foreach (Widget w in widgets)
            {
                if (w == focused)
                    continue;
                
                if (w.ProcessColdKey(key))
                    return true;
            }
            return false;
        }

        public override void ProcessMouse(Curses.MouseEvent ev)
        {
            int bx, by;

            GetBase(out bx, out by);
            ev.X -= x;
            ev.Y -= y;
            
            foreach (Widget w in widgets)
            {
                int wx = w.x + bx;
                int wy = w.y + by;

                if ((ev.X < wx) || (ev.X > (wx + w.w)))
                    continue;

                if ((ev.Y < wy) || (ev.Y > (wy + w.h)))
                    continue;
                
                ev.X -= bx;
                ev.Y -= by;

                w.ProcessMouse(ev);
                return;
            }           
        }
        
        public override void DoSizeChanged()
        {
            foreach (Widget widget in widgets)
            {
                widget.DoSizeChanged();

                if ((widget.Fill & Fill.Horizontal) != 0)
                {
                    widget.w = w - (Border * 2) - widget.x;
                }

                if ((widget.Fill & Fill.Vertical) != 0)
                    widget.h = h - (Border * 2) - widget.y;
            }
        }

        /// <summary>
        ///   Raised when the size of this container changes.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public event EventHandler SizeChangedEvent;
        
        /// <summary>
        ///   This method is invoked when the size of this
        ///   container changes. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void SizeChanged()
        {
            if (SizeChangedEvent != null)
                SizeChangedEvent(this, EventArgs.Empty);
            DoSizeChanged();
        }
    }
}
