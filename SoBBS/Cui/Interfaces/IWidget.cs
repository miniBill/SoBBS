using Sobbs.Config.Sizes;

namespace Sobbs.Cui.Interfaces
{
    public interface IWidget
    {
        Size Width { get; }
        Size Height { get; }
        void Refresh(int px, int py, int pwidth, int pheight);
    }
}