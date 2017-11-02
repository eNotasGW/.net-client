using eNotasGW.Client.Lib.Exceptions.Base;
using eNotasGW.Client.Lib.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Exceptions
{
    public class GWLibAuthorizationException : GWLibException
    {
        public GWLibAuthorizationException(GWLibErro[] errors)
            : base(errors)
        {
            this.Summary = GWLibMessages.AuthorizationError;
        }

        public GWLibAuthorizationException(string message, GWLibErro[] errors)
            : base(errors)
        {
            this.Summary = message;
        }
    }
}
