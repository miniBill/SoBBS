using System;

namespace Sobbs.Config.Sizes
{
    [Serializable]
    class SizeParseException : Exception
    {
        public SizeParseException(string message)
            : base(message)
        {
        }
    }
}
