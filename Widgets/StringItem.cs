using System;
using Mono.Terminal;

namespace Sobbs.Widgets
{
    public class StringItem : IListItem
    {
        public string Value
        {
            get;
            private set;
        }

        public StringItem(string value)
        {
            Value = value;
        }

        public void Render(int line, int col, int width)
        {
            Curses.move(line, col);
            Curses.addstr(width < Value.Length ? Value.Substring(0, width) : Value);
        }
    }
}
