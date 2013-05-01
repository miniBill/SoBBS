using System;
using Sobbs.Config.Ini;
using Sobbs.Data.List;

namespace Sobbs.Config.Windows
{
    public struct Percent
    {
        public readonly double Value;

        public Percent(double value)
        {
            Value = value;
        }

        public static Percent Parse(string input)
        {
            return new Percent(double.Parse(input.Substring(0, input.Length - 1)));
        }
    }
}
