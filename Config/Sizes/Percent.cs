namespace Sobbs.Config.Sizes
{
    public struct Percent
    {
        public readonly double Value;

        private Percent(double value)
        {
            Value = value;
        }

        public static Percent Parse(string input)
        {
            return new Percent(double.Parse(input.Substring(0, input.Length - 1)));
        }
    }
}
