using System.Collections.Generic;
using Mono.Terminal;

namespace Sobbs.Cui
{
    public abstract class AbstractSimpleListProvider<T> : IListProvider
    {
        private readonly List<T> _items = new List<T>();

        public bool IsMarked(int item)
        {
            return false;
        }

        public abstract void Render(int line, int col, int width, int item);

        public void SetListView(ListView target)
        {
        }

        public bool ProcessKey(int ch)
        {
            return false;
        }

        public void SelectedChanged()
        {
        }

        protected T this[int index]
        {
            get
            {
                return _items[index];
            }
        }

        public int Items
        {
            get
            {
                return _items.Count;
            }
        }

        public bool AllowMark
        {
            get
            {
                return false;
            }
        }

        public void Add(T item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
