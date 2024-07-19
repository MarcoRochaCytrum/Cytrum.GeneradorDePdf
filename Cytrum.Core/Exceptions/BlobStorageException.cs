using System;

namespace Cytrum.Core.Exceptions
{
    public class BlobStorageException : Exception
    {
        public BlobStorageException() : base()
        {
        }

        public BlobStorageException(string message)
            : base(message)
        {
        }
    }
}
