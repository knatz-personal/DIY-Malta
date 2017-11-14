using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.CustomExceptions
{
    public class DuplicateDataException : Exception
    {
       public DuplicateDataException()
            : base("Duplicate records detected!")
        {

        }

        public DuplicateDataException(string message)
            : base(message)
        {

        }

        public DuplicateDataException(string message, Exception innException)
            : base(message, innException)
        {

        }
    }
}
