using System;
using Sobbs.Functional.Data.ThreeEither;

namespace Sobbs.Config.Sizes
{
    public class Size : IThreeEither<int, Percent, Star>
    {
        public static Size Parse(string input)
        {
            if (input == "*")
                return Star;
            if (input.Length == 0)
                throw new SizeParseException("Empty size");
            if (input[input.Length - 1] == '%')
            {
                return new Size(Percent.Parse(input));
            }
            return new Size(Int32.Parse(input));
        }

        private IThreeEither<int, Percent, Star> Value
        {
            get;
            set;
        }

        private static readonly Lazy<Size> ZeroLazy = new Lazy<Size>(() => new Size(0));

        public static Size Zero
        {
            get { return ZeroLazy.Value; }
        }

        private static readonly Lazy<Size> StarLazy = new Lazy<Size>(() => new Size(Sizes.Star.Instance));

        public static Size Star
        {
            get { return StarLazy.Value; }
        }

        private Size(int value)
        {
            Value = new ThreeLeft<int, Percent, Star>(value);
        }

        private Size(Percent value)
        {
            Value = new ThreeMid<int, Percent, Star>(value);
        }

        private Size(Star instance)
        {
            Value = new ThreeRight<int, Percent, Star>(instance);
        }

        public T Either<T>(Func<int, T> fLeft, Func<Percent, T> fMid, Func<Star, T> fRight)
        {
            return Value.Either(fLeft, fMid, fRight);
        }

        public override string ToString()
        {
            return Value.Either(i => i.ToString(), percent => percent.ToString(), star => star.ToString());
        }
    }
}
