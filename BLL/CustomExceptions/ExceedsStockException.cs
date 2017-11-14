
using System;

namespace BLL.CustomExceptions
{
    public class ExceedsStockException : Exception
    {
        public ExceedsStockException()
            : base("The quantity you requested exceeds current stock!")
        {

        }

        public ExceedsStockException(string message)
            : base(message)
        {

        }

        public ExceedsStockException(string message, Exception innException)
            : base(message, innException)
        {

        }
    }
}
