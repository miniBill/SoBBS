namespace Sobbs.Data.List
{
    public interface IImmutableList<T>
    {
        bool IsEmpty
        {
            get;
        }

        T Value
        {
            get;
        }

        IImmutableList<T> Tail
        {
            get;
        }

        IImmutableList<T> Add(T value);
    }
}