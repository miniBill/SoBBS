using Mono.Terminal;

namespace Sobbs.Widgets
{
    public class StringItem : IListItem
    {
        private string Value
        {
            get;
            set;
        }

        public StringItem(string value)
        {
            Value = value;
        }

        public void Render(int line, int col, int width)
        {
            Curses.move(line, col);
            Curses.AddStr(width < Value.Length ? Value.Substring(0, width) : Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

