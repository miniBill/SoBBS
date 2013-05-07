namespace Sobbs.Functional.Data.Maybe
{
    public static class Operations
    {
        private static IMaybe<T> ToMaybe<T>(this T value) where T : class
        {
            if (value == null)
                return Nothing<T>.Instance;
            return new Just<T>(value);
        }

        public static IMaybe<T> MaybeCast<T>(this object value) where T : class
        {
            var cast = value as T;
            return cast.ToMaybe();
        }
    }
}
