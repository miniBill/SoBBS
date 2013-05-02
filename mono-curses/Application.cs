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
	///   gui.cs Application driver.
	/// </summary>
	/// <remarks>
	///   Before using gui.cs, you must call Application.Init, then
	///   you would create your toplevel container (typically by
	///   calling:  new Container (0, 0, Application.Cols,
	///   Application.Lines), adding widgets to it and finally
	///   calling Application.Run on the toplevel container. 
	/// </remarks>
	public class Application {
		/// <summary>
		///   Color used for unfocused widgets.
		/// </summary>
		public static int ColorNormal;
		/// <summary>
		///   Color used for focused widgets.
		/// </summary>
		public static int ColorFocus;
		/// <summary>
		///   Color used for hotkeys in unfocused widgets.
		/// </summary>
		public static int ColorHotNormal;
		/// <summary>
		///   Color used for hotkeys in focused widgets.
		/// </summary>
		public static int ColorHotFocus;
		
		/// <summary>
		///   Color used for marked entries.
		/// </summary>
		public static int ColorMarked;
		/// <summary>
		///   Color used for marked entries that are currently
		///   selected with the cursor.
		/// </summary>
		public static int ColorMarkedSelected;
		
		/// <summary>
		///   Color for unfocused widgets on a dialog.
		/// </summary>
		public static int ColorDialogNormal;
		/// <summary>
		///   Color for focused widgets on a dialog.
		/// </summary>
		public static int ColorDialogFocus;
		/// <summary>
		///   Color for hotkeys in an unfocused widget on a dialog.
		/// </summary>
		public static int ColorDialogHotNormal;
		/// <summary>
		///   Color for a hotkey in a focused widget on a dialog.
		/// </summary>
		public static int ColorDialogHotFocus;

		/// <summary>
		///   Color used for error text.
		/// </summary>
		public static int ColorError;

		/// <summary>
		///   Color used for focused widgets on an error dialog.
		/// </summary>
		public static int ColorErrorFocus;
		
		/// <summary>
		///   Color used for hotkeys in error dialogs
		/// </summary>
		public static int ColorErrorHot;
		
		/// <summary>
		///   Color used for hotkeys in a focused widget in an error dialog
		/// </summary>
		public static int ColorErrorHotFocus;
		
		/// <summary>
		///   The basic color of the terminal.
		/// </summary>
		public static int ColorBasic;

		/// <summary>
		///   The regular color for a selected item on a menu
		/// </summary>
		public static int ColorMenuSelected;

		/// <summary>
		///   The hot color for a selected item on a menu
		/// </summary>
		public static int ColorMenuHotSelected;

		/// <summary>
		///   The regular color for a menu entry
		/// </summary>
		public static int ColorMenu;
		
		/// <summary>
		///   The hot color for a menu entry
		/// </summary>
		public static int ColorMenuHot;
		
		/// <summary>
		///   This event is raised on each iteration of the
		///   main loop. 
		/// </summary>
		/// <remarks>
		///   See also <see cref="Timeout"/>
		/// </remarks>
		static public event EventHandler Iteration;

		// Private variables
		static List<Container> toplevels = new List<Container> ();
		static short last_color_pair;
		static bool inited;
		static Container empty_container;
		
		/// <summary>
		///    A flag indicating which mouse events are available
		/// </summary>
		public static Curses.Event MouseEventsAvailable;
		
		/// <summary>
		///    Creates a new Curses color to be used by Gui.cs apps
		/// </summary>
		public static int MakeColor (short f, short b)
		{
			Curses.init_pair (++last_color_pair, f, b);
			return Curses.ColorPair (last_color_pair);
		}

		/// <summary>
		///    The singleton EmptyContainer that covers the entire screen.
		/// </summary>
		static public Container EmptyContainer {
			get {
				return empty_container;
			}
		}

		static Window main_window;
		static MainLoop mainloop;
		public static MainLoop MainLoop {
			get {
				return mainloop;
			}
		}
		
		public static bool UsingColor { get; private set; }
		
		/// <summary>
		///    Initializes the runtime.   The boolean flag
		///   indicates whether we are forcing color to be off.
		/// </summary>
		public static void Init (bool disable_color)
		{
			if (inited)
				return;
			inited = true;

			empty_container = new Container (0, 0, Application.Cols, Application.Lines);

			try {
				main_window = Curses.initscr ();
			} catch (Exception e){
				Console.WriteLine ("Curses failed to initialize, the exception is: " + e);
				throw new Exception ("Application.Init failed");
			}
			Curses.raw ();
			Curses.noecho ();
			//Curses.nonl ();
			Window.Standard.keypad (true);

#if BREAK_UTF8_RENDERING
			Curses.Event old = 0;
			MouseEventsAvailable = Curses.console_sharp_mouse_mask (
				Curses.Event.Button1Clicked | Curses.Event.Button1DoubleClicked, out old);
#endif
	
			UsingColor = false;
			if (!disable_color)
				UsingColor = Curses.has_colors ();
			
			Curses.start_color ();
			Curses.use_default_colors ();
			if (UsingColor){
				ColorNormal = MakeColor (Curses.COLOR_WHITE, Curses.COLOR_BLUE);
				ColorFocus = MakeColor (Curses.COLOR_BLACK, Curses.COLOR_CYAN);
				ColorHotNormal = Curses.A_BOLD | MakeColor (Curses.COLOR_YELLOW, Curses.COLOR_BLUE);
				ColorHotFocus = Curses.A_BOLD | MakeColor (Curses.COLOR_YELLOW, Curses.COLOR_CYAN);

				ColorMenu = Curses.A_BOLD | MakeColor (Curses.COLOR_WHITE, Curses.COLOR_CYAN);
				ColorMenuHot = Curses.A_BOLD | MakeColor (Curses.COLOR_YELLOW, Curses.COLOR_CYAN);
				ColorMenuSelected = Curses.A_BOLD | MakeColor (Curses.COLOR_WHITE, Curses.COLOR_BLACK);
				ColorMenuHotSelected = Curses.A_BOLD | MakeColor (Curses.COLOR_YELLOW, Curses.COLOR_BLACK);
				
				ColorMarked = ColorHotNormal;
				ColorMarkedSelected = ColorHotFocus;

				ColorDialogNormal    = MakeColor (Curses.COLOR_BLACK, Curses.COLOR_WHITE);
				ColorDialogFocus     = MakeColor (Curses.COLOR_BLACK, Curses.COLOR_CYAN);
				ColorDialogHotNormal = MakeColor (Curses.COLOR_BLUE,  Curses.COLOR_WHITE);
				ColorDialogHotFocus  = MakeColor (Curses.COLOR_BLUE,  Curses.COLOR_CYAN);

				ColorError = Curses.A_BOLD | MakeColor (Curses.COLOR_WHITE, Curses.COLOR_RED);
				ColorErrorFocus = MakeColor (Curses.COLOR_BLACK, Curses.COLOR_WHITE);
				ColorErrorHot = Curses.A_BOLD | MakeColor (Curses.COLOR_YELLOW, Curses.COLOR_RED);
				ColorErrorHotFocus = ColorErrorHot;
			} else {
				ColorNormal = Curses.A_NORMAL;
				ColorFocus = Curses.A_REVERSE;
				ColorHotNormal = Curses.A_BOLD;
				ColorHotFocus = Curses.A_REVERSE | Curses.A_BOLD;

				ColorMenu = Curses.A_REVERSE;
				ColorMenuHot = Curses.A_NORMAL;
				ColorMenuSelected = Curses.A_BOLD;
				ColorMenuHotSelected = Curses.A_NORMAL;
				
				ColorMarked = Curses.A_BOLD;
				ColorMarkedSelected = Curses.A_REVERSE | Curses.A_BOLD;

				ColorDialogNormal = Curses.A_REVERSE;
				ColorDialogFocus = Curses.A_NORMAL;
				ColorDialogHotNormal = Curses.A_BOLD;
				ColorDialogHotFocus = Curses.A_NORMAL;

				ColorError = Curses.A_BOLD;
			}
			ColorBasic = MakeColor (-1, -1);
			mainloop = new MainLoop ();
			mainloop.AddWatch (0, MainLoop.Condition.PollIn, x => {
				Container top = toplevels.Count > 0 ? toplevels [toplevels.Count-1] : null;
				if (top != null)
					ProcessChar (top);

				return true;
			});
		}

		/// <summary>
		///   The number of lines on the screen
		/// </summary>
		static public int Lines {	
			get {
				return Curses.Lines;
			}
		}

		/// <summary>
		///   The number of columns on the screen
		/// </summary>
		static public int Cols {
			get {
				return Curses.Cols;
			}
		}

		/// <summary>
		///   Displays a message on a modal dialog box.
		/// </summary>
		/// <remarks>
		///   The error boolean indicates whether this is an
		///   error message box or not.   
		/// </remarks>
		static public void Msg (bool error, string caption, string t)
		{
			var lines = new List<string> ();
			int last = 0;
			int max_w = 0;
			string x;
			for (int i = 0; i < t.Length; i++){
				if (t [i] == '\n'){
					x = t.Substring (last, i-last);
					lines.Add (x);
					last = i + 1;
					if (x.Length > max_w)
						max_w = x.Length;
				}
			}
			x = t.Substring (last);
			if (x.Length > max_w)
				max_w = x.Length;
			lines.Add (x);

			Dialog d = new Dialog (System.Math.Max (caption.Length + 8, max_w + 8), lines.Count + 7, caption);
			if (error)
				d.ErrorColors ();
			
			for (int i = 0; i < lines.Count; i++)
				d.Add (new Label (1, i + 1, (string) lines [i]));

			Button b = new Button (0, 0, "Ok", true);
			d.AddButton (b);
			b.Clicked += delegate { b.Container.Running = false; };

			Application.Run (d);
		}
		
		/// <summary>
		///   Displays an error message.
		/// </summary>
		/// <remarks>
		/// </remarks>
		static public void Error (string caption, string text)
		{
			Msg (true, caption, text);
		}
		
		/// <summary>
		///   Displays an error message.
		/// </summary>
		/// <remarks>
		///   Overload that allows for String.Format parameters.
		/// </remarks>
		static public void Error (string caption, string format, params object [] pars)
		{
			string t = String.Format (format, pars);

			Msg (true, caption, t);
		}

		/// <summary>
		///   Displays an informational message.
		/// </summary>
		/// <remarks>
		/// </remarks>
		static public void Info (string caption, string text)
		{
			Msg (false, caption, text);
		}
		
		/// <summary>
		///   Displays an informational message.
		/// </summary>
		/// <remarks>
		///   Overload that allows for String.Format parameters.
		/// </remarks>
		static public void Info (string caption, string format, params object [] pars)
		{
			string t = String.Format (format, pars);

			Msg (false, caption, t);
		}

		static void Shutdown ()
		{
			Curses.endwin ();
		}

		static void Redraw (Container container)
		{
			container.Redraw ();
			Curses.refresh ();
		}

		/// <summary>
		///   Forces a repaint of the screen.
		/// </summary>
		/// <remarks>
		/// </remarks>
		public static void Refresh ()
		{
			Container last = null;

			Curses.redrawwin (main_window.Handle);
			foreach (Container c in toplevels){
				c.Redraw ();
				last = c;
			}
			Curses.refresh ();
			if (last != null)
				last.PositionCursor ();
		}

		/// <summary>
		///   Starts running a new container or dialog box.
		/// </summary>
		/// <remarks>
		///   Use this method if you want to start the dialog, but
		///   you want to control the main loop execution manually
		///   by calling the RunLoop method (for example, to start
		///   the dialog, but continuing to process events).
		///
		///    Use the returned value as the argument to RunLoop
		///    and later to the End method to remove the container
		///    from the screen.
		/// </remarks>
		static public RunState Begin (Container container)
		{
			if (container == null)
				throw new ArgumentNullException ("container");
			var rs = new RunState (container);
			
			Init (false);
			
			Curses.timeout (-1);

			toplevels.Add (container);

			container.Prepare ();
			container.SizeChanged ();			
			container.FocusFirst ();
			Redraw (container);
			container.PositionCursor ();
			Curses.refresh ();
			
			return rs;
		}

		/// <summary>
		///   Runs the main loop for the created dialog
		/// </summary>
		/// <remarks>
		///   Calling this method will block until the
		///   dialog has completed execution.
		/// </remarks>
		public static void RunLoop (RunState state)
		{
			RunLoop (state, true);
		}
		
		/// <summary>
		///   Runs the main loop for the created dialog
		/// </summary>
		/// <remarks>
		///   Use the wait parameter to control whether this is a
		///   blocking or non-blocking call.
		/// </remarks>
		public static void RunLoop (RunState state, bool wait)
		{
			if (state == null)
				throw new ArgumentNullException ("state");
			if (state.Container == null)
				throw new ObjectDisposedException ("state");
			
			for (state.Container.Running = true; state.Container.Running; ){
				if (mainloop.EventsPending (wait)){
					mainloop.MainIteration ();
					if (Iteration != null)
						Iteration (null, EventArgs.Empty);
				} else if (wait == false)
					return;
			}
		}

		public static void Stop ()
		{
			if (toplevels.Count == 0)
				return;
			toplevels [toplevels.Count-1].Running = false;
			MainLoop.Stop ();
		}
		
		/// <summary>
		///   Runs the main loop on the given container.
		/// </summary>
		/// <remarks>
		///   This method is used to start processing events
		///   for the main application, but it is also used to
		///   run modal dialog boxes.
		/// </remarks>
		static public void Run (Container container)
		{
			var runToken = Begin (container);
			RunLoop (runToken);
			End (runToken);
		}

		/// <summary>
		///   Use this method to complete an execution started with Begin
		/// </summary>
		static public void End (RunState state)
		{
			if (state == null)
				throw new ArgumentNullException ("state");
			state.Dispose ();
		}

		// Called by the Dispose handler.
		internal static void End (Container container)
		{
			toplevels.Remove (container);
			if (toplevels.Count == 0)
				Shutdown ();
			else
				Refresh ();
		}
				
		static void ProcessChar (Container container)
		{
			int ch = Curses.getch ();

			if ((ch == -1) || (ch == Curses.KeyResize)){
				if (Curses.CheckWinChange ()){
					EmptyContainer.Clear ();
					foreach (Container c in toplevels)
						c.SizeChanged ();
					Refresh ();
				}
				return;
			}
				
			if (ch == Curses.KeyMouse){
				Curses.MouseEvent ev;
				
				Curses.console_sharp_getmouse (out ev);
				container.ProcessMouse (ev);
				return;
			}
				
			if (ch == 27){
				Curses.timeout (100);
				int k = Curses.getch ();
				if (k != Curses.ERR && k != 27)
					ch = Curses.KeyAlt | k;
				Curses.timeout (-1);
			}
			
			if (container.ProcessHotKey (ch))
				return;
			
			if (container.ProcessKey (ch))
				return;

			if (container.ProcessColdKey (ch))
				return;
			
			// Control-c, quit the current operation.
			if (ch == 3){
				container.Running = false;
				return;
			}
			
			// Control-z, suspend execution, then repaint.
			if (ch == 26){
				Curses.console_sharp_sendsigtstp ();
				Window.Standard.redrawwin ();
				Curses.refresh ();
			}
			
			//
			// Focus handling
			//
			if (ch == 9 || ch == Curses.KeyDown || ch == Curses.KeyRight){
				if (!container.FocusNext ())
					container.FocusNext ();
				Curses.refresh ();
			} else if (ch == Curses.KeyUp || ch == Curses.KeyLeft){
				if (!container.FocusPrev ())
					container.FocusPrev ();
				Curses.refresh ();
			}
		}
	}
}
