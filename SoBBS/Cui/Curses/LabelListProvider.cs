namespace Sobbs.Cui.Curses
{
    public class LabelListProvider : AbstractSimpleListProvider<string>
    {
        public override void Render(int line, int col, int width, int item)
        {
            string value = this[item];
            Mono.Terminal.Curses.move(line, col);
            Mono.Terminal.Curses.addstr(value);
        }
    }
}
