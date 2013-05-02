using System;

namespace Sobbs.Widgets
{
    public class ListItemProvider : AbstractSimpleListProvider<IListItem>
    {
        public ListItemProvider()
        {
        }

        public override void Render(int line, int col, int width, int item)
        {
            this[item].Render(line, col, width);
        }
    }
}
