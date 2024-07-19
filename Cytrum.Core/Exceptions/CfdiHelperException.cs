using System;

namespace Cytrum.Core.Exceptions
{
    public class CfdiHelperException : Exception
    {
        public CfdiHelperException() : base()
        {
        }

        public CfdiHelperException(string message)
            : base(message)
        {
        }
    }
}