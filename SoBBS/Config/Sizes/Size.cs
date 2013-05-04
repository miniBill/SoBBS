using System;
using Sobbs.Functional.Data.ThreeEither;

namespace Sobbs.Config.Sizes
{
    public class Size : IThreeEither<int, Percent, Star>
    {
        public static Size Parse(string input)
        {
            if (input == "*")
                return StarSize;
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
            get; set;
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

        private static readonly Size StarSize = new Size(Star.Instance);

        public T Either<T>(Func<int, T> fLeft, Func<Percent, T> fMid, Func<Star, T> fRight)
        {
            return Value.Either(fLeft, fMid, fRight);
        }
    }
}
