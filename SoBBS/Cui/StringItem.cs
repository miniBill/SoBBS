using Sobbs.Cui.Curses;

namespace Sobbs.Cui
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
#if __MONO_CS__
            Mono.Terminal.Curses.move(line, col);
            Mono.Terminal.Curses.addstr(width < Value.Length ? Value.Substring(0, width) : Value);
#else
            throw new NotImplementedException();
#endif
        }

        public override string ToString()
        {
            return Value;
        }
    }
}

