namespace Sobbs.Config.Sizes
{
    public class Star
    {
        private Star()
        {
        }

        public static readonly Star Instance = new Star();

        public override string ToString()
        {
            return "*";
        }
    }
}
