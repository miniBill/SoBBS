using System;

namespace Sobbs.Config.Sizes
{
    class SizeParseException : Exception
    {
        public SizeParseException(string message)
            : base(message)
        {
        }
    }
}
