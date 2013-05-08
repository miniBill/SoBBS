namespace Sobbs.Config.Sizes
{
    public struct Percent
    {
        private Percent(double value)
            : this()
        {
            Value = value;
        }

        public double Value { get; private set; }

        public static Percent Parse(string input)
        {
            return new Percent(double.Parse(input.Substring(0, input.Length - 1)));
        }
    }
}
