namespace Sobbs.Cui
{
    public interface IWidget
    {
        event KeyPressedEventHandler<IFrame> OnProcessHotKey;
        int w { get; }
        int h { get; }
    }
}