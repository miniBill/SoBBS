namespace Sobbs.Cui
{
    public interface IWidget
    {
        event KeyPressedEventHandler<IFrame> OnProcessHotKey;
        int W { get; }
        int H { get; }
    }
}