using System.Collections;
using System.Collections.Generic;

namespace Sobbs.Cui.Forms
{
    public class FormContainer : FormWidget, IContainer
    {
        private readonly List<FormWidget> _children = new List<FormWidget>();

        protected FormContainer(WidgetInfo info)
            : base(info)
        {
        }

        public IListView Add(ListViewInfo info)
        {
            var widget = new FormListView(info);
            Add(widget);
            return widget;
        }

        public IFrame Add(FrameInfo info)
        {
            var widget = new FormFrame(info);
            Add(widget);
            return widget;
        }

        private void Add(FormWidget widget)
        {
            Controls.Add(widget);
            _children.Add(widget);
            Refresh();
        }

        public IEnumerator<IWidget> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}