namespace Sobbs.Cui
{
    public interface IWidget
    {
        event KeyPressedEventHandler OnProcessHotKey;
        int w { get; }
        int h { get; }
    }
}