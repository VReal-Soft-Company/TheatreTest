using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTheare.Shared.Data.Exceptions
{
    public class AppException : Exception
    {
        public int StatusCode = 400;
        public AppException() : base() { }

        public AppException(string message) : base(message) { }
        public AppException(string message, int statusCode)
           : base(message)
        {
            StatusCode = statusCode;
        }
        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
