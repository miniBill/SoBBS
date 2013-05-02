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
	///   Base class for creating curses widgets
	/// </summary>
	public abstract class Widget {
		/// <summary>
		///    Points to the container of this widget
		/// </summary>
		public Container Container;
		
		/// <summary>
		///    The x position of this widget
		/// </summary>
		public int x;

		/// <summary>
		///    The y position of this widget
		/// </summary>
		public int y;

		/// <summary>
		///    The width of this widget, it is the area that receives mouse events and that must be repainted.
		/// </summary>
		public int w;

		/// <summary>
		///    The height of this widget, it is the area that receives mouse events and that must be repainted.
		/// </summary>
		public int h;
		
		bool can_focus;
		bool has_focus;
		public Fill Fill;
		
		static StreamWriter l;
		
		/// <summary>
		///   Utility function to log messages
		/// </summary>
		/// <remarks>
		///    <para>This is a utility function that you can use to log messages
		///    that will be stored in a file (as curses has taken over the
		///    screen and you can not really log information there).</para>
		///    <para>
		///    The data is written to the file "log2" for now</para>
		/// </remarks>
		public static void Log (string s)
		{
			if (l == null)
				l = new StreamWriter (File.Create ("log2"));
			
			l.WriteLine (s);
			l.Flush ();
		}

		/// <summary>
		///   Utility function to log messages
		/// </summary>
		/// <remarks>
		///    <para>This is a utility function that you can use to log messages
		///    that will be stored in a file (as curses has taken over the
		///    screen and you can not really log information there). </para>
		///    <para>
		///    The data is written to the file "log2" for now</para>
		/// </remarks>
		public static void Log (string s, params object [] args)
		{
			Log (String.Format (s, args));
		}
		
		/// <summary>
		///   Public constructor for widgets
		/// </summary>
		/// <remarks>
		///   <para>
		///      Constructs a widget that starts at positio (x,y) and has width w and height h.
		///      These parameters are used by the methods <see cref="Clear"/> and <see cref="Redraw"/>
		///   </para>
		/// </remarks>
		public Widget (int x, int y, int w, int h)
		{
			this.x = x;
			this.y = y;
			this.w = w;
			this.h = h;
			Container = Application.EmptyContainer;
		}

		/// <summary>
		///   Focus status of this widget
		/// </summary>
		/// <remarks>
		///   <para>
		///     This is used typically by derived classes to flag whether
		///     this widget can receive focus or not.    Focus is activated
		///     by either clicking with the mouse on that widget or by using
		////    the tab key.
		///   </para>
		/// </remarks>
		public bool CanFocus {
			get {
				return can_focus;
			}

			set {
				can_focus = value;
			}
		}

		/// <summary>
		///   Gets or sets the current focus status.
		/// </summary>
		/// <remarks>
		///   <para>
		///     A widget can grab the focus by setting this value to true and
		///     the current focus status can be inquired by using this property.
		///   </para>
		/// </remarks>
		public virtual bool HasFocus {
			get {
				return has_focus;
			}

			set {
				has_focus = value;
				Redraw ();
			}
		}

		/// <summary>
		///   Moves inside the first location inside the container
		/// </summary>
		/// <remarks>
		///     <para>This moves the current cursor position to the specified
		///     line and column relative to the container
		///     client area where this widget is located.</para>
		///   <para>The difference between this
		///     method and <see cref="BaseMove"/> is that this
		///     method goes to the beginning of the client area
		///     inside the container while <see cref="BaseMove"/> goes to the first
		///     position that container uses.</para>
		///   <para>
		///     For example, a Frame usually takes up a couple
		///     of characters for padding.   This method would
		///     position the cursor inside the client area,
		///     while <see cref="BaseMove"/> would position
		///     the cursor at the top of the frame.
		///   </para>
		/// </remarks>
		public void Move (int line, int col)
		{
			Container.ContainerMove (line, col);
		}

		/// <summary>
		///   Move relative to the top of the container
		/// </summary>
		/// <remarks>
		///     <para>This moves the current cursor position to the specified
		///     line and column relative to the start of the container
		///     where this widget is located.</para>
		///   <para>The difference between this
		///     method and <see cref="Move"/> is that this
		///     method goes to the beginning of the container,
		///     while <see cref="Move"/> goes to the first
		///     position that widgets should use.</para>
		///   <para>
		///     For example, a Frame usually takes up a couple
		///     of characters for padding.   This method would
		///     position the cursor at the beginning of the
		///     frame, while <see cref="Move"/> would position
		///     the cursor within the frame.
		///   </para>
		/// </remarks>
		public void BaseMove (int line, int col)
		{
			Container.ContainerBaseMove (line, col);
		}
		
		/// <summary>
		///   Clears the widget region withthe current color.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This clears the entire region used by this widget.
		///   </para>
		/// </remarks>
		public void Clear ()
		{
			for (int line = 0; line < h; line++){
				BaseMove (y + line, x);
				for (int col = 0; col < w; col++){
					Curses.addch (' ');
				}
			}
		}
		
		/// <summary>
		///   Redraws the current widget, must be overwritten.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method should be overwritten by classes
		///     that derive from Widget.   The default
		///     implementation of this method just fills out
		///     the region with the character 'x'. 
		///   </para>
		///   <para>
		///     Widgets are responsible for painting the
		///     entire region that they have been allocated.
		///   </para>
		/// </remarks>
		public virtual void Redraw ()
		{
			for (int line = 0; line < h; line++){
				Move (y + line, x);
				for (int col = 0; col < w; col++){
					Curses.addch ('x');
				}
			}
		}

		/// <summary>
		///   If the widget is focused, gives the widget a
		///     chance to process the keystroke. 
		/// </summary>
		/// <remarks>
		///   <para>
		///     Widgets can override this method if they are
		///     interested in processing the given keystroke.
		///     If they consume the keystroke, they must
		///     return true to stop the keystroke from being
		///     processed by other widgets or consumed by the
		///     widget engine.    If they return false, the
		///     keystroke will be passed out to other widgets
		///     for processing. 
		///   </para>
		/// </remarks>
		public virtual bool ProcessKey (int key)
		{
			return false;
		}

		/// <summary>
		///   Gives widgets a chance to process the given
		///     mouse event. 
		/// </summary>
		/// <remarks>
		///     Widgets can inspect the value of
		///     ev.ButtonState to determine if this is a
		///     message they are interested in (typically
		///     ev.ButtonState &amp; Curses.Event.Button1Clicked).
		/// </remarks>
		public virtual void ProcessMouse (Curses.MouseEvent ev)
		{
		}

		/// <summary>
		///   This method can be overwritten by widgets that
		///     want to provide accelerator functionality
		///     (Alt-key for example).
		/// </summary>
		/// <remarks>
		///   <para>
		///     Before keys are sent to the widgets on the
		///     current Container, all the widgets are
		///     processed and the key is passed to the widgets
		///     to allow some of them to process the keystroke
		///     as a hot-key. </para>
		///  <para>
		///     For example, if you implement a button that
		///     has a hotkey ok "o", you would catch the
		///     combination Alt-o here.  If the event is
		///     caught, you must return true to stop the
		///     keystroke from being dispatched to other
		///     widgets.
		///  </para>
		///  <para>
		///    Typically to check if the keystroke is an
		///     Alt-key combination, you would use
		///     Curses.IsAlt(key) and then Char.ToUpper(key)
		///     to compare with your hotkey.
		///  </para>
		/// </remarks>
		public virtual bool ProcessHotKey (int key)
		{
			return false;
		}

		/// <summary>
		///   This method can be overwritten by widgets that
		///     want to provide accelerator functionality
		///     (Alt-key for example), but without
		///     interefering with normal ProcessKey behavior.
		/// </summary>
		/// <remarks>
		///   <para>
		///     After keys are sent to the widgets on the
		///     current Container, all the widgets are
		///     processed and the key is passed to the widgets
		///     to allow some of them to process the keystroke
		///     as a cold-key. </para>
		///  <para>
		///    This functionality is used, for example, by
		///    default buttons to act on the enter key.
		///    Processing this as a hot-key would prevent
		///    non-default buttons from consuming the enter
		///    keypress when they have the focus.
		///  </para>
		/// </remarks>
		public virtual bool ProcessColdKey (int key)
		{
			return false;
		}
		
		/// <summary>
		///   Moves inside the first location inside the container
		/// </summary>
		/// <remarks>
		///   <para>
		///     A helper routine that positions the cursor at
		///     the logical beginning of the widget.   The
		///     default implementation merely puts the cursor at
		///     the beginning, but derived classes should find a
		///     suitable spot for the cursor to be shown.
		///   </para>
		///   <para>
		///     This method must be overwritten by most
		///     widgets since screen repaints can happen at
		///     any point and it is important to leave the
		///     cursor in a position that would make sense for
		///     the user (as not all terminals support hiding
		///     the cursor), and give the user an impression of
		///     where the cursor is.   For a button, that
		///     would be the position where the hotkey is, for
		///     an entry the location of the editing cursor
		///     and so on.
		///   </para>
		/// </remarks>
		public virtual void PositionCursor ()
		{
			Move (y, x);
		}

		/// <summary>
		///   Method to relayout on size changes.
		/// </summary>
		/// <remarks>
		///   <para>
		///     This method can be overwritten by widgets that
		///     might be interested in adjusting their
		///     contents or children (if they are
		///     containers). 
		///   </para>
		/// </remarks>
		public virtual void DoSizeChanged ()
		{
		}
		
		/// <summary>
		///   Utility function to draw frames
		/// </summary>
		/// <remarks>
		///    Draws a frame with the current color in the
		///    specified coordinates.
		/// </remarks>
		static public void DrawFrame (int col, int line, int width, int height)
		{
			DrawFrame (col, line, width, height, false);
		}

		/// <summary>
		///   Utility function to draw strings that contain a hotkey
		/// </summary>
		/// <remarks>
		///    Draws a string with the given color.   If a character "_" is
		///    found, then the next character is drawn using the hotcolor.
		/// </remarks>
		static public void DrawHotString (string s, int hotcolor, int color)
		{
			Curses.attrset (color);
			foreach (char c in s){
				if (c == '_'){
					Curses.attrset (hotcolor);
					continue;
				}
				Curses.addch (c);
				Curses.attrset (color);
			}
		}

		/// <summary>
		///   Utility function to draw frames
		/// </summary>
		/// <remarks>
		///    Draws a frame with the current color in the
		///    specified coordinates.
		/// </remarks>
		static public void DrawFrame (int col, int line, int width, int height, bool fill)
		{
			int b;
			Curses.move (line, col);
			Curses.addch (Curses.ACS_ULCORNER);
			for (b = 0; b < width-2; b++)
				Curses.addch (Curses.ACS_HLINE);
			Curses.addch (Curses.ACS_URCORNER);
			
			for (b = 1; b < height-1; b++){
				Curses.move (line+b, col);
				Curses.addch (Curses.ACS_VLINE);
				if (fill){
					for (int x = 1; x < width-1; x++)
						Curses.addch (' ');
				} else
					Curses.move (line+b, col+width-1);
				Curses.addch (Curses.ACS_VLINE);
			}
			Curses.move (line + height-1, col);
			Curses.addch (Curses.ACS_LLCORNER);
			for (b = 0; b < width-2; b++)
				Curses.addch (Curses.ACS_HLINE);
			Curses.addch (Curses.ACS_LRCORNER);
		}

		/// <summary>
		///   The color used for rendering an unfocused widget.
		/// </summary>
		public int ColorNormal {
			get {
				return Container.ContainerColorNormal;
			}
		}

		/// <summary>
		///   The color used for rendering a focused widget.
		/// </summary>
		public int ColorFocus {
			get {
				return Container.ContainerColorFocus;
			}
		}

		/// <summary>
		///   The color used for rendering the hotkey on an
		///     unfocused widget. 
		/// </summary>
		public int ColorHotNormal {
			get {
				return Container.ContainerColorHotNormal;
			}
		}

		/// <summary>
		///   The color used to render a hotkey in a focused widget.
		/// </summary>
		public int ColorHotFocus {
			get {
				return Container.ContainerColorHotFocus;
			}
		}
	}
}
