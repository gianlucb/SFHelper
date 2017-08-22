using System;

namespace SFHelper
{
    public class SFHelperException : Exception
    {
        public SFHelperException() : base() { }
        public SFHelperException(string message) : base(message) { }
        public SFHelperException(string message, Exception inner) : base(message,inner) { }

    }
}
