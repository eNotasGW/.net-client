using eNotasGW.Client.Lib.Data;
using eNotasGW.Client.Lib.Exceptions.Base;
using eNotasGW.Client.Lib.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Exceptions
{
    public class GWLibAuthenticationException : GWLibException
    {
        public GWLibAuthenticationException(GWLibErro[] errors)
            : base(errors)
        {
            this.Summary = GWLibMessages.AuthenticationError;
        }

        public GWLibAuthenticationException(string message, GWLibErro[] errors)
            : base(errors)
        {
            this.Summary = message;
        }
    }
}
