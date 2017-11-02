using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace eNotasGW.Client.Lib.Exceptions
{
    public class HttpResponseInternalServerErrorException : Exception
    {
        public HttpResponseInternalServerErrorException(string message)
            : base(message)
        {
        }
    }
}
