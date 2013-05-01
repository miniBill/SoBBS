using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows
{
    public class Size : IEither<int, Percent, Star>
    {
        public static Size Parse(string input)
        {
            if (input == "*")
                return Wrap(Star.Instance);
            if (input.Length == 0)
                throw new SizeParseException("Empty size");
            if (input[input.Length - 1] == '%')
            {
                return Wrap(Percent.Parse(input));
            }
            return Wrap(Int32.Parse(input));
        }

        public IEither<int, Percent, Star> Value
        {
            get;
            private set;
        }

        public Size(int value)
        {
            Value = new Left<int, Percent, Star>(value);
        }
        public Size(Percent value)
        {
            Value = new Mid<int, Percent, Star>(value);
        }


        private Size(Star instance)
        {
            Value = new Right<int, Percent, Star>(instance);
        }


        public static readonly Size StarSize = new Size(Star.Instance);

        private static Size Wrap(Star instance)
        {
            return StarSize;
        }

        private static Size Wrap(Percent value)
        {
            return new Size(value);
        }

        private static Size Wrap(int value)
        {
            return new Size(value);
        }

        public T Either<T>(Func<int, T> fLeft, Func<Percent, T> fMid, Func<Star, T> fRight)
        {
            return Value.Either(fLeft, fMid, fRight);
        }
    }
}
