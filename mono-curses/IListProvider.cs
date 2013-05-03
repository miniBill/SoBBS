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
	///   Model for the <see cref="ListView"/> widget.
	/// </summary>
	/// <remarks>
	///   Consumers of the <see cref="ListView"/> widget should
	///   implement this interface
	/// </remarks>
	public interface IListProvider {
		/// <summary>
		///   Number of items in the model.
		/// </summary>
		/// <remarks>
		///   This should return the number of items in the
		///   model. 
		/// </remarks>
		int Items { get; }

		/// <summary>
		///   Whether the ListView should allow items to be
		///   marked. 
		/// </summary>
		bool AllowMark { get; }

		/// <summary>
		///   Whether the given item is marked.
		/// </summary>
		bool IsMarked (int item);

		/// <summary>
		///   This should render the item at the given line,
		///   col with the specified width.
		/// </summary>
		void Render (int line, int col, int width, int item);

		/// <summary>
		///   Callback: this is the way that the model is
		///   hooked up to its actual view. 
		/// </summary>
		void SetListView (ListView target);

		/// <summary>
		///   Allows the model to process the given keystroke.
		/// </summary>
		/// <remarks>
		///   The model should return true if the key was
		///   processed, false otherwise.
		/// </remarks>
		bool ProcessKey (int ch);

		/// <summary>
		///   Callback: invoked when the selected item has changed.
		/// </summary>
		void SelectedChanged ();
	}
}