namespace Sobbs.Cui.Curses
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
            Mono.Terminal.Curses.move(line, col);
            Mono.Terminal.Curses.addstr(width < Value.Length ? Value.Substring(0, width) : Value);
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

