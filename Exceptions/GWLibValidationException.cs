using eNotasGW.Client.Lib.Exceptions.Base;
using eNotasGW.Client.Lib.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasGW.Client.Lib.Exceptions
{
    class GWLibValidationException : GWLibException
    {
        public GWLibValidationException(GWLibErro[] errors)
            : base(errors)
        {
            this.Summary = GWLibMessages.ValidationError;
        }

        public GWLibValidationException(string message, GWLibErro[] errors)
            : base(errors)
        {
            this.Summary = message;
        }
    }
}
