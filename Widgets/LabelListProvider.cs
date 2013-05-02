using Mono.Terminal;

namespace Sobbs.Widgets
{
    class LabelListProvider : AbstractSimpleListProvider<string>
    {
        public override void Render(int line, int col, int width, int item)
        {
            string value = this[item];
            Curses.move(line, col);
            Curses.addstr(value);
        }
    }
}
